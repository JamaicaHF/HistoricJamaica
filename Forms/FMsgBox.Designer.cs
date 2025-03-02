
namespace HistoricJamaica
{
    partial class FMsgBox
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.Msg1 = new System.Windows.Forms.Label();
            this.Msg2 = new System.Windows.Forms.Label();
            this.Msg3 = new System.Windows.Forms.Label();
            this.Msg4 = new System.Windows.Forms.Label();
            this.Msg5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(60, 227);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 30);
            this.button1.TabIndex = 0;
            this.button1.Text = "Yes";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(169, 227);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 30);
            this.button2.TabIndex = 1;
            this.button2.Text = "No";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Msg1
            // 
            this.Msg1.AutoSize = true;
            this.Msg1.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Msg1.Location = new System.Drawing.Point(41, 30);
            this.Msg1.Name = "Msg1";
            this.Msg1.Size = new System.Drawing.Size(48, 18);
            this.Msg1.TabIndex = 2;
            this.Msg1.Text = "Msg1";
            // 
            // Msg2
            // 
            this.Msg2.AutoSize = true;
            this.Msg2.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Msg2.Location = new System.Drawing.Point(41, 60);
            this.Msg2.Name = "Msg2";
            this.Msg2.Size = new System.Drawing.Size(48, 18);
            this.Msg2.TabIndex = 3;
            this.Msg2.Text = "Msg2";
            // 
            // Msg3
            // 
            this.Msg3.AutoSize = true;
            this.Msg3.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Msg3.Location = new System.Drawing.Point(41, 90);
            this.Msg3.Name = "Msg3";
            this.Msg3.Size = new System.Drawing.Size(48, 18);
            this.Msg3.TabIndex = 4;
            this.Msg3.Text = "Msg3";
            // 
            // Msg4
            // 
            this.Msg4.AutoSize = true;
            this.Msg4.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Msg4.Location = new System.Drawing.Point(41, 120);
            this.Msg4.Name = "Msg4";
            this.Msg4.Size = new System.Drawing.Size(48, 18);
            this.Msg4.TabIndex = 5;
            this.Msg4.Text = "Msg4";
            // 
            // Msg5
            // 
            this.Msg5.AutoSize = true;
            this.Msg5.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Msg5.Location = new System.Drawing.Point(41, 150);
            this.Msg5.Name = "Msg5";
            this.Msg5.Size = new System.Drawing.Size(48, 18);
            this.Msg5.TabIndex = 6;
            this.Msg5.Text = "Msg5";
            // 
            // MsgBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 285);
            this.Controls.Add(this.Msg5);
            this.Controls.Add(this.Msg4);
            this.Controls.Add(this.Msg3);
            this.Controls.Add(this.Msg2);
            this.Controls.Add(this.Msg1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Location = new System.Drawing.Point(100, 100);
            this.Name = "MsgBox";
            this.Text = "Message";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label Msg1;
        private System.Windows.Forms.Label Msg2;
        private System.Windows.Forms.Label Msg3;
        private System.Windows.Forms.Label Msg4;
        private System.Windows.Forms.Label Msg5;
    }
}