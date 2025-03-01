using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    public class FFamilyTree : Form
    {
        private TextBox A1_textBox;
        private TextBox B1_textBox;
        private TextBox C3_textBox;
        private TextBox D6_textBox;
        private TextBox B2_textBox;
        private TextBox C2_textBox;
        private TextBox D1_textBox;
        private TextBox E1_textBox;
        private TextBox E2_textBox;
        private TextBox E3_textBox;
        private TextBox E4_textBox;
        private TextBox E5_textBox;
        private TextBox E6_textBox;
        private TextBox E7_textBox;
        private TextBox E8_textBox;
        private TextBox E9_textBox;
        private TextBox D2_textBox;
        private TextBox D4_textBox;
        private TextBox D5_textBox;
        private TextBox C1_textBox;
        private TextBox D7_textBox;
        private TextBox F1_textBox;
        private TextBox F2_textBox;
        private TextBox F3_textBox;
        private TextBox F4_textBox;
        private TextBox F5_textBox;
        private TextBox F6_textBox;
        private TextBox F7_textBox;
        private TextBox F8_textBox;
        private TextBox F9_textBox;
        private TextBox F10_textBox;
        private TextBox F11_textBox;
        private TextBox F12_textBox;
        private TextBox F13_textBox;
        private TextBox F14_textBox;
        private TextBox F15_textBox;
        private TextBox F16_textBox;
        private TextBox F17_textBox;
        private TextBox F18_textBox;
        private TextBox F19_textBox;
        private TextBox F20_textBox;
        private TextBox F21_textBox;
        private TextBox F22_textBox;
        private TextBox F23_textBox;
        private TextBox F24_textBox;
        private TextBox F25_textBox;
        private TextBox E10_textBox;
        private TextBox E11_textBox;
        private TextBox E12_textBox;
        private TextBox E13_textBox;
        private TextBox F26_textBox;
        private TextBox F27_textBox;
        private TextBox F28_textBox;
        private TextBox F29_textBox;
        private TextBox F30_textBox;
        private TextBox E14_textBox;
        private TextBox E15_textBox;
        private TextBox F31_textBox;
        private TextBox F32_textBox;
        private TextBox E16_textBox;
        private TextBox D3_textBox;
        private TextBox D8_textBox;
        private TextBox C4_textBox;
        private System.ComponentModel.IContainer components = null;
        //****************************************************************************************************************************
        public FFamilyTree()
        {
            InitializeComponent();
        }
        //****************************************************************************************************************************
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        //****************************************************************************************************************************
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics G = e.Graphics;
            UU.PaintParentLines(G, A1_textBox,  B1_textBox,  B2_textBox);
            UU.PaintParentLines(G, B1_textBox, C1_textBox, C2_textBox);
            UU.PaintParentLines(G, B2_textBox, C3_textBox, C4_textBox);
            UU.PaintParentLines(G, C1_textBox, D1_textBox, D2_textBox);
            UU.PaintParentLines(G, C2_textBox, D3_textBox, D4_textBox);
            UU.PaintParentLines(G, C3_textBox, D5_textBox, D6_textBox);
            UU.PaintParentLines(G, C4_textBox, D7_textBox, D8_textBox);
            UU.PaintParentLines(G, D1_textBox, E1_textBox, E2_textBox);
            UU.PaintParentLines(G, D2_textBox, E3_textBox, E4_textBox);
            UU.PaintParentLines(G, D3_textBox, E5_textBox, E6_textBox);
            UU.PaintParentLines(G, D4_textBox, E7_textBox, E8_textBox);
            UU.PaintParentLines(G, D5_textBox, E9_textBox, E10_textBox);
            UU.PaintParentLines(G, D6_textBox, E11_textBox, E12_textBox);
            UU.PaintParentLines(G, D7_textBox, E13_textBox, E14_textBox);
            UU.PaintParentLines(G, D8_textBox, E15_textBox, E16_textBox);
            UU.PaintParentLines(G, E1_textBox, F1_textBox, F2_textBox);
            UU.PaintParentLines(G, E2_textBox, F3_textBox, F4_textBox);
            UU.PaintParentLines(G, E3_textBox, F5_textBox, F6_textBox);
            UU.PaintParentLines(G, E4_textBox, F7_textBox, F8_textBox);
            UU.PaintParentLines(G, E5_textBox, F9_textBox, F10_textBox);
            UU.PaintParentLines(G, E6_textBox, F11_textBox, F12_textBox);
            UU.PaintParentLines(G, E7_textBox, F13_textBox, F14_textBox);
            UU.PaintParentLines(G, E8_textBox, F15_textBox, F16_textBox);
            UU.PaintParentLines(G, E9_textBox, F17_textBox, F18_textBox);
            UU.PaintParentLines(G, E10_textBox, F19_textBox, F20_textBox);
            UU.PaintParentLines(G, E11_textBox, F21_textBox, F22_textBox);
            UU.PaintParentLines(G, E12_textBox, F23_textBox, F24_textBox);
            UU.PaintParentLines(G, E13_textBox, F25_textBox, F26_textBox);
            UU.PaintParentLines(G, E14_textBox, F27_textBox, F28_textBox);
            UU.PaintParentLines(G, E15_textBox, F29_textBox, F30_textBox);
            UU.PaintParentLines(G, E16_textBox, F31_textBox, F32_textBox);
            base.OnPaint(e);
        }
        //****************************************************************************************************************************
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.A1_textBox = new System.Windows.Forms.TextBox();
            this.B1_textBox = new System.Windows.Forms.TextBox();
            this.C3_textBox = new System.Windows.Forms.TextBox();
            this.D6_textBox = new System.Windows.Forms.TextBox();
            this.B2_textBox = new System.Windows.Forms.TextBox();
            this.C2_textBox = new System.Windows.Forms.TextBox();
            this.D1_textBox = new System.Windows.Forms.TextBox();
            this.E1_textBox = new System.Windows.Forms.TextBox();
            this.E2_textBox = new System.Windows.Forms.TextBox();
            this.E3_textBox = new System.Windows.Forms.TextBox();
            this.E4_textBox = new System.Windows.Forms.TextBox();
            this.E5_textBox = new System.Windows.Forms.TextBox();
            this.E6_textBox = new System.Windows.Forms.TextBox();
            this.E7_textBox = new System.Windows.Forms.TextBox();
            this.E8_textBox = new System.Windows.Forms.TextBox();
            this.E9_textBox = new System.Windows.Forms.TextBox();
            this.D2_textBox = new System.Windows.Forms.TextBox();
            this.D4_textBox = new System.Windows.Forms.TextBox();
            this.D5_textBox = new System.Windows.Forms.TextBox();
            this.C1_textBox = new System.Windows.Forms.TextBox();
            this.D7_textBox = new System.Windows.Forms.TextBox();
            this.F1_textBox = new System.Windows.Forms.TextBox();
            this.F2_textBox = new System.Windows.Forms.TextBox();
            this.F3_textBox = new System.Windows.Forms.TextBox();
            this.F4_textBox = new System.Windows.Forms.TextBox();
            this.F5_textBox = new System.Windows.Forms.TextBox();
            this.F6_textBox = new System.Windows.Forms.TextBox();
            this.F7_textBox = new System.Windows.Forms.TextBox();
            this.F8_textBox = new System.Windows.Forms.TextBox();
            this.F9_textBox = new System.Windows.Forms.TextBox();
            this.F10_textBox = new System.Windows.Forms.TextBox();
            this.F11_textBox = new System.Windows.Forms.TextBox();
            this.F12_textBox = new System.Windows.Forms.TextBox();
            this.F13_textBox = new System.Windows.Forms.TextBox();
            this.F14_textBox = new System.Windows.Forms.TextBox();
            this.F15_textBox = new System.Windows.Forms.TextBox();
            this.F16_textBox = new System.Windows.Forms.TextBox();
            this.F17_textBox = new System.Windows.Forms.TextBox();
            this.F18_textBox = new System.Windows.Forms.TextBox();
            this.F19_textBox = new System.Windows.Forms.TextBox();
            this.F20_textBox = new System.Windows.Forms.TextBox();
            this.F21_textBox = new System.Windows.Forms.TextBox();
            this.F22_textBox = new System.Windows.Forms.TextBox();
            this.F23_textBox = new System.Windows.Forms.TextBox();
            this.F24_textBox = new System.Windows.Forms.TextBox();
            this.F25_textBox = new System.Windows.Forms.TextBox();
            this.E10_textBox = new System.Windows.Forms.TextBox();
            this.E11_textBox = new System.Windows.Forms.TextBox();
            this.E12_textBox = new System.Windows.Forms.TextBox();
            this.E13_textBox = new System.Windows.Forms.TextBox();
            this.F26_textBox = new System.Windows.Forms.TextBox();
            this.F27_textBox = new System.Windows.Forms.TextBox();
            this.F28_textBox = new System.Windows.Forms.TextBox();
            this.F29_textBox = new System.Windows.Forms.TextBox();
            this.F30_textBox = new System.Windows.Forms.TextBox();
            this.E14_textBox = new System.Windows.Forms.TextBox();
            this.E15_textBox = new System.Windows.Forms.TextBox();
            this.F31_textBox = new System.Windows.Forms.TextBox();
            this.F32_textBox = new System.Windows.Forms.TextBox();
            this.E16_textBox = new System.Windows.Forms.TextBox();
            this.D3_textBox = new System.Windows.Forms.TextBox();
            this.D8_textBox = new System.Windows.Forms.TextBox();
            this.C4_textBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // A1_textBox
            // 
            this.A1_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.A1_textBox.Location = new System.Drawing.Point(74, 400);
            this.A1_textBox.Name = "A1_textBox";
            this.A1_textBox.Size = new System.Drawing.Size(100, 20);
            this.A1_textBox.TabIndex = 0;
            // 
            // B1_textBox
            // 
            this.B1_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.B1_textBox.Location = new System.Drawing.Point(254, 200);
            this.B1_textBox.Name = "B1_textBox";
            this.B1_textBox.Size = new System.Drawing.Size(100, 20);
            this.B1_textBox.TabIndex = 1;
            // 
            // C3_textBox
            // 
            this.C3_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.C3_textBox.Location = new System.Drawing.Point(424, 500);
            this.C3_textBox.Name = "C3_textBox";
            this.C3_textBox.Size = new System.Drawing.Size(100, 20);
            this.C3_textBox.TabIndex = 2;
            // 
            // D6_textBox
            // 
            this.D6_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.D6_textBox.Location = new System.Drawing.Point(594, 550);
            this.D6_textBox.Name = "D6_textBox";
            this.D6_textBox.Size = new System.Drawing.Size(100, 20);
            this.D6_textBox.TabIndex = 3;
            // 
            // B2_textBox
            // 
            this.B2_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.B2_textBox.Location = new System.Drawing.Point(254, 600);
            this.B2_textBox.Name = "B2_textBox";
            this.B2_textBox.Size = new System.Drawing.Size(100, 20);
            this.B2_textBox.TabIndex = 4;
            // 
            // C2_textBox
            // 
            this.C2_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.C2_textBox.Location = new System.Drawing.Point(424, 300);
            this.C2_textBox.Name = "C2_textBox";
            this.C2_textBox.Size = new System.Drawing.Size(100, 20);
            this.C2_textBox.TabIndex = 5;
            // 
            // D1_textBox
            // 
            this.D1_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.D1_textBox.Location = new System.Drawing.Point(594, 50);
            this.D1_textBox.Name = "D1_textBox";
            this.D1_textBox.Size = new System.Drawing.Size(100, 20);
            this.D1_textBox.TabIndex = 6;
            // 
            // E1_textBox
            // 
            this.E1_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.E1_textBox.Location = new System.Drawing.Point(764, 23);
            this.E1_textBox.Name = "E1_textBox";
            this.E1_textBox.Size = new System.Drawing.Size(100, 20);
            this.E1_textBox.TabIndex = 7;
            // 
            // E2_textBox
            // 
            this.E2_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.E2_textBox.Location = new System.Drawing.Point(764, 73);
            this.E2_textBox.Name = "E2_textBox";
            this.E2_textBox.Size = new System.Drawing.Size(100, 20);
            this.E2_textBox.TabIndex = 8;
            // 
            // E3_textBox
            // 
            this.E3_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.E3_textBox.Location = new System.Drawing.Point(764, 123);
            this.E3_textBox.Name = "E3_textBox";
            this.E3_textBox.Size = new System.Drawing.Size(100, 20);
            this.E3_textBox.TabIndex = 9;
            // 
            // E4_textBox
            // 
            this.E4_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.E4_textBox.Location = new System.Drawing.Point(764, 173);
            this.E4_textBox.Name = "E4_textBox";
            this.E4_textBox.Size = new System.Drawing.Size(100, 20);
            this.E4_textBox.TabIndex = 10;
            // 
            // E5_textBox
            // 
            this.E5_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.E5_textBox.Location = new System.Drawing.Point(764, 223);
            this.E5_textBox.Name = "E5_textBox";
            this.E5_textBox.Size = new System.Drawing.Size(100, 20);
            this.E5_textBox.TabIndex = 11;
            // 
            // E6_textBox
            // 
            this.E6_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.E6_textBox.Location = new System.Drawing.Point(764, 273);
            this.E6_textBox.Name = "E6_textBox";
            this.E6_textBox.Size = new System.Drawing.Size(100, 20);
            this.E6_textBox.TabIndex = 12;
            // 
            // E7_textBox
            // 
            this.E7_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.E7_textBox.Location = new System.Drawing.Point(764, 323);
            this.E7_textBox.Name = "E7_textBox";
            this.E7_textBox.Size = new System.Drawing.Size(100, 20);
            this.E7_textBox.TabIndex = 13;
            // 
            // E8_textBox
            // 
            this.E8_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.E8_textBox.Location = new System.Drawing.Point(764, 373);
            this.E8_textBox.Name = "E8_textBox";
            this.E8_textBox.Size = new System.Drawing.Size(100, 20);
            this.E8_textBox.TabIndex = 14;
            // 
            // E9_textBox
            // 
            this.E9_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.E9_textBox.Location = new System.Drawing.Point(764, 423);
            this.E9_textBox.Name = "E9_textBox";
            this.E9_textBox.Size = new System.Drawing.Size(100, 20);
            this.E9_textBox.TabIndex = 15;
            // 
            // D2_textBox
            // 
            this.D2_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.D2_textBox.Location = new System.Drawing.Point(594, 150);
            this.D2_textBox.Name = "D2_textBox";
            this.D2_textBox.Size = new System.Drawing.Size(100, 20);
            this.D2_textBox.TabIndex = 16;
            // 
            // D4_textBox
            // 
            this.D4_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.D4_textBox.Location = new System.Drawing.Point(594, 350);
            this.D4_textBox.Name = "D4_textBox";
            this.D4_textBox.Size = new System.Drawing.Size(100, 20);
            this.D4_textBox.TabIndex = 17;
            // 
            // D5_textBox
            // 
            this.D5_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.D5_textBox.Location = new System.Drawing.Point(594, 450);
            this.D5_textBox.Name = "D5_textBox";
            this.D5_textBox.Size = new System.Drawing.Size(100, 20);
            this.D5_textBox.TabIndex = 18;
            // 
            // C1_textBox
            // 
            this.C1_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.C1_textBox.Location = new System.Drawing.Point(424, 100);
            this.C1_textBox.Name = "C1_textBox";
            this.C1_textBox.Size = new System.Drawing.Size(100, 20);
            this.C1_textBox.TabIndex = 19;
            // 
            // D7_textBox
            // 
            this.D7_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.D7_textBox.Location = new System.Drawing.Point(594, 650);
            this.D7_textBox.Name = "D7_textBox";
            this.D7_textBox.Size = new System.Drawing.Size(100, 20);
            this.D7_textBox.TabIndex = 20;
            // 
            // F1_textBox
            // 
            this.F1_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F1_textBox.Location = new System.Drawing.Point(934, 10);
            this.F1_textBox.Name = "F1_textBox";
            this.F1_textBox.Size = new System.Drawing.Size(100, 20);
            this.F1_textBox.TabIndex = 21;
            // 
            // F2_textBox
            // 
            this.F2_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F2_textBox.Location = new System.Drawing.Point(934, 35);
            this.F2_textBox.Name = "F2_textBox";
            this.F2_textBox.Size = new System.Drawing.Size(100, 20);
            this.F2_textBox.TabIndex = 22;
            // 
            // F3_textBox
            // 
            this.F3_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F3_textBox.Location = new System.Drawing.Point(934, 60);
            this.F3_textBox.Name = "F3_textBox";
            this.F3_textBox.Size = new System.Drawing.Size(100, 20);
            this.F3_textBox.TabIndex = 23;
            // 
            // F4_textBox
            // 
            this.F4_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F4_textBox.Location = new System.Drawing.Point(934, 85);
            this.F4_textBox.Name = "F4_textBox";
            this.F4_textBox.Size = new System.Drawing.Size(100, 20);
            this.F4_textBox.TabIndex = 24;
            // 
            // F5_textBox
            // 
            this.F5_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F5_textBox.Location = new System.Drawing.Point(934, 110);
            this.F5_textBox.Name = "F5_textBox";
            this.F5_textBox.Size = new System.Drawing.Size(100, 20);
            this.F5_textBox.TabIndex = 25;
            // 
            // F6_textBox
            // 
            this.F6_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F6_textBox.Location = new System.Drawing.Point(934, 135);
            this.F6_textBox.Name = "F6_textBox";
            this.F6_textBox.Size = new System.Drawing.Size(100, 20);
            this.F6_textBox.TabIndex = 26;
            // 
            // F7_textBox
            // 
            this.F7_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F7_textBox.Location = new System.Drawing.Point(934, 160);
            this.F7_textBox.Name = "F7_textBox";
            this.F7_textBox.Size = new System.Drawing.Size(100, 20);
            this.F7_textBox.TabIndex = 27;
            // 
            // F8_textBox
            // 
            this.F8_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F8_textBox.Location = new System.Drawing.Point(934, 185);
            this.F8_textBox.Name = "F8_textBox";
            this.F8_textBox.Size = new System.Drawing.Size(100, 20);
            this.F8_textBox.TabIndex = 28;
            // 
            // F9_textBox
            // 
            this.F9_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F9_textBox.Location = new System.Drawing.Point(934, 210);
            this.F9_textBox.Name = "F9_textBox";
            this.F9_textBox.Size = new System.Drawing.Size(100, 20);
            this.F9_textBox.TabIndex = 29;
            // 
            // F10_textBox
            // 
            this.F10_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F10_textBox.Location = new System.Drawing.Point(934, 235);
            this.F10_textBox.Name = "F10_textBox";
            this.F10_textBox.Size = new System.Drawing.Size(100, 20);
            this.F10_textBox.TabIndex = 30;
            // 
            // F11_textBox
            // 
            this.F11_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F11_textBox.Location = new System.Drawing.Point(934, 260);
            this.F11_textBox.Name = "F11_textBox";
            this.F11_textBox.Size = new System.Drawing.Size(100, 20);
            this.F11_textBox.TabIndex = 31;
            // 
            // F12_textBox
            // 
            this.F12_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F12_textBox.Location = new System.Drawing.Point(934, 285);
            this.F12_textBox.Name = "F12_textBox";
            this.F12_textBox.Size = new System.Drawing.Size(100, 20);
            this.F12_textBox.TabIndex = 32;
            // 
            // F13_textBox
            // 
            this.F13_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F13_textBox.Location = new System.Drawing.Point(934, 310);
            this.F13_textBox.Name = "F13_textBox";
            this.F13_textBox.Size = new System.Drawing.Size(100, 20);
            this.F13_textBox.TabIndex = 33;
            // 
            // F14_textBox
            // 
            this.F14_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F14_textBox.Location = new System.Drawing.Point(934, 335);
            this.F14_textBox.Name = "F14_textBox";
            this.F14_textBox.Size = new System.Drawing.Size(100, 20);
            this.F14_textBox.TabIndex = 34;
            // 
            // F15_textBox
            // 
            this.F15_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F15_textBox.Location = new System.Drawing.Point(934, 360);
            this.F15_textBox.Name = "F15_textBox";
            this.F15_textBox.Size = new System.Drawing.Size(100, 20);
            this.F15_textBox.TabIndex = 35;
            // 
            // F16_textBox
            // 
            this.F16_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F16_textBox.Location = new System.Drawing.Point(934, 385);
            this.F16_textBox.Name = "F16_textBox";
            this.F16_textBox.Size = new System.Drawing.Size(100, 20);
            this.F16_textBox.TabIndex = 36;
            // 
            // F17_textBox
            // 
            this.F17_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F17_textBox.Location = new System.Drawing.Point(934, 410);
            this.F17_textBox.Name = "F17_textBox";
            this.F17_textBox.Size = new System.Drawing.Size(100, 20);
            this.F17_textBox.TabIndex = 37;
            // 
            // F18_textBox
            // 
            this.F18_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F18_textBox.Location = new System.Drawing.Point(934, 435);
            this.F18_textBox.Name = "F18_textBox";
            this.F18_textBox.Size = new System.Drawing.Size(100, 20);
            this.F18_textBox.TabIndex = 38;
            // 
            // F19_textBox
            // 
            this.F19_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F19_textBox.Location = new System.Drawing.Point(934, 460);
            this.F19_textBox.Name = "F19_textBox";
            this.F19_textBox.Size = new System.Drawing.Size(100, 20);
            this.F19_textBox.TabIndex = 39;
            // 
            // F20_textBox
            // 
            this.F20_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F20_textBox.Location = new System.Drawing.Point(934, 485);
            this.F20_textBox.Name = "F20_textBox";
            this.F20_textBox.Size = new System.Drawing.Size(100, 20);
            this.F20_textBox.TabIndex = 40;
            // 
            // F21_textBox
            // 
            this.F21_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F21_textBox.Location = new System.Drawing.Point(934, 510);
            this.F21_textBox.Name = "F21_textBox";
            this.F21_textBox.Size = new System.Drawing.Size(100, 20);
            this.F21_textBox.TabIndex = 41;
            // 
            // F22_textBox
            // 
            this.F22_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F22_textBox.Location = new System.Drawing.Point(934, 535);
            this.F22_textBox.Name = "F22_textBox";
            this.F22_textBox.Size = new System.Drawing.Size(100, 20);
            this.F22_textBox.TabIndex = 42;
            // 
            // F23_textBox
            // 
            this.F23_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F23_textBox.Location = new System.Drawing.Point(934, 560);
            this.F23_textBox.Name = "F23_textBox";
            this.F23_textBox.Size = new System.Drawing.Size(100, 20);
            this.F23_textBox.TabIndex = 43;
            // 
            // F24_textBox
            // 
            this.F24_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F24_textBox.Location = new System.Drawing.Point(934, 585);
            this.F24_textBox.Name = "F24_textBox";
            this.F24_textBox.Size = new System.Drawing.Size(100, 20);
            this.F24_textBox.TabIndex = 44;
            // 
            // F25_textBox
            // 
            this.F25_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F25_textBox.Location = new System.Drawing.Point(934, 610);
            this.F25_textBox.Name = "F25_textBox";
            this.F25_textBox.Size = new System.Drawing.Size(100, 20);
            this.F25_textBox.TabIndex = 45;
            // 
            // E10_textBox
            // 
            this.E10_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.E10_textBox.Location = new System.Drawing.Point(764, 473);
            this.E10_textBox.Name = "E10_textBox";
            this.E10_textBox.Size = new System.Drawing.Size(100, 20);
            this.E10_textBox.TabIndex = 46;
            // 
            // E11_textBox
            // 
            this.E11_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.E11_textBox.Location = new System.Drawing.Point(764, 523);
            this.E11_textBox.Name = "E11_textBox";
            this.E11_textBox.Size = new System.Drawing.Size(100, 20);
            this.E11_textBox.TabIndex = 47;
            this.E11_textBox.TextChanged += new System.EventHandler(this.textBox45_TextChanged);
            // 
            // E12_textBox
            // 
            this.E12_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.E12_textBox.Location = new System.Drawing.Point(764, 573);
            this.E12_textBox.Name = "E12_textBox";
            this.E12_textBox.Size = new System.Drawing.Size(100, 20);
            this.E12_textBox.TabIndex = 48;
            // 
            // E13_textBox
            // 
            this.E13_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.E13_textBox.Location = new System.Drawing.Point(764, 623);
            this.E13_textBox.Name = "E13_textBox";
            this.E13_textBox.Size = new System.Drawing.Size(100, 20);
            this.E13_textBox.TabIndex = 49;
            // 
            // F26_textBox
            // 
            this.F26_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F26_textBox.Location = new System.Drawing.Point(934, 635);
            this.F26_textBox.Name = "F26_textBox";
            this.F26_textBox.Size = new System.Drawing.Size(100, 20);
            this.F26_textBox.TabIndex = 50;
            // 
            // F27_textBox
            // 
            this.F27_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F27_textBox.Location = new System.Drawing.Point(934, 660);
            this.F27_textBox.Name = "F27_textBox";
            this.F27_textBox.Size = new System.Drawing.Size(100, 20);
            this.F27_textBox.TabIndex = 51;
            // 
            // F28_textBox
            // 
            this.F28_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F28_textBox.Location = new System.Drawing.Point(934, 685);
            this.F28_textBox.Name = "F28_textBox";
            this.F28_textBox.Size = new System.Drawing.Size(100, 20);
            this.F28_textBox.TabIndex = 52;
            // 
            // F29_textBox
            // 
            this.F29_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F29_textBox.Location = new System.Drawing.Point(934, 710);
            this.F29_textBox.Name = "F29_textBox";
            this.F29_textBox.Size = new System.Drawing.Size(100, 20);
            this.F29_textBox.TabIndex = 53;
            // 
            // F30_textBox
            // 
            this.F30_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F30_textBox.Location = new System.Drawing.Point(934, 735);
            this.F30_textBox.Name = "F30_textBox";
            this.F30_textBox.Size = new System.Drawing.Size(100, 20);
            this.F30_textBox.TabIndex = 54;
            // 
            // E14_textBox
            // 
            this.E14_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.E14_textBox.Location = new System.Drawing.Point(764, 673);
            this.E14_textBox.Name = "E14_textBox";
            this.E14_textBox.Size = new System.Drawing.Size(100, 20);
            this.E14_textBox.TabIndex = 55;
            // 
            // E15_textBox
            // 
            this.E15_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.E15_textBox.Location = new System.Drawing.Point(764, 723);
            this.E15_textBox.Name = "E15_textBox";
            this.E15_textBox.Size = new System.Drawing.Size(100, 20);
            this.E15_textBox.TabIndex = 56;
            // 
            // F31_textBox
            // 
            this.F31_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F31_textBox.Location = new System.Drawing.Point(934, 760);
            this.F31_textBox.Name = "F31_textBox";
            this.F31_textBox.Size = new System.Drawing.Size(100, 20);
            this.F31_textBox.TabIndex = 57;
            // 
            // F32_textBox
            // 
            this.F32_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.F32_textBox.Location = new System.Drawing.Point(934, 785);
            this.F32_textBox.Name = "F32_textBox";
            this.F32_textBox.Size = new System.Drawing.Size(100, 20);
            this.F32_textBox.TabIndex = 58;
            // 
            // E16_textBox
            // 
            this.E16_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.E16_textBox.Location = new System.Drawing.Point(764, 773);
            this.E16_textBox.Name = "E16_textBox";
            this.E16_textBox.Size = new System.Drawing.Size(100, 20);
            this.E16_textBox.TabIndex = 59;
            // 
            // D3_textBox
            // 
            this.D3_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.D3_textBox.Location = new System.Drawing.Point(594, 250);
            this.D3_textBox.Name = "D3_textBox";
            this.D3_textBox.Size = new System.Drawing.Size(100, 20);
            this.D3_textBox.TabIndex = 60;
            // 
            // D8_textBox
            // 
            this.D8_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.D8_textBox.Location = new System.Drawing.Point(594, 745);
            this.D8_textBox.Name = "D8_textBox";
            this.D8_textBox.Size = new System.Drawing.Size(100, 20);
            this.D8_textBox.TabIndex = 61;
            // 
            // C4_textBox
            // 
            this.C4_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.C4_textBox.Location = new System.Drawing.Point(424, 700);
            this.C4_textBox.Name = "C4_textBox";
            this.C4_textBox.Size = new System.Drawing.Size(100, 20);
            this.C4_textBox.TabIndex = 62;
            // 
            // FFamilyTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1128, 810);
            this.Controls.Add(this.C4_textBox);
            this.Controls.Add(this.D8_textBox);
            this.Controls.Add(this.D3_textBox);
            this.Controls.Add(this.E16_textBox);
            this.Controls.Add(this.F32_textBox);
            this.Controls.Add(this.F31_textBox);
            this.Controls.Add(this.E15_textBox);
            this.Controls.Add(this.E14_textBox);
            this.Controls.Add(this.F30_textBox);
            this.Controls.Add(this.F29_textBox);
            this.Controls.Add(this.F28_textBox);
            this.Controls.Add(this.F27_textBox);
            this.Controls.Add(this.F26_textBox);
            this.Controls.Add(this.E13_textBox);
            this.Controls.Add(this.E12_textBox);
            this.Controls.Add(this.E11_textBox);
            this.Controls.Add(this.E10_textBox);
            this.Controls.Add(this.F25_textBox);
            this.Controls.Add(this.F24_textBox);
            this.Controls.Add(this.F23_textBox);
            this.Controls.Add(this.F22_textBox);
            this.Controls.Add(this.F21_textBox);
            this.Controls.Add(this.F20_textBox);
            this.Controls.Add(this.F19_textBox);
            this.Controls.Add(this.F18_textBox);
            this.Controls.Add(this.F17_textBox);
            this.Controls.Add(this.F16_textBox);
            this.Controls.Add(this.F15_textBox);
            this.Controls.Add(this.F14_textBox);
            this.Controls.Add(this.F13_textBox);
            this.Controls.Add(this.F12_textBox);
            this.Controls.Add(this.F11_textBox);
            this.Controls.Add(this.F10_textBox);
            this.Controls.Add(this.F9_textBox);
            this.Controls.Add(this.F8_textBox);
            this.Controls.Add(this.F7_textBox);
            this.Controls.Add(this.F6_textBox);
            this.Controls.Add(this.F5_textBox);
            this.Controls.Add(this.F4_textBox);
            this.Controls.Add(this.F3_textBox);
            this.Controls.Add(this.F2_textBox);
            this.Controls.Add(this.F1_textBox);
            this.Controls.Add(this.D7_textBox);
            this.Controls.Add(this.C1_textBox);
            this.Controls.Add(this.D5_textBox);
            this.Controls.Add(this.D4_textBox);
            this.Controls.Add(this.D2_textBox);
            this.Controls.Add(this.E9_textBox);
            this.Controls.Add(this.E8_textBox);
            this.Controls.Add(this.E7_textBox);
            this.Controls.Add(this.E6_textBox);
            this.Controls.Add(this.E5_textBox);
            this.Controls.Add(this.E4_textBox);
            this.Controls.Add(this.E3_textBox);
            this.Controls.Add(this.E2_textBox);
            this.Controls.Add(this.E1_textBox);
            this.Controls.Add(this.D1_textBox);
            this.Controls.Add(this.C2_textBox);
            this.Controls.Add(this.B2_textBox);
            this.Controls.Add(this.D6_textBox);
            this.Controls.Add(this.C3_textBox);
            this.Controls.Add(this.B1_textBox);
            this.Controls.Add(this.A1_textBox);
            this.Location = new System.Drawing.Point(50, 20);
            this.Name = "FFamilyTree";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FFamilyTree";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void textBox45_TextChanged(object sender, EventArgs e)
        {

        }
    }
}