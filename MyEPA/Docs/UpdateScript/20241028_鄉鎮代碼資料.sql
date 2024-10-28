----註冊：鄉鎮代碼資料

Select * From City Where City = '臺中市'

Select Id, CityId, Name, IsTown From Town Where Name = '板橋區'


---------------------------------------------------
--Select b.City, a.Name
--From Town　a
--Left Join City b On a.CityId = b.Id
--Order By b.Sort, a.Id

--Select * From City Where City = '臺中市'

--Select Id, CityId, Name, IsTown From Town Where Name = '板橋區'
