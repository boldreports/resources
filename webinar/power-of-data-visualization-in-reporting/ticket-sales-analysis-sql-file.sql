--
-- Database: `ReportSampleDatabase`
--

--
-- Connection String
--
Data Source=dataplatformdemodata.syncfusion.com;Initial Catalog=SampleDB;user id=demoreadonly@data-platform-demo;password=N@c)=Y8s*1&dh
--
--
--

-- Authentication Type
-- None
--

-- --------------------------------------------------------

--
-- Dataset `ShowDetails`
--

SELECT [dbo].[Theater].[ShowDate],
[dbo].[Theater].[ShowStartDate],
[dbo].[Theater].[ShowName],
[dbo].[Theater].[BoxName],
[dbo].[Theater].[TicketsSold],
[dbo].[Theater].[UnitPrice] FROM [dbo].[Theater]

--
-- Dataset `TopShowDetails`
--

SELECT [dbo].[Theater].[ShowDate],
[dbo].[Theater].[ShowStartDate],
[dbo].[Theater].[ShowName],
[dbo].[Theater].[BoxName],
Sum([dbo].[Theater].[TicketsSold]) as TicketsSold,
Sum([dbo].[Theater].[UnitPrice]) as UnitPrice FROM [dbo].[Theater] Group by [dbo].[Theater].[ShowName],[dbo].[Theater].[BoxName],[dbo].[Theater].[ShowDate],[dbo].[Theater].[ShowStartDate]

--
-- Dataset `TopShows`
--

SELECT 
[dbo].[Theater].[ShowName],
Sum([dbo].[Theater].[TicketsSold]) as TotalTicketsSold FROM [dbo].[Theater] Group by [dbo].[Theater].[ShowName]
