using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace TestTelegramBot
{
    public class Program
    {
        static IControllDataBase controllDataBase = new ControllDataBase();
        public static ITelegramBotClient bot = new TelegramBotClient("TOKEN");
        //static List<String> messagesStore = new List<string>();
        static ObservableCollection<string> messagesStore = new ObservableCollection<string>();

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, 
                                                   Update update, 
                                                   CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));

            if(update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                string messageText = message.Text.ToLower();
                var telegramUser = message.From;
                var exist = controllDataBase.CheckUser(telegramUser.Id);

                if (messageText == "/start")
                {
                    if (!exist)
                    {
                        User user = new User(telegramUser.Id,
                                             message.Chat.Id,
                                             telegramUser.Username,
                                             telegramUser.FirstName,
                                             telegramUser.LastName,
                                             DateTime.Now);
                        controllDataBase.Add(user);
                        await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать.");
                    }
                    else
                        await botClient.SendTextMessageAsync(message.Chat, "Вы уже зарегистрированы.");
                    return;
                }
                else if (messageText == "/my-id")
                {
                    string userId = telegramUser.Id.ToString();
                    if(exist)
                        await botClient.SendTextMessageAsync(message.Chat, $"Ваш ID: {userId}");
                    else
                        await botClient.SendTextMessageAsync(message.Chat, "Для доступа к этой команде " +
                                                                           "необходима регистрация (/start).");
                    return;
                }
                else if (messageText == "/reg-date")
                {
                    long identify = telegramUser.Id;
                    var date = controllDataBase.GetDate(identify);
                    if (exist)
                        await botClient.SendTextMessageAsync(message.Chat, $"Ваша дата регистрации: {date}");
                    else
                        await botClient.SendTextMessageAsync(message.Chat, "Для доступа к этой команде " +
                                                                           "необходима регистрация (/start).");
                    return;
                }
                else if(messageText == "/date")
                {
                    if (exist)
                        await botClient.SendTextMessageAsync(message.Chat, $"Текущая дата: {DateTime.Now.Date}");
                    else
                        await botClient.SendTextMessageAsync(message.Chat, "Для доступа к этой команде " +
                                                                           "необходима регистрация (/start).");
                    return;
                }
                else if (messageText == "/time")
                {
                    if (exist)
                        await botClient.SendTextMessageAsync(message.Chat, $"Текущее время: {DateTime.Now.ToShortTimeString()}");
                    else
                        await botClient.SendTextMessageAsync(message.Chat, "Для доступа к этой команде " +
                                                                           "необходима регистрация (/start).");
                    return;
                }
                else if (messageText == "/day")
                {
                    if (exist)
                        await botClient.SendTextMessageAsync(message.Chat, $"Текущий день: {DateTime.Now.Day.ToString()}");
                    else
                        await botClient.SendTextMessageAsync(message.Chat, "Для доступа к этой команде " +
                                                                           "необходима регистрация (/start).");
                    return;
                }
            }            
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
                                                  CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        public static async void SendMessageFromMessagesStore_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Add)
            {
                List<long> chatId = controllDataBase.GetChatId();

                foreach (var id in chatId)
                {
                    await bot.SendTextMessageAsync(id, (string)(e.NewItems[0]));
                }
                messagesStore.RemoveAt(0);
            }
        }

        public static async Task WaitingInput()
        {
            await Task.Run(() => 
            {
                while (true)
                {
                    var message = Console.ReadLine();
                    messagesStore.Add(message);
                    Thread.Sleep(1);
                }
            });
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            messagesStore.CollectionChanged +=
                SendMessageFromMessagesStore_CollectionChanged;
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiveOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };

            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiveOptions,
                cancellationToken
                );

            while (true)
            {
                await WaitingInput();
            }
        }
    }
}
