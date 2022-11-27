create or replace view managers_with_optional_info as
	select m.fullname man_fullname, 
		m.Man_ID, 
		m.email, 
		m.password, 
		m.percent, 
		m.hire_day, 
		m.comments, 
		m.d_id,
		(select fullname from Managers where man_id = m.parent_id) lead_fullname, 
		m.parent_id lead_id
	from Managers m;
	

create or replace view contracts_with_optional_Info as
	select Contracts.id, 
		(select name from contragents where contr_id = Contracts.contr_id) contr_fullname, 
		Contracts.contr_id, 
		(select fullname from managers where man_id = Contracts.man_id) man_fullname, 
		Contracts.man_id, 
		Contracts.dayfrom,
		Contracts.dayto,
		Contracts.comments
	from Contracts 
		left join Contragents on Contracts.contr_id = Contragents.contr_id
	order by Contracts.id;


create or replace view products_with_optional_info as
	select g.name group_name, 
		g.group_id,
		p.name prod_name, 
		p.prod_id 
	from Products p
		left join Groups g on p.group_id = g.group_id
	order by g.group_id, p.prod_id;
	

create or replace view incoming_with_optional_info as
	select distinct Incoming.inc_id transaction_id,
		(select c.id from Contracts c where c.contr_id = Incoming.contr_id and c.man_id = Incoming.man_id) contract_id,
		Incoming.inc_date transaction_date, 
		Products.name prod_name,
		Incoming.prod_id, 
		Incoming.quantify prod_quantity,
		Incoming.Quantify * Prices.Value * (select value from cources where cur_idfrom = Currencies.cur_id and cur_idto = 1) transaction_paid,
		Incoming.cost * (select value from cources where cur_idfrom = Currencies.cur_id and cur_idto = 1) cost
	from Currencies
		right join Prices on Currencies.cur_id = Prices.cur_id
		right join Products on Prices.Prod_ID = Products.Prod_ID
		right join Incoming on Incoming.Prod_ID = Products.Prod_ID
		left join Managers on Incoming.man_id = Managers.man_id
		right join Contracts on Managers.man_id = Contracts.man_id
		left join Contragents on Contracts.contr_id = Contragents.contr_id
	where Prices.DayFrom <= Incoming.Inc_Date and Incoming.Inc_Date <= Prices.DateTo;
	
	
create or replace view outgoing_with_optional_info as
	select distinct Outgoing.out_id transaction_id,
		(select c.id from Contracts c where c.contr_id = Outgoing.contr_id and c.man_id = Outgoing.man_id) contract_id,
		Outgoing.out_date transaction_date, 
		Products.name prod_name,
		Outgoing.prod_id, 
		Outgoing.quantify prod_quantity,
		Outgoing.Quantify * Prices.Value * (select value from cources where cur_idfrom = Currencies.cur_id and cur_idto = 1) transaction_earn,
		Managers.percent * Outgoing.Quantify * Prices.Value * (select value from cources where cur_idfrom = Currencies.cur_id and cur_idto = 1) manager_earn,
		Outgoing.cost * (select value from cources where cur_idfrom = Currencies.cur_id and cur_idto = 1) cost
	from Currencies
		right join Prices on Currencies.cur_id = Prices.cur_id
		right join Products on Prices.Prod_ID = Products.Prod_ID
		right join Outgoing on Outgoing.Prod_ID = Products.Prod_ID
		left join Managers on Outgoing.man_id = Managers.man_id
		right join Contracts on Managers.man_id = Contracts.man_id
		left join Contragents on Contracts.contr_id = Contragents.contr_id
	where Prices.DayFrom <= Outgoing.out_Date and Outgoing.out_Date <= Prices.DateTo;