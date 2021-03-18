namespace WinFormsPlanes
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
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
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mainTimer = new System.Windows.Forms.Timer(this.components);
            this.lblHp = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblClick = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mainTimer
            // 
            this.mainTimer.Tick += new System.EventHandler(this.mainTimer_Tick);
            // 
            // lblHp
            // 
            this.lblHp.AutoSize = true;
            this.lblHp.Location = new System.Drawing.Point(16, 11);
            this.lblHp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHp.Name = "lblHp";
            this.lblHp.Size = new System.Drawing.Size(71, 17);
            this.lblHp.TabIndex = 0;
            this.lblHp.Text = "HP: 100%";
            this.lblHp.Visible = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(16, 27);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(84, 17);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Вы готовы?";
            this.lblTitle.Click += new System.EventHandler(this.lblTitle_Click);
            // 
            // lblClick
            // 
            this.lblClick.AutoSize = true;
            this.lblClick.Location = new System.Drawing.Point(16, 43);
            this.lblClick.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblClick.Name = "lblClick";
            this.lblClick.Size = new System.Drawing.Size(166, 17);
            this.lblClick.TabIndex = 2;
            this.lblClick.Text = "<Кликните на форму...>";
            this.lblClick.Click += new System.EventHandler(this.lblClick_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(16, 59);
            this.lblInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(173, 34);
            this.lblInfo.TabIndex = 3;
            this.lblInfo.Text = "A - перемещение влево\r\nD - перемещение вправо";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1312, 814);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lblClick);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblHp);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.Text = "WinFormsPlanes";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer mainTimer;
        private System.Windows.Forms.Label lblHp;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblClick;
        private System.Windows.Forms.Label lblInfo;
    }
}

