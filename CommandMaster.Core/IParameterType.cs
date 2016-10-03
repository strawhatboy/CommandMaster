using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandMaster.Core
{
    public interface IParameterType
    {
        bool isFlag { get; }
    }
}
