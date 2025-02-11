using IWshRuntimeLibrary;
using System;
using System.Windows.Forms;

namespace Assistant
{
   static class AutoStartFeature
   {
      /* The Windows Scripting Host (WSH) enables a number of file system and network operations
       * to be performed from a script file. Fortunately, it is very simple to directly program
       * the WSH in a .NET program by including a reference to the WSH runtime library (IWshRuntimeLibrary).
       * To do this within the Visual Studio .NET IDE, do the following:
       * after creating a new project, right-click on the project name within the Solution Explorer,
       * select "Add Reference", select the "COM" tab, find and select the "Windows Script Host Object Model"
       * in the listbox, click "Select", and then click "OK".
       * Next, include a reference to the library, for example, within a C# file use the following: */

      public static string startUpFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
      public static string appName = Application.ProductName;
      public static string startUpFolderAppPath = startUpFolderPath + "\\" + appName + ".lnk";

      /* Removes the application shotcut from startup folder. */
      public static void Disable_AutoStart()
      {
         if (System.IO.File.Exists(startUpFolderAppPath)) System.IO.File.Delete(startUpFolderAppPath);
      }

      /* Creates application shotcut into startup folder  */
      public static void Enable_AutoStart()
      {
         WshShell wshShell = new WshShell();
         IWshRuntimeLibrary.IWshShortcut shortcut;

         // Create the shortcut
         shortcut = (IWshRuntimeLibrary.IWshShortcut)wshShell.CreateShortcut(startUpFolderAppPath);
         shortcut.TargetPath = Application.ExecutablePath;
         shortcut.WorkingDirectory = Application.StartupPath;
         shortcut.Description = "Launch My Application";
         shortcut.Save();
      }

      /* Enables or disables AutoStart feature based on the received parameter. */
      public static void Set_AutoStart(bool IsEnabled)
      {
         if (IsEnabled)
            Enable_AutoStart();
         else
            Disable_AutoStart();
      }
   }
}
