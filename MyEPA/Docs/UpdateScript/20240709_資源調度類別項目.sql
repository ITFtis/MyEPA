--1.�s�W��ƪ�θ��(DisinfectorType)

--2.�s�W��ƪ�θ��(DisinfectantType)

--3.�s�W���(RecResource => TypeItems)
alter Table RecResource add TypeItems int
alter TABLE RecResource ALTER COLUMN Items  [nvarchar](100)

alter Table RecResourceSet add SetTypeItems int
alter TABLE RecResourceSet ALTER COLUMN SetItems  [nvarchar](100)
