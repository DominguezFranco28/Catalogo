using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class CategoriaNegocio
    {
        public List<Categoria> Listar()
        {
            List<Categoria> listadoCategoria = new List<Categoria>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("select Id, Descripcion from CATEGORIAS");
                datos.EjecutarLectura();
                while (datos.Lector.Read())
                {
                    Categoria categoria = new Categoria();
                    categoria.Id = (int)datos.Lector["Id"];
                    categoria.Descripcion = (string)datos.Lector["Descripcion"];
                    listadoCategoria.Add(categoria);
                }

                return listadoCategoria;
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
        public void Agregar(Categoria nuevaCategoria)
        {
            if (ValidarExistencia(nuevaCategoria.Descripcion))
            {
                throw new Exception("Ya existe una Categoria con ese nombre " + nuevaCategoria.Descripcion.ToString() + ". Por favor, ingrese otro nombre."); 
            }
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("INSERT INTO CATEGORIAS (Descripcion) values (@descripcion)");
                datos.SetearParametro("@descripcion", nuevaCategoria.Descripcion);
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
        public bool ValidarExistencia (string descripcion)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("Select * FROM CATEGORIAS WHERE Descripcion = @descripcion");
                datos.SetearParametro("@descripcion", descripcion); //seteo el valor de la descripcion
                datos.EjecutarLectura();

                if (datos.Lector.Read()) //Si el lector lee algo, significa que ya existe una categoria con ese nombre
                {
                    return true;
                }
                else
                {
                    return false; //Si no lee nada, no hay problema, se puede agregar la categoria
                }
            }
            catch (Exception ex)
            {
                //throw new exception permite dar un mensaje detallado, ya que este no es un form no pueod usar el message box
                throw new Exception("Error al validar existencia de la categoría", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        
    }
}

