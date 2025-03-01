using System;
using System.Windows.Forms;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    public class CMain
    {
        //****************************************************************************************************************************
        public CMain() { }
        //****************************************************************************************************************************
        [STAThread]
        static void Main()
        {
//            try
//            {
                FHistoricJamaica HJ = new FHistoricJamaica();
                HJ.ShowDialog();
//                Form3 Form11 = new Form3();
//                Form11.ShowDialog();
//            }
//            catch (HistoricJamaicaException e)
//            {
//                UU.ShowErrorMessage(e);
//            }
//            catch (Exception e)
//            {
//                MessageBox.Show("Fatal Error: " + e.Message);
//            }
        }
    }
}
