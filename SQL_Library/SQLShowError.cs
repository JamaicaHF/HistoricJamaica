using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Data;

namespace SQL_Library
{
    public static partial class errors
    {
        public static void SQLShowError(string message)
        {
            MessageBox.Show(message);
        }
    }
}
