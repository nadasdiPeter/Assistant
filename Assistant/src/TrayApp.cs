using System;
using System.Drawing;
using System.Windows.Forms;
using static Assistant.Program;

namespace Assistant
{
   class TrayApp
   {
      private NotifyIcon mTrayApp = new NotifyIcon();
      private CallbackFunction mCallback;

      private ContextMenuStrip mMenu = new ContextMenuStrip();
      private ToolStripMenuItem mExit = new ToolStripMenuItem("Exit");
      private ToolStripMenuItem mAutoStart = new ToolStripMenuItem("Autostart");
      private ToolStripMenuItem mCancelShutdown = new ToolStripMenuItem("Cancel shutdown");

      public TrayApp(CallbackFunction callback)
      {
         mCallback = callback;

         /* Setup TrayIcon */
         mTrayApp.Icon = new Icon(Assistant.Properties.Resources.LetterA, 40, 40);
         mTrayApp.Visible = true;

         /* OnClick functions */
         mExit.Click += Exit_OnClick;
         mAutoStart.Click += AutoStart_OnClick;
         mCancelShutdown.Click += CancelShutdown_OnClick;

         /* Add menu items */
         mMenu.Items.Add(mCancelShutdown);
         mMenu.Items.Add(mAutoStart);
         mMenu.Items.Add(mExit);

         mTrayApp.ContextMenuStrip = mMenu;
      }

      private void Exit_OnClick(object sender, EventArgs e)
      {
         mTrayApp.Dispose();
         Application.Exit();
      }

      private void AutoStart_OnClick(object sender, EventArgs e)
      {
         mAutoStart.Checked = !mAutoStart.Checked;
         AutoStartFeature.Set_AutoStart(mAutoStart.Checked);
         Properties.Settings.Default.AUTOSTART = mAutoStart.Checked;
         Properties.Settings.Default.Save();
      }

      private void CancelShutdown_OnClick(object sender, EventArgs e) => mCallback(Configuration.commands[(int)Command_e.Cancel_shutdown]);

      public void TextUpdate_mTrayIcon(string text) => mTrayApp.Text = "Assistant (" + text + ")";

      public void Notification(string title, string message)
      {
         mTrayApp.Icon = Assistant.Properties.Resources.LetterA;
         mTrayApp.BalloonTipTitle= title;
         mTrayApp.BalloonTipText = message;
         mTrayApp.ShowBalloonTip(1000);
      }
   }
}
