CREATE PROCEDURE "movebreadcrumb" 
@CurrentRoot CHAR(100), 
@NewParent CHAR(100)
AS

-- Need two parameters: (1) CurrentRoot, and (2) NewParent.
DECLARE @Origin_Lft INTEGER;
DECLARE @Origin_Rgt INTEGER;
DECLARE @NewParent_Rgt INTEGER;


SELECT 
    @Origin_Lft= Lft,
    @Origin_Rgt = Rgt
FROM  "breadcrumblinks"
WHERE Page_Title = @CurrentRoot;


SET @NewParent_Rgt = (SELECT Rgt FROM "breadcrumblinks"
	WHERE Page_Title = @NewParent);

UPDATE "breadcrumblinks" 
	SET Lft = Lft + 
	CASE
		WHEN @NewParent_Rgt < @Origin_Lft
			THEN CASE
				WHEN Lft BETWEEN @Origin_Lft AND @Origin_Rgt
					THEN @NewParent_Rgt - @Origin_Lft
				WHEN Lft BETWEEN @NewParent_Rgt	AND @Origin_Lft - 1
					THEN @Origin_Rgt - @Origin_Lft + 1
				ELSE 0 END
		WHEN @NewParent_Rgt > @Origin_Rgt
			THEN CASE
				WHEN Lft BETWEEN @Origin_Lft AND @Origin_Rgt
					THEN @NewParent_Rgt - @Origin_Rgt - 1
				WHEN Lft BETWEEN @Origin_Rgt + 1 AND @NewParent_Rgt - 1
					THEN @Origin_Lft - @Origin_Rgt - 1
				ELSE 0 END
			ELSE 0 END,
	Rgt = Rgt + 
	CASE
		WHEN @NewParent_Rgt < @Origin_Lft
			THEN CASE
		WHEN Rgt BETWEEN @Origin_Lft AND @Origin_Rgt
			THEN @NewParent_Rgt - @Origin_Lft
		WHEN Rgt BETWEEN @NewParent_Rgt AND @Origin_Lft - 1
			THEN @Origin_Rgt - @Origin_Lft + 1
		ELSE 0 END
		WHEN @NewParent_Rgt > @Origin_Rgt
			THEN CASE
				WHEN Rgt BETWEEN @Origin_Lft AND @Origin_Rgt
					THEN @NewParent_Rgt - @Origin_Rgt - 1
				WHEN Rgt BETWEEN @Origin_Rgt + 1	AND @NewParent_Rgt - 1
					THEN @Origin_Lft - @Origin_Rgt - 1
				ELSE 0 END
			ELSE 0 END;