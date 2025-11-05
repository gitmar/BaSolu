using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GxShared.GlobModels
{
    public class RegisterResult
    {
        public bool Succeeded { get; set; } = false;
        public string Usrid { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string EmailConfirmationToken { get; set; } = string.Empty;
        public int ErrTyp { get; set; } = 0;
        public ICollection<string>? ErrNumb { get; set; } = new List<string>();
        public List<string> ErrList { get; set; } = new List<string>();
    }
}
