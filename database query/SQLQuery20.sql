Exec sp_helptext spAddUserQuestion
Alter procedure spAddUserQuestion @Questions nvarchar(300), @Answer nvarchar(70), @Category nvarchar(15),     
 @Option1 nvarchar(70), @Option2 nvarchar(70), @Option3 nvarchar(70), @Option4 nvarchar(70),  
 @flag nvarchar(15)  
 As  
 Begin  
 Declare @responsedescription nvarchar(60)  
 Declare @responsecode int  
  Begin Try  
   IF (@flag = 'User')  
    BEGIN   
      INSERT INTO Questionaddedbyuser Values( @Questions, @Answer, @Category, @Option1, @Option2, @Option3, @Option4)  
   Set @responsecode=200; Set @responsedescription = 'Successfully added Questions'  
    END  
 ELSE  
    BEGIN  
      
      Set @responsecode=401; Set @responsedescription = 'Unauthorized Access'  
    END  
  END TRY  
  BEGIN CATCH  
    Set @responsecode=400; Set @responsedescription = 'Bad Request'  
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
				
				Set @responsecode= 200; Set @responsedescription = 'Search Completed';
				Select @responsecode as ResponseCode, @responsedescription as ResponseDescription;
				SELECT *
				FROM tblQuestions (nolock)
				WHERE LOWER(Questions) LIKE LOWER('%' + @inputtext + '%')
				   OR LOWER(Answer) LIKE LOWER('%' + @inputtext + '%');
			End
		Else
		 Begin
			Set @responsecode= 404; Set @responsedescription = 'Search Text Not Found';
			Select @responsecode as ResponseCode, @responsedescription as ResponseDescription
		 End
	End
End

Use IQ_Mania
Exec spSearchquestiontext '%Lion%', 'Search'

Select * from Questionaddedbyuser
Select * from tblQuestions
Select Replace(data,' ', '') from (Select 'The Man who sold the Earth' data)x 
Select * from tblOptions



Declare @qstn1 nvarchar(300)= 'Match the following:  a) Gatthigal - 1) Chitwan  b) The Lake where Cock roars - 2) Myagdi    c) Rupse jharana - 3) Myagdi  d) Bishajari Taal - 4) Baitadi'
Declare @qstn2 nvarchar(300) = '<p>Match the following:</p><p> a) Gatthigal &nbsp &nbsp &nbsp 1) Chitwan </p> <p> b) The Lake where Cock roars &nbsp &nbsp 2) Myagdi </p> <p>  c) Rupse jharana &nbsp &nbsp 3) Myagdi </p> <p> d) Bishajari Taal &nbsp &nbsp 4) Baitadi </p>'
UPDATE tblQuestions
SET Questions = REPLACE(Questions,
Questions, @qstn2)
Select * from tblQuestions WHERE Questions like '%'+@qstn1+'%'
 Select * from tblAccount

 ----------------------------------------------------------------------
 Create procedure spGetUserAddedQuestions
 @flag nvarchar(10)
 as
 Begin
	Declare @responsecode int; Declare @responsedescription nvarchar(35)
	 If(@flag = 'AdminUser')
		Begin
		Set @responsecode = 200;
			If Exists(Select * from Questionaddedbyuser)
			 Begin
				Set @responsedescription = 'Questionaddedbyuser table is empty';
				Select @responsecode as ResponseCode, @responsedescription as ResponseDescription;
				Select * from Questionaddedbyuser
			End
			Else
				Begin
				 Set @responsedescription = 'Questionaddedbyuser table is empty';
				 Select @responsecode as ResponseCode, @responsedescription as ResponseDescription;
			End
		End
	Else
    	Begin
    	Set @responsecode = 401;Set @responsedescription = 'Access denied';
		 Select @responsecode as ResponseCode, @responsedescription as ResponseDescription; 
    End
End
Exec sp_helptext spGetUserAddedQuestions 'Adminuser'
