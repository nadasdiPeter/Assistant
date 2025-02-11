using System.Threading;
using System.Windows.Forms;

namespace Assistant
{
   class Program
   {
      private TelegramBot mTelegramBot;
      private ShutdownHandler mShutdownHandler;
      private TrayApp mTrayApp;

      /* Define the callback function signature */
      public delegate void CallbackFunction(Command_t command, string[] args = null);


      void Start()
      {
         /* Start TelegramBot */
         mTelegramBot = new TelegramBot(Callback);

         /* Start ShutdownHandler */
         mShutdownHandler  = new ShutdownHandler(Callback);

         /* Start TrayApp */
         Thread TrayApp_Thread = new Thread(
                     delegate ()
                     {
                        mTrayApp = new TrayApp(Callback);
                        mTrayApp.TextUpdate_mTrayIcon(mShutdownHandler.Get_ShutdownTime());
                        Application.Run();
                     }
                 );
         TrayApp_Thread.Start();

         /* Triggers notification about the startup  */
         Callback(Configuration.commands[(int)Command_e.Startup]);
      }


      /* Callback function */
      public void Callback(Command_t command, string[] args = null)
      {
         if( command.NotificationRequired ) Notification(command.Title, command.Command, command.Message);

         switch ( command.Id )
         {
            case Command_e.Shutdown:
               mShutdownHandler.Shutdown();
               break;

            case Command_e.Cancel_shutdown:
               Notification("ShutdownHandler", command.Command, mShutdownHandler.Cancel_Shutdown() ? "Shutdown time: " + mShutdownHandler.Get_ShutdownTime() : "Process failed.");
               mTrayApp.TextUpdate_mTrayIcon(mShutdownHandler.Get_ShutdownTime());
               break;

            case Command_e.Get_shutdown_time:
               Notification("ShutdownHandler", command.Command, "Shutdown time: " + mShutdownHandler.Get_ShutdownTime());
               break;

            case Command_e.Set_shutdown_time:
               mShutdownHandler.Set_ShutdownTime(args[1]);
               Notification("ShutdownHandler", command.Command, "Default shutdown time changed!\nShutdown time: " + mShutdownHandler.Get_ShutdownTime());
               mTrayApp.TextUpdate_mTrayIcon(mShutdownHandler.Get_ShutdownTime());
               break;

            case Command_e.Get_shutdown_timer:
               Notification("ShutdownHandler", command.Command, "Shutdown will occur in: " + mShutdownHandler.Get_ShutdownTimerState());
               break;

            case Command_e.Lock:
               mShutdownHandler.LockWorkStation();
               break;

            default:
               /* No action required */
               break;
         }
      }

      private void Notification(string component, string command, string message)
      {
         mTelegramBot.Notification(component, command, message);
         mTrayApp.Notification(component, command + "\n" + message);
      }

      static void Main(string[] args) => new Program().Start();
   }
}
