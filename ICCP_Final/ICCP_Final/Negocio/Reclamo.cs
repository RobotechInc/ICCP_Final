using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace ICCP_Final.Negocio
{

    public class Reclamo
    {
        public int Id { get; set; } // tabla reclamo
        public string Nombre { get; set; } // tabla usuario
        public string Apellido { get; set; }
        public string Rut { get; set; }
        public string Email { get; set; }
        public int Telefono { get; set; }
        public int Tipo { get; set; } // tipo reclamo
        public string Comentarios { get; set; } // reclamo
        public DateTime Fecha { get; set; } // reclamo
        public string Pdf { get; set; }
        public int Sla_Id { get; set; }

        public Reclamo(int Id, string Nombre, string Apellido, string Rut, string Email, int Telefono, int Tipo, string Comentarios, DateTime Fecha, string Pdf, int Sla_Id)
        {
            this.Id = Id;
            this.Nombre = Nombre;
            this.Apellido = Apellido;
            this.Rut = Rut;
            this.Email = Email;
            this.Telefono = Telefono;
            this.Tipo = Tipo;
            this.Comentarios = Comentarios;
            this.Fecha = Fecha;
            this.Pdf = Pdf;
            this.Sla_Id = Sla_Id;
        }
        public Reclamo()
        {
            this.Id = 0;
            this.Nombre = "";
            this.Apellido = "";
            this.Rut = "";
            this.Email = "";
            this.Telefono = 0;
            this.Tipo = 1;
            this.Comentarios = "";
            this.Fecha = DateTime.Now;
            this.Pdf = "";
            this.Sla_Id = 1;
        }
    }
}