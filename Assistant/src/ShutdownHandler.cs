using System;
using System.Diagnostics;
using System.Timers;
using static Assistant.Program;

namespace Assistant
{
   class ShutdownHandler
   {
      private DateTime mLastTimeUpdated;
      private DateTime mPreShutdownTime;
      private DateTime mShutdownTime;

      private Timer mPreShutdownTimer = new Timer();
      private Timer mConfirmationTimer = new Timer();

      CallbackFunction mCallback;

      public ShutdownHandler(CallbackFunction callback)
      {
         /* Save callback function for later use */
         mCallback = callback;

         /* Initialize times and timers */
         DateTime now = DateTime.Now;
         int MINUTE = Properties.Settings.Default.DEFAULT_SHUTDOWN_MINUTE;
         int HOUR = Properties.Settings.Default.DEFAULT_SHUTDOWN_HOUR;
         Initialize(new DateTime( now.Year, now.Month, now.Day, HOUR, MINUTE, 0 ));
      }

      private bool Initialize(DateTime requested_shutdownTime)
      {
         mConfirmationTimer.Stop();
         mPreShutdownTimer.Stop();

         /* Set time */
         mLastTimeUpdated = DateTime.Now;
         mPreShutdownTime = requested_shutdownTime;
         mShutdownTime = requested_shutdownTime.AddMinutes(5);

         /* Initialize and start timer */
         mPreShutdownTimer.Interval = (mPreShutdownTime - mLastTimeUpdated).TotalMilliseconds;
         mPreShutdownTimer.Elapsed += PreShutdownEvent;
         mPreShutdownTimer.Start();

         /* Initialize confirmation timer */
         mConfirmationTimer.Interval = 300000; // 5 minutes
         mConfirmationTimer.Elapsed += NoConfirmationEvent;

         return true;
      }

      public string Get_ShutdownTime() => mShutdownTime.ToShortDateString() + " - " + mShutdownTime.ToShortTimeString();

      public string Get_ShutdownTimerState()
      {
         TimeSpan time = TimeSpan.FromMilliseconds(mPreShutdownTimer.Interval + 300000);
         return (int)time.TotalHours + "h " + time.Minutes + "min";
      }

      public bool Set_ShutdownTime(string time)
      {
         string[] t = time.Split(':');

         int hour, minute;
         int.TryParse(t[0], out hour);
         int.TryParse(t[1], out minute);

         Properties.Settings.Default.DEFAULT_SHUTDOWN_HOUR = hour;
         Properties.Settings.Default.DEFAULT_SHUTDOWN_MINUTE = minute;
         Properties.Settings.Default.Save();

         DateTime now = DateTime.Now;
         Initialize(new DateTime(now.Year, now.Month, now.Day, hour, minute, 0));

         return true;
      }

      private void PreShutdownEvent(object sender, EventArgs e)
      {
         mPreShutdownTimer.Stop();
         mConfirmationTimer.Start();
         mCallback(Configuration.commands[(int)Command_e.Pre_shutdown]);
      }

      private void NoConfirmationEvent(object sender, EventArgs e) => mCallback(Configuration.commands[(int)Command_e.Shutdown]);

      public bool Cancel_Shutdown() => Initialize(mShutdownTime.AddDays(1));

      public void Shutdown() => Process.Start("shutdown", "/s /t 0");
   }
}
