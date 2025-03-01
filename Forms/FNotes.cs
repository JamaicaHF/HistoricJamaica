using System;
using System.Drawing;
using System.Windows;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    public class FNotes : Form
    {
        private int m_iBuildingItem = 9999;
        private RichTextBox Notes_richTextBox;
    
        public int BuildingItem { get { return m_iBuildingItem; } }
        private System.ComponentModel.IContainer components = null;
        private CSql m_SQL;
        private int m_iID;
        private int m_iBuildingType;
        private int m_iBuildingID;
        private string m_sNotes = "";
        private int m_iArticleID = -1;
        private string m_OriginalArticle;
        //****************************************************************************************************************************
        public string sNotes
        {
            get 
            {
                return BuildingNotes.GetNotesWithCensusInfo(m_iBuildingType, m_iID, m_iBuildingID);
            }
        }
        //****************************************************************************************************************************
        public int ArticleID
        {
            get { return m_iArticleID; }
        }
        //****************************************************************************************************************************
        public FNotes(CSql Sql,
                             int iBuildingType,
                             int iID,
                             int iBuildingID,
                             int iLocationX,
                             int iLocationY,
                             int iSelectedRow)
        {
            m_SQL = Sql;
            m_iBuildingType = iBuildingType;
            m_iID = iID;
            m_iBuildingID = iBuildingID;
            InitializeComponent();
            int iScreenHeight = Screen.PrimaryScreen.Bounds.Height;
            int iScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            int iNewLocationX = iLocationX + 150;
            int iNewLocationY = iLocationY + (iSelectedRow * 22);
            int iMaxLocationY = iScreenHeight - 192;
            if (iMaxLocationY < iNewLocationY)
                iNewLocationY = iMaxLocationY;
            this.Location = new System.Drawing.Point(iNewLocationX, iNewLocationY);
            m_OriginalArticle = BuildingNotes.GetNotes(m_iBuildingType, m_iID, m_iBuildingID);
            Notes_richTextBox.Text = m_OriginalArticle;
        }
        //****************************************************************************************************************************
        public FNotes(CSql cSQL,
                      int iArticleID,
                      int iLocationX,
                      int iLocationY,
                      int iSelectedRow)
        {
            m_SQL = cSQL;
            m_iArticleID = iArticleID;
            InitializeComponent();
            int iScreenHeight = Screen.PrimaryScreen.Bounds.Height;
            int iScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            int iNewLocationX = iLocationX + 150;
            int iNewLocationY = iLocationY + (iSelectedRow * 22);
            int iMaxLocationY = iScreenHeight - 192;
            if (iMaxLocationY < iNewLocationY)
                iNewLocationY = iMaxLocationY;
            this.Location = new System.Drawing.Point(iNewLocationX, iNewLocationY);
            m_OriginalArticle = SQL.GetArticle(m_iArticleID);
            Notes_richTextBox.Text = m_OriginalArticle;
        }
        //****************************************************************************************************************************
        public FNotes(CSql cSQL,
                      string str,
                      int iLocationX,
                      int iLocationY,
                      int iSelectedRow)
        {
            m_SQL = cSQL;
            InitializeComponent();
            int iScreenHeight = Screen.PrimaryScreen.Bounds.Height;
            int iScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            int iNewLocationX = iLocationX + 150;
            int iNewLocationY = iLocationY + (iSelectedRow * 22);
            int iMaxLocationY = iScreenHeight - 192;
            if (iMaxLocationY < iNewLocationY)
                iNewLocationY = iMaxLocationY;
            this.Location = new System.Drawing.Point(iNewLocationX, iNewLocationY);
            Notes_richTextBox.Text = str;
        }
        //****************************************************************************************************************************
        public string GetText()
        {
            return Notes_richTextBox.Text.Trim();
        }
        //****************************************************************************************************************************
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (m_iArticleID < 0) // notes, not an article
            {
                m_sNotes = Notes_richTextBox.Text.ToString().Trim();
                if (m_sNotes != m_OriginalArticle)
                {
                    SQL.SaveNotes(m_iBuildingType, m_iID, m_iBuildingID, m_sNotes);
                }
            }
            else
            {
                m_sNotes = Notes_richTextBox.Text.ToString().Trim();
                if (m_sNotes.Length == 0)
                {
                    if (m_iArticleID != 0)
                    {
                        SQL.DeleteWithParms(U.Articles_Table, new NameValuePair(U.ArticleID_col, m_iArticleID));
                    }
                    m_iArticleID = 0;
                }
                else if (m_iArticleID == 0)
                {
                    m_iArticleID = SQL.InsertArticle(m_sNotes);
                }
                else
                {
                    SQL.UpdateArticle(m_iArticleID, m_sNotes);
                }
            }
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
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.Notes_richTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // Notes_richTextBox
            // 
            this.Notes_richTextBox.Location = new System.Drawing.Point(0, 0);
            this.Notes_richTextBox.Name = "Notes_richTextBox";
            this.Notes_richTextBox.Size = new System.Drawing.Size(809, 153);
            this.Notes_richTextBox.TabIndex = 0;
            this.Notes_richTextBox.Text = "";
            // 
            // FNotes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 151);
            this.Controls.Add(this.Notes_richTextBox);
            this.Name = "FNotes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.ResumeLayout(false);

        }
        #endregion
    }
}
