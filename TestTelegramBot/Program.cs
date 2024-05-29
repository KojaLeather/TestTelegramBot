using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using TestTelegramBot.Models;
using TestTelegramBot.Services;
using TestTelegramBot.ServicesAbstraction;
using TestTelegramBot;

namespace TestTelegramBot
{

    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder().AddUserSecrets<Program>();
            IConfiguration configuration = builder.Build();


            var botClient = new TelegramBotClient(configuration["TelegramAPIKey"]);
            CancellationTokenSource cts = new();

            UpdateHandler updateHandler = new UpdateHandler(botClient);
            botClient.StartReceiving(
                updateHandler,
                receiverOptions: null,
                cancellationToken: cts.Token);

            var botInfo = await botClient.GetMeAsync();
            Console.WriteLine($"Bot {botInfo.Username} started listening");
            Console.ReadLine();
            cts.Cancel();
        }
    }
}