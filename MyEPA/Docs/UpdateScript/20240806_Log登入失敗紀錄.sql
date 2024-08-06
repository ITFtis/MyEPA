--Log登入失敗紀錄
--每個登入的紀錄 包括 日期時間 帳號輸入的密碼以及IP

--3個欄位(Type, PwdKeyin, SourceIP)
--Type：1正常登入,2登入失敗
--IsOver：是否納入登入失敗次數(0否1是)
alter Table UserLoginLog add [Type] [int] NULL
alter Table UserLoginLog add [PwdKeyIn] [nvarchar](50) NULL
alter Table UserLoginLog add [SourceIP] [varchar](100) NULL
alter Table UserLoginLog add [IsOver] [bit] NULL
Go

Update UserLoginLog
Set Type = 0
Go