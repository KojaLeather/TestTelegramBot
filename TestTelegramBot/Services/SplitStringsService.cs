using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTelegramBot.ServicesAbstraction;

namespace TestTelegramBot.Services
{
    public class SplitStringsService : ISplitStringService
    {
        public List<string> SplitInn(string inn)
        {
            string[] parts = inn.Split(new string[] { ", " }, StringSplitOptions.None);

            List<string> result = new List<string>(parts);

            return result;
        }
    }
}
