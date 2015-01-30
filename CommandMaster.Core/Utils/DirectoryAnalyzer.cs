using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CommandMaster.Core.Utils
{
    public static class DirectoryAnalyzer
    {
        public static IEnumerable<ICommand> analyze(string dir)
        {
            DirectoryInfo di = new DirectoryInfo(dir);
            if (di.Exists)
            {
                foreach (var f in di.GetFiles()) {
                    if (new string[] { ".exe", ".cmd", ".bat", ".com" }.Contains(f.Extension.ToLower())) 
                    {
                        yield return new Command() {
                            name = f.Name.Remove(f.Name.IndexOf(f.Extension)),
                            description = FileVersionInfo.GetVersionInfo(f.FullName).FileDescription,
                            type = f.Extension,
                            target = f.FullName,
                            startDir = f.DirectoryName
                        };
                    }
                }
            }
        }
    }
}
