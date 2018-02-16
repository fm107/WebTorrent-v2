CREATE PROCEDURE MoveNode
	@pnodeId INT,--node need to move
	@pparentNodeId INT, -- parent node id that you want to move to it
	@pleft INT,
	@pright INT
AS
BEGIN

	DECLARE @width INT, @distance INT,
	@pleftParentNodeId int, @prightParentNodeId INT, @tmp int;
	-- insert to right side
	SET @width = @pright - @pleft + 1;
	SELECT @pleftParentNodeId = lft, @prightParentNodeId = Rgt FROM dbo.DirectoryTreeMap WHERE id = @pparentNodeId;
	SET @distance = @prightParentNodeId - @pleft;
	PRINT 'distance: ' + CONVERT(NVARCHAR(10),@distance)
	PRINT 'width: '  + CONVERT(NVARCHAR(10), @width)
	PRINT 'left: ' + CONVERT(NVARCHAR(10), @pleftParentNodeId)

	PRINT 'right: ' + CONVERT(NVARCHAR(10), @prightParentNodeId)
	IF(@distance > 0)
	BEGIN
		SET @tmp = @distance - @width

		CREATE TABLE #working_DirectoryTreeMap
		(
			NodeId INT NOT NULL,
			[ParentId] [INT] NOT NULL,
			[Lft] [INT] NOT NULL,
			[Rgt] [INT] NOT NULL,
			[Name] [NVARCHAR](MAX) NULL,
		);

		INSERT #working_DirectoryTreeMap (NodeId, ParentId, Lft, Rgt, Name)
				SELECT Id, ParentId, Lft, Rgt, Name FROM dbo.DirectoryTreeMap WHERE Lft >= @pleft AND Rgt <= @pright;
	
		SELECT NodeId, Name FROM #working_DirectoryTreeMap
		--DELETE FROM #working_DirectoryTreeMap WHERE Lft >= @pleft AND Rgt <= @pright;

		--update left side
		UPDATE dbo.DirectoryTreeMap SET Lft = Lft - @width WHERE Lft > @pright AND Lft < @prightParentNodeId

		UPDATE dbo.DirectoryTreeMap SET Rgt = Rgt - @width WHERE Rgt > @pright AND Rgt < @prightParentNodeId

		-- update right side

		UPDATE dbo.DirectoryTreeMap SET Lft = Lft + @width WHERE Lft > @prightParentNodeId

		UPDATE dbo.DirectoryTreeMap SET Rgt = Rgt + @width WHERE Rgt > @prightParentNodeId

		-- update move folder
		UPDATE dbo.DirectoryTreeMap
			SET
				DirectoryTreeMap.lft = B.Lft + @tmp,
				DirectoryTreeMap.Rgt = B.Rgt + @tmp
			FROM
				DirectoryTreeMap AS A
				INNER JOIN #working_DirectoryTreeMap AS B
					ON A.id = B.NodeId
	
		UPDATE dbo.DirectoryTreeMap SET ParentId = @pparentNodeId WHERE id = @pnodeId

		----- 

		DROP TABLE #working_DirectoryTreeMap
	END
	ELSE
    BEGIN
		SET @tmp = @distance - @width -- -1

		CREATE TABLE #working_DirectoryTreeMap1
		(
			NodeId INT NOT NULL,
			[ParentId] [INT] NOT NULL,
			[Lft] [INT] NOT NULL,
			[Rgt] [INT] NOT NULL,
			[Name] [NVARCHAR](MAX) NULL,
		);

		INSERT #working_DirectoryTreeMap1 (NodeId, ParentId, Lft, Rgt, Name)
				SELECT Id, ParentId, Lft, Rgt, Name FROM dbo.DirectoryTreeMap WHERE Lft >= @pleft AND Rgt <= @pright;
	
		SELECT NodeId, Name FROM #working_DirectoryTreeMap1
		--DELETE FROM #working_DirectoryTreeMap WHERE Lft >= @pleft AND Rgt <= @pright;

		--update left side
		--UPDATE dbo.DirectoryTreeMap SET Lft = Lft - @width WHERE Lft > @pright AND Lft < @prightParentNodeId

		--UPDATE dbo.DirectoryTreeMap SET Rgt = Rgt - @width WHERE Rgt > @pright AND Rgt < @prightParentNodeId

		-- update right side

		UPDATE dbo.DirectoryTreeMap SET Lft = Lft + @width WHERE Lft > @prightParentNodeId

		UPDATE dbo.DirectoryTreeMap SET Rgt = Rgt + @width WHERE Rgt > @prightParentNodeId

		-- update move folder
		UPDATE dbo.DirectoryTreeMap
			SET
				DirectoryTreeMap.lft = B.Lft + @tmp,
				DirectoryTreeMap.Rgt = B.Rgt + @tmp
			FROM
				DirectoryTreeMap AS A
				INNER JOIN #working_DirectoryTreeMap1 AS B
					ON A.id = B.NodeId
	
		UPDATE dbo.DirectoryTreeMap SET ParentId = @pparentNodeId WHERE id = @pnodeId

		----- 

		DROP TABLE #working_DirectoryTreeMap1
	END
	
        
END