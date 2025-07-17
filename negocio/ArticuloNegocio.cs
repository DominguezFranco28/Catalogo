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
                datos.SetearConsulta("select A.Id, Codigo, Nombre, A.Descripcion, ImagenUrl , Precio, M.Descripcion AS Marca, C.Descripcion as Categoría, a.IdCategoria, A.IdMarca from ARTICULOS A, CATEGORIAS C, MARCAS M where A.IdCategoria = C.ID and M.Id = A.IdMarca ");
                datos.EjecutarLectura();
                while (datos.Lector.Read()) //Read pasa al siguiente registro de la lista de lo que este leyendo en la DB. Mientras haya un articulo en la base de datos, sigue en el while
                {
                    Articulo articulo = new Articulo();
                    articulo.Id = (int)datos.Lector["Id"];
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
        public void Agregar (Articulo nuevoArticulo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("insert into ARTICULOS  (Codigo , Nombre, Descripcion, ImagenUrl, Precio, IdCategoria, IdMarca) values (@codigo, @nombre, @descripcion, @imagenUrl, @precio, @idCategoria, @idMarca)");
                datos.SetearParametro("@codigo", nuevoArticulo.CodigoArticulo);
                datos.SetearParametro("@nombre", nuevoArticulo.Nombre);
                datos.SetearParametro("@descripcion", nuevoArticulo.Descripcion);
                datos.SetearParametro("@imagenUrl", nuevoArticulo.Imagen);
                datos.SetearParametro("@precio", nuevoArticulo.Precio);
                datos.SetearParametro("@idCategoria", nuevoArticulo.Categoria.Id);
                datos.SetearParametro("@idMarca", nuevoArticulo.Marca.Id);
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

        public void Modificar(Articulo articuloModificar)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("Update ARTICULOS set Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, ImagenUrl = @imagenUrl, Precio = @precio, IdCategoria = @idCategoria, IdMarca = @idMarca where Id = @id");
                datos.SetearParametro("@codigo", articuloModificar.CodigoArticulo);
                datos.SetearParametro("@nombre", articuloModificar.Nombre);
                datos.SetearParametro("@descripcion", articuloModificar.Descripcion);
                datos.SetearParametro("@imagenUrl", articuloModificar.Imagen);
                datos.SetearParametro("@precio", articuloModificar.Precio);
                datos.SetearParametro("@idCategoria", articuloModificar.Categoria.Id);
                datos.SetearParametro("@idMarca", articuloModificar.Marca.Id);
                datos.SetearParametro("@id", articuloModificar.Id);
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

        public void Eliminar(Articulo articuloSeleccionado)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("delete from ARTICULOS where Id = @id");
                datos.SetearParametro("@id", articuloSeleccionado.Id);
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
    }
 
}
