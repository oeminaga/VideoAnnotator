namespace VideoAnnotatot
{
    partial class GantChart
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // GantChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "GantChart";
            this.Size = new System.Drawing.Size(448, 143);
            this.Load += new System.EventHandler(this.GantChart_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GantChart_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GantChart_MouseDown);
            this.MouseHover += new System.EventHandler(this.GantChart_MouseHover);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GantChart_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GantChart_MouseUp);
            this.Resize += new System.EventHandler(this.GantChart_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
    }
}
