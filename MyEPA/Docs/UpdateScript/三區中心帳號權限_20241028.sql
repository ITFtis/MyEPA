--三區中心帳號權限
Declare @UserId AS Nvarchar(100)
Update [Users] Set DutyId = 5 Where UserName = 'chchang'
select @UserId = Id from [Users] where UserName = 'chchang'
IF Not EXISTS ( select * from UserArea where UserId = @UserId ) 
Begin	
	Insert Into UserArea(UserId, AreaId)
	Select @UserId, 1
End
Else
Begin
	Update UserArea
	Set AreaId = 1
	Where UserId = @UserId
End
Update [Users] Set DutyId = 5 Where UserName = 'chmuchang'
select @UserId = Id from [Users] where UserName = 'chmuchang'
IF Not EXISTS ( select * from UserArea where UserId = @UserId ) 
Begin	
	Insert Into UserArea(UserId, AreaId)
	Select @UserId, 1
End
Else
Begin
	Update UserArea
	Set AreaId = 1
	Where UserId = @UserId
End
Update [Users] Set DutyId = 5 Where UserName = 'hwchang'
select @UserId = Id from [Users] where UserName = 'hwchang'
IF Not EXISTS ( select * from UserArea where UserId = @UserId ) 
Begin	
	Insert Into UserArea(UserId, AreaId)
	Select @UserId, 1
End
Else
Begin
	Update UserArea
	Set AreaId = 1
	Where UserId = @UserId
End
Update [Users] Set DutyId = 5 Where UserName = 'jkchou'
select @UserId = Id from [Users] where UserName = 'jkchou'
IF Not EXISTS ( select * from UserArea where UserId = @UserId ) 
Begin	
	Insert Into UserArea(UserId, AreaId)
	Select @UserId, 1
End
Else
Begin
	Update UserArea
	Set AreaId = 1
	Where UserId = @UserId
End
Update [Users] Set DutyId = 5 Where UserName = 'lmling'
select @UserId = Id from [Users] where UserName = 'lmling'
IF Not EXISTS ( select * from UserArea where UserId = @UserId ) 
Begin	
	Insert Into UserArea(UserId, AreaId)
	Select @UserId, 3
End
Else
Begin
	Update UserArea
	Set AreaId = 1
	Where UserId = @UserId
End
Update [Users] Set DutyId = 5 Where UserName = 'sajulin'
select @UserId = Id from [Users] where UserName = 'sajulin'
IF Not EXISTS ( select * from UserArea where UserId = @UserId ) 
Begin	
	Insert Into UserArea(UserId, AreaId)
	Select @UserId, 3
End
Else
Begin
	Update UserArea
	Set AreaId = 1
	Where UserId = @UserId
End
Update [Users] Set DutyId = 5 Where UserName = 'weihung.hsu'
select @UserId = Id from [Users] where UserName = 'weihung.hsu'
IF Not EXISTS ( select * from UserArea where UserId = @UserId ) 
Begin	
	Insert Into UserArea(UserId, AreaId)
	Select @UserId, 1
End
Else
Begin
	Update UserArea
	Set AreaId = 1
	Where UserId = @UserId
End
Update [Users] Set DutyId = 5 Where UserName = 'wenchuan.kuo'
select @UserId = Id from [Users] where UserName = 'wenchuan.kuo'
IF Not EXISTS ( select * from UserArea where UserId = @UserId ) 
Begin	
	Insert Into UserArea(UserId, AreaId)
	Select @UserId, 1
End
Else
Begin
	Update UserArea
	Set AreaId = 1
	Where UserId = @UserId
End
Update [Users] Set DutyId = 5 Where UserName = 'Z092'
select @UserId = Id from [Users] where UserName = 'Z092'
IF Not EXISTS ( select * from UserArea where UserId = @UserId ) 
Begin	
	Insert Into UserArea(UserId, AreaId)
	Select @UserId, 1
End
Else
Begin
	Update UserArea
	Set AreaId = 1
	Where UserId = @UserId
End
Update [Users] Set DutyId = 5 Where UserName = 'Z118'
select @UserId = Id from [Users] where UserName = 'Z118'
IF Not EXISTS ( select * from UserArea where UserId = @UserId ) 
Begin	
	Insert Into UserArea(UserId, AreaId)
	Select @UserId, 1
End
Else
Begin
	Update UserArea
	Set AreaId = 1
	Where UserId = @UserId
End
Update [Users] Set DutyId = 5 Where UserName = 'Z415'
select @UserId = Id from [Users] where UserName = 'Z415'
IF Not EXISTS ( select * from UserArea where UserId = @UserId ) 
Begin	
	Insert Into UserArea(UserId, AreaId)
	Select @UserId, 1
End
Else
Begin
	Update UserArea
	Set AreaId = 1
	Where UserId = @UserId
End
Update [Users] Set DutyId = 5 Where UserName = 'chiawei.lin'
select @UserId = Id from [Users] where UserName = 'chiawei.lin'
IF Not EXISTS ( select * from UserArea where UserId = @UserId ) 
Begin	
	Insert Into UserArea(UserId, AreaId)
	Select @UserId, 1
End
Else
Begin
	Update UserArea
	Set AreaId = 1
	Where UserId = @UserId
End
Update [Users] Set DutyId = 5 Where UserName = 'chiahsiang.lee'
select @UserId = Id from [Users] where UserName = 'chiahsiang.lee'
IF Not EXISTS ( select * from UserArea where UserId = @UserId ) 
Begin	
	Insert Into UserArea(UserId, AreaId)
	Select @UserId, 1
End
Else
Begin
	Update UserArea
	Set AreaId = 1
	Where UserId = @UserId
End
Update [Users] Set DutyId = 5 Where UserName = 'ycchang'
select @UserId = Id from [Users] where UserName = 'ycchang'
IF Not EXISTS ( select * from UserArea where UserId = @UserId ) 
Begin	
	Insert Into UserArea(UserId, AreaId)
	Select @UserId, 1
End
Else
Begin
	Update UserArea
	Set AreaId = 1
	Where UserId = @UserId
End
Update [Users] Set DutyId = 5 Where UserName = 'jkliang'
select @UserId = Id from [Users] where UserName = 'jkliang'
IF Not EXISTS ( select * from UserArea where UserId = @UserId ) 
Begin	
	Insert Into UserArea(UserId, AreaId)
	Select @UserId, 1
End
Else
Begin
	Update UserArea
	Set AreaId = 1
	Where UserId = @UserId
End
Update [Users] Set DutyId = 5 Where UserName = 'hocchang'
select @UserId = Id from [Users] where UserName = 'hocchang'
IF Not EXISTS ( select * from UserArea where UserId = @UserId ) 
Begin	
	Insert Into UserArea(UserId, AreaId)
	Select @UserId, 1
End
Else
Begin
	Update UserArea
	Set AreaId = 1
	Where UserId = @UserId
End
Update [Users] Set DutyId = 5 Where UserName = 'chihming.lin'
select @UserId = Id from [Users] where UserName = 'chihming.lin'
IF Not EXISTS ( select * from UserArea where UserId = @UserId ) 
Begin	
	Insert Into UserArea(UserId, AreaId)
	Select @UserId, 1
End
Else
Begin
	Update UserArea
	Set AreaId = 1
	Where UserId = @UserId
End
Update [Users] Set DutyId = 5 Where UserName = 'hchchen'
select @UserId = Id from [Users] where UserName = 'hchchen'
IF Not EXISTS ( select * from UserArea where UserId = @UserId ) 
Begin	
	Insert Into UserArea(UserId, AreaId)
	Select @UserId, 1
End
Else
Begin
	Update UserArea
	Set AreaId = 1
	Where UserId = @UserId
End
Update [Users] Set DutyId = 5 Where UserName = 'fangju'
select @UserId = Id from [Users] where UserName = 'fangju'
IF Not EXISTS ( select * from UserArea where UserId = @UserId ) 
Begin	
	Insert Into UserArea(UserId, AreaId)
	Select @UserId, 1
End
Else
Begin
	Update UserArea
	Set AreaId = 1
	Where UserId = @UserId
End

