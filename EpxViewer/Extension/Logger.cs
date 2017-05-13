using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpxViewer
{
    public class Logger
    {
        private Dictionary<string, object> storeDictionary;
        private volatile static Logger instance = null;
        private static readonly object lockHelper = new object();
        private Logger() { initilize(); }

        public static Logger getLogger()
        {
            if (instance == null)
            {
                lock (lockHelper)
                {
                    if (instance == null)
                        instance = new Logger();
                }
            }
            return instance;
        }

        public object getLogOfObject(string name)
        {
            if (storeDictionary != null)
            {
                if (storeDictionary.ContainsKey(name)) return storeDictionary[name];
                else return null;
            }
            return null;
        }

        public void logValue(string name, object value)
        {
            if(storeDictionary != null)
            {
                if (!storeDictionary.ContainsKey(name)) storeDictionary.Add(name, value);
                else storeDictionary[name] = value;
            }
        }

        public void save()
        {
            SerializeHelper.serializeObject(storeDictionary);
        }
        
        private void initilize()
        {
            object obj = SerializeHelper.deserializeObject();
            if(obj != null)
            {
                var readedObj = obj as Dictionary<string, object>;
                if(readedObj != null)
                {
                    storeDictionary = readedObj;
                }
                else
                {
                    storeDictionary = new Dictionary<string, object>();
                }
            }
            else
            {
                storeDictionary = new Dictionary<string, object>();
            }
        }
    }
}
