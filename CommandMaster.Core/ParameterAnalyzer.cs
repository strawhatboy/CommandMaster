using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandMaster.Core
{
    public class ParameterAnalyzer
    {
        static List<IParameterStyle> styles = new List<IParameterStyle>() {
            new WindowsParameterStyle(),
            new LinuxParameterStyle()
        };
        public static IParameterType analyze(string parameter, IParameterStyle type)
        {
            Dictionary<string, string> keyValue = new Dictionary<string, string>();
            var matches = type.flagStyle.Match(parameter);
            var count = matches.Captures.Count;
            if (count == 1)
            {
                // flag
                return new FlagParameter(matches.Groups[1].Value);
            }

            matches = type.parameterStyle.Match(parameter);
            count = matches.Captures.Count;
            if (count == 1)
            {
                // parameter
                return new KeyValueParameter(matches.Groups[1].Value, matches.Groups[2].Value);
            }
            

            return null;
        }

        public static IParameterType analyze(string parameter)
        {
            foreach (var style in styles) {
                var type = analyze(parameter, style);
                if (type != null) {
                    return type;
                }
            }

            return null;
        }
    }
}
