using CommandMaster.Core;
using CommandMaster.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandMaster.CLI
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            string usage = @"Usage:
                add -name=<name> -target=<target> -startDir=<startDir> [-description=<description>]
                remove -name=<name>
                analyze [-dir=<dir>] [-output=<outputJsonFile>] [--p]
                list [--system | --user | --all]
                install         // install Command Master to context menu for [exe, cmd, bat, com] files in Explorer
                uninstall       
             ";
            var subCommands = new string[] { "add", "remove", "analyze", "list" };
            if (args.Length < 1)
            {
                Console.WriteLine(usage);
                return;
            }

            var subArgs = args.Skip(1).ToArray();
            var parameter = new Parameter(subArgs);

            switch (args[0])
            {
                case "add":
                    add(parameter);
                    break;
                case "remove":
                    remove(parameter);
                    break;
                case "analyze":
                    analyze(parameter);
                    break;
                case "list":
                    list(parameter);
                    break;
                case "install":
                    install();
                    break;
                case "uninstall":
                    uninstall();
                    break;
                default:
                    Console.WriteLine(usage);
                    return;
            }

            Console.WriteLine("Done.");
            Console.ReadKey();
        }

        static void add(Parameter parameter)
        {
            new Master().add(parameter);
        }

        static void remove(Parameter parameter)
        {
            new Master().remove(parameter);
        }

        static void analyze(Parameter parameter)
        {
            new Master().analyze(parameter);
        }

        static void list(Parameter parameter)
        {
            new Master().list(parameter);
        }

        static void install()
        {
            new Master().install();
        }

        static void uninstall()
        {
            new Master().uninstall();
        }
    }
}
