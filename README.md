# Roger Teixeira - Magalu Desafio

## Api de teste de cadastro de clientes

Linguagens e tecnologias utilizadas:

* Asp.Net Core C# 
* Visual Studio 2019 
* Visual Studio Code 
* Swagger (para documentar a api) 
* Angular 6 / TypeScript
* Docker(com imagem MySQL para cadastro de admin e imagem MongoDB para cadastro de clientes e lista de favoritos) 




## Para testar o desenvolvimento siga os seguintes passos:

1. Fazer o download dos artefatos para máquina local.

2. Com o Visual Studio fazer o build do projeto para que o Nuget baixe as dependências.

3. Colocar o projeto ApiMagalu como principal (Set as StartUp Project)

4. Na pasta raiz existe o arquivo docker-compose.yml (É necessário ter o docker for windows instalado).

    #### https://hub.docker.com/?overlay=onboarding

5. Executar docker compose up em cmd nessa localização para instalar a imagem do MySQL:

6. No visual studio TOOLS => NUGET PACKAGE MANAGER => PACKAGE MANAGER CONSOLE e execute o seguinte comando para criar as tabelas no MySQL:

    #### EntityFrameworkCore\Update-Database -Verbose

7. Executar o seguinte comando para criar um banco MongoDB para que a aplicacão faça a conexão para o cadastro de clientes:

    #### docker run -d -p 27017-27019:27017-27019 --name rogermagalu mongo:4.0.4

8. Executar o start no visual studio e acessar o seguinte endereco:

    #### http://localhost:4000/swagger/index.html

9. A primeira transacão a ser executada deve ser a /ADMIN/REGISTER para que se crie um usuário admin para os propósitos de teste

10. Em seguida deve executar a transação /ADMIN/LOGIN para que o usuário receba um BEARER TOKEN. 

11. Copie o token e clique no cadeado no topo da página para inserir o token da seguinte forma: Bearer {token}

12. Depois disso é só testar as outras transações.



## Observacão:

Não foi pedido, mas eu criei uma simples aplicação em Angular 6 para testar melhor as regras de negócio e fazer o consumo da api para simular um cliente.

Ela se encontra em:

https://github.com/rogall/RogerMagalu/tree/master/WebMagalu

Para utiliza-la, recomendo o Visual Studio Code como editor/IDE

Abra a pasta com o VS code, abra um terminal e execute o comando -npm install- para baixar as dependências e em seguida execute o comando -npm start-

Para que essa aplicacão acesse a api, tive que habilitar o CORS, mas em caso de colocar em um ambiente de producão, essa feature deve ser removida.

## Considerações finais:

O cadastro de clientes é feito por um usuário admin para que fosse implementado a autenticação, autorização e tokenização.

Em um outo cenário  eu abriria esse cadastro para o próprio cliente, e o persistiria na mesma tabela do admin e os diferenciaria através de perfil de acesso, ou aguardaria para conhecer as estruturas de perfilamento da Magalu.

Utilizando o banco que eu implementei, isso seria feito com a tabela aspNetRoles.



## Muito Obrigado!




