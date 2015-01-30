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
            /**
             * add -name=<name> -target=<target> -startDir=<startDir> [-description=<description>]
             * remove -name=<name>
             * analyze <dir>
             * list [--system | --user | --all]
             * */
            var subCommands = new string[] { "add", "remove", "analyze", "list" };


            Console.ReadKey();
        }
    }
}
