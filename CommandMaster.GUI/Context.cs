using CommandMaster.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandMaster.GUI
{
    public class Context
    {
        private static Context _instance = new Context();

        public static Context getInstance() {
            return _instance;
        }

        public ICommand Command { set; get; }
    }
}
