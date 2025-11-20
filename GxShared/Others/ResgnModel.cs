using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GxShared.Others
{
    public class ResgnModel
    {
        public long Id { get; set; } = 0;
        public int ReqInt { get; set; } = 0;
        public string Usrid { get; set; } = string.Empty;
        public int Idorg { get; set; } = 0;
        public int Orig { get; set; } = 0;
        public bool Succeeded { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public int Iqmax { get; set; } = 0;
        public int FIdorg { get; set; } = 0;
        public int siDrap { get; set; } = 0;
        public int siDrap2 { get; set; } = 0;
        public int siDrap3 { get; set; } = 0;
        public ICollection<string> ErrNumb { get; set; } = new List<string>();
        public ICollection<string> ErrList { get; set; } = new List<string>();
        public ICollection<UAdm> LsAdms { get; set; } = new List<UAdm>();
        public DateTime Datact { get; set; } = DateTime.Now;
        public string ReqPara1 { get; set; } = string.Empty;
        public string ReqPara2 { get; set; } = string.Empty;
    }

    public class UAdm()
    {
        public int Id { get; set; } = 0;
        public string Fullnom { get; set; } = string.Empty;
        public string Usrid { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int Idorg { get; set; } = 0;
        public int SiOrga { get; set; } = 0;
    }
}
