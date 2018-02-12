CREATE PROCEDURE "addnewbreadcrumb" 
@PageTitleParent VARCHAR(50),
@PageTitle VARCHAR(50), 
@PageURL VARCHAR(50)
AS

GoodBye: 
-- Need three parameters (PageTitleParent, PageTitle, and PageURL), 
-- look at this line --> "Page_Title" = PageTitleParent);
-- look at this line --> VALUES (PageTitle, PageURL, ParentLevel, (ParentLevel + 1));
DECLARE @ParentLevel int;
DECLARE @RecCount int;
DECLARE @CheckRecCount int;
DECLARE @MyPageTitle VARCHAR(100);
 
SET @ParentLevel = (SELECT Rgt FROM "breadcrumblinks" WHERE 
Page_Title = @PageTitleParent);
 
SET @CheckRecCount = (SELECT COUNT(*) AS RecCount FROM "breadcrumblinks" WHERE 
"Page_Title" = @PageTitle);
	IF @CheckRecCount > 0 
	BEGIN
		SET @MyPageTitle = CONCAT('The following Page_Title is already exists in database: ', @PageTitle);
		SELECT @MyPageTitle;
		GOTO GoodBye;
	END

 
UPDATE "breadcrumblinks"
   SET Lft = CASE WHEN Lft > @ParentLevel THEN
      Lft + 2
    ELSE
      Lft + 0
    END,
   Rgt = CASE WHEN Rgt >= @ParentLevel THEN
      Rgt + 2
   ELSE
      Rgt + 0
   END
WHERE  Rgt >= @ParentLevel;
 
SET @RecCount = (SELECT COUNT(*) FROM "breadcrumblinks");
	IF @RecCount = 0
	BEGIN
		-- this is for handling the first record
		INSERT INTO "breadcrumblinks" (Page_Title, Page_URL, Lft, Rgt)
					VALUES (@PageTitle, @PageURL, 1, 2);
	END
	ELSE
	BEGIN
		-- whereas the following is for the second record, and so forth!
		INSERT INTO "breadcrumblinks" (Page_Title, Page_URL, Lft, Rgt)
					VALUES (@PageTitle, @PageURL, @ParentLevel, (@ParentLevel + 1));
	END