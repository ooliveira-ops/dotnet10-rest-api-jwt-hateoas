# 🚀 REST API with ASP.NET Core 10

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
- **SQL Server** — Banco de dados relacional
- **Docker & Docker Compose** — Containerização
- **JWT (JSON Web Token)** — Autenticação e autorização
- **Swagger / Scalar** — Documentação da API
- **Serilog** — Logging estruturado
- **Evolve** — Migrations de banco de dados
- **HATEOAS** — Hypermedia como motor de estado da aplicação

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
├── db/                      # Scripts SQL e migrations
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
├── Dockerfile.sqlserver     # Imagem customizada do SQL Server
├── docker-compose.yml       # Orquestração dos containers
├── docker-compose.dev.yml   # Configuração de desenvolvimento
├── Program.cs               # Ponto de entrada da aplicação
└── appsettings.json         # Configurações (usar .example para referência)
```

---

## ✅ Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Git](https://git-scm.com/)

---

## 🚀 Como Rodar

### Com Docker (recomendado)

```bash
# Clone o repositório
git clone https://github.com/ooliveira-ops/Projeto-curso-Erudio-.NET.git
cd Projeto-curso-Erudio-.NET

# Copie o arquivo de exemplo e configure as variáveis
cp appsettings.example.json appsettings.Development.json

# Suba os containers
docker compose up --build
```

A API estará disponível em: `http://localhost:8080`

### Localmente (sem Docker)

```bash
# Restaura as dependências
dotnet restore

# Roda a aplicação
dotnet run --project RestWithASPNET10Erudio
```

---

## 🔐 Variáveis de Ambiente

Crie um arquivo `appsettings.Development.json` baseado no `appsettings.example.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=rest_api;User=sa;Password=SuaSenha;"
  },
  "TokenConfigurations": {
    "Audience": "ExampleAudience",
    "Issuer": "ExampleIssuer",
    "Secret": "sua-chave-secreta-aqui",
    "Minutes": 60,
    "DaysToExpiry": 7
  },
  "EmailConfiguration": {
    "From": "seu-email@gmail.com",
    "SmtpServer": "smtp.gmail.com",
    "Port": 587,
    "Password": "sua-senha-de-app"
  }
}
```

> ⚠️ **Nunca commite** o arquivo `appsettings.Development.json` com dados reais. Ele já está no `.gitignore`.

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
| GET | `/api/v1/person` | Lista todas as pessoas |
| GET | `/api/v1/person/{id}` | Busca por ID |
| POST | `/api/v1/person` | Cria uma pessoa |
| PUT | `/api/v1/person` | Atualiza uma pessoa |
| DELETE | `/api/v1/person/{id}` | Remove uma pessoa |

### Livros
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/v1/book` | Lista todos os livros |
| GET | `/api/v1/book/{id}` | Busca por ID |
| POST | `/api/v1/book` | Cria um livro |
| PUT | `/api/v1/book` | Atualiza um livro |
| DELETE | `/api/v1/book/{id}` | Remove um livro |

### Arquivos
| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/v1/file/upload` | Upload de CSV ou XLSX |
| GET | `/api/v1/file/download/{name}` | Download de arquivo |

> 📖 Documentação completa disponível em `/swagger` ou `/scalar` após subir a aplicação.

---

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.
