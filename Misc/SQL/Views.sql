--Представление, в котором хранится дополнительная информация о менеджерах, а именно фио руководителя, если тот, конечно, имеется
create or replace view managers_with_optional_info as
	select m.fullname man_fullname, m.Man_ID, m.email, m.password, m.percent, m.hire_day, m.comments, m.d_id,
		(select fullname from Managers where man_id = m.parent_id) lead_fullname, m.parent_id lead_id
	from Managers m;
	
	
--Представление, в котором хранится дополнительная информация о подписанных контрактах между менеджерами и контрагентами
create or replace view contracts_with_optional_Info as
	select Contracts.id, (select name from contragents where contr_id = Contracts.contr_id) contr_fullname, Contracts.contr_id, (select fullname from managers where man_id = Contracts.man_id) man_fullname, Contracts.man_id, Contracts.dayfrom, Contracts.dayto
	from Contracts left join Contragents on Contracts.contr_id = Contragents.contr_id;


--Представление, в котором хранится дополнительная информация о покупках менеджеров: ID контракта, сколько было заплачено и сколько денег "взяли" налоги
create or replace view incoming_with_optional_info as
	select i.inc_id, 
		c.id contract_id, 
		i.prod_id, 
		i.quantify quantity, 
		i.inc_date, 
		i.Quantify * Prices.Value paid,
		i.cost
	from Prices
		right join Products on Prices.Prod_ID = Products.Prod_ID
		right join Incoming i on i.Prod_ID = Products.Prod_ID
		left join Managers m on i.man_id = m.man_id
		left join Contracts c on m.man_id = c.man_id
	where Prices.DayFrom <= i.Inc_Date and i.Inc_Date <= Prices.DateTo;


--Представление, в котором хранится дополнительная информация о продажах менеджеров: ID контракта, сколько было заработано предприятием и менеджером, а также сколько денег "взяли" налоги
create or replace view outgoing_with_optional_info as
	select i.out_id, 
		c.id Contract_ID, 
		i.prod_id, 
		i.quantify Quantity, 
		i.out_date, 
		i.Quantify * Prices.Value earned,
		m.percent * i.Quantify * Prices.Value man_earn,  
		i.cost
	from Prices
		right join Products on Prices.Prod_ID = Products.Prod_ID
		right join Outgoing i on i.Prod_ID = Products.Prod_ID
		left join Managers m on i.man_id = m.man_id
		left join Contracts c on m.man_id = c.man_id
	where Prices.DayFrom <= i.out_Date and i.out_Date <= Prices.DateTo;