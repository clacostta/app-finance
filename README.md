# FinanceApp

Sistema web de controle financeiro pessoal/familiar com arquitetura em camadas (Clean Architecture), ASP.NET Core, EF Core, PostgreSQL/SQL Server e frontend MVC + jQuery.

## Fase 1 entregue

- Visão de arquitetura
- Estrutura de solução
- Entidades de domínio iniciais
- `AppDbContext`
- Migração inicial (placeholder orientado a gerar EF migration real no ambiente com SDK)
- Autenticação com ASP.NET Core Identity
- Layout base + telas iniciais

## Arquitetura

```text
src/
  FinanceApp.Domain         -> Entidades, enums e regras de domínio
  FinanceApp.Application    -> Casos de uso/serviços e contratos
  FinanceApp.Infrastructure -> EF Core, Identity, seed, migrações
  FinanceApp.Web            -> Controllers, Views, assets e bootstrap da aplicação
tests/
  FinanceApp.Tests          -> Testes unitários/integrados (base)
```

## Decisões técnicas

- **Identity** para autenticação segura e gestão de credenciais.
- **EF Core** com provider configurável (`DatabaseProvider`: `PostgreSql` ou `SqlServer`).
- **Cookie seguro** com `HttpOnly`, `SecurePolicy=Always`, antiforgery nas ações de formulário.
- **Modelagem pronta para evolução** dos módulos OFX, categorização automática, relatórios e metas.

## Como executar localmente

### Pré-requisitos

- .NET SDK 8+
- PostgreSQL 14+ (ou SQL Server 2019+)

### Passos

1. Ajuste `src/FinanceApp.Web/appsettings.json`.
2. Gere migração real:

```bash
dotnet ef migrations add InitialCreate -p src/FinanceApp.Infrastructure -s src/FinanceApp.Web -o Migrations
```

3. Atualize banco:

```bash
dotnet ef database update -p src/FinanceApp.Infrastructure -s src/FinanceApp.Web
```

4. Execute:

```bash
dotnet run --project src/FinanceApp.Web
```

### Usuário seed

- Email: `admin@financeapp.local`
- Senha: `Admin@1234`

## Próximas fases

### Fase 2
- Upload OFX, parser robusto e fluxo de preview/revisão.
- Deduplicação por hash + assinatura da transação.
- Histórico de importações e logs.

### Fase 3
- CRUD completo de lançamentos, categorias, contas e cartões.
- Regras automáticas de categorização.

### Fase 4
- Dashboard com KPIs avançados e gráficos reais.
- Relatórios por período/categoria com exportação.
- Insights e detecção de anomalias.

### Fase 5
- Orçamentos, metas, assinaturas e alertas.

### Fase 6
- Testes avançados, hardening de segurança e otimizações.
- Documentação final de produção.
