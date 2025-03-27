------環管署Duty調整
----Select DutyId, Duty, 3, '環境部'
----From Users 
----Where DutyId = 6


Select *
Into Users_20250327
From Users 

Update Users
Set DutyId = 3,
    Duty = '環境部'
Where DutyId = 6
