# Migrations

Este diretório contém uma migration inicial placeholder para manter o scaffold da solução.

## Atenção

A migration `202609010001_InitialCreate` não cria tabelas de domínio; ela existe apenas como placeholder.

## Gerar migration real

```bash
dotnet ef migrations add InitialCreate -p src/FinanceApp.Infrastructure -s src/FinanceApp.Web -o Migrations
```

## Aplicar no banco

### PostgreSQL / SQL Server

```bash
dotnet ef database update -p src/FinanceApp.Infrastructure -s src/FinanceApp.Web
```

### SQLite

Configure no `appsettings.json`:

```json
"DatabaseProvider": "Sqlite",
"ConnectionStrings": {
  "DefaultConnection": "Data Source=financeapp.db"
}
```

Com o provider SQLite, o startup da aplicação chama `EnsureCreated` automaticamente para criar o schema local enquanto você ainda não gerou uma migration real.

Se você já tiver gerado migration real, pode aplicar normalmente com:

```bash
dotnet ef database update -p src/FinanceApp.Infrastructure -s src/FinanceApp.Web
```
