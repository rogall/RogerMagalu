using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.MagaluApiProdutos;
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
    [Authorize]
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
            //var prod = _produtosService.GetProduto("958ec015-cfcf-258d-c6df-1721de0ab6ea");
            var prods = _produtosService.GetProdutos(1);
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

            _clientesService.DeleteCliente(cliente);

            return NoContent();
        }

        [HttpGet(Name = "GetProdutos")]
        public async Task<List<Produto>> GetProdutos(int pagination)
        {
            var ret = await _produtosService.GetProdutos(pagination);
            return ret;
        }

        [HttpGet(Name = "GetProduto")]
        public async Task<Produto> GetProduto(string id)
        {
            var ret = await _produtosService.GetProduto(id);
            return ret;
        }
    }
}
