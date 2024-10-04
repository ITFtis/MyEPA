--集合時間
alter Table RecResource add GatherDate [datetime] NULL
--集合地點
alter Table RecResource add GatherPlace [nvarchar](100) NULL
--報到人    
alter Table RecResource add  CheckPerson [nvarchar](50) NULL
--報到人電話  
alter Table RecResource add  CheckMobilePhone [nvarchar](50) NULL
--報到指揮官  
alter Table RecResource add COPerson [nvarchar](50) NULL
--指揮官電話  
alter Table RecResource add COMobilePhone [nvarchar](50) NULL

