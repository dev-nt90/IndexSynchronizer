--retrieve indexes 
--TODO: figure out deployment model

DECLARE @TableName SYSNAME;

--foreach index in tablename
DECLARE @CurrentIndexName SYSNAME;
DECLARE IndexNameCursor CURSOR LOCAL FAST_FORWARD 
FOR
SELECT i.name
FROM sys.tables t
JOIN sys.indexes i
	ON t.object_id = i.object_id
WHERE t.name LIKE @TableName;

OPEN IndexNameCursor

FETCH NEXT FROM IndexNameCursor
INTO @CurrentIndexName

DECLARE @SQL NVARCHAR(MAX);

WHILE(@@FETCH_STATUS = 0)
BEGIN
	SET @SQL = (
		SELECT
			'CREATE ' +
			CASE
				WHEN i.is_unique = 1 THEN 'UNIQUE '
				ELSE ''
			END +

			/*
				note that i.type_desc will correspond to one of these types:
				HEAP
				CLUSTERED
				NONCLUSTERED
				XML
				SPATIAL
				CLUSTERED COLUMNSTORE
				NONCLUSTERED COLUMNSTORE
				NONCLUSTERED HASH
			*/
			i.type_desc + ' INDEX ' +
			QUOTENAME(i.name) + ' ON ' +

			--now we need the object identifier (schema, name)
			--quotename is important here to ensure we include valid escape characters
			--e.g. quotename('abc[]def') spits out abc[]]def - an escaped right bracket
			QUOTENAME(OBJECT_SCHEMA_NAME(i.object_id)) + '.' + QUOTENAME(OBJECT_NAME(i.object_id)) + ' (' +

			--now we need the actual columns of the index
			STUFF((
				SELECT ', ' + QUOTENAME(c.name) + CASE WHEN ic.is_descending_key = 1 THEN ' DESC' ELSE ' ASC' END
				FROM sys.index_columns AS ic
				INNER JOIN sys.columns AS c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
				WHERE i.object_id = ic.object_id AND i.index_id = ic.index_id
				FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') + ');'
		FROM sys.indexes AS i
		WHERE i.name = @CurrentIndexName
		FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)');

	PRINT @SQL;

	FETCH NEXT FROM IndexNameCursor
	INTO @CurrentIndexName
END;

CLOSE IndexNameCursor;
DEALLOCATE IndexNameCursor;
GO


