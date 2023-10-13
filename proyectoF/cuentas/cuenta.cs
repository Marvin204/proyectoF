using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoF.cuentas
{

    public class Cuenta
    {
        public string DNI { get; set; }
        public string PIN { get; set; }
        public decimal Saldo { get; set; }

        public List<transaccion> HistorialTransacciones { get; set; } = new List<transaccion>();

        public class transaccion
        {
            public DateTime Fecha { get; set; }
            public decimal Monto { get; set; }
            public TipoTransaccion Tipo { get; set; }

            public transaccion(decimal monto, TipoTransaccion tipo)
            {
                Fecha = DateTime.Now;
                Monto = monto;
                Tipo = tipo;
            }
        }

        public enum TipoTransaccion
        {
            Retiro,
            Deposito
        }

        public void RegistrarTransaccion(transaccion transaccion)
        {
            HistorialTransacciones.Add(transaccion);
        }

    }
}