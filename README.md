# ZapZap

O ZapZap é o resultado de uma POC para validar a utilização de protocolos de rede. 

O projeto consiste em uma sala de bate papo, permitindo a conexão de vários clientes para a troca de mensagens. Ao se conectar, o usuário escolhe um apelido único para depois entrar em uma sala, onde poderá conversar com outros usuários que estejam conectados.

Dentro da sala os usuários podem enviar os seguintes tipos de mensagens:
    * Uma mensagem pública para todos na sala
    * Uma mensagem pública para um usuário da sala
    * Uma mensagem privada para um usuário da sala
    * Criar uma nova sala de bate papo
    * Listar todos as salas existentes
    * Trocar de sala
    * Pedir ajuda
    * Sair do bate papo

## 🚀 Começando

Essas instruções permitirão que você obtenha uma cópia do projeto em operação na sua máquina local para fins de desenvolvimento e teste.

Consulte **Implantação** para saber como implantar o projeto.

### 📋 Pré-requisitos

```
 - Visual Studio 2019
 - Microsoft .NET SDK 5.0.101
```

### 🔧 Instalação

```
 - Faça download do repositório
 - Abra o projeto no Visual Studio
 - Compile a solução
 - Execute o projeto "zapzap"
```

## 📦 Arquitetura

O projeto foi desenvolvido com ASP.NET Core Web Application, implementado utilizando os padrões S.O.L.I.D.
Foi criada uma classe de middleware, onde centralizo todas as conexões do socket. A partir do middleware, chamo a classe de chat, onde processo as mensagens enviadas.

Os repositórios foram implementados em memória, não sendo a melhor solução (apenas para a POC).
Para tornar o projeto escalável, podemos utilizar o Redis como cache distribuído.

## ✒️ Referências

* <a href="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/websockets?view=aspnetcore-3.0#sample-app/" rel="nofollow">WebSockets support in ASP.NET Core</a>
* <a href="https://gunnarpeipman.com/aspnet-core-websocket-chat/" rel="nofollow">ASP.NET Core: Building chat room using WebSocket</a>
* <a href="https://radu-matei.com/blog/aspnet-core-websockets-middleware/" rel="nofollow">Creating a WebSockets middleware for ASP .NET Core 3</a>

