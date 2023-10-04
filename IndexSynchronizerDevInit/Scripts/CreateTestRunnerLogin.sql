USE master;

--note: sections wrapped in '#' characters indicate a replacement token to be injected at runtime
--note: we can get away with this because we control the characters directly - i.e. we're not vulnerable to injection
IF NOT EXISTS
(
	SELECT 1
	FROM sys.server_principals
	WHERE name = 'IndexSyncTestLogin'
)
BEGIN
	CREATE LOGIN [IndexSyncTestLogin] WITH PASSWORD = '#Password#';
END;

USE #TargetDatabase#

IF NOT EXISTS 
(
	SELECT 1 
	FROM sys.database_principals 
	WHERE name = 'IndexSyncTestUser'
)
BEGIN
	CREATE USER [IndexSyncTestUser] FOR LOGIN [IndexSyncTestLogin];
END;