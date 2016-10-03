using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommandMaster.Core
{
    public class LinuxParameterStyle : IParameterStyle
    {
        private static Regex _flagStyle = new Regex(@"^\s*--([^\-][\w\-]*)\s*$");
        private static Regex _parameterStyle = new Regex(@"^\s*-([^\-][\w\-]*)\=""?([^\-][\w\-/\\\s\.:]*)""?\s*$");
        public Regex flagStyle
        {
            get { return _flagStyle; }
        }

        public Regex parameterStyle
        {
            get { return _parameterStyle; }
        }
    }
}
