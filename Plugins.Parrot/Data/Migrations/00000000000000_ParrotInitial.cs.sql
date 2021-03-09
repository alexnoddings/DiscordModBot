IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'00000000000000_ParrotInitial')
BEGIN
    CREATE TABLE [UserMessages] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] decimal(20,0) NOT NULL,
        [Message] nvarchar(max) NOT NULL,
        [TriggerAt] datetime2 NOT NULL,
        CONSTRAINT [PK_UserMessages] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'00000000000000_ParrotInitial')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'00000000000000_ParrotInitial', N'6.0.0-preview.1.21102.2');
END;
GO

COMMIT;
GO

