--Представление, в котором хранится дополнительная информация о подписанных контрактах между менеджерами и контрагентами
create or replace view ContractsWithOptionalInfo as
	select Contracts.id "ID", Contracts.contr_id "ContrID", Contragents.name "ContrFullName", Contracts.man_id "ManID", Contracts.dayfrom "DayFrom", Contracts.dayto "DayTo"
	from Contracts left join Contragents on Contracts.contr_id = Contragents.contr_id;

--Представление, в котором хранится дополнительная информация о покупках менеджеров: ID контракта, сколько было заплачено и сколько денег "взяли" налоги
create or replace view IncomingWithOptionalInfo as
	select i.inc_id "IncID", 
		c.id "ContractID", 
		i.prod_id "ProdID", 
		i.quantify "Quantity", 
		i.inc_date "IncDate", 
		i.Quantify * Prices.Value "Paid",
		i.cost "Cost"
	from Prices
		right join Products on Prices.Prod_ID = Products.Prod_ID
		right join Incoming i on i.Prod_ID = Products.Prod_ID
		left join Managers m on i.man_id = m.man_id
		left join Contracts c on m.man_id = c.man_id
	where Prices.DayFrom <= i.Inc_Date and i.Inc_Date <= Prices.DateTo;

--Представление, в котором хранится дополнительная информация о продажах менеджеров: ID контракта, сколько было заработано предприятием и менеджером, а также сколько денег "взяли" налоги
create or replace view OutgoingWithOptionalInfo as
	select i.out_id "OutID", 
		c.id "ContractID", 
		i.prod_id "ProdID", 
		i.quantify "Quantity", 
		i.out_date "IncDate", 
		i.Quantify * Prices.Value "Earned",
		m.percent * i.Quantify * Prices.Value  "ManEarn",  
		i.cost "Cost"
	from Prices
		right join Products on Prices.Prod_ID = Products.Prod_ID
		right join Outgoing i on i.Prod_ID = Products.Prod_ID
		left join Managers m on i.man_id = m.man_id
		left join Contracts c on m.man_id = c.man_id
	where Prices.DayFrom <= i.out_Date and i.out_Date <= Prices.DateTo;