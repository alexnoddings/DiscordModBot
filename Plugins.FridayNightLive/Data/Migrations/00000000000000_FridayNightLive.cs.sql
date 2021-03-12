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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'00000000000000_FridayNightLive')
BEGIN
    CREATE TABLE [FnlGuildConfig] (
        [GuildId] decimal(20,0) NOT NULL,
        [WinnerRoleId] decimal(20,0) NOT NULL,
        [HostRoleId] decimal(20,0) NOT NULL,
        [ThumbnailUrl] nvarchar(max) NULL,
        [LeaderboardMessageId] decimal(20,0) NOT NULL,
        CONSTRAINT [PK_FnlGuildConfig] PRIMARY KEY CLUSTERED ([GuildId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'00000000000000_FridayNightLive')
BEGIN
    CREATE TABLE [FnlSessions] (
        [GuildId] decimal(20,0) NOT NULL,
        [Number] int NOT NULL IDENTITY,
        [Date] datetime2 NULL,
        [Activity] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_FnlSessions] PRIMARY KEY CLUSTERED ([GuildId], [Number])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'00000000000000_FridayNightLive')
BEGIN
    CREATE TABLE [FnlSessionHosts] (
        [GuildId] decimal(20,0) NOT NULL,
        [SessionNumber] int NOT NULL,
        [UserId] decimal(20,0) NOT NULL,
        CONSTRAINT [PK_FnlSessionHosts] PRIMARY KEY CLUSTERED ([GuildId], [SessionNumber], [UserId]),
        CONSTRAINT [FK_FnlSessionHosts_FnlSessions_GuildId_SessionNumber] FOREIGN KEY ([GuildId], [SessionNumber]) REFERENCES [FnlSessions] ([GuildId], [Number]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'00000000000000_FridayNightLive')
BEGIN
    CREATE TABLE [FnlSessionWinners] (
        [GuildId] decimal(20,0) NOT NULL,
        [SessionNumber] int NOT NULL,
        [UserId] decimal(20,0) NOT NULL,
        CONSTRAINT [PK_FnlSessionWinners] PRIMARY KEY CLUSTERED ([GuildId], [SessionNumber], [UserId]),
        CONSTRAINT [FK_FnlSessionWinners_FnlSessions_GuildId_SessionNumber] FOREIGN KEY ([GuildId], [SessionNumber]) REFERENCES [FnlSessions] ([GuildId], [Number]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'00000000000000_FridayNightLive')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'00000000000000_FridayNightLive', N'6.0.0-preview.1.21102.2');
END;
GO

COMMIT;
GO

