﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.MagaluApiProdutos
{
    public class Produto
    {        
        public string IdProduto { get; set; }
        
        public string Titulo { get; set; }
        
        public string Imagem { get; set; }
       
        public double Preco { get; set; }

        public bool IsFavorito { get; set; }
    }
}
