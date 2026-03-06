PRAGMA foreign_keys = ON;

-- =============================================
-- ASP.NET Core Identity tables
-- =============================================
CREATE TABLE IF NOT EXISTS AspNetRoles (
    Id TEXT NOT NULL PRIMARY KEY,
    Name TEXT NULL,
    NormalizedName TEXT NULL,
    ConcurrencyStamp TEXT NULL
);

CREATE UNIQUE INDEX IF NOT EXISTS IX_AspNetRoles_NormalizedName
    ON AspNetRoles (NormalizedName);

CREATE TABLE IF NOT EXISTS AspNetUsers (
    Id TEXT NOT NULL PRIMARY KEY,
    FullName TEXT NOT NULL,
    UserName TEXT NULL,
    NormalizedUserName TEXT NULL,
    Email TEXT NULL,
    NormalizedEmail TEXT NULL,
    EmailConfirmed INTEGER NOT NULL,
    PasswordHash TEXT NULL,
    SecurityStamp TEXT NULL,
    ConcurrencyStamp TEXT NULL,
    PhoneNumber TEXT NULL,
    PhoneNumberConfirmed INTEGER NOT NULL,
    TwoFactorEnabled INTEGER NOT NULL,
    LockoutEnd TEXT NULL,
    LockoutEnabled INTEGER NOT NULL,
    AccessFailedCount INTEGER NOT NULL
);

CREATE INDEX IF NOT EXISTS IX_AspNetUsers_NormalizedEmail
    ON AspNetUsers (NormalizedEmail);

CREATE UNIQUE INDEX IF NOT EXISTS IX_AspNetUsers_NormalizedUserName
    ON AspNetUsers (NormalizedUserName);

CREATE TABLE IF NOT EXISTS AspNetRoleClaims (
    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    RoleId TEXT NOT NULL,
    ClaimType TEXT NULL,
    ClaimValue TEXT NULL,
    FOREIGN KEY (RoleId) REFERENCES AspNetRoles (Id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS IX_AspNetRoleClaims_RoleId
    ON AspNetRoleClaims (RoleId);

CREATE TABLE IF NOT EXISTS AspNetUserClaims (
    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    UserId TEXT NOT NULL,
    ClaimType TEXT NULL,
    ClaimValue TEXT NULL,
    FOREIGN KEY (UserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS IX_AspNetUserClaims_UserId
    ON AspNetUserClaims (UserId);

CREATE TABLE IF NOT EXISTS AspNetUserLogins (
    LoginProvider TEXT NOT NULL,
    ProviderKey TEXT NOT NULL,
    ProviderDisplayName TEXT NULL,
    UserId TEXT NOT NULL,
    PRIMARY KEY (LoginProvider, ProviderKey),
    FOREIGN KEY (UserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS IX_AspNetUserLogins_UserId
    ON AspNetUserLogins (UserId);

CREATE TABLE IF NOT EXISTS AspNetUserRoles (
    UserId TEXT NOT NULL,
    RoleId TEXT NOT NULL,
    PRIMARY KEY (UserId, RoleId),
    FOREIGN KEY (UserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE,
    FOREIGN KEY (RoleId) REFERENCES AspNetRoles (Id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS IX_AspNetUserRoles_RoleId
    ON AspNetUserRoles (RoleId);

CREATE TABLE IF NOT EXISTS AspNetUserTokens (
    UserId TEXT NOT NULL,
    LoginProvider TEXT NOT NULL,
    Name TEXT NOT NULL,
    Value TEXT NULL,
    PRIMARY KEY (UserId, LoginProvider, Name),
    FOREIGN KEY (UserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE
);

-- =============================================
-- FinanceApp domain tables
-- =============================================
CREATE TABLE IF NOT EXISTS Users (
    Id TEXT NOT NULL PRIMARY KEY,
    IdentityUserId TEXT NOT NULL,
    FullName TEXT NOT NULL,
    TimeZone TEXT NOT NULL,
    CurrencyCode TEXT NOT NULL,
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT NULL,
    FOREIGN KEY (IdentityUserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE
);

CREATE UNIQUE INDEX IF NOT EXISTS IX_Users_IdentityUserId
    ON Users (IdentityUserId);

CREATE TABLE IF NOT EXISTS UserPreferences (
    Id TEXT NOT NULL PRIMARY KEY,
    UserId TEXT NOT NULL,
    EnableEmailNotifications INTEGER NOT NULL,
    EnablePushNotifications INTEGER NOT NULL,
    Theme TEXT NOT NULL,
    Language TEXT NOT NULL,
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT NULL,
    FOREIGN KEY (UserId) REFERENCES Users (Id) ON DELETE CASCADE
);

CREATE UNIQUE INDEX IF NOT EXISTS IX_UserPreferences_UserId
    ON UserPreferences (UserId);

CREATE TABLE IF NOT EXISTS FinancialInstitutions (
    Id TEXT NOT NULL PRIMARY KEY,
    Name TEXT NOT NULL,
    Code TEXT NULL,
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT NULL
);

CREATE TABLE IF NOT EXISTS Accounts (
    Id TEXT NOT NULL PRIMARY KEY,
    UserId TEXT NOT NULL,
    FinancialInstitutionId TEXT NOT NULL,
    Name TEXT NOT NULL,
    InitialBalance NUMERIC NOT NULL,
    IsActive INTEGER NOT NULL,
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT NULL,
    FOREIGN KEY (UserId) REFERENCES Users (Id) ON DELETE CASCADE,
    FOREIGN KEY (FinancialInstitutionId) REFERENCES FinancialInstitutions (Id) ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS IX_Accounts_UserId
    ON Accounts (UserId);

CREATE INDEX IF NOT EXISTS IX_Accounts_FinancialInstitutionId
    ON Accounts (FinancialInstitutionId);

CREATE TABLE IF NOT EXISTS CreditCards (
    Id TEXT NOT NULL PRIMARY KEY,
    UserId TEXT NOT NULL,
    FinancialInstitutionId TEXT NOT NULL,
    Name TEXT NOT NULL,
    LimitAmount NUMERIC NOT NULL,
    ClosingDay INTEGER NOT NULL,
    DueDay INTEGER NOT NULL,
    IsActive INTEGER NOT NULL,
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT NULL,
    FOREIGN KEY (UserId) REFERENCES Users (Id) ON DELETE CASCADE,
    FOREIGN KEY (FinancialInstitutionId) REFERENCES FinancialInstitutions (Id) ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS IX_CreditCards_UserId
    ON CreditCards (UserId);

CREATE INDEX IF NOT EXISTS IX_CreditCards_FinancialInstitutionId
    ON CreditCards (FinancialInstitutionId);

CREATE TABLE IF NOT EXISTS TransactionCategories (
    Id TEXT NOT NULL PRIMARY KEY,
    UserId TEXT NOT NULL,
    Name TEXT NOT NULL,
    IsSystemDefault INTEGER NOT NULL,
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT NULL,
    FOREIGN KEY (UserId) REFERENCES Users (Id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS IX_TransactionCategories_UserId
    ON TransactionCategories (UserId);

CREATE TABLE IF NOT EXISTS TransactionSubcategories (
    Id TEXT NOT NULL PRIMARY KEY,
    CategoryId TEXT NOT NULL,
    Name TEXT NOT NULL,
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT NULL,
    FOREIGN KEY (CategoryId) REFERENCES TransactionCategories (Id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS IX_TransactionSubcategories_CategoryId
    ON TransactionSubcategories (CategoryId);

CREATE TABLE IF NOT EXISTS ImportBatches (
    Id TEXT NOT NULL PRIMARY KEY,
    UserId TEXT NOT NULL,
    FileName TEXT NOT NULL,
    ImportedAt TEXT NOT NULL,
    Status INTEGER NOT NULL,
    TotalRecords INTEGER NOT NULL,
    ImportedRecords INTEGER NOT NULL,
    DuplicatedRecords INTEGER NOT NULL,
    FailedRecords INTEGER NOT NULL,
    FileHash TEXT NOT NULL,
    ErrorMessage TEXT NULL,
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT NULL,
    FOREIGN KEY (UserId) REFERENCES Users (Id) ON DELETE CASCADE
);

CREATE UNIQUE INDEX IF NOT EXISTS IX_ImportBatches_UserId_FileHash
    ON ImportBatches (UserId, FileHash);

CREATE TABLE IF NOT EXISTS ImportedFiles (
    Id TEXT NOT NULL PRIMARY KEY,
    ImportBatchId TEXT NOT NULL,
    StoragePath TEXT NOT NULL,
    OriginalName TEXT NOT NULL,
    SizeBytes INTEGER NOT NULL,
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT NULL,
    FOREIGN KEY (ImportBatchId) REFERENCES ImportBatches (Id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS IX_ImportedFiles_ImportBatchId
    ON ImportedFiles (ImportBatchId);

CREATE TABLE IF NOT EXISTS Transactions (
    Id TEXT NOT NULL PRIMARY KEY,
    UserId TEXT NOT NULL,
    AccountId TEXT NULL,
    CreditCardId TEXT NULL,
    Type INTEGER NOT NULL,
    Description TEXT NOT NULL,
    Amount NUMERIC NOT NULL,
    TransactionDate TEXT NOT NULL,
    PostedDate TEXT NULL,
    CategoryId TEXT NULL,
    SubcategoryId TEXT NULL,
    ExternalId TEXT NULL,
    Source TEXT NOT NULL,
    IsRecurring INTEGER NOT NULL,
    IsSubscriptionCandidate INTEGER NOT NULL,
    ImportBatchId TEXT NULL,
    Notes TEXT NULL,
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT NULL,
    FOREIGN KEY (UserId) REFERENCES Users (Id) ON DELETE CASCADE,
    FOREIGN KEY (AccountId) REFERENCES Accounts (Id) ON DELETE SET NULL,
    FOREIGN KEY (CreditCardId) REFERENCES CreditCards (Id) ON DELETE SET NULL,
    FOREIGN KEY (CategoryId) REFERENCES TransactionCategories (Id) ON DELETE SET NULL,
    FOREIGN KEY (SubcategoryId) REFERENCES TransactionSubcategories (Id) ON DELETE SET NULL,
    FOREIGN KEY (ImportBatchId) REFERENCES ImportBatches (Id) ON DELETE SET NULL
);

CREATE INDEX IF NOT EXISTS IX_Transactions_UserId_TransactionDate
    ON Transactions (UserId, TransactionDate);

CREATE INDEX IF NOT EXISTS IX_Transactions_UserId_ExternalId
    ON Transactions (UserId, ExternalId);

CREATE INDEX IF NOT EXISTS IX_Transactions_AccountId
    ON Transactions (AccountId);

CREATE INDEX IF NOT EXISTS IX_Transactions_CreditCardId
    ON Transactions (CreditCardId);

CREATE INDEX IF NOT EXISTS IX_Transactions_CategoryId
    ON Transactions (CategoryId);

CREATE INDEX IF NOT EXISTS IX_Transactions_SubcategoryId
    ON Transactions (SubcategoryId);

CREATE INDEX IF NOT EXISTS IX_Transactions_ImportBatchId
    ON Transactions (ImportBatchId);

CREATE TABLE IF NOT EXISTS CategorizationRules (
    Id TEXT NOT NULL PRIMARY KEY,
    UserId TEXT NOT NULL,
    FieldName TEXT NOT NULL,
    Operator TEXT NOT NULL,
    CompareValue TEXT NOT NULL,
    CategoryId TEXT NOT NULL,
    SubcategoryId TEXT NULL,
    Priority INTEGER NOT NULL,
    IsActive INTEGER NOT NULL,
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT NULL,
    FOREIGN KEY (UserId) REFERENCES Users (Id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS IX_CategorizationRules_UserId
    ON CategorizationRules (UserId);

CREATE TABLE IF NOT EXISTS Budgets (
    Id TEXT NOT NULL PRIMARY KEY,
    UserId TEXT NOT NULL,
    CategoryId TEXT NOT NULL,
    Year INTEGER NOT NULL,
    Month INTEGER NOT NULL,
    PlannedAmount NUMERIC NOT NULL,
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT NULL,
    FOREIGN KEY (UserId) REFERENCES Users (Id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS IX_Budgets_UserId
    ON Budgets (UserId);

CREATE TABLE IF NOT EXISTS Goals (
    Id TEXT NOT NULL PRIMARY KEY,
    UserId TEXT NOT NULL,
    Name TEXT NOT NULL,
    TargetAmount NUMERIC NOT NULL,
    CurrentAmount NUMERIC NOT NULL,
    TargetDate TEXT NULL,
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT NULL,
    FOREIGN KEY (UserId) REFERENCES Users (Id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS IX_Goals_UserId
    ON Goals (UserId);

CREATE TABLE IF NOT EXISTS Notifications (
    Id TEXT NOT NULL PRIMARY KEY,
    UserId TEXT NOT NULL,
    Title TEXT NOT NULL,
    Message TEXT NOT NULL,
    IsRead INTEGER NOT NULL,
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT NULL,
    FOREIGN KEY (UserId) REFERENCES Users (Id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS IX_Notifications_UserId
    ON Notifications (UserId);

CREATE TABLE IF NOT EXISTS SubscriptionDetections (
    Id TEXT NOT NULL PRIMARY KEY,
    UserId TEXT NOT NULL,
    Merchant TEXT NOT NULL,
    AverageAmount NUMERIC NOT NULL,
    OccurrenceCount INTEGER NOT NULL,
    IsActive INTEGER NOT NULL,
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT NULL,
    FOREIGN KEY (UserId) REFERENCES Users (Id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS IX_SubscriptionDetections_UserId
    ON SubscriptionDetections (UserId);

CREATE TABLE IF NOT EXISTS AuditLogs (
    Id TEXT NOT NULL PRIMARY KEY,
    UserId TEXT NOT NULL,
    Action TEXT NOT NULL,
    Resource TEXT NOT NULL,
    OldValues TEXT NULL,
    NewValues TEXT NULL,
    IpAddress TEXT NOT NULL,
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT NULL,
    FOREIGN KEY (UserId) REFERENCES Users (Id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS IX_AuditLogs_UserId
    ON AuditLogs (UserId);

-- Opcional para cenários com EF migrations
CREATE TABLE IF NOT EXISTS __EFMigrationsHistory (
    MigrationId TEXT NOT NULL PRIMARY KEY,
    ProductVersion TEXT NOT NULL
);
