--水質通報細項設定

--Null設定
--[O_Turbidity]　　　　原水濁度	
--[Chlorine]　　　　　　自由有效餘氯　檢驗值
--[EColi]　　　　　　　　大腸桿菌群　檢驗值
--[Hydrogen]　　　　　　氫離子濃度指數　檢驗值
--[Turbidity]　　　　　　濁度　檢驗值

--WaterCheckDetail
alter TABLE WaterCheckDetail ALTER COLUMN [O_Turbidity] [decimal](18, 3) NULL
alter TABLE WaterCheckDetail ALTER COLUMN [Chlorine] [decimal](18, 3) NULL
alter TABLE WaterCheckDetail ALTER COLUMN [EColi] [decimal](18, 3) NULL
alter TABLE WaterCheckDetail ALTER COLUMN [Hydrogen] [decimal](18, 3) NULL
alter TABLE WaterCheckDetail ALTER COLUMN [Turbidity] [decimal](18, 3) NULL


