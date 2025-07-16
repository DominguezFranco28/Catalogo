using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> ListarArticulos()
        {
            List<Articulo> listaArticulos = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("select Codigo, Nombre, A.Descripcion, ImagenUrl , Precio, M.Descripcion AS Marca, C.Descripcion as Categoría, a.IdCategoria, A.IdMarca from ARTICULOS A, CATEGORIAS C, MARCAS M where A.IdCategoria = C.ID and M.Id = A.IdMarca ");
                datos.EjecutarLectura();
                while (datos.Lector.Read()) //Read pasa al siguiente registro de la lista de lo que este leyendo en la DB. Mientras haya un articulo en la base de datos, sigue en el while
                {
                    Articulo articulo = new Articulo();
                    articulo.CodigoArticulo = (string)datos.Lector["Codigo"];
                    articulo.Nombre = (string)datos.Lector["Nombre"];
                    articulo.Descripcion = (string)datos.Lector["Descripcion"];
                    articulo.Imagen = (string)datos.Lector["ImagenUrl"];
                    articulo.Precio = (decimal)datos.Lector["Precio"];

                    articulo.Categoria = new Categoria();
                    articulo.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    articulo.Categoria.Descripcion= (string)datos.Lector["Categoría"];  
                    
                    articulo.Marca = new Marca();
                    articulo.Marca.Id = (int)datos.Lector["IdMarca"];
                    articulo.Marca.Descripcion= (string)datos.Lector["Marca"];


                    listaArticulos.Add(articulo);
                }
                return listaArticulos; //Cuando sale del while xq no hay mas datos, retornar la lista de articulos cargados.

            
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
