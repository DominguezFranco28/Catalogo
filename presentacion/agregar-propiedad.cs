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
    public partial class agregar_propiedad: Form
    {
        private Categoria categoria = null;
        private Marca marca = null; 
        public agregar_propiedad(Categoria categoria)
        {
            InitializeComponent();
            Text = "Agregar Categoria";
            lblNombre.Text = "Nombre de la Categoría"; //seteo el texto del label para que sea "dinamico"
            this.categoria = categoria;
        }
        public agregar_propiedad(Marca marca)
        {
            InitializeComponent();
            Text = "Agregar Marca";
            lblNombre.Text = "Nombre de la Marca";
            this.marca = marca;
        }


        private void AgregarCategoria(Categoria categoria)
        {
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            try
            {
                string nombre = txtNombreNuevo.Text.Trim(); //Trim metodo para borrar espacios al principio y al final del texto ingresado
               if (string.IsNullOrWhiteSpace(nombre)) //Validacion para que no se pueda agregar una categoria vacia
                {
                    MessageBox.Show("Debe ingresar un nombre para la Categoría");
                    return;

                }
                //tendria que hacer una validacion de numeros
                categoria.Descripcion = nombre; //seteo el nombre de la categoria con el texto ingresado en el textbox
                categoriaNegocio.Agregar(categoria);
                MessageBox.Show(nombre.ToString().ToUpper(), "Categoría agregada con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            catch (Exception ex)
            {

                MessageBox.Show("Ocurrió un error al cargar la Categoría" + ex.ToString());
            }
        }
        private void AgregarMarca (Marca marca)
        {
                MarcaNegocio marcaNegocio = new MarcaNegocio();
                try
                {
                    string nombre = txtNombreNuevo.Text.Trim(); //Trim metodo para borrar espacios al principio y al final del texto ingresado
                    if (string.IsNullOrWhiteSpace(nombre)) //Validacion para que no se pueda agregar una categoria vacia
                    {
                        MessageBox.Show("Debe ingresar un nombre para la Marca");
                        return;

                    }
                    //tendria que hacer una validacion de numeros\

                    marca.Descripcion = nombre; //seteo el nombre de la categoria con el texto ingresado en el textbox
                    marcaNegocio.Agregar(marca);
                MessageBox.Show(nombre.ToString().ToUpper(), "Marca agregada con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {

                    MessageBox.Show("Ocurrió un error al cargar la Marca" + ex.ToString());
                }
            
        }
        //me di cuenta
        //que no es necesario validar si existe una categoria o marca, ya que al momento de agregarla era mas facil validarla en el negocio mismo

        //private bool ValidarCategoria(string nombre)
        //{
        //    CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
        //    if (categoriaNegocio.ValidarExistencia(nombre)) //si la validacion devuelve verdadero, entonces ya existe el nombre
        //    {
        //        return false;
        //    }
        //    else //sino,  no existe asi que se puede crear
        //        return true;
        //}
        //private bool ValidarMarca(string nombre)
        //{
        //    MarcaNegocio marcaNegocio= new MarcaNegocio();
        //    if (marcaNegocio.ValidarExistencia(nombre)) //si la validacion devuelve verdadero, entonces ya existe el nombre
        //    {
        //        return false;
        //    }
        //    else //sino,  no existe asi que se puede crear
        //        return true;
        //}


        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (categoria != null)
            {
                AgregarCategoria(categoria);
                Close();

            }
            else if (marca != null)
            {
                AgregarMarca(marca);
                Close();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
