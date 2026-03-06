# Migrations

Este diretório contém a migration inicial da Fase 1.

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

Depois execute o mesmo comando de update:

```bash
dotnet ef database update -p src/FinanceApp.Infrastructure -s src/FinanceApp.Web
```
