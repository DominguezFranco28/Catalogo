﻿using System;
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
                    articulo.Categoria.Descripcion = (string)datos.Lector["Categoría"];

                    articulo.Marca = new Marca();
                    articulo.Marca.Id = (int)datos.Lector["IdMarca"];
                    articulo.Marca.Descripcion = (string)datos.Lector["Marca"];


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
        public void Agregar(Articulo nuevoArticulo)
        {
            AccesoDatos datos = new AccesoDatos();
            if (ValidarExistencia(nuevoArticulo.CodigoArticulo))
            {
                throw new Exception("Ya existe un articulo con ese codigo de articulo" +  nuevoArticulo.CodigoArticulo + ". Por favor, ingrese otro nombre."); //si ya existe un articulo con ese nombre, se lanza una excepcion y no se agrega nada
                
            }
             //llamo al metodo para validar que no exista un articulo con el mismo nombre, si no existe, se agrega, si existe, se lanza una excepcion y no se agrega nada
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

        public List<Articulo> Filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> listaArticulo = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "select A.Id, Codigo, Nombre, A.Descripcion, ImagenUrl , Precio, M.Descripcion AS Marca, C.Descripcion as Categoría, a.IdCategoria, A.IdMarca from ARTICULOS A, CATEGORIAS C, MARCAS M where A.IdCategoria = C.ID and M.Id = A.IdMarca and ";
                //la base de la consulta es la misma que el metodo listar,
                //ya que son los articulos que estan lsitados los que hay que filtrar. Solo se agrega "and" al final para concatenar el string que sigue en el condicional
                if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "Nombre like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "Nombre like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "Nombre like '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Marca")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "M.Descripcion like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "M.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "M.Descripcion like '%" + filtro + "%'";
                            break;
                    }

                }
                else if (campo == "Categoría")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "C.Descripcion like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "C.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "C.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Es mayor a ":
                            consulta += "ROUND(Precio, 2) > " + filtro; //este casteo de redondeo necesario porque esta con formato money en SQL, y yo lo redondee en la dgv
                            break;
                        case "Es menor a ":
                            consulta += "ROUND(Precio, 2) < " + filtro;
                            break;
                        default:
                            consulta += "ROUND(Precio, 2) =" + filtro;
                            break;
                    }
                }
                //si los botoes de campo y criterio tuvieran mas opciones, podria usarse un switch para esta parte.
                datos.SetearConsulta(consulta);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
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
                    articulo.Categoria.Descripcion = (string)datos.Lector["Categoría"];

                    articulo.Marca = new Marca();
                    articulo.Marca.Id = (int)datos.Lector["IdMarca"];
                    articulo.Marca.Descripcion = (string)datos.Lector["Marca"];


                    listaArticulo.Add(articulo);
                    //misma logica que el metodo listar, pero con filtro aplicado
                }
                return listaArticulo;
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
        public bool ValidarExistencia(string nombre) 
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("Select * FROM Articulos WHERE Codigo = @codigo ");
                datos.SetearParametro("@codigo", nombre); //seteo el valor de la descripcion
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