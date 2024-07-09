--1.新增資料表及資料(DisinfectorType)

--2.新增資料表及資料(DisinfectantType)

--3.新增欄位(RecResource => TypeItems)
alter Table RecResource add TypeItems int
alter TABLE RecResource ALTER COLUMN Items  [nvarchar](100)

alter Table RecResourceSet add SetTypeItems int
alter TABLE RecResourceSet ALTER COLUMN SetItems  [nvarchar](100)
