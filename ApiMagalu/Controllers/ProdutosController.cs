using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DTOs;
using Entities.MagaluApiProdutos;
using Entities.Mongo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Concrete;
using Services.Interface;

namespace ApiMagalu.Controllers
{

    //Criei essa controler para utilizar na web app que construi para testar a api de teste Magalu
    //https://gist.github.com/Bgouveia/9e043a3eba439489a35e70d1b5ea08ec

    [Route("[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutosService _produtosService;
        private IMapper _mapper;
               
        public ProdutosController(ProdutosService produtosService, IMapper mapper)
        {
            _produtosService = produtosService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetProdutos")]
        public async Task<List<ProdutoDTO>> GetProdutos(int id, string idCliente)
        {
            var ret = await _produtosService.GetProdutos(id, idCliente);
            var retDTO = _mapper.Map<List<ProdutoDTO>>(ret);
            return retDTO;
        }        
    }
}