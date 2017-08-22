namespace WindowsFormsApplication2
{
    partial class contactForm
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
            this.ChatBtn = new System.Windows.Forms.Button();
            this.ContactList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ChatBtn
            // 
            this.ChatBtn.Location = new System.Drawing.Point(3, 254);
            this.ChatBtn.Name = "ChatBtn";
            this.ChatBtn.Size = new System.Drawing.Size(75, 25);
            this.ChatBtn.TabIndex = 1;
            this.ChatBtn.Text = "Chat";
            this.ChatBtn.UseVisualStyleBackColor = true;
            this.ChatBtn.Click += new System.EventHandler(this.ChatBtn_Click);
            // 
            // ContactList
            // 
            this.ContactList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ContactList.FormattingEnabled = true;
            this.ContactList.ItemHeight = 15;
            this.ContactList.Location = new System.Drawing.Point(3, 19);
            this.ContactList.Name = "ContactList";
            this.ContactList.Size = new System.Drawing.Size(207, 229);
            this.ContactList.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Contacts";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.ContactList);
            this.flowLayoutPanel1.Controls.Add(this.ChatBtn);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(210, 288);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // contactForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(234, 312);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "contactForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chat Application";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.contactForm_FormClosed);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ChatBtn;
        private System.Windows.Forms.ListBox ContactList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;

    }
}

