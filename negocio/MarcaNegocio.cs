using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class MarcaNegocio
    {
        public List<Marca> Listar()
        {
            List<Marca> listadoMarca = new List<Marca>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("select Id, Descripcion from MARCAS");
                datos.EjecutarLectura();
                while (datos.Lector.Read())
                {
                    Marca marca = new Marca();
                    marca.Id = (int)datos.Lector["Id"];
                    marca.Descripcion = (string)datos.Lector["Descripcion"];
                    listadoMarca.Add(marca);
                }

                return listadoMarca;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
        public void Agregar(Marca nuevaMarca)
        {
            AccesoDatos datos = new AccesoDatos();
            if (ValidarExistencia(nuevaMarca.Descripcion))
            {
                throw new Exception("Ya existe un articulo con ese nombre de Marca " + nuevaMarca.Descripcion.ToString() + ". Por favor, ingrese otro nombre.");
            }
            try
            {
                datos.SetearConsulta("INSERT INTO MARCAS (Descripcion) values (@descripcion)");
                datos.SetearParametro("@descripcion", nuevaMarca.Descripcion);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }

        }
        public bool ValidarExistencia(string descripcion)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("Select * FROM MARCAS WHERE Descripcion = @descripcion");
                datos.SetearParametro("@descripcion", descripcion); //seteo el valor de la descripcion
                datos.EjecutarLectura();

                if (datos.Lector.Read()) //Si el lector lee algo, significa que ya existe una marcca con ese nombre
                {
                    return true;
                }
                else
                {
                    return false; //Si no lee nada, no hay problema, se puede agregar la Marca
                }
            }
            catch (Exception ex)
            {
                //throw new exception permite dar un mensaje detallado, ya que este no es un form no puedo usar el message box
                throw new Exception("Error al validar existencia de la categoría", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
    }
}
