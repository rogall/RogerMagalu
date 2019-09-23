using Entities.Mongo;
using Entities.Settings;
using EntitiesMongo;
using MongoDB.Driver;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Concrete
{
    public class ClientesService : IClientesService
    {
        private readonly IMongoCollection<Cliente> _clientes;
        private readonly IMongoCollection<ProdutoCliente> _produtosClientes;

        public ClientesService(IMongoDBSettings settings)
        {
            var db = new MongoClient(settings.ConnectionString);           
            var database = db.GetDatabase(settings.DatabaseName);

            _clientes = database.GetCollection<Cliente>(settings.ClientesCollectionName);
            _produtosClientes = database.GetCollection<ProdutoCliente>(settings.ClientesProdutosCollectionName);
        }

        public List<Cliente> GetAllClientes() =>
            _clientes.Find(cliente => true).ToList();

        public Cliente GetClienteById(string id) =>
            _clientes.Find<Cliente>(cliente => cliente.Id == id).FirstOrDefault();

        public Cliente GetClienteByEmail(string email) =>
            _clientes.Find<Cliente>(cliente => cliente.Email == email).FirstOrDefault();

        public Cliente CreateCliente(Cliente cliente)
        {
            _clientes.InsertOne(cliente);
            return cliente;
        }

        public void UpdateCliente(string id, Cliente cliente) =>
            _clientes.ReplaceOne(cl => cl.Id == id, cliente);

        public void DeleteCliente(Cliente cliente) =>
            _clientes.DeleteOne(cl => cl.Id == cliente.Id);

        public ProdutoCliente AddProduto(ProdutoCliente produtoCliente)
        {
            _produtosClientes.InsertOne(produtoCliente);
            return produtoCliente;
        }

        public List<ProdutoCliente> GetProdutosByClienteId(string id) =>
           _produtosClientes.Find(_produtosClientes => _produtosClientes.IdCliente == id).ToList();

    }
}
