
Alter procedure spEditprofile
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

Exec spEditprofile  'changepass','ABCD','abc@def.com','123@Bcd','7575675'
	
	Select * from tblAccount
-------------------------------------------------------------------------------------------

