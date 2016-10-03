using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
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

    [DataContract]
    public class Command : ICommand, INotifyPropertyChanged
    {
        private string _name;
        [DataMember]
        public string name
        {
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
            get 
            {
                return _name;
            }
        }

        private string _description;
        [DataMember]
        public string description
        {
            set
            {
                _description = value;
                OnPropertyChanged("description");
            }
            get
            {
                return _description;
            }
        }

        private string _target;
        [DataMember]
        public string target
        {
            set
            {
                _target = value;
                OnPropertyChanged("target");
            }
            get
            {
                return _target;
            }
        }

        private string _startDir;
        [DataMember]
        public string startDir
        {
            set
            {
                _startDir = value;
                OnPropertyChanged("startDir");
            }
            get
            {
                return _startDir;
            }
        }

        private string _type;
        /**
         * cmd exe com bat
         * */
        [DataMember]
        public string type
        {
            set
            {
                _type = value;
                OnPropertyChanged("type");
            }
            get
            {
                return _type;
            }
        }

        public override string ToString()
        {
            return string.Format("command: {{ name: {0}, type: {1} }}", name , type);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
