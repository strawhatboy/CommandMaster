using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandMaster.Core
{
    public class KeyValueParameter : IParameterType
    {
        public bool isFlag { get { return false; } }
        public KeyValuePair<string, string> value { get; private set; }

        public KeyValueParameter(string key, string value) : this(new KeyValuePair<string, string>(key, value)) { }

        public KeyValueParameter(KeyValuePair<string, string> keyValue) { this.value = keyValue; }

        public override string ToString()
        {
            return string.Format("[key: {0}, value: {1}]", this.value.Key, this.value.Value);
        }
    }
}
