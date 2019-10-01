using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DTOs;
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
    [Authorize("Bearer")]
    public class ClientesController : ControllerBase
    {
        /// <summary>
        /// Controller com operacões para CRUD de clientes conforme o documento
        /// E uma operacao para adicionar um produto ou remove-lo da lista de favoritos
        /// </summary>
        private readonly IClientesService _clientesService;
        private readonly IProdutosService _produtosService;
        private IMapper _mapper;

        public ClientesController(ClientesService clientesService, ProdutosService produtosService, IMapper mapper)
        {
            _clientesService = clientesService;
            _produtosService = produtosService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<Cliente>> Get()
        {
            try
            {
                var ret = _clientesService.GetAllClientes();
                return ret;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id:length(24)}", Name = "GetCliente")]
        public ActionResult<Cliente> Get(string id)
        {
            try
            {
                var cliente = _clientesService.GetClienteById(id);

                if (cliente == null)
                {
                    return NotFound();
                }

                return cliente;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Create(Cliente cliente)
        {
            try
            {
                var clienteEmail = _clientesService.GetClienteByEmail(cliente.Email);

                if (clienteEmail == null)
                {
                    cliente.Id = string.Empty;
                    _clientesService.CreateCliente(cliente);
                    return NoContent();
                }
                else
                    return Ok("Email já utilizado");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Cliente clIn)
        {
            try
            {
                var pCliente = _clientesService.GetClienteById(id);
                var anyCliente = _clientesService.GetClienteByEmail(clIn.Email);

                if (anyCliente != null)
                {
                    if (clIn.Email == anyCliente.Email && pCliente.Id != anyCliente.Id)
                    {
                        return Ok("Email já utilizado");
                    }
                }

                clIn.Email = pCliente.Email;
                _clientesService.UpdateCliente(id, clIn);

                return Ok("Atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            try
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
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("AddOrRemoveProduto")]
        public ActionResult AddOrRemoveProduto(ProdutoClienteDTO pcDTO)
        {
            var pc = _mapper.Map<ProdutoCliente>(pcDTO);
            try
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
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        ///Este método eu utilizei Task para ressaltar a performance comparando com os outros métodos        
        [HttpGet]
        [Route("GetListaProdutosFavoritos")]      
        public async Task<ActionResult<List<ProdutoCliente>>> GetListaProdutosFavoritos(string idCliente)
        {
            try
            {
                var ret = await _clientesService.GetProdutosFavoritosCliente(idCliente);
                return ret;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
