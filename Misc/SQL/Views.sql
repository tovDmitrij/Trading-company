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


create or replace view prices_with_optional_info as
	select Products.name product_name, Products.prod_id product_id, Prices.dayfrom price_dayfrom, Prices.dateto price_dayto, Prices.value * (
		select coalesce(
			(select value from cources
			 where cur_idfrom = Currencies.cur_id and cur_idto = 1 and dayfrom < Prices.dateto and Prices.dateto <= dayto),
			(select value from cources 
			 where cur_idfrom = Currencies.cur_id and cur_idto = 1 and dayfrom < now() and now() <= dayto))
		) price_value
	from Products
		left join Prices on Products.prod_id = Prices.prod_id
		left join Currencies on Prices.cur_id = Currencies.cur_id
	order by Products.name, Prices.dateto
	

create or replace view products_with_optional_info as
	select distinct g.name group_name, 
		g.group_id,
		p.name prod_name, 
		p.prod_id,
		c.shortname
	from Currencies c
		right join Prices pr on c.cur_id = pr.cur_id
		right join Products p on p.prod_id = pr.prod_id
		left join Groups g on p.group_id = g.group_id
	order by g.group_id, p.prod_id;


create or replace view course_with_optional_info as
	select (
		select cr.shortname from Currencies cr
		where Cources.cur_idfrom = cr.cur_id
	) cur_namefrom, Cources.cur_idfrom,
	(
		select cr.shortname from Currencies cr
		where Cources.cur_idto = cr.cur_id
	) cur_nameto, Cources.cur_idto,
	Cources.dayfrom,
	Cources.dayto,
	Cources.value
	from Currencies
		left join Cources on Currencies.cur_id = Cources.cur_idfrom;
	

create or replace view warehouse as
	select p.name prod_name,
		p.prod_id,
		g.name group_name,
		g.group_id,
		(select coalesce(Sum(Incoming.quantify),0) from Incoming 
		 	left join Products on Incoming.prod_id = Products.prod_id 
		 where Products.prod_id = p.prod_id and Incoming.Inc_Date <= now())
		- 
		(select coalesce(Sum(Outgoing.quantify),0) from Outgoing 
		 	left join Products on Outgoing.prod_id = Products.prod_id 
		 where Products.prod_id = p.prod_id and Outgoing.Out_Date <= now()) prod_quantity
	from Products p
		left join Groups g on p.group_id = g.group_id
	order by p.prod_id, g.group_id;


create or replace view incoming_with_optional_info as
	select distinct Incoming.inc_id transaction_id,
		(select c.id from Contracts c where c.contr_id = Incoming.contr_id and c.man_id = Incoming.man_id and Incoming.Inc_Date < c.dayto limit 1) contract_id,
		Incoming.inc_date transaction_date, 
		Products.name prod_name,
		Incoming.prod_id, 
		Incoming.quantify prod_quantity,
		Incoming.Quantify * (select p.value from Prices p where p.prod_id = Prices.prod_id and p.dayfrom < Incoming.Inc_Date and Incoming.Inc_Date <= p.dateto) * (
			select coalesce(
				(select value from cources
				 where cur_idfrom = Currencies.cur_id and cur_idto = 1 and dayfrom < Incoming.Inc_Date and Incoming.Inc_Date <= dayto),
				(select value from cources 
				 where cur_idfrom = Currencies.cur_id and cur_idto = 1 and dayfrom < now() and now() <= dayto)
			)) transaction_paid,
		Incoming.cost * (
			select coalesce(
				(select value from cources
				where cur_idfrom = Currencies.cur_id and cur_idto = 1 and dayfrom < Incoming.Inc_Date  and Incoming.Inc_Date <= dayto),
				(select value from cources 
				where cur_idfrom = Currencies.cur_id and cur_idto = 1 and dayfrom < now() and now() <= dayto)
			)) cost
	from Currencies
		right join Prices on Currencies.cur_id = Prices.cur_id
		right join Products on Prices.Prod_ID = Products.Prod_ID
		right join Incoming on Incoming.Prod_ID = Products.Prod_ID
		left join Managers on Incoming.man_id = Managers.man_id
		right join Contracts on Managers.man_id = Contracts.man_id
		left join Contragents on Contracts.contr_id = Contragents.contr_id;


create or replace view outgoing_with_optional_info as
	select distinct Outgoing.out_id transaction_id,
		(select c.id from Contracts c where c.contr_id = Outgoing.contr_id and c.man_id = Outgoing.man_id and Outgoing.out_date < c.dayto limit 1) contract_id,
		Outgoing.out_date transaction_date, 
		Products.name prod_name,
		Outgoing.prod_id, 
		Outgoing.quantify prod_quantity,
		Outgoing.Quantify * (select p.value from Prices p where p.prod_id = Prices.prod_id and p.dayfrom < Outgoing.Out_Date and Outgoing.Out_Date <= p.dateto) * (
			select coalesce(
				(select value from cources
				 where cur_idfrom = Currencies.cur_id and cur_idto = 1 and dayfrom < Outgoing.Out_Date  and Outgoing.Out_Date <= dayto),
				(select value from cources 
				 where cur_idfrom = Currencies.cur_id and cur_idto = 1 and dayfrom < now() and now() <= dayto)
			)) 
			-
			Managers.percent * Outgoing.Quantify * (select p.value from Prices p where p.prod_id = Prices.prod_id and p.dayfrom < Outgoing.Out_Date and Outgoing.Out_Date <= p.dateto) * (
			select coalesce(
				(select value from cources
				 where cur_idfrom = Currencies.cur_id and cur_idto = 1 and dayfrom < Outgoing.Out_Date  and Outgoing.Out_Date <= dayto),
				(select value from cources 
				 where cur_idfrom = Currencies.cur_id and cur_idto = 1 and dayfrom < now() and now() <= dayto)
			)) transaction_earn,
		Managers.percent * Outgoing.Quantify * (select p.value from Prices p where p.prod_id = Prices.prod_id and p.dayfrom < Outgoing.Out_Date and Outgoing.Out_Date <= p.dateto) * (
			select coalesce(
				(select value from cources
				 where cur_idfrom = Currencies.cur_id and cur_idto = 1 and dayfrom < Outgoing.Out_Date  and Outgoing.Out_Date <= dayto),
				(select value from cources 
				 where cur_idfrom = Currencies.cur_id and cur_idto = 1 and dayfrom < now() and now() <= dayto)
			)) manager_earn,
		Outgoing.cost * (
			select coalesce(
				(select value from cources
				 where cur_idfrom = Currencies.cur_id and cur_idto = 1 and dayfrom < Outgoing.Out_Date  and Outgoing.Out_Date <= dayto),
				(select value from cources 
				 where cur_idfrom = Currencies.cur_id and cur_idto = 1 and dayfrom < now() and now() <= dayto)
			)) cost
	from Currencies
		right join Prices on Currencies.cur_id = Prices.cur_id
		right join Products on Prices.Prod_ID = Products.Prod_ID
		right join Outgoing on Outgoing.Prod_ID = Products.Prod_ID
		left join Managers on Outgoing.man_id = Managers.man_id
		right join Contracts on Managers.man_id = Contracts.man_id
		left join Contragents on Contracts.contr_id = Contragents.contr_id;