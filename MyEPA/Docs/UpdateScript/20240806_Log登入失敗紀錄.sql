--Log�n�J���Ѭ���
--�C�ӵn�J������ �]�A ����ɶ� �b����J���K�X�H��IP

--3�����(Type, PwdKeyin, SourceIP)
--Type�G1���`�n�J,2�n�J����
--IsOver�G�O�_�W�L�n�J���Ħ���
alter Table UserLoginLog add [Type] [int] NULL
alter Table UserLoginLog add [PwdKeyIn] [nvarchar](50) NULL
alter Table UserLoginLog add [SourceIP] [varchar](100) NULL
alter Table UserLoginLog add [IsOver] [bit] NULL
Go

Update UserLoginLog
Set Type = 1
Go