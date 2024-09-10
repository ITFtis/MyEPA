
Update Duty Set Name = '環境部' Where Name = '環保署'

Update City Set City = '環境部' Where City = '環保署'
Update [Users] Set City = '環境部' Where City = '環保署'
Update [Users] Set Duty = '環境部' Where Duty = '環保署'

--1.代碼更新
--Select * From [Town] Where CityId = 23

Update [Town] Set Name = '部長辦公室' Where CityId = 23 And Name = '署長辦公室'
Update [Town] Set Name = '環境管理署' Where CityId = 23 And Name = '環境督察總隊'
Update [Town] Set Name = '環境管理署北區環境管理中心' Where CityId = 23 And Name = '北區環境督察大隊'
Update [Town] Set Name = '環境管理署中區環境管理中心' Where CityId = 23 And Name = '中區環境督察大隊'
Update [Town] Set Name = '環境管理署南區環境管理中心' Where CityId = 23 And Name = '南區環境督察大隊'
Update [Town] Set Name = '環境保護司' Where CityId = 23 And Name = '綜合計畫處'
Update [Town] Set Name = '大氣環境司' Where CityId = 23 And Name = '空氣品質保護及噪音管制處'
Update [Town] Set Name = '資源循環署' Where CityId = 23 And Name = '廢棄物管理處'
Update [Town] Set Name = '綜合規劃司' Where CityId = 23 And Name = '管制考核及糾紛處理處'
Update [Town] Set Name = '監測資訊司' Where CityId = 23 And Name = '環境監測及資訊處'
Update [Town] Set Name = '秘書處' Where CityId = 23 And Name = '秘書室'
Update [Town] Set Name = '資源循環署' Where CityId = 23 And Name = '廢棄物管理處'
Update [Town] Set Name = '氣候變遷署' Where CityId = 23 And Name = '環境衛生及毒物管理處'
Update [Town] Set Name = '人事處' Where CityId = 23 And Name = '人事室'
Update [Town] Set Name = '政風處' Where CityId = 23 And Name = '政風室'
Update [Town] Set Name = '會計處' Where CityId = 23 And Name = '會計室'
Update [Town] Set Name = '統計處' Where CityId = 23 And Name = '統計室'
Update [Town] Set Name = '綜合規劃司' Where CityId = 23 And Name = '公害糾紛裁決委員會'
Update [Town] Set Name = '法制處' Where CityId = 23 And Name = '法規委員會'
Update [Town] Set Name = '法制處' Where CityId = 23 And Name = '訴願審議委員會'
Update [Town] Set Name = '大氣環境司' Where CityId = 23 And Name = '空氣污染防治基金管理委員會'
Update [Town] Set Name = '資源循環署回收基金管理會' Where CityId = 23 And Name = '資源回收管理基金管理委員會'
Update [Town] Set Name = '環境管理署土壤及地下水污染整治基金管理會' Where CityId = 23 And Name = '土壤及地下水污染整治基金管理委員會'
Update [Town] Set Name = '國家環境研究院' Where CityId = 23 And Name = '環境檢驗所'
Update [Town] Set Name = '國家環境研究院' Where CityId = 23 And Name = '環境保護人員訓練所'
Update [Town] Set Name = '化學物質管理署' Where CityId = 23 And Name = '化學局'
Update [Town] Set Name = '水質保護司' Where CityId = 23 And Name = '水質保護處'
Go


--2.資料更新(存中文)
--Update xxxxx Set cccccc = 'ooooo' Where cccccc = 'ooooooo'

--Select　Distinct Duty From [Users]
Update [Users] Set Duty = '三區中心' Where Duty = '北中南督察大隊'
Update [Users] Set Duty = '環管署' Where Duty = '督察總隊'
Go

--Select Distinct Town, Len(Town) From [Users]　Order By Len(Town) DESC
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

--Select Distinct Name, Len(Name)  From [Department]　Order By Len(Name) DESC
Update [Department] Set Name = '氣候變遷署' Where Name = '環境衛生及毒物管理處'
Update [Department] Set Name = '環境管理署北區環境管理中心' Where Name = '北區環境督察大隊'
Update [Department] Set Name = '環境管理署中區環境管理中心' Where Name = '中區環境督察大隊'
Update [Department] Set Name = '環境管理署南區環境管理中心' Where Name = '南區環境督察大隊'
Update [Department] Set Name = '監測資訊司' Where Name = '環境監測及資訊處'
Update [Department] Set Name = '資源循環署' Where Name = '廢棄物管理處'
Update [Department] Set Name = '環境管理署' Where Name = '環境督察總隊'
Update [Department] Set Name = '水質保護司' Where Name = '水質保護處'
Update [Department] Set Name = '秘書處' Where Name = '秘書室'

--Select Distinct Name, Len(Name) From [ContactManualDepartment] Order By Len(Name) DESC
------- ??? 名稱差異大


--Select * From [Area]
------- ???

--Select Distinct COUNTY_NAME, Len(COUNTY_NAME) From [DRINK_DETAIL] Order By Len(COUNTY_NAME) DESC
Update DRINK_DETAIL Set COUNTY_NAME = '環境管理署北區環境管理中心' Where COUNTY_NAME = '北區環境督察大隊'
Update DRINK_DETAIL Set COUNTY_NAME = '環境管理署南區環境管理中心' Where COUNTY_NAME = '南區環境督察大隊'
Update DRINK_DETAIL Set COUNTY_NAME = '環境管理署中區環境管理中心' Where COUNTY_NAME = '中區環境督察大隊'

--Select Distinct COUNTY_NAME, Len(COUNTY_NAME) From [DRINK_SECOND]　Order By Len(COUNTY_NAME) DESC
Update DRINK_SECOND Set COUNTY_NAME = '環境管理署北區環境管理中心' Where COUNTY_NAME = '北區環境督察大隊'
Update DRINK_SECOND Set COUNTY_NAME = '環境管理署南區環境管理中心' Where COUNTY_NAME = '南區環境督察大隊'
Update DRINK_SECOND Set COUNTY_NAME = '環境管理署中區環境管理中心' Where COUNTY_NAME = '中區環境督察大隊'


--Select Distinct Town, Len(Town) From [View_Users_Position] Order By Len(Town) DESC　
-------View檢視表不用改

--Select Distinct Town, Len(Town) From [View_ContactManul]　Order By Len(Town) DESC　
-------View檢視表不用改

----------------------------------------------

--留住有town(400,379,421)
--delete (401,397,391,390)
Delete [Town] Where Id In (401,397,391,390)
