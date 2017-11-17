using ICCP_Final.Negocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ICCP_Final.Datos
{
    public class Datos
    {
        private const string Str =
                "Server= localhost; Database=ICCP_Final; Integrated Security=SSPI;";

        public int GetLastReclamo() // la función siguiente devuelve un INTEGER (count) , tiene como objetivo obtener el número mayor de ID en la tabla Reclamos para hacer ingresos adecuados
        {
            var count = 0; // parte count en 0
            using (SqlConnection conn = new SqlConnection(Str)) // se abre conexión
            {
                conn.Open();
                var consulta = "SELECT MAX(id) FROM reclamo"; // usamos la consulta MAX(id)
                var cmd = new SqlCommand(consulta, conn);
                var reader = cmd.ExecuteReader();
                reader.Read(); // leemos
                count = reader.GetInt32(0); // guardamos en count
            }
            return count; // retornamos count
        }

        public void IngresoReclamo(Reclamo rec) // Ingreso de reclamos, recibe un Reclamo sin retornar algo a cambio, sin embargo puede generar excepción
        // tiene como función ejecutar un procedimiento almacenado para registrar un reclamo
        {
            using (var conn = new SqlConnection(Str))
            {
                /*
                 *    @id INTEGER,
                      @nombre VARCHAR(20),
                      @apellido VARCHAR(20),    
                      @rut VARCHAR(12),
                      @email VARCHAR(100), 
                      @telefono INTEGER, 
                      @area INTEGER, 
                      @comentarios text,
                      @fecha DATETIME,
                      @pdf VARCHAR(100),
                      @sla_id INTEGER,
                      @ip VARCHAR(100)
                      
                      Se llama a procedimiento almacenado en SQL Server con la estructura anterior, pasando los datos parametrizados
                 */
                var cmd = new SqlCommand("IngresoReclamo", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("id", rec.Id);
                cmd.Parameters.AddWithValue("nombre", rec.Nombre);
                cmd.Parameters.AddWithValue("apellido", rec.Apellido);
                cmd.Parameters.AddWithValue("rut", rec.Rut);
                cmd.Parameters.AddWithValue("email", rec.Email);
                cmd.Parameters.AddWithValue("telefono", rec.Telefono);
                cmd.Parameters.AddWithValue("area", rec.Tipo);
                cmd.Parameters.AddWithValue("comentarios", rec.Comentarios);
                cmd.Parameters.AddWithValue("fecha", rec.Fecha);
                cmd.Parameters.AddWithValue("pdf", rec.Pdf);
                cmd.Parameters.AddWithValue("sla_id", rec.Sla_Id);
                cmd.Parameters.AddWithValue("ip", "192.168.0.1"); // se envía IP local para entorno de pruebas


                try
                { // se envía la query
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar reclamo " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }


        }
        public List<Reclamo> ListadoReclamos() // Función ListadoReclamos, retorna una Lista (l) con todos los reclamos existentes en la base de datos
        {
            var l = new List<Reclamo>(); // generamos la variable Listado
            using (var conn = new SqlConnection(Str))
            {
                // ejecutamos la consulta
                var consulta = "SELECT reclamo.id, usuarios.nombre, usuarios.apellido, usuarios.rut, usuarios.correo, usuarios.telefono, tipoReclamo.id, reclamo.comentarios, reclamo.fecha, reclamo.pdf, reclamo.sla_id  from reclamo INNER JOIN usuarios on reclamo.rut=usuarios.rut INNER JOIN tipoReclamo on reclamo.area=tipoReclamo.id";
                var cmd = new SqlCommand(consulta, conn);
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read()) // guardamos todos los datos en un reclamo (r)
                {
                    var ide = Convert.ToInt32(reader[0]);
                    var nom = reader[1].ToString();
                    var ape = reader[2].ToString();
                    var rut = reader[3].ToString();
                    var ema = reader[4].ToString();
                    var tel = Convert.ToInt32(reader[5]);
                    var tip = Convert.ToInt32(reader[6].ToString());
                    var com = reader[7].ToString();
                    var fec = Convert.ToDateTime(reader[8]);
                    var pdf = reader[9].ToString();
                    var sla = Convert.ToInt32(reader[10]);
                    var r = new Reclamo(ide, nom, ape, rut, ema, tel, tip, com, fec, pdf, sla);
                    l.Add(r); // agregamos cada reclamo r a nuestra lista (l)
                }
            }
            return l; // retornamos la lista
        }

        public Reclamo GetReclamo(int id) // Función GetReclamo, retorna un Reclamo (r) y recibe un Id, con el cuál se ubica el reclamo en la base de datos
        {
            Reclamo r = null; // creamos un reclamo nulo para empezar
            using (var conn = new SqlConnection(Str)) // iniciamos la consulta relacional
            {
                string consulta = "SELECT reclamo.id, usuarios.nombre, usuarios.apellido, usuarios.rut, usuarios.correo, usuarios.telefono, tipoReclamo.id, reclamo.comentarios, reclamo.fecha, reclamo.pdf, reclamo.sla_id  from reclamo INNER JOIN usuarios on reclamo.rut=usuarios.rut INNER JOIN tipoReclamo on reclamo.area=tipoReclamo.id where reclamo.id = " + id;
                var cmd = new SqlCommand(consulta, conn);
                conn.Open();
                var reader = cmd.ExecuteReader(); // ejecutamos los parámetros
                while (reader.Read()) // leemos los datos
                { // y guardamos el reclamo en la variable r
                    var ide = Convert.ToInt32(reader[0]);
                    var nom = reader[1].ToString();
                    var ape = reader[2].ToString();
                    var rut = reader[3].ToString();
                    var ema = reader[4].ToString();
                    var tel = Convert.ToInt32(reader[5]);
                    var tip = Convert.ToInt32(reader[6].ToString());
                    var com = reader[7].ToString();
                    var fec = Convert.ToDateTime(reader[8]);
                    var pdf = reader[9].ToString();
                    var sla = Convert.ToInt32(reader[10]);
                    r = new Reclamo(ide, nom, ape, rut, ema, tel, tip, com, fec, pdf, sla);
                }
            }
            return r; // retornamos r
        }


        /* Función no operativa
        public string EditarReclamo(Reclamo rec)
        {
            var result = "Error al intentar conectar a la base de datos";
            // imprime string
            try
            {
                using (var conexion = new SqlConnection(Str))
                {
                    var fecha = rec.Fecha.Date.ToString("s");
                    var consulta = "update Reclamo set Nombre=\'"+rec.Nombre+"\',Apellido=\'"+rec.Apellido+"\',Rut=\'"+rec.Rut+"\',Telefono="+rec.Telefono+",Tipo=\'"+rec.Tipo+"\',Comentarios=\'"+rec.Comentarios+"\',Fecha=\'"+fecha+"\' where Id = "+rec.Id;
                    var cmd = new SqlCommand(consulta, conexion);
                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    result = "Editado correctamente";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
        */

        public string ImprimirReclamo(Reclamo rec) // función para entorno de pruebas
        {
            var retorno = "ID: " + rec.Id + " | Nombre: " + rec.Nombre + " | Rut: " + rec.Rut + " | Comentarios: " +
                          rec.Comentarios + " | Fecha: " + rec.Fecha + " | PDF: " + rec.Pdf;
            return retorno;
        }
    }
}