using Telegram.Bot.Types;
using TestTelegramBot.Models;

namespace TestTelegramBot.ServicesAbstraction
{
    public interface IFetchService
    {
        public Task<string> GetNameAndAddress(string inn);
        public Task<string> GetOgrn(string inn);
        public string GetEgrulLink(string ogrn); 
    }
}
