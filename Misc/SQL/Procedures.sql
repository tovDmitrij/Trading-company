--call UpdateCources();
--Процедуры, обновляющие значения всех курсов валют
create or replace procedure UpdateCources() as
	$$
		declare
			CurrenciesList cursor for
				select cur_id from Currencies where cur_id != 1 order by cur_id;
		begin
			for cource in CurrenciesList loop
				call add_new_cource_value(cource.cur_id);
			end loop;
		end;
	$$ language plpgsql;
create or replace procedure add_new_cource_value(curIDFrom integer) as
	$$
		declare
			courceValue numeric(5,2) := -1;
			dayFrom date := now();
			dayTo date := now() + interval '1 days';
		begin
			while courceValue < 0 loop
				select Sum(value)/Count(*) into courceValue from Cources
				where cur_idfrom = curIDFrom and cur_idto = 1;
				
				courceValue = courceValue + RandomBetween(-2,2);
			end loop;
			
			insert into Cources
			values(curIDFrom, 1, dayFrom, dayTo, courceValue);
			
			insert into Cources
			values(1, curIDFrom, dayFrom, dayTo, 1.0 / courceValue);
		end;
	$$ language plpgsql;


--call UpdatePrices();
--Процедуры, обновляющие ценники у всех товаров
create or replace procedure UpdatePrices() as
	$$
		declare
			ProductList cursor for
				select distinct prod_id from Products order by prod_id;
		begin
			for product in ProductList loop
				call add_new_price_value(product.prod_id);
			end loop;
		end;
	$$ language plpgsql;
create or replace procedure add_new_price_value(prodID integer) as
	$$
		declare
			priceValue numeric(10,2) := -1;
			dayFrom date := now();
			dayTo date := now() + interval '7 days';
			curID integer;
		begin
			while priceValue < 0 loop
				select Sum(value)/Count(*) into priceValue from Prices
				where prod_id = prodID;
				
				priceValue = priceValue + RandomBetween(-5,5);
			end loop;
			
			select cur_id into curID from Prices where prod_id = prodID;
			
			insert into Prices(prod_id, dayfrom, dateto, value, cur_id)
			values(prodID, dayFrom, dayTo, priceValue, curID);
		end;
	$$ language plpgsql;