--retrieve indexes 
--TODO: figure out deployment model

CREATE TABLE #Indexes (IndexDefinition NVARCHAR(MAX), IndexName NVARCHAR(MAX));

--NOTE: @SchemaName and @TableName is injected at runtime
DECLARE @FullyQualifiedTableName NVARCHAR(4000) = CONCAT(@SchemaName, '.', @TableName);

--foreach index in tablename
DECLARE @CurrentIndexName SYSNAME;
DECLARE IndexNameCursor CURSOR LOCAL FAST_FORWARD 
FOR
SELECT i.name
FROM sys.tables t
JOIN sys.indexes i
	ON t.object_id = i.object_id
WHERE t.object_id = OBJECT_ID(@FullyQualifiedTableName)
	AND i.is_primary_key = 0 -- TODO: figure out how to support indexes supporting PKs

OPEN IndexNameCursor

FETCH NEXT FROM IndexNameCursor
INTO @CurrentIndexName

DECLARE @FinalSQL NVARCHAR(MAX);
DECLARE @UniquePredicate NVARCHAR(MAX);
DECLARE @IndexType NVARCHAR(MAX);
DECLARE @FullyQualifiedName NVARCHAR(MAX);
DECLARE @KeyColumns NVARCHAR(MAX);
DECLARE @IncludedColumns NVARCHAR(MAX);
DECLARE @FilterPredicate NVARCHAR(MAX);
DECLARE @FileGroupName NVARCHAR(MAX);
DECLARE @PadIndexOption NVARCHAR(MAX);
DECLARE @NoRecomputeOption NVARCHAR(MAX);
DECLARE @IgnoreDupeKeyOption NVARCHAR(MAX);
DECLARE @DropExistingOption NVARCHAR(MAX);
DECLARE @OnlineOption NVARCHAR(MAX);
DECLARE @AllowRowLocksOption NVARCHAR(MAX);
DECLARE @AllowPageLocksOption NVARCHAR(MAX);
DECLARE @OptimizeForSequentialKeyOption NVARCHAR(MAX);

WHILE(@@FETCH_STATUS = 0)
BEGIN
	--using the following documentation, generate a string which can be used to completely rebuild an index from scratch:
	--https://learn.microsoft.com/en-us/sql/t-sql/statements/create-index-transact-sql?view=sql-server-ver16

	SELECT
		@UniquePredicate = 
			CASE
			WHEN i.is_unique = 1
			THEN 'UNIQUE'
			ELSE ''
			END,
		@IndexType = i.type_desc,
		@FullyQualifiedName = QUOTENAME(OBJECT_SCHEMA_NAME(i.object_id)) + '.' + QUOTENAME(OBJECT_NAME(i.object_id)),
		@KeyColumns = STUFF(
		(
			SELECT 
				', ' + QUOTENAME(c.name) + 
					CASE 
					WHEN ic.is_descending_key = 1 
					THEN ' DESC' 
					ELSE ' ASC' 
					END
			FROM sys.index_columns AS ic
			INNER JOIN sys.columns AS c 
				ON ic.object_id = c.object_id 
				AND ic.column_id = c.column_id
			WHERE i.object_id = ic.object_id 
				AND i.index_id = ic.index_id
				AND ic.is_included_column = 0
			FOR XML PATH(''), TYPE
		).value('.', 'NVARCHAR(MAX)'), 1, 2, ''),

		@IncludedColumns = STUFF(
		(
			SELECT 
				', ' + QUOTENAME(c.name)
			FROM sys.index_columns AS ic
			INNER JOIN sys.columns AS c 
				ON ic.object_id = c.object_id 
				AND ic.column_id = c.column_id
			WHERE i.object_id = ic.object_id 
				AND i.index_id = ic.index_id
				AND ic.is_included_column = 1
			FOR XML PATH(''), TYPE
		).value('.', 'NVARCHAR(MAX)'), 1, 2, ''),

		@FilterPredicate = i.filter_definition,
		@FileGroupName = QUOTENAME(ds.name),

		--e.g. WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
		@PadIndexOption = CONCAT('PAD_INDEX=', CASE WHEN i.is_padded = 1 THEN 'ON' ELSE 'OFF' END),
		@NoRecomputeOption = CONCAT('STATISTICS_NORECOMPUTE=', CASE WHEN s.no_recompute = 1 THEN 'ON' ELSE 'OFF' END),
		@IgnoreDupeKeyOption = CONCAT('IGNORE_DUP_KEY=', CASE WHEN i.ignore_dup_key = 1 THEN 'ON' ELSE 'OFF' END),
		@DropExistingOption = 'DROP_EXISTING=OFF',
		@OnlineOption = 'ONLINE=OFF',
		@AllowRowLocksOption = CONCAT('ALLOW_ROW_LOCKS=', CASE WHEN i.allow_row_locks =1 THEN 'ON' ELSE 'OFF' END),
		@AllowPageLocksOption = CONCAT('ALLOW_PAGE_LOCKS=', CASE WHEN i.allow_page_locks=1 THEN 'ON' ELSE 'OFF' END),
		@OptimizeForSequentialKeyOption = CONCAT('OPTIMIZE_FOR_SEQUENTIAL_KEY=', CASE WHEN i.optimize_for_sequential_key=1 THEN 'ON' ELSE 'OFF' END)
	FROM sys.indexes i
	JOIN sys.tables t
		ON i.object_id = t.object_id
	JOIN sys.data_spaces ds
		ON i.data_space_id = ds.data_space_id
	JOIN sys.stats s
		ON s.object_id = i.object_id
	WHERE t.name LIKE @TableName
		AND i.name LIKE @CurrentIndexName
		AND i.is_hypothetical = 0
		AND i.index_id <> 0;

	--assemble the disparate pieces into our final definition
	SET @FinalSQL = CONCAT(
		'CREATE ',
		@UniquePredicate, ' ',
		@IndexType, ' ',
		' INDEX ', 
		@CurrentIndexName, ' ',
		' ON ',
		@FullyQualifiedName, ' (',
		@KeyColumns, ') ',
		CASE WHEN @IncludedColumns IS NOT NULL THEN ' INCLUDE (' + @IncludedColumns + ') ' ELSE '' END,
		CASE WHEN @FilterPredicate IS NOT NULL THEN ' WHERE ' + @FilterPredicate ELSE '' END,
		' WITH (',
		@PadIndexOption, ', ',
		@NoRecomputeOption, ', ',
		@IgnoreDupeKeyOption, ', ',
		@DropExistingOption, ', ',
		@OnlineOption, ', ',
		@AllowRowLocksOption, ', ',
		@AllowPageLocksOption, ', ',
		@OptimizeForSequentialKeyOption, ') ',
		' ON ', 
		@FileGroupName
	);

	IF(@FinalSQL IS NOT NULL)
	BEGIN
		INSERT #Indexes
		SELECT @FinalSQL, @CurrentIndexName;
	END;
		
	FETCH NEXT FROM IndexNameCursor
	INTO @CurrentIndexName
END;

SELECT IndexDefinition, IndexName
FROM #Indexes

CLOSE IndexNameCursor;
DEALLOCATE IndexNameCursor;
