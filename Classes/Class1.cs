using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HistoricJamaica
{
public class Form3 : Form
{
public Form3()
{
InitializeComponent();

this.dataGridView1.ContextMenuStrip = new ContextMenuStrip();
this.dataGridView1.ContextMenuStrip.Items.Add("Test 1");
this.dataGridView1.ContextMenuStrip.Items.Add("test 2");
this.dataGridView1.ContextMenuStrip.Items.Add("Test 3");


this.dataGridView1.ContextMenuStrip.Items.AddRange(tsmFile.DropDownItems);

//foreach (ToolStripItem t in tsmFile.DropDownItems)
// this.dataGridView1.ContextMenuStrip.Items.Add(t);
}

private void deleteToolStripMenuItem_Click(object sender,
EventArgs e)
{ MessageBox.Show("Clicked delete!"); }

private void editToolStripMenuItem_Click(object sender,
EventArgs e)
{ MessageBox.Show("Clicked edit!"); }

private void addToolStripMenuItem_Click(object sender, EventArgs e)
{ MessageBox.Show("Clicked add!"); }

#region Designer-Generated
/// <summary>
/// Required designer variable.
/// </summary>
private System.ComponentModel.IContainer components = null;

/// <summary>
/// Clean up any resources being used.
/// </summary>
/// <param name="disposing">true if managed resources should be
//disposed; otherwise, false.</param>
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
this.dataGridView1 = new System.Windows.Forms.DataGridView();
this.menuStrip1 = new System.Windows.Forms.MenuStrip();
this.tsmFile = new System.Windows.Forms.ToolStripMenuItem();
this.addToolStripMenuItem = new
System.Windows.Forms.ToolStripMenuItem();
this.editToolStripMenuItem = new
System.Windows.Forms.ToolStripMenuItem();
this.deleteToolStripMenuItem = new
System.Windows.Forms.ToolStripMenuItem();

((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
this.menuStrip1.SuspendLayout();
this.SuspendLayout();
//
// dataGridView1
//
this.dataGridView1.ColumnHeadersHeightSizeMode =
System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
this.dataGridView1.Location = new System.Drawing.Point(37, 28);
this.dataGridView1.Name = "dataGridView1";
this.dataGridView1.Size = new System.Drawing.Size(240, 150);
this.dataGridView1.TabIndex = 0;
//
// menuStrip1
//
this.menuStrip1.Items.AddRange(new
System.Windows.Forms.ToolStripItem[] {
this.tsmFile});
this.menuStrip1.Location = new System.Drawing.Point(0, 0);
this.menuStrip1.Name = "menuStrip1";
this.menuStrip1.Size = new System.Drawing.Size(292, 24);
this.menuStrip1.TabIndex = 1;
this.menuStrip1.Text = "menuStrip1";
//
// tsmFile
//
this.tsmFile.DropDownItems.AddRange(new
System.Windows.Forms.ToolStripItem[] {
this.addToolStripMenuItem,
this.editToolStripMenuItem,
this.deleteToolStripMenuItem});
this.tsmFile.Name = "tsmFile";
this.tsmFile.Size = new System.Drawing.Size(35, 20);
this.tsmFile.Text = "File";
//
// addToolStripMenuItem
//
this.addToolStripMenuItem.Name = "addToolStripMenuItem";
this.addToolStripMenuItem.Size = new
System.Drawing.Size(152, 22);
this.addToolStripMenuItem.Text = "Add";
this.addToolStripMenuItem.Click += new
System.EventHandler(this.addToolStripMenuItem_Click);
//
// editToolStripMenuItem
//
this.editToolStripMenuItem.Name = "editToolStripMenuItem";
this.editToolStripMenuItem.Size = new
System.Drawing.Size(152, 22);
this.editToolStripMenuItem.Text = "Edit";
this.editToolStripMenuItem.Click += new
System.EventHandler(this.editToolStripMenuItem_Click);
//
// deleteToolStripMenuItem
//
this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
this.deleteToolStripMenuItem.Size = new
System.Drawing.Size(152, 22);
this.deleteToolStripMenuItem.Text = "Delete";
this.deleteToolStripMenuItem.Click += new
System.EventHandler(this.deleteToolStripMenuItem_Click);
//
// Form3
//
this.ClientSize = new System.Drawing.Size(292, 266);
this.Controls.Add(this.dataGridView1);
this.Controls.Add(this.menuStrip1);
this.MainMenuStrip = this.menuStrip1;
this.Name = "Form3";

((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
this.menuStrip1.ResumeLayout(false);
this.menuStrip1.PerformLayout();
this.ResumeLayout(false);
this.PerformLayout();

}

#endregion

private System.Windows.Forms.DataGridView dataGridView1;
private System.Windows.Forms.MenuStrip menuStrip1;
private System.Windows.Forms.ToolStripMenuItem tsmFile;
private System.Windows.Forms.ToolStripMenuItem
addToolStripMenuItem;
private System.Windows.Forms.ToolStripMenuItem
editToolStripMenuItem;
private System.Windows.Forms.ToolStripMenuItem
deleteToolStripMenuItem;
#endregion
}
}
