namespace Client
{
    partial class ClientForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {         
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.edtLogin = new System.Windows.Forms.TextBox();
            this.edtPassword = new System.Windows.Forms.TextBox();
            this.btnLogIn = new System.Windows.Forms.Button();
            this.lblLogIn = new System.Windows.Forms.Label();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.сервисToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.просмотрСпискаОнлайнУчастниковToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.lbxOnlineClients = new System.Windows.Forms.ListBox();
            this.lblOnlineClientsList = new System.Windows.Forms.Label();
            this.pbScreen = new System.Windows.Forms.PictureBox();
            this.помощьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu.SuspendLayout();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbScreen)).BeginInit();
            this.SuspendLayout();
            // 
            // edtLogin
            // 
            this.edtLogin.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.edtLogin.Location = new System.Drawing.Point(438, 262);
            this.edtLogin.MaximumSize = new System.Drawing.Size(193, 20);
            this.edtLogin.Name = "edtLogin";
            this.edtLogin.Size = new System.Drawing.Size(193, 20);
            this.edtLogin.TabIndex = 0;
            // 
            // edtPassword
            // 
            this.edtPassword.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.edtPassword.Location = new System.Drawing.Point(438, 298);
            this.edtPassword.MaximumSize = new System.Drawing.Size(193, 20);
            this.edtPassword.Name = "edtPassword";
            this.edtPassword.Size = new System.Drawing.Size(193, 20);
            this.edtPassword.TabIndex = 1;
            this.edtPassword.UseSystemPasswordChar = true;
            // 
            // btnLogIn
            // 
            this.btnLogIn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnLogIn.Location = new System.Drawing.Point(438, 335);
            this.btnLogIn.MaximumSize = new System.Drawing.Size(193, 23);
            this.btnLogIn.Name = "btnLogIn";
            this.btnLogIn.Size = new System.Drawing.Size(193, 23);
            this.btnLogIn.TabIndex = 2;
            this.btnLogIn.Text = "Авторизация";
            this.btnLogIn.UseVisualStyleBackColor = true;
            this.btnLogIn.Click += new System.EventHandler(this.btnLogIn_Click);
            // 
            // lblLogIn
            // 
            this.lblLogIn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblLogIn.AutoSize = true;
            this.lblLogIn.Location = new System.Drawing.Point(461, 237);
            this.lblLogIn.Name = "lblLogIn";
            this.lblLogIn.Size = new System.Drawing.Size(147, 13);
            this.lblLogIn.TabIndex = 3;
            this.lblLogIn.Text = "Авторизация пользователя";
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.сервисToolStripMenuItem,
            this.справкаToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(1104, 24);
            this.mainMenu.TabIndex = 4;
            this.mainMenu.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.выходToolStripMenuItem1});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // выходToolStripMenuItem1
            // 
            this.выходToolStripMenuItem1.Name = "выходToolStripMenuItem1";
            this.выходToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.выходToolStripMenuItem1.Text = "Выход";
            this.выходToolStripMenuItem1.Click += new System.EventHandler(this.выходToolStripMenuItem1_Click);
            // 
            // сервисToolStripMenuItem
            // 
            this.сервисToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.просмотрСпискаОнлайнУчастниковToolStripMenuItem,
            this.помощьToolStripMenuItem});
            this.сервисToolStripMenuItem.Name = "сервисToolStripMenuItem";
            this.сервисToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.сервисToolStripMenuItem.Text = "Сервис";
            // 
            // просмотрСпискаОнлайнУчастниковToolStripMenuItem
            // 
            this.просмотрСпискаОнлайнУчастниковToolStripMenuItem.Name = "просмотрСпискаОнлайнУчастниковToolStripMenuItem";
            this.просмотрСпискаОнлайнУчастниковToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
            this.просмотрСпискаОнлайнУчастниковToolStripMenuItem.Text = "Обновить список участников онлайн";
            this.просмотрСпискаОнлайнУчастниковToolStripMenuItem.Click += new System.EventHandler(this.просмотрСпискаОнлайнУчастниковToolStripMenuItem_Click);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.оПрограммеToolStripMenuItem});
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.справкаToolStripMenuItem.Text = "Справка";
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.оПрограммеToolStripMenuItem.Text = "О программе";
            this.оПрограммеToolStripMenuItem.Click += new System.EventHandler(this.оПрограммеToolStripMenuItem_Click);
            // 
            // mainPanel
            // 
            this.mainPanel.AutoScroll = true;
            this.mainPanel.AutoSize = true;
            this.mainPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.mainPanel.Controls.Add(this.lbxOnlineClients);
            this.mainPanel.Controls.Add(this.lblOnlineClientsList);
            this.mainPanel.Location = new System.Drawing.Point(0, 27);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(294, 600);
            this.mainPanel.TabIndex = 6;
            this.mainPanel.Visible = false;
            // 
            // lbxOnlineClients
            // 
            this.lbxOnlineClients.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbxOnlineClients.FormattingEnabled = true;
            this.lbxOnlineClients.Location = new System.Drawing.Point(16, 50);
            this.lbxOnlineClients.Name = "lbxOnlineClients";
            this.lbxOnlineClients.ScrollAlwaysVisible = true;
            this.lbxOnlineClients.Size = new System.Drawing.Size(244, 316);
            this.lbxOnlineClients.TabIndex = 8;
            this.lbxOnlineClients.SelectedIndexChanged += new System.EventHandler(this.lbxOnlineClients_SelectedIndexChanged);
            // 
            // lblOnlineClientsList
            // 
            this.lblOnlineClientsList.AutoSize = true;
            this.lblOnlineClientsList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblOnlineClientsList.ForeColor = System.Drawing.Color.Black;
            this.lblOnlineClientsList.Location = new System.Drawing.Point(13, 22);
            this.lblOnlineClientsList.Name = "lblOnlineClientsList";
            this.lblOnlineClientsList.Size = new System.Drawing.Size(133, 13);
            this.lblOnlineClientsList.TabIndex = 7;
            this.lblOnlineClientsList.Text = "Список клиентов онлайн";
            // 
            // pbScreen
            // 
            this.pbScreen.BackColor = System.Drawing.Color.Silver;
            this.pbScreen.Location = new System.Drawing.Point(300, 27);
            this.pbScreen.Name = "pbScreen";
            this.pbScreen.Size = new System.Drawing.Size(800, 600);
            this.pbScreen.TabIndex = 7;
            this.pbScreen.TabStop = false;
            this.pbScreen.Visible = false;
            // 
            // помощьToolStripMenuItem
            // 
            this.помощьToolStripMenuItem.Name = "помощьToolStripMenuItem";
            this.помощьToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
            this.помощьToolStripMenuItem.Text = "Помощь";
            this.помощьToolStripMenuItem.Click += new System.EventHandler(this.помощьToolStripMenuItem_Click_1);
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1104, 627);
            this.Controls.Add(this.pbScreen);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.lblLogIn);
            this.Controls.Add(this.btnLogIn);
            this.Controls.Add(this.edtPassword);
            this.Controls.Add(this.edtLogin);
            this.Controls.Add(this.mainMenu);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.mainMenu;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1120, 665);
            this.MinimumSize = new System.Drawing.Size(1120, 665);
            this.Name = "ClientForm";
            this.Text = "Remote Desktop application";
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbScreen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox edtLogin;
        private System.Windows.Forms.TextBox edtPassword;
        private System.Windows.Forms.Button btnLogIn;
        private System.Windows.Forms.Label lblLogIn;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem сервисToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem просмотрСпискаОнлайнУчастниковToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Label lblOnlineClientsList;
        private System.Windows.Forms.ListBox lbxOnlineClients;
        private System.Windows.Forms.PictureBox pbScreen;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem помощьToolStripMenuItem;

    }
}

