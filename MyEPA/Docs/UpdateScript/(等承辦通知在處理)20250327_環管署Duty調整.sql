------���޸pDuty�վ�
----Select DutyId, Duty, 3, '���ҳ�'
----From Users 
----Where DutyId = 6


Select *
Into Users_20250327
From Users 

Update Users
Set DutyId = 3,
    Duty = '���ҳ�'
Where DutyId = 6
