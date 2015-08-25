/*
生成一个命令文件，这些命令包含了每一个从CS->DLL的命令
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MultiCompile
{
    
    public class ConfigTree 
    {


        public string GenerateFolder = "Debug/";//the folder of the target file will be crated
        public string IntermediateFileName = "intermediate_a.out";//intermediate file's name

        protected Stack<string> confTreeArgumentStack = new Stack<string>();
        /// <summary>
        /// The enum of the target generated file
        /// </summary>
        public enum GenerateFileState : byte
        {
            EXISTS = 0,
            NOFILE,
        }
       

        /// <summary>
        /// the config file object
        /// </summary>
        private ConfigFile configFileObj;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cfgFileObj"></param>
        public ConfigTree(ConfigFile cfgFileObj)
        {
            if (cfgFileObj != null)
            {
                configFileObj = cfgFileObj;
               
            }
            else
            {
                throw  new NullReferenceException("Null referece of config file.");
            }
        }


        public Stack<string> getArgumentsStack()
        {
            return confTreeArgumentStack;
        }

        /// <summary>
        /// check the target file exist or not
        /// </summary>
        /// <param name="destidll"></param>
        /// <returns></returns>
        bool CheckDestiExist(string destidll)
        {
            if (File.Exists(@destidll))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// generate per command acording to the HeadIndexDictionary
        /// </summary>
        /// <param name="perItem"></param>
        /// <returns></returns>
        protected string createPerArgument(KeyValuePair<string, string> perItem)
        {
            if (CheckDestiExist(perItem.Key))
            {
                return GenerateFileState.EXISTS.ToString();
            }


            if (perItem.Key.IndexOf(".exe") != -1)
            {
                return " /t:exe   /out:./" + GenerateFolder + perItem.Key.ToString() + " " + ModReference(perItem.Value) ;
            }


            ///check where

            return " /t:module   /out:./" + GenerateFolder + perItem.Key.ToString() + "  " + ModReference(perItem.Value);
        }



        protected string ModReference(string Arguments)
        {
            char[] split = {',' };
            string[] tempArgs = Arguments.Split(split);
            string mods = "";
            string files = "";
            for (int i = 0; i < tempArgs.Length; i++)
            {
                //need add the dll module to generate target program
                if (tempArgs[i].IndexOf(".dll") != -1)
                {
                    mods += "/addmodule:./" + GenerateFolder + tempArgs[i] + "  ";
                }
                else if(tempArgs[i].IndexOf(".cs")!=-1)
                {
                    files += tempArgs[i] + "  ";
                }
            }


            return mods + files;
        }

        /// <summary>
        /// create all the command 
        /// </summary>

        public void CreateAllArguments()
        {
            Dictionary<string, string> HeadIndexDictionary = configFileObj.getHeadIndexDictionary();

            foreach (KeyValuePair<string,string> perItem in HeadIndexDictionary)
            {
               
                confTreeArgumentStack.Push(createPerArgument(perItem));
            }

        }
        

        /// <summary>
        /// generate the intermediate file
        /// </summary>
        public void createIntermediateFile()
        {
            FileStream createFileStream;
            if (CheckDestiExist(IntermediateFileName))
            {
                File.Delete(IntermediateFileName);//delete the old intermediate file         
            }

            createFileStream = new FileStream(IntermediateFileName, FileMode.Create, FileAccess.Write);
            createFileStream.Seek(0, SeekOrigin.Begin);
            StreamWriter confTreeStreamWrite = new StreamWriter(createFileStream, Encoding.Default);

            foreach (string percommand in confTreeArgumentStack)
            {
                confTreeStreamWrite.WriteLine(percommand);
            } 
                     
            confTreeStreamWrite.Close();
            createFileStream.Close();

        }
        
    }
}
