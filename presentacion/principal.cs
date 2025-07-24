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
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Marca");
            cboCampo.Items.Add("Categoría");
            cboCampo.Items.Add("Precio");
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
            dgvListadoArticulos.Columns["Id"].Visible = false;
            dgvListadoArticulos.Columns["Precio"].DefaultCellStyle.Format = "$0.00"; ///PARA QUE EL DECIMAL NO SEA TAN LARGO y le agrego el signo $
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
        private void CargarImagen(string imagen)

        {
            try
            {
                pbxImagen.Load(imagen);
            }
            catch (Exception)
            {
                pbxImagen.Load("https://programacion.net/files/article/20161110041116_image-not-found.png");

            }

        }
        private bool ValidarNumeros(string cadena)
        {
            foreach (char caracter in cadena) //ciclo foreach para recorrer cada caracter dentro del texto del filtro. Si encuentra alguno que no sea numero, retorna falso
            {
                if (!(char.IsNumber(caracter))) //si el caracter NO es un numero..
                    return false;
            }
            return true;
        }
        private bool ValidarFiltro()
        {
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el campo para filtrar");
                return true;
            }
            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el criterio para filtrar");
                return true;
            }
            if (cboCampo.SelectedItem.ToString() == "Precio")
            {
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                {
                    MessageBox.Show("Debes cargar el filtro (solo númericos)!");
                    return true;
                }
                if (!(ValidarNumeros(txtFiltroAvanzado.Text)))  //si no son numeros...
                {
                    MessageBox.Show("Por favor, escriba solo números enteros para filtrar por un campo númerico..");
                    return true;
                }
            }

            return false;
        }
        private bool ValidarSeleccion()
        {//metodo para validar que el usuario tenga siempre un objeto seleccionado si quiere eliminar o modificar un articulo (se rompia un poco con el filtro rapido)
            if (dgvListadoArticulos.CurrentRow == null || dgvListadoArticulos.CurrentRow.DataBoundItem == null) //currentRow porque deje que el usuario pueda seleccionar de esta manera las filas
            {
                MessageBox.Show("Debe tener un articulo seleccionado");
                return true;
            }
            return false;
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
            //saque lo de volver a indicar el seleccionado aca, porque si por ejemplo usabas el filtro, ponias cualquier cosa y dabas a modificar se rompia la app
            //de esta manera, te modifica el ultimo item que haya quedado seleccionado (cuya imagen se ve)
            if (ValidarSeleccion()) //chequeo si el usuario tiene un objeto seleccionado
                return;
            info_articulo modificarForm = new info_articulo(articuloSeleccionado);
           modificarForm.ShowDialog();
           Cargar();           
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (ValidarSeleccion()) //chequeo si el usuario tiene un objeto seleccionado
                return;
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();
            articuloSeleccionado = (Articulo)dgvListadoArticulos.CurrentRow.DataBoundItem;
            try
            {

                DialogResult respuesta = MessageBox.Show("¿Desea eliminar el Artículo?", "Eliminando Objeto", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    articuloNegocio.Eliminar(articuloSeleccionado);
                    MessageBox.Show("Artículo eliminado", "Eliminación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                     Cargar();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
               
        }

        private void btnVerDetalle_Click(object sender, EventArgs e)
        {
            if (ValidarSeleccion())
                    return;
            try
            { 
                detalle_articulo verDetalle = new detalle_articulo(articuloSeleccionado);
                verDetalle.ShowDialog();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void lblFiltro_Click(object sender, EventArgs e)
        {

        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            // chequeamos la opcion del primer criterio del filtro, para quede opciones acorde para el sig criterio del filtro
            if (opcion == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Es mayor a ");
                cboCriterio.Items.Add("Es menor a ");
                cboCriterio.Items.Add("Es igual");
            }
            else //son todos los demas textos, asi que no hago mas condiciones, todos comparten el mismo criterio en este caso
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            
                ArticuloNegocio articulo = new ArticuloNegocio();
                try
                {
                    if (ValidarFiltro())
                        return; ; //chequea las validaciones preestablecidas, si alguna retorna true, se sale del try
                    string campo = cboCampo.SelectedItem.ToString();
                    string criterio = cboCriterio.SelectedItem.ToString();
                    string filtro = txtFiltroAvanzado.Text;
                    dgvListadoArticulos.DataSource = articulo.Filtrar(campo, criterio, filtro);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            cboCriterio.Items.Clear();
            cboCampo.Items.Clear();
            txtFiltroAvanzado.Clear();
            txtFiltro.Clear();
            Cargar();
        }
    }
}
