# EPAemis

[測試帳密](# 測試帳密)

## 2021 更新日誌

###  1216

- 將環管處調整為環境督察總隊

### 1211

- 環保局長通聯名冊權限調整

```sql
UPDATE [epaemis_local_PROD].[dbo].[ContactManualDepartment]
SET TYPE = 4
WHERE Name Like '%市' OR Name Like '%縣'
```

### 1115

#### 當月累積工時 3.5h

- 搜尋列表下拉選單功能調整

### 1114

#### 當月累積工時 3h

- 聯絡人資料維護-修改(3h)

### 1104

- 修正 責任區域劃分表，新增人員沒有單位
- 修正責任區域劃分表抓錯 Type 的問題
- 修正監資處檔案上傳問題

### 1101

- 新增 Sort 欄位
- 修正維護人員單位查詢

```sql
ALTER TABLE [ContactManual] ADD Sort INT default(0) NOT NULL	
```



### 1028

#### 當月累積工時 12.5h

- 修正 水質抽驗結果通報(1h)

### 1024

#### 當月累積工時 11.5h

- 聯絡人資料維護-列表 (5h)
  - 搜尋功能
  - 刪除

- 新增 City Type 欄位

```sql
ALTER TABLE [City] ADD Type INT default(0) NOT NULL
UPDATE [epaemis_local_PROD].[dbo].[City]
SET Type = 1
WHERE ID IN (23)
UPDATE [epaemis_local_PROD].[dbo].[City]
SET Type = 2
WHERE ID > 23
```

### 1021

#### 當月累積工時 6.5h

- 點擊刪除至頂 (0.25h)
- 調整 水質抽驗結果通報 地點(0.25h)

### 1016

- Users 新增 ContactManualDepartmentId

```sql
ALTER TABLE [Users] ADD ContactManualDepartmentId INT NULL
```



- 新增手冊下載紀錄

```sql
CREATE TABLE [dbo].[ContactManualDownloadRecord](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[MobilePhone] [nvarchar](50) NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ReportDownloadRecord] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]	
```

### 1012

- 使用者資料維護 新增 角色

- Users 新增 ContactManualDuty 欄位(電子手冊角色)

```sql
ALTER TABLE [Users] ADD ContactManualDuty INT NOT NULL Default(0)
```



### 1007

#### 當月累積工時 6h

- 整備通報失敗修正 0.5h
- 「聯絡人資料查詢」苗栗縣新增三灣 0.2h
- 災情通報確認後跳回原本的路徑 0.3h

### 1003

#### 當月累積工時 5h

- 水質抽驗日期須至主題結束後七日 0.5h
- 列表合格件數應只顯示自己單位的 1.5h
- 填報了無災情，下載的PDF顯示無災情 1.5h
- 有建置通報案例時，不可點選本日無災情；無災情時不可建置通報案例。 1.5h

### 0925

- 化學局24小時勤情單位

```sql
INSERT INTO [dbo].[ContactManualDepartment]([Name],[Type],[CreateDate],[CreateUser],[UpdateDate],[UpdateUser])
VALUES('北區環境事故專業技術小組',3,GETDATE(),'Tony',GETDATE(),'Tony')
INSERT INTO [dbo].[ContactManualDepartment]([Name],[Type],[CreateDate],[CreateUser],[UpdateDate],[UpdateUser])
VALUES('中區環境事故專業技術小組',3,GETDATE(),'Tony',GETDATE(),'Tony')
INSERT INTO [dbo].[ContactManualDepartment]([Name],[Type],[CreateDate],[CreateUser],[UpdateDate],[UpdateUser])
VALUES('南區環境事故專業技術小組',3,GETDATE(),'Tony',GETDATE(),'Tony')
INSERT INTO [dbo].[ContactManualDepartment]([Name],[Type],[CreateDate],[CreateUser],[UpdateDate],[UpdateUser])
VALUES('環境事故監控中心',3,GETDATE(),'Tony',GETDATE(),'Tony')
INSERT INTO [dbo].[ContactManualDepartment]([Name],[Type],[CreateDate],[CreateUser],[UpdateDate],[UpdateUser])
VALUES('環境事故諮詢中心',3,GETDATE(),'Tony',GETDATE(),'Tony')
```

### 0921

- 值班日期

```sql
CREATE TABLE [dbo].[ContactManualDate](
	[Id] [int] NOT NULL,
	[Date] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ContactManualDate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
```

### 0917

督導

```sql
CREATE TABLE [dbo].[ContactManualSupervise](
	[Id] [int] NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[Describe] [nvarchar](max) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[UpdateUser] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ContactManualSupervise] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
```



### 0915

手冊角色

```sql
CREATE TABLE [dbo].[ContactManualRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Type] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[UpdateUser] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ContactManualRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
```

### 0914

手冊部門單位

```sql
CREATE TABLE [dbo].[ContactManualDepartment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Type] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[UpdateUser] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ContactManualDepartment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [dbo].[ContactManualDepartment] ADD  CONSTRAINT [DF_ContactManualDepartment_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
ALTER TABLE [dbo].[ContactManualDepartment] ADD  CONSTRAINT [DF_ContactManualDepartment_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
```



### 0912

當月累積工時 10h

- 災情及環境清理統計 2.5h
  - 修正首頁「災情及環境清理統計」連結
  - 「災情及環境清理統計」是否結案
  - 移除自來水公司四筆資料
  - 「飲用水抽驗結果」連結
- 主要聯絡人修改問題修正 (2h)
- 公告連結改為 超連結(0.5)
### 0907

- [開發]手冊資料

```sql
CREATE TABLE [dbo].[ContactManual](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NOT NULL,
	[SourceId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Note] [nvarchar](200) NULL,
	[RoleId] [int] NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar](50) NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[UpdateUser] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ContactManual] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
```

### 0905

當月累積工時 5h

- 修正環保局 查詢聯絡人資料 0.5h
- 新增 總隊管理資訊/年度統計報表 2h
- 新增 總隊管理資訊/請求支援通報確認 2.5h
- [開發]單位資料維護

### 0904

- [開發]基本資料維護

### 0826

#### 當月累積工時 14h

- 修正消毒設備表中若點選「縣市」即會出現錯誤畫面(0.25h)
- 消毒設備填報欄位亦麻煩移除「消毒車」(0.25h)

### 0820

#### 當月累積工時 13.5h

- 同一單位負責人不得設定一個以上(1h)

- 新增屏東區管理處 (0.25h)
- 消毒設備報表 - 移除「消毒車」 (0.25h)
- 「線上輪班表」 移除 (0.25h)
- 「台北自來水事業處」 調整 (0.25h)

```sql
INSERT INTO [dbo].[WaterDivision]
([Name]) VALUES  ('屏東區管理處')
UPDATE City
SET WaterDivisionId = @@IDENTITY 
WHERE City  = '屏東縣'
```

### 0815

#### 當月累積工時 11.5h

- 災情通報 多檔上傳 顯示 修改 刪除(2h)
- 修正 災情通報 修改畫面顯示

### 0812

#### 當月累積工時 9.5h

- 調整災情通報可填入小數至第三位(0.5h)

```sql
ALTER TABLE Damage ALTER COLUMN DamageArea DECIMAL(18, 3);
ALTER TABLE Damage ALTER COLUMN FloodArea DECIMAL(18, 3);
ALTER TABLE Damage ALTER COLUMN DisinfectArea DECIMAL(18, 3);
ALTER TABLE Damage ALTER COLUMN PR_Garbage DECIMAL(18, 3);
ALTER TABLE Damage ALTER COLUMN CLE_Disinfect DECIMAL(18, 3);
ALTER TABLE Damage ALTER COLUMN CLE_MUD DECIMAL(18, 3);
ALTER TABLE Damage ALTER COLUMN CLE_Trash DECIMAL(18, 3);
ALTER TABLE Damage ALTER COLUMN CLE_Garbage DECIMAL(18, 3);
```

### 0810

#### 當月累積工時 9h

- 環保局水質通報加入 不合格數、抽檢中(0.5h)
- 修正環保局水質通報會重覆顯示資料的問題(1h)

### 0809

#### 當月累積工時 7.5h

- 水質抽驗統計修正(0.5h)

### 0808

#### 當月累積工時 7h

- 修正環境設施災損查詢其他類別填入處理情形出現錯誤(0.5h)
- 水質通報合格不合格判斷修正、抽檢數量修正(1h)
- 自來公司抽檢結果通報表修正、環保局水質通報修正(0.5h)

### 0807

#### 當月累積工時 5h

- 災情通報確認，認列出所有鄉鎮(無論有無填寫)(3.5h)

### 0804

#### 當月累積工時 1.5h

- 新增災情通報確認日期篩選清單(1.5h)
- 資料按照區排序

### 0724

- 調整水質抽驗結果查詢，新增"原水濁度"欄位 (0.5h)
- 水質抽檢通報表，PDF檔案新增"原水濁度"欄位 (0.5h)
- PDF 改為顯示第二區 (1h)

### 0723

#### 今天才開始統計時數，這個月已經超時

- 移除無災情通報彈跳視窗(0.5h)
- 區大隊確認時間修正 (2.5h)
  - 新增區大隊確認時間欄位
- 修正報表異動時間(0.5h))

```sql
ALTER TABLE Damage ADD TeamConfirmTime datetime2 NULL 
```



### 0722

- 通報無災情後轉到列表頁

### 0721

- 修正環保局自來水抽驗結果通報
- 自來水抽驗結果通報地址欄為非必填

### 0718

- 隱藏 線上輪值表 
- 任務編組 檔案上傳下載

### 0717

- 補上 檢核表單 xls

### 0711

- 發簡訊、查詢經緯度，強制使用 Tls12

### 0709

- 修正車輛統計

### 0707

- 加入 NLog 套件
- 簡訊查詢加入 Log

### 0629

- 修正地圖顯示錯誤(不會全部變色的問題)
- 修正 整備通報 環保局回報卻沒有改狀態的問題

### 0627

- 更新地圖 Json資訊 epaemis_local_PROD.SystemConfigSetting

### 0626

- [修正]請求人力支援中，國軍少個"天"字
- [修正]線上報名人數無法修改
- [調整]線上報名照會議時間排序
- 請求支援調整

  1. 其他項目的顯示麻煩改回"件數"，以符合標頭。

  2. 最後一欄的環保署補助金額請顯示各項需求中環保署有核定的補助金額合計

### 0530

- 首頁整備通報圖資中，邏輯修改為只要環保局通報完成，整個市即可變色。

### 0524

- 地址快速定位(經緯度 座標)

### 0521

- 水質抽驗結果通報，修改列表顯示合格、不合格數
- 輸出 PDF 跑版修正 改為橫式

### 0515

- 請求支援設備-新增 Days 欄位
  - 新增、修改、列表

```sql
ALTER TABLE [ApplyDisinfectionEquipmentDetail] ADD Days INT NOT NULL Default(0)
```



### 0513

- 修改消毒設備單位名稱，天改為台
- 請求天數加入金額計算
- 請求支援統計表-補助金額計算調整

### 0428

- 請求支援統計調整
- 修改災情資料後，會變成有災情

### 0424

- 修正請求支援環保局審核不會更改狀態的問題

- 人力支援調整 
  - 新增 Table
  - 人力支援功能實作
- 消毒設備調整
  - 新增 Table
  - 消毒設備功能實作

#### 人力支援 Table 新增

```sql
CREATE TABLE [dbo].[ApplyPeopleHandlingSituation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ApplyId] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[PeopleType] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Day] [int] NOT NULL,
	[Subsidy] [decimal](18, 4) NOT NULL,
 CONSTRAINT [PK_ApplyPeopleHandlingSituation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求申請 id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyPeopleHandlingSituation', @level2type=N'COLUMN',@level2name=N'ApplyId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人力支援、補助款' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyPeopleHandlingSituation', @level2type=N'COLUMN',@level2name=N'Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支援種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyPeopleHandlingSituation', @level2type=N'COLUMN',@level2name=N'PeopleType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyPeopleHandlingSituation', @level2type=N'COLUMN',@level2name=N'Quantity'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'天' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyPeopleHandlingSituation', @level2type=N'COLUMN',@level2name=N'Day'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助款' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyPeopleHandlingSituation', @level2type=N'COLUMN',@level2name=N'Subsidy'
GO
```

#### 消毒設備 Table 新增

```sql
CREATE TABLE [dbo].[ApplyDisinfectionEquipmentHandlingSituation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ApplyId] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[Name] [nvarchar](200) NULL,
	[Day] [int] NOT NULL,
	[Subsidy] [decimal](18, 4) NOT NULL,
 CONSTRAINT [PK_ApplyDisinfectionEquipmentHandlingSituation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求申請 id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyDisinfectionEquipmentHandlingSituation', @level2type=N'COLUMN',@level2name=N'ApplyId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消毒設備、補助款' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyDisinfectionEquipmentHandlingSituation', @level2type=N'COLUMN',@level2name=N'Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消毒設備名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyDisinfectionEquipmentHandlingSituation', @level2type=N'COLUMN',@level2name=N'Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'天' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyDisinfectionEquipmentHandlingSituation', @level2type=N'COLUMN',@level2name=N'Day'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助款' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyDisinfectionEquipmentHandlingSituation', @level2type=N'COLUMN',@level2name=N'Subsidy'
GO
```



### 0331

修正

- 點選"水質抽驗結果查詢"無法修改結果且仍然看的到其他區處資料
2. 通報表應顯示抽驗日期 亦即通報時之"建置"日期 便於後續彙整(為避免隔日才通報之結果)
3. 通報後仍顯示未檢測，不過本日無災情及無法抽顯之勾選有效
4. 抽驗通報應可建置多筆

### 0327

- 無障礙網頁 3.1、8.2、9.1問題修正

### 0322

- alt 文字調整

### 0320

- 水質通報及水質抽檢通報表 請按舊系統方式呈現
- 請求支援統計「環境消毒藥劑」修正
- 無障礙網頁調整 8.1、8.2
- 移除台南市安平區

### 0315

- 請求支援核定權限問題調整

### 0314

- 水質抽驗結果通報列印內容修正

### 0313

- 請求支援 - 轉呈環保署才顯示「環保署狀態」且環保署才能「確認核定」
- 聯絡人資料查詢/維護清單 - 修正沒有台南市的安平中西區
- 環境災損 - 總隊處理情形
- 環境災損 - 備註

### 0308

- 修正有 [ ] 會錯誤的問題
- 修正總隊開始辦理
- 水質抽檢通報表
- 管理處調整
- 1.1是要左上角Logo圖示的alt值要網站或機關全名+LOGO的方式,不能用EPA LOGO這樣的簡稱
- 1.6則是需要那些圖示的alt值=""不能有值
- 8.1必須在首頁地方點選網址列加Tab鍵時,會跑出跳到主要內容的連結,然後點選後會網頁轉至最新消息內容或是公告之類的地方

### 0221

- 請求支援詳細資料
  - 請求支援-車輛設備
  - 請求支援-環境消毒藥劑
  - 請求支援-環境消毒設備
  - 請求支援-人力支援
  - 請求支援-請求補助款
  - 請求支援-其他
- 總隊可審查[請求支援-車輛設備]
- 整備通報 Report 匯出檔案標題新增災害名稱

### 0214

- 實作 消毒藥劑支援 辦理情形

```sql
CREATE TABLE [dbo].[ApplyMedicineHandlingSituation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ApplyId] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[MedicineType] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Subsidy] [decimal](18, 4) NOT NULL,
 CONSTRAINT [PK_ApplyMedicineHandlingSituation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求申請 id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyMedicineHandlingSituation', @level2type=N'COLUMN',@level2name=N'ApplyId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助款、環境消毒藥劑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyMedicineHandlingSituation', @level2type=N'COLUMN',@level2name=N'Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'藥劑種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyMedicineHandlingSituation', @level2type=N'COLUMN',@level2name=N'MedicineType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyMedicineHandlingSituation', @level2type=N'COLUMN',@level2name=N'Quantity'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助款金額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyMedicineHandlingSituation', @level2type=N'COLUMN',@level2name=N'Subsidy'
GO
```



### 0207

- 流動廁所 新增、修改、刪除 調整
- 實作 消毒設備支援
- 實作 補助支援
- 宜蘭縣加上南澳鎮

### 0203

   ```sql
CREATE UNIQUE INDEX 
UniqueIndexGroupIdUserId ON UserGroupMapp ([GroupId], [UserId]);   
   ```

### 0131

- 更新**請求支援其他** EPA 辦理情形
- 新增 ApplyHandlingSituation Table
- 實作 人力支援
- 實作 其他支援

```sql
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ApplyHandlingSituation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ApplyType] [int] NOT NULL,
	[ApplyId] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[Description] [nvarchar](200) NULL,
	[Subsidy] [decimal](18, 4) NOT NULL,
 CONSTRAINT [PK_ApplyHandlingSituation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求支援類型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyHandlingSituation', @level2type=N'COLUMN',@level2name=N'ApplyType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求支援ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyHandlingSituation', @level2type=N'COLUMN',@level2name=N'ApplyId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助類型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyHandlingSituation', @level2type=N'COLUMN',@level2name=N'Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyHandlingSituation', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助款' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyHandlingSituation', @level2type=N'COLUMN',@level2name=N'Subsidy'
GO
```

### 0124

- 新增 車輛辦理情形 Table
- 車輛請求支援-開始辦理 加入辦理情形 新增、修改功能實現
- 加入 ForeignkeyAttribute
- 加入新方法 GetListByForeignkey
- 加入新方法 DeleteByForeignkey

```sql
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ApplyCarHandlingSituation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ApplyId] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[CarType] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Day] [int] NOT NULL,
	[Subsidy] [decimal](18, 4) NOT NULL,
 CONSTRAINT [PK_ApplyCarHandlingSituation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求申請 id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyCarHandlingSituation', @level2type=N'COLUMN',@level2name=N'ApplyId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助款、車輛設備' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyCarHandlingSituation', @level2type=N'COLUMN',@level2name=N'Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車子的種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyCarHandlingSituation', @level2type=N'COLUMN',@level2name=N'CarType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyCarHandlingSituation', @level2type=N'COLUMN',@level2name=N'Quantity'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'天' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyCarHandlingSituation', @level2type=N'COLUMN',@level2name=N'Day'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助款' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplyCarHandlingSituation', @level2type=N'COLUMN',@level2name=N'Subsidy'
GO
```

### 0116

- 全部請求支援
  - 確認核定實作
  - 取消核定實作

### 0111

- 指引十 修正 新聞 連結取消
- 指引五 登入按鈕如果 focus 變顏色 記住我的密碼 可以 focus
- 指引四 右下方-向上箭頭 top  focus 變顏色 
- 指引三 題標籤的層級依序使用

### 0103

- 水質抽驗結果通報 輸出 PDF
- 新增檢查 沒有輸入車輛資訊時跳出彈跳視窗

## 測試帳密

| 名稱                             | 帳號            | 密碼             |
| -------------------------------- | --------------- | ---------------- |
| 環保署                           | EPA-01          | ABCABC0109       |
| 環保局                           | yhwei           | yhwei1980        |
| 大園區清潔隊                     | 74031           | a12345678        |
| 台灣自來水公司<br />第十區管理處 | 8293            | a12345678        |
| 環保署                           | simenvi         | @Simenvi9999     |
| 環保局                           | chiayuchang0131 | 1122father       |
| 督察總隊歐昆霖                   | Z688-1          | zxcvb333zxcvb333 |
| 區大隊                           | 055229          | Z000005529       |
|                                  |                 |                  |
