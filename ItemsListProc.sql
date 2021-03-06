USE [GalAMM_Test]
GO
/****** Object:  StoredProcedure [dbo].[mpzGetTmc]    Script Date: 18.05.2016 10:31:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE 
	[dbo].[mpzGetTmc]
	@g Identifiers readonly
AS

SET NOCOUNT ON;
begin
select 
	Transition = bi.Переход,
	Ready = 
		case bi.Готовая
		when 0 then ''
		when 1 then '*'
		end,

	Task = docum.Номер,
	Stat = 
		case bi.СтатусЗапаса
		when 1 then 'Контроль'
		when 2 then 'Блок'
		when 3 then 'Очередь'
		when 4 then 'Стоп'
		when 5 then 'Нельзя'
		else ''
		end,

	OidNp = np.Oid,
	OidStore = un.OidMx,
	Groupp = ng.Наименование,
	StoreString = pe.СтроковоеПредставление,
	StoreName = pe.Наименование,
	StoreKey = pe.Код,
	Cell = se.Наименование,
	ComNum = np.КСО_ID,
	KeyArt = np.Код,
	NP = np.Наименование,
	EI = ei.АББР,
	Party = sp.Партия,
	DocumentNumber = doc.Номер,
	DocumentName = dc.Наименование,
	NextOperation = o.МХПолучатели + '  ' + j.Наименование,
	UserName =
		coalesce(
			(select top 1 ФИО + ' | ' + Примечание from SecuritySystemUser where UserName = dc.ИзмененПользователем and GCRecord is null),
			(select top 1 ФИО + ' | ' + Примечание from SecuritySystemUser where UserName = dc.ИзмененПользователем),
			dc.ИзмененПользователем),

	Movement = pem.Код,
	DocumentType = 
		case un.DocumentType
		when 0 then 'Приход'
		when 1 then 'Отпуск'
		when 2 then 'Получение'
		when 3 then 'Списание'
		when 4 then 'Выпуск'
		when 5 then 'Передача'
		when 6 then 'Уведомление'
		when 7 then 'Запуск'
		when 8 then 'Отгрузка'
		when 9 then 'Разделение'
		when 14 then 'Коррекция'
		when 17 then 'Изменение'
		when 21 then 'Разборка'
		when 22 then 'Решение'
		when 24 then 'Перемещение'
		end,

	Quantity = 
		case way 
		when 0 then
			(case un.Направление 
			when 0 then un.Значение
			when 1 then -un.Значение
			end)
		when 1 then 0
		end,

	Wait =
		case way
		when 0 then 0
		when 1 then 
			(case un.Направление 
			when 0 then un.Значение
			when 1 then -un.Значение
			end)
		end,

	Dates = un.Дата,
	OrderRP = dc.Номер,
	Refill = 
		case np.СпособПополнения 
		when 1 then 'ПКИ'
		when 2 then 'ДСЕ'
		when 3 then 'ПКИ'
		when 4 then 'ДСЕ'
		else '' end

from (
	select 
		OidMx = bi.МХ, 
		OidBi = bi.Oid,
		tmc.Дата,
		tmc.Направление,
		tmc.Значение,
		tmc.Регистратор,
		tmc.ПозицияРегистратора,
		way = 0,
		DocumentType = bdu.ТипДокументаУчета,
		Movement = 
			case tmc.Направление
			when 0 then bdu.МХПоставщик
			when 1 then bdu.МХПолучатель
			end
		
	from dbo.ТМЦДвижение as tmc
		join dbo.БазовыйИзмерение as bi on tmc.Измерение = bi.Oid
		left join dbo.БазовыйДокументУчета as bdu on tmc.Регистратор = bdu.Oid

	where bi.МХ in (select guid from @g)

	union all

	select 
		OidMx = bdu.МХПоставщик, 
		OidBi = bi.Oid,
		ways.Дата,
		ways.Направление,
		ways.Значение,
		ways.Регистратор,
		ways.ПозицияРегистратора,
		way = 0,
		DocumentType = bdu.ТипДокументаУчета,
		Movement = 
			case ways.Направление
			when 0 then bdu.МХПоставщик
			when 1 then bdu.МХПолучатель
			end

	from dbo.ВПутиДвижение as ways
		join dbo.БазовыйИзмерение as bi on ways.Измерение = bi.Oid
		join dbo.БазовыйДокументУчета as bdu on ways.Регистратор = bdu.Oid
		
	where bdu.МХПоставщик in (select guid from @g)

	union all

	select 
		OidMx = bi.МХ, 
		OidBi = bi.Oid,
		ways.Дата,
		ways.Направление,
		ways.Значение,
		ways.Регистратор,
		ways.ПозицияРегистратора,
		way = 1,
		DocumentType = bdu.ТипДокументаУчета,
		Movement = 
			case ways.Направление
			when 0 then bdu.МХПоставщик
			when 1 then bdu.МХПолучатель
			end

	from dbo.ВПутиДвижение as ways
		join dbo.БазовыйИзмерение as bi on ways.Измерение = bi.Oid
		join dbo.БазовыйДокументУчета as bdu on ways.Регистратор = bdu.Oid
		
	where bi.МХ in (select guid from @g)

	union all

	--в пути извне
	select
		OidMx = np.МХОтпуска, 
		OidBi = bi.Oid,
		ways.Дата,
		ways.Направление,
		ways.Значение,
		ways.Регистратор,
		ways.ПозицияРегистратора,
		way = 1,
		DocumentType = bu.ТипДокументаУчета,
		Movement = 
			case ways.Направление
			when 0 then bu.МХПоставщик
			when 1 then bu.МХПолучатель
			end

	from dbo.ВПутиДвижение as ways
		join dbo.БазовыйИзмерение as bi on ways.Измерение = bi.Oid
		join dbo.НоменклатурнаяПозиция as np on bi.НП = np.Oid
		join dbo.БазовыйДокументУчета as bu on ways.Регистратор = bu.Oid

	where np.МХОтпуска in (select guid from @g)
		and bu.ТипДокументаУчета = 6
		and ways.ExternalID2 is not null
		and bu.ДатаПроведения > '20150830'
) as un
	join dbo.БазовыйИзмерение as bi on un.OidBi = bi.Oid
	join dbo.НоменклатурнаяПозиция as np on bi.НП = np.Oid
	join dbo.ПроизводственнаяЕдиница as pe on un.OidMx = pe.Oid
	left join dbo.ПроизводственнаяЕдиница as pem on un.Movement = pem.Oid
	left join dbo.СкладскаяЯчейка as se on np.ЯчейкаОтпуска = se.Oid
	left join dbo.ЕдиницаИзмерения as ei on np.ЕдиницаИзмерения = ei.Oid
	left join dbo.НоменклатурнаяГруппа as ng on np.НоменклатурнаяГруппа = ng.Oid
	left join dbo.СпецификацияПартия as sp on bi.Партия = sp.Oid
	left join dbo.Документ as dc on bi.ЗаказНаГП = dc.Oid
	left join dbo.Документ as doc on un.Регистратор = doc.Oid
	left join dbo.Документ as docum on bi.Заказ = docum.Oid
	left join dbo.ПозицияДокументаУчета as pdu on un.ПозицияРегистратора = pdu.Oid
	left join dbo.СпецификацияОперация as o on pdu.ОперацияПриемник = o.Oid 
	left join dbo.ВидРаботы j on o.ВидРабот = j.Oid
	
where np.GCRecord is null

order by Dates desc, un.Направление desc

end
