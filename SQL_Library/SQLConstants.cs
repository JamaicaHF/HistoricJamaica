using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicLibrary
{
    public static partial class SQL
    {
        public const string Recordings_Table = "Recordings";
        public const string ID_col = "ID";
        public const string Composer_col = "Composer";
        public const string Album_col = "Album";
        public const string Title_col = "Title";
        public const string Artist_col = "Artist";
        public const string Genre_col = "Genre";
        public const string TrackNumber_col = "TrackNumber";
        public const string TrackCount_col = "TrackCount";
        public const string Grouping_col = "Grouping";
        public const string Extension_col = "FileExtension";
        public const int Exception = -999999999;
        public const string NoOrderBy = "";
    }
}
