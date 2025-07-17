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
    }
}

