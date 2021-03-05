namespace AI_LAB_2.Forms
{
    partial class PlotForm
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
            this.PlotPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PlotPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // PlotPictureBox
            // 
            this.PlotPictureBox.Location = new System.Drawing.Point(0, 0);
            this.PlotPictureBox.Name = "PlotPictureBox";
            this.PlotPictureBox.Size = new System.Drawing.Size(800, 600);
            this.PlotPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PlotPictureBox.TabIndex = 0;
            this.PlotPictureBox.TabStop = false;
            // 
            // PlotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(800, 761);
            this.Controls.Add(this.PlotPictureBox);
            this.Name = "PlotForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RawDataForm";
            ((System.ComponentModel.ISupportInitialize)(this.PlotPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PlotPictureBox;
    }
}