
Update Duty Set Name = '���ҳ�' Where Name = '���O�p'

Update City Set City = '���ҳ�' Where City = '���O�p'
Update [Users] Set City = '���ҳ�' Where City = '���O�p'
Update [Users] Set Duty = '���ҳ�' Where Duty = '���O�p'

--1.�N�X��s
--Select * From [Town] Where CityId = 23

Update [Town] Set Name = '�����줽��' Where CityId = 23 And Name = '�p���줽��'
Update [Town] Set Name = '���Һ޲z�p' Where CityId = 23 And Name = '���ҷ����`��'
Update [Town] Set Name = '���Һ޲z�p�_�����Һ޲z����' Where CityId = 23 And Name = '�_�����ҷ���j��'
Update [Town] Set Name = '���Һ޲z�p�������Һ޲z����' Where CityId = 23 And Name = '�������ҷ���j��'
Update [Town] Set Name = '���Һ޲z�p�n�����Һ޲z����' Where CityId = 23 And Name = '�n�����ҷ���j��'
Update [Town] Set Name = '���ҫO�@�q' Where CityId = 23 And Name = '��X�p�e�B'
Update [Town] Set Name = '�j�����ҥq' Where CityId = 23 And Name = '�Ů�~��O�@�ξ����ި�B'
Update [Town] Set Name = '�귽�`���p' Where CityId = 23 And Name = '�o�󪫺޲z�B'
Update [Town] Set Name = '��X�W���q' Where CityId = 23 And Name = '�ި�Ү֤ΪȯɳB�z�B'
Update [Town] Set Name = '�ʴ���T�q' Where CityId = 23 And Name = '���Һʴ��θ�T�B'
Update [Town] Set Name = '���ѳB' Where CityId = 23 And Name = '���ѫ�'
Update [Town] Set Name = '�귽�`���p' Where CityId = 23 And Name = '�o�󪫺޲z�B'
Update [Town] Set Name = '����ܾE�p' Where CityId = 23 And Name = '���ҽåͤάr���޲z�B'
Update [Town] Set Name = '�H�ƳB' Where CityId = 23 And Name = '�H�ƫ�'
Update [Town] Set Name = '�F���B' Where CityId = 23 And Name = '�F����'
Update [Town] Set Name = '�|�p�B' Where CityId = 23 And Name = '�|�p��'
Update [Town] Set Name = '�έp�B' Where CityId = 23 And Name = '�έp��'
Update [Town] Set Name = '��X�W���q' Where CityId = 23 And Name = '���`�ȯɵ��M�e���|'
Update [Town] Set Name = '�k��B' Where CityId = 23 And Name = '�k�W�e���|'
Update [Town] Set Name = '�k��B' Where CityId = 23 And Name = '�D�@�fĳ�e���|'
Update [Town] Set Name = '�j�����ҥq' Where CityId = 23 And Name = '�Ů�ìV���v����޲z�e���|'
Update [Town] Set Name = '�귽�`���p�^������޲z�|' Where CityId = 23 And Name = '�귽�^���޲z����޲z�e���|'
Update [Town] Set Name = '���Һ޲z�p�g�[�Φa�U���ìV��v����޲z�|' Where CityId = 23 And Name = '�g�[�Φa�U���ìV��v����޲z�e���|'
Update [Town] Set Name = '��a���Ҭ�s�|' Where CityId = 23 And Name = '���������'
Update [Town] Set Name = '��a���Ҭ�s�|' Where CityId = 23 And Name = '���ҫO�@�H���V�m��'
Update [Town] Set Name = '�ƾǪ���޲z�p' Where CityId = 23 And Name = '�ƾǧ�'
Update [Town] Set Name = '����O�@�q' Where CityId = 23 And Name = '����O�@�B'
Go


--2.��Ƨ�s(�s����)
--Update xxxxx Set cccccc = 'ooooo' Where cccccc = 'ooooooo'

--Select�@Distinct Duty From [Users]
Update [Users] Set Duty = '�T�Ϥ���' Where Duty = '�_���n����j��'
Update [Users] Set Duty = '���޸p' Where Duty = '�����`��'
Go

--Select Distinct Town, Len(Town) From [Users]�@Order By Len(Town) DESC
Update [Users]
Set Town = b.Name
From [Users] a 
Inner Join 
(
	SELECT Id, Name 
	FROM [Town]
	WHERE  CityId IN (23)
)b On a.TownId = b.Id
Go

--Select Distinct Name, Len(Name)  From [Department]�@Order By Len(Name) DESC
Update [Department] Set Name = '����ܾE�p' Where Name = '���ҽåͤάr���޲z�B'
Update [Department] Set Name = '���Һ޲z�p�_�����Һ޲z����' Where Name = '�_�����ҷ���j��'
Update [Department] Set Name = '���Һ޲z�p�������Һ޲z����' Where Name = '�������ҷ���j��'
Update [Department] Set Name = '���Һ޲z�p�n�����Һ޲z����' Where Name = '�n�����ҷ���j��'
Update [Department] Set Name = '�ʴ���T�q' Where Name = '���Һʴ��θ�T�B'
Update [Department] Set Name = '�귽�`���p' Where Name = '�o�󪫺޲z�B'
Update [Department] Set Name = '���Һ޲z�p' Where Name = '���ҷ����`��'
Update [Department] Set Name = '����O�@�q' Where Name = '����O�@�B'
Update [Department] Set Name = '���ѳB' Where Name = '���ѫ�'

--Select Distinct Name, Len(Name) From [ContactManualDepartment] Order By Len(Name) DESC
------- ??? �W�ٮt���j


--Select * From [Area]
------- ???

--Select Distinct COUNTY_NAME, Len(COUNTY_NAME) From [DRINK_DETAIL] Order By Len(COUNTY_NAME) DESC
Update DRINK_DETAIL Set COUNTY_NAME = '���Һ޲z�p�_�����Һ޲z����' Where COUNTY_NAME = '�_�����ҷ���j��'
Update DRINK_DETAIL Set COUNTY_NAME = '���Һ޲z�p�n�����Һ޲z����' Where COUNTY_NAME = '�n�����ҷ���j��'
Update DRINK_DETAIL Set COUNTY_NAME = '���Һ޲z�p�������Һ޲z����' Where COUNTY_NAME = '�������ҷ���j��'

--Select Distinct COUNTY_NAME, Len(COUNTY_NAME) From [DRINK_SECOND]�@Order By Len(COUNTY_NAME) DESC
Update DRINK_SECOND Set COUNTY_NAME = '���Һ޲z�p�_�����Һ޲z����' Where COUNTY_NAME = '�_�����ҷ���j��'
Update DRINK_SECOND Set COUNTY_NAME = '���Һ޲z�p�n�����Һ޲z����' Where COUNTY_NAME = '�n�����ҷ���j��'
Update DRINK_SECOND Set COUNTY_NAME = '���Һ޲z�p�������Һ޲z����' Where COUNTY_NAME = '�������ҷ���j��'


--Select Distinct Town, Len(Town) From [View_Users_Position] Order By Len(Town) DESC�@
-------View�˵����Χ�

--Select Distinct Town, Len(Town) From [View_ContactManul]�@Order By Len(Town) DESC�@
-------View�˵����Χ�

----------------------------------------------

--�d��town(400,379,421)
--delete (401,397,391,390)
Delete [Town] Where Id In (401,397,391,390)
