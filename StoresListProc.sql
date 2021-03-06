USE [GalAMM_Test]
GO
/****** Object:  StoredProcedure [dbo].[mpzGetMxList]    Script Date: 18.05.2016 10:30:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<gets store names and guids where quantities are not null>
-- =============================================
ALTER PROCEDURE [dbo].[mpzGetMxList]
	-- Add the parameters for the stored procedure here
	
AS
SET NOCOUNT ON;
BEGIN
	declare @p table
	(
		OidStore uniqueidentifier
	)

	insert into @p
	select 
		OidStore = un.Oid

	from (
		select 
			bi.МХ as Oid,
			tmc.Направление,
			tmc.Значение,
			way = 0
		from dbo.ТМЦДвижение as tmc
		join dbo.БазовыйИзмерение as bi on tmc.Измерение = bi.Oid

		union all

		select 
			bu.МХПоставщик as Oid, 
			ways.Направление, 
			ways.Значение,
			way = 0
		from dbo.ВПутиДвижение as ways
		join dbo.БазовыйИзмерение as bi on ways.Измерение = bi.Oid
		join dbo.БазовыйДокументУчета as bu on ways.Регистратор = bu.Oid
		
		union all
		
		select 
			bi.МХ as Oid,
			Направление = 0,
			Значение = 0,
			way = ways.Значение
		from dbo.ВПутиДвижение as ways
		join dbo.БазовыйИзмерение as bi on ways.Измерение = bi.Oid) as un
	 
	group by un.Oid
	having
		(sum(
			case un.Направление 
			when 0 then un.Значение 
			when 1 then -un.Значение 
			end) <> 0)
		or (sum(way) <> 0)

	select Oid as OidStore, СтроковоеПредставление as StoreString, coalesce((select OidStore from @p where OidStore = Вышестоящий), Oid) as Higher 
	from @p
	join dbo.ПроизводственнаяЕдиница p on OidStore = p.Oid
	where Oid in (select OidStore from @p) and GCRecord is null
	order by StoreString



END
