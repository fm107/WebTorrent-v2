CREATE PROCEDURE "deletebreadcrumbbasedonpagetitle" 
@PageTitle varchar(100)
AS

-- Need one parameter (PageTitle), look at the line: WHERE  Page_Title = PageTitle;
DECLARE @DeletedPageTitle CHAR(50);
DECLARE @DeletedLft INT;
DECLARE @DeletedRgt INT;


SELECT 
    @DeletedPageTitle= Page_Title,
    @DeletedLft = Lft,
    @DeletedRgt = Rgt
FROM  "breadcrumblinks"
WHERE Page_Title = @PageTitle;

 
DELETE FROM "breadcrumblinks"
WHERE Lft BETWEEN @DeletedLft AND @DeletedRgt;
 
UPDATE "breadcrumblinks"
   SET Lft = CASE WHEN Lft > @DeletedLft THEN
             Lft - (@DeletedRgt - @DeletedLft + 1)
          ELSE
             Lft
          END,
       Rgt = CASE WHEN Rgt > @DeletedLft THEN
             Rgt - (@DeletedRgt - @DeletedLft + 1)
          ELSE
             Rgt
          END
   WHERE Lft > @DeletedLft
      OR Rgt > @DeletedLft;
