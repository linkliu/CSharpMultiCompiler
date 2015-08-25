using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiCompile
{
    class Program
    {
        static void Main(string[] args)
        {

            //get the file of makeing program
            string makefile = args[0].Length == 0 ? "" : args[0];

            Console.WriteLine("Construct the Config file");
            ConfigFile config = new ConfigFile(makefile);
            config.ReadFile();
            if (config != null)
            {
                if (config.CheckFile())
                {
                    Console.WriteLine("Check file successful.");
                }

            }
            else
            {
                Console.WriteLine("Read config file fail.");
            }


            ConfigTree cgTree = new ConfigTree(config);

            cgTree.CreateAllArguments();
            cgTree.createIntermediateFile();
            Console.WriteLine("Intermediate file created.");
            Console.WriteLine("Beging generator component.......");
            Generator.configTree = cgTree;
            Generator g = new Generator();
            g.CreateComponent();
            Console.WriteLine("gerate process has finished");

            Console.WriteLine("CSharpMultiFileCompiler test.");
            Console.ReadKey();
        }
    }
}
