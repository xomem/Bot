using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotEducation
{
    class Program
    {
        private static readonly TelegramBotClient Bot = new TelegramBotClient("513795138:AAFKuUNeAr94mscC_dSa_v36smeVVoslmaQ");
        static ReplyKeyboardMarkup markup = new ReplyKeyboardMarkup();
        public static int lastUpdateID = 0;
        public static bool inTrap = false;
        
        public static bool db = false;
        static void Main(string[] args)
        {
            Bot.OnMessage += Bot_OnMessage;
            Bot.OnMessageEdited += Bot_OnMessage;
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }
        async static void SendHelloDobby(MessageEventArgs e)
        {
            Random random = new Random();
            var FileUrl = @"C:\Users\SOVAJJ\source\repos\BotEducation\BotEducation\Dobbys\hello" + random.Next(1, 3) +".jpg";
            
            using (var stream = System.IO.File.Open(FileUrl, FileMode.Open))
            {
                Telegram.Bot.Types.FileToSend fts = new Telegram.Bot.Types.FileToSend();
                fts.Content = stream;
                fts.Filename = Path.GetFileName(FileUrl);
                await Bot.SendPhotoAsync(e.Message.Chat.Id, fts, fts.Filename);
            }
            db = true;
            await Bot.SendTextMessageAsync(e.Message.Chat.Id, ("Вы меня отпустите?"));
        }
        async static void SendHeppyDobby(MessageEventArgs e)
        {
            Random random = new Random();
            var FileUrl = @"C:\Users\SOVAJJ\source\repos\BotEducation\BotEducation\Dobbys\heppyDobby" + random.Next(1, 3) + ".jpg";

            using (var stream = System.IO.File.Open(FileUrl, FileMode.Open))
            {
                Telegram.Bot.Types.FileToSend fts = new Telegram.Bot.Types.FileToSend();
                fts.Content = stream;
                fts.Filename = Path.GetFileName(FileUrl);
                await Bot.SendPhotoAsync(e.Message.Chat.Id, fts, fts.Filename);
            }
            await Bot.SendTextMessageAsync(e.Message.Chat.Id, ("Спасибо, " + e.Message.Chat.Username + "!"));
        }
        async static void SendSadDobby(MessageEventArgs e)
        {
            Random random = new Random();
            var FileUrl = @"C:\Users\SOVAJJ\source\repos\BotEducation\BotEducation\Dobbys\sadDobby" + random.Next(1, 3) + ".jpg";

            using (var stream = System.IO.File.Open(FileUrl, FileMode.Open))
            {
                Telegram.Bot.Types.FileToSend fts = new Telegram.Bot.Types.FileToSend();
                fts.Content = stream;
                fts.Filename = Path.GetFileName(FileUrl);
                await Bot.SendPhotoAsync(e.Message.Chat.Id, fts, fts.Filename);
            }
            await Bot.SendTextMessageAsync(e.Message.Chat.Id, ("....."));
        }
        private static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage && db == true)
            {
                if (e.Message.Text == "Да" || e.Message.Text == "да")
                {
                    SendHeppyDobby(e);
                    db = false;
                    inTrap = false;
                }
                else if (e.Message.Text == "Нет" || e.Message.Text == "нет")
                {
                    SendSadDobby(e);
                }
                else
                {
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, ("Скажите пожалуйста, да или нет."));
                }
            }
            //throw new NotImplementedException();
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage && inTrap == false)
            {
                if (e.Message.Text == "/start")
                {
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, ("Введите номер телефона в формате - \"79993332222\""));
                }
                else if (e.Message.Text.Length < 11)
                {
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, ("К сожелению у вас на одну цифру меньше чем требуеться. Попробуйте еще раз. Введите номер телефона в формате - \"79993332222\""));
                }
                else if (e.Message.Text.Length > 11)
                {
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, ("К сожелению у вас на одну цифру больше чем требуеться. Попробуйте еще раз. Введите номер телефона в формате - \"79993332222\""));
                }
                else if (e.Message.Text.Length == 11 && !(e.Message.Text.Any(c => char.IsLetter(c))))
                {

                    char phoneLast = e.Message.Text[e.Message.Text.Length - 1];
                    int number = Convert.ToInt32(phoneLast);
                    if (number % 2 == 0)
                    {
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, ("Hello Word!"));
                    }
                    else
                    {
                        inTrap = true;
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, ("Привет хозяин!"));
                        SendHelloDobby(e);
                    }
                }
                else if ((e.Message.Text.Any(c => char.IsLetter(c))) && !inTrap)
                {
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, ("Вы можете вводить только цифры. Попробуйте еще раз. Введите номер телефона в формате - \"79993332222\""));
                }
                if (e.Message.Text == "exitTrap")
                {
                    inTrap = false;
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, ("inTrap = false"));
                }
               
            }
            else
            {
                if (e.Message.Text == "exitTrap")
                {
                    inTrap = false;
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, ("inTrap = false"));
                }
            }
        }

    }
}
