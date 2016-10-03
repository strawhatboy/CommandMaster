using CommandMaster.Core.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace CommandMaster.Core
{
    public class Master
    {
        static string sysCommandsCache = "config/sysConfig.json";
        static string usrCommandsCache = "config/usrConfig.json";
        static string config = "config/config.json";
        static string myPath = "mypath";
        static string configPath = "config";
        static string guiPath = "CommandMaster.GUI.exe";
        static string cliPath = "CommandMaster.CLI.exe";
        List<ICommand> _sysCommands;
        List<ICommand> _userCommands;

        public IList<ICommand> sysCommands
        {
            get
            {
                return _sysCommands;
            }
            private set
            {
                _sysCommands = new List<ICommand>(value);
            }
        }

        public IList<ICommand> userCommands
        {
            get
            {
                return _userCommands;
            }
            private set
            {
                _userCommands = new List<ICommand>(value);
            }
        }

        public Master()
        {
            init();
        }

        private void init()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            var curPath = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
            myPath = Path.Combine(curPath, myPath);
            configPath = Path.Combine(curPath, configPath);
            guiPath = Path.Combine(curPath, guiPath);
            cliPath = Path.Combine(curPath, cliPath);
            sysCommandsCache = Path.Combine(curPath, sysCommandsCache);
            usrCommandsCache = Path.Combine(curPath, usrCommandsCache);

            // add new system variable
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("COMMAND_MASTER")))
            {
                Environment.SetEnvironmentVariable("COMMAND_MASTER", new DirectoryInfo(myPath).FullName, EnvironmentVariableTarget.Machine);
            }

            // add the variable to path
            var pathStr = Environment.GetEnvironmentVariable("path");
            var path = pathStr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (path.FirstOrDefault(a => a.Contains(myPath)) == null)
            {
                Environment.SetEnvironmentVariable("path", pathStr + (pathStr[pathStr.Length - 1] != ';' ? ";" : "") + "%COMMAND_MASTER%", EnvironmentVariableTarget.Machine);
            }

            // add the dir 
            if (!Directory.Exists(myPath))
            {
                Directory.CreateDirectory(myPath);
            }

            // add config dir
            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }

            // load config
            loadConfig();
        }
        public void loadConfig()
        {
            if (!File.Exists(sysCommandsCache) || !File.Exists(usrCommandsCache))
            {
                using (File.Create(sysCommandsCache)) { };
                using (File.Create(usrCommandsCache)) { };
                analyzeSystemCommands();
                saveCaches();
            }
            else
            {
                sysCommands = new List<ICommand>();
                userCommands = new List<ICommand>();
                foreach (var cmd in JsonSerializer.fileToObject<List<Command>>(sysCommandsCache))
                {
                    sysCommands.Add(cmd);
                }
                foreach (var cmd in JsonSerializer.fileToObject<List<Command>>(usrCommandsCache))
                {
                    userCommands.Add(cmd);
                }
            }
        }

        public void analyzeSystemCommands()
        {
            var _sysCommands = new List<ICommand>();
            var _userCommands = new List<ICommand>();
            var paths = System.Environment.GetEnvironmentVariable("path").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var _myPath = new DirectoryInfo(myPath);
            foreach (var path in paths)
            {
                var _path = Environment.ExpandEnvironmentVariables(path);
                var cmds = DirectoryAnalyzer.analyze(_path);
                if (new DirectoryInfo(_path).ToString() == _myPath.ToString())
                {
                    _userCommands.AddRange(cmds);
                }
                else
                {
                    _sysCommands.AddRange(cmds);
                }
            }
            sysCommands = _sysCommands;
            userCommands = _userCommands;
        }

        public void saveCaches()
        {
            JsonSerializer.objectToFile<List<Command>>((from p in sysCommands select (Command)p).ToList(), sysCommandsCache);
            JsonSerializer.objectToFile<List<Command>>((from p in userCommands select (Command)p).ToList(), usrCommandsCache);
        }

        public ICommand add(Parameter parameter)
        {
            var values = parameter.keyValues;
            var cmd = new Command()
            {
                name = values.ContainsKey("name") ? values["name"] : "unknown",
                description = values.ContainsKey("description") ? values["description"] : null,
                startDir = values.ContainsKey("startDir") ? values["startDir"] : null,
                target = values.ContainsKey("target") ? values["target"] : null
            };
            return add(cmd);
        }

        public ICommand add(ICommand cmd)
        {
            if (string.IsNullOrEmpty(cmd.target))
            {
                // error

            }
            else
            {
                cmd.type = cmd.target.Substring(cmd.target.LastIndexOf('.'));
            }

            if (userCommands.FirstOrDefault(a => a.name == cmd.name) != null)
            {
                // already existed command, reject
                throw new InvalidOperationException(string.Format("The command {0} is already added.", cmd.name));
            }

            //create cmd file
            StringBuilder command = new StringBuilder();
            //StringBuilder command = new StringBuilder("start ");
            //command.AppendFormat("\"{0}\" ", cmd.name);
            //if (cmd.startDir != null) command.AppendFormat("/D \"{0}\"", cmd.startDir);
            if (cmd.target != null) command.AppendFormat("\"{0}\" %*", cmd.target);
            File.WriteAllText(Path.Combine(myPath, cmd.name + ".cmd"), string.Format(@"@echo off
{0}
", command.ToString()));

            //add to cache and save cache
            userCommands.Add(cmd);
            saveCaches();

            return cmd;
        }

        public void remove(Parameter parameter)
        {
            var values = parameter.keyValues;
            var name = values.ContainsKey("name") ? values["name"] : null;
            var cmd = userCommands.FirstOrDefault(a => a.name == name);
            remove(cmd);
        }

        public void remove(ICommand cmd)
        {
            if (cmd == null || cmd.name == null)
            {
                // error
            }
            else
            {
                // rename to _delete
                var fi = new FileInfo(Path.Combine(myPath, cmd.name + ".cmd"));
                if (fi.Exists)
                {
                    fi.MoveTo(Path.Combine(myPath, cmd.name + ".cmd_deleted"));
                }
            }

            // update cache
            userCommands.Remove(cmd);
            saveCaches();
        }

        public IEnumerable<ICommand> analyze(Parameter parameter)
        {
            var values = parameter.keyValues;
            var flags = parameter.flags;

            var dir = values.ContainsKey("dir") ? values["dir"] : ".";
            var di = new DirectoryInfo(dir);
            var output = values.ContainsKey("output") ? values["output"] : (di.Name + ".json");
            var result = DirectoryAnalyzer.analyze(dir);

            if (flags.Contains("p"))
            {
                foreach (var cmd in result)
                {
                    Console.WriteLine(cmd.ToString());
                }
            }

            JsonSerializer.objectToFile<List<Command>>((from p in result select (Command)p).ToList(), output);
            return result;
        }

        public void list(Parameter parameter)
        {
            var flags = parameter.flags;
            analyzeSystemCommands();

            if (flags.Contains("all"))
            {
                printSystemCommands();
                printUserCommands();
            }
            else
            {
                if (flags.Contains("system"))
                {
                    printSystemCommands();
                }
                if (flags.Contains("user"))
                {
                    printUserCommands();
                }
            }

            saveCaches();
        }
        public bool install()
        {
            var classRoot = Registry.ClassesRoot;
            var apps = classRoot.OpenSubKey("Applications", true);
            if (!apps.GetSubKeyNames().Contains("cmgui.exe"))
            {
                var openKey = apps.CreateSubKey("cmgui.exe").CreateSubKey("shell").CreateSubKey("open");
                openKey.SetValue(null, "[COMMAND MASTER] Add this to system command...");
                var cmdKey = openKey.CreateSubKey("command");
                cmdKey.SetValue(null, "\"" + new FileInfo(guiPath).FullName + "\" \"%1\"");

                // add file types
                addFileTypesToReg(".exe");
                addFileTypesToReg(".cmd");
                addFileTypesToReg(".bat");
                addFileTypesToReg(".com");
                //exeKey.-j
                return true;
            }

            return false;
        }

        private void addFileTypesToReg(string extension)
        {
            var classRoot = Registry.ClassesRoot;
            var exeKey = classRoot.OpenSubKey(extension, true);
            var exeFileKeyName = exeKey.GetValue(null).ToString();
            var exeFileKey = classRoot.OpenSubKey(exeFileKeyName, true);
            var shellKey = exeFileKey.OpenSubKey("shell", true);
            var exeOpenKey = shellKey.CreateSubKey("cm");
            exeOpenKey.SetValue(null, "[COMMAND MASTER] Add this to system command...");
            var cmdKey = exeOpenKey.CreateSubKey("command");
            cmdKey.SetValue(null, "\"" + new FileInfo(guiPath).FullName + "\" \"%1\"");
        }

        public bool uninstall()
        {
            //TODO: remove the reg keys
            var classRoot = Registry.ClassesRoot;
            var apps = classRoot.OpenSubKey("Applications", true);
            if (apps.GetSubKeyNames().Contains("cmgui.exe"))
            {
                apps.DeleteSubKeyTree("cmgui.exe");

                removeFileTypesFromReg(".exe");
                removeFileTypesFromReg(".cmd");
                removeFileTypesFromReg(".bat");
                removeFileTypesFromReg(".com");
                return true;
            }

            return false;
        }

        private void removeFileTypesFromReg(string extension)
        {
            var classRoot = Registry.ClassesRoot;
            var exeKey = classRoot.OpenSubKey(extension, true);
            var exeFileKeyName = exeKey.GetValue(null).ToString();
            var exeFileKey = classRoot.OpenSubKey(exeFileKeyName, true);
            var shellKey = exeFileKey.OpenSubKey("shell", true);
            shellKey.DeleteSubKeyTree("cm");
        }
        void printSystemCommands()
        {
            Console.WriteLine("system commands");
            Console.WriteLine("---------------");
            foreach (var cmd in sysCommands)
            {
                Console.WriteLine(cmd.ToString());
            }
        }

        void printUserCommands()
        {
            Console.WriteLine("user commands");
            Console.WriteLine("-------------");
            foreach (var cmd in userCommands)
            {
                Console.WriteLine(cmd.ToString());
            }
        }

        [DataContract]
        protected class Config
        {
            [DataMember]
            public IList<string> myPathes { set; get; }
        }
    }
}
