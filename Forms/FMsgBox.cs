using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HistoricJamaica
{
    public partial class FMsgBox : Form
    {
        public enum Yesno
        {
            yes,
            no
        }
        public Yesno yesno = Yesno.no;
        public FMsgBox(string msg1, string msg2, string msg3, string msg4, string msg5)
        {
            InitializeComponent();
            this.Msg1.Text = msg1;
            this.Msg2.Text = msg2;
            this.Msg3.Text = msg3;
            this.Msg4.Text = msg4;
            this.Msg5.Text = msg5;
        }
        //****************************************************************************************************************************
        private void button1_Click(object sender, System.EventArgs e)
        {
            yesno = Yesno.yes;
            this.Close();
        }
        //****************************************************************************************************************************
        private void button2_Click(object sender, System.EventArgs e)
        {
            yesno = Yesno.no;
            this.Close();
        }
    }
}
