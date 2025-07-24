using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace presentacion
{
    public partial class detalle_articulo: Form
    {
        private Articulo articulo = null;
        public detalle_articulo( Articulo articulo)
        {

            InitializeComponent();
            this.articulo = articulo;
            
        }

        private void CargarImagen(string imagen)
        {
            try
            {
                pbxImagen.Load(imagen);
            }
            catch (Exception)
            {

                pbxImagen.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void detalle_articulo_Load(object sender, EventArgs e)
        {
            try
            {
                if (articulo != null) //si es un Articulo distinto de nulo, es decir, se uso el boton Modificar y llamo al constructor que recibe parametro Art
                {
                    txtCodigo.Text = articulo.CodigoArticulo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    pbxImagen.Text = articulo.Imagen;
                    txtPrecio.Text= articulo.Precio.ToString("$#,0.00");
                    txtCategoria.Text = articulo.Categoria.Descripcion;
                    txtMarca.Text = articulo.Marca.Descripcion;
                    CargarImagen(articulo.Imagen);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
    }
    
}
