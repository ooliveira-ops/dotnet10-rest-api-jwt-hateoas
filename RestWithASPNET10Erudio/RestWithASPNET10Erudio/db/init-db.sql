IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'asp_net_10_erudio')
BEGIN
    CREATE DATABASE asp_net_10_erudio;
END
GO