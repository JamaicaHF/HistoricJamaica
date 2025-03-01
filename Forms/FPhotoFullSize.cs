using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SQL_Library;

namespace HistoricJamaica
{
    public enum FullSizeMode
    {
        Zoom_Mode,
        Cemetery_Mode
    }
    public partial class FPhotoFullSize : FPaintPhoto
    {
//        public static extern bool GetCursorPos(int X, int Y);
        private Bitmap m_Photo;
        private Bitmap m_ZoomPhoto = null;
        private bool m_bZoom = false;
        private int m_iMaxScreenHeight = 800;
        private int m_iMaxScreenWidth = 1190;
        private int m_iScreenHeight;
        private int m_iScreenWidth;
        private int m_iZoomHeight;
        private int m_iZoomWidth;
        private int m_iPhotoCursorX = 0;
        private int m_iPhotoCursorY = 0;
        private Button FullSize_button;
        private int m_iZoomLevel = 5;
        private Button SouthHill_button;
        private Button SageHill_button;
        private Button SeventhDayAdventist_button;
        private Button Robbins_button;
        private Button Village_button;
        private Button Rawsonville_button;
        private Button SouthWinham_button;
        private Button PikesFalls_button;
        private Button WestJamaica_button;
        private Button EastJamaica_button;
        private Button AllCemeteries_button;
        private Button NewRecord_button;
        private FullSizeMode m_eFullSizeMode;
        private bool m_bPhotoAlreadyDrawn = true;
        private CSql m_SQL = null;
        private Button CivalWar_button;
        private System.ComponentModel.IContainer components = null;
        //****************************************************************************************************************************
        public FPhotoFullSize(Bitmap HFPhoto)
        {
            m_Photo = HFPhoto;
            m_eFullSizeMode = FullSizeMode.Zoom_Mode;
            InitializeFullSizePhoto();
            InitializeCemeteryButtons(false);
        }
        //****************************************************************************************************************************
        public FPhotoFullSize(Bitmap HFPhoto,
                                CSql   SQL)
        {
            m_Photo = HFPhoto;
            m_SQL = SQL;
            m_eFullSizeMode = FullSizeMode.Cemetery_Mode;
            InitializeFullSizePhoto();
            InitializeCemeteryButtons(true);
            SouthHill_button.Location = new System.Drawing.Point(m_iScreenWidth + 4, 20);
            SageHill_button.Location = new System.Drawing.Point(m_iScreenWidth + 4, 70);
            SeventhDayAdventist_button.Location = new System.Drawing.Point(m_iScreenWidth + 4, 120);
            Robbins_button.Location = new System.Drawing.Point(m_iScreenWidth + 4, 170);
            Village_button.Location = new System.Drawing.Point(m_iScreenWidth + 4, 220);
            Rawsonville_button.Location = new System.Drawing.Point(m_iScreenWidth + 4, 270);
            SouthWinham_button.Location = new System.Drawing.Point(m_iScreenWidth + 4, 320);
            PikesFalls_button.Location = new System.Drawing.Point(m_iScreenWidth + 4, 370);
            WestJamaica_button.Location = new System.Drawing.Point(m_iScreenWidth + 4, 420);
            EastJamaica_button.Location = new System.Drawing.Point(m_iScreenWidth + 4, 470);
            AllCemeteries_button.Location = new System.Drawing.Point(m_iScreenWidth + 4, 570);
            NewRecord_button.Location = new System.Drawing.Point(m_iScreenWidth + 4, 620);
            CivalWar_button.Location = new System.Drawing.Point(m_iScreenWidth + 4, 700);
        }
        //****************************************************************************************************************************
        private void InitializeFullSizePhoto()
        {
            m_iMaxScreenHeight = Screen.PrimaryScreen.Bounds.Height - 50;
            m_iMaxScreenWidth = Screen.PrimaryScreen.Bounds.Width - 90;
            m_iScreenHeight = m_iMaxScreenHeight;
            m_iScreenWidth = m_iMaxScreenWidth;
            m_iZoomHeight = m_iMaxScreenHeight;
            m_iZoomWidth = m_iMaxScreenWidth;
            U.ProportionalCoordinates(m_iMaxScreenHeight, m_iMaxScreenWidth, m_Photo.Height, m_Photo.Width,
                                    ref m_iScreenHeight, ref m_iScreenWidth);
            m_iPhotoCursorX = m_Photo.Width / 2;
            m_iPhotoCursorY = m_Photo.Height / 2;
            SetZoomPhoto(m_iZoomLevel);
            InitializeComponent();
            this.FullSize_button.Location = new System.Drawing.Point(m_iScreenWidth + 4, 652);
            m_bZoom = false;
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
        private int NewLocation(ref int iPhotoCursor,
                                int iZoomSize,
                                int iMaxSize)
        {
            int iLocation = iPhotoCursor - (iZoomSize / 2);
            if (iLocation < 0)
            {
                iPhotoCursor = iZoomSize / 2;
                return 0;
            }
            else if (iLocation + iZoomSize > iMaxSize)
            {
                iPhotoCursor = iMaxSize - (iZoomSize / 2);
                return iMaxSize - iZoomSize;
            }
            else
                return iLocation;
        }
        //****************************************************************************************************************************
        private void ZoomImage(PaintEventArgs myArgs)
        {
            Graphics g = Graphics.FromImage(m_ZoomPhoto);int iNewLocationX = NewLocation(ref m_iPhotoCursorX, m_iZoomWidth, m_Photo.Width);
            int iNewLocationY = NewLocation(ref m_iPhotoCursorY, m_iZoomHeight, m_Photo.Height);
            Rectangle srcRect = new Rectangle(iNewLocationX, iNewLocationY, m_iZoomWidth, m_iZoomHeight);
            Rectangle dstRect = new Rectangle(0, 0, m_ZoomPhoto.Width, m_ZoomPhoto.Height);
            g.DrawImage(m_Photo, dstRect, srcRect, GraphicsUnit.Pixel);
        }
        //****************************************************************************************************************************
        protected override void OnPaint(PaintEventArgs myArgs)
        {
            if (m_Photo != null && !m_bPhotoAlreadyDrawn)
            {
                if (m_bZoom)
                {
                    ZoomImage(myArgs);
                    PaintPhoto(myArgs, m_ZoomPhoto, m_iScreenHeight, m_iScreenWidth, 0);
                }
                else
                    PaintPhoto(myArgs, m_Photo, m_iScreenHeight, m_iScreenWidth, 0);
            }
            m_bPhotoAlreadyDrawn = false;
        }
        //****************************************************************************************************************************
        private void InitializeCemeteryButtons(bool bValue)
        {
            this.FullSize_button.Visible = !bValue;
            this.SouthHill_button.Visible = bValue;
            this.SageHill_button.Visible = bValue;
            this.SeventhDayAdventist_button.Visible = bValue;
            this.Robbins_button.Visible = bValue;
            this.Village_button.Visible = bValue;
            this.Rawsonville_button.Visible = bValue;
            this.SouthWinham_button.Visible = bValue;
            this.PikesFalls_button.Visible = bValue;
            this.WestJamaica_button.Visible = bValue;
            this.EastJamaica_button.Visible = bValue;
            this.AllCemeteries_button.Visible = bValue;
            this.NewRecord_button.Visible = bValue;
            this.CivalWar_button.Visible = bValue;
        }
        //****************************************************************************************************************************
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.FullSize_button = new System.Windows.Forms.Button();
            this.SouthHill_button = new System.Windows.Forms.Button();
            this.SageHill_button = new System.Windows.Forms.Button();
            this.SeventhDayAdventist_button = new System.Windows.Forms.Button();
            this.Robbins_button = new System.Windows.Forms.Button();
            this.Village_button = new System.Windows.Forms.Button();
            this.Rawsonville_button = new System.Windows.Forms.Button();
            this.SouthWinham_button = new System.Windows.Forms.Button();
            this.PikesFalls_button = new System.Windows.Forms.Button();
            this.WestJamaica_button = new System.Windows.Forms.Button();
            this.EastJamaica_button = new System.Windows.Forms.Button();
            this.AllCemeteries_button = new System.Windows.Forms.Button();
            this.NewRecord_button = new System.Windows.Forms.Button();
            this.CivalWar_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // FullSize_button
            // 
            this.FullSize_button.Location = new System.Drawing.Point(1195, 652);
            this.FullSize_button.Name = "FullSize_button";
            this.FullSize_button.Size = new System.Drawing.Size(75, 23);
            this.FullSize_button.TabIndex = 0;
            this.FullSize_button.Text = "Zoom";
            this.FullSize_button.UseVisualStyleBackColor = true;
            this.FullSize_button.Click += new System.EventHandler(this.ZoomButton_Click);
            // 
            // SouthHill_button
            // 
            this.SouthHill_button.Location = new System.Drawing.Point(1075, 21);
            this.SouthHill_button.Name = "SouthHill_button";
            this.SouthHill_button.Size = new System.Drawing.Size(89, 22);
            this.SouthHill_button.TabIndex = 1;
            this.SouthHill_button.Text = "South Hill";
            this.SouthHill_button.UseVisualStyleBackColor = true;
            this.SouthHill_button.Click += new System.EventHandler(this.SouthHill_button_Click);
            // 
            // SageHill_button
            // 
            this.SageHill_button.Location = new System.Drawing.Point(1075, 61);
            this.SageHill_button.Name = "SageHill_button";
            this.SageHill_button.Size = new System.Drawing.Size(89, 22);
            this.SageHill_button.TabIndex = 2;
            this.SageHill_button.Text = "Sage Hill";
            this.SageHill_button.UseVisualStyleBackColor = true;
            this.SageHill_button.Click += new System.EventHandler(this.SageHill_button_Button_Click);
            // 
            // SeventhDayAdventist_button
            // 
            this.SeventhDayAdventist_button.Location = new System.Drawing.Point(1075, 101);
            this.SeventhDayAdventist_button.Name = "SeventhDayAdventist_button";
            this.SeventhDayAdventist_button.Size = new System.Drawing.Size(89, 22);
            this.SeventhDayAdventist_button.TabIndex = 3;
            this.SeventhDayAdventist_button.Text = "7th Day ";
            this.SeventhDayAdventist_button.UseVisualStyleBackColor = true;
            this.SeventhDayAdventist_button.Click += new System.EventHandler(this.SeventhDayAdventist_button_Click);
            // 
            // Robbins_button
            // 
            this.Robbins_button.Location = new System.Drawing.Point(1075, 141);
            this.Robbins_button.Name = "Robbins_button";
            this.Robbins_button.Size = new System.Drawing.Size(89, 22);
            this.Robbins_button.TabIndex = 5;
            this.Robbins_button.Text = "Robbins";
            this.Robbins_button.UseVisualStyleBackColor = true;
            this.Robbins_button.Click += new System.EventHandler(this.Robbins_button_Click);
            // 
            // Village_button
            // 
            this.Village_button.Location = new System.Drawing.Point(1075, 181);
            this.Village_button.Name = "Village_button";
            this.Village_button.Size = new System.Drawing.Size(89, 22);
            this.Village_button.TabIndex = 9;
            this.Village_button.Text = "Village";
            this.Village_button.UseVisualStyleBackColor = true;
            this.Village_button.Click += new System.EventHandler(this.Village_button_Click);
            // 
            // Rawsonville_button
            // 
            this.Rawsonville_button.Location = new System.Drawing.Point(1075, 221);
            this.Rawsonville_button.Name = "Rawsonville_button";
            this.Rawsonville_button.Size = new System.Drawing.Size(89, 22);
            this.Rawsonville_button.TabIndex = 10;
            this.Rawsonville_button.Text = "Rawsonville";
            this.Rawsonville_button.UseVisualStyleBackColor = true;
            this.Rawsonville_button.Click += new System.EventHandler(this.Rawsonville_button_Click);
            // 
            // SouthWinham_button
            // 
            this.SouthWinham_button.Location = new System.Drawing.Point(1075, 261);
            this.SouthWinham_button.Name = "SouthWinham_button";
            this.SouthWinham_button.Size = new System.Drawing.Size(89, 22);
            this.SouthWinham_button.TabIndex = 11;
            this.SouthWinham_button.Text = "South Winham";
            this.SouthWinham_button.UseVisualStyleBackColor = true;
            this.SouthWinham_button.Click += new System.EventHandler(this.SouthWinham_button_Click);
            // 
            // PikesFalls_button
            // 
            this.PikesFalls_button.Location = new System.Drawing.Point(1075, 301);
            this.PikesFalls_button.Name = "PikesFalls_button";
            this.PikesFalls_button.Size = new System.Drawing.Size(89, 22);
            this.PikesFalls_button.TabIndex = 13;
            this.PikesFalls_button.Text = "Pikes Falls";
            this.PikesFalls_button.UseVisualStyleBackColor = true;
            this.PikesFalls_button.Click += new System.EventHandler(this.PikesFalls_button_Click);
            // 
            // WestJamaica_button
            // 
            this.WestJamaica_button.Location = new System.Drawing.Point(1075, 341);
            this.WestJamaica_button.Name = "WestJamaica_button";
            this.WestJamaica_button.Size = new System.Drawing.Size(89, 22);
            this.WestJamaica_button.TabIndex = 15;
            this.WestJamaica_button.Text = "West Jamaica";
            this.WestJamaica_button.UseVisualStyleBackColor = true;
            this.WestJamaica_button.Click += new System.EventHandler(this.WestJamaica_button_Click);
            // 
            // EastJamaica_button
            // 
            this.EastJamaica_button.Location = new System.Drawing.Point(1075, 381);
            this.EastJamaica_button.Name = "EastJamaica_button";
            this.EastJamaica_button.Size = new System.Drawing.Size(89, 22);
            this.EastJamaica_button.TabIndex = 16;
            this.EastJamaica_button.Text = "East Jamaica";
            this.EastJamaica_button.UseVisualStyleBackColor = true;
            this.EastJamaica_button.Click += new System.EventHandler(this.EastJamaica_button_Click);
            // 
            // AllCemeteries_button
            // 
            this.AllCemeteries_button.Location = new System.Drawing.Point(1075, 507);
            this.AllCemeteries_button.Name = "AllCemeteries_button";
            this.AllCemeteries_button.Size = new System.Drawing.Size(89, 22);
            this.AllCemeteries_button.TabIndex = 17;
            this.AllCemeteries_button.Text = "All Cemeteries";
            this.AllCemeteries_button.UseVisualStyleBackColor = true;
            this.AllCemeteries_button.Click += new System.EventHandler(this.AllCemeteries_button_Click);
            // 
            // NewRecord_button
            // 
            this.NewRecord_button.Location = new System.Drawing.Point(1075, 551);
            this.NewRecord_button.Name = "NewRecord_button";
            this.NewRecord_button.Size = new System.Drawing.Size(89, 22);
            this.NewRecord_button.TabIndex = 18;
            this.NewRecord_button.Text = "New Record";
            this.NewRecord_button.UseVisualStyleBackColor = true;
            this.NewRecord_button.Click += new System.EventHandler(this.NewCemetery_button_Click);
            // 
            // CivalWar_button
            // 
            this.CivalWar_button.Location = new System.Drawing.Point(1075, 618);
            this.CivalWar_button.Name = "CivalWar_button";
            this.CivalWar_button.Size = new System.Drawing.Size(89, 22);
            this.CivalWar_button.TabIndex = 19;
            this.CivalWar_button.Text = "Cival War";
            this.CivalWar_button.UseVisualStyleBackColor = true;
            this.CivalWar_button.Click += new System.EventHandler(this.AllPersonCW_button_Click);
            // 
            // FPhotoFullSize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1284, 670);
            this.Controls.Add(this.CivalWar_button);
            this.Controls.Add(this.NewRecord_button);
            this.Controls.Add(this.AllCemeteries_button);
            this.Controls.Add(this.EastJamaica_button);
            this.Controls.Add(this.WestJamaica_button);
            this.Controls.Add(this.PikesFalls_button);
            this.Controls.Add(this.SouthWinham_button);
            this.Controls.Add(this.Rawsonville_button);
            this.Controls.Add(this.Village_button);
            this.Controls.Add(this.Robbins_button);
            this.Controls.Add(this.SeventhDayAdventist_button);
            this.Controls.Add(this.SageHill_button);
            this.Controls.Add(this.SouthHill_button);
            this.Controls.Add(this.FullSize_button);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FPhotoFullSize";
            this.Text = "Photo Viewer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Click += new System.EventHandler(this.ZoomPoint_Click);
            this.ResumeLayout(false);

        }
        #endregion
        //****************************************************************************************************************************
        private int CalculateNewPosition(int iScreenSize,
                                         int iCursorPosition,
                                         int iPhotoSize,
                                         int iPhotoCursorPosition)
        {
            double dDistance = iCursorPosition - iScreenSize / 2;
            double dFactor = dDistance / (double)m_iScreenWidth;
            dDistance = iPhotoSize / m_iZoomLevel * dFactor;
            return iPhotoCursorPosition + (int)dDistance;
        }
        //****************************************************************************************************************************
        private void ShowCemetery(Cemetery eCemetery)
        {
            int iCemeteryID = (int) eCemetery;
            DataTable tbl = new DataTable();
            if (eCemetery == Cemetery._Cemetery)
            {
                //SQL.SelectAll(U.CemeteryRecord_Table, SQL.OrderBy(U.CemeteryID_col, U.LotNumber_col, U.CemeteryRecordID_col), tbl);
                SQL.SelectAll(U.CemeteryRecord_Table, SQL.OrderBy(U.CemeteryID_col, U.CemeteryRecordID_col), tbl);
            }
            else
            {
                //SQL.SelectAll(U.CemeteryRecord_Table, SQL.OrderBy(U.LotNumber_col, U.CemeteryRecordID_col), tbl,
                SQL.SelectAll(U.CemeteryRecord_Table, SQL.OrderBy(U.CemeteryRecordID_col), tbl,
                                          new NameValuePair(U.CemeteryID_col, iCemeteryID));
            }
            CGridCemeteryRecord GridDataViewCemetery = new CGridCemeteryRecord(m_SQL, tbl, eCemetery);
            GridDataViewCemetery.ShowDialog();
            int CemeteryRecordID = GridDataViewCemetery.SelectedCemeteryRecordID;
            if (CemeteryRecordID != 0)
            {
                FCemeteryRecord cemeteryRecord = new FCemeteryRecord(m_SQL, CemeteryRecordID);
                cemeteryRecord.ShowDialog();
            }
        }
        //****************************************************************************************************************************
        private void GoToCemeteryPage(int iCursorX,
                                      int iCursorY)
        {
            if (iCursorX > 810 && iCursorX < 840 && iCursorY > 635 && iCursorY < 675)
                ShowCemetery(Cemetery.EastJamaica_Cemetery);
            else if (iCursorX > 635 && iCursorX < 765 && iCursorY > 485 && iCursorY < 515)
                ShowCemetery(Cemetery.Village_Cemetery);
            else if (iCursorX > 550 && iCursorX < 580 && iCursorY > 720 && iCursorY < 750)
                ShowCemetery(Cemetery.SouthHill_Cemetery);
            else if (iCursorX > 375 && iCursorX < 405 && iCursorY > 585 && iCursorY < 615)
                ShowCemetery(Cemetery.SageHill_Cemetery);
            else if (iCursorX > 195 && iCursorX < 225 && iCursorY > 510 && iCursorY < 540)
                ShowCemetery(Cemetery.SeventhDayAdventist_Cemetery);
            else if (iCursorX > 885 && iCursorX < 915 && iCursorY > 655 && iCursorY < 685)
                ShowCemetery(Cemetery.Robbins_Cemetery);
            else if (iCursorX > 285 && iCursorX < 315 && iCursorY > 125 && iCursorY < 155)
                ShowCemetery(Cemetery.Rawsonville_Cemetery);
            else if (iCursorX > 985 && iCursorX < 1115 && iCursorY > 325 && iCursorY < 355)
                ShowCemetery(Cemetery.SouthWindham_Cemetery);
            else if (iCursorX > 225 && iCursorX < 255 && iCursorY > 500 && iCursorY < 530)
                ShowCemetery(Cemetery.PikesFalls_Cemetery);
            else if (iCursorX > 265 && iCursorX < 295 && iCursorY > 710 && iCursorY < 740)
                ShowCemetery(Cemetery.WestJamaica_Cemetery);
       }
        //****************************************************************************************************************************
        private void ZoomPoint_Click(object sender, System.EventArgs e)
        {
            int iCursorX = Cursor.Position.X;
            int iCursorY = Cursor.Position.Y - 20;
            if (m_eFullSizeMode == FullSizeMode.Cemetery_Mode)
            {
                GoToCemeteryPage(iCursorX, iCursorY);
            }
            else
            {
                if (m_bZoom)
                {
                    m_iPhotoCursorX = CalculateNewPosition(m_iScreenWidth, iCursorX, m_Photo.Width, m_iPhotoCursorX);
                    m_iPhotoCursorY = CalculateNewPosition(m_iScreenHeight, iCursorY, m_Photo.Height, m_iPhotoCursorY);
                }
                else
                {
                    m_iPhotoCursorX = m_Photo.Width * iCursorX / m_iScreenWidth;
                    m_iPhotoCursorY = m_Photo.Height * iCursorY / m_iScreenHeight;
                    this.FullSize_button.Text = "Full Size";
                    m_bZoom = true;
                }
                m_bPhotoAlreadyDrawn = false;
                base.Refresh();
            }
        }
        //****************************************************************************************************************************
        private void ZoomButton_Click(object sender, System.EventArgs e)
        {
            if (m_bZoom)
            {
                m_bZoom = false;
                this.FullSize_button.Text = "Zoom";
            }
            else
            {
                m_bZoom = true;
                this.FullSize_button.Text = "Full Size";
            }
            base.Refresh();
        }
        //****************************************************************************************************************************
        private void SouthHill_button_Click(object sender, System.EventArgs e)
        {
            ShowCemetery(Cemetery.SouthHill_Cemetery);
        }
        //****************************************************************************************************************************
        private void SageHill_button_Button_Click(object sender, System.EventArgs e)
        {
            ShowCemetery(Cemetery.SageHill_Cemetery);
        }
        //****************************************************************************************************************************
        private void SeventhDayAdventist_button_Click(object sender, System.EventArgs e)
        {
            ShowCemetery(Cemetery.SeventhDayAdventist_Cemetery);
        }
        //****************************************************************************************************************************
        private void MundellBourne_button_Click(object sender, System.EventArgs e)
        {
            ShowCemetery(Cemetery.MundellBourne_Cemetery);
        }
        //****************************************************************************************************************************
        private void Robbins_button_Click(object sender, System.EventArgs e)
        {
            ShowCemetery(Cemetery.Robbins_Cemetery);
        }
        //****************************************************************************************************************************
        private void RobinsonDalewood_button_Click(object sender, System.EventArgs e)
        {
            ShowCemetery(Cemetery.RobinsonDalewood_Cemetery);
        }
        //****************************************************************************************************************************
        private void Pratt_button_Click(object sender, System.EventArgs e)
        {
            ShowCemetery(Cemetery.Pratt_Cemetery);
        }
        //****************************************************************************************************************************
        private void Ramsdell_button_Click(object sender, System.EventArgs e)
        {
            ShowCemetery(Cemetery.Ramsdell_Cemetery);
        }
        //****************************************************************************************************************************
        private void Village_button_Click(object sender, System.EventArgs e)
        {
            ShowCemetery(Cemetery.Village_Cemetery);
        }
        //****************************************************************************************************************************
        private void Rawsonville_button_Click(object sender, System.EventArgs e)
        {
            ShowCemetery(Cemetery.Rawsonville_Cemetery);
        }
        //****************************************************************************************************************************
        private void SouthWinham_button_Click(object sender, System.EventArgs e)
        {
            ShowCemetery(Cemetery.SouthWindham_Cemetery);
        }
        //****************************************************************************************************************************
        private void Wilder_button_Click(object sender, System.EventArgs e)
        {
            ShowCemetery(Cemetery.Wilder_Cemetery);
        }
        //****************************************************************************************************************************
        private void PikesFalls_button_Click(object sender, System.EventArgs e)
        {
            ShowCemetery(Cemetery.PikesFalls_Cemetery);
        }
        //****************************************************************************************************************************
        private void WestJamaica_button_Click(object sender, System.EventArgs e)
        {
            ShowCemetery(Cemetery.WestJamaica_Cemetery);
        }
        //****************************************************************************************************************************
        private void AllCemeteries_button_Click(object sender, System.EventArgs e)
        {
            ShowCemetery(Cemetery._Cemetery);
        }
        //****************************************************************************************************************************
        private void AllPersonCW_button_Click(object sender, System.EventArgs e)
        {
            CGridPersonCW gridPersonCW = new CGridPersonCW(ref m_SQL, false);
            gridPersonCW.ShowDialog();
            //ShowPersonCW(Cemetery._Cemetery);
        }
        //****************************************************************************************************************************
        private void NewCemetery_button_Click(object sender, System.EventArgs e)
        {
            FCemeteryRecord CemeteryRecord = new FCemeteryRecord(m_SQL);
            CemeteryRecord.ShowDialog();
        }
        //****************************************************************************************************************************
        private void EastJamaica_button_Click(object sender, System.EventArgs e)
        {
            ShowCemetery(Cemetery.EastJamaica_Cemetery);
        }
        //****************************************************************************************************************************
        private void Fessenden_button_Click(object sender, System.EventArgs e)
        {
            ShowCemetery(Cemetery.Fessenden_Cemetery);
        }
        //****************************************************************************************************************************
        private void SetZoomPhoto(int iZoomLevel)
        {
            m_bZoom = true;
            m_iZoomLevel = iZoomLevel;
            m_iZoomHeight = m_Photo.Height / m_iZoomLevel;
            m_iZoomWidth = m_Photo.Width / m_iZoomLevel;
            m_ZoomPhoto = new Bitmap(m_iZoomWidth, m_iZoomHeight);
        }
    }
}