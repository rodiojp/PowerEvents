
namespace PowerEvents.WinForm
{
    partial class PowerEventsForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PowerEventsForm));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.miPE = new System.Windows.Forms.MenuItem();
            this.miAutoStart = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.miExit = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miPE});
            // 
            // miPE
            // 
            this.miPE.Index = 0;
            this.miPE.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miAutoStart,
            this.menuItem3,
            this.miExit});
            this.miPE.Text = "&PE";
            // 
            // miAutoStart
            // 
            this.miAutoStart.Index = 0;
            this.miAutoStart.Text = "&Auto Start";
            this.miAutoStart.Click += new System.EventHandler(this.miAutoStart_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "-";
            // 
            // miExit
            // 
            this.miExit.Index = 2;
            this.miExit.Text = "E&xit";
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // PowerEventsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 424);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.Name = "PowerEventsForm";
            this.Text = "Power Events";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PowerEventsForm_FormClosing);
            this.Load += new System.EventHandler(this.PowerEventsForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem miPE;
        private System.Windows.Forms.MenuItem miAutoStart;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem miExit;
    }
}