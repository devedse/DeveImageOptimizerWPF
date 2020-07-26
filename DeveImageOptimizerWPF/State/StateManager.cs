using System.IO;
using System.Xml.Serialization;

namespace DeveImageOptimizerWPF.State
{
    public class StateManager<T> where T : class, new()
    {
        private readonly string _stateFileName;
        private T _currentUserSettings;
        private readonly object _lockject = new object();

        public StateManager(string stateFileName)
        {
            _stateFileName = stateFileName;
        }

        public T State => _currentUserSettings ?? (_currentUserSettings = LoadFromFile(_stateFileName));

        public void Save()
        {
            SaveToFile(_stateFileName);
        }

        private T LoadFromFile(string filename)
        {
            if (File.Exists(filename))
            {
                lock (_lockject)
                {
                    using (var sw = new StreamReader(filename))
                    {
                        var xmls = new XmlSerializer(typeof(T));
                        var retval = xmls.Deserialize(sw) as T;
                        if (retval != null)
                        {
                            var changable = retval as IChangable;
                            if (changable != null)
                            {
                                changable.IsChanged = false;
                            }
                            return retval;
                        }
                    }
                }
            }
            return new T();
        }

        private void SaveToFile(string filename)
        {
            var curState = State;
            lock (_lockject)
            {
                using (var sw = new StreamWriter(filename))
                {
                    var xmls = new XmlSerializer(typeof(T));
                    xmls.Serialize(sw, State);
                }

                var changable = curState as IChangable;
                if (changable != null)
                {
                    changable.IsChanged = false;
                }
            }
        }
    }
}
