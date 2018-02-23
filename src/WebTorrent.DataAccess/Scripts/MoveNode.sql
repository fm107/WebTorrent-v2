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
	SELECT @pleftParentNodeId = lft, @prightParentNodeId = Rgt FROM dbo.Content WHERE id = @pparentNodeId;
	SET @distance = @prightParentNodeId - @pleft;
	PRINT 'distance: ' + CONVERT(NVARCHAR(10),@distance)
	PRINT 'width: '  + CONVERT(NVARCHAR(10), @width)
	PRINT 'left: ' + CONVERT(NVARCHAR(10), @pleftParentNodeId)

	PRINT 'right: ' + CONVERT(NVARCHAR(10), @prightParentNodeId)
	IF(@distance > 0)
	BEGIN
		SET @tmp = @distance - @width

		CREATE TABLE #working_Content
		(
			NodeId INT NOT NULL,
			[ParentId] [INT] NOT NULL,
			[Lft] [INT] NOT NULL,
			[Rgt] [INT] NOT NULL,
			[Hash] [NVARCHAR](MAX) NULL,
		);

		INSERT #working_Content (NodeId, ParentId, Lft, Rgt, Hash)
				SELECT Id, ParentId, Lft, Rgt, Hash FROM dbo.Content WHERE Lft >= @pleft AND Rgt <= @pright;
	
		SELECT NodeId, Hash FROM #working_Content
		--DELETE FROM #working_Content WHERE Lft >= @pleft AND Rgt <= @pright;

		--update left side
		UPDATE dbo.Content SET Lft = Lft - @width WHERE Lft > @pright AND Lft < @prightParentNodeId

		UPDATE dbo.Content SET Rgt = Rgt - @width WHERE Rgt > @pright AND Rgt < @prightParentNodeId

		-- update right side

		UPDATE dbo.Content SET Lft = Lft + @width WHERE Lft > @prightParentNodeId

		UPDATE dbo.Content SET Rgt = Rgt + @width WHERE Rgt > @prightParentNodeId

		-- update move folder
		UPDATE dbo.Content
			SET
				Content.lft = B.Lft + @tmp,
				Content.Rgt = B.Rgt + @tmp
			FROM
				dbo.Content AS A
				INNER JOIN #working_Content AS B
					ON A.id = B.NodeId
	
		UPDATE dbo.Content SET ParentId = @pparentNodeId WHERE id = @pnodeId

		----- 

		DROP TABLE #working_Content
	END
	ELSE
    BEGIN
		SET @tmp = @distance - @width -- -1

		CREATE TABLE #working_Content1
		(
			NodeId INT NOT NULL,
			[ParentId] [INT] NOT NULL,
			[Lft] [INT] NOT NULL,
			[Rgt] [INT] NOT NULL,
			[Hash] [NVARCHAR](MAX) NULL,
		);

		INSERT #working_Content1 (NodeId, ParentId, Lft, Rgt, Hash)
				SELECT Id, ParentId, Lft, Rgt, Hash FROM dbo.Content WHERE Lft >= @pleft AND Rgt <= @pright;
	
		SELECT NodeId, Hash FROM #working_Content1
		--DELETE FROM #working_Content WHERE Lft >= @pleft AND Rgt <= @pright;

		--update left side
		--UPDATE dbo.Content SET Lft = Lft - @width WHERE Lft > @pright AND Lft < @prightParentNodeId

		--UPDATE dbo.Content SET Rgt = Rgt - @width WHERE Rgt > @pright AND Rgt < @prightParentNodeId

		-- update right side

		UPDATE dbo.Content SET Lft = Lft + @width WHERE Lft > @prightParentNodeId

		UPDATE dbo.Content SET Rgt = Rgt + @width WHERE Rgt > @prightParentNodeId

		-- update move folder
		UPDATE dbo.Content
			SET
				Content.lft = B.Lft + @tmp,
				Content.Rgt = B.Rgt + @tmp
			FROM
				dbo.Content AS A
				INNER JOIN #working_Content1 AS B
					ON A.id = B.NodeId
	
		UPDATE dbo.Content SET ParentId = @pparentNodeId WHERE id = @pnodeId

		----- 

		DROP TABLE #working_Content1
	END        
END