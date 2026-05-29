IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'asp_net_10_erudio')
BEGIN
    CREATE DATABASE asp_net_10_erudio;
    PRINT 'Database asp_net_10_erudio created successfully.';
END
ELSE
BEGIN
    PRINT 'Database asp_net_10_erudio already exists.';
END
GO

USE asp_net_10_erudio;
GO