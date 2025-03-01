using System;
using System.IO;
using System.Drawing;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    public class CPhotoViewerAddMode:FPhotoViewer
    {
        string m_sActiveFolder = "";
        //****************************************************************************************************************************
        public CPhotoViewerAddMode(ref CSql cSQL, ArrayList CopyCategoryValues, string sActiveFolder): base(ref cSQL, false)
        {
            m_CopyCategoryValues = CopyCategoryValues;
            CopyCategory_button.Visible = false;
            PasteCategory_button.Visible = (m_CopyCategoryValues == null || m_CopyCategoryValues.Count == 0) ? false : true;
            m_sActiveFolder = sActiveFolder;
            this.PhotoList_button.Visible = true;
            this.PhotoList_button.Text = "New Photo";
            this.Previous_button.Visible = true;
            this.Previous_button.Text = "New Browse";
            this.Next_button.Visible = true;
            this.Next_button.Text = "Refresh";
            PhotoTitle_label.Visible = false;
            m_sPhotoSource = "";
            buttonSlideShow.Visible = false;

            this.Categories_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CategorylistBox_Click);
            this.PicturedPersons_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PicturedPersonlistBox_Click);
            this.PicturedBuildings_ListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PicturedBuildinglistBox_Click);
            InitializeFields();
            Delete_button.Visible = false;
        }
        //****************************************************************************************************************************
        public string GetActiveFolder()
        {
            return m_sActiveFolder;
        }
        //****************************************************************************************************************************
        private void InitializeFields()
        {
            PhotoName_textBox.Text = SQL.GetNextPhotoName();
            string path = SQL.JpgFolder() + PhotoName_textBox.Text + ".jpg";
            if (File.Exists(path))
            {
                m_myFileName = path;
            }
            if (string.IsNullOrEmpty(m_sActiveFolder))
            {
                m_sActiveFolder = SQL.DataDirectory() + "\\PhotosFromJHFNotebook\\";
            }
            PhotoYear_TextBox.Text = "";
            PhotoSource_comboBox.Text = m_sPhotoSource;
            PhotoDrawer_textBox.Text = "1";
            PhotoFolder_textBox.Text = "1";
            PhotoNotes_TextBox.Text = "";
            NumPicturedPersons_textBox.Text = "";
            NumPicturedBuildings_textBox.Text = "";
            PhotoSource_comboBox.Focus();
        }
        //****************************************************************************************************************************
        private void OpenImageFile()
        {
            string sFilter = "Image Files (JPEG,GIF,BMP)|*.jpg;*.jpeg;*.gif;*.bmp|JPEG Files(*.jpg;*.jpeg)|*.jpg;*.jpeg|GIF Files(*.gif)|*.gif|BMP Files(*.bmp)|*.bmp";
            m_myFileName = UU.SelectFile(sFilter, m_sActiveFolder);
            m_sActiveFolder = m_myFileName.DirectoryFromFullFilename();
            if (m_myFileName.Length != 0)
            {
                try
                {
                    m_Photo = new Bitmap(m_myFileName);
                    this.Text = "Photo Viewer - " + m_myFileName;
                    this.Invalidate();
                }
                catch
                {
                    MessageBox.Show(String.Format("{0} is not a valid Image File", m_myFileName));
                    m_myFileName = "";
                }
            }
        }
        //****************************************************************************************************************************
        public override void NextButton_Click(object sender, System.EventArgs e)
        {
            this.Refresh();
        }
        //****************************************************************************************************************************
        public override void Previous_Button_Click(object sender, System.EventArgs e)
        {
            if (CancelIfChanged())
                return;
            InitializeNewPhoto();
            OpenImageFile();
            if (m_myFileName.Length != 0)
            {
                int iEndOfDirectory = m_myFileName.LastIndexOf('\\');
                string sPhotoName = m_myFileName.Substring(iEndOfDirectory + 1);
                m_OldPhotoName = m_myFileName.FileNameFromFullFilename();
                //PhotoName_textBox.Text = m_OldPhotoName;
            }
            PasteCategory_button.Visible = (m_CopyCategoryValues == null || m_CopyCategoryValues.Count == 0) ? false : true;
            CopyCategory_button.Visible = false;
        }
        //****************************************************************************************************************************
        public override void PhotoList_Click(object sender, System.EventArgs e)
        {
            if (CancelIfChanged())
                return;
            InitializeNewPhoto();
        }
        //****************************************************************************************************************************
        private void InitializeNewPhoto()
            {
            //            m_Photo.Length = 0;
            m_Photo = new Bitmap(1, 1);
            m_bPhotoIsNull = true;
            //            if (m_Photo != null)
            //            {
            //                m_Photo.Dispose();
            //                m_Photo = null;
            //            }
            m_iPhotoID = 0;
            m_sTableName = "";
            m_iNumPeoplePictured = 0;
            m_iNumBuildingsPictured = 0;
            m_aborted = false;
            m_myFileName = "";
            InitializeFields();
            SetToUnmodified();
            PicturedPersons_listBox.Items.Clear();
            PicturedBuildings_ListBox.Items.Clear();
            Categories_listBox.Items.Clear();
            m_CategoryListboxChanged = false;
            Then_button.BackColor = Color.LightSteelBlue;
            Now_button.BackColor = Color.LightSteelBlue;
            m_Photo_ds.Clear();
            this.Refresh();
        }
        //****************************************************************************************************************************
        public override void DisplayPhotoInViewer(int iPersonID)
        {
            SQL.DefinePhoto(m_Photo_ds);
            NumPhotos_Label.Visible = false;
            PhotoList_button.Visible = false;
            Previous_button.Visible = false;
            Next_button.Visible = false;
        }
        //****************************************************************************************************************************
        public override void NewPhotoButton_Click(object sender, System.EventArgs e)
        {
            InitializeNewPhoto();
            m_sActiveFolder = SQL.JpgFolder();
            OpenImageFile();
        }
        //****************************************************************************************************************************
        public override void SaveButton_Click(object sender, System.EventArgs e)
        {
            SaveAndDisplayPhoto();
        }
    }
}
