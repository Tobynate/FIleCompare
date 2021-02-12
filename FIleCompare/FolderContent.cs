using System;
using System.Collections.Generic;
using System.Text;

namespace FIleCompare
{
    public class FolderContent
    {
        private List<string> _files = new List<string>();

        public List<string> Files
        {
            get { return _files; }
            set { _files = value; }
        }

        private List<string> _pathSubDirectory = new List<string>();

        public List<string> PathSubDirectories
        {
            get { return _pathSubDirectory; }
            set { _pathSubDirectory = value; }
        }

        private string _path;

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public List<string> Display()
        {
            List<string> output = new List<string>();

            foreach (string s in Files)
            {
                output.Add(s.Replace($"{Path}\\", null));
            }

            return output;
        }
    }
}
