using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public abstract class Cmd
    {
        private static string CmdPath = @"C:\Windows\System32\cmd.exe";

        /// <summary>
        /// ejecuta el comando cmd
        /// Utilice el conector de comando por lotes para varios comandos:
        /// <![CDATA[
        /// &: ejecuta dos comandos simultáneamente
        /// |: utilice la salida del comando anterior como entrada del siguiente comando
        /// &&: cuando el comando anterior a && tiene éxito, se ejecuta el comando posterior a &&
        /// ||: cuando el comando anterior a || falla, ejecute el comando después de ||]>
        /// Otro por favor Baidu
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="output"></param>
        public static void RunCmd(string cmd, out string output)
        {
            cmd = cmd.Trim().TrimEnd('&') + "& exit"; // Descripción: El comando de salida se ejecuta independientemente de si el comando es exitoso; de lo contrario, al llamar al método ReadToEnd (), estará en estado muerto
            using (Process p = new Process())
            {
                p.StartInfo.FileName = CmdPath;
                p.StartInfo.UseShellExecute = false; // Si usar el shell del sistema operativo para iniciar
                p.StartInfo.RedirectStandardInput = true; // Aceptar información de entrada del programa de llamada
                p.StartInfo.RedirectStandardOutput = true; // Obtener la información de salida del programa de llamada
                p.StartInfo.RedirectStandardError = true; // Redirige la salida de error estándar
                p.StartInfo.CreateNoWindow = true; // No muestra la ventana del programa
                p.Start(); // Inicia el programa

                // Escribir comando en la ventana de cmd
                p.StandardInput.WriteLine(cmd);
                p.StandardInput.AutoFlush = true;

                // Obtener la información de salida de la ventana cmd
                output = p.StandardOutput.ReadToEnd();
                p.WaitForExit(); // Espera a que el programa termine y salga del proceso
                p.Close();
            }
        }

        public static void HideConsole(string ConsoleName)
        {
            System.Diagnostics.Process HideConsole = new System.Diagnostics.Process();
            HideConsole.StartInfo.UseShellExecute = false;
            HideConsole.StartInfo.Arguments = ConsoleName + " /hid";
            HideConsole.StartInfo.FileName = "cmdow.exe";
            HideConsole.Start();
        }
    }
}
