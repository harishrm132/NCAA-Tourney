
namespace TrackerUI
{
    partial class TournamentDashboardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TournamentDashboardForm));
            this.CreatePrizelabel = new System.Windows.Forms.Label();
            this.LoadExistingTournamentDropDown = new System.Windows.Forms.ComboBox();
            this.LoadExistingTournamentlabel = new System.Windows.Forms.Label();
            this.LoadTournamentbutton = new System.Windows.Forms.Button();
            this.CreateTournamentbutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CreatePrizelabel
            // 
            this.CreatePrizelabel.AutoSize = true;
            this.CreatePrizelabel.Font = new System.Drawing.Font("Segoe UI Light", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.CreatePrizelabel.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (51)))), ((int) (((byte) (153)))), ((int) (((byte) (255)))));
            this.CreatePrizelabel.Location = new System.Drawing.Point(77, 23);
            this.CreatePrizelabel.Name = "CreatePrizelabel";
            this.CreatePrizelabel.Size = new System.Drawing.Size(313, 41);
            this.CreatePrizelabel.TabIndex = 16;
            this.CreatePrizelabel.Text = "Tournament Dashboard";
            // 
            // LoadExistingTournamentDropDown
            // 
            this.LoadExistingTournamentDropDown.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.LoadExistingTournamentDropDown.FormattingEnabled = true;
            this.LoadExistingTournamentDropDown.Location = new System.Drawing.Point(49, 147);
            this.LoadExistingTournamentDropDown.Name = "LoadExistingTournamentDropDown";
            this.LoadExistingTournamentDropDown.Size = new System.Drawing.Size(369, 36);
            this.LoadExistingTournamentDropDown.TabIndex = 21;
            // 
            // LoadExistingTournamentlabel
            // 
            this.LoadExistingTournamentlabel.AutoSize = true;
            this.LoadExistingTournamentlabel.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.LoadExistingTournamentlabel.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (51)))), ((int) (((byte) (153)))), ((int) (((byte) (255)))));
            this.LoadExistingTournamentlabel.Location = new System.Drawing.Point(88, 105);
            this.LoadExistingTournamentlabel.Name = "LoadExistingTournamentlabel";
            this.LoadExistingTournamentlabel.Size = new System.Drawing.Size(290, 32);
            this.LoadExistingTournamentlabel.TabIndex = 20;
            this.LoadExistingTournamentlabel.Text = "Load Existing Tournament";
            // 
            // LoadTournamentbutton
            // 
            this.LoadTournamentbutton.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.LoadTournamentbutton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int) (((byte) (102)))), ((int) (((byte) (102)))), ((int) (((byte) (102)))));
            this.LoadTournamentbutton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int) (((byte) (242)))), ((int) (((byte) (242)))), ((int) (((byte) (242)))));
            this.LoadTournamentbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoadTournamentbutton.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.LoadTournamentbutton.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (51)))), ((int) (((byte) (153)))), ((int) (((byte) (255)))));
            this.LoadTournamentbutton.Location = new System.Drawing.Point(98, 200);
            this.LoadTournamentbutton.Name = "LoadTournamentbutton";
            this.LoadTournamentbutton.Size = new System.Drawing.Size(271, 42);
            this.LoadTournamentbutton.TabIndex = 22;
            this.LoadTournamentbutton.Text = "Load Tournament";
            this.LoadTournamentbutton.UseVisualStyleBackColor = true;
            this.LoadTournamentbutton.Click += new System.EventHandler(this.LoadTournamentbutton_Click);
            // 
            // CreateTournamentbutton
            // 
            this.CreateTournamentbutton.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.CreateTournamentbutton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int) (((byte) (102)))), ((int) (((byte) (102)))), ((int) (((byte) (102)))));
            this.CreateTournamentbutton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int) (((byte) (242)))), ((int) (((byte) (242)))), ((int) (((byte) (242)))));
            this.CreateTournamentbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CreateTournamentbutton.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.CreateTournamentbutton.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (51)))), ((int) (((byte) (153)))), ((int) (((byte) (255)))));
            this.CreateTournamentbutton.Location = new System.Drawing.Point(96, 278);
            this.CreateTournamentbutton.Name = "CreateTournamentbutton";
            this.CreateTournamentbutton.Size = new System.Drawing.Size(274, 86);
            this.CreateTournamentbutton.TabIndex = 23;
            this.CreateTournamentbutton.Text = "Create Tournament";
            this.CreateTournamentbutton.UseVisualStyleBackColor = true;
            this.CreateTournamentbutton.Click += new System.EventHandler(this.CreateTournamentbutton_Click);
            // 
            // TournamentDashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(471, 416);
            this.Controls.Add(this.CreateTournamentbutton);
            this.Controls.Add(this.LoadTournamentbutton);
            this.Controls.Add(this.LoadExistingTournamentDropDown);
            this.Controls.Add(this.LoadExistingTournamentlabel);
            this.Controls.Add(this.CreatePrizelabel);
            this.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "TournamentDashboardForm";
            this.Text = "Tournament Dashboard";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label CreatePrizelabel;
        private System.Windows.Forms.ComboBox LoadExistingTournamentDropDown;
        private System.Windows.Forms.Label LoadExistingTournamentlabel;
        private System.Windows.Forms.Button LoadTournamentbutton;
        private System.Windows.Forms.Button CreateTournamentbutton;
    }
}