using System;
using System.Linq;
using System.ServiceProcess;

namespace Services
{
    public abstract class ServicesActions
    {
        /// <summary>
        /// Verify if a service exists
        /// </summary>
        /// <param name="ServiceName">Nombre del Servicio</param>
        /// <returns></returns>
        public static bool ServiceExists(string ServiceName)
        {
            return ServiceController.GetServices().Any(serviceController => serviceController.ServiceName.ToLower().Equals(ServiceName.ToLower()));
        }

        /// <summary>
        /// Iniciar un servicio por su nombre
        /// </summary>
        /// <param name="ServiceName"></param>
        public static void StartService(string ServiceName)
        {
            if (ServiceExists(ServiceName))
            {
                ServiceController sc = new ServiceController(ServiceName);

                Console.WriteLine("El estado del servicio {0} está configurado actualmente en {1}", ServiceName, sc.Status.ToString());

                if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    // Inicie el servicio si se detiene el estado actual.
                    Console.WriteLine("Starting the {0} service ...", ServiceName);
                    try
                    {
                        // Inicie el servicio y espere hasta que su estado sea "En ejecución".
                        sc.Start();
                        sc.WaitForStatus(ServiceControllerStatus.Running);

                        // Muestra el estado actual del servicio.
                        Console.WriteLine("El estado del servicio {0} ahora se establece en {1}.", ServiceName, sc.Status.ToString());
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine("No se pudo iniciar el servicio {0}.", ServiceName);
                        Console.WriteLine(e.Message);
                    }
                }
                else
                {
                    Console.WriteLine("El servicio {0} ya se está ejecutando.", ServiceName);
                }
            }
            else
            {
                Console.WriteLine("El servicio proporcionado {0} no existe", ServiceName);
            }
        }

        /// <summary>
        /// Detener un servicio que está activo
        /// </summary>
        /// <param name="ServiceName"></param>
        public static void StopService(string ServiceName)
        {
            if (ServiceExists(ServiceName))
            {
                ServiceController sc = new ServiceController(ServiceName);

                Console.WriteLine("The {0} service status is currently set to {1}", ServiceName, sc.Status.ToString());

                if (sc.Status == ServiceControllerStatus.Running)
                {
                    // Inicie el servicio si se detiene el estado actual.
                    Console.WriteLine("Stopping the {0} service ...", ServiceName);
                    try
                    {
                        // Inicie el servicio y espere hasta que su estado sea "En ejecución".
                        sc.Stop();
                        sc.WaitForStatus(ServiceControllerStatus.Stopped);

                        // Muestra el estado actual del servicio.
                        Console.WriteLine("El estado del servicio {0} ahora se establece en {1}.", ServiceName, sc.Status.ToString());
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine("Could not stop the {0} service.", ServiceName);
                        Console.WriteLine(e.Message);
                    }
                }
                else
                {
                    Console.WriteLine("No se puede detener el servicio {0} porque ya está inactivo.", ServiceName);
                }
            }
            else
            {
                Console.WriteLine("El servicio proporcionado {0} no existe", ServiceName);
            }
        }

        /// <summary>
        /// Reinicia un servicio
        /// </summary>
        /// <param name="ServiceName"></param>
        public static void RebootService(string ServiceName)
        {
            if (ServiceExists(ServiceName))
            {
                if (ServiceIsRunning(ServiceName))
                {
                    StopService(ServiceName);
                }
                else
                {
                    StartService(ServiceName);
                }
            }
            else
            {
                Console.WriteLine("El servicio proporcionado {0} no existe", ServiceName);
            }
        }

        /// <summary>
        ///  Verify if a service is running.
        /// </summary>
        /// <param name="ServiceName"></param>
        public static bool ServiceIsRunning(string ServiceName)
        {
            ServiceController sc = new ServiceController(ServiceName);

            if (sc.Status == ServiceControllerStatus.Running)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
