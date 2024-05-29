using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot;
using TestTelegramBot.Services;

namespace TestTelegramBot
{
    public class UpdateHandler : IUpdateHandler
    {
        private readonly ITelegramBotClient _botClient;
        private ConcurrentDictionary<long, string> lastMessageOfUser = new ConcurrentDictionary<long, string>();
        public ConcurrentDictionary<long, string> statusOfChat = new ConcurrentDictionary<long, string>();

        private string lastCommand;
        public UpdateHandler(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            StringIValidationService isValid = new StringIValidationService();
            if (update.Message != null && update.Message.Text != null)
            {
                var message = update.Message;
                statusOfChat.TryGetValue(message.Chat.Id, out string? status);
                if (status != string.Empty && status != null) //Если бот имеет статус ожидания значения ИНН от пользователя
                {
                    if (!isValid.InnIsValid(message.Text)) await InvalidValue(_botClient, message, cancellationToken);
                    else
                    {
                        switch (statusOfChat[message.Chat.Id])
                        {
                            case "InnStatus":
                                await InnResponse(_botClient, message, cancellationToken);
                                break;
                            case "ErgulStatus":
                                await ErgulResponse(_botClient, message, cancellationToken);
                                break;
                            case "OgrnStatus":
                                await OgrnResponse(_botClient, message, cancellationToken);
                                break;
                            default:
                                await InvalidValue(_botClient, message, cancellationToken);
                                break;
                        }
                    }
                    statusOfChat[message.Chat.Id] = string.Empty;
                }

                else //Если бот не ожидает значения ИНН от пользователя
                {
                    if (message.Text == "/last")
                    {
                        lastMessageOfUser.TryGetValue(message.Chat.Id, out string? lastMessage);
                        if (lastMessage != null && lastMessage != string.Empty) message.Text = lastMessage;
                        else message.Text = string.Empty;
                    }

                    switch (message.Text)
                    {
                        case "/start":
                            await StartCommand(_botClient, message, cancellationToken);
                            break;
                        case "/help":
                            await HelpCommand(_botClient, message, cancellationToken);
                            break;
                        case "/hello":
                            await HelloCommand(_botClient, message, cancellationToken);
                            break;
                        case "/inn":
                            await InnCommand(_botClient, message, cancellationToken);
                            statusOfChat[message.Chat.Id] = "InnStatus";
                            break;
                        case "/ergul":
                            await ErgulCommand(_botClient, message, cancellationToken);
                            statusOfChat[message.Chat.Id] = "ErgulStatus";
                            break;
                        case "/ogrn":
                            await OgrnCommand(_botClient, message, cancellationToken);
                            statusOfChat[message.Chat.Id] = "OgrnStatus";
                            break;
                        default:
                            await InvalidCommand(_botClient, message, cancellationToken);
                            break;
                    }
                    lastMessageOfUser[message.Chat.Id] = message.Text;
                }

            }
            static async Task<Message> StartCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
            {
                return await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: BotMessages.StartCommandMessage,
                    cancellationToken: cancellationToken);
            }

            static async Task<Message> HelpCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
            {
                return await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: BotMessages.HelpCommandMessage,
                    cancellationToken: cancellationToken);
            }

            static async Task<Message> HelloCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
            {
                return await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: BotMessages.HelloCommandMessage,
                    cancellationToken: cancellationToken);
            }

            static async Task<Message> InnCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
            {
                return await botClient.SendTextMessageAsync
                    (chatId: message.Chat.Id,
                    text: BotMessages.InnCommandMessage,
                    cancellationToken: cancellationToken);
            }

            static async Task<Message> ErgulCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
            {

                return await botClient.SendTextMessageAsync
                    (chatId: message.Chat.Id,
                    text: BotMessages.ErgulCommandMessage,
                    cancellationToken: cancellationToken);
            }

            static async Task<Message> OgrnCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
            {
                return await botClient.SendTextMessageAsync
                    (chatId: message.Chat.Id,
                    text: BotMessages.OgrnCommandMessage,
                    cancellationToken: cancellationToken);
            }

            static async Task<Message> InnResponse(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
            {
                FetchService innService = new FetchService();
                string text = await innService.GetNameAndAddress(message.Text);
                return await botClient.SendTextMessageAsync
                    (chatId: message.Chat.Id,
                    text: text,
                    cancellationToken: cancellationToken);
            }
            static async Task<Message> ErgulResponse(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
            {
                FetchService fetchService = new FetchService();
                string ogrn = await fetchService.GetOgrn(message.Text!);
                if (ogrn.Contains("Не получилось"))
                {
                    return await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: ogrn,
                        cancellationToken: cancellationToken);
                }    
                try
                {
                    await botClient.SendDocumentAsync(
                        chatId: message.Chat.Id,
                        InputFile.FromUri(fetchService.GetEgrulLink(ogrn)),
                        cancellationToken: cancellationToken);
                    return await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: $"Выше представлен PDF-файл выписки из ЕРГЮЛ по ИНН {message.Text}",
                        cancellationToken: cancellationToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught: " + ex);
                    return await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Не получилось скачать выписку ЕРГЮЛ, попробуйте позже",
                        cancellationToken: cancellationToken);
                }
            }

            static async Task<Message> OgrnResponse(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
            {
                FetchService fetchService = new FetchService();
                string text = await fetchService.GetOgrn(message.Text);
                if (text.Contains("Не получилось"))
                {
                    return await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: text,
                        cancellationToken: cancellationToken);
                }
                return await botClient.SendTextMessageAsync
                    (chatId: message.Chat.Id,
                    text: $"По ИНН {message.Text} был найден следующий ОГРН - {text}",
                    cancellationToken: cancellationToken);
            }

            static async Task<Message> InvalidCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
            {
                return await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: BotMessages.InvalidCommandMessage,
                    cancellationToken: cancellationToken);
            }
            static async Task<Message> InvalidValue(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
            {
                return await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: BotMessages.InvalidValueMessage,
                    cancellationToken: cancellationToken);
            }
        }
        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception;
            Console.WriteLine($"Bot catched this exception: {exception.Message}");
            return Task.CompletedTask;
        }
    }
}
