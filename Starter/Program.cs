using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Commands;
using Services;
using System.Net;
using System.IO;

namespace Starter
{
    class Program
    {
        const string XAMPP_INSTALL_FOLDER = "C:/xampp";
        const string TOMCAT_LOCATION = XAMPP_INSTALL_FOLDER + "/tomcat/bin";

        static void Main(string[] args)
        {
            Console.Title = "Starter";

            if (IsXamppInstalled())
            {
                RunServices();

                Thread t = new Thread(() =>
                {
                    Cmd.RunCmd(@"cd " + TOMCAT_LOCATION + "&& catalina.bat start", out string outp);
                });

                Thread t2 = new Thread(() =>
                {
                    if (APIRunning())
                    {
                        Console.WriteLine("La API ya esta iniciada");
                    }
                    else
                    {
                        Console.WriteLine("Iniciando la API... Por favor, no cierre las ventanas emergentes.");
                        t.Start();
                        while (APIRunning())
                        {
                            break;
                        }
                        Cmd.HideConsole("Tomcat");
                        Console.WriteLine("API iniciada.");
                    }
                    Cmd.RunCmd("start http://localhost/contracts/", out string outp);
                    Environment.Exit(0);
                });
                t2.Start();
            }
            else
            {
                Console.WriteLine("El servidor XAMPP no esta instalado...");
                Console.ReadKey();
            }
        }

        public static bool IsXamppInstalled()
        {
            return Directory.Exists(XAMPP_INSTALL_FOLDER);
        }

        public static void RunServices()
        {
            ServicesActions.StartService("MySQL");
            ServicesActions.StartService("Apache2.4");
        }

        public static bool APIRunning()
        {
            var url = $"http://localhost:8080/contracts/";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";

            try
            {
                using (WebResponse response = request.GetResponse())

                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return false;
                        else return true;
                    }
                }
            }
            catch (WebException)
            {
                return false;
            }

        }
    }
}
