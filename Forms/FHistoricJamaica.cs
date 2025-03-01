using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    public class FHistoricJamaica : Form
	{
        public static bool RunPhotoSlideShow = false;

        private const string databasename = "DataBase=HistoricJamaica;";
        private CSql m_SQL;
        private string m_sDataDirectory = "";
        private MainMenu mainMenu1;
        private MenuItem File_menuItem;
        private MenuItem Exit;
        private MenuItem Family_menuItem;
        private MenuItem Maps_menuItem;
        private Bitmap m_Photo;
        private MenuItem CopyDatabase_menuItem;
        private MenuItem ImportGedcom_menuItem;
        private MenuItem menuItem1;
        private MenuItem View_MenuItem;
        private MenuItem Cemeteries_menuItem;
        private MenuItem ImportCemetery_menuItem;
        private MenuItem VitalRecord_menuItem;
        private MenuItem BirthMale_menuItem;
        private MenuItem BirthFemale_menuItem;
        private MenuItem DeathMale_menuItem;
        private MenuItem DeathFemale_menuItem;
        private MenuItem MarriageBride_menuItem;
        private MenuItem MarriageGroom_menuItem;
        private MenuItem CivilUnionPartyA_menuItem;
        private MenuItem CivilUnionPartyB_menuItem;
        private MenuItem Burial_menuItem;
        private MenuItem SearchRecords_menuItem;
        private MenuItem AlternativeSpellings_menuItem;
        private MenuItem ExportSQL_menuItem;
        private MenuItem Collections_menuItem;
        private MenuItem ModernRoads_menuItem;
        private MenuItem AddMultiplsPhotos_menuItem;
        private MenuItem menuItem2;
        private MenuItem AlternativeSpellingsLastName_menuItem;
        private MenuItem ExportBuildings_menuItem;
        private MenuItem Test_menuItem;
        private MenuItem TestHistoricJamaica;
        private MenuItem menuItem3;
        private MenuItem UpdateMultipleMariages_menuItem;
        private ArrayList CopyCategoryValues;
        private string m_sActiveFolder ="";
        private MenuItem MenuPhotoSlideShow;
        private MenuItem ImportSchoolRecords_menuItem;
        private MenuItem menuSchoolRecords;
        private MenuItem ImportCivalWarRecords_menuItem;
        private MenuItem ImportGrandList_menuItem;
        private IContainer components;
        //****************************************************************************************************************************
        public FHistoricJamaica()
		{
            string sServer = UU.GetServerFromIniFile("c:\\JHF\\HistoricJamaica.ini", ref m_sDataDirectory);

            m_SQL = new CSql(databasename, sServer, m_sDataDirectory, true);
            SQL.OpenConnection(databasename, sServer, m_sDataDirectory, true);
            try
            {
                string sFileName = m_sDataDirectory + "\\Wallpaper.jpg";
                m_Photo = new Bitmap(sFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Data directory not Mapped:" + m_sDataDirectory);
                throw ex;
            }
            InitializeComponent();
            ExportSQL_menuItem.Visible = true;
        }
        //****************************************************************************************************************************
        protected override void OnPaint(PaintEventArgs myArgs)
        {
            if (m_Photo != null)
            {
                Graphics myGraph = myArgs.Graphics;
                RectangleF bounds = new RectangleF(
                               (float)this.ClientRectangle.X,
                               (float)this.ClientRectangle.Y, ClientSize.Width, ClientSize.Height);
                myGraph.DrawImage(m_Photo, bounds);
            }
        }
        //****************************************************************************************************************************
        protected override void Dispose(bool disposing)
		{
            SQL.CloseConnection();
            if (m_SQL != null)
            {
                m_SQL.CloseConnection();
                if (disposing)
                {
                    if (components != null)
                    {
                        components.Dispose();
                    }
                }
            }
			base.Dispose( disposing );
		}
        //****************************************************************************************************************************
        #region Windows Form Designer generated code
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.MenuItem People_MenuItem;
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.File_menuItem = new System.Windows.Forms.MenuItem();
            this.Exit = new System.Windows.Forms.MenuItem();
            this.CopyDatabase_menuItem = new System.Windows.Forms.MenuItem();
            this.ImportGedcom_menuItem = new System.Windows.Forms.MenuItem();
            this.ImportCemetery_menuItem = new System.Windows.Forms.MenuItem();
            this.AlternativeSpellingsLastName_menuItem = new System.Windows.Forms.MenuItem();
            this.AlternativeSpellings_menuItem = new System.Windows.Forms.MenuItem();
            this.ImportSchoolRecords_menuItem = new System.Windows.Forms.MenuItem();
            this.ImportCivalWarRecords_menuItem = new System.Windows.Forms.MenuItem();
            this.AddMultiplsPhotos_menuItem = new System.Windows.Forms.MenuItem();
            this.ImportGrandList_menuItem = new System.Windows.Forms.MenuItem();
            this.ExportSQL_menuItem = new System.Windows.Forms.MenuItem();
            this.ExportBuildings_menuItem = new System.Windows.Forms.MenuItem();
            this.UpdateMultipleMariages_menuItem = new System.Windows.Forms.MenuItem();
            this.Test_menuItem = new System.Windows.Forms.MenuItem();
            this.Family_menuItem = new System.Windows.Forms.MenuItem();
            this.ModernRoads_menuItem = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.View_MenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.Cemeteries_menuItem = new System.Windows.Forms.MenuItem();
            this.Maps_menuItem = new System.Windows.Forms.MenuItem();
            this.VitalRecord_menuItem = new System.Windows.Forms.MenuItem();
            this.BirthMale_menuItem = new System.Windows.Forms.MenuItem();
            this.BirthFemale_menuItem = new System.Windows.Forms.MenuItem();
            this.DeathMale_menuItem = new System.Windows.Forms.MenuItem();
            this.DeathFemale_menuItem = new System.Windows.Forms.MenuItem();
            this.MarriageBride_menuItem = new System.Windows.Forms.MenuItem();
            this.MarriageGroom_menuItem = new System.Windows.Forms.MenuItem();
            this.CivilUnionPartyA_menuItem = new System.Windows.Forms.MenuItem();
            this.CivilUnionPartyB_menuItem = new System.Windows.Forms.MenuItem();
            this.Burial_menuItem = new System.Windows.Forms.MenuItem();
            this.SearchRecords_menuItem = new System.Windows.Forms.MenuItem();
            this.Collections_menuItem = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.TestHistoricJamaica = new System.Windows.Forms.MenuItem();
            this.MenuPhotoSlideShow = new System.Windows.Forms.MenuItem();
            this.menuSchoolRecords = new System.Windows.Forms.MenuItem();
            People_MenuItem = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // People_MenuItem
            // 
            People_MenuItem.Index = 1;
            People_MenuItem.Text = "People";
            People_MenuItem.Click += new System.EventHandler(this.PersonButton_Click);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.File_menuItem,
            People_MenuItem,
            this.Family_menuItem,
            this.ModernRoads_menuItem,
            this.menuItem1,
            this.View_MenuItem,
            this.menuItem3,
            this.Cemeteries_menuItem,
            this.Maps_menuItem,
            this.VitalRecord_menuItem,
            this.Collections_menuItem,
            this.menuItem2,
            this.TestHistoricJamaica,
            this.MenuPhotoSlideShow,
            this.menuSchoolRecords});
            // 
            // File_menuItem
            // 
            this.File_menuItem.Index = 0;
            this.File_menuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.Exit,
            this.CopyDatabase_menuItem,
            this.ImportGedcom_menuItem,
            this.ImportCemetery_menuItem,
            this.AlternativeSpellingsLastName_menuItem,
            this.AlternativeSpellings_menuItem,
            this.ImportSchoolRecords_menuItem,
            this.ImportCivalWarRecords_menuItem,
            this.AddMultiplsPhotos_menuItem,
            this.ImportGrandList_menuItem,
            this.ExportSQL_menuItem,
            this.ExportBuildings_menuItem,
            this.UpdateMultipleMariages_menuItem,
            this.Test_menuItem});
            this.File_menuItem.Text = "File";
            // 
            // Exit
            // 
            this.Exit.Index = 0;
            this.Exit.Text = "Exit";
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // CopyDatabase_menuItem
            // 
            this.CopyDatabase_menuItem.Index = 1;
            this.CopyDatabase_menuItem.Text = "Copy Database";
            this.CopyDatabase_menuItem.Click += new System.EventHandler(this.CopyDatabase_Click);
            // 
            // ImportGedcom_menuItem
            // 
            this.ImportGedcom_menuItem.Index = 2;
            this.ImportGedcom_menuItem.Text = "Import GEDCOM File";
            this.ImportGedcom_menuItem.Click += new System.EventHandler(this.Import_Click);
            // 
            // ImportCemetery_menuItem
            // 
            this.ImportCemetery_menuItem.Index = 3;
            this.ImportCemetery_menuItem.Text = "Import Cemetery File";
            this.ImportCemetery_menuItem.Click += new System.EventHandler(this.ImportCemetery_click);
            // 
            // AlternativeSpellingsLastName_menuItem
            // 
            this.AlternativeSpellingsLastName_menuItem.Index = 4;
            this.AlternativeSpellingsLastName_menuItem.Text = "Define Last Name Alternative Spellings";
            this.AlternativeSpellingsLastName_menuItem.Click += new System.EventHandler(this.AlternativeSpellingsLastName_Click);
            // 
            // AlternativeSpellings_menuItem
            // 
            this.AlternativeSpellings_menuItem.Index = 5;
            this.AlternativeSpellings_menuItem.Text = "Define First Name Alternative Spellings";
            this.AlternativeSpellings_menuItem.Click += new System.EventHandler(this.AlternativeSpellingsFirstName_Click);
            // 
            // ImportSchoolRecords_menuItem
            // 
            this.ImportSchoolRecords_menuItem.Index = 6;
            this.ImportSchoolRecords_menuItem.Text = "Import School Records";
            this.ImportSchoolRecords_menuItem.Click += new System.EventHandler(this.ImportSchoolRecords_Click);
            // 
            // ImportCivalWarRecords_menuItem
            // 
            this.ImportCivalWarRecords_menuItem.Index = 7;
            this.ImportCivalWarRecords_menuItem.Text = "Import Cival War Records";
            this.ImportCivalWarRecords_menuItem.Click += new System.EventHandler(this.ImportCivalWarRecords_Click);
            // 
            // AddMultiplsPhotos_menuItem
            // 
            this.AddMultiplsPhotos_menuItem.Index = 8;
            this.AddMultiplsPhotos_menuItem.Text = "Import Multiple Photos";
            this.AddMultiplsPhotos_menuItem.Click += new System.EventHandler(this.AddMultiplsPhotos__Click);
            // 
            // ImportGrandList_menuItem
            // 
            this.ImportGrandList_menuItem.Index = 9;
            this.ImportGrandList_menuItem.Text = "Import Grand List";
            this.ImportGrandList_menuItem.Click += new System.EventHandler(this.ImportGrandListClick);
            // 
            // ExportSQL_menuItem
            // 
            this.ExportSQL_menuItem.Index = 10;
            this.ExportSQL_menuItem.Text = "Export SQL";
            this.ExportSQL_menuItem.Click += new System.EventHandler(this.ExportSQL_Click);
            // 
            // ExportBuildings_menuItem
            // 
            this.ExportBuildings_menuItem.Index = 11;
            this.ExportBuildings_menuItem.Text = "Export Buildings";
            this.ExportBuildings_menuItem.Click += new System.EventHandler(this.ExportBuildings_click);
            // 
            // UpdateMultipleMariages_menuItem
            // 
            this.UpdateMultipleMariages_menuItem.Index = 12;
            this.UpdateMultipleMariages_menuItem.Text = "Update Multiple Mariages";
            this.UpdateMultipleMariages_menuItem.Click += new System.EventHandler(this.UpdateMultipleMariages_click);
            // 
            // Test_menuItem
            // 
            this.Test_menuItem.Index = 13;
            this.Test_menuItem.Text = "Test";
            this.Test_menuItem.Click += new System.EventHandler(this.TestClick);
            // 
            // Family_menuItem
            // 
            this.Family_menuItem.Index = 2;
            this.Family_menuItem.Text = "Family";
            this.Family_menuItem.Click += new System.EventHandler(this.Family_Click);
            // 
            // ModernRoads_menuItem
            // 
            this.ModernRoads_menuItem.Index = 3;
            this.ModernRoads_menuItem.Text = "Modern Roads";
            this.ModernRoads_menuItem.Click += new System.EventHandler(this.ModernRoads_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 4;
            this.menuItem1.Text = "New Photograph";
            this.menuItem1.Click += new System.EventHandler(this.AddPhoto_Click);
            // 
            // View_MenuItem
            // 
            this.View_MenuItem.Index = 5;
            this.View_MenuItem.Text = "View All Photographs";
            this.View_MenuItem.Click += new System.EventHandler(this.View_button_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 6;
            this.menuItem3.Text = "Search for Person";
            this.menuItem3.Click += new System.EventHandler(this.SearchForPersonButton_Click);
            // 
            // Cemeteries_menuItem
            // 
            this.Cemeteries_menuItem.Index = 7;
            this.Cemeteries_menuItem.Text = "Cemeteries";
            this.Cemeteries_menuItem.Click += new System.EventHandler(this.Cemeteries_button_Click);
            // 
            // Maps_menuItem
            // 
            this.Maps_menuItem.Index = 8;
            this.Maps_menuItem.Text = "Maps";
            this.Maps_menuItem.Click += new System.EventHandler(this.ShowMaps_Click);
            // 
            // VitalRecord_menuItem
            // 
            this.VitalRecord_menuItem.Index = 9;
            this.VitalRecord_menuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.BirthMale_menuItem,
            this.BirthFemale_menuItem,
            this.DeathMale_menuItem,
            this.DeathFemale_menuItem,
            this.MarriageBride_menuItem,
            this.MarriageGroom_menuItem,
            this.CivilUnionPartyA_menuItem,
            this.CivilUnionPartyB_menuItem,
            this.Burial_menuItem,
            this.SearchRecords_menuItem});
            this.VitalRecord_menuItem.Text = "Vital Records";
            // 
            // BirthMale_menuItem
            // 
            this.BirthMale_menuItem.Index = 0;
            this.BirthMale_menuItem.Text = "Birth-Male";
            this.BirthMale_menuItem.Click += new System.EventHandler(this.BirthMaleButton_Click);
            // 
            // BirthFemale_menuItem
            // 
            this.BirthFemale_menuItem.Index = 1;
            this.BirthFemale_menuItem.Text = "Birth-Female";
            this.BirthFemale_menuItem.Click += new System.EventHandler(this.BirthFemaleButton_Click);
            // 
            // DeathMale_menuItem
            // 
            this.DeathMale_menuItem.Index = 2;
            this.DeathMale_menuItem.Text = "Death-Male";
            this.DeathMale_menuItem.Click += new System.EventHandler(this.DeathMaleButton_Click);
            // 
            // DeathFemale_menuItem
            // 
            this.DeathFemale_menuItem.Index = 3;
            this.DeathFemale_menuItem.Text = "Death-Female";
            this.DeathFemale_menuItem.Click += new System.EventHandler(this.DeathFemaleButton_Click);
            // 
            // MarriageBride_menuItem
            // 
            this.MarriageBride_menuItem.Index = 4;
            this.MarriageBride_menuItem.Text = "Marriage-Bride";
            this.MarriageBride_menuItem.Click += new System.EventHandler(this.MarriageBrideButton_Click);
            // 
            // MarriageGroom_menuItem
            // 
            this.MarriageGroom_menuItem.Index = 5;
            this.MarriageGroom_menuItem.Text = "Marriage-Groom";
            this.MarriageGroom_menuItem.Click += new System.EventHandler(this.MarriageGroomButton_Click);
            // 
            // CivilUnionPartyA_menuItem
            // 
            this.CivilUnionPartyA_menuItem.Index = 6;
            this.CivilUnionPartyA_menuItem.Text = "Marriage-Party A";
            this.CivilUnionPartyA_menuItem.Click += new System.EventHandler(this.CivilUnionPartyAButton_Click);
            // 
            // CivilUnionPartyB_menuItem
            // 
            this.CivilUnionPartyB_menuItem.Index = 7;
            this.CivilUnionPartyB_menuItem.Text = "Marriage-Party B";
            this.CivilUnionPartyB_menuItem.Click += new System.EventHandler(this.CivilUnionPartyBButton_Click);
            // 
            // Burial_menuItem
            // 
            this.Burial_menuItem.Index = 8;
            this.Burial_menuItem.Text = "Burial";
            this.Burial_menuItem.Click += new System.EventHandler(this.BurialButton_Click);
            // 
            // SearchRecords_menuItem
            // 
            this.SearchRecords_menuItem.Index = 9;
            this.SearchRecords_menuItem.Text = "Search Records";
            this.SearchRecords_menuItem.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // Collections_menuItem
            // 
            this.Collections_menuItem.Index = 10;
            this.Collections_menuItem.Text = "Collections";
            this.Collections_menuItem.Click += new System.EventHandler(this.Collections_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 11;
            this.menuItem2.Text = "Categories";
            this.menuItem2.Click += new System.EventHandler(this.Categories_Click);
            // 
            // TestHistoricJamaica
            // 
            this.TestHistoricJamaica.Index = 12;
            this.TestHistoricJamaica.Text = "Slide Show";
            this.TestHistoricJamaica.Click += new System.EventHandler(this.SlideShow_Click);
            // 
            // MenuPhotoSlideShow
            // 
            this.MenuPhotoSlideShow.Index = 13;
            this.MenuPhotoSlideShow.Text = "Photo Slide Show";
            this.MenuPhotoSlideShow.Click += new System.EventHandler(this.PhotoSlideShow_Click);
            // 
            // menuSchoolRecords
            // 
            this.menuSchoolRecords.Index = 14;
            this.menuSchoolRecords.Text = "School Records";
            this.menuSchoolRecords.Click += new System.EventHandler(this.SchoolRecords_Click);
            // 
            // FHistoricJamaica
            // 
            this.AllowDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1523, 563);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "FHistoricJamaica";
            this.Text = "Jamaica Historical Society";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Template_Load);
            this.ResumeLayout(false);

		}
		#endregion
        //****************************************************************************************************************************
        private void ImportSchoolRecords_Click(object sender, System.EventArgs e)
        {
            CImportSchoolRecords Import = new CImportSchoolRecords(m_SQL, SQL.DataDirectory());
            Import.GetSchoolRecordsForFile();
        }
        //****************************************************************************************************************************
        private void ImportCivalWarRecords_Click(object sender, System.EventArgs e)
        {
            CImportPersonCWRecords Import = new CImportPersonCWRecords(m_SQL, SQL.DataDirectory());
        }
        //****************************************************************************************************************************
        private void SlideShow_Click(object sender, System.EventArgs e)
        {
            DataTable Photo_tbl = SQL.GetSlideShow();
            if (Photo_tbl.Rows.Count == 0)
            {
                return;
            }
            FPhotoViewer PhotoViewer = new FPhotoViewer(ref m_SQL, Photo_tbl, U.SlideShow_Table, FHistoricJamaica.RunPhotoSlideShow);
            PhotoViewer.ShowDialog();
        }
        //****************************************************************************************************************************
        private void PersonButton_Click(object sender, System.EventArgs e)
        {
            FPerson Person = new FPerson(m_SQL, false);
            Person.ShowDialog();
        }
        //****************************************************************************************************************************
        private void SearchForPersonButton_Click(object sender, System.EventArgs e)
        {
            FSearchForPerson SearchForPerson = new FSearchForPerson(m_SQL);
            SearchForPerson.ShowDialog();
        }
        //****************************************************************************************************************************
        private void Family_Click(object sender, System.EventArgs e)
		{
            FFamily Family = new FFamily(m_SQL);
            Family.ShowDialog();
        }
        //****************************************************************************************************************************
        private void ModernRoads_Click(object sender, System.EventArgs e)
        {
            CGridGroupValuesModernRoads GridGroupValuesModernRoads = new CGridGroupValuesModernRoads(m_SQL, 0, false, false, false);
            GridGroupValuesModernRoads.ShowDialog();
        }
        //****************************************************************************************************************************
        private void Collections_Click(object sender, System.EventArgs e)
        {
            CGridGroupValuesCollection GridGroupValuesCollection = new CGridGroupValuesCollection(m_SQL);
            GridGroupValuesCollection.ShowDialog();
        }
        //****************************************************************************************************************************
        private void Categories_Click(object sender, System.EventArgs e)
        {
            CGridGroupValuesCategory GridGroupValuesCategory = new CGridGroupValuesCategory(m_SQL);
            GridGroupValuesCategory.ShowDialog();
        }
        //****************************************************************************************************************************
        private void FamilyTree_Click(object sender, System.EventArgs e)
        {
            FFamilyTree FamilyTree = new FFamilyTree();
            FamilyTree.ShowDialog();
        }
        //****************************************************************************************************************************
        private void AddPhoto_Click(object sender, System.EventArgs e)
		{
            CPhotoViewerAddMode PhotoViewer = new CPhotoViewerAddMode(ref m_SQL, CopyCategoryValues, m_sActiveFolder);
            PhotoViewer.ShowDialog();
            m_sActiveFolder = PhotoViewer.GetActiveFolder();
            CopyCategoryValues = PhotoViewer.GetCopyCategoryValues();
        }
        //****************************************************************************************************************************
        private void SchoolRecords_Click(object sender, System.EventArgs e)
        {
            CGridSchoolRecords gridSchoolRecords = new CGridSchoolRecords(m_SQL, false);
            gridSchoolRecords.ShowDialog();
        }
        //****************************************************************************************************************************
        private void PhotoSlideShow_Click(object sender, System.EventArgs e)
        {
            //RunPhotoSlideShow = true;
            FPhotoViewer photoSlideShow = new FPhotoViewer(ref m_SQL, CopyCategoryValues, U.Photo_Table, true);
            photoSlideShow.ShowDialog();
            CopyCategoryValues = photoSlideShow.GetCopyCategoryValues();
        }
        //****************************************************************************************************************************
        private void View_button_Click(object sender, System.EventArgs e)
        {
            //RunPhotoSlideShow = false;
            FPhotoViewer PhotoViewer = new FPhotoViewer(ref m_SQL, CopyCategoryValues, U.Photo_Table, false);
            PhotoViewer.ShowDialog();
            CopyCategoryValues = PhotoViewer.GetCopyCategoryValues();
        }
        //****************************************************************************************************************************
        private void Cemeteries_button_Click(object sender, System.EventArgs e)
        {
            Bitmap HFPhoto = null;
            try
            {
                HFPhoto = new Bitmap(m_sDataDirectory + "\\JamaicaCemeteryMap.jpg");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to find Cemetery Map");
                return;
            }
            FPhotoFullSize PhotoFullSize = new FPhotoFullSize(HFPhoto, m_SQL);
            PhotoFullSize.ShowDialog();
        }
        //****************************************************************************************************************************
        private void BirthMaleButton_Click(object sender, System.EventArgs e)
        {
            FVitalRecord VitalRecordBirth = new FVitalRecord(EVitalRecordType.eBirthMale, m_SQL);
            VitalRecordBirth.ShowDialog();
        }
        //****************************************************************************************************************************
        private void BirthFemaleButton_Click(object sender, System.EventArgs e)
        {
            FVitalRecord VitalRecordBirth = new FVitalRecord(EVitalRecordType.eBirthFemale, m_SQL);
            VitalRecordBirth.ShowDialog();
        }
        //****************************************************************************************************************************
        private void DeathMaleButton_Click(object sender, System.EventArgs e)
        {
            FVitalRecord VitalRecordDeath = new FVitalRecord(EVitalRecordType.eDeathMale, m_SQL);
            VitalRecordDeath.ShowDialog();
        }
        //****************************************************************************************************************************
        private void DeathFemaleButton_Click(object sender, System.EventArgs e)
        {
            FVitalRecord VitalRecordDeath = new FVitalRecord(EVitalRecordType.eDeathFemale, m_SQL);
            VitalRecordDeath.ShowDialog();
        }
        //****************************************************************************************************************************
        private void BurialButton_Click(object sender, System.EventArgs e)
        {
            FVitalRecord VitalRecordBurial = new FVitalRecord(EVitalRecordType.eBurial, m_SQL);
            VitalRecordBurial.ShowDialog();
        }
        //****************************************************************************************************************************
        private void IntegrateAllButton_Click(object sender, System.EventArgs e)
        {
            FVitalRecord VitalRecordIntegrateAll = new FVitalRecord(EVitalRecordType.eIntegrateAll, m_SQL);
            VitalRecordIntegrateAll.ShowDialog();
        }
        //****************************************************************************************************************************
        private void ShowAllPersonsWhereSexIsBlank()
        {
            DataTable Person_tbl = new DataTable();
            SQL.SelectAll(U.Person_Table, Person_tbl, new NameValuePair(U.Sex_col, " = ''"));
            foreach(DataRow Person_row in Person_tbl.Rows)
            {
                DataTable tbl = new DataTable();
                SQL.GetVitalRecordsForPerson(tbl, Person_row[U.PersonID_col].ToInt(), U.PersonID_col);
                if (tbl.Rows.Count != 0)
                {
                    DataRow row = U.VitalRecordRow(tbl, EVitalRecordType.eBirthMale, EVitalRecordType.eBirthFemale);
                    if (row == null || row[U.Sex_col].ToString().Length == 0)
                    {
                        row = U.VitalRecordRow(tbl, EVitalRecordType.eDeathMale, EVitalRecordType.eDeathFemale);
                    }
                    if (row == null || row[U.Sex_col].ToString().Length == 0)
                    {
                        row = U.VitalRecordRow(tbl, EVitalRecordType.eMarriageBride, EVitalRecordType.eMarriageGroom);
                    }
                    if (row == null || row[U.Sex_col].ToString().Length == 0)
                    {
                        row = U.VitalRecordRow(tbl, EVitalRecordType.eCivilUnionPartyA, EVitalRecordType.eCivilUnionPartyB);
                    }
                    if (row == null || row[U.Sex_col].ToString().Length == 0)
                    {
                        row = U.VitalRecordRow(tbl, EVitalRecordType.eBurial, EVitalRecordType.eBurial);
                    }
                    if (row != null)
                    {
                        string sSex = row[U.Sex_col].ToString();
                        if (sSex.Length != 0 && sSex[0] != ' ')
                            Person_row[U.Sex_col] = sSex;
                    }
                }
            }
            if (Person_tbl.Rows.Count != 0)
            {
                SQL.UpdateWithDA(Person_tbl, U.Person_Table, U.PersonID_col, new ArrayList(new string[] { U.Sex_col }));
//              SQL.UpdateValue(Person_tbl, U.Person_Table, U.PersonID_col, U.Sex_col, SqlDbType.VarChar, 1));
            }
            Person_tbl.Clear();
            SQL.SelectAll(U.Person_Table, Person_tbl, new NameValuePair(U.Sex_col, " = ''"));
            foreach (DataRow Person_row in Person_tbl.Rows)
            {
                FPerson Person = new FPerson(m_SQL, Person_row[U.PersonID_col].ToInt(), false);
                Person.ShowDialog();
            }
            DataTable VitalRecord_tbl = new DataTable();
            SQL.SelectAll(U.VitalRecord_Table, VitalRecord_tbl, new NameValuePair(U.Sex_col, " = ''"));
            foreach (DataRow VitalRecord_row in VitalRecord_tbl.Rows)
            {
                EVitalRecordType VitalRecordType = (EVitalRecordType) VitalRecord_row[U.VitalRecordType_col];
                FVitalRecord VitalRecord = new FVitalRecord(VitalRecordType, VitalRecord_row[U.PersonID_col].ToInt(), m_SQL);
                VitalRecord.ShowDialog();
            }
        }
        //****************************************************************************************************************************
        private void CheckForCircularReferences()
        {
            DataTable Person_tbl = new DataTable();
            SQL.SelectAll(U.Person_Table, Person_tbl);
            foreach (DataRow Person_row in Person_tbl.Rows)
            {
                int iPerson = Person_row[U.PersonID_col].ToInt();
                int iFather = Person_row[U.FatherID_col].ToInt();
                DataTable Marriage_tbl = new DataTable();
                SQL.GetAllSpouses(Marriage_tbl, iPerson);
                foreach (DataRow Marriage_row in Marriage_tbl.Rows)
                {
                    int iPersonID = Marriage_row[U.PersonID_col].ToInt();
                    int iSpouseID = Marriage_row[U.SpouseID_col].ToInt();
                    if (!UU.NotCircularReference(m_SQL, iPerson, iSpouseID))
                    {
                        if (UU.SameAncestorList == null || UU.SameAncestorList.Count == 0)
                        {
                            MessageBox.Show("Circular Reference: " + iPerson.ToString() + "-" + iFather.ToString());
                        }
                    }
                }
            }
        }
        //****************************************************************************************************************************
        private void ExportBuildings_click(object sender, System.EventArgs e)
        {
            CExportBuilding ExportBuilding = new CExportBuilding(m_SQL);
        }
        //****************************************************************************************************************************
        private void ExportSQL_Click(object sender, System.EventArgs e)
        {
            CExportSQL ExportSQL = new CExportSQL(m_SQL, m_sDataDirectory);
        }
        //****************************************************************************************************************************
        private void SetCensusYear(DataRow row,
                                   int year)
        {
            int iBornPage = row[U.BornPage_col].ToInt();
            if (iBornPage == 0)
            {
                iBornPage = 99;
            }
            row[U.CensusYearCol(year)] = iBornPage;
            row[U.BornPage_col] = 0;
        }
        //****************************************************************************************************************************
        private void AddPreviousSpouse()
        {
            DataTable person_tbl = SQL.GetAllPersons();
            int count = 0;
            DataTable newPersonTbl = SQL.DefinePersonTable();
            DataTable marriageTbl = SQL.DefineMarriageTable();
            SqlCommand insertPersonCommand = SQL.InsertCommand(null, newPersonTbl, U.Person_Table, true);
            SqlCommand insertMarriageCommand = SQL.InsertCommand(null, marriageTbl, U.Marriage_Table, false);
            foreach (DataRow person_row in person_tbl.Rows)
            {
                if (!String.IsNullOrEmpty(person_row[U.MarriedName2_col].ToString()))
                {
                    int personId = person_row[U.PersonID_col].ToInt();
                    newPersonTbl.Rows.Clear();
                    DataRow row = newPersonTbl.NewRow();
                    SQL.InitializePersonTable(row);
                    row[U.FirstName_col] = "Previous";
                    row[U.MiddleName_col] = "Spouse";
                    row[U.Sex_col] = 'M';
                    row[U.LastName_col] = person_row[U.MarriedName2_col];
                    newPersonTbl.Rows.Add(row);
                    SQL.InsertWithDA(newPersonTbl, insertPersonCommand);
                    int spouseId = newPersonTbl.Rows[0][U.PersonID_col].ToInt();
                    marriageTbl.Rows.Clear();
                    DataRow marriageRow = marriageTbl.NewRow();
                    marriageRow[U.PersonID_col] = personId;
                    marriageRow[U.SpouseID_col] = spouseId;
                    marriageRow[U.DateMarried_col] = "";
                    marriageRow[U.Divorced_col] = "M";
                    marriageTbl.Rows.Add(marriageRow);
                    SQL.InsertWithDA(marriageTbl, insertMarriageCommand);
                    count++;
                }
            }
        }
        //****************************************************************************************************************************
        private void DuplicateDescriptions()
        {
            DataTable person_tbl = SQL.GetAllPersons();
            foreach (DataRow personRow in person_tbl.Rows)
            {
            }
        }
        //****************************************************************************************************************************
        private void CheckMarriedNameAgainstFathersLastName()
        {
            DataTable person_tbl = SQL.GetAllPersons();
            //DataTable person_tbl = new DataTable();
            //SQL.GetPerson(person_tbl, 7771);
            int count = 0;
            int count1 = 0;
            int count2 = 0;
            ArrayList personList = new ArrayList();
            foreach (DataRow personRow in person_tbl.Rows)
            {
                if (personRow[U.Sex_col].ToChar() == 'M' && personRow[U.MarriedName_col].ToString().Length != 0)
                {
                    count++;
                }
                string fathersLastName, mothersLastName;
                FatherMotherLastName(personRow, out fathersLastName, out mothersLastName);
                string marriedName = personRow[U.MarriedName_col].ToString();
                string lastName = personRow[U.LastName_col].ToString();
                if (String.IsNullOrEmpty(fathersLastName))
                {
                }
                else if (fathersLastName == marriedName)
                {
                    string tmpLastName = personRow[U.LastName_col].ToString();
                    //personRow[U.LastName_col] = fathersLastName;
                    if (String.IsNullOrEmpty(tmpLastName) &&  tmpLastName != fathersLastName)
                    {
                        personRow[U.MarriedName_col] = tmpLastName;
                        count1++;
                    }
                    //SQL.UpdatePersonTableForField(personRow[U.PersonID_col].ToInt(), new NameValuePair(U.LastName_col, fathersLastName), new NameValuePair(U.MarriedName_col, tmpLastName));
                }
                else if (String.IsNullOrEmpty(lastName))
                {
                    lastName = fathersLastName;
                    count2++;
                }
                else if (FathersOrMotherLastNameNotEqualLastName(personRow, fathersLastName, mothersLastName, lastName))
                {
                    personList.Add(new LastNameTrail(lastName + " " + personRow[U.FirstName_col].ToString(), fathersLastName, personRow[U.PersonID_col].ToInt()));
                }
            }
            PrintLastNameTrail(personList);
            SQL.UpdateWithDA(person_tbl, U.Person_Table, U.PersonID_col, new ArrayList(new string[] { U.Sex_col, U.LastName_col, U.MarriedName_col, U.MarriedName2_col }));

        }
        //****************************************************************************************************************************
        private void FatherMotherLastName(DataRow personRow, out string fatherLastName, out string motherLastName)
        {
            fatherLastName = "";
            motherLastName = "";
            DataRow fatherRow = SQL.GetPerson(personRow[U.FatherID_col].ToInt());
            DataRow motherRow = SQL.GetPerson(personRow[U.MotherID_col].ToInt());
            checkMotherOrFather(personRow, fatherRow, 'M');
            if (fatherRow != null)
            {
                fatherLastName = fatherRow[U.LastName_col].ToString();
            }
            checkMotherOrFather(personRow, motherRow, 'F');
            if (motherRow != null)
            {
                motherLastName = motherRow[U.LastName_col].ToString();
            }
        }
        //****************************************************************************************************************************
        private bool FathersOrMotherLastNameNotEqualLastName(DataRow personRow,
                                                             string  fathersLastName, 
                                                             string  mothersLastName, 
                                                             string  lastName)
        {
            int personId = personRow[U.PersonID_col].ToInt();
            if (fathersLastName == lastName)
            {
                return false;
            }
            if (mothersLastName == lastName)
            {
                return false;
            }
            DataTable LastNameAlternativeSpellings_tbl = SQL.GetAlternativeSpellings(U.AlternativeSpellingsLastName_Table, lastName);
            foreach (DataRow LastNameAlternativeSpellings_row in LastNameAlternativeSpellings_tbl.Rows)
            {
                string alternateLastName = LastNameAlternativeSpellings_row[U.AlternativeSpelling_Col].ToString();
                if (fathersLastName.ToLower() == alternateLastName.ToLower())
                {
                    return false;
                }
                if (mothersLastName.ToLower() == alternateLastName.ToLower())
                {
                    return false;
                }
            }
            switch (personId)
            {
                case 6849: return false;
                case 2779: return false;
                case 11178: return false;
                case 11960: return false;
                default:
                    personRow[U.MarriedName2_col] = personRow[U.LastName_col];
                    personRow[U.LastName_col] = fathersLastName;
                    break;
            }
            return true;
        }
        //****************************************************************************************************************************
        private void checkMotherOrFather(DataRow personRow,
                                           DataRow parentRow,
                                           char parentSex)
        {
            if (parentRow == null)
            {
                return;
            }
            char Sex = parentRow[U.Sex_col].ToChar();
            if (Sex != parentSex)
            {
                int personID = personRow[U.PersonID_col].ToInt();
                object oldMother = OldIdIfNotPersonID(personID, personRow[U.MotherID_col].ToInt());
                object oldFather = OldIdIfNotPersonID(personID, personRow[U.FatherID_col].ToInt());
                //                SQL.UpdatePersonTableForField(personRow[U.PersonID_col].ToInt(), new NameValuePair(U.MotherID_col, oldFather), new NameValuePair(U.FatherID_col, oldMother));
                //personRow[U.MotherID_col] = oldFather;
                //personRow[U.FatherID_col] = oldMother;
                parentRow = SQL.GetPerson(personRow[U.FatherID_col].ToInt());
                if (parentRow == null)
                {
                    return;
                }
            }
        }
        //****************************************************************************************************************************
        private int OldIdIfNotPersonID(int personID,
                          int id)
        {
            if (personID == id)
            {
                return 0;
            }
            else
            {
                return id;
            }
        }
        //****************************************************************************************************************************
        private void CheckMarriageRecordsForDate()
        {
            DataTable tbl = SQL.GetAllMarriageRecords();
            foreach (DataRow personVitalRecordRow in tbl.Rows)
            {
                int personID = personVitalRecordRow[U.PersonID_col].ToInt();
                if (personID != 0)
                {
                    int spouseVitalRecordID = personVitalRecordRow[U.SpouseID_col].ToInt();
                    DataRow spouseVitalRecordRow = SQL.GetVitalRecord(spouseVitalRecordID);
                    if (spouseVitalRecordRow != null)
                    {
                        UpdateMarriageRecord(spouseVitalRecordRow, personID);
                    }
                }
            }
        }
        //****************************************************************************************************************************
        private void UpdateMarriageRecord(DataRow spouseVitalRecordRow,
                                          int personID)
        {
            int spouseID = spouseVitalRecordRow[U.PersonID_col].ToInt();
            DataTable marriageTbl = new DataTable();
            if (SQL.GetMarriage(marriageTbl, personID, spouseID))
            {
                DataRow marriageRow = marriageTbl.Rows[0];
                string VitalRecordDate = U.BuildDate(spouseVitalRecordRow[U.DateYear_col].ToInt(),
                                                     spouseVitalRecordRow[U.DateMonth_col].ToInt(),
                                                     spouseVitalRecordRow[U.DateDay_col].ToInt());
                if (marriageRow[U.DateMarried_col].ToString() != VitalRecordDate)
                {
                    SQL.UpdateMarriageDate(personID, spouseID, VitalRecordDate);
                }
            }
        }
        //****************************************************************************************************************************
        private void CheckFemaleDeathRecords()
        {
            int count = 0;
            DataTable vitalRecordTbl = SQL.GetAllDeathFemaleVitalRecords();
            foreach (DataRow vitalRecordRow in vitalRecordTbl.Rows)
            {
                int personID = vitalRecordRow[U.PersonID_col].ToInt();
                DataRow personRow = SQL.GetPerson(personID);
                string fathersLastName = PersonFathersLastName(personRow);
                if (fathersLastName.Length != 0)
                {
                    string lastName = personRow[U.LastName_col].ToString();
                    string marriedName = personRow[U.MarriedName_col].ToString();
                    if (fathersLastName.ToLower().Trim() != lastName.ToLower().Trim())
                    {
                        if (lastName.Length == 0 || marriedName.Length != 0)
                        {
                            SQL.UpdatePersonTableForField(personID, new NameValuePair(U.LastName_col, fathersLastName));
                            count++;
                        }
                        else
                        {
                            SQL.UpdatePersonTableForField(personID, new NameValuePair(U.MarriedName_col, lastName),
                                                                new NameValuePair(U.LastName_col, fathersLastName));
                            count++;
                        }
                    }
                }
            }
        }
        //****************************************************************************************************************************
        private string PersonFathersLastName(DataRow personRow)
        {
            if (personRow == null)
            {
                return "";
            }
            int fatherID = personRow[U.FatherID_col].ToInt();
            if (fatherID == 0)
            {
                return "";
            }
            DataRow fatherRow = SQL.GetPerson(fatherID);
            if (fatherRow == null)
            {
                return "";
            }
            return fatherRow[U.LastName_col].ToString();
        }
        //****************************************************************************************************************************
        private void CheckVitalRecordIds()
        {
            ArrayList personsToDelete = new ArrayList();
            ArrayList multiplePersonsToDelete = new ArrayList();
            DataTable Person_tbl = SQL.GetAllPersons();
            DataTable VitelRecord_tbl = SQL.GetAllVitalRecords();
            ArrayList vitalrecordids = new ArrayList();
            foreach (DataRow vitalRecord_row in VitelRecord_tbl.Rows)
            {
                if (vitalRecord_row.RowState != DataRowState.Unchanged)
                {
                }
                int vitalRecordId = vitalRecord_row[U.VitalRecordID_col].ToInt();
                int vitalRecordpersonId = vitalRecord_row[U.PersonID_col].ToInt();
                int vitalRecordFatherId = vitalRecord_row[U.FatherID_col].ToInt();
                int vitalRecordMotherId = vitalRecord_row[U.MotherID_col].ToInt();
                if (vitalRecordpersonId != 0)
                {
                    string selectStatement = ("PersonID = '" + vitalRecordpersonId) + "'";
                    DataRow[] results = Person_tbl.Select(selectStatement);
                    if (results.Length == 1)
                    {
                        DataRow person_row = results[0];
                        CheckParent(personsToDelete, multiplePersonsToDelete, vitalRecord_row, vitalRecordFatherId, person_row[U.FatherID_col].ToInt(), U.FatherID_col, vitalrecordids);
                        CheckParent(personsToDelete, multiplePersonsToDelete, vitalRecord_row, vitalRecordMotherId, person_row[U.MotherID_col].ToInt(), U.MotherID_col, vitalrecordids);
                    }
                }
            }
            PrintLastNameTrail(vitalrecordids);
        }
        //****************************************************************************************************************************
        private void CheckParent(ArrayList personsToDelete,
                                 ArrayList multiplePersonsToDelete,
                                 DataRow vitalRecord_row, 
                                 int vitalRecordParentId, 
                                 int parentId, 
                                 string parentId_col,
                                 ArrayList vitalrecordids)
        {
            if (vitalRecordParentId != 0 && parentId != vitalRecordParentId)
            {
                if (parentId == 0)
                {
                    vitalRecord_row[parentId_col] = 0;
                }
                else
                {
                    vitalRecord_row[parentId_col] = parentId;
                    personsToDelete.Add(vitalRecordParentId);
                    vitalrecordids.Add(new LastNameTrail(vitalRecord_row[U.FirstName_col].ToString(), vitalRecord_row[U.LastName_col].ToString(), vitalRecord_row[U.VitalRecordID_col].ToInt()));
                }
            }
        }
        //****************************************************************************************************************************
        private void CheckMaleSex()
        {
            int count = 0;
            DataTable person_tbl = SQL.GetAllPersons();
            foreach (DataRow person_row in person_tbl.Rows)
            {
                char sex = SQL.GetFirstNameSex(person_row[U.FirstName_col].ToString());
                if (sex != ' ' && sex != 'B' && person_row[U.Sex_col].ToChar() != sex)
                {
                    if (sex == 'M')
                    {
                        person_row[U.MarriedName_col] = "";
                        person_row[U.Sex_col] = 'M';
                        count++;
                    }
                }
            }
            SQL.UpdatePersonTableForField(person_tbl, U.MarriedName_col, U.Sex_col);
        }
        //****************************************************************************************************************************
        private void CheckNameAndSexPerson()
        {
            int count = 0;
            int count1 = 0;
            DataTable person_tbl = SQL.GetAllPersons();
            foreach (DataRow person_row in person_tbl.Rows)
            {
                char sex = SQL.GetFirstNameSex(person_row[U.FirstName_col].ToString());
                if (sex != ' ' && sex != 'B' && person_row[U.Sex_col].ToChar() != sex)
                {
                    if (sex == 'M')
                    {
                        person_row[U.MarriedName_col] = "";
                        person_row[U.Sex_col] = 'M';
                        count++;
                    }
                    else
                    {
                        count1++;
                    }
                }
            }
            SQL.UpdatePersonTableForField(person_tbl, U.MarriedName_col, U.Sex_col);
        }
        //****************************************************************************************************************************
        private void ConvertDates()
        {
            DataTable Person_tbl = SQL.GetAllPersons();
            ArrayList dates = new ArrayList();
            foreach (DataRow personRow in Person_tbl.Rows)
            {
                CheckDate(personRow, U.BornDate_col);
                CheckDate(personRow, U.DiedDate_col);
                CheckDate(personRow, U.BuriedDate_col);
            }
        }
        //****************************************************************************************************************************
        private void CheckDate(DataRow personRow,
                               string  dateCol)
        {
            string inString = personRow[dateCol].ToString();
            if (inString.Length > 0)
            {
                string outString = UU.ConvertToDateYMD(personRow[dateCol].ToString());
                if (inString != outString)
                {
                    SQL.UpdatePersonTableForField(personRow[U.PersonID_col].ToInt(), new NameValuePair(dateCol, outString));
                }
            }
        }
        //****************************************************************************************************************************
        private void RenamePhotos()
        {
            string path = @"c:\temp\Census\Census 1930";
            var dirFiles = Directory.GetFiles(path);
            foreach (var file in dirFiles)
            {
                if (file.Substring(0, 6) != "Thumbs")
                {
                    string str = file.Replace("Jamaica ", "HF");
                    str = str.Replace("_", "");
                    File.Copy(file, str);
                }
            }
        }
        //****************************************************************************************************************************
        private void CheckedPicturedElements(string table,
                                             string picturedOrderCol)
        {
            DataTable tbl = SQL.GetAllPicturedElements(table, picturedOrderCol);
            int picturedNumber = 0;
            int photoID = 0;
            int previousNumber = 0;
            foreach (DataRow row in tbl.Rows)
            {
                int rowPhotoID = row[U.PhotoID_col].ToInt();
                int rowPicturedNumber = row[picturedOrderCol].ToInt();
                if (rowPhotoID != photoID)
                {
                    photoID = rowPhotoID;
                    picturedNumber = rowPicturedNumber;
                    previousNumber = 0;
                }
                if (rowPicturedNumber != picturedNumber)
                {
                    if (rowPicturedNumber == previousNumber)
                    {
                    }
                    else
                    {
                        picturedNumber = rowPicturedNumber;
                    }
                }
                previousNumber = picturedNumber;
                picturedNumber++;
            }
        }
        //****************************************************************************************************************************
        private void TestPersonBuildings()
        {
            ArrayList BuildingIDs = new ArrayList();
            DataTable tbl = SQL.GetAllPersons();
            foreach (DataRow row in tbl.Rows)
            {
                int personID = row[U.PersonID_col].ToInt();
                DataTable personTbl = SQL.GetAllBuildingOccupantsForPerson(row[U.PersonID_col].ToInt());
                if (personTbl.Rows.Count > 1)
                {
                    BuildingIDs.Clear();
                    foreach (DataRow personRow in personTbl.Rows)
                    {
                        int personBuildingID = personRow[U.BuildingID_col].ToInt();
                        BuildingIDs.Add(personBuildingID);
                    }
                }
            }
        }
        //****************************************************************************************************************************
        private class LastNameTrail
        {
            public string lastname;
            public string firstname;
            public string bornYear;
            public LastNameTrail(object lastname,
                               object firstname,
                               object bornYear)
            {
                this.lastname = lastname.ToString();
                this.firstname = firstname.ToString();
                this.bornYear = bornYear.ToString();
            }
        }
        //****************************************************************************************************************************
        private class PersonStats
        {
            public int personId = 0;
            public string firstName = "";
            public string lastName = "";
            public string middleName = "";
            public string marriedName = "";
            public int bornYear = 0;
            public int diedYear = 0;
            public int fatherId = 0;
            public int motherId = 0;
            public char sex = ' ';
            public PersonStats()
            {
            }
            public PersonStats(DataRow dataRow, int bornYear, int diedYear)
            {
                firstName = dataRow[U.FirstName_col].ToString();
                middleName = dataRow[U.MiddleName_col].ToString();
                lastName = dataRow[U.LastName_col].ToString();
                marriedName = dataRow[U.MarriedName_col].ToString();
                this.bornYear = bornYear;
                this.diedYear = diedYear;
                sex = dataRow[U.Sex_col].ToChar();
                fatherId = dataRow[U.FatherID_col].ToInt();
                motherId = dataRow[U.MotherID_col].ToInt();
            }
            public PersonStats(DataRowView ViewRow)
            {
                personId = ViewRow[U.PersonID_col].ToInt();
                firstName = ViewRow[U.FirstName_col].ToString();
                lastName = ViewRow[U.LastName_col].ToString();
                bornYear = ViewRow["BornYear"].ToInt();
                fatherId = ViewRow[U.FatherID_col].ToInt();
                motherId = ViewRow[U.MotherID_col].ToInt();
            }
            public void CopyPersonStats(PersonStats copyPersonStats)
            {
                personId = copyPersonStats.personId;
                firstName = copyPersonStats.firstName;
                middleName = copyPersonStats.middleName;
                lastName = copyPersonStats.lastName;
                marriedName = copyPersonStats.marriedName;
                bornYear = copyPersonStats.bornYear;
                diedYear = copyPersonStats.diedYear;
                fatherId = copyPersonStats.fatherId;
                motherId = copyPersonStats.motherId;
            }
        }
        private void CheckForDuplicatesLastNameByDate()
        {
            ArrayList personList = new ArrayList();
            DataTable personTbl = SQL.GetAllPersonsFirst(U.LastName_col);
            DataTable ViewTbl = DefineDataViewTable();
            foreach (DataRow person_row in personTbl.Rows)
            {
                string bornDate = SQL.GetBornDate(person_row, EVitalRecordType.eBirthMale, EVitalRecordType.eBirthFemale);
                if (!String.IsNullOrEmpty(bornDate))
                {
                    int bornYear = bornDate.BornYear();
                    if (person_row[U.Sex_col].ToChar() == 'F' && !String.IsNullOrEmpty(person_row[U.MarriedName_col].ToString()))
                    {
                        ViewTbl.Rows.Add(NewViewRow(person_row, ViewTbl.NewRow(), bornYear, person_row[U.MarriedName_col], person_row[U.FatherID_col], person_row[U.MotherID_col]));
                    }
                    else if (!String.IsNullOrEmpty(person_row[U.LastName_col].ToString()))
                    {
                        ViewTbl.Rows.Add(NewViewRow(person_row, ViewTbl.NewRow(), bornYear, person_row[U.LastName_col], person_row[U.FatherID_col], person_row[U.MotherID_col]));
                    }
                }
            }
            DataView dv = new DataView(ViewTbl);
            dv.Sort = "Firstname, LastName, BornYear ASC";
            PersonStats prevPersonStats = new PersonStats();
            foreach (DataRowView ViewRow in dv)
            {
                PersonStats personStats = new PersonStats(ViewRow);
                if (MayBeSamePerson(personStats, prevPersonStats))
                {
                    personList.Add(new LastNameTrail(personStats.lastName, personStats.firstName, personStats.bornYear));
                }
                prevPersonStats.CopyPersonStats(personStats);
            }
            PrintLastNameTrail(personList);
        }
        //****************************************************************************************************************************
        private bool MayBeSamePerson(PersonStats personStats, PersonStats prevPersonStats, bool checkName = true)
        {
            if (personStats.sex != prevPersonStats.sex)
            {
                return false;
            }
            if (checkName)
            {
                if (personStats.firstName != prevPersonStats.firstName || personStats.lastName != prevPersonStats.lastName)
                {
                    return false;
                }
            }
            if (personStats.bornYear > prevPersonStats.bornYear + 2 || prevPersonStats.bornYear > personStats.bornYear + 2)
            {
                return false;
            }
            if (personStats.diedYear != 0 && prevPersonStats.diedYear != 0)
            {
                if (personStats.diedYear > prevPersonStats.diedYear + 2 || prevPersonStats.diedYear > personStats.diedYear + 2)
                {
                    return false;
                }
            }
            if (!SameId(personStats.fatherId, prevPersonStats.fatherId) && !SameId(personStats.motherId, prevPersonStats.motherId))
            {
                return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        private bool SameId(int id,
                            int prevId)
        {
            if (id == 0 || prevId == 0)
            {
                return true;
            }
            return id == prevId;
        }
        //****************************************************************************************************************************
        private void CheckForDuplicatesSimilarNameByDate()
        {
            ArrayList personList = new ArrayList();
            DataTable personTbl = SQL.GetAllPersonsFirst(U.LastName_col);
            //DataTable personTbl = new DataTable();
            //SQL.GetPerson(personTbl, 17132);
            foreach (DataRow person_row in personTbl.Rows)
            {
                //string bornDate = SQL.GetBornDate(person_row, EVitalRecordType.eBirthMale, EVitalRecordType.eBirthFemale);
                int iPersonID = person_row[U.PersonID_col].ToInt();
                string bornDate, bornPlace, bornHome, diedDate, diedPlace, diedHome;
                SQL.GetBornDiedDate(iPersonID, person_row, out bornDate, out bornPlace, out bornHome, out diedDate, out diedPlace, out diedHome);
                if (String.IsNullOrEmpty(bornDate))
                {
                    continue;
                }
                int bornYear = bornDate.BornYear();
                int diedYear = diedDate.BornYear();
                int personId = person_row[U.PersonID_col].ToInt();

                if (!String.IsNullOrEmpty(person_row[U.LastName_col].ToString()))
                {
                    CheckForSimilarPersons(personList, person_row, bornYear, diedYear);
                }
                if (person_row[U.Sex_col].ToChar() == 'F' && !String.IsNullOrEmpty(person_row[U.MarriedName_col].ToString()))
                {
                    string marriedName = person_row[U.MarriedName_col].ToString();
                    person_row[U.MarriedName_col] = person_row[U.LastName_col];
                    person_row[U.LastName_col] = marriedName;
                    CheckForSimilarPersons(personList, person_row, bornYear, diedYear);
                }
            }
            PrintLastNameTrail(personList);
        }
        //****************************************************************************************************************************
        private void CheckForSimilarPersons(ArrayList personList, DataRow person_row, int personBornYear, int personDiedYear=0)
        {
            DataTable SimilarPerson_tbl = SQL.GetSimilarPersons(person_row);
            PersonStats personStats = new PersonStats(person_row, personBornYear, personDiedYear);
            if (personStats.firstName.Length == 1)
            {
                return;
            }
            int personId = person_row[U.PersonID_col].ToInt();

            foreach (DataRow similarRow in SimilarPerson_tbl.Rows)
            {
                //string similarBornDate = SQL.GetBornDate(similarRow, EVitalRecordType.eBirthMale, EVitalRecordType.eBirthFemale);
                int iSimilarPersonID = similarRow[U.PersonID_col].ToInt();
                string similarBornDate, bornPlace, bornHome, similarDiedDate, diedPlace, diedHome;
                SQL.GetBornDiedDate(iSimilarPersonID, similarRow, out similarBornDate, out bornPlace, out bornHome, out similarDiedDate, out diedPlace, out diedHome);
                if (String.IsNullOrEmpty(similarBornDate))
                {
                    continue;
                }
                int similarBornYear = similarBornDate.BornYear();
                int similarDiedYear = similarDiedDate.BornYear();
                int similarPersonId = similarRow[U.PersonID_col].ToInt();
                if (similarPersonId != personId)
                {
                    PersonStats prevPersonStats = new PersonStats(similarRow, similarBornYear, similarDiedYear);
                    if (MayBeSamePerson(personStats, prevPersonStats, false))
                    {
                        AddToList(personList, personStats, personBornYear);
                        AddToList(personList, prevPersonStats, similarBornYear, true);
                    }
                    else if (similarRow[U.Sex_col].ToChar() == 'F' && !String.IsNullOrEmpty(similarRow[U.MarriedName_col].ToString()))
                    {
                        prevPersonStats.lastName = similarRow[U.MarriedName_col].ToString();
                        if (MayBeSamePerson(personStats, prevPersonStats, false))
                        {
                            AddToList(personList, personStats, personBornYear);
                            AddToList(personList, prevPersonStats, similarBornYear, true);
                        }
                    }
                }
            }
        }
        //****************************************************************************************************************************
        private void AddToList(ArrayList personList,
                               PersonStats personStats,
                               int bornYear,
                               bool indentFirstname=false)
        {
            string firstName = (indentFirstname) ? "    " + personStats.firstName : personStats.firstName;
            if (!String.IsNullOrEmpty(personStats.middleName))
            {
                firstName += (" " + personStats.middleName);
            }
            string lastName = personStats.lastName;
            if (!String.IsNullOrEmpty(personStats.marriedName))
            {
                if (!String.IsNullOrEmpty(lastName))
                {
                    lastName += "-";
                }
                lastName += personStats.marriedName;
            }
            personList.Add(new LastNameTrail(lastName, firstName, bornYear));
        }
        //****************************************************************************************************************************
        private void CheckForInconsistantParentIds()
        {
            DataTable personTbl = SQL.GetAllPersonsFirst(U.LastName_col);
            foreach (DataRow personRow in personTbl.Rows)
            {
                int personId = personRow[U.PersonID_col].ToInt();
                int fatherId = personRow[U.FatherID_col].ToInt();
                int motherId = personRow[U.MotherID_col].ToInt();
                SQL.GetVitalRecordParents(personId);
            }
        }
        //****************************************************************************************************************************
        private void CheckMarriages()
        {
            ArrayList personList = new ArrayList();
            DataTable marriagesTbl = SQL.GetMarriageRecords();
            DataTable vitalRecordTbl = SQL.GetAllVitalRecords();
            int count = 0;
            int count1 = 0;
            foreach (DataRow vitalRecordRow in vitalRecordTbl.Rows)
            {
                int vitalRecordId = vitalRecordRow[U.VitalRecordID_col].ToInt();
                int personId = vitalRecordRow[U.PersonID_col].ToInt();
                int fatherId = vitalRecordRow[U.FatherID_col].ToInt();
                int motherId = vitalRecordRow[U.MotherID_col].ToInt();
                int spouseId = vitalRecordRow[U.SpouseID_col].ToInt();
                string selectStatement = "VitalRecordID = '" + spouseId + "'";
                DataRow[] vitalRecordResults = vitalRecordTbl.Select(selectStatement);
                if (vitalRecordResults.Length != 1)
                {
                    continue;
                }
                spouseId = vitalRecordResults[0][U.PersonID_col].ToInt();
                if (spouseId == 0)
                {
                    personList.Add(new LastNameTrail("SpouseId", spouseId.ToString(), vitalRecordId));
                    continue;
                }
                if (personId != 0 && spouseId != 0)
                {
                    selectStatement = "(PersonID = '" + personId + "' and SpouseId = '" + spouseId + "') or (PersonID = '" + spouseId + "' and SpouseId = '" + personId + "')";
                    DataRow[] results = marriagesTbl.Select(selectStatement);
                    if (results.Length != 1)
                    {
                        string lastName = vitalRecordRow[U.LastName_col].ToString();
                        string firstName = vitalRecordRow[U.FirstName_col].ToString();
                            personList.Add(new LastNameTrail(lastName, firstName, vitalRecordId));
                    }
                    else 
                    {
                        DataRow marriageRow = results[0];
                        string oldDateMarried = marriageRow[U.DateMarried_col].ToString();
                        string dateMarried = U.BuildDate(vitalRecordRow[U.DateYear_col].ToInt(), vitalRecordRow[U.DateMonth_col].ToInt(), vitalRecordRow[U.DateDay_col].ToInt());
                        if (String.IsNullOrEmpty(marriageRow[U.DateMarried_col].ToString()))
                        {
                            marriageRow[U.DateMarried_col] = dateMarried;
                            count++;
                        }
                        else if (oldDateMarried != dateMarried)
                        {
                            //marriageRow[U.DateMarried_col] = dateMarried;
                            count1++;
                        }
                    }
                }
            }
            string[] sKeyColumns = new string[] { U.PersonID_col, U.SpouseID_col };
            //SQL.UpdateWithDA(marriagesTbl, U.Marriage_Table, sKeyColumns, new ArrayList(new string[] { U.DateMarried_col }));
        }
        //****************************************************************************************************************************
        private void CheckForDuplicatesLastNameSpouse()
        {
            DataTable personTbl = SQL.GetAllPersonsFirst(U.LastName_col);
            ArrayList personList = new ArrayList();
            foreach (DataRow person_row in personTbl.Rows)
            {
                int personId = person_row[U.PersonID_col].ToInt();
                DataTable personMarriage_tbl = SQL.GetSpouseNames(personId);
                if (personMarriage_tbl.Rows.Count == 0)
                {
                    continue;
                }
                if (DuplicateSpouses(personMarriage_tbl))
                {
                    string lastName = person_row[U.LastName_col].ToString();
                    if (String.IsNullOrEmpty(lastName))
                    {
                        lastName = person_row[U.MarriedName_col].ToString();
                    }
                    personList.Add(new LastNameTrail(lastName, person_row[U.FirstName_col].ToString(), personId));
                    continue;
                }
            }
            PrintLastNameTrail(personList);
        }
        //****************************************************************************************************************************
        private bool DuplicateSpouses(DataTable spouseTbl)
        {
            foreach (DataRow spouseRow in spouseTbl.Rows)
            {
                int spouseID = spouseRow[U.PersonID_col].ToInt();
                string personLastName = spouseRow[U.LastName_col].ToString();
                string personMarriedName = spouseRow[U.MarriedName_col].ToString();
                foreach (DataRow otherSpouseRow in spouseTbl.Rows)
                {
                    if (otherSpouseRow[U.PersonID_col].ToInt() != spouseID)
                    {
                        if (CheckAllSimilarSpouses(spouseRow, otherSpouseRow, spouseID))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool CheckAllSimilarSpouses(DataRow spouseRow, DataRow otherSpouseRow, int spouseId)
        {
            DataTable SimilarPerson_tbl = SQL.GetSimilarPersons(otherSpouseRow);
            foreach (DataRow SimilarPerson_row in SimilarPerson_tbl.Rows)
            {
                int otherSpouseId = SimilarPerson_row[U.PersonID_col].ToInt();
                if (otherSpouseId == spouseId)
                {
                    continue;
                }
                if (SameName(spouseRow[U.FirstName_col].ToString(), SimilarPerson_row[U.FirstName_col].ToString()))
                {
                    if (SameLastName(spouseRow, SimilarPerson_row))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool SameLastName(DataRow personRow, DataRow otherPersonRow)
        {
            string personLastName = personRow[U.LastName_col].ToString();
            string personMarriedName = personRow[U.MarriedName_col].ToString();
            string otherPersonLastName = otherPersonRow[U.LastName_col].ToString();
            string otherPersonMarriedName = otherPersonRow[U.MarriedName_col].ToString();
            if (SameName(personLastName, otherPersonLastName))
            {
                return true;
            }
            if (SameName(personLastName, otherPersonMarriedName))
            {
                return true;
            }
            if (SameName(personMarriedName, otherPersonLastName))
            {
                return true;
            }
            if (SameName(personMarriedName, otherPersonMarriedName))
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool SameName(string personName,
                              string otherPersonName)
        {
            if (string.IsNullOrEmpty(personName) || string.IsNullOrEmpty(otherPersonName))
            {
                return false;
            }
            return (personName == otherPersonName);
        }
        //****************************************************************************************************************************
        private void AddIfNotYetInList(ArrayList personList,
                                       DataTable personMarriage_tbl,
                                       string firstName,
                                       string lastName,
                                       int otherPersonId)
        {
            DataTable spouseMarriageTbl = SQL.GetSpouseNames(otherPersonId);

            bool found = false;
            foreach (LastNameTrail personTrail in personList)
            {
                if (personTrail.bornYear.ToInt() == otherPersonId)
                {
                    found = true;
                    continue;
                }
            }
            if (!found)
            {
                personList.Add(new LastNameTrail(lastName, firstName, otherPersonId));
            }
        }
        //****************************************************************************************************************************
        private DataRow NewViewRow(DataRow person_row, DataRow viewRow, int bornYear, object lastName, object fatherId, object motherId)
        {
            viewRow[U.PersonID_col] = person_row[U.PersonID_col];
            viewRow[U.FirstName_col] = person_row[U.FirstName_col];
            viewRow[U.LastName_col] = lastName;
            viewRow["BornYear"] = bornYear;
            viewRow[U.FatherID_col] = fatherId;
            viewRow[U.MotherID_col] = motherId;
            return viewRow;
        }
        //****************************************************************************************************************************
        private void PrintLastNameTrail(ArrayList personList)
        {
            StreamWriter streamWriter = new StreamWriter(@"C:\Temp\PrintDuplicates.txt");
            foreach (LastNameTrail person in personList)
            {
                string bornYear = (person.bornYear.ToInt() == 0) ? "" : person.bornYear;
                streamWriter.WriteLine(person.firstname + " " + person.lastname + " " + bornYear);

            }
            streamWriter.Close();
        }
        //****************************************************************************************************************************
        private DataTable DefineDataViewTable()
        {
            DataTable tbl = new DataTable(U.Person_Table);
            tbl.Columns.Add(U.PersonID_col, typeof(int));
            tbl.Columns.Add(U.FirstName_col, typeof(string));
            tbl.Columns.Add(U.LastName_col, typeof(string));
            tbl.Columns.Add("BornYear", typeof(int));
            tbl.Columns.Add(U.FatherID_col, typeof(int));
            tbl.Columns.Add(U.MotherID_col, typeof(int));
            tbl.Columns[U.FirstName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.LastName_col].MaxLength = U.iMaxNameLength;
            return tbl;
        }
        //****************************************************************************************************************************
        private void CheckForDuplicates()
        {
            CheckForDuplicates(SQL.GetAllPersonsFirst(U.LastName_col), U.LastName_col, true);
            CheckForDuplicates(SQL.GetAllPersonsFirst(U.MarriedName_col), U.MarriedName_col, false);
        }
        //****************************************************************************************************************************
        private void CheckForDuplicates(DataTable personTbl, string LastName_col, bool checkAllStats)
        {
            DataTable personTblOrderByLast = null;
            if (!checkAllStats)
            {
                personTblOrderByLast = SQL.GetAllPersonsOrderBy(U.LastName_col);
            }
            DataRow previousPerson = personTbl.Rows[0];
            string previousBornDate = "xxxxxxxx";
            string prevfirstName = "xxxxxx";
            ArrayList tmp = new ArrayList();
            ArrayList tmp1 = new ArrayList();
            foreach (DataRow person_row in personTbl.Rows)
            {
                string firstName = person_row[U.FirstName_col].ToString();
                string bornDate = SQL.GetBornDate(person_row, EVitalRecordType.eBirthMale, EVitalRecordType.eBirthFemale);
                if (checkAllStats)
                {
                    CheckAllPersonStats(person_row, bornDate, tmp, tmp1);
                }
                /*
                if (IsPersonToCheck(person_row, previousPerson, firstName, prevfirstName, LastName_col))
                {
                    if (SameLastName(person_row, previousPerson, LastName_col))
                    {
                        if (!String.IsNullOrEmpty(bornDate))
                        {
                            int bornYear = bornDate.BornYear();
                            int prevBornYear = previousBornDate.BornYear();
                            if (bornYear <= prevBornYear + 2 && prevBornYear <= bornYear + 2)
                            {
                                count++;
                            }
                        }
                    }
                    if (personTblOrderByLast != null)
                    {
                        CheckMaidenMarried(personTblOrderByLast, firstName, person_row[U.MarriedName_col].ToString(), bornDate);
                    }
                }
                */
                prevfirstName = firstName;
                previousBornDate = bornDate;
                previousPerson = person_row;
            }
        }
        //****************************************************************************************************************************
        private bool IsPersonToCheck(DataRow person_row, DataRow previousPerson, string firstName, string prevfirstName, string LastName_col)
        {
            if (String.IsNullOrEmpty(firstName) || firstName.Length <= 1)
            {
                return false;
            }
            if (firstName != prevfirstName)
            {
                return false;
            }
            if (UnknownName(firstName))
            {
                return false;
            }
            if (String.IsNullOrEmpty(person_row[LastName_col].ToString()))
            {
                return false;
            }
            if (person_row[U.Sex_col].ToString() != previousPerson[U.Sex_col].ToString())
            {
                return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        private void CheckMaidenMarried(DataTable personTblOrderByLast, string firstname, string lastName, string bornDate)
        {
            lastName = lastName.Replace("'", "''");
            string selectStatement = "FirstName = '" + firstname + "' and Lastname = '" + lastName + "'";
            DataRow[] foundRows = personTblOrderByLast.Select(selectStatement);
            if (foundRows.Length != 0)
            {
                foreach (DataRow LastDateRow in foundRows)
                {
                    if (!String.IsNullOrEmpty(bornDate))
                    {
                        int bornYear = bornDate.BornYear();
                        int prevBornYear = LastDateRow[U.BornDate_col].ToString().BornYear();
                        if (bornYear <= prevBornYear + 2 && prevBornYear <= bornYear + 2)
                        {
                        }
                    }
                }
            }
        }
        //****************************************************************************************************************************
        private void CheckAllPersonStats(DataRow person_row, string bornDate, ArrayList tmp, ArrayList tmp1)
        {
            if (!String.IsNullOrEmpty(person_row[U.LastName_col].ToString()) && person_row[U.LastName_col].ToString().Length < 3)
            {
            }
            if (person_row[U.Sex_col].ToString() == "M" && !String.IsNullOrEmpty(person_row[U.MarriedName_col].ToString()))
            {
            }
            if (person_row[U.LastName_col].ToString() == person_row[U.MarriedName_col].ToString())
            {
            }
            if (CensusBornDateNotEntered(person_row, bornDate))
            {
                if (String.IsNullOrEmpty(person_row[U.LastName_col].ToString()))
                {
                    tmp.Add(person_row[U.MarriedName_col]);
                }
                else
                {
                    tmp.Add(person_row[U.LastName_col]);
                }
                tmp1.Add(person_row[U.FirstName_col]);
            }
        }
        //****************************************************************************************************************************
        private bool CensusBornDateNotEntered(DataRow person_row, string bornDate)
        {
            if (person_row[U.Census1860_col].ToInt() == 0)
            {
                return false;
            }
            if (!String.IsNullOrEmpty(person_row[U.BornDate_col].ToString()))
            {
                return false;
            }
            if (person_row[U.BornSource_col].ToString() == "No Age in Census")
            {
                return false;
            }
            if (string.IsNullOrEmpty(bornDate))
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool SameLastName(DataRow person_row, DataRow previousPerson, string LastName_col)
        {
            string lastName = person_row[LastName_col].ToString();
            string prevLastName = previousPerson[LastName_col].ToString();
            if (!String.IsNullOrEmpty(lastName) && lastName == prevLastName)
            {
                return true;
            }
            if (person_row[U.Sex_col].ToString() != "F")
            {
                return false;
            }
            string marriedName = person_row[U.MarriedName_col].ToString();
            string prevMarriedName = previousPerson[U.MarriedName_col].ToString();
            if (!String.IsNullOrEmpty(marriedName) && marriedName == prevMarriedName)
            {
                return true;
            }
            if (!String.IsNullOrEmpty(lastName) && lastName == prevMarriedName)
            {
                return true;
            }
            if (!String.IsNullOrEmpty(marriedName) && marriedName == prevMarriedName)
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool UnknownName(string firstName)
        {
            if (firstName.ToLower() == "unknown")
            {
                return true;
            }
            if (firstName.ToLower() == "baby")
            {
                return true;
            }
            if (firstName.ToLower() == "not named")
            {
                return true;
            }
            if (firstName.ToLower() == "infant")
            {
                return true;
            }
            if (firstName.ToLower() == "stillborn")
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        private void ImportCemeteries()
        {
            //CImportCemeteryWorkbook ImportCemeteryWorkbook = new CImportCemeteryWorkbook(ref m_SQL);
        }
        private class RecordTrail
        {
            public string lastname;
            public string OriginalLastName;
            public string firstname;
            public string recordId;
            public string personId;
            public string spouseId;
            public string fatherId;
            public string motherId;
            public RecordTrail(object lastname,
                               object firstname,
                               object OriginalLastName,
                               object recordId,
                               object personId,
                               object fatherId,
                               object motherId,
                               object spouseId)
            {
                this.lastname = lastname.ToString();
                this.OriginalLastName = OriginalLastName.ToString();
                this.firstname = firstname.ToString();
                this.recordId = recordId.ToString();
                this.personId = personId.ToString();
                this.fatherId = fatherId.ToString();
                this.motherId = motherId.ToString();
                this.spouseId = (spouseId == null) ? "" : spouseId.ToString();
            }
        }
        //****************************************************************************************************************************
        private void IntegrateCemeteryRecords()
        {
            ArrayList personList = new ArrayList();
            DataTable CemeteryRecord_tbl = SQL.GetAllCemeteryRecords();
            //DataTable CemeteryRecord_tbl = SQL.DefineCemeteryRecordTable();
            //SQL.GetCemeteryRecord(CemeteryRecord_tbl, 342);
            int rowIndex = 0;
            ArrayList recordTrailList = new ArrayList();
            foreach (DataRow cemeteryRecord_row in CemeteryRecord_tbl.Rows)
            {
                rowIndex++;
                string cemeteryBornDate = U.GetCemeteryBornDate(cemeteryRecord_row, "");
                ////if (cemeteryRecord_row[U.PersonID_col].ToInt() == 0 && !String.IsNullOrEmpty(cemeteryBornDate) && cemeteryBornDate.Length >= 4)
                if (cemeteryRecord_row[U.PersonID_col].ToInt() == 0)
                {
                    Integrate(cemeteryRecord_row[U.CemeteryRecordID_col].ToInt(), personList);
                }
            }
            PrintArraylist(personList);
        }
        //****************************************************************************************************************************
        private void Integrate(int cemeteryRecordID, ArrayList recordTrailList)
        {
            DataTable CemeteryRecord_tbl = SQL.DefineCemeteryRecordTable();
            SQL.GetCemeteryRecord(CemeteryRecord_tbl, cemeteryRecordID);
            if (CemeteryRecord_tbl.Rows.Count == 0)
            {
                return;
            }
            DataRow cemeteryRecord_row = CemeteryRecord_tbl.Rows[0];
            CIntegrateCemeteryRecord IntegrateCemeteryRecord = new CIntegrateCemeteryRecord(m_SQL, false);
            bool personIntegrated = cemeteryRecord_row[U.PersonID_col].ToInt() != 0;
            bool spouseIntegrated = cemeteryRecord_row[U.SpouseID_col].ToInt() != 0;
            bool fatherIntegrated = cemeteryRecord_row[U.FatherID_col].ToInt() != 0;
            bool motherIntegrated = cemeteryRecord_row[U.MotherID_col].ToInt() != 0;
            string originalLastName = cemeteryRecord_row[U.LastName_col].ToString();
            if (IntegrateCemeteryRecord.IntegrateRecord(cemeteryRecord_row, personIntegrated, fatherIntegrated, motherIntegrated, spouseIntegrated))
            {
                if (!SQL.SaveIntegratedCemeteryRecords(CemeteryRecord_tbl))
                {
                    MessageBox.Show("Integrate Unsuccesful");
                }
                else
                {
                    recordTrailList.Add(new RecordTrail(cemeteryRecord_row[U.LastName_col], cemeteryRecord_row[U.FirstName_col], originalLastName, 
                                        cemeteryRecord_row[U.CemeteryRecordID_col], cemeteryRecord_row[U.PersonID_col], cemeteryRecord_row[U.FatherID_col], cemeteryRecord_row[U.MotherID_col], cemeteryRecord_row[U.SpouseID_col]));
                }
            }
        }
        //****************************************************************************************************************************
        private void IntegrateVitalRecords()
        {
            ArrayList personList = new ArrayList();
            DataTable VitalRecord_tbl = SQL.GetAllVitalRecords();
            //DataTable VitalRecord_tbl = SQL.DefineVitalRecord_Table();
            //SQL.GetVitalRecord(VitalRecord_tbl, 10696);
            int rowIndex = 0;
            ArrayList recordTrailList = new ArrayList();
            foreach (DataRow vitalRecord_row in VitalRecord_tbl.Rows)
            {
                rowIndex++;
                if (vitalRecord_row[U.PersonID_col].ToInt() == 0)
                {
                    EVitalRecordType eVitalRecordType = (EVitalRecordType)vitalRecord_row[U.VitalRecordType_col].ToInt();
                    //if (eVitalRecordType.MarriageRecord())
                    //if (eVitalRecordType.IsBirthOrDeathRecord() && !UnknownName(vitalRecord_row[U.FirstName_col].ToString()))
                    //if (eVitalRecordType == EVitalRecordType.eBurial && !UnknownName(vitalRecord_row[U.FirstName_col].ToString()))
                    {
                        IntegrateVitalRecs(vitalRecord_row[U.VitalRecordID_col].ToInt(), personList);
                    }
                }
                //if (personList.Count > 20) break;
            }
            PrintArraylist(personList);
            MessageBox.Show("Bulk Integrate Complete");
        }
        //****************************************************************************************************************************
        private void PrintArraylist(ArrayList personList)
        {
            StreamWriter streamWriter = new StreamWriter(@"C:\Temp\PrintReport.txt");
            string comma = ", ";
            foreach (RecordTrail person in personList)
            {
                streamWriter.WriteLine(person.firstname + " " + person.lastname + " " + person.OriginalLastName + comma + person.recordId + comma +
                                       person.personId + comma + person.fatherId + comma + person.motherId + comma + person.spouseId);

            }
            streamWriter.Close();
        }
        //****************************************************************************************************************************
        private void IntegrateVitalRecs(int vitalRecordID, ArrayList recordTrailList)
        {
            DataTable VitalRecord_tbl = SQL.DefineVitalRecord_Table();
            SQL.GetVitalRecord(VitalRecord_tbl, vitalRecordID);
            if (VitalRecord_tbl.Rows.Count == 0)
            {
                return;
            }
            DataRow VitalRecord_row = VitalRecord_tbl.Rows[0];
            DataRow SpouseVitalRecord_row = null;
            EVitalRecordType vitalRecordType = (EVitalRecordType)VitalRecord_row[U.VitalRecordType_col].ToInt();
            bool spouseIntegrated = false;
            bool spouseFatherIntegrated = false;
            bool spouseMotherIntegrated = false;
            if (U.MarriageRecord(vitalRecordType))
            {
                int spouseId = VitalRecord_row[U.SpouseID_col].ToInt();
                if (spouseId == 0)
                {
                    return;
                }
                SQL.GetVitalRecordsForPerson(VitalRecord_tbl, spouseId, U.VitalRecordID_col);
                if (VitalRecord_tbl.Rows.Count < 2)
                {
                    return;
                }
                SpouseVitalRecord_row = VitalRecord_tbl.Rows[1];
                spouseIntegrated = SpouseVitalRecord_row[U.PersonID_col].ToInt() != 0;
                spouseFatherIntegrated = SpouseVitalRecord_row[U.FatherID_col].ToInt() != 0;
                spouseMotherIntegrated = SpouseVitalRecord_row[U.MotherID_col].ToInt() != 0;
            }
            CIntegrateVitalRecord IntegrateVitalRecord = new CIntegrateVitalRecord(m_SQL, false);
            bool personIntegrated = VitalRecord_row[U.PersonID_col].ToInt() != 0;
            bool fatherIntegrated = VitalRecord_row[U.FatherID_col].ToInt() != 0;
            bool motherIntegrated = VitalRecord_row[U.MotherID_col].ToInt() != 0;
            string originalLastName = VitalRecord_row[U.LastName_col].ToString();
            if (IntegrateVitalRecord.IntegrateRecord(VitalRecord_row, SpouseVitalRecord_row, personIntegrated, fatherIntegrated, motherIntegrated, spouseIntegrated, spouseFatherIntegrated, spouseMotherIntegrated))
            {
                if (!SQL.SaveIntegratedVitalRecords(VitalRecord_tbl))
                {
                    MessageBox.Show("Integrate Unsuccesful");
                }
                else
                {
                    recordTrailList.Add(new RecordTrail(VitalRecord_row[U.LastName_col], VitalRecord_row[U.FirstName_col], originalLastName,
                                                        VitalRecord_row[U.VitalRecordID_col], VitalRecord_row[U.PersonID_col], VitalRecord_row[U.FatherID_col], VitalRecord_row[U.MotherID_col], 0));
                }
            }
        }
        //****************************************************************************************************************************
        private void CheckCemeteryRecords()
        {
            int count = 0;
            int onecount = 0;
            int lowestID = 999999;
            DataTable cemeteryRecord_tbl = SQL.GetAllCemeteryRecords();
            foreach (DataRow cemeteryRecord_row in cemeteryRecord_tbl.Rows)
            {
                DataTable person_tbl = SQL.GetPersonByFirstLastNames(cemeteryRecord_row[U.FirstName_col].ToString(), cemeteryRecord_row[U.LastName_col].ToString());
                if (person_tbl.Rows.Count == 1)
                {
                    int personID = person_tbl.Rows[0][U.PersonID_col].ToInt();
                    if (personID > 9235)
                    {
                        if (cemeteryRecord_row[U.PersonID_col].ToInt() != personID)
                        {
                            onecount++;
                        }
                    }
                }
                else
                {
                    foreach (DataRow person_row in person_tbl.Rows)
                    {
                        int personID = person_row[U.PersonID_col].ToInt();
                        if (personID > 9235)
                        {
                            if (personID < lowestID)
                            {
                                lowestID = personID;
                            }
                            count++;
                        }
                    }
                }
            }
        }
        private const string home = ", home ";
        private const string buildings = ", buildings ";
        private const string houseDo = ", house do";
        private const string house = ", house ";
        private const string houseAnd = ", house and ";
        private const string houseAndLot = ", house and lot";
        private const string houseAndLotOn = ", house and lot on ";
        private const string gazetteerString = "1884 Gazetteer ";
        //****************************************************************************************************************************
        private void GetVitalRecordsToIntegrate()
        {
            DataTable person_tbl = SQL.GetAllPersons();
            DataTable father_tbl = new DataTable();
            foreach (DataRow person_row in person_tbl.Rows)
            {
                string lastName = person_row[U.LastName_col].ToString();
                string firstName = person_row[U.FirstName_col].ToString();
                father_tbl.Rows.Clear();
                if (SQL.GetMultipleVitalRecords(father_tbl, lastName, firstName) > 4)
                {
                }
            }
        }
        //****************************************************************************************************************************
        private void RepairCemeteryRecords()
        {
            int count = 0;
            int lowestID = 999999;
            DataTable person_tbl = SQL.GetAllPersons();
            foreach (DataRow person_row in person_tbl.Rows)
            {
                DataTable cemeteryPerson_tbl = SQL.DefineCemeteryRecordTable();
                string lastname = String.IsNullOrEmpty(person_row[U.LastName_col].ToString()) ? person_row[U.MarriedName_col].ToString() : person_row[U.LastName_col].ToString();
                SQL.GetCemeteryRecordLastNameFirstname(cemeteryPerson_tbl, lastname, person_row[U.FirstName_col].ToString());
                foreach (DataRow cemeteryPerson_row in cemeteryPerson_tbl.Rows)
                {
                    if (cemeteryPerson_row[U.PersonID_col].ToInt() == 0)
                    {
                        int personID = person_row[U.PersonID_col].ToInt();
                        if (personID > 9235)
                        {
                            if (personID < lowestID)
                            {
                                lowestID = personID;
                            }
                            count++;
                        }
                    }
                }
            }
        }     
        //****************************************************************************************************************************
        private void UpdateMultipleMariages_click(object sender, System.EventArgs e)
        {
            SQL.UpdateMultipleMarriages();
        }
        //****************************************************************************************************************************
        public void GetPhotosInList()
        {
            string path = SQL.m_sDataDirectory + "\\NoNames.txt";
            if (!File.Exists(path))
            {
                return;
            }
            DataTable slideShowTbl = SQL.DefineSlideShowTable();
            int photoSequence = 0;
            using (StreamReader file = new StreamReader(path))
            {
                while (file.Peek() >= 0)
                {
                    string sPhotoName = file.ReadLine();
                    int photoID = SQL.GetPhotoID(sPhotoName);
                    if (photoID == 0)
                    {
                    }
                    else if (!PhotoAlreadyInTable(slideShowTbl, photoID))
                    {
                        DataRow row = slideShowTbl.NewRow();
                        row[U.PhotoID_col] = photoID;
                        row[U.PhotoSequence_col] = photoSequence;
                        slideShowTbl.Rows.Add(row);
                    }
                    photoSequence++;
                }
            }
            SqlCommand insertCommand = SQL.InsertCommand(null, slideShowTbl, U.SlideShow_Table, false);
            SQL.InsertWithDA(slideShowTbl, insertCommand);
        }
        //****************************************************************************************************************************
        private bool PhotoAlreadyInTable(DataTable slideShowTbl, int photoId)
        {
            foreach (DataRow slideShowRow in slideShowTbl.Rows)
            {
                int rowPhotoId = slideShowRow[U.PhotoID_col].ToInt();
                if (photoId == rowPhotoId)
                {
                    return true;
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        private void ImportAllSchoolRecords()
        {
            CImportSchoolRecords Import = new CImportSchoolRecords(m_SQL, SQL.DataDirectory());
            Import.GetSchoolRecordsForAllFiles();
        }
        //****************************************************************************************************************************
        private void CopyAllCensusRecords()
        {
            ImportExportEPPlus importExportEPPlus = new ImportExportEPPlus(m_SQL, SQL.DataDirectory());
            importExportEPPlus.CopyCensusInfo();
        }
        //****************************************************************************************************************************
        private void RestoreCensusRecords()
        {
            ImportExportEPPlus importExportEPPlus = new ImportExportEPPlus(m_SQL, SQL.DataDirectory());
            importExportEPPlus.RestoreCensusInfo();
        }
        //****************************************************************************************************************************
        private void ImportGrandListClick(object sender, System.EventArgs e)
        {
            CImportGrandList cImportGrandList = new CImportGrandList(m_SQL, SQL.DataDirectory());
            cImportGrandList.ImportGrandList();
        }
        //****************************************************************************************************************************
        private void ImportGrandListHistory()
        {
            CImportGrandList cImportGrandList = new CImportGrandList(m_SQL, SQL.DataDirectory());
            cImportGrandList.ImportGrandListHistory();
        }
        //****************************************************************************************************************************
        private void CheckMarriageBirthDateWithBirthDate()
        {
            int count = 0;
            ArrayList numDiffs = new ArrayList();
            DataTable personTable = SQL.GetAllPersons();
            foreach (DataRow Person_row in personTable.Rows)
            {
                DataTable VitalRecord_tbl = SQL.DefineVitalRecord_Table();
                SQL.GetVitalRecordsForPerson(VitalRecord_tbl, Person_row[U.PersonID_col].ToInt(), U.PersonID_col);
                foreach (DataRow VitalRecord_row in VitalRecord_tbl.Rows)
                {
                    if (MarriageRecordHasBornAge(VitalRecord_row))
                    {
                        count++;
                        CompareMarriageBirthDate(VitalRecord_tbl, Person_row, VitalRecord_row, ref numDiffs);
                    }
                }
            }
        }
        //****************************************************************************************************************************
        private bool MarriageRecordHasBornAge(DataRow VitalRecord_row)
        {
            EVitalRecordType vitalRecordType = (EVitalRecordType)VitalRecord_row[U.VitalRecordType_col].ToInt();
            if (!vitalRecordType.IsMarriageRecord())
            {
                return false;
            }
            return (VitalRecord_row[U.AgeYears_col].ToInt() != 0 ||
                    VitalRecord_row[U.AgeMonths_col].ToInt() != 0 ||
                    VitalRecord_row[U.AgeDays_col].ToInt() != 0);
        }
        //****************************************************************************************************************************
        private void CompareMarriageBirthDate(DataTable VitalRecord_tbl, DataRow Person_row, DataRow VitalRecord_row, ref ArrayList numDiffs)
        {
            int personId = Person_row[U.PersonID_col].ToInt();
            DataTable CemeteryRecord_tbl = SQL.DefineVitalRecord_Table();
            SQL.GetCemeteryRecordForPerson(CemeteryRecord_tbl, personId);
            string birthDate = "";
            string place = "";
            string home = "";
            string book = "";
            string page = "";
            string source = "";
            bool verified = U.GetPersonVitalStatistics(VitalRecord_tbl, CemeteryRecord_tbl, Person_row, EVitalRecordType.eBirthMale, EVitalRecordType.eBirthFemale,
                                                                ref book, ref page, ref birthDate, ref place, ref home, ref source, false);
            if (String.IsNullOrEmpty(birthDate))
            {
                return;
            }
            if (birthDate.Length < 4)
            {
                MessageBox.Show("Invalid Date: " +  birthDate);
                return;
            }
            string marriageBirthDate = U.ReturnIndirectBirthDate(VitalRecord_row, birthDate);
            if (marriageBirthDate.Length < 4)
            {
                MessageBox.Show("Invalid Date: " + marriageBirthDate);
                return;
            }
            int dateDiff = birthDate.Substring(0, 4).ToInt() - marriageBirthDate.Substring(0, 4).ToInt();
            if (Math.Abs(dateDiff) > 3)// && !KnownDifferences(personId))
            {
                 numDiffs.Add(dateDiff);
            }
        }
        //****************************************************************************************************************************
        private bool KnownDifferences(int personId)
        {
            switch (personId)
            {
                case 920: return true;
                case 1536: return true;
                case 1577: return true;
                case 3375: return true;
                case 3859: return true;
                case 3940: return true;
                case 4737: return true;
                case 5272: return true;
                case 5314: return true;
                case 5528: return true;
                case 5735: return true;
                case 5824: return true;
                case 6212: return true;
                case 6345: return true;
                case 6548: return true;
                case 7027: return true;
                case 7251: return true;
                case 7387: return true;
                case 8896: return true;
                case 10875: return true;
                default: return false;
            }
        }
        //****************************************************************************************************************************
        private void TestClick(object sender, System.EventArgs e)
        {
            CheckMarriageBirthDateWithBirthDate();

            //SQL.CheckSchoolRecordsBirthDate();
            //SQL.CheckCensusBirthDate();
            //SQL.CheckForDuplicateBuildingValues();
            //QRCodes qrCodes = new QRCodes();
            //SQL.CheckVitalRecordParents();
            //SQL.CheckSchoolRecordDates();

            //IntegrateVitalRecords();
            //RestoreCensusRecords();
            //CopyAllCensusRecords();

            //ImportAllSchoolRecords();
            //SQL.CheckVitalRecordsFirstname();
            //SQL.CreateFirstNameTable();
            //SQL.SetPersonNameToIntegratedName();
            //SQL.MergeAlternativeSpellingsTables();
            //CImportNicknames Import = new CImportNicknames(m_SQL, SQL.DataDirectory());

            //SQL.MovePeoplePhotos();
            //GetPhotosInList();
            //ImportGrandListHistory();

            //CheckMarriedNameAgainstFathersLastName();
            //AddPreviousSpouse();

            //DuplicateDescriptions();
            //CheckForDuplicatesSimilarNameByDate();
            //CheckForDuplicatesLastNameByDate();
            //CheckForDuplicatesLastNameSpouse();
            //CheckVitalRecordIds();
            //CheckForCircularReferences();
            //SQL.CheckPersonMaidenMarriedName();
            //CheckForDuplicates();
            //CheckMarriages();
            //SQL.FixPersonSource();

            //CheckForInconsistantParentIds();

            //GetVitalRecordsToIntegrate();
            //IntegrateCemeteryRecords();
            //SQL.CheckPersonSex();
            //SQL.CreateFirstNameTable();
            //CheckMaleSex();
            //CheckNameAndSexPerson();
            //CheckCemeteryRecords();
            //ImportCemeteries();
            //SQL.SetBurialAgeFromDeathRecord();
            //TestPersonBuildings();
            //CheckedPicturedElements(U.PicturedPerson_Table, U.PicturedPersonNumber_col);
            //CheckedPicturedElements(U.PicturedBuilding_Table, U.PicturedBuildingNumber_col);
            //RenamePhotos();
            //CImportModernRoadValues Import = new CImportModernRoadValues(m_SQL, SQL.DataDirectory());
            //ConvertDates();
            //CheckFemaleDeathRecords();
            //CheckMarriageRecordsForDate();
            //SQL.RemoveLastNameWhenSameSaMarriedName();
            ////////SQL.CheckSexForAllVitalRecord();
            //SQL.SetFirstNameSexToValueInFirstnameTable(U.Person_Table, U.PersonID_col);
            //SQL.SetFirstNameSexToValueInFirstnameTable(U.VitalRecord_Table, U.VitalRecordID_col);
            //SQL.CheckSpouseLastNames();

            //SQL.GrandListUpperToLower();
            //SQL.checkAllFatherMother();
            //SQL.CheckForMarriageDuplicates();
            //SQL.CheckPrimaryBuildingValue();
            //SQL.CheckDuplicatePicturedPeople();
            //SQL.CheckDuplicatePicturedBuildings();
            //SQL.RemoveJPGExtension();
            //CImportGrandList Import = new CImportGrandList(m_SQL, SQL.DataDirectory());
            //SQL.RemoveIntegratedIDIfPersonDoesNotExist();
            //SQL.RemoveMarriagesWithoutAssociatedPersonRecords();
            //if (!SQL.SetAllBlankMarriageDateWithVitalRecordDates())
            //                MessageBox.Show("Save Unsuccesful");
            //SQL.UpdateVitalRecordSex();
            //ShowAllPersonsWhereSexIsBlank();
            //SQL.UpdateVitalRecordBurialParents();
            //SQL.UpdateVitalRecordMotherLastName();
            //SQL.CheckAllFemaleMarriedMaidenNames();
            //SQL.MoveModernRoads();
            //SQL.GrandListRoads();
            //SQL.CheckSexOfFatherAndMother();
            //SQL.CheckAllBuildingValueForOccupants();
            //MessageBox.Show("Test Complete");
        }
        //****************************************************************************************************************************
        private void SearchButton_Click(object sender, System.EventArgs e)
        {
            FVitalRecord VitalRecordBurial = new FVitalRecord(EVitalRecordType.eSearch, m_SQL);
            if (!VitalRecordBurial.AbortVitalRecord())
                VitalRecordBurial.ShowDialog();
        }
        //****************************************************************************************************************************
        private void AlternativeSpellingsFirstName_Click(object sender, System.EventArgs e)
        {
            CGridAlternativeSpellings GridAlternativeSpellings = new CGridAlternativeSpellings(U.AlternativeSpellingsFirstName_Table);
            GridAlternativeSpellings.ShowDialog();
        }
        //****************************************************************************************************************************
        private void AlternativeSpellingsLastName_Click(object sender, System.EventArgs e)
        {
            CGridAlternativeSpellings GridAlternativeSpellings = new CGridAlternativeSpellings(U.AlternativeSpellingsLastName_Table);
            GridAlternativeSpellings.ShowDialog();
        }
        //****************************************************************************************************************************
        private void MarriageBrideButton_Click(object sender, System.EventArgs e)
        {
            FVitalRecord VitalRecordCivilUnion = new FVitalRecord(EVitalRecordType.eMarriageBride, m_SQL);
            VitalRecordCivilUnion.ShowDialog();
        }
        //****************************************************************************************************************************
        private void MarriageGroomButton_Click(object sender, System.EventArgs e)
        {
            FVitalRecord VitalRecordCivilUnion = new FVitalRecord(EVitalRecordType.eMarriageGroom, m_SQL);
            VitalRecordCivilUnion.ShowDialog();
        }
        //****************************************************************************************************************************
        private void CivilUnionPartyAButton_Click(object sender, System.EventArgs e)
        {
            FVitalRecord VitalRecordCivilUnion = new FVitalRecord(EVitalRecordType.eCivilUnionPartyA, m_SQL);
            VitalRecordCivilUnion.ShowDialog();
        }
        //****************************************************************************************************************************
        private void CivilUnionPartyBButton_Click(object sender, System.EventArgs e)
        {
            FVitalRecord VitalRecordCivilUnion = new FVitalRecord(EVitalRecordType.eCivilUnionPartyB, m_SQL);
            VitalRecordCivilUnion.ShowDialog();
        }
        //****************************************************************************************************************************
        private void ShowMaps_Click(object sender, System.EventArgs e)
        {
            DataTable tbl = new DataTable(U.Map_Table);
            SQL.SelectAll(U.Map_Table, tbl);
            CGridPhoto GridPhoto = new CGridPhoto(ref m_SQL, ref tbl, U.Map_Table);
            GridPhoto.ShowDialog();
            int iCurrentPhoto = GridPhoto.SelectedRow;
            if (iCurrentPhoto >= 0)
            {
                string sMapFileName = SQL.MapFileName(tbl.Rows[iCurrentPhoto]);
                Bitmap HFPhoto = new Bitmap(sMapFileName);
                FPhotoFullSize PhotoFullSize = new FPhotoFullSize(HFPhoto);
                PhotoFullSize.ShowDialog();
            }
        }
        //****************************************************************************************************************************
        private void Template_Load(object sender, System.EventArgs e)
		{
		}
        //****************************************************************************************************************************
        private void Import_Click(object sender, System.EventArgs e)
        {
            CImportGedcom Import = new CImportGedcom(m_SQL, m_sDataDirectory);
        }
        //****************************************************************************************************************************
        private void ImportCemetery_click(object sender, System.EventArgs e)
        {
            CImportCemetery Import = new CImportCemetery(m_SQL, m_sDataDirectory);
        }
        //****************************************************************************************************************************
        private void CopyDatabase_Click(object sender, System.EventArgs e)
		{
            CCopyDatabase CopyDatabase = new CCopyDatabase();
            CopyDatabase.ShowDialog();
		}
        //****************************************************************************************************************************
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Dispose(true);
            this.Close();
        }
        //****************************************************************************************************************************
        private void AddMultiplsPhotos__Click(object sender, System.EventArgs e)
		{
            FImportPhotos ImportPhotos = new FImportPhotos(m_SQL);
            ImportPhotos.ShowDialog();
        }
        //****************************************************************************************************************************
        private void Exit_Click(object sender, System.EventArgs e)
		{
            Dispose(true);
            this.Close();
		}
        //****************************************************************************************************************************
    }
}
