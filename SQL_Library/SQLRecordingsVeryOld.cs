using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text;

namespace SQL_Library
{
    public static partial class SQL
    {
        public const string LatestExportedID_Table = "LatestExportedID";
        public const string IndividualTitles = "Individual Titles";
        public const string Recordings_Table = "Recordings";
        public const string RecordingsOld_Table = "RecordingsOld";
        public const string Movements_Table = "Movements";
        public const string PlayLists_Table = "PlayLists";
        public const string ID_col = "Id";
        public const string MovementID_col = "MovementId";
        public const string PreviousID_col = "PreviousID";
        public const string PreviousMovementID_col = "PreviousMovementID";
        public const string Composer_col = "Composer";
        public const string Album_col = "Album";
        public const string Title_col = "Title";
        public const string Artist_col = "Artist";
        public const string Genre_col = "Genre";
        public const string PlayList_col = "PlayList";
        public const string TrackNumber_col = "TrackNumber";
        public const string Extension_col = "Extension";
        public const string IPod_col = "IPod";
        public const int Exception = -999999999;
        public const string NoOrderBy = "";
        public const string MusicFolder = @"M:\Music";
        public const string AlacFolder = @"M:\MusicALAC";
        public const string MusicFolderWithSlash = @"M:\Music\";
        public const string AlacFolderWithSlash = @"M:\MusicALAC\";
        public const string WavExtension = "wav";
        public const string AlacExtension = "wav";
        public const string WavExtensionWithDot = ".wav";
        public const string AlacExtensionWithDot = ".m4a";
        public const string Mp3ExtensionWithDot = ".mp3";

        //****************************************************************************************************************************
        public static DataTable DefineRecordingsTable()
        {
            DataTable tbl = new DataTable(Recordings_Table);
            DataColumn Column = new DataColumn();
            tbl.Columns.Add(ID_col, typeof(int));
            tbl.Columns.Add(Composer_col, typeof(string));
            tbl.Columns.Add(Album_col, typeof(string));
            tbl.Columns.Add(Artist_col, typeof(string));
            tbl.Columns.Add(Genre_col, typeof(string));
            tbl.Columns.Add(Extension_col, typeof(string));
            tbl.Columns.Add(IPod_col, typeof(int));
            tbl.PrimaryKey = new DataColumn[] { tbl.Columns[ID_col] };
            SetRecordingsVarcharColumnsMaxLength(tbl);
            return tbl;
        }
        //****************************************************************************************************************************
        public static void SetRecordingsVarcharColumnsMaxLength(DataTable tbl)
        {
            tbl.Columns[Composer_col].MaxLength = 100;
            tbl.Columns[Album_col].MaxLength = 250;
            tbl.Columns[Artist_col].MaxLength = 250;
            tbl.Columns[Genre_col].MaxLength = 100;
            tbl.Columns[Extension_col].MaxLength = 10;
        }
        //****************************************************************************************************************************
        public static DataTable DefineMovementsTable()
        {
            DataTable tbl = new DataTable(Movements_Table);
            DataColumn Column = new DataColumn();
            tbl.Columns.Add(MovementID_col, typeof(int));
            tbl.Columns.Add(ID_col, typeof(int));
            tbl.Columns.Add(Title_col, typeof(string));
            tbl.Columns.Add(TrackNumber_col, typeof(int));
            tbl.PrimaryKey = new DataColumn[] { tbl.Columns[MovementID_col] };
            tbl.Columns[Title_col].MaxLength = 250;
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable DefinePlayListTable(string tableName)
        {
            DataTable tbl = new DataTable(tableName);
            DataColumn Column = new DataColumn();
            tbl.Columns.Add(ID_col, typeof(int));
            tbl.Columns.Add(PlayList_col, typeof(string));

            tbl.Columns[PlayList_col].MaxLength = 50;
            return tbl;
        }
        //****************************************************************************************************************************
        public static void DeleteAllRecordings()
        {
            DeleteWithParms(Recordings_Table);
            DeleteWithParms(Movements_Table);
        }
        //****************************************************************************************************************************
        public static int CreateRecording(DataTable recording_tbl)
        {
            try
            {
                SqlCommand insertCommand = InsertCommand(recording_tbl, Recordings_Table, true);
                InsertWithDA(recording_tbl, insertCommand);
                return recording_tbl.Rows.Count == 0 ? 0 : recording_tbl.Rows[0][ID_col].ToInt();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        //****************************************************************************************************************************
        public static void UpdatePlaylists(DataTable PlayLists_tbl)
        {
            SqlTransaction txn = sqlConnection.BeginTransaction();
            try
            {
                SqlCommand insertCommand = InsertCommand(txn, PlayLists_tbl, PlayLists_Table, false);
                SqlCommand deleteCommand = DeleteCommandWithDA(txn, PlayLists_tbl, PlayLists_Table, ID_col, PlayList_col);
                ArrayList fieldValues = ColumnList(ID_col, PlayList_col);
                SqlCommand updateCommand = UpdateCommandNoKeys(txn, PlayLists_tbl.Columns, PlayLists_Table, fieldValues);
                DeleteUpdateInsertWithDA(PlayLists_tbl, deleteCommand, updateCommand, insertCommand);
                txn.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to Update Playlists: " + ex.Message);
                txn.Rollback();
            }
        }
        //****************************************************************************************************************************
        public static DataRow SearchForRecording(DataTable recordings_tbl,
                                               string composer,
                                               string album)
        {
            string selectStatement = "Composer = '" + composer + "' and Album = '" + album + "'";
            DataRow[] results = recordings_tbl.Select(selectStatement);
            if (results.Length == 0)
            {
                return null;
            }
            else
            {
                return results[0];
            }
        }
        //****************************************************************************************************************************
        public static DataRow SearchForPlaylist(DataTable playList_tbl,
                                               int recordingID,
                                               string playlist)
        {
            string selectStatement = "ID = " + recordingID + " and PlayList = '" + playlist + "'";
            DataRow[] results = playList_tbl.Select(selectStatement);
            if (results.Length == 0)
            {
                return null;
            }
            else
            {
                return results[0];
            }
        }
        //****************************************************************************************************************************
        public static DataTable GetPlayLists(ArrayList playlists)
        {
            DataTable distinctPlaylists_tbl = GetDistinctPlaylists();
            distinctPlaylists_tbl.PrimaryKey = new DataColumn[] { distinctPlaylists_tbl.Columns[PlayList_col] };
            foreach (DataRow distinctPlayList_row in distinctPlaylists_tbl.Rows)
            {
                if (!TableInDataset(playlists, distinctPlayList_row[0].ToString()))
                {
                    DeleteWithParms(PlayLists_Table, new NameValuePair(PlayList_col, distinctPlayList_row[0].ToString()));
                }
            }
            return GetAllPlayLists();
        }
        //****************************************************************************************************************************
        public static DataTable GetDistinctPlaylists()
        {
            DataTable distinctPlaylists_tbl = new DataTable();
            GetDistinctValues(distinctPlaylists_tbl, PlayLists_Table, PlayList_col);
            return distinctPlaylists_tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetDistinctGenres()
        {
            DataTable distinctGenres_tbl = new DataTable();
            GetDistinctValues(distinctGenres_tbl, Recordings_Table, Genre_col);
            return distinctGenres_tbl;
        }
        //****************************************************************************************************************************
        private static bool TableInDataset(ArrayList playlists, string playListLookingFor)
        {
            foreach (string playList in playlists)
            {
                if (playList == playListLookingFor)
                {
                    return true;
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        public static void AddAlbumMovements(DataTable movements_tbl)
        {
            if (movements_tbl.Rows.Count != 0)
            {
                SqlCommand insertCommand = InsertCommand(movements_tbl, Movements_Table, true);
                InsertWithDA(movements_tbl, insertCommand);
            }
        }
        //****************************************************************************************************************************
        public static DataTable GetAllPlayLists()
        {
            DataTable PlayLists_tbl = DefinePlayListTable(PlayLists_Table);
            SelectAll(PlayLists_Table, PlayLists_tbl);
            return PlayLists_tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetAllRecordings(string recordingsTable)
        {
            DataTable recordings_tbl = DefineRecordingsTable();
            SelectAll(recordingsTable, OrderBy(Composer_col, Album_col), recordings_tbl);
            return recordings_tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetRecordingsForMovements(DataTable movements_tbl)
        {
            if (movements_tbl.Rows.Count == 0)
            {
                return null;
            }
            var recordingsList = new ArrayList();
            foreach (DataRow movements_row in movements_tbl.Rows)
            {
                var recordingsID = movements_row[ID_col].ToInt();
                var found = false;
                foreach (int recording in recordingsList)
                {
                    if (recording == recordingsID)
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    recordingsList.Add(recordingsID);
                }
            }
            DataTable movementRecordings_tbl = DefineMovementsTable();
            foreach (int recordingId in recordingsList)
            {
            }
            return DefineRecordingsTable();
        }
        //****************************************************************************************************************************
        public static void GetNewRecordingsForIPod(DataTable recordings_tbl)
        {
            if (recordings_tbl == null || recordings_tbl.Rows.Count == 0)
            {
                return;
            }
            do
            {
            }
            while (RemoveIfNotForIPod(recordings_tbl));
        }
        //****************************************************************************************************************************
        public static bool RemoveIfNotForIPod(DataTable recordings_tbl)
        {
            foreach (DataRow recordings_row in recordings_tbl.Rows)
            {
                if (recordings_row[IPod_col].ToInt() != 1)
                {
                    recordings_tbl.Rows.Remove(recordings_row);
                    return true;
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        public static DataTable GetNewRecordings(out DataTable movements_tbl)
        {
            DataTable LatestRecordingID_tbl = new DataTable();
            SelectAll(LatestExportedID_Table, LatestRecordingID_tbl);
            if (LatestRecordingID_tbl.Rows.Count == 0)
            {
                throw new Exception("Unable to Get Lastest Recordings Table");
            }
            DataRow LatestRecordingID_row = LatestRecordingID_tbl.Rows[0];
            var LatestRecordingID = LatestRecordingID_row[ID_col].ToInt();
            DataTable recordings_tbl = DefineRecordingsTable();
            SelectAllGreaterThan(Recordings_Table, OrderBy(Composer_col, Album_col), recordings_tbl, new NameValuePair(ID_col, LatestRecordingID));
            var LatestMovementID = LatestRecordingID_row[MovementID_col].ToInt();
            movements_tbl = DefineMovementsTable();
            SelectAllGreaterThan(Movements_Table, OrderBy(MovementID_col), movements_tbl, new NameValuePair(MovementID_col, LatestMovementID));
            if (recordings_tbl.Rows.Count == 0 && movements_tbl.Rows.Count == 0)
            {
                throw new Exception("There are no new recordings");
            }
            if (recordings_tbl.Rows.Count > 0)
            {
                var latestRecordingIDRow = recordings_tbl.Rows.Count - 1;
                DataRow recordings_row = recordings_tbl.Rows[latestRecordingIDRow];
                var latestID = recordings_row[ID_col].ToInt();
                LatestRecordingID_row[PreviousID_col] = LatestRecordingID_row[ID_col];
                LatestRecordingID_row[ID_col] = latestID;
            }

            if (movements_tbl.Rows.Count > 0)
            {
                var latestMovementIDRow = movements_tbl.Rows.Count - 1;
                DataRow Movements_row = movements_tbl.Rows[latestMovementIDRow];
                var latestMovementID = Movements_row[MovementID_col].ToInt();
                LatestRecordingID_row[PreviousMovementID_col] = LatestRecordingID_row[MovementID_col];
                LatestRecordingID_row[MovementID_col] = latestMovementID;
            }
            
            UpdateWithDA(LatestRecordingID_tbl, LatestExportedID_Table, ID_col, ColumnList(ID_col, PreviousID_col));
            return recordings_tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetAllRecordingsForIPod()
        {
            DataTable recordings_tbl = DefineRecordingsTable();
            SelectAll(Recordings_Table, recordings_tbl, new NameValuePair(IPod_col, 1));
            return recordings_tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetAllRecordingsForPlaylist(string playlist)
        {
            string selectStatement = "SELECT * FROM Recordings FULL OUTER JOIN Playlists ON Recordings.ID=Playlists.ID where Playlists.Playlist='" + playlist + "'";
            DataTable recordings_tbl = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = selectStatement;
            cmd.Connection = sqlConnection;
            int iNum = ExecuteSelectStatement(recordings_tbl, cmd);
            return recordings_tbl;
        }
        //****************************************************************************************************************************
        public static DataTable CreateSingleMovement(string title)
        {
            DataTable movements_tbl = DefineMovementsTable();
            DataRow movements_row = movements_tbl.NewRow();
            movements_row[MovementID_col] = 0;
            movements_row[ID_col] = 0;
            movements_row[Title_col] = title;
            movements_row[TrackNumber_col] = 1;
            movements_tbl.Rows.Add(movements_row);
            return movements_tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetAllMovements()
        {
            DataTable movements_tbl = DefineMovementsTable();
            SelectAll(Movements_Table, movements_tbl);
            return movements_tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetMovements(int id)
        {
            DataTable movements_tbl = new DataTable();
            SelectAll(Movements_Table, movements_tbl, new NameValuePair(ID_col, id));
            return movements_tbl;
        }
        //****************************************************************************************************************************
        public static int GetNumMovements(int id)
        {
            DataTable movements_tbl = GetMovements(id);
            return GetNumMovements(movements_tbl);
        }
        //****************************************************************************************************************************
        public static int GetNumMovements(DataTable movements_tbl)
        {
            var numMovements = movements_tbl.Rows.Count;
            return (numMovements == 0) ? 1 : numMovements;
        }
        //****************************************************************************************************************************
        public static DataTable GetRecording(string composer, string album)
        {
            DataTable recordings_tbl = DefineRecordingsTable();
            SelectAll(Recordings_Table, recordings_tbl, new NameValuePair(Composer_col, composer), new NameValuePair(Album_col, album));
            return recordings_tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetRecording(int recordingId)
        {
            DataTable recordings_tbl = DefineRecordingsTable();
            SelectAll(Recordings_Table, recordings_tbl, new NameValuePair(ID_col, recordingId));
            return recordings_tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetRecordingsOld(string composer, string album)
        {
            DataTable RecordingOld_tbl = new DataTable();
            SelectAll(RecordingsOld_Table, RecordingOld_tbl, new NameValuePair(Composer_col, composer), new NameValuePair(Album_col, album));
            return RecordingOld_tbl;
        }
        //****************************************************************************************************************************
        public static void UpdateRecording(DataTable recording_tbl)
        {
            UpdateWithDA(recording_tbl, Recordings_Table, ID_col, ColumnList(Album_col, Genre_col, IPod_col));
        }
        //****************************************************************************************************************************
        public static void UpdateMovement(DataTable movements_tbl)
        {
            UpdateWithDA(movements_tbl, Movements_Table, MovementID_col, ColumnList(Title_col));
        }
        //****************************************************************************************************************************
        public static int GetLatestRecordingID()
        {
            return GetMaxValue(Recordings_Table, ID_col);
        }
        //****************************************************************************************************************************
        public static void UpdateLatestExportedID()
        {
            var LatestRecordingID = GetLatestRecordingID();
            DataTable LatestExportedID_tbl = new DataTable();
            LatestExportedID_tbl = SelectAll(LatestExportedID_Table);
            if (LatestExportedID_tbl.Rows.Count != 0)
            {
                DataRow LatestExportedID_tbl_row = LatestExportedID_tbl.Rows[0];
                LatestExportedID_tbl_row[ID_col] = LatestRecordingID;
                UpdateWithDA(LatestExportedID_tbl, LatestExportedID_Table, ID_col, ColumnList(ID_col));
            }
        }
        //****************************************************************************************************************************
    }
}
