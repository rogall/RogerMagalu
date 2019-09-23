using Entities.MagaluApiProdutos;
using Entities.Mongo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IProdutosService
    {
        Task<List<Produto>> GetProdutos(int pagination, string idCliente);
        Task<Produto> GetProduto(string id);
    }
}
