using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Threading; 

namespace AlphamaConverter
{
    static class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool SetConsoleOutputCP(uint wCodePageID);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

           // Converter ct = null;
            

            //string output = "";

            //if (args.Count() != 0)
            //{

            //    if (args[0] != "" && args[0] != null)
            //    {
            //        ct = new Converter(args[0], ref output);
            //    }
                
               

            //}
            //else
            //{
            //    ct = new Converter();
                
            //}

            Application.Run(new Converter());

            //Application.Run(new MultiFormContext(ct, ct1));



           // SetConsoleOutputCP(65001);
            //Console.OutputEncoding = System.Text.Encoding.UTF8;




           // Console.WriteLine(output);

        }

        public class MultiFormContext : ApplicationContext
        {
            private int openForms;
            public MultiFormContext(params Form[] forms)
            {
                openForms = forms.Length;

                foreach (var form in forms)
                {
                    form.FormClosed += (s, args) =>
                    {
                        //When we have closed the last of the "starting" forms, 
                        //end the program.
                        if (Interlocked.Decrement(ref openForms) == 0)
                            ExitThread();
                    };

                    form.Show();
                }
            }
        }

        //static void RestartApp(int pid, string applicationName)
        //{
        //    // Wait for the process to terminate
        //    Process process = null;
        //    try
        //    {
        //        process = Process.GetProcessById(pid);
        //        process.WaitForExit(1000);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        // ArgumentException to indicate that the 
        //        // process doesn't exist?   LAME!!
        //    }
        //    Process.Start(applicationName, "");
        //}

       
    }
}