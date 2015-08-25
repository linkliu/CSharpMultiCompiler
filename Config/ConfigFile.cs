using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiCompile
{
    public class ConfigFile : Config
    {


        /// <summary>
        /// the Dictionary of head index and tail information patches;
        /// </summary>
        protected Dictionary<string, string> HeadIndexDictionary;
        /// <summary>
        /// config head string
        /// </summary>
        protected string HeadString;
        /// <summary>
        /// config end string
        /// </summary>
        protected string EndString;

        protected List<string> errorLog;
 
        /// <summary>
        /// constructor of 
        /// </summary>
        /// <param name="configFileName"></param>
        public ConfigFile(string configFileName)
        {
            ConfigFileName = configFileName;
            HeadIndexDictionary = new Dictionary<string, string>();
            errorLog = new List<string>();
        }


        /// <summary>
        /// forbidden the null parameter constructor
        /// </summary>
        private ConfigFile()
        {

        }

        /// <summary>
        /// Read config file and return the reference of opened config file
        /// </summary>
        /// <param name="configFileName">config file name will open</param>
        /// <returns>reference of Opened FileStream</returns>
        public override StreamReader ReadFile()
        {
            ConfigFileStream = new FileStream(ConfigFileName, FileMode.Open, FileAccess.Read);
            ConfigFileStream.Seek(0, SeekOrigin.Begin);
            ConfigStreamReader = new StreamReader(ConfigFileStream, Encoding.Default);
          

            return ConfigStreamReader;
        }

        /// <summary>
        /// it's legal or not about the config file's format 
        /// </summary>
        /// <returns></returns>
        public override bool CheckFile()
        {
            bool kvPairPass = true;
            //generator the headIndex dictionary
            PutInDictionary(HeadIndexDictionary);

            foreach (KeyValuePair<string,string> perItem in HeadIndexDictionary)
            {
                if (!featureExtracte(perItem.Value))
                {
                    kvPairPass = false;
                }
            }


            //throw the error list
            if (!kvPairPass)
            {
                throwError(errorLog);
            }

            return kvPairPass;
        }


        public Dictionary<string, string> getHeadIndexDictionary()
        {
            return HeadIndexDictionary;
        }

        protected bool featureExtracte(string extractObjects)
        {
            bool featureCheckPass = true;// check pass or no
            char[] splitor = { ',' };//
            string[] objects = extractObjects.Split(splitor);//get all the files
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].IndexOf(".cs") > 0)// just check the cs files
                {
                    if (!File.Exists(@objects[i]))//judge the file is exsist or not
                    {
                        featureCheckPass = false;
                        errorLog.Add(objects[i]);
                    }
                }
            }

            

            return featureCheckPass;
        }

        /// <summary>
        /// throw the error list of check config file
        /// </summary>
        /// <param name="errorlist"></param>
        protected void throwError(List<string> errorlist)
        {
            foreach (var item in errorlist)
            {
                Console.WriteLine("missing:{0}",item);
            }
        }
        

        /// <summary>
        /// put the head index and tail informatin patches in the dictionary
        /// </summary>
        /// <param name="dictionary"></param>
        protected void PutInDictionary(Dictionary<string, string> dictionary)
        {
            HeadString = ConfigStreamReader.ReadLine();//config head
            //config content
            string content = ConfigStreamReader.ReadLine();
            while (content != null)
            {
                int splitPos = content.IndexOf(':');
                string headindex = content.Substring(0, splitPos);//head index
                string tailpatches = content.Substring(splitPos + 1, content.Length - splitPos - 1);//tail patches;

                HeadIndexDictionary.Add(headindex, tailpatches);//add to the Key value pairs to dictionary
                content = ConfigStreamReader.ReadLine();

                if (content.ToUpper() == "END")
                {
                    break;
                }
            }

            EndString = content;//config end

          
        }
    }
}
