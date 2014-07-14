using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BionicLab10.Model
{
    public class MyFile
    {
        public string FileName { get; set; }
        public DateTime FileDate1 { get; set; }
        public DateTime FileDate2 { get; set; }
        public string Status { get; set; }
        public bool Sync { get; set; }
    }
}
