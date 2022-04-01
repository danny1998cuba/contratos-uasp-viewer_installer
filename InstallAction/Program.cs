using System;
using System.Threading;
using System.IO;
using MySql.Data.MySqlClient;
using System.Configuration;
using Services;
using Commands;

namespace InstallAction
{
    class Program
    {
        static readonly string CONTAINER = $"{Environment.CurrentDirectory}\\container";

        const string XAMPP_DIRECTION = @"C:\xampp";
        const string HTDOCS = XAMPP_DIRECTION + @"\htdocs";
        const string WEBAPPS = XAMPP_DIRECTION + @"\tomcat\webapps";

        const string WAR = "contracts.war";
        const string SQL = "contracts.sql";
        const string CONTRACTS = "contracts";

        const string MYSQL_BIN = XAMPP_DIRECTION + @"\mysql\bin";

        static void Main(string[] args)
        {
            if (!File.Exists(Path.Combine(WEBAPPS, WAR)))
            {
                Console.WriteLine("Desplegando la API...");
                File.Copy(Path.Combine(CONTAINER, WAR), Path.Combine(WEBAPPS, WAR), true);
                Console.WriteLine("Desplegada la API.\n");
            }
            else
            {
                Console.WriteLine("La API ya se encuentra desplegada.\n");
            }

            if (!Directory.Exists(Path.Combine(HTDOCS, CONTRACTS)))
            {
                Console.WriteLine("Desplegando el visual...");
                CopiarDirectorio(new DirectoryInfo(Path.Combine(CONTAINER, CONTRACTS)), new DirectoryInfo(Path.Combine(HTDOCS, CONTRACTS)));
                Console.WriteLine("Desplegado el visual.\n");
            }
            else
            {
                Console.WriteLine("El visual ya se encuentra desplegado.\n");
            }

            TestServices();

            Console.WriteLine("\nCreando Base de datos...");
            DataBase();
            Console.WriteLine("Base de datos creada.\n");


            Console.WriteLine("Todo instalado!!!");

            Thread.Sleep(1000);
        }

        private static void TestServices()
        {
            if (!ServicesActions.ServiceExists("MySQL"))
            {
                Cmd.RunCmd(@"C:\xampp\mysql\bin\mysqld.exe install", out string outp);
                Console.WriteLine(outp);
            }

            if (!ServicesActions.ServiceExists("Apache2.4"))
            {
                Cmd.RunCmd(@"C:\xampp\apache\bin\httpd.exe -k install", out string outp);
                Console.WriteLine(outp);
            }

            ServicesActions.StartService("MySQL");
            ServicesActions.StartService("Apache2.4");
        }

        private static void DataBase()
        {
            string script = File.ReadAllText(Path.Combine(CONTAINER, SQL));
            MySqlConnection con = null;

            try
            {
                con = new MySqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
                con.Open();
                try
                {
                    new MySqlCommand(script, con).ExecuteNonQuery();
                }
                catch (Exception)
                {
                    Console.WriteLine("Ya existia la base de datos.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if(con!= null)
                    con.Close();
            }

        }

        public static void CopiarDirectorio(DirectoryInfo origen, DirectoryInfo destino)
        {
            // Comprueba que el destino exista:
            if (!destino.Exists)
            {
                destino.Create();
            }

            // Copia todos los archivos del directorio actual:
            foreach (FileInfo archivo in origen.EnumerateFiles())
            {
                archivo.CopyTo(Path.Combine(destino.FullName, archivo.Name), true);
            }

            // Procesamiento recursivo de subdirectorios:
            foreach (DirectoryInfo directorio in origen.EnumerateDirectories())
            {
                // Obtención de directorio de destino:
                string directorioDestino = Path.Combine(destino.FullName, directorio.Name);

                // Invocación recursiva del método `CopiarDirectorio`:
                CopiarDirectorio(directorio, new DirectoryInfo(directorioDestino));
            }
        }

    }
}
