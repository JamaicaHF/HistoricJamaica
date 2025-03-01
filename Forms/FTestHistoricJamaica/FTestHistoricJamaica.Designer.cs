namespace HistoricJamaica
{
    partial class FTestHistoricJamaica
    {
        private System.ComponentModel.IContainer components = null;
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
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.Person_Checkbox = new System.Windows.Forms.CheckBox();
            this.RunTest_Button = new System.Windows.Forms.Button();
            this.VitalRecord_CheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(251, 189);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(120, 274);
            this.checkedListBox1.TabIndex = 0;
            // 
            // Person_Checkbox
            // 
            this.Person_Checkbox.AutoSize = true;
            this.Person_Checkbox.Location = new System.Drawing.Point(261, 216);
            this.Person_Checkbox.Name = "Person_Checkbox";
            this.Person_Checkbox.Size = new System.Drawing.Size(83, 17);
            this.Person_Checkbox.TabIndex = 1;
            this.Person_Checkbox.Text = "Person Test";
            this.Person_Checkbox.UseVisualStyleBackColor = true;
            // 
            // RunTest_Button
            // 
            this.RunTest_Button.Location = new System.Drawing.Point(269, 427);
            this.RunTest_Button.Name = "RunTest_Button";
            this.RunTest_Button.Size = new System.Drawing.Size(75, 23);
            this.RunTest_Button.TabIndex = 2;
            this.RunTest_Button.Text = "Run Tests";
            this.RunTest_Button.UseVisualStyleBackColor = true;
            this.RunTest_Button.Click += new System.EventHandler(this.RunTests_button_Click);
            // 
            // VitalRecord_CheckBox
            // 
            this.VitalRecord_CheckBox.AutoSize = true;
            this.VitalRecord_CheckBox.Location = new System.Drawing.Point(261, 239);
            this.VitalRecord_CheckBox.Name = "VitalRecord_CheckBox";
            this.VitalRecord_CheckBox.Size = new System.Drawing.Size(108, 17);
            this.VitalRecord_CheckBox.TabIndex = 3;
            this.VitalRecord_CheckBox.Text = "Vital Record Test";
            this.VitalRecord_CheckBox.UseVisualStyleBackColor = true;
            // 
            // FTestHistoricJamaica
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 560);
            this.Controls.Add(this.VitalRecord_CheckBox);
            this.Controls.Add(this.RunTest_Button);
            this.Controls.Add(this.Person_Checkbox);
            this.Controls.Add(this.checkedListBox1);
            this.Name = "FTestHistoricJamaica";
            this.Text = "TestHistoricJamaica";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.CheckBox Person_Checkbox;
        private System.Windows.Forms.Button RunTest_Button;
        private System.Windows.Forms.CheckBox VitalRecord_CheckBox;
    }
}