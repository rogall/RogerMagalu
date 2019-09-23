﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.MagaluApiProdutos;
using Entities.Mongo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Concrete;
using Services.Interface;

namespace ApiMagalu.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutosService _produtosService;       

        public ProdutosController(ProdutosService produtosService)
        {
            _produtosService = produtosService;         
        }

        [HttpGet]
        public async Task<List<Produto>> Get()
        {
            var ret = await _produtosService.GetProdutos(1);
            return ret;
        }        
    }
}