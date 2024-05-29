using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestTelegramBot.ServicesAbstraction;

namespace TestTelegramBot.Services
{
    public class StringIValidationService : IStringValidationService
    {
        public bool InnIsValid (string inn)
        {
            string singleInnPattern = @"^\d{10}$";

            string multipleInnPattern = @"^\d{10}(,\s*\d{10})*$";

            return Regex.IsMatch(inn, singleInnPattern) || Regex.IsMatch(inn, multipleInnPattern);
        }
    }
}
