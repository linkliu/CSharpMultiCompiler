using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MultiCompile
{
    public class Generator
    {

        /// <summary>
        /// the object of ConfigTree
        /// </summary>
        public static ConfigTree configTree = null;

        protected Process pGenerator = new Process();

        /// <summary>
        /// create the Component of final Program
        /// </summary>
        public void CreateComponent()
        {


            InitGenerator();

            if (configTree == null)
            {
                Console.WriteLine("Init the configTree first.");
            }

            Stack<string> arguments = configTree.getArgumentsStack();
            foreach (string perarg  in arguments)
            {
                pGenerator.StartInfo.Arguments = perarg;
                pGenerator.Start();//generate the component of final program

                string outputInfo = pGenerator.StandardOutput.ReadToEnd();//get cmd's output information 
                Console.Write(outputInfo);
                
            }


        }


        /// <summary>
        /// Init the argument of Generator Process
        /// </summary>
        protected void InitGenerator()
        {
            pGenerator.StartInfo.FileName = "csc.exe";
            pGenerator.StartInfo.RedirectStandardOutput = true;
            pGenerator.StartInfo.UseShellExecute = false;   

        }


    }
}
