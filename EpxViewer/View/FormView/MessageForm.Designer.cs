namespace EpxViewer
{
    partial class MessageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageForm));
            this.RootPane = new System.Windows.Forms.Panel();
            this.CenterPane = new System.Windows.Forms.Panel();
            this.MessageLabel = new System.Windows.Forms.Label();
            this.BottomPane = new System.Windows.Forms.Panel();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OkBtn = new System.Windows.Forms.Button();
            this.TopPane = new System.Windows.Forms.Panel();
            this.AlertLabel = new System.Windows.Forms.Label();
            this.RootPane.SuspendLayout();
            this.CenterPane.SuspendLayout();
            this.BottomPane.SuspendLayout();
            this.TopPane.SuspendLayout();
            this.SuspendLayout();
            // 
            // RootPane
            // 
            this.RootPane.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RootPane.Controls.Add(this.CenterPane);
            this.RootPane.Controls.Add(this.BottomPane);
            this.RootPane.Controls.Add(this.TopPane);
            this.RootPane.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RootPane.Location = new System.Drawing.Point(0, 0);
            this.RootPane.Margin = new System.Windows.Forms.Padding(0);
            this.RootPane.Name = "RootPane";
            this.RootPane.Size = new System.Drawing.Size(350, 250);
            this.RootPane.TabIndex = 0;
            this.RootPane.Paint += new System.Windows.Forms.PaintEventHandler(this.RootPane_Paint);
            // 
            // CenterPane
            // 
            this.CenterPane.BackColor = System.Drawing.Color.LightGray;
            this.CenterPane.Controls.Add(this.MessageLabel);
            this.CenterPane.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CenterPane.Location = new System.Drawing.Point(0, 60);
            this.CenterPane.Name = "CenterPane";
            this.CenterPane.Size = new System.Drawing.Size(348, 118);
            this.CenterPane.TabIndex = 0;
            // 
            // MessageLabel
            // 
            this.MessageLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MessageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MessageLabel.Location = new System.Drawing.Point(0, 0);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(348, 118);
            this.MessageLabel.TabIndex = 0;
            this.MessageLabel.Text = "Message";
            this.MessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BottomPane
            // 
            this.BottomPane.BackColor = System.Drawing.Color.White;
            this.BottomPane.Controls.Add(this.CancelBtn);
            this.BottomPane.Controls.Add(this.OkBtn);
            this.BottomPane.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPane.Location = new System.Drawing.Point(0, 178);
            this.BottomPane.Name = "BottomPane";
            this.BottomPane.Size = new System.Drawing.Size(348, 70);
            this.BottomPane.TabIndex = 0;
            // 
            // CancelBtn
            // 
            this.CancelBtn.BackColor = System.Drawing.Color.Transparent;
            this.CancelBtn.FlatAppearance.BorderSize = 0;
            this.CancelBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.CancelBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.CancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CancelBtn.Image = ((System.Drawing.Image)(resources.GetObject("CancelBtn.Image")));
            this.CancelBtn.Location = new System.Drawing.Point(246, 10);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(47, 46);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.UseVisualStyleBackColor = false;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            this.CancelBtn.MouseLeave += new System.EventHandler(this.CancelBtn_MouseLeave);
            this.CancelBtn.MouseHover += new System.EventHandler(this.CancelBtn_MouseHover);
            // 
            // OkBtn
            // 
            this.OkBtn.BackColor = System.Drawing.Color.Transparent;
            this.OkBtn.FlatAppearance.BorderSize = 0;
            this.OkBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.OkBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.OkBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OkBtn.ForeColor = System.Drawing.Color.Transparent;
            this.OkBtn.Image = ((System.Drawing.Image)(resources.GetObject("OkBtn.Image")));
            this.OkBtn.Location = new System.Drawing.Point(53, 9);
            this.OkBtn.Margin = new System.Windows.Forms.Padding(0);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(47, 49);
            this.OkBtn.TabIndex = 0;
            this.OkBtn.UseVisualStyleBackColor = false;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            this.OkBtn.MouseLeave += new System.EventHandler(this.OkBtnMouseLeave);
            this.OkBtn.MouseHover += new System.EventHandler(this.OkBtnMouseHover);
            // 
            // TopPane
            // 
            this.TopPane.BackColor = System.Drawing.Color.White;
            this.TopPane.Controls.Add(this.AlertLabel);
            this.TopPane.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPane.Location = new System.Drawing.Point(0, 0);
            this.TopPane.Name = "TopPane";
            this.TopPane.Size = new System.Drawing.Size(348, 60);
            this.TopPane.TabIndex = 0;
            // 
            // AlertLabel
            // 
            this.AlertLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AlertLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AlertLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AlertLabel.ForeColor = System.Drawing.Color.DimGray;
            this.AlertLabel.Location = new System.Drawing.Point(0, 0);
            this.AlertLabel.Name = "AlertLabel";
            this.AlertLabel.Size = new System.Drawing.Size(348, 60);
            this.AlertLabel.TabIndex = 0;
            this.AlertLabel.Text = "Alert";
            this.AlertLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.AlertLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MovePane_MouseDown);
            // 
            // MessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(350, 250);
            this.ControlBox = false;
            this.Controls.Add(this.RootPane);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageForm";
            this.Opacity = 0D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MessageForm";
            this.TransparencyKey = System.Drawing.Color.WhiteSmoke;
            this.RootPane.ResumeLayout(false);
            this.CenterPane.ResumeLayout(false);
            this.BottomPane.ResumeLayout(false);
            this.TopPane.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel RootPane;
        private System.Windows.Forms.Panel CenterPane;
        private System.Windows.Forms.Panel TopPane;
        private System.Windows.Forms.Panel BottomPane;
        private System.Windows.Forms.Label AlertLabel;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.Label MessageLabel;
    }
}