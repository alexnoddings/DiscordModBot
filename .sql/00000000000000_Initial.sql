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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'00000000000000_Initial')
BEGIN
    CREATE TABLE [UserGuildRoles] (
        [Guild] decimal(20,0) NOT NULL,
        [User] decimal(20,0) NOT NULL,
        [Roles] nvarchar(max) NOT NULL,
        [LeftUtc] datetime2 NOT NULL,
        CONSTRAINT [PK_UserGuildRoles] PRIMARY KEY CLUSTERED ([User], [Guild])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'00000000000000_Initial')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'00000000000000_Initial', N'6.0.0-preview.1.21102.2');
END;
GO

COMMIT;
GO
