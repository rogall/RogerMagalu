using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public ClientesController(ClientesService clientesService)
        {
            _clientesService = clientesService;
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

            _clientesService.DeleteCliente(cliente);

            return NoContent();
        }
    }
}
