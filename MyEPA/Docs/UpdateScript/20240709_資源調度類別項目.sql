--1.新增資料表及資料(DisinfectorType)

--2.新增資料表及資料(DisinfectantType)

--3.新增欄位(RecResource => TypeItems)
alter Table RecResource add TypeItems int
alter TABLE RecResource ALTER COLUMN Items  [nvarchar](100)

alter Table RecResourceSet add SetTypeItems int
alter TABLE RecResourceSet ALTER COLUMN SetItems  [nvarchar](100)
Go

--4.更新資料
Update RecResource
Set TypeItems = 1,
     Items = Case When Items = '1' Then 'A01'
        When Items = '2' Then 'B01'
		When Items = '3' Then 'B02'
		When Items = '4' Then 'B03'
		When Items = '5' Then 'B04'
		When Items = '6' Then 'A02'
		When Items = '7' Then 'B05'
		When Items = '8' Then 'B06'
		When Items = '9' Then 'B07'
		When Items = '10' Then 'B08'
		When Items = '11' Then 'B09'
		When Items = '12' Then 'B10'
		When Items = '13' Then 'A03'
		Else Items
	End
Where TypeItems Is Null

Update RecResourceSet
Set SetTypeItems = 1,
    SetItems = Case When SetItems = '1' Then 'A01'
        When SetItems = '2' Then 'B01'
		When SetItems = '3' Then 'B02'
		When SetItems = '4' Then 'B03'
		When SetItems = '5' Then 'B04'
		When SetItems = '6' Then 'A02'
		When SetItems = '7' Then 'B05'
		When SetItems = '8' Then 'B06'
		When SetItems = '9' Then 'B07'
		When SetItems = '10' Then 'B08'
		When SetItems = '11' Then 'B09'
		When SetItems = '12' Then 'B10'
		When SetItems = '13' Then 'A03'
		Else SetItems
	End
Where SetTypeItems Is Null
Go