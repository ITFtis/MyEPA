--三區中心權限_公式_script
Declare @UserId AS Nvarchar(100)

"Update [Users] Set DutyId = 5 Where UserName = 'kai.liu'
select @UserId = Id from [Users] where UserName = 'kai.liu'
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
End"
"Update [Users] Set DutyId = 5 Where UserName = 'yuteng.huang'
select @UserId = Id from [Users] where UserName = 'yuteng.huang'
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
End"
"Update [Users] Set DutyId = 5 Where UserName = 'mctsai'
select @UserId = Id from [Users] where UserName = 'mctsai'
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
End"
"Update [Users] Set DutyId = 5 Where UserName = 'menghsun.tsai'
select @UserId = Id from [Users] where UserName = 'menghsun.tsai'
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
End"
"Update [Users] Set DutyId = 5 Where UserName = 't006'
select @UserId = Id from [Users] where UserName = 't006'
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
End"
"Update [Users] Set DutyId = 5 Where UserName = 'tkcheng'
select @UserId = Id from [Users] where UserName = 'tkcheng'
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
End"
"Update [Users] Set DutyId = 5 Where UserName = 'sytsay'
select @UserId = Id from [Users] where UserName = 'sytsay'
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
End"
"Update [Users] Set DutyId = 5 Where UserName = 'pllin'
select @UserId = Id from [Users] where UserName = 'pllin'
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
End"
"Update [Users] Set DutyId = 5 Where UserName = 'mehhuang'
select @UserId = Id from [Users] where UserName = 'mehhuang'
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
End"
"Update [Users] Set DutyId = 5 Where UserName = 'cytuan'
select @UserId = Id from [Users] where UserName = 'cytuan'
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
End"
"Update [Users] Set DutyId = 5 Where UserName = 'hungyi.lu'
select @UserId = Id from [Users] where UserName = 'hungyi.lu'
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
End"
"Update [Users] Set DutyId = 5 Where UserName = 'hcli'
select @UserId = Id from [Users] where UserName = 'hcli'
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
End"
"Update [Users] Set DutyId = 5 Where UserName = 'TWDEP'
select @UserId = Id from [Users] where UserName = 'TWDEP'
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
End"
"Update [Users] Set DutyId = 5 Where UserName = 'sajulin'
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
End"

