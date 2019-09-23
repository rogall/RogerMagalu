using Entities.MagaluApiProdutos;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Concrete
{
    public class ProdutosService : IProdutosService
    {
        public IEnumerable<Produto> GetProducts(int pagination)
        {
            throw new NotImplementedException();
        }
    }
}
