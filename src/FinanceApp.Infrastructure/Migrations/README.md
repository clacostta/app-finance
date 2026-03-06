# Migrations

Este diretório contém a migration inicial da Fase 1.

## Gerar migration real

```bash
dotnet ef migrations add InitialCreate -p src/FinanceApp.Infrastructure -s src/FinanceApp.Web -o Migrations
```

## Aplicar no banco

```bash
dotnet ef database update -p src/FinanceApp.Infrastructure -s src/FinanceApp.Web
```
