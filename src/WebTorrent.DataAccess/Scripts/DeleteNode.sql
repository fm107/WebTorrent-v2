--Only delete if it is a leaf
CREATE PROCEDURE DeleteNode
	@pnodeId INT--node need to move
AS
BEGIN
	DECLARE @leftNodeId INT, @rightNodeId INT, @width INT, @count INT;
    SELECT @leftNodeId = lft, @rightNodeId = Rgt FROM dbo.Content WHERE id = @pnodeId;
	SET @width = @rightNodeId - @leftNodeId + 1;
	SET @count = @width / 2;
	IF(@count = 1)
	BEGIN
		UPDATE dbo.Content SET Lft = Lft - @width WHERE Lft > @leftNodeId;
		UPDATE dbo.Content SET Rgt = Rgt - @width WHERE Rgt > @rightNodeId;
		DELETE dbo.Content WHERE id = @pnodeId
	END
END