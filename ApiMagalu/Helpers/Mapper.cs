using AutoMapper;
using DTOs;
using Entities.MagaluApiProdutos;
using Entities.Mongo;
using EntitiesMongo;

namespace Helpers
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<Cliente, ClienteDTO>();
            CreateMap<ClienteDTO, Cliente>();

            CreateMap<ProdutoCliente, ProdutoClienteDTO>();
            CreateMap<ProdutoClienteDTO, ProdutoCliente>();

            CreateMap<Produto, ProdutoDTO>();
            CreateMap<ProdutoDTO, Produto>();
        }
    }
}