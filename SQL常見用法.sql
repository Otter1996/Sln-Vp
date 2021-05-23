
/*修改原欄位資料長度.資料型態*/
ALTER TABLE [dbo].[Order] ALTER COLUMN Date datetime

/*修改資料表名稱*/
EXEC sp_rename '資料庫名稱.dbo.舊資料表名稱', '新資料表名稱'