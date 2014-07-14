using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BionicLab10.Model
{
    class ListFiles
    {
        public List<MyFile> NewList { get; set; }
        public string Dir1 { get; set; }
        public string Dir2 { get; set; }

        public ListFiles()
        {
            NewList = new List<MyFile>();
            Dir1 = "c:\\test";
            Dir2 = "c:\\test2";
            NewList.Add(new MyFile {FileName = "ggg", FileDate1 = default(DateTime), FileDate2 = default(DateTime), Status = "eee", Sync = false});
        }
    }
}
