using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace presentacion
{
    public partial class principal: Form
    {
        private List<Articulo> listaArticulos;
        private Articulo articuloSeleccionado;
        public principal()
        {
            InitializeComponent();
        }

        private void Cargar()
        {
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();
            try
            {
                listaArticulos = articuloNegocio.ListarArticulos(); //Llamo al metodo ListarArticulo para asignarle ese contenido a la lista de esta clase, y que posteriormente lo muestre en la grilla
                dgvListadoArticulos.DataSource = listaArticulos;
                CargarImagen(listaArticulos[0].Imagen);//indice 0 para que elija el primer elemento de la lista y comience con una imagen cargada.
                OcultarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void OcultarColumnas()
        {
            dgvListadoArticulos.Columns["Imagen"].Visible = false;
            dgvListadoArticulos.Columns["Precio"].DefaultCellStyle.Format = "$#,0.00"; //PARA QUE EL DECIMAL NO SEA TAN LARGO y le agrego el signo $
        }
        private void Filtrar()
        {
            List<Articulo> listaFiltrada;
            string filtro = txtFiltro.Text;
            if (filtro != "") //filtro sencillo, solo por Nombre o Codigo
            {
                listaFiltrada = listaArticulos.FindAll(articulo => articulo.Nombre.ToUpper().Contains(filtro.ToUpper()) || articulo.CodigoArticulo.ToUpper().Contains(filtro.ToUpper()));  //Filtro por nombre                
            }
            else
            {
                listaFiltrada = listaArticulos;
            }
            dgvListadoArticulos.DataSource = null;
            dgvListadoArticulos.DataSource = listaFiltrada;
            OcultarColumnas();  

        }
        private void CargarImagen( string imagen)

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
        private void principal_Load(object sender, EventArgs e)
        {
            Cargar();
        }

        private void dgvListadoArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvListadoArticulos.CurrentRow != null)
            {
                articuloSeleccionado = (Articulo)dgvListadoArticulos.CurrentRow.DataBoundItem;
                //Se indica que cada fila (currentRow) tiene un objeto enlazado. Casteo explicito para indicar que son articulos
                CargarImagen(articuloSeleccionado.Imagen);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Filtrar();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            info_articulo agregarForm = new info_articulo();
            agregarForm.ShowDialog();
            Cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            
           articuloSeleccionado = (Articulo)dgvListadoArticulos.CurrentRow.DataBoundItem;
           info_articulo modificarForm = new info_articulo(articuloSeleccionado);
           modificarForm.ShowDialog();
           Cargar();           
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();
            articuloSeleccionado = (Articulo)dgvListadoArticulos.CurrentRow.DataBoundItem;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Desea eliminar el Disco?", "Eliminando Objeto", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    articuloNegocio.Eliminar(articuloSeleccionado);
                    MessageBox.Show("Disco eliminado", "Eliminación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                     Cargar();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }
    }
}
