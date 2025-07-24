using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;
using System.Data.Sql;
using System.Data.SqlClient;

namespace negocio
{
    public class AccesoDatos
    {
        private SqlCommand comando;
        private SqlDataReader lector;
        private SqlConnection conexion;
        public AccesoDatos()
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true");
            comando = new SqlCommand();
        }
        public SqlDataReader Lector { get { return lector; } } //PP para leer el lector desde otras clases
        public void SetearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text; //Sentencia sql a usar, tipo text el mas comun
            comando.CommandText = consulta;
        }

        public void EjecutarLectura()
        {
            comando.Connection = conexion; //Se conecta a la DB
            try
            {
                conexion.Open(); //Abrimos conexion
                lector = comando.ExecuteReader(); // Manda el comando, devuelve un SqlDataReader, se asigna a la variable lector
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar lectura de datos.", ex);
            }
        }

        public void EjecutarAccion() //Para realizar cambios en la DB, no solo un comando de lectura
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }

        public void CerrarConexion()
        {
            if (lector != null)
                lector.Close();
            conexion.Close();
        }

    }
}
