namespace Assistant
{
   public enum Command_e
   {
      Shutdown,
      Cancel_shutdown,
      Get_shutdown_time,
      Set_shutdown_time,
      Pre_shutdown,
      Get_shutdown_timer,
      Startup
   }

   public struct Command_t
   {
      public Command_e Id;
      public string Command;
      public bool HasArgument;
      public string Title;
      public string Message;
      public bool NotificationRequired;

      public Command_t(Command_e id, string command, bool hasArgument, bool notificationRequired, string title, string message)
      {
         Id = id;
         Command = command;
         HasArgument = hasArgument;
         Title = title;
         Message = message;
         NotificationRequired = notificationRequired;
      }

      public bool EqualCmd(string arg) => arg.Equals(Command);
   }

   public static class Configuration
   {
      public static Command_t[] commands =
      {
         /*            Command_e.id,                  Command,                HasArgument,  NotificationRequired, Title,                  Message */               
         new Command_t(Command_e.Shutdown,            "/shutdown",            false,        true,                 "SHUTDOWN",             "The system shutting down."),
         new Command_t(Command_e.Cancel_shutdown,     "/cancel_shutdown",     false,        false,                "CANCEL SHUTDOWN",      ""),
         new Command_t(Command_e.Get_shutdown_time,   "/get_shutdown_time",   false,        false,                "GET SHUTDOWN-TIME",    ""),
         new Command_t(Command_e.Set_shutdown_time,   "/set_shutdown_time",   true,         false,                "SET SHUTDOWN-TIME",    ""),
         new Command_t(Command_e.Pre_shutdown,        "/pre_shutdown",        false,        true,                 "PRE SHUTDOWN",         "The system will shutdown in 5 minutes!"),
         new Command_t(Command_e.Get_shutdown_timer,  "/get_shutdown_timer",  false,        false,                "SHUTDOWN TIMER STATE", ""),
         new Command_t(Command_e.Startup,             "/startup",             false,        true,                 "SYSTEM",               "Assistant started"),
      };
   }
}
