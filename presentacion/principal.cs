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
        }
        private void principal_Load(object sender, EventArgs e)
        {
            Cargar();
        }

        private void dgvListadoArticulos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvListadoArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvListadoArticulos.CurrentRow != null)
            {
                Articulo articuloSeleccionado = (Articulo)dgvListadoArticulos.CurrentRow.DataBoundItem;
                //Se indica que cada fila (currentRow) tiene un objeto enlazado. Casteo explicito para indicar que son articulos
                //metodo para cargar imagen de disco seleccionado.
            }
        }
    }
}
