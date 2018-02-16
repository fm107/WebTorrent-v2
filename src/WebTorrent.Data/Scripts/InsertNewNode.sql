CREATE PROCEDURE InsertNewNode
	@pparent_id INT,
	@name NVARCHAR(MAX),
	@file_type INT
AS
BEGIN
	DECLARE @nodeId INT;
	--insert home directory
	IF(@pparent_id = -1)
	BEGIN		
		INSERT INTO dbo.DirectoryTreeMap
			(ParentId, Lft, Rgt, Name, FileType )
			VALUES  ( 0, -- ParentId - int
					  1, -- Lft - int
					  2, -- Rgt - int
					  @name,
					  @file_type)
			SELECT @nodeId = 1--@@IDENTITY
			
		--INSERT INTO TreeContent (NodeId, Name) VALUES (@nodeId, @name);
	END
	ELSE
    BEGIN
		SELECT @nodeId = rgt FROM DirectoryTreeMap WHERE Id = @pparent_id;
				UPDATE DirectoryTreeMap SET rgt = rgt + 2 WHERE rgt >= @nodeId;
				UPDATE DirectoryTreeMap SET lft = lft + 2 WHERE lft > @nodeId;
			INSERT INTO DirectoryTreeMap (lft, rgt, parentId, Name, FileType) VALUES (@nodeId, (@nodeId + 1), @pparent_id, @name, @file_type);
			SELECT @nodeId = SCOPE_IDENTITY();
			--INSERT INTO TreeContent (NodeId, Name) VALUES (@nodeId, @name);
	END
END

