using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTCore.CollectionService.Assignment
{
    public class GlobalState
    {
        private static GlobalState _instance = new GlobalState();
        public static GlobalState Instance => _instance;

        private GlobalState() { }

        public string _logFilePath { get; set; }
        public string _adeptOutputFileName { get; set; }
        public string _csvFolderPath { get; set; }
        public string _archiveFolderPath { get; set; }

    }
}
