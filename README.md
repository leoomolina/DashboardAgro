# 🌱 Agro Dashboard — Plataforma de Visualização de Dados Abertos do Agronegócio

## 📌 Descrição
Este projeto é um desafio técnico cujo objetivo é **desenvolver uma aplicação que consuma dados públicos do agronegócio, processe-os via
backend e os exiba em um dashboard interativo no frontend**.  
A solução consome dados públicos do **IBGE (Produção Agrícola Municipal - PAM)**, processa e normaliza os dados via **microsserviços backend**, e os exibe em um **dashboard interativo (frontend)** com foco em **visualização geográfica (mapa)** e **indicadores demográficos**.

---

## 🏗️ Arquitetura
A arquitetura segue o modelo de **microsserviços containerizados**:

- **DashboardAgro.Importer** → Responsável por consumir os dados públicos do IBGE, tratar e popular o banco de dados SQL Server.  
- **DashboardAgro.API** → Fornece uma **API REST** para o frontend, acessando os dados já tratados no banco.  
- **Front-end (Angular)** → Dashboard interativo com mapas, gráficos e filtros.  



---

## ⚙️ Tecnologias Utilizadas
### Backend
- **.NET 9 / C#** — API e microsserviço para importar dados
- **SQL Server** — Banco de dados para armazenar dados normalizados  
- **Docker** — Containerização e orquestração via `docker-compose`

### Frontend
- **Angular** — Framework para criação do dashboard  
- **** — Visualizações gráficas  
- **** — Visualização geográfica interativa  

### Dados
- [**BigQuery - Pesquisa Agrícola Municipal (PAM)**](https://sidra.ibge.gov.br/pesquisa/pam/tabelas) — Fonte oficial de dados da Produção Agrícola Municipal (PAM)  

---

## 🚀 Como Rodar o Projeto

### Pré-requisitos
- [Docker + Docker Compose](https://docs.docker.com/get-docker/)  
- [Git](https://git-scm.com/)  

### Passos
1. Clone este repositório:
   ```bash
   git clone https://github.com/leoomolina/DashboardAgro.git
   cd DashboardAgro
   ```
2. Suba os containers:
   ```bash
   docker compose up --build
   ```
3. Acesse os serviços:
    - API → ```http://localhost:5000/api/v1/...```
    - Frontend → ```http://localhost:4200```

    ---

## 📊 Fluxo de Dados

1. **DashboardAgro.Importer** → Faz ingestão histórica (1974–2022) uma única vez + sincronização do ano passado e ano corrente (2024+).
2. **SQL Server** → Armazena os dados tratados.
3. **DashboardAgro.API** → Expõe endpoints REST para o front.
4. **Frontend** → Consome dados da API e exibe em gráficos e mapas interativos.

    ---

## ✅ To-Do List — Projeto Agro Dashboard

### 🔹 Infraestrutura e Banco de Dados
- [x] Criar **banco de dados SQL Server**
- [x] Definir estrutura final das tabelas para receber dados da Produção Agrícola Estadual
- [ ] Criar índices/chaves para melhorar performance nas consultas

---

### 🔹 Microsserviço de Ingestão de Dados (DashboardAgro.Importer)
- [x] Criar microsserviço responsável por ingestão dos dados públicos do IBGE
- [ ] Implementar ingestão **histórica (1974–2022)** → rodar **uma vez** e importar ano a ano
- [ ] Implementar rotina de **sincronização do ano corrente (2023 em diante)** → verificar atualização do dataset e importar apenas dados novos
- [ ] Adicionar logs para acompanhar status da importação (ex.: “ano 1998 importado com sucesso”)
- [ ] Criar tabela de **controle de status de importação** (ano, status, data de importação)
- [ ] Expor endpoint interno ou sinalização para o front verificar quais anos já foram importados

---

### 🔹 REST API (DashboardAgro.API)
- [ ] Criar microsserviço **ms-api** que expõe dados do SQL Server via **REST API**
- [ ] Implementar endpoints para:
  - [ ] Listar dados agregados (por ano, estado, região)
  - [ ] Buscar detalhes de um ano específico
  - [ ] Consultar status da ingestão (anos importados x pendentes)
- [ ] Aplicar normalização dos dados antes de entregar ao front
- [ ] Configurar versionamento básico (ex.: `/api/v1/...`)

---

### 🔹 Front-end (Angular)
- [x] Criar projeto Angular base
- [ ] Montar layout inicial (header, sidebar, dashboards)
- [ ] Criar tela de **dashboard com gráficos**
- [ ] Integrar **mapa do Brasil** com dados agregados
- [ ] Criar consulta de **status de ingestão** mostrando anos já importados / em importação
- [ ] Integrar chamadas a API
- [ ] Adicionar loading/spinners para feedback durante consultas

---

### 🔹 DevOps & Deploy
- [x] Criar **Dockerfile** para cada microsserviço (`DashboardAgro.Importer`, `DashboardAgro.API`, `front-end`)
- [ ] Criar `docker-compose.yml` para orquestrar **SQL Server + DashboardAgro.Importer + DashboardAgro.API + front**
- [ ] Testar execução local (importação → api → front consumindo)
- [ ] Publicar imagens no **Docker Hub**

---

### 🔹 Documentação (README)
- [x] Descrever tecnologias utilizadas (SQL Server, .NET, Angular, IBGE API)
- [x] Listar o que já foi feito (BD, ms-ingestor base, API sem ms-api, front sem telas)
- [x] Adicionar **to-do list** (esse que estamos montando)
- [x] Explicar **como rodar o projeto localmente** (`docker compose up`)
- [ ] Explicar como rodar ingestão histórica x atualização de dados correntes
- [ ] Explicar arquitetura do código no backend (camadas, repositórios, controllers)
- [ ] Incluir prints/telas assim que estiverem prontas
