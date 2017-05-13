using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpxViewer
{
    class SerializeHelper
    {
        public static void serializeObject(object sObject)
        {
            serializeObject(defaultPath, sObject);
        }
        public static object deserializeObject()
        {
            return deserializeObject(defaultPath);
        }

        /// <summary>
        /// Serialize object to path
        /// </summary>
        /// <param name="path">Target path</param>
        /// <param name="sObject">Serialize object</param>
        private static void serializeObject(string path, object sObject)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(path);
                if (!fileInfo.Directory.Exists)
                    Directory.CreateDirectory(fileInfo.Directory.FullName);
                //Open or create stream
                using (FileStream fs = new FileStream(path, FileMode.Truncate, FileAccess.ReadWrite, FileShare.Write))
                {
                    //Use BinaryFormatter to write and save data
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    bf.Serialize(fs, sObject);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + " Line 34, SerializeHelper.cs");
            }
        }

        /// <summary>
        /// Deserialize object from path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static object deserializeObject(string path)
        {
            object dObject;
            try
            {
                //Read the path and create it if it doesn't exits
                FileInfo fileInfo = new FileInfo(path);
                if (!fileInfo.Directory.Exists)
                    Directory.CreateDirectory(fileInfo.Directory.FullName);
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Write))
                {
                    //Deserialize process
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    dObject = bf.Deserialize(fs);
                }
                return dObject;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + " Line 62, SerializeHelper.cs");
            }
            return null;
        }

        private static string defaultPath = AppDomain.CurrentDomain.BaseDirectory + "Configs\\configs.dat";
    }
}
