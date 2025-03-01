using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    public class FImportPhotos : Form
    {
        private CSql m_SQL;
        private ProgressBar progressBar1;
        private string m_sAlphaPrefix;
        public FImportPhotos(CSql sql)
        {
            m_SQL = sql;
            InitializeComponent();
            LoadPrefixAndStartPosition(SQL.GetNextPhotoName());
            Folder_textBox.Text = SQL.JpgFolder();
            UU.LoadSourceComboBox(PhotoSource_comboBox);
        }
        private System.ComponentModel.IContainer components = null;
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
        private void InitializeComponent()
        {
            this.PhotoSource_comboBox = new System.Windows.Forms.ComboBox();
            this.Prefix_textBox = new System.Windows.Forms.TextBox();
            this.End_textBox = new System.Windows.Forms.TextBox();
            this.Start_textBox = new System.Windows.Forms.TextBox();
            this.Import_button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Folder_textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // PhotoSource_comboBox
            // 
            this.PhotoSource_comboBox.FormattingEnabled = true;
            this.PhotoSource_comboBox.Location = new System.Drawing.Point(112, 176);
            this.PhotoSource_comboBox.Name = "PhotoSource_comboBox";
            this.PhotoSource_comboBox.Size = new System.Drawing.Size(233, 21);
            this.PhotoSource_comboBox.TabIndex = 1;
            // 
            // Prefix_textBox
            // 
            this.Prefix_textBox.Enabled = false;
            this.Prefix_textBox.HideSelection = false;
            this.Prefix_textBox.Location = new System.Drawing.Point(112, 91);
            this.Prefix_textBox.Name = "Prefix_textBox";
            this.Prefix_textBox.Size = new System.Drawing.Size(100, 20);
            this.Prefix_textBox.TabIndex = 2;
            // 
            // End_textBox
            // 
            this.End_textBox.Location = new System.Drawing.Point(268, 136);
            this.End_textBox.Name = "End_textBox";
            this.End_textBox.Size = new System.Drawing.Size(77, 20);
            this.End_textBox.TabIndex = 4;
            // 
            // Start_textBox
            // 
            this.Start_textBox.Location = new System.Drawing.Point(112, 136);
            this.Start_textBox.Name = "Start_textBox";
            this.Start_textBox.Size = new System.Drawing.Size(67, 20);
            this.Start_textBox.TabIndex = 6;
            this.Start_textBox.Click += new System.EventHandler(this.Start_textBox_Enter);
            // 
            // Import_button
            // 
            this.Import_button.Location = new System.Drawing.Point(154, 221);
            this.Import_button.Name = "Import_button";
            this.Import_button.Size = new System.Drawing.Size(75, 23);
            this.Import_button.TabIndex = 7;
            this.Import_button.Text = "Import";
            this.Import_button.UseVisualStyleBackColor = true;
            this.Import_button.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 179);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Source";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 139);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "First Photo";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(204, 139);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Last Photo";
            // 
            // Folder_textBox
            // 
            this.Folder_textBox.Enabled = false;
            this.Folder_textBox.Location = new System.Drawing.Point(112, 47);
            this.Folder_textBox.Name = "Folder_textBox";
            this.Folder_textBox.Size = new System.Drawing.Size(233, 20);
            this.Folder_textBox.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Folder";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(37, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Prefix";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(40, 263);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(305, 23);
            this.progressBar1.TabIndex = 14;
            // 
            // FImportPhotos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 377);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Folder_textBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Import_button);
            this.Controls.Add(this.Start_textBox);
            this.Controls.Add(this.End_textBox);
            this.Controls.Add(this.Prefix_textBox);
            this.Controls.Add(this.PhotoSource_comboBox);
            this.Name = "FImportPhotos";
            this.Text = "Add Multiple Photos";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        //****************************************************************************************************************************
        protected System.Windows.Forms.ComboBox PhotoSource_comboBox;
        private System.Windows.Forms.TextBox Prefix_textBox;
        private System.Windows.Forms.TextBox End_textBox;
        private System.Windows.Forms.TextBox Start_textBox;
        private System.Windows.Forms.Button Import_button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Folder_textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        //****************************************************************************************************************************
        private void LoadPrefixAndStartPosition(string sFileName)
        {
            int iEndLocationOfPrefix = sFileName.Length;
            for (int i = sFileName.Length - 1; i > 0; --i)
            {
                if (!U.IsNumeric(sFileName[i]))
                {
                    iEndLocationOfPrefix = i;
                    break;
                }
            }
            m_sAlphaPrefix = sFileName.Substring(0, iEndLocationOfPrefix + 1);
/*            for (int i = iEndLocationOfPrefix + 1; i < sFileName.Length; ++i)
            {
                if (sFileName[i] != '0')
                {
                    iEndLocationOfPrefix = i;
                    break;
                }
            }
            End_textBox.Text = "";*/
            iEndLocationOfPrefix++;
            Prefix_textBox.Text = sFileName.Substring(0, iEndLocationOfPrefix);
            if (iEndLocationOfPrefix == sFileName.Length)
            {
                Start_textBox.Text = "";
                End_textBox.Enabled = false;
            }
            else
            {
                Start_textBox.Text = sFileName.Substring(iEndLocationOfPrefix).ToInt().ToString();
                End_textBox.Enabled = true;
            }
        }
        //****************************************************************************************************************************
        private void Start_textBox_Enter(object sender, System.EventArgs e)
        {
            string sFilter =
                "Image Files (JPEG)|*.jpg;*.jpeg;|JPEG Files(*.jpg;*.jpeg)|*.jpg;*.jpeg";
            string sFullFileName = UU.SelectFile(sFilter, SQL.JpgFolder());
            if (sFullFileName.Length != 0)
            {
                int iExtensionStart = sFullFileName.LastIndexOf(".");
                int iFilenameStart = sFullFileName.LastIndexOf("\\");
                Folder_textBox.Text = sFullFileName.Substring(0, iFilenameStart);
                string sFileName = sFullFileName.Remove(iExtensionStart).Substring(iFilenameStart + 1);
                LoadPrefixAndStartPosition(sFileName);
            }
            PhotoSource_comboBox.Focus();
        }
        //****************************************************************************************************************************
        private bool ThisIsHFPhotoName(string PhotoName)
        {
            return PhotoName == "HF";
        }
        //****************************************************************************************************************************
        private void SetValuesForRenamingFiles(ref bool bNotHFPhotoName,
                                               ref string sOldTIFDirectory,
                                               ref string sNextPhotoName,
                                               ref string sNewDirectory)
        {
            bNotHFPhotoName = true;
            sOldTIFDirectory += "\\";
            sNextPhotoName = SQL.GetNextPhotoName();
            sNextPhotoName = U.GetNextFilename(sNextPhotoName, SQL.JpgFolder());
        }
        //****************************************************************************************************************************
        private string GetNextPhotoName(ref string sNextPhoto,
                                        string sSourcePhotoName)
        {
            if (ThisIsHFPhotoName(sSourcePhotoName.Remove(2)))
                return sSourcePhotoName;
            string sReturnPhoto = sNextPhoto;
            sNextPhoto = U.AddOneToPhotoName(sNextPhoto);
            return sReturnPhoto;
        }
        //****************************************************************************************************************************
        private int LocationOfExtension(string sPhotoName)
        {
            int iLocationOfExtension = sPhotoName.ToLower().LastIndexOf(U.sJPGExtension);
            if (iLocationOfExtension < 0)
            {
                MessageBox.Show("Invalue jpg extension");
                return 0;
            }
            return iLocationOfExtension;
        }
        //****************************************************************************************************************************
        private bool ValidNewPhoto(string sPhotoName,
                                   string sPrefix,
                                   ref int iNewPhoto,
                                   ref int iLocationOfExtension)
        {
            iLocationOfExtension = LocationOfExtension(sPhotoName);
            iNewPhoto = U.GetPhotoNumber(sPhotoName.Remove(iLocationOfExtension), sPrefix.Length);
            if (iNewPhoto < 0)
            {
                return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        private void InsertPhoto(int iNewPhoto,
                                 int iStartPhotoNumber,
                                 int iLocationOfExtension,
                                 bool bNotHFPhotoName,
                                 DataTable Photo_tbl,
                                 ArrayList PhotosToDelete,
                                 ArrayList PhotosCopied,
                                 FileInfo[] rgFiles,
                                 FileInfo fi,
                                 ref int iCount,
                                 ref string sNextPhotoName,
                                 string sSource,
                                 string sOldJPGDirectory,
                                 string sNewDirectory,
                                 string sOldTIFDirectory)
        {
            if (iNewPhoto == iStartPhotoNumber)
            {
                progressBar1.Maximum = rgFiles.Length - iCount;
                progressBar1.Minimum = 0;
                progressBar1.Value = 0;
                progressBar1.Step = 1;
            }
            string sNewPhotoName = GetNextPhotoName(ref sNextPhotoName, fi.Name.Remove(8));
            if (bNotHFPhotoName)
            {
                if (progressBar1.Value == progressBar1.Maximum)
                {
                    progressBar1.Value = progressBar1.Minimum;
                }
                progressBar1.PerformStep();
                string sOldFileName = sOldJPGDirectory + "\\" + fi.Name;
                string sNewFileName = sNewDirectory + sNewPhotoName + U.sJPGExtension;
                if (U.CopyOrRenamePhotoFile(PhotosToDelete, PhotosCopied, sOldFileName, sNewFileName))
                {
                    string sOldTIF = U.FileNameWithExtension(fi.Name.Remove(iLocationOfExtension),
                                                             sOldTIFDirectory, U.sTIFExtension);
                    string sNewTIF = U.FileNameWithExtension(sNewPhotoName, SQL.TifFolder(), U.sTIFExtension);
                    U.CopyOrRenamePhotoFile(PhotosToDelete, PhotosCopied, sOldTIF, sNewTIF);
                }
            }
            else if (SQL.PhotoNameExists(fi.Name.Remove(iLocationOfExtension)))
            {
                string sMessage = fi.Name.Remove(8) + " already exists in Database to end the series to import";
                MessageBox.Show(sMessage);
            }
            InsertPhotoIntoTable(Photo_tbl, U.sJPGExtension, "", 0, sSource, 1, 1,
                             sNewPhotoName, 0, 0);
        }
        //****************************************************************************************************************************
        public bool InsertPhotoIntoTable(DataTable tbl,
                                         string sFileExtension,
                                         string sPhotoNotes,
                                         int iPhotoYear,
                                         string sPhotoSource,
                                         int iPhotoDrawer,
                                         int iPhotoFolder,
                                         string sPhotoName,
                                         int iNumPeoplePictured,
                                         int iNumBuildingsPictured)
        {
            DataRow Photo_row = tbl.NewRow();
            Photo_row[U.PhotoExtension_col] = sFileExtension;
            Photo_row[U.PhotoNotes_col] = sPhotoNotes;
            Photo_row[U.PhotoYear_col] = iPhotoYear;
            Photo_row[U.PhotoSource_col] = sPhotoSource;
            Photo_row[U.PhotoDrawer_col] = iPhotoFolder;
            Photo_row[U.PhotoFolder_col] = iPhotoFolder;
            Photo_row[U.PhotoName_col] = sPhotoName;
            Photo_row[U.NumPicturedPersons_col] = iNumPeoplePictured.ToString();
            Photo_row[U.NumPicturedBuildings_col] = iNumBuildingsPictured.ToString();
            tbl.Rows.Add(Photo_row);
            return true;
        }
        //****************************************************************************************************************************
        private DataTable InsertPhotosIntoTable(
                                    ArrayList PhotosToDelete,
                                    ArrayList PhotosCopied,
                                    string sFolder,
                                    string sPrefix,
                                    int iStartPhotoNumber,
                                    int iEndPhotoNumber,
                                    string sSource)
        {
            string sOldJPGDirectory = sFolder;
            string sOldTIFDirectory = sFolder;
            string sNewDirectory = SQL.JpgFolder();
            string sNextPhotoName = "";
            bool bNotHFPhotoName = false;
            if (!ThisIsHFPhotoName(sPrefix))
                SetValuesForRenamingFiles(ref bNotHFPhotoName, ref sOldTIFDirectory, ref sNextPhotoName, ref sNewDirectory);
            FileInfo[] rgFiles = U.GetAllFilesWithPrefix(sFolder, sPrefix);
            DataTable Photo_tbl = SQL.DefinePhotoTable();
            int iCount = 0;
            foreach (FileInfo fi in rgFiles)
            {
                if (iStartPhotoNumber == 0)
                {
                    InsertPhoto(iCount, iStartPhotoNumber, LocationOfExtension(fi.Name), bNotHFPhotoName,
                                Photo_tbl, PhotosToDelete, PhotosCopied,
                                rgFiles, fi, ref iCount, ref sNextPhotoName, sSource, sOldJPGDirectory,
                                sNewDirectory, sOldTIFDirectory);
                    iCount++;
                }
                else
                {
                    int iNewPhoto = 0;
                    int iLocationOfExtension = 0;
                    if (ValidNewPhoto(fi.Name, sPrefix, ref iNewPhoto, ref iLocationOfExtension))
                    {
                        if (iNewPhoto > iEndPhotoNumber) // we are done
                            break;
                        iCount++;
                        if (iNewPhoto >= iStartPhotoNumber)
                        {
                            InsertPhoto(iNewPhoto, iStartPhotoNumber, iLocationOfExtension, bNotHFPhotoName,
                                        Photo_tbl, PhotosToDelete, PhotosCopied,
                                        rgFiles, fi, ref iCount, ref sNextPhotoName, sSource, sOldJPGDirectory,
                                        sNewDirectory, sOldTIFDirectory);
                        }
                    }
                }
            }
            return Photo_tbl;
        }
        //****************************************************************************************************************************
        private void ImportButton_Click(object sender, EventArgs e)
        {
            if (PhotoSource_comboBox.Text.TrimString().Length == 0)
            {
                if (MessageBox.Show("Source Required. Do you wish to set Photo Source To HF Collection?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    PhotoSource_comboBox.Text = "HF Collection";
                else
                    return;
            }
            int iEndPhotoNumber = End_textBox.Text.ToInt();
            if (iEndPhotoNumber == 0)
                iEndPhotoNumber = 999999;
            ArrayList PhotosToDelete = new ArrayList();
            ArrayList PhotosCopied = new ArrayList();
            int iStartNumber = Start_textBox.Text.ToInt();
            if (iStartNumber == 0)
                m_sAlphaPrefix = "";
            DataTable Photo_tbl = InsertPhotosIntoTable(PhotosToDelete, PhotosCopied, Folder_textBox.Text.ToString(),
                                             m_sAlphaPrefix, iStartNumber, iEndPhotoNumber,
                                             PhotoSource_comboBox.Text.ToString());
            if (Photo_tbl != null && SQL.AddAllNewPhotos(Photo_tbl, PhotosToDelete, PhotosCopied,
                                             Folder_textBox.Text.ToString(), m_sAlphaPrefix,
                                             Start_textBox.Text.ToInt(), iEndPhotoNumber,
                                             PhotoSource_comboBox.Text.ToString()))
            {
                MessageBox.Show("Import Successful");
                this.Close();
            }
            else
            {
                MessageBox.Show("Import Unsuccessful");
            }
        }
    }
}
