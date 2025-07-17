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
    public partial class info_articulo: Form
    {
        private Articulo articulo = null; //Esto en null para que cada vez que lo cargue comience en null este atributo del form
        public info_articulo()
        {
            InitializeComponent();
            Text = "Agregar Articulo";
        }
        public info_articulo(Articulo articuloSeleccionado)
        {
            InitializeComponent();
            Text = "Modificar Articulo";
            this.articulo = articuloSeleccionado;
        }

        private void info_articulo_Load(object sender, EventArgs e)
        {
           MarcaNegocio marcaNegocio= new MarcaNegocio();
           CategoriaNegocio categoriaNegocio= new CategoriaNegocio();
            try
            {
                cboMarca.DataSource = marcaNegocio.Listar();
                cboMarca.ValueMember = "Id"; //agregar value y display fue necesario para que se preselecciones el tipo y estilo cuando se le da a modificar.
                cboMarca.DisplayMember = "Descripcion";
                cboCategoria.DataSource = categoriaNegocio.Listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";
                if (articulo != null) //si es un Articulo distinto de nulo, es decir, se uso el boton Modificar y llamo al constructor que recibe parametro Art
                {
                    txtCodigo.Text = articulo.CodigoArticulo;
                    txtNombre.Text= articulo.Nombre;
                    txtDescripcion.Text= articulo.Descripcion;
                    txtImagen.Text = articulo.Imagen;
                    nudPrecio.Value = articulo.Precio;
                    cboCategoria.SelectedValue = articulo.Categoria.Id;
                    cboMarca.SelectedValue = articulo.Marca.Id;
                    CargarImagen(articulo.Imagen);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void CargarImagen(string imagen)
        {
            try
            {
                pictureBox1.Load(imagen);
            }
            catch (Exception)
            {

                pictureBox1.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {          
                if (articulo == null)
                {
                    articulo = new Articulo(); //Si no tiene nada pasado por parametro, lo creamos aca.
                }
                articulo.CodigoArticulo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Imagen= txtImagen.Text;
                articulo.Precio = nudPrecio.Value;
                articulo.Categoria= (Categoria)cboCategoria.SelectedItem;
                articulo.Marca= (Marca)cboMarca.SelectedItem;          
                //Sea agregar o modif, hasta aca cargan los campos de igual manera en el item
               if (articulo.Id != 0)
                {
                    //Si el ID es distinto de 0, entonces existia previamente
                    negocio.Modificar(articulo);
                    MessageBox.Show("Articulo modificado con exito");

                }
                else
                {
                    negocio.Agregar(articulo);
                    MessageBox.Show("Articulo agregado con exito");
                }
                    Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txtImagen_Leave(object sender, EventArgs e)
        {
            CargarImagen(txtImagen.Text);
        }
    }
}
