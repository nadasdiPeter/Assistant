using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static Assistant.Program;

namespace Assistant
{
   class TelegramBot
   {
      private CallbackFunction mCallback;
      private static TelegramBotClient mBotClient = new TelegramBotClient(Credentials.Token);

      public TelegramBot(CallbackFunction callback)
      {
         mCallback = callback;
         mBotClient.OnMessage += OnMessage;
      }

      private Task OnMessage(Message message, UpdateType type)
      {
         if (message.Text != null)
         {
            string[] msg = message.Text.Split(' ');
            foreach (var cmd in Configuration.commands)
            {
               if (cmd.EqualCmd(msg[0]))
               {
                  mCallback(cmd, msg);
                  break;
               }
            }
         }
         return null;
      }

      private void Send(string message)
         => mBotClient.SendMessage(Credentials.ChatID, message, parseMode: ParseMode.Html);

      public void Notification(string component, string command, string message)
         => mBotClient.SendMessage(Credentials.ChatID, "<b>" + component + "</b>\n" + ">> " + command  + "\n" + "<i>" + message + "</i>", parseMode: ParseMode.Html);
   }
}
