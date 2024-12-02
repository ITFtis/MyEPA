--調整消毒設備藥劑空值資料
--Select IsSupportCity, SupportCityNum From Disinfector Where IsSupportCity Is Null
--Select IsSupportCity, SupportCityNum From Disinfectant  Where IsSupportCity Is Null

Update Disinfector
Set IsSupportCity = 1
Where IsSupportCity Is Null

Update Disinfectant
Set IsSupportCity = 1
Where IsSupportCity Is Null

