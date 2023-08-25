Create table tblAccount(Id int Primary Key identity, FullName nvarchar(55), Email nvarchar(35), Phone nvarchar(15),
 Password nvarchar(12))
 
 Use IQ_Mania 
 Select * from tblAccount

 Update tblAccount
 Set Role='AdminUser'
 where Id=1
 
 Alter procedure spcreateUser
 @Fullname nvarchar(55), @Email nvarchar(35),
 @Password nvarchar(12), @Phone bigint, @flag nvarchar(15)
 As
 Begin
 Declare @responsecode int 
 Declare @responsedescription nvarchar(70)
  
  Begin Try
	 If(@flag='Signup')
	 Begin
	   Insert into tblAccount (FullName, Email, Phone, Password) 
	   values
	   (@FullName, @Email,@Phone, @Password)
	   Set @responsecode = 201; Set @responsedescription = 'Successfully created user'
	End
	Else
	 Begin
	 Set @responsecode = 401; Set @responsedescription = 'Unauthorized request for creation'
	 
	End
 End Try
 Begin Catch
	Set @responsecode = 400; Set @responsedescription = 'Bad request for creation'
 End Catch
 Select @responsecode as ResponseCode, @responsedescription as ResponseDescription
End
Exec sp_helptext spcreateUser


ALTER TABLE tblAccount
ADD CONSTRAINT tbl_User_Role DEFAULT 'User' FOR [Role];


--Query to check options selected

--Create procedure spTestResult
--@answer varchar(30), @QuestionId Bigint
--As
--Begin
--IF EXISTS(SELECT 1 FROM tblOptions OP (NOLOCK) 
--        INNER JOIN tblQuestions TQ (NOLOCK)
--		ON OP.Question=TQ.Question_Number
--		WHERE TQ.Answer=@answer and op.Question=@QuestionId)
--		BEGIN

--		PRINT('CORRECT')
--		RETURN;
--		END 

--		PRINT('INCORRECT')
--		End

--		Exec spTestResult @answer='Mandev', @Questionid=2

		--Query to select random numbers
	SELECT *
FROM (
  SELECT TOP 30 Question_Number, Questions, Option1, Option2, Option3, Option4
  from vWMultipleChoiceQuestions (nolock)
  WHERE Category = 'History'
  ORDER BY NEWID()
) AS GeographyQuestions
UNION
SELECT *
FROM (
  SELECT TOP 30 Question_Number, Questions, Option1, Option2, Option3, Option4
  from vWMultipleChoiceQuestions (nolock)
  WHERE Category = 'Geography'
  ORDER BY NEWID()
) AS HistoryQuestions;


Select * from tblQuestions
Select * from tblAccount

Drop table tblAccount

Alter procedure spGetLogininfo
@Email nvarchar(35),
 @Password nvarchar(12)
As
 Begin
 Select Id,FullName,Email, Phone, Role  from tblAccount (nolock)  where (Email = @Email) AND (Password = @Password) 
 End

 declare @Email nvarchar(35), @Password nvarchar(12)
 Exec spGetLogininfo @Email = 'abc@def.com', @Password = 'abc123defA#'

 --Create a table to store user evaluation details
-- Create PROCEDURE spCreateEvaluationTable
--  @UID INT
--AS
--BEGIN
--  DECLARE @TableName NVARCHAR(128)

--  SET @TableName = N'User' + CAST(@UID AS NVARCHAR(10)) + N'EvaluationTable'

--  IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES (NOLOCK) WHERE TABLE_NAME = @TableName)
--  BEGIN
--    DECLARE @SqlStatement NVARCHAR(MAX)
    
--    SET @SqlStatement = N'
--      CREATE TABLE ' + QUOTENAME(@TableName) + N' (
--        SN INT,
--		Question nvarchar(200),
--        OptionSelected NVARCHAR(70),
--        Answer NVARCHAR(70)
--      )'
    
--    EXEC sp_executesql @SqlStatement
--  END
--END
  --------------
Create table UserEvaluationTable (UID int, QID int, Answer nvarchar(70), SubmittedAnswer nvarchar(70), IsCorrect nvarchar(10));
Alter table UserEvaluationTable 
Add Session bigint

  --Query to check options selected
  Select * from vWMultipleChoiceQuestions
   Select * from UserEvaluationTable

Alter procedure spTestResult
@selectedanswer varchar(70), @questionId Bigint, 
@answer nvarchar(70) output,
@isCorrect int output
As
Begin
Select @answer=  Answer from vWMultipleChoiceQuestions (nolock) where Question_Number=@QuestionId;

IF EXISTS(SELECT 1 FROM vWMultipleChoiceQuestions VW (NOLOCK)
		WHERE Question_Number=@QuestionId and upper(Answer)=upper(@selectedanswer))
		BEGIN
		 --Insert into UserEvaluationTable(UID, QID, Answer, SubmittedAnswer, IsCorrect)
		 --Values(@UID, @QuestionId, 
		
		Select @isCorrect = '1'
		RETURN;
		END 
		--Insert into UserEvaluationTable(UID, QID, Answer, SubmittedAnswer, IsCorrect)
		--Values(@UID, @QuestionId, 
		Select @isCorrect = '0'
		End
		
Declare @A nvarchar(70),@B nvarchar(20)
EXEC spTestResult
    @selectedanswer = 'aaaa',
    @questionId = '1',
    @answer = @A OUTPUT,
   @isCorrect= @B OUTPUT;
    Select @B

   Alter procedure spMainTestResult
@UID int,
@QuestionId Bigint, 
@Selectedanswer nvarchar(70),
@Usertoken bigint
As
Begin
 Declare @Answer varchar(70), @Iscorrect int
	Execute spTestResult @selectedanswer=@Selectedanswer, @questionId = @QuestionId,
	@answer= @Answer Output,@isCorrect= @Iscorrect output; 
		MERGE UserEvaluationTable AS target
		USING (VALUES (@UID, @QuestionId, @Answer, @Selectedanswer, @Iscorrect, @Usertoken )) AS source(UID, QId, Answer, selectedAnswer, Iscorrect, Session)
		ON target.UID = source.UID and target.QID = source.QId
		WHEN MATCHED Then
		
    UPDATE SET target.SubmittedAnswer = source.selectedAnswer, target.Iscorrect = source.Iscorrect, target.Session = source.Session
WHEN NOT MATCHED THEN
    INSERT (UID, QID, Answer, SubmittedAnswer, IsCorrect, Session) VALUES (source.UID, source.QId, source.Answer, source.selectedAnswer, source.Iscorrect, source.Session);
	End
	

   Exec spMainTestResult @UID=4, @QuestionId=5, @Selectedanswer='Antarasan and Parmasan ', @Usertoken = 45457
   Select * from UserEvaluationTable
   Delete Top(1) from UserEvaluationTable
   
  
-- Now the @answer and @isCorrect variables will hold the output values returned by the stored procedure.


                         Select * from tblAccount                                                                            
						 Select * from dbo.vWMultipleChoiceQuestions
						 
						  
						 

						 Update UserEvaluationTable
						 Delete from UserEvaluationTable where IsCorrect='CORRECT'

truncate table UserEvaluationTable

Update tblQuestions
Set Answer = RTRIM(LTRIM(Replace(Answer, Char(9), '')))
Use IQ_Mania
Select * from tblQuestions

Alter procedure spViewResult
@UID int, @session bigint
As
 Begin
 Select ue.QID, tq.Questions, ue.Answer, ue.SubmittedAnswer, ue.IsCorrect from  UserEvaluationTable (nolock) as ue
 join tblQuestions (nolock) as tq 
 on  ue.QID = tq.Question_Number
 where UID=@UID and Session=@session
End

Execute spViewResult 4,45457

UPDATE tblOptions
SET Question = tblQuestions.Question_Number
FROM tblQuestions
WHERE tblOptions.Question IS NULL
  AND tblOptions.SN = tblQuestions.Question_Number;


Exec sp_helptext spAddMCQ

Create table Questionaddedbyuser (Questions nvarchar(300), Answer nvarchar(70), Category nvarchar(15),   
 Option1 nvarchar(70), Option2 nvarchar(70), Option3 nvarchar(70), Option4 nvarchar(70))
 Alter table Questionaddedbyuser Alter column Questions nvarchar(500)
 Select * from Questionaddedbyuser

 Use IQ_Mania
Alter procedure spAddUserQuestion @Questions nvarchar(300), @Answer nvarchar(70), @Category nvarchar(15),   
 @Option1 nvarchar(70), @Option2 nvarchar(70), @Option3 nvarchar(70), @Option4 nvarchar(70),
 @flag nvarchar(15)
 As
 Begin
 Declare @response nvarchar(20)
  Begin Try
	  IF (@flag = 'User')
    BEGIN
      -- Insert options
      INSERT INTO Questionaddedbyuser Values( @Questions, @Answer, @Category, @Option1, @Option2, @Option3, @Option4)
    END
	ELSE
    BEGIN
      RAISERROR('Invalid flag value. Expected "AddminUser".', 16, 1)
    END
  END TRY
  BEGIN CATCH
    -- Handle the error here
    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
    DECLARE @ErrorSeverity INT = ERROR_SEVERITY()
    DECLARE @ErrorState INT = ERROR_STATE()

    -- You can log the error or take appropriate actions based on the error
    -- For example, you can use PRINT to output the error message
    Select 'Error: ' + @ErrorMessage

  END CATCH
END

Exec  spAddUserQuestion 'jghhj', 'hjhj', 'hjhj', 'hjhj', 'hjhj', 'hjhj','hjhj', 'dfdf'
--------------------
select * from Questionaddedbyuser
truncate table Questionaddedbyuser

Execute sp_helptext spcountrows
--
CREATE procedure spcountrows @flag nvarchar(10)  
As  
Begin  
if(@flag = 'AdminUser')  
 Begin  
 SELECT COUNT(*) AS row_count 
 FROM Questionaddedbyuser  
 End  
 End  
 Exec spcountrows @flag = 'AdminUser'

 
 