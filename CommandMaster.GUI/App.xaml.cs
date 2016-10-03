using CommandMaster.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace CommandMaster.GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var args = e.Args;
            if (args.Length == 0)
            {
                // install
                var result = MessageBox.Show("Invalid operation.");
                Application.Current.Shutdown(-1);
            }
            else
            {
                var f = new FileInfo(args[0]);
                var cmd = new Command
                {
                    name = f.Name.Remove(f.Name.IndexOf(f.Extension)),
                    description = FileVersionInfo.GetVersionInfo(f.FullName).FileDescription,
                    type = f.Extension,
                    target = f.FullName,
                    startDir = f.DirectoryName
                };

                Context.getInstance().Command = cmd;
            }
        }
    }
}
