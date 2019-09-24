using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.MagaluApiProdutos;
using Entities.Mongo;
using EntitiesMongo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Concrete;
using Services.Interface;

namespace ApiMagalu.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize("Bearer")]
    public class ClientesController : ControllerBase
    {
        private readonly IClientesService _clientesService;
        private readonly IProdutosService _produtosService;

        public ClientesController(ClientesService clientesService, ProdutosService produtosService)
        {
            _clientesService = clientesService;
            _produtosService = produtosService;
        }

        [HttpGet]
        public ActionResult<List<Cliente>> Get()
        {           
            var ret = _clientesService.GetAllClientes();
            return ret;
        }

        [HttpGet("{id:length(24)}", Name = "GetCliente")]
        public ActionResult<Cliente> Get(string id)
        {
            var cliente = _clientesService.GetClienteById(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        [HttpPost]
        public ActionResult Create(Cliente cliente)
        {
            var clienteEmail = _clientesService.GetClienteByEmail(cliente.Email);

            if (clienteEmail == null)
            {
                _clientesService.CreateCliente(cliente);
                return NoContent();
            }
            else
                return NotFound();
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Cliente clIn)
        {
            var cliente = _clientesService.GetClienteById(id);

            if (cliente == null)
            {
                return NotFound();
            }

            clIn.Email = cliente.Email;
            _clientesService.UpdateCliente(id, clIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var cliente = _clientesService.GetClienteById(id);

            if (cliente == null)
            {
                return NotFound();
            }

            var ret = _clientesService.GetProdutosByClienteId(id);

            foreach (var item in ret)
            {
                _clientesService.RemoveProduto(item.IdProduto, id);
            }

            _clientesService.DeleteCliente(cliente);

            return NoContent();
        }

        [HttpPost]
        [Route("AddOrRemoveProduto")]
        public ActionResult AddOrRemoveProduto(ProdutoCliente pc)
        {
            if (pc.Tipo == "1")
            {             
                _clientesService.AddProduto(pc);
            }
            else
            {
                _clientesService.RemoveProduto(pc.IdProduto, pc.IdCliente);
            }

            return NoContent();
        }
    }
}
