--1.�s�W���(���ҲM�z CleanDay)
alter Table Damage add CleanDay [date] NULL
Go

Update Damage
Set CleanDay = ReportDay
Go

