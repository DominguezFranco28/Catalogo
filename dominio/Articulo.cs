﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Articulo
    {
        public int Id{ get; set; }

        [DisplayName("Código")]
        public string CodigoArticulo { get; set; }
        public string Nombre { get; set; }
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public decimal Precio { get; set; } //concatenar cuon $ cuando se llame
        public Marca Marca { get; set; }
        [DisplayName("Categoría")]
        public Categoria Categoria{ get; set; }
        
    }
}
