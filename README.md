# FinanceApp

Sistema web de controle financeiro pessoal/familiar com arquitetura em camadas (Clean Architecture), ASP.NET Core, EF Core, PostgreSQL/SQL Server e frontend MVC + jQuery.

## Fases entregues

### Fase 1
- Visão de arquitetura
- Estrutura de solução
- Entidades de domínio iniciais
- `AppDbContext`
- Migração inicial (placeholder orientado a gerar EF migration real no ambiente com SDK)
- Autenticação com ASP.NET Core Identity
- Layout base + telas iniciais

### Fase 2
- Upload manual de arquivo OFX
- Parser OFX com tolerância a variações simples
- Preview de importação (novos x duplicados)
- Prevenção de duplicidade por `FITID` e assinatura (`data + valor + descrição`)
- Idempotência por hash de arquivo (não reimporta lote já processado)
- Persistência de lote (`ImportBatch`) e metadados de arquivo (`ImportedFile`)
- Histórico de importações com status e métricas

### Fase 3
- CRUD de contas bancárias
- CRUD de cartões de crédito
- CRUD de categorias personalizadas (mantendo categorias padrão de sistema)
- CRUD de lançamentos com filtros por período e busca textual
- Seeds iniciais de categorias padrão

### Fase 4
- Dashboard analítico com KPIs, despesas por categoria, evolução mensal e fluxo diário
- Área de insights automáticos (variação mensal, gasto anômalo e recorrências)
- Relatórios com filtros por período e visões de gastos/receitas/top despesas

### Fase 5
- Orçamentos mensais por categoria com cálculo de realizado x planejado
- Metas financeiras com acompanhamento de progresso
- Painel de assinaturas detectadas com projeção anual
- Central de notificações e geração de alertas de orçamento excedido

### Fase 6
- Hardening de segurança: antiforgery global, rate limiting, security headers e página de erro amigável
- Reforço de validação no upload OFX (size limit, sanitização de nome e validação de conteúdo)
- Otimizações de performance em leituras com `AsNoTracking` e paginação server-side de lançamentos
- Ampliação de testes para paginação/filtro e marcação de notificações como lidas
- Documentação final consolidada para execução e evolução

## Arquitetura

```text
src/
  FinanceApp.Domain         -> Entidades, enums e regras de domínio
  FinanceApp.Application    -> Casos de uso/serviços e contratos
  FinanceApp.Infrastructure -> EF Core, Identity, seed, parser OFX, migrações
  FinanceApp.Web            -> Controllers, Views, assets e bootstrap da aplicação
tests/
  FinanceApp.Tests          -> Testes unitários/integrados (base)
```

## Decisões técnicas

- **Identity** para autenticação segura e gestão de credenciais.
- **EF Core** com provider configurável (`DatabaseProvider`: `PostgreSql` ou `SqlServer`).
- **Cookie seguro** com `HttpOnly`, `SecurePolicy=Always`, antiforgery nas ações de formulário.
- **Importação OFX** com preview e validações de tipo/tamanho.
- **Idempotência** via SHA-256 do arquivo e controle por lote de importação.

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

