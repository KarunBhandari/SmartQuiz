--Creating a database
create database IQ_Mania
Alter table tblQuestions Alter column Questions nvarchar(500)
Select * from tblQuestions
truncate table tblQuestions
Use IQ_Mania
go
Create table new_tblQuestions(Questions nvarchar(500), Answer nvarchar(70), Category nvarchar(15))
Insert into tblQuestions(Questions, Answer, Category) 
(Select Questions, Answer, Category from new_tblQuestions)

DBCC CHECKIDENT('tblQuestions', RESEED,1)


--Insert datas to table
Alter table tblOptions
Drop Constraint FK__tblOption__Quest__34C8D9D1
--DEFAULT('History') For Category

Update tblQuestions
Set [Category] ='History'
where [Category] Is NULL

Alter table tblOptions
Add Question int Foreign Key(Question) references tblQuestions(Question_Number)


Drop procedure spGetQuestions
--Creating sps
Alter procedure spGetQuestions
@flag nvarchar(15), @category nvarchar(35)= Null
As 
Begin
If(@flag = 'GetMCQs')
 Begin
	If(@category='History')
	 Begin
		Select Top 100 Question_Number, Questions, Answer from tblQuestions (nolock) where Category='History'
		 Order By NEWID()
	End
    Else if(@category='Geography')
	 Begin
		Select Top 100 Question_Number, Questions, Answer from tblQuestions (nolock) where Category='Geography'
		Order By NEWID()
	End
	Else if(@category='Sports')
	 Begin
		Select Top 100 Question_Number, Questions, Answer from tblQuestions (nolock) where Category='Sports'
		Order By NEWID()
	End
	Else if(@category='Time and Events')
	 Begin
		Select Top 100 Question_Number, Questions, Answer from tblQuestions (nolock) where Category='Time and Events'
		Order By NEWID()
	End
	Else if(@category='Economy')
	 Begin
		Select Top 100 Question_Number, Questions, Answer from tblQuestions (nolock) where Category='Economy'
		Order By NEWID()
	End
	Else if(@category='Politics')
	 Begin
		Select Top 100 Question_Number, Questions, Answer from tblQuestions (nolock) where Category='Politics'
		Order By NEWID()
	End
	End
	Else if(@flag='All')
	 Begin
		Select Question_Number, Questions, Answer from tblQuestions (nolock)
	 
	
 End
End

Exec sp_helptext spGetQuestions

Execute spGetQuestions @flag='All', @category='Geography'
--table options
Create table Options ( Option1 nvarchar(70), Option2 nvarchar(70),Option3 nvarchar(70), Option4 nvarchar(70), Question int)

Truncate table tblOptions
DBCC CHECKIDENT ('tblOptions', RESEED, 1);


--Create table Options(SN int Primary Key Identity(1,1), Option1 nvarchar(70), Option2 nvarchar(70), Option3 nvarchar(70), Option4 nvarchar(70), Question int NOT NULL, Foreign Key (Question) references dbo.tblQuestions(Question_Number));
Insert into tblOptions 
Select Option1, Option2, Option3, Option4, Question from Options
Select * from tblOptions
Drop table Options
EXEC sp_rename 'Options', 'tblOptions';

Exec sp_helptext spGetMCQs
Exec sp_helptext PK__tblQuest

Select * from tblOptions
Select * from tblQuestions



Alter View vWMultipleChoiceQuestions
as
Select Question_Number, Questions,Answer, Category, Option1, Option2, Option3, Option4 from tblQuestions (nolock)
join tblOptions (nolock)
on tblQuestions.Question_Number = tblOptions.Question

--Alter procedure spGetMCQs
--@flag nvarchar(10)
--As
--Begin
--If(@flag = 'Qstbycat')
--Begin
--	SELECT *
--FROM (
--  SELECT TOP 20 Question_Number, Questions, Option1, Option2, Option3, Option4
--  from vWMultipleChoiceQuestions (nolock)
--  WHERE Category = 'History'
  
--  ORDER BY NEWID()
--) AS HistoryQuestions 
--UNION
--SELECT *
--FROM (
--  SELECT TOP 20 Question_Number, Questions, Option1, Option2, Option3, Option4
--  from vWMultipleChoiceQuestions (nolock)
--  WHERE Category = 'Geography'
--  ORDER BY NEWID()
--) AS GeographyQuestions
--UNION
--SELECT *
--FROM (
--  SELECT TOP 20 Question_Number, Questions, Option1, Option2, Option3, Option4
--  from vWMultipleChoiceQuestions (nolock)
--  WHERE Category = 'Sports'
--  ORDER BY NEWID()
--) AS SportsQuestions
--UNION
--SELECT *
--FROM (
--  SELECT TOP 20 Question_Number, Questions, Option1, Option2, Option3, Option4
--  from vWMultipleChoiceQuestions (nolock)
--  WHERE Category = 'Politics'
--  ORDER BY NEWID()
--) AS PoliticsQuestions
--UNION
--SELECT *
--FROM (
--  SELECT TOP 20 Question_Number, Questions, Option1, Option2, Option3, Option4
--  from vWMultipleChoiceQuestions (nolock)
--  WHERE Category = 'Time and Events'
--  ORDER BY NEWID()
--) AS TimeandEventsQuestions
--End
--End 

--Update tblQuestions
--Set Category = 'History'


  Alter PROCEDURE spGetMCQs
    @flag nvarchar(10)
AS
BEGIN
    IF (@flag = 'Qstbycat')
    BEGIN
        SELECT Question_Number, Questions, Option1, Option2, Option3, Option4
        FROM (
            SELECT *,
                   ROW_NUMBER() OVER (PARTITION BY Category ORDER BY CHECKSUM(NEWID())) AS rn
            FROM vWMultipleChoiceQuestions (NOLOCK)
        ) AS RandomizedQuestions
        WHERE rn <= 20
        AND Category IN ('History', 'Geography', 'Sports', 'Politics', 'Time and Events')
    END
END

Execute spGetMCQs @flag= 'Qstbycat'

Select Question_Number, Questions, Option1, Option2, Option3, Option4 from vWMultipleChoiceQuestions where Category = @Category  

Execute sp_helptext spGetMCQs

 Select * from tblQuestions where Category='History'  
 Insert into tblQuestions
Values
('Who was the first king of Gopal Dynasty?','Bhuktaman',	'A')

 Alter procedure spAddMCQ
 @Question nvarchar(200), @Answer nvarchar(70), @Category nvarchar(15), 
 @Option1 nvarchar(70), @Option2 nvarchar(70), @Option3 nvarchar(70), @Option4 nvarchar(70),
 @flag nvarchar(15)
 AS
 Begin
  If
  (@flag = 'AddminUser')
  Begin
  Insert into tblOptions (Option1, Option2, Option3, Option4)
  values
  (@Option1, @Option2, @Option3, @Option4)
  Insert into dbo.tblQuestions (Questions, Answer, Category)
  values
  (@Question, @Answer, @Category)
 End
End

EXEC spAddMCQ
@flag = 'AddminUser',
  @Question = 'What is the world economic inflation rate for the year 2023AD?',
  @Answer = '4.7%',
  @Category = 'Economy',
  @Option1 = '4.8%',
  @Option2 = '4.7%',
  @Option3 = '4.2%',
  @Option4 = '5.1%'

Alter trigger tr_tblOptions_ForInsert
ON tblQuestions
AFTER INSERT
AS
BEGIN
    UPDATE tblOptions
    SET Question = (SELECT IDENT_CURRENT('tblQuestions') )
    WHERE tblOptions.Question Is NULL;
END


Delete from tblOptions where SN>=35
Delete from tblQuestions where Question_Number>=35

ALTER TABLE tblOptions
ALTER COLUMN Question int NULL;

INSERT INTO tblOptions (Question)
SELECT Question_Number
FROM tblQuestions;

Select * from vWMultipleChoiceQuestions
Select * from tblOptions
Select * from tblQuestions


Alter table tblOptions
Add Constraint FK_Question_Qno
Foreign Key(Question) references tblQuestions(Question_Number)
Use IQ_Mania
UPDATE tblOptions
SET Question = (
    SELECT Question_Number
    FROM tblQuestions
    WHERE tblQuestions.Question_Number = tblOptions.SN)

Create table Options(Option1 nvarchar(70), Option2 nvarchar(70), Option3 nvarchar(70), Option4 nvarchar(70))
Update tblOptions
Set Option2 ='Tundra Coniferous forest found in Russia'
where tblOptions.Question = 45

truncate table tblOptions
Insert into tblOptions(Option1, Option2, Option3,Option4)
Select Option1, Option2, Option3, Option4 from Options where Question=48

Select * from tblOptions
Select * from tblQuestions
Set identity_insert tblOptions OFF
DBCC CHECKIDENT('tblOptions', RESEED,1)

USE [IQ_Mania]
GO

Drop table Options
WHERE Option3 IN (
    SELECT Option3
    FROM Options
    GROUP BY Option3
    HAVING COUNT(*) > 1
);

Select * from tblOptions
/****** Object:  Table [dbo].[tblOptions]    Script Date: 7/14/2023 11:27:37 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create TABLE [dbo].[new_tblOptions] 
(
	[SN] [int] IDENTITY(1,1)  ,
	[Option1] [nvarchar](70) ,
	[Option2] [nvarchar](70) ,
	[Option3] [nvarchar](70) ,
	[Option4] [nvarchar](70) ,
	[Option4] [int])


	USE [IQ_Mania]
GO

ALTER TABLE [dbo].[tblOptions]  WITH CHECK ADD FOREIGN KEY([Question])
REFERENCES [dbo].[tblQuestions] ([Question_Number])
GO


