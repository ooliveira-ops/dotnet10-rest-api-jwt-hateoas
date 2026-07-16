# 🚀 REST API with ASP.NET Core 10

[![Continuous Integration, Delivery and Deployment with Github Actions and .NET 10](https://github.com/ooliveira-ops/Projeto-curso-Erudio-.NET/actions/workflows/continuous-deployment.yml/badge.svg)](https://github.com/ooliveira-ops/Projeto-curso-Erudio-.NET/actions/workflows/continuous-deployment.yml)
[![Docker Hub Repo](https://img.shields.io/docker/pulls/ooliveir4/rest-with-asp-net-10-erudio.svg)](https://hub.docker.com/repository/docker/ooliveir4/rest-with-asp-net-10-erudio)

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-Latest-239120?style=flat-square&logo=csharp)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?style=flat-square&logo=docker)
![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1?style=flat-square&logo=mysql)
![License](https://img.shields.io/badge/License-MIT-yellow?style=flat-square)

Projeto desenvolvido durante o curso **"ASP.NET Core 2026 REST API's from 0 to Azure and GCP with .NET 10, Docker e Kubernetes"** na Udemy.

Uma API RESTful completa com autenticação JWT, HATEOAS, versionamento, upload/download de arquivos, envio de e-mails, documentação com Swagger/Scalar e suporte a Docker.

Inclui também um **client em React** (pasta [`client/`](client)) que consome a API para login e CRUD de livros.

---

## 📋 Índice

- [Tecnologias](#-tecnologias)
- [Funcionalidades](#-funcionalidades)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Pré-requisitos](#-pré-requisitos)
- [Como Rodar](#-como-rodar)
- [Client React](#-client-react)
- [Variáveis de Ambiente](#-variáveis-de-ambiente)
- [Endpoints](#-endpoints)

---

## 🛠 Tecnologias

- **[.NET 10](https://dotnet.microsoft.com/)** — Framework principal
- **C#** — Linguagem de programação
- **MySQL** — Banco de dados relacional (local para desenvolvimento; compatível com Google Cloud SQL)
- **Docker & Docker Compose** — Containerização
- **JWT (JSON Web Token)** — Autenticação e autorização
- **Swagger / Scalar** — Documentação da API
- **Serilog** — Logging estruturado
- **Evolve** — Migrations de banco de dados
- **HATEOAS** — Hypermedia como motor de estado da aplicação
- **Pomelo.EntityFrameworkCore.MySql** — Provider EF Core para MySQL
- **GitHub Actions** — CI/CD pipeline
- **React** — Client web (pasta `client/`), bootstrapado com Create React App
- **React Router DOM** — Roteamento do client
- **Axios** — Consumo da API a partir do client

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
- ❌ Deploy em nuvem com **Google Cloud SQL** - REMOVIDO da Plataforma
- ✅ Pipeline CI/CD com **GitHub Actions** (build, testes e push de imagem)
- ✅ **Client React** com tela de login (JWT) e CRUD de livros (listagem paginada, criação, edição e remoção)

---


## 📁 Estrutura do Projeto

```
RestWithASPNET10Erudio/
├── client/                  # Client React (Create React App) que consome a API
│   ├── src/
│   │   ├── pages/
│   │   │   ├── Login/       # Tela de login (JWT)
│   │   │   ├── Books/       # Listagem paginada de livros
│   │   │   └── NewBook/     # Criação/edição de livro
│   │   ├── services/        # Configuração do Axios (baseURL da API)
│   │   └── routes.js        # Rotas do client (react-router-dom)
│   └── package.json
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

# Copie o .env.example para .env e preencha com seus dados reais
cp .env.example .env

# Suba os containers
docker compose up --build
```

A API estará disponível em: `http://localhost:8080`

> 💡 **MySQL instalado localmente no Windows (fora do Docker)?** Nesse caso, o container não consegue se conectar usando `127.0.0.1` ou `localhost` — esse endereço aponta pro próprio container, não pra máquina host. Use `DB_HOST=host.docker.internal` no `.env`, que é o endereço especial do Docker para acessar serviços rodando na máquina hospedeira.

### Localmente (sem Docker)

```bash
# Restaura as dependências
dotnet restore

# Configure a connection string no appsettings.json

# Roda a aplicação
dotnet run --project RestWithASPNET10Erudio
```

---

## 💻 Client React

O client em React (pasta [`client/`](client)) consome a API para autenticação e CRUD de livros. Para rodar:

```bash
cd client
npm install
npm start
```

O client sobe em `http://localhost:3000` e espera a API rodando em `http://localhost:8080` (configurado em `client/src/services/api.js`). Ajuste a `baseURL` nesse arquivo caso a API esteja em outro endereço.

**Fluxo do client:**
- **Login** (`/`) — autentica via `POST /api/auth/signin` e guarda `accessToken`/`refreshToken` no `localStorage`.
- **Livros** (`/books`) — lista os livros de forma paginada (`GET /api/book/v1/{sortDirection}/{pageSize}/{page}`), com opções de editar, remover e carregar mais.
- **Novo/Editar Livro** (`/book/new/:bookId`) — `bookId=0` cria um livro novo (`POST /api/book/v1`); qualquer outro valor edita um existente (`GET`/`PUT /api/book/v1`).
- **Logout** — revoga o token via `POST /api/auth/revoke`.

> ⚠️ `node_modules` e `build` do client não são versionados (ver `.gitignore`). Sempre rode `npm install` após clonar o repositório.

---

## 🔐 Variáveis de Ambiente

O projeto usa um arquivo `.env` (baseado no `.env.example`) para as variáveis do banco de dados, referenciadas no `docker-compose.yml`:

```env
DB_HOST=seu-host-aqui
DB_NAME=seu-banco-aqui
DB_USER=seu-usuario-aqui
DB_PASSWORD=sua-senha-aqui
JWT_SECRET=gere-uma-chave-aleatoria-aqui
```

> 🔑 `JWT_SECRET` é a chave usada para assinar os tokens de autenticação. Gere um valor aleatório próprio (ex.: `openssl rand -base64 32`) — nunca reaproveite exemplos ou valores de outros projetos.

> ⚠️ O `.env` nunca é commitado (está no `.gitignore`). Copie o `.env.example` e preencha com seus valores reais.

Para rodar localmente sem Docker, configure a connection string e demais chaves diretamente no `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "MySQLServerSqlConnectionStrings": "Server=<host>;Port=3306;Database=<database>;Uid=<user>;Pwd=<password>;"
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

Para o pipeline de CI/CD (GitHub Actions), configure os mesmos valores como **Secrets** do repositório, em `Settings → Secrets and variables → Actions`: `DB_HOST`, `DB_NAME`, `DB_USER`, `DB_PASSWORD`.

> ⚠️ Nunca commite credenciais reais. Use sempre variáveis de ambiente (`.env`) ou secrets do GitHub Actions.

---

## 🧪 Testes

O projeto conta com testes unitários e de integração, usando **xUnit**, **FluentAssertions** e **Testcontainers** (para os testes de integração, que sobem um banco MySQL real em container).

```bash
dotnet test
```

- **Testes unitários**: cobrem a lógica pura (ex.: `PersonQueryBuilder`, `BookQueryBuilder`, conversores), sem dependência de banco de dados.
- **Testes de integração**: validam o fluxo completo da API (Auth, CORS, HATEOAS, negociação de conteúdo JSON/XML) contra um banco real via Testcontainers.


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
| GET | `/api/book/v1/{sortDirection}/{pageSize}/{page}` | Lista paginada (filtro opcional por `title`) |
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
