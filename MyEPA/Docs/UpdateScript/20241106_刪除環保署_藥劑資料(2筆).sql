----刪除環保署_藥劑資料(2筆)
--Select * 
--From Disinfectant 
--Where City = '環保署'
--And Amount = 100
--And Id In (9543, 9562)

Delete Disinfectant 
Where City = '環保署'
And Amount = 100
And Id In (9543, 9562)
