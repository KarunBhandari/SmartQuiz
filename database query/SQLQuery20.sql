Exec sp_helptext spAddUserQuestion
Create procedure spAddUserQuestion @Questions nvarchar(300), @Answer nvarchar(70), @Category nvarchar(15),     
 @Option1 nvarchar(70), @Option2 nvarchar(70), @Option3 nvarchar(70), @Option4 nvarchar(70),  
 @flag nvarchar(15)  
 As  
 Begin  
 Declare @responsedescription nvarchar(60)  
 Declare @responsecode int  
  Begin Try  
   IF (@flag = 'User')  
    BEGIN  
      -- Insert options  
      INSERT INTO Questionaddedbyuser Values( @Questions, @Answer, @Category, @Option1, @Option2, @Option3, @Option4)  
   Set @responsecode=200; Set @responsedescription = 'Successfully added Questions'  
    END  
 ELSE  
    BEGIN  
      -- Handle the case when @flag is not 'AddminUser'  
      -- You can add code here to handle this scenario, or simply raise an error  
      Set @responsecode=401; Set @responsedescription = 'Unauthorized Access'  
    END  
  END TRY  
  BEGIN CATCH  
    -- Handle the error here  
    Set @responsecode=400; Set @responsedescription = 'Bad Request'  
  
    -- You can log the error or take appropriate actions based on the error  
    -- For example, you can use PRINT to output the error message  
  
    -- You can re-raise the error to the client if needed  
    -- RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)  
  END CATCH  
  Select @responsedescription as ResponseDescription, @responsecode as ResponseCode  
END

----------------------------------------------------------------------------------------

Exec sp_helptext spEditprofile
CREATE procedure spEditprofile  
@flag nvarchar(20), @Fullname nvarchar(55), @Email nvarchar(35),  
 @Password nvarchar(12), @Phone nvarchar(15)  
 As  
 Begin   
 Declare @responsecode int, @responsedescription nvarchar(50)  
 If(@flag= 'changepass')  
  Begin  
  If((Select COUNT(*) AS row_count   
   FROM tblAccount where FullName=@Fullname and Email = @Email and Phone=@Phone)=1)  
  Begin  
   Update tblAccount  
   Set Password=@Password where FullName=@Fullname and Email = @Email and Phone=@Phone  
   Set @responsecode=200 Set @responsedescription='Successfully updated password'  
  End  
  Else  
   Begin  
    Set @responsecode  = 401;  
    Set @responsedescription  ='User with provided information do not exists'  
   End  
  End  
 else  
 Begin  
  Set @responsecode  = 400;  
  Set @responsedescription ='Invalid Transaction'  
 End  
 Select @responsecode as ResponseCode, @responsedescription as ResponseDescription  
End

---------------------------------------------------------------------------------------

Exec sp_helptext spGetLogininfo
Alter procedure spGetLogininfo  
@Email nvarchar(35),  
 @Password nvarchar(12),  
 @flag nvarchar(15)  
As  
 Begin  
   Declare @responsecode int; Declare @responsedescription nvarchar(30)  
   If(@flag='AuthLogin')  
   Begin  
 If Exists(Select 1 from tblAccount where (Email=@Email) AND (Password = @Password))   
  Begin  
  Select Id,FullName,Email, Phone, Role  from tblAccount (nolock)  where (Email = @Email) AND (Password = @Password)  
   Set @responsecode=302; Set @responsedescription='Login Successful'  
  
  End  
 Else  
  Begin  
  Set @responsecode=404; Set @responsedescription='User Not Found'  
   End  
 End  
 Select @responsecode as ResponseCode, @responsedescription as ResponseDescription  
End

---------------------------------

Alter procedure spSearchquestiontext
@inputtext nvarchar(200), @flag nvarchar(15)
As
Begin
	Declare @responsecode int; Declare @responsedescription nvarchar(30)
	If(@flag = 'Search')
		Begin
		If Exists(SELECT *
				  FROM tblQuestions (nolock)
				  WHERE LOWER(Questions) LIKE LOWER('%' + @inputtext + '%')
				  OR LOWER(Answer) LIKE LOWER('%' + @inputtext + '%'))
			Begin
				SELECT *
				FROM tblQuestions (nolock)
				WHERE LOWER(Questions) LIKE LOWER('%' + @inputtext + '%')
				   OR LOWER(Answer) LIKE LOWER('%' + @inputtext + '%');
				Set @responsecode= 200; Set @responsedescription = 'Search Completed'
			End
		Else
		 Begin
			Set @responsecode= 404; Set @responsedescription = 'Search Text Not Found'
		 End
	End
	Select @responsecode as ResponseCode, @responsedescription as ResponseDescription
End


Exec spSearchquestiontext 'Nepal', 'Search'
