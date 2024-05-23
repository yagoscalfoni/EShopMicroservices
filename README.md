# EShopMicroservices

Desenvolvimento de vários microsserviços que implementam módulos de e-commerce para Catálogo, Carrinho de Compras, Desconto e Pedidos, utilizando bancos de dados NoSQL (DocumentDb, Redis) e Relacionais (PostgreSQL, Sql Server), comunicando-se através de RabbitMQ com comunicação orientada a eventos.

Catálogo

Implementação da Arquitetura Vertical Slice com pastas de recursos e um único arquivo .cs que inclui diferentes classes.
Implementação de CQRS usando a biblioteca MediatR.
Validação de Pipeline de CQRS com MediatR e FluentValidation.
Uso da biblioteca Marten para Document DB Transacional no PostgreSQL.
Definição de endpoints de API Minimal com Carter.
Preocupações transversais como Logging, Tratamento Global de Exceções e Verificações de Saúde.

Carrinho de Compras

Aplicativo Web API seguindo os princípios REST API para operações CRUD.
Uso do Redis como Cache Distribuído para o banco de dados da cesta (basketdb).
Implementação dos padrões Proxy, Decorator e Cache-aside.
Consumo do serviço Grpc de Desconto para comunicação síncrona inter-serviços e cálculo do preço final do produto.
Publicação da fila BasketCheckout usando MassTransit e RabbitMQ.

Desconto

Aplicativo Servidor Grpc.
Construção de comunicação Grpc inter-serviços de alto desempenho com o microsserviço de Cesta.
Exposição de serviços Grpc com criação de mensagens Protobuf.
Uso do Entity Framework Core ORM com provedor de dados SQLite e migrações para simplificar o acesso aos dados e garantir alto desempenho.
Conexão com banco de dados SQLite e conteinerização.
Comunicação entre Microsserviços
Comunicação síncrona inter-serviços com Grpc.
Comunicação assíncrona entre microsserviços com RabbitMQ como broker de mensagens.
Uso do modelo de troca de tópicos Publish/Subscribe do RabbitMQ.
Uso de MassTransit para abstração sobre o sistema de broker de mensagens RabbitMQ.
Publicação do evento BasketCheckout na fila a partir do microsserviço de Cesta e assinatura deste evento no microsserviço de Pedidos.
Criação da biblioteca RabbitMQ EventBus.Messages e adição de referências nos microsserviços.

Pedidos

Implementação de DDD, CQRS e Arquitetura Limpa seguindo as melhores práticas.
Desenvolvimento de CQRS usando os pacotes MediatR, FluentValidation e Mapster.
Consumo do evento BasketCheckout na fila RabbitMQ usando a configuração MassTransit-RabbitMQ.
Conexão e conteinerização do banco de dados SqlServer.
Uso do Entity Framework Core ORM com migração automática para SqlServer na inicialização da aplicação.

Docker Compose
Estabelecimento de todos os microsserviços no Docker:
Conteinerização de microsserviços.
Conteinerização de bancos de dados.
Sobrescrita de variáveis de ambiente.
