using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandMaster.Core
{
    public class Parameter
    {
        IDictionary<string, string> _keyValues = new Dictionary<string, string>();
        IList<string> _flags = new List<string>();
        public IDictionary<string, string> keyValues { get { return _keyValues; } }
        public IEnumerable<string> flags { get { return _flags; } }

        public Parameter(params string[] parameters)
        {

            foreach (var parameter in parameters)
            {
                var result = ParameterAnalyzer.analyze(parameter);
                if (result.isFlag)
                {
                    _flags.Add(((FlagParameter)result).flag);
                }
                else
                {
                    _keyValues.Add(((KeyValueParameter)result).value);
                }
            }
        }
    }
}
