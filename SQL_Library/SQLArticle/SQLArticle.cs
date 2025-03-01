using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SQL_Library
{
    public partial class SQL
    {
        //****************************************************************************************************************************
        public static string ArticleInsertString(string sParmChar)
        {
            return "(" +
                   sParmChar + U.Article_col + U.comma +
                   sParmChar + U.ArticleContinueID_col +
                   ")";
        }
        //****************************************************************************************************************************
        public static DataTable DefineArticleTable()
        {
            DataTable tbl = new DataTable(U.Articles_Table);
            tbl.Columns.Add(U.ArticleID_col, typeof(int));
            tbl.Columns.Add(U.Article_col, typeof(string));
            tbl.Columns.Add(U.ArticleContinueID_col, typeof(int));
            tbl.Columns[U.Article_col].MaxLength = U.iMaxArticleLength;
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetAllArticles()
        {
            DataTable article_tbl = new DataTable();
            SelectAll(U.Articles_Table, U.NoOrderBy, article_tbl);
            return article_tbl;
        }
        //****************************************************************************************************************************
        public static string GetArticle(int iArticleID)
        {
            DataTable tbl = new DataTable(U.Articles_Table);
            SelectAll(U.Articles_Table, tbl, new NameValuePair(U.ArticleID_col, iArticleID));
            if (tbl.Rows.Count == 0)     // does not exist
                return "";
            else
            {
                DataRow row = tbl.Rows[0];
                return row[U.Article_col].ToString();
            }
        }
        //****************************************************************************************************************************
        public static int InsertArticle(string sArticle)
        {
            DataTable Article_tbl = DefineArticleTable();
            DataRow Article_row = Article_tbl.NewRow();
            Article_row[U.Article_col] = sArticle;
            Article_row[U.ArticleContinueID_col] = 0;
            Article_tbl.Rows.Add(Article_row);
            SqlCommand insertCommand = InsertCommand(Article_tbl, U.Articles_Table, true);
            if (InsertWithDA(Article_tbl, insertCommand))
                return Article_tbl.Rows[0][U.ArticleID_col].ToInt();
            else
                return 0;
        }
        //****************************************************************************************************************************
        public static void UpdateArticle(int iArticleID,
                                  string sArticle)
        {
            string updateString = UpdateString(U.Articles_Table) + " Set " + ColumnEquals(U.Article_col) +
                                 " where " + ColumnEquals(U.ArticleID_col);
            SqlCommand cmd = new SqlCommand(updateString, sqlConnection);
            cmd.Parameters.Add(new SqlParameter("@" + U.ArticleID_col, iArticleID));
            cmd.Parameters.Add(new SqlParameter("@" + U.Article_col, sArticle));
            ExecuteUpdateStatement(cmd);
        }
        //****************************************************************************************************************************
    }
}
