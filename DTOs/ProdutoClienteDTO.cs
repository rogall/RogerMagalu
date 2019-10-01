using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class ProdutoClienteDTO
    {       
        public string Id { get; set; }
        
        public string IdCliente { get; set; }
        
        public string IdProduto { get; set; }
        
        public string Titulo { get; set; }
        
        public string Imagem { get; set; }
        
        public double Preco { get; set; }

        public string Tipo { get; set; }
    }
}
