using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandMaster.Core
{
    public class FlagParameter : IParameterType
    {
        public bool isFlag
        {
            get { return true; }
        }

        public string flag
        {
            get;
            private set;
        }

        public FlagParameter(string flag)
        {
            this.flag = flag;
        }

        public override string ToString()
        {
            return string.Format("[{0}: true]", this.flag);
        }
    }
}
