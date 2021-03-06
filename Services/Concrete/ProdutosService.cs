﻿using Entities.MagaluApiProdutos;
using Entities.Mongo;
using MongoDB.Driver;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Services.Concrete
{
    public class ProdutosService : IProdutosService
    {
        private readonly IClientesService _clientesService;

        public ProdutosService(ClientesService service)
        {
            _clientesService = service;
        }

        public ProdutosService()
        {

        }

        /// <summary>
        /// Um simples request para recuperar produtos e ao mesmo tempo marcar o que o cliente tem como favoritos
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        public async Task<List<Produto>> GetProdutos(int pagination, string idCliente)
        {
            //primeiramente recupero os produtos da lista de favoritos
            var produtosFavoritosCliente = _clientesService.GetProdutosByClienteId(idCliente);
            List<string> ids = produtosFavoritosCliente.Select(_ => _.IdProduto).ToList();

            String ret = string.Empty;
            List<Produto> result = new List<Produto>();

            try
            {
                ListProducts list = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://challenge-api.luizalabs.com/api/product/?page=" + pagination.ToString());
                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    ret = await reader.ReadToEndAsync();
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ListProducts));
                    MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(ret));
                    list = (ListProducts)serializer.ReadObject(ms);
                }

                if (list != null && list.products != null && list.products.Count > 0)
                {
                    foreach (var item in list.products)
                    {
                        Produto p = new Produto();
                        p.IdProduto = item.id;
                        p.Imagem = item.image;
                        p.Preco = item.price;
                        p.Titulo = item.title;

                        //se o produto da lista da magalu estiver na lista de favoritos do cliente, setar favorito = true
                        p.IsFavorito = ids.Contains(p.IdProduto);
                        result.Add(p);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }            

            return result;
        }
        
        /// <summary>
        /// Request para recuperar um produto pelo seu id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Produto> GetProduto(string id)
        {            
            String ret = string.Empty;
            Product obj = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://challenge-api.luizalabs.com/api/product/" + id);
            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                ret = await reader.ReadToEndAsync();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Product));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(ret));
                obj = (Product)serializer.ReadObject(ms);                
            }

            Produto p = new Produto();
            p.IdProduto = obj.id;
            p.Imagem = obj.image;
            p.Preco = obj.price;
            p.Titulo = obj.title;
            return p;
        }
    }
}

/// <summary>
/// Classes para parsear o JSON que vem da api https://gist.github.com/Bgouveia/9e043a3eba439489a35e70d1b5ea08ec
/// </summary>
public class Meta
{
    public int page_number { get; set; }
    public int page_size { get; set; }
}

public class Product
{
    public double price { get; set; }
    public string image { get; set; }
    public string brand { get; set; }
    public string id { get; set; }
    public string title { get; set; }
    public double? reviewScore { get; set; }
}

public class ListProducts
{
    public Meta meta { get; set; }
    public List<Product> products { get; set; }
}