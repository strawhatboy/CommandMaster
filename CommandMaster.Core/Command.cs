using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandMaster.Core
{
    public interface ICommand
    {
        string name { set; get; }
        string description { set; get; }
        string target { set; get; }
        string startDir { set; get; }

        /**
         * cmd exe com bat
         * */
        string type { set; get; }
    }

    public class Command : ICommand
    {
        public string name { set; get; }
        public string description { set; get; }
        public string target { set; get; }
        public string startDir { set; get; }
        
        /**
         * cmd exe com bat
         * */
        public string type { set; get; }
    }
}
