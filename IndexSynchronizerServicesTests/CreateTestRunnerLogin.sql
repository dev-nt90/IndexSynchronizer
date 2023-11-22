USE #TargetDatabase#

--recreate the user and login
--it's cleaner to first drop dependent objects of logins, so torch the user first
IF EXISTS
(
    SELECT 1 
    FROM sys.database_principals 
    WHERE name = 'IndexSyncTestUser'
)
BEGIN
    ALTER ROLE db_ddladmin DROP MEMBER [IndexSyncTestUser];
    DROP USER [IndexSyncTestUser];
END;

USE master;

IF EXISTS
(
    SELECT 1
    FROM sys.server_principals
    WHERE name = 'IndexSyncTestLogin'
)
BEGIN
    DROP LOGIN [IndexSyncTestLogin];
END;

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
    GRANT CREATE ANY DATABASE TO [IndexSyncTestLogin];
    ALTER SERVER ROLE dbcreator ADD MEMBER [IndexSyncTestLogin];
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
    ALTER ROLE db_ddladmin ADD MEMBER [IndexSyncTestUser];
END;