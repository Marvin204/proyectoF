using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using proyectoF.cuentas;
using Newtonsoft.Json;
using System.Security.Principal;

namespace proyectoF
{


    class Program
    {
       
        static void Main()
        {
          
            Console.WriteLine("Cajero Automatico");
            
            //Se encierra el menu principal en un bucle while
            while (true)
            {
                Console.WriteLine("\nSeleccione una opción:");
                Console.WriteLine("1. Crear cuenta");
                Console.WriteLine("2. Iniciar sesión");
                Console.WriteLine("3. Salir");

                // se le asigna un espacio de memoria a la variable de seleccion
                int opcion = Convert.ToInt32(Console.ReadLine());
                
                // se crea un switch para seleccion de las distantas opciones como metodos
                // se utilizan metodos para facilitar la lectura, comprnsion e identificacion de errore en el codigo
                switch (opcion)
                {
                    case 1:
                        CrearCuenta();
                        break;
                    case 2:
                        IniciarSesion();
                        break;
                    case 3:
                        Console.WriteLine("Gracias por preferirnos");
                        return;
                    default:
                        Console.WriteLine("Opción inválida. Por favor, seleccione una opción válida.");
                        break;
                }
            }
        }

        // se crea el metodo CrearCuenta 
        public static void CrearCuenta()
        {
            string path = @"C:\Users\User\Desktop\Proyec. Algoritmos\proyectoF\proyectoF\cuentas\cuentas.json";
            Console.WriteLine("\nCreación de una nueva cuenta.");
            Console.WriteLine("\nCreación de una nueva cuenta.");
            Console.WriteLine("Ingrese su DNI:");
            string dni = Console.ReadLine();
            Console.WriteLine("Ingrese su PIN:");
            string pin = Console.ReadLine(); Console.WriteLine("Ingrese su saldo inicial:");
            decimal saldoInicial = Convert.ToDecimal(Console.ReadLine());
            List<Cuenta> cuentas = new List<Cuenta>();
            try
            {
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    cuentas = JsonConvert.DeserializeObject<List<Cuenta>>(json);
                    foreach (var cuenta in cuentas)
                    {
                        if (cuenta.DNI == dni && cuenta.PIN == pin)
                        {
                            Console.WriteLine("La cuenta ya existe, elige otro PIN");
                            return;
                        }
                    }
                }

                Cuenta nuevaCuenta = new Cuenta
                {
                    DNI = dni,
                    PIN = pin,
                    Saldo = saldoInicial
                };

                cuentas.Add(nuevaCuenta);

                string jsonToWrite = JsonConvert.SerializeObject(cuentas);
                File.WriteAllText(path, jsonToWrite);

                Console.WriteLine("Cuenta creada exitosamente.");
            }
            catch (IOException e)
            {
                Console.WriteLine("Error al escribir en el archivo: " + e.Message);
            }
        }



        public static void IniciarSesion()
        {
            string path = @"C:\Users\User\Desktop\Proyec. Algoritmos\proyectoF\proyectoF\cuentas\cuentas.json";
            Console.WriteLine("\nInicio de sesión.");

            Console.WriteLine("Ingrese su DNI:");
            string dni = Console.ReadLine();

            Console.WriteLine("Ingrese su PIN:");
            string pin = Console.ReadLine();

            try
            {
                if (!File.Exists(path))
                {
                    Console.WriteLine("La cuenta no existe, crea una cuenta.");
                    return;
                }
                string json = File.ReadAllText(path);
                List<Cuenta> cuentas = JsonConvert.DeserializeObject<List<Cuenta>>(json);
                foreach (var cuenta in cuentas)
                {
                    if (cuenta.DNI == dni && cuenta.PIN == pin)
                    {
                        {
                            Console.WriteLine("\nInicio exitoso");
                            Console.WriteLine("1. Consulta de saldo");
                            Console.WriteLine("2. Retiro");
                            Console.WriteLine("3. Deposito");
                            Console.WriteLine("4. Historial");
                            Console.WriteLine("5. Cerrar secion");
                            int opcion = Convert.ToInt32(Console.ReadLine());
                            switch (opcion)
                            {
                                case 1:
                                    ConsultarSaldo(cuenta);
                                    break;
                                case 2:
                                    RealizarRetiro(cuenta, cuentas);
                                    break;
                                case 3:
                                    RealizarDeposito(cuenta, cuentas);
                                    break;
                                case 4:
                                    VerHistorial(cuenta);
                                   
                                    break;
                                case 5:

                                    // convierte la lista de cuentas  a un formato JSON y luego escribir ese JSON en un archivo en el disc0
                                    string nuevoJson = JsonConvert.SerializeObject(cuentas);
                                    File.WriteAllText(path, nuevoJson);
                                    return; 
                                default:
                                    Console.WriteLine("Opción inválida. Por favor, seleccione una opción válida.");
                                    break;
                            }
                        }
                    } 
                }
                //esepciones 
                Console.WriteLine("Cuenta no encontrada.");
            }
            catch (IOException e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        //Este método recibe un parámetro cuenta de tipo Cuenta.
        public static void ConsultarSaldo(Cuenta cuenta)
        {
            //muestra el saldo actual, almacenado como saldo en la clase cuenta.
            Console.WriteLine($"Tu saldo actual es: Q{cuenta.Saldo}");
        }

        // ete método recibe dos parámetros: cuenta de tipo Cuenta y cuentas de tipo List<Cuenta>.
        public static void RealizarRetiro(Cuenta cuenta, List<Cuenta> cuentas)
        {
            Console.WriteLine("Ingrese la cantidad que desea retirar:");
            decimal cantidad = Convert.ToDecimal(Console.ReadLine());
            if (cantidad > 0 && cantidad <= cuenta.Saldo)
            {
                cuenta.Saldo -= cantidad;
                cuenta.RegistrarTransaccion(new Cuenta.transaccion(cantidad, Cuenta.TipoTransaccion.Retiro));
                Console.WriteLine($"Has retirado Q{cantidad}. Tu saldo actual es: Q{cuenta.Saldo}.");
                ActualizarArchivoJson(cuentas);
            }
            else
            {
                Console.WriteLine("Cantidad no válida o insuficiente saldo.");
            }
        }

        public static void RealizarDeposito(Cuenta cuenta, List<Cuenta> cuentas)
        {
            Console.WriteLine("Ingrese la cantidad que desea depositar:");
            decimal cantidad = Convert.ToDecimal(Console.ReadLine());

            if (cantidad > 0)
            {
                cuenta.Saldo += cantidad;
                cuenta.RegistrarTransaccion(new Cuenta.transaccion(cantidad, Cuenta.TipoTransaccion.Deposito));
                Console.WriteLine($"Has depositado Q{cantidad}. Tu saldo actual es: Q{cuenta.Saldo}.");
                ActualizarArchivoJson(cuentas);
            }
            else
            {
                Console.WriteLine("Cantidad no válida.");
            }
        }
        public static void VerHistorial(Cuenta cuenta)
        {
            Console.WriteLine("Historial de Transacciones:");
            foreach (var transaccion in cuenta.HistorialTransacciones)
            {
                Console.WriteLine($"Fecha: {transaccion.Fecha}, Monto: {transaccion.Monto}, Tipo: {transaccion.Tipo}");
            }
        }

        public static void ActualizarArchivoJson(List<Cuenta> cuentas)
        {
            string path = @"C:\Users\User\Desktop\Proyec. Algoritmos\proyectoF\proyectoF\cuentas\cuentas.json";

            try
            {
                
                string nuevoJson = JsonConvert.SerializeObject(cuentas);
                File.WriteAllText(path, nuevoJson);
            }
            catch (IOException e)
            {
                Console.WriteLine("Error al actualizar el archivo JSON: " + e.Message);
            }
        }

    }
}