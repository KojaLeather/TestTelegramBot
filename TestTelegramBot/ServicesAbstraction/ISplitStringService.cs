using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTelegramBot.ServicesAbstraction
{
    public interface ISplitStringService
    {
        public List<string> SplitInn(string inn);
    }
}
