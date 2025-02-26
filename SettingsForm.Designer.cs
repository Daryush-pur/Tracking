namespace Tracking
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lab_server = new System.Windows.Forms.Label();
            this.lab_database = new System.Windows.Forms.Label();
            this.lab_user = new System.Windows.Forms.Label();
            this.lab_pass = new System.Windows.Forms.Label();
            this.text_server = new System.Windows.Forms.TextBox();
            this.text_database = new System.Windows.Forms.TextBox();
            this.text_user = new System.Windows.Forms.TextBox();
            this.text_pass = new System.Windows.Forms.TextBox();
            this.btn_save = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lab_server
            // 
            this.lab_server.AutoSize = true;
            this.lab_server.Font = new System.Drawing.Font("0 Badr", 14.25F);
            this.lab_server.Location = new System.Drawing.Point(78, 76);
            this.lab_server.Name = "lab_server";
            this.lab_server.Size = new System.Drawing.Size(73, 34);
            this.lab_server.TabIndex = 0;
            this.lab_server.Text = "Server";
            // 
            // lab_database
            // 
            this.lab_database.AutoSize = true;
            this.lab_database.Font = new System.Drawing.Font("0 Badr", 14.25F);
            this.lab_database.Location = new System.Drawing.Point(78, 145);
            this.lab_database.Name = "lab_database";
            this.lab_database.Size = new System.Drawing.Size(99, 34);
            this.lab_database.TabIndex = 1;
            this.lab_database.Text = "Database";
            // 
            // lab_user
            // 
            this.lab_user.AutoSize = true;
            this.lab_user.Font = new System.Drawing.Font("0 Badr", 14.25F);
            this.lab_user.Location = new System.Drawing.Point(78, 220);
            this.lab_user.Name = "lab_user";
            this.lab_user.Size = new System.Drawing.Size(77, 34);
            this.lab_user.TabIndex = 2;
            this.lab_user.Text = "User Id";
            // 
            // lab_pass
            // 
            this.lab_pass.AutoSize = true;
            this.lab_pass.Font = new System.Drawing.Font("0 Badr", 14.25F);
            this.lab_pass.Location = new System.Drawing.Point(78, 295);
            this.lab_pass.Name = "lab_pass";
            this.lab_pass.Size = new System.Drawing.Size(101, 34);
            this.lab_pass.TabIndex = 3;
            this.lab_pass.Text = "Password";
            // 
            // text_server
            // 
            this.text_server.Location = new System.Drawing.Point(222, 84);
            this.text_server.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.text_server.Name = "text_server";
            this.text_server.Size = new System.Drawing.Size(176, 26);
            this.text_server.TabIndex = 4;
            // 
            // text_database
            // 
            this.text_database.Location = new System.Drawing.Point(222, 154);
            this.text_database.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.text_database.Name = "text_database";
            this.text_database.Size = new System.Drawing.Size(176, 26);
            this.text_database.TabIndex = 5;
            // 
            // text_user
            // 
            this.text_user.Location = new System.Drawing.Point(222, 226);
            this.text_user.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.text_user.Name = "text_user";
            this.text_user.Size = new System.Drawing.Size(176, 26);
            this.text_user.TabIndex = 6;
            // 
            // text_pass
            // 
            this.text_pass.Location = new System.Drawing.Point(222, 301);
            this.text_pass.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.text_pass.Name = "text_pass";
            this.text_pass.Size = new System.Drawing.Size(176, 26);
            this.text_pass.TabIndex = 7;
            // 
            // btn_save
            // 
            this.btn_save.Font = new System.Drawing.Font("0 Badr", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.btn_save.Location = new System.Drawing.Point(194, 387);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(121, 51);
            this.btn_save.TabIndex = 8;
            this.btn_save.Text = "ذخیره";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 476);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.text_pass);
            this.Controls.Add(this.text_user);
            this.Controls.Add(this.text_database);
            this.Controls.Add(this.text_server);
            this.Controls.Add(this.lab_pass);
            this.Controls.Add(this.lab_user);
            this.Controls.Add(this.lab_database);
            this.Controls.Add(this.lab_server);
            this.Font = new System.Drawing.Font("B Zar", 8.25F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingsForm_FormClosed);
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lab_server;
        private System.Windows.Forms.Label lab_database;
        private System.Windows.Forms.Label lab_user;
        private System.Windows.Forms.Label lab_pass;
        private System.Windows.Forms.TextBox text_server;
        private System.Windows.Forms.TextBox text_database;
        private System.Windows.Forms.TextBox text_user;
        private System.Windows.Forms.TextBox text_pass;
        private System.Windows.Forms.Button btn_save;
    }
}