using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Mongo
{
    public class ProdutoCliente
    {
        //O dispositivo que irá renderizar a resposta fornecida por essa nova API irá
        //apresentar o Título, Imagem, Preço e irá utilizar o ID do produto...
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("IdCliente")]
        public string IdCliente { get; set; }

        [BsonElement("IdProduto")]
        public string IdProduto { get; set; }

        [BsonElement("Titulo")]
        public string Titulo { get; set; }

        [BsonElement("Imagem")]
        public string Imagem { get; set; }

        [BsonElement("Preco")]
        public decimal Preco { get; set; }
    }
}
