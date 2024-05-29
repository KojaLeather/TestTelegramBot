using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTelegramBot.Models
{
    public class CompanyInfo
    {
        public string Inn { get; set; } = null!;
        public string? CompanyName { get; set; }
        public string? CompanyAddress { get; set; }
        public CompanyInfo ()
        {

        }
        
    }
}
