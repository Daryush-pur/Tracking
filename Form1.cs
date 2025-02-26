using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tracking
{
    public partial class Form1 : Form
    {
        // Constants for station names
        private const string WarehouseStation = "انبار";
        private const string QCWeldingStation = "تایید کنترل کیفیت (جوشکاری)";
        private const string QCAssemblyStation = "تایید کنترل کیفیت (مونتاژ)";

        // Application variables
        private string station = "";
        private string mainStation = "";
        private string PreviousStation = "";
        private string QC = "";
        private string connectionString;

        // Timer for clearing error messages
        private System.Windows.Forms.Timer timerClearErrorLabel;

        public Form1()
        {
            InitializeComponent();
            connectionString = LoadConnectionSettings(); // Load connection string from settings
            InitializeTimers();
            warning.Text = "";
        }

        private void InitializeTimers()
        {
            timer_focus.Start();
            timer1.Start();
            timerClearErrorLabel = new System.Windows.Forms.Timer { Interval = 5000 }; // 5 seconds
            timerClearErrorLabel.Tick += clearErrorLabelTimer_Tick;

            daychange.Interval = 60000; // 1 minute
            daychange.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lab_ip.Text = localIpAddress();
            FindStation();
            lab_sta.Text = station;
            this.Activate();
            tex_scan.Focus();
            check_counter(); // Check for day changes on app start

            ToggleQCVisibility();

            // Initialize LastDate only if not set
            if (Properties.Settings.Default.LastDate == DateTime.MinValue)
            {
                Properties.Settings.Default.LastDate = DateTime.Now.Date;
                Properties.Settings.Default.Save();
            }

            // Update the counter display
            label6.Text = Properties.Settings.Default.Count.ToString();
        }

        private void ToggleQCVisibility()
        {
            label7.Visible = !string.IsNullOrEmpty(QC);
            listBox1.Visible = !string.IsNullOrEmpty(QC);

            if (!string.IsNullOrEmpty(QC))
            {
                load_list();
            }
        }

        private async void tex_scan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                await ProcessBarcodeAsync();
            }
        }

        private async Task ProcessBarcodeAsync()
        {
            try
            {
                string[] barcode = tex_scan.Text.Split('=');
                tex_scan.Clear();

                if (barcode.Length == 2)
                {
                    await InsertBarcodeAsync(barcode[1]);
                }
                else
                {
                    ShowError("بارکد نمیتواند خالی باشد");
                    tex_scan.BackColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private async Task InsertBarcodeAsync(string barcode)
        {
            if (PreviousStation == WarehouseStation)
            {
                await InsertBarcodeForWarehouseAsync(barcode);
            }
            else
            {
                await InsertBarcodeForOtherStationsAsync(barcode);
            }
        }

        private async Task InsertBarcodeForWarehouseAsync(string barcode)
        {
            string query = @"
                SELECT CASE 
                WHEN EXISTS (SELECT 1 FROM ProcessRecords WHERE Serial_Number = @barcode AND Station_Name = @station)
                THEN 1 ELSE 0 END AS IsDuplicate";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                SqlCommand checkCommand = new SqlCommand(query, conn);
                checkCommand.Parameters.AddWithValue("@barcode", barcode);
                checkCommand.Parameters.AddWithValue("@station", station);

                bool isDuplicate = (int)await checkCommand.ExecuteScalarAsync() == 1;

                if (isDuplicate)
                {
                    ShowError("این شماره سریال یکبار اسکن شده است");
                    tex_scan.BackColor = Color.Red;
                }
                else
                {
                    string insertQuery = @"
                        INSERT INTO ProcessRecords (Serial_Number, Station_Name, Time, Date)
                        VALUES (@barcode, @station, @time, @date)";

                    SqlCommand insertCommand = new SqlCommand(insertQuery, conn);
                    insertCommand.Parameters.AddWithValue("@barcode", barcode);
                    insertCommand.Parameters.AddWithValue("@station", station);
                    insertCommand.Parameters.AddWithValue("@time", DateTime.Now.ToString("HH:mm:ss"));
                    insertCommand.Parameters.AddWithValue("@date", GetCurrentPersianDate());

                    await insertCommand.ExecuteNonQueryAsync();
                    ShowSuccess("ثبت شد");
                    UpdateCounter();
                }
            }
        }

        private async Task InsertBarcodeForOtherStationsAsync(string barcode)
        {
            string query = @"
                SELECT CASE 
                WHEN EXISTS (SELECT 1 FROM ProcessRecords WHERE Serial_Number = @barcode AND Station_Name = @station)
                THEN 1 ELSE 0 END AS IsDuplicate;

                SELECT CASE 
                WHEN EXISTS (SELECT 1 FROM ProcessRecords WHERE Serial_Number = @barcode AND Station_Name = @pre)
                THEN 1 ELSE 0 END AS HasRequiredStation";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                SqlCommand checkCommand = new SqlCommand(query, conn);
                checkCommand.Parameters.AddWithValue("@barcode", barcode);
                checkCommand.Parameters.AddWithValue("@station", station);
                checkCommand.Parameters.AddWithValue("@pre", PreviousStation);

                using (SqlDataReader reader = await checkCommand.ExecuteReaderAsync())
                {
                    reader.Read();
                    bool isDuplicate = reader.GetInt32(0) == 1;
                    reader.NextResult();
                    reader.Read();
                    bool hasRequiredStation = reader.GetInt32(0) == 1;

                    if (isDuplicate)
                    {
                        ShowError("این شماره سریال یکبار اسکن شده است");
                        tex_scan.BackColor = Color.Red;
                    }
                    else if (!hasRequiredStation)
                    {
                        ShowError($"این سریال از خط {PreviousStation} خارج نشده است");
                        tex_scan.BackColor = Color.Red;
                    }
                    else
                    {
                        string insertQuery = @"
                            INSERT INTO ProcessRecords (Serial_Number, Station_Name, Time, Date)
                            VALUES (@barcode, @station, @time, @date)";

                        SqlCommand insertCommand = new SqlCommand(insertQuery, conn);
                        insertCommand.Parameters.AddWithValue("@barcode", barcode);
                        insertCommand.Parameters.AddWithValue("@station", station);
                        insertCommand.Parameters.AddWithValue("@time", DateTime.Now.ToString("HH:mm:ss"));
                        insertCommand.Parameters.AddWithValue("@date", GetCurrentPersianDate());

                        await insertCommand.ExecuteNonQueryAsync();
                        ShowSuccess("ثبت شد");
                        UpdateCounter();
                    }
                }
            }
        }

        private void UpdateCounter()
        {
            if (station != QCWeldingStation && station != QCAssemblyStation)
            {
                lock (Properties.Settings.Default)
                {
                    Properties.Settings.Default.Count++;
                    Properties.Settings.Default.Save();
                }
                UpdateLabel6(Properties.Settings.Default.Count.ToString());
            }
        }

        private void UpdateLabel6(string text)
        {
            if (label6.InvokeRequired)
            {
                label6.Invoke(new Action(() => label6.Text = text));
            }
            else
            {
                label6.Text = text;
            }
        }

        private void ShowError(string message)
        {
            warning.Text = message;
            tex_scan.BackColor = Color.Red;
            clearErrorLabelTimer.Start();
        }

        private void ShowSuccess(string message)
        {
            warning.Text = message;
            tex_scan.BackColor = Color.Green;
            clearErrorLabelTimer.Start();
        }

        private void clearErrorLabelTimer_Tick(object sender, EventArgs e)
        {
            warning.Text = "";
            tex_scan.BackColor = Color.White;
            clearErrorLabelTimer.Stop();
        }

        private void FindStation()
        {
            station = string.Empty; // Initialize the station variable to store the result
            string select = "SELECT Name , PreviousStation , QC FROM Station WHERE IP = @ip";

            try
            {
                using (SqlConnection conn2 = new SqlConnection(connectionString))
                {
                    SqlCommand search = new SqlCommand(select, conn2);
                    search.Parameters.AddWithValue("@ip", localIpAddress());

                    conn2.Open();

                    using (SqlDataReader reader = search.ExecuteReader())
                    {
                        if (reader.HasRows) // Check if any rows were returned
                        {
                            reader.Read();
                            station = reader["Name"].ToString(); // Retrieve the 'Name' column value
                            mainStation = reader["Name"].ToString();
                            QC = reader["QC"].ToString();
                            PreviousStation = reader["PreviousStation"].ToString();
                        }
                        else
                        {
                            ShowError("هیچ ایستگاهی با این آی پی شناسایی نشد");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                ShowError("Database error: " + ex.Message);
            }
        }

        private string localIpAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork) // Only IPv4
                    {
                        return ip.ToString();
                    }
                }
                throw new Exception("سیستم فاقد آی پی ورژن 4 میباشد");
            }
            catch
            {
                ShowError("کارت شبکه و یا آی پی ورژن 4 شناسایی نشد");
                return string.Empty;
            }
        }

        private string LoadConnectionSettings()
        {
            string server = Properties.Settings.Default.DbServer;
            string database = Properties.Settings.Default.DbDatabase;
            string userId = Properties.Settings.Default.DbUserID;
            string password = Properties.Settings.Default.DbPass;

            return $"Server={server};Database={database};User Id={userId};Password={password};Encrypt=False;";
        }

        private void btn_setting_Click(object sender, EventArgs e)
        {
            SettingsForm form2 = new SettingsForm(this);
            form2.ShowDialog();
        }

        public void refreshdata()
        {
            lab_ip.Text = localIpAddress();
            FindStation();
            lab_sta.Text = station;
            label3.Text = GetCurrentPersianDate();
        }

        public static string GetCurrentPersianDate()
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            DateTime now = DateTime.Now;
            int persianYear = persianCalendar.GetYear(now);
            int persianMonth = persianCalendar.GetMonth(now);
            int persianDay = persianCalendar.GetDayOfMonth(now);
            return $"{persianYear:0000}/{persianMonth:00}/{persianDay:00}";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label5.Text = DateTime.Now.ToString("HH:mm:ss");

            string persianDate = GetCurrentPersianDate();
            if (label3.Text != persianDate)
            {
                label3.Text = persianDate;
            }
        }

        private void timer_focus_Tick(object sender, EventArgs e)
        {
            if (!tex_scan.Focused && ActiveControl != tex_scan)
            {
                tex_scan.Focus();
            }
        }

        private void daychange_Tick(object sender, EventArgs e)
        {
            check_counter();
        }

        private void check_counter()
        {
            if (Properties.Settings.Default.LastDate != DateTime.Now.Date)
            {
                Properties.Settings.Default.Count = 0;
                Properties.Settings.Default.LastDate = DateTime.Now.Date;
                Properties.Settings.Default.Save();
                label6.Text = Properties.Settings.Default.Count.ToString();
            }
        }

        private void load_list()
        {
            if (string.IsNullOrEmpty(QC))
            {
                return;
            }

            listBox1.Items.Clear();
            string load = @"
                SELECT Serial_Number
                FROM ProcessRecords
                WHERE Station_Name = @pre_station AND Date = @date
                  AND Serial_Number NOT IN (
                      SELECT Serial_Number
                      FROM ProcessRecords
                      WHERE Station_Name = @station
                  );";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand loader = new SqlCommand(load, conn);
                loader.Parameters.AddWithValue("@Station", QC);
                loader.Parameters.AddWithValue("@pre_station", station);
                loader.Parameters.AddWithValue("@date", GetCurrentPersianDate());

                using (SqlDataReader reader = loader.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listBox1.Items.Add(reader["Serial_Number"].ToString());
                    }
                }
                conn.Close();
            }
        }
    }
}