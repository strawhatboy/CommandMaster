using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommandMaster.Core
{
    public interface IParameterStyle
    {
        Regex flagStyle { get; }
        Regex parameterStyle { get; }
    }
}
