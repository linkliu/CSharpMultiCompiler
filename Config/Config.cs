using System;
using System.Text;
using System.IO;

namespace MultiCompile
{
    public abstract class Config
    {
        //reference of filestream
        private FileStream configFileStream;
        public FileStream ConfigFileStream
        {
            get;
            set;
        }

        //streamread of config file
        private StreamReader configStreamReader;
        public StreamReader ConfigStreamReader
        {
            set;
            get;
        }
        //the file name of the config
        private string configfileName;
        public string ConfigFileName
        {
            get;
            set;
        }

        /// <summary>
        /// Read config file and return the reference of opened config file
        /// </summary>
        /// <param name="configFileName">config file name will open</param>
        /// <returns>reference of Opened FileStream</returns>
        abstract  public  StreamReader ReadFile();

        /// <summary>
        /// it's legal or not about the config file's format 
        /// </summary>
        /// <returns></returns>
        abstract public bool CheckFile();
        
    }
}
