using Xunit;
using Services;
using Services.Interface;
using Services.Concrete;

namespace Services.Tests
{
    /// <summary>
    /// O ideal é usar testmock para não acessar recursos de banco e para testar somente a estrutura dos métodos
    /// </summary>
    public class ProdutosService_Tests
    {
        private readonly IProdutosService _produtosService;

        public ProdutosService_Tests()
        {
            _produtosService = new ProdutosService();
        }

        /// <summary>
        /// Simples método para checar o nome de um produto existente
        /// </summary>
        [Fact]
        public async void CheckProduto()
        {
            var result = await _produtosService.GetProduto("4bd442b1-4a7d-2475-be97-a7b22a08a024");
            Assert.Equal(result.Titulo, "Cadeira para Auto Axiss Bébé Confort Robin Red");
        }        
    }
}