using Entities.Mongo;
using EntitiesMongo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interface
{
    public interface IClientesService
    {
        List<Cliente> GetAllClientes();
        Cliente GetClienteById(string id);
        Cliente CreateCliente(Cliente user);
        void UpdateCliente(string id, Cliente cliente);
        void DeleteCliente(Cliente cliente);
        ProdutoCliente AddProduto(ProdutoCliente produto);
        List<ProdutoCliente> GetProdutosByClienteId(string id);
        Cliente GetClienteByEmail(string email);
    }
}
