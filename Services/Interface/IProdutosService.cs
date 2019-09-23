using Entities.MagaluApiProdutos;
using Entities.Mongo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interface
{
    public interface IProdutosService
    {
        IEnumerable<Produto> GetProducts(int pagination);
    }
}
