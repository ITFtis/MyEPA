--����q���Ӷ��]�w

--Null�]�w
--[O_Turbidity]�@�@�@�@����B��	
--[Chlorine]�@�@�@�@�@�@�ۥѦ��ľl��@�����
--[EColi]�@�@�@�@�@�@�@�@�j�z��߸s�@�����
--[Hydrogen]�@�@�@�@�@�@�B���l�@�׫��ơ@�����
--[Turbidity]�@�@�@�@�@�@�B�ס@�����

--WaterCheckDetail
alter TABLE WaterCheckDetail ALTER COLUMN [O_Turbidity] [decimal](18, 3) NULL
alter TABLE WaterCheckDetail ALTER COLUMN [Chlorine] [decimal](18, 3) NULL
alter TABLE WaterCheckDetail ALTER COLUMN [EColi] [decimal](18, 3) NULL
alter TABLE WaterCheckDetail ALTER COLUMN [Hydrogen] [decimal](18, 3) NULL
alter TABLE WaterCheckDetail ALTER COLUMN [Turbidity] [decimal](18, 3) NULL


