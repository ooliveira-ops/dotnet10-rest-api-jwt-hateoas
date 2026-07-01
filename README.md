# 🚀 REST API with ASP.NET Core 10

[![Continuous Integration, Delivery and Deployment with Github Actions and .NET 10](https://github.com/ooliveira-ops/Projeto-curso-Erudio-.NET/actions/workflows/continuous-deployment.yml/badge.svg)](https://github.com/ooliveira-ops/Projeto-curso-Erudio-.NET/actions/workflows/continuous-deployment.yml)
[![Docker Hub Repo](https://img.shields.io/docker/pulls/ooliveir4/rest-with-asp-net-10-erudio.svg)](https://hub.docker.com/repository/docker/ooliveir4/rest-with-asp-net-10-erudio)

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-Latest-239120?style=flat-square&logo=csharp)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?style=flat-square&logo=docker)
![SQL Server](https://img.shields.io/badge/SQL_Server-Latest-CC2927?style=flat-square&logo=microsoftsqlserver)
![License](https://img.shields.io/badge/License-MIT-yellow?style=flat-square)

Projeto desenvolvido durante o curso **"ASP.NET Core 2026 REST API's from 0 to Azure and GCP with .NET 10, Docker e Kubernetes"** na Udemy.

Uma API RESTful completa com autenticação JWT, HATEOAS, versionamento, upload/download de arquivos, envio de e-mails, documentação com Swagger/Scalar e suporte a Docker.

---

## 📋 Índice

- [Tecnologias](#-tecnologias)
- [Funcionalidades](#-funcionalidades)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Pré-requisitos](#-pré-requisitos)
- [Como Rodar](#-como-rodar)
- [Variáveis de Ambiente](#-variáveis-de-ambiente)
- [Endpoints](#-endpoints)

---

## 🛠 Tecnologias

- **[.NET 10](https://dotnet.microsoft.com/)** — Framework principal
- **C#** — Linguagem de programação
- **MySQL (Google Cloud SQL)** — Banco de dados relacional em nuvem
- **Docker & Docker Compose** — Containerização
- **JWT (JSON Web Token)** — Autenticação e autorização
- **Swagger / Scalar** — Documentação da API
- **Serilog** — Logging estruturado
- **Evolve** — Migrations de banco de dados
- **HATEOAS** — Hypermedia como motor de estado da aplicação
- **Pomelo.EntityFrameworkCore.MySql** — Provider EF Core para MySQL
- **GitHub Actions** — CI/CD pipeline

---

## ✨ Funcionalidades

- ✅ CRUD completo de **Pessoas** (v1 e v2) e **Livros**
- ✅ Autenticação e autorização com **JWT**
- ✅ **HATEOAS** — respostas com links de navegação
- ✅ **Versionamento de API** (V1 e V2)
- ✅ Upload e download de arquivos (**CSV** e **XLSX**)
- ✅ Envio de **e-mails**
- ✅ Negociação de conteúdo (**JSON** e **XML**)
- ✅ Documentação interativa com **Swagger** e **Scalar**
- ✅ **CORS** configurável
- ✅ Migrations automáticas com **Evolve**
- ✅ Containerização completa com **Docker Compose**
- ✅ Deploy em nuvem com **Google Cloud SQL**
- ✅ Pipeline CI/CD com **GitHub Actions** (build, testes e push de imagem)

---


## 📁 Estrutura do Projeto

```
RestWithASPNET10Erudio/
├── Auth/                    # Autenticação JWT (contratos e implementações)
├── Configurations/          # Configurações modulares (DB, Auth, CORS, Swagger...)
├── Controllers/
│   ├── V1/                  # Controllers versão 1
│   └── V2/                  # Controllers versão 2
├── DATA/                    # Contexto e entidades do banco de dados
├── db/
│   ├── migrations/          # Scripts DDL (criação de tabelas)
│   └── dataset/             # Scripts DML (dados iniciais)
├── Files/
│   ├── Exporters/           # Exportação CSV e XLSX
│   └── Importers/           # Importação CSV e XLSX
├── Hypermedia/              # Implementação HATEOAS
├── JsonSerializers/         # Serializadores customizados
├── Mail/                    # Serviço de envio de e-mail
├── Model/                   # Models e DTOs
├── Repositories/            # Repositórios (Generic + específicos)
├── Services/                # Serviços de negócio
├── UploadDir/               # Diretório de uploads
├── Dockerfile               # Imagem da aplicação
├── docker-compose.yml       # Orquestração dos containers
├── Program.cs               # Ponto de entrada da aplicação
└── appsettings.json         # Configurações da aplicação
```


---

## ✅ Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Git](https://git-scm.com/)
- Instância MySQL acessível (local ou Google Cloud SQL)

---

## 🚀 Como Rodar

### Com Docker (recomendado)

```bash
# Clone o repositório
git clone https://github.com/ooliveira-ops/Projeto-curso-Erudio-.NET.git
cd Projeto-curso-Erudio-.NET

# Configure a connection string no docker-compose.yml
# ConnectionStrings__MySQLServerSqlConnectionStrings=Server=<host>;Port=3306;Database=asp_net_10_erudio;Uid=<user>;Pwd=<password>;

# Suba os containers
docker compose up --build
```

A API estará disponível em: `http://localhost:8080`

### Localmente (sem Docker)

```bash
# Restaura as dependências
dotnet restore

# Configure a connection string no appsettings.json

# Roda a aplicação
dotnet run --project RestWithASPNET10Erudio
```

---

## 🔐 Variáveis de Ambiente

Configure a connection string no `appsettings.json` ou via variável de ambiente:

```json
{
  "ConnectionStrings": {
    "MySQLServerSqlConnectionStrings": "Server=<host>;Port=3306;Database=asp_net_10_erudio;Uid=<user>;Pwd=<password>;"
  },
  "TokenConfigurations": {
    "Audience": "ExampleAudience",
    "Issuer": "ExampleIssuer",
    "Secret": "sua-chave-secreta-aqui",
    "Minutes": 60,
    "DaysToExpiry": 7
  },
  "Email": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "Username": "seu-email@gmail.com",
    "Password": "sua-senha-de-app"
  }
}
```

> ⚠️ Nunca commite credenciais reais. Use variáveis de ambiente ou secrets do GitHub Actions.

---

## 📡 Endpoints

### Autenticação
| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/auth/signin` | Login — retorna token JWT |
| POST | `/api/auth/refresh` | Renova o token |
| POST | `/api/auth/revoke` | Revoga o token |

### Pessoas (V1)
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/person/v1/{sortDirection}/{pageSize}/{page}` | Lista paginada |
| GET | `/api/person/v1/{id}` | Busca por ID |
| POST | `/api/person/v1` | Cria uma pessoa |
| PUT | `/api/person/v1` | Atualiza uma pessoa |
| PATCH | `/api/person/v1/{id}` | Habilita/desabilita |
| DELETE | `/api/person/v1/{id}` | Remove uma pessoa |

### Livros
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/book/v1` | Lista todos os livros |
| GET | `/api/book/v1/{id}` | Busca por ID |
| POST | `/api/book/v1` | Cria um livro |
| PUT | `/api/book/v1` | Atualiza um livro |
| DELETE | `/api/book/v1/{id}` | Remove um livro |

### Arquivos
| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/file/v1/uploadFile` | Upload de CSV ou XLSX |
| POST | `/api/file/v1/uploadFiles` | Upload múltiplo |
| GET | `/api/file/v1/downloadFile/{fileName}` | Download de arquivo |

> 📖 Documentação completa disponível em `/swagger` ou `/scalar` após subir a aplicação.

---

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.


