--Select Town, '繷カ' From [Users] Where Town = '繷马'
--Select Name, '繷カ' From Town Where Name = '繷马'

--1.眀腹
Update [Users]
Set Town = '繷カ' 
Where Town = '繷马'

--2.秏马絏
Update Town
Set Name = '繷カ' 
Where Name = '繷马'
