create or replace trigger insert_manager instead of insert on managers_with_optional_info for each row
	execute procedure add_new_manager();
create or replace function add_new_manager() returns trigger as 
	$$
		begin
			insert into Managers(email,password,fullname,percent,hire_day,comments,parent_id)
			values(new.email, new.password, new.man_fullname, new.percent, new.hire_day, new.comments, new.lead_id);
			
			return new;
		end;
	$$ language plpgsql;
	
	
create or replace trigger delete_manager instead of delete on managers_with_optional_info for each row
	execute procedure delete_current_manager();
create or replace function delete_current_manager() returns trigger as
	$$
		begin
			delete from Incoming where man_id = old.man_id;
			delete from Outgoing where man_id = old.man_id;
			delete from Contracts where man_id = old.man_id;
			update Managers set comments = 'Отсутствует руководитель' where parent_id = old.man_id;
			update Managers set parent_id = null where parent_id = old.man_id;
			delete from Managers where man_id = old.man_id;
			
			return new;
		end;
	$$ language plpgsql;
	
	
create or replace trigger insert_contract instead of insert on contracts_with_optional_info for each row
	execute procedure add_new_contract();
create or replace function add_new_contract() returns trigger as 
	$$
		begin
			insert into Contracts(contr_id, man_id, dayfrom, dayto)
			values(new.Contr_ID, new.Man_ID, new.DayFrom, new.DayTo);
			
			return new;
		end;
	$$ language plpgsql;


create or replace trigger update_contract instead of update on contracts_with_optional_info for each row
	execute procedure update_current_contract();
create or replace function update_current_contract() returns trigger as
	$$
		begin
			update Contracts set dayto = new.dayto, comments = new.comments
			where id = new.id;
		
			return new;
		end;
	$$ language plpgsql;
	
--ПЕРЕДЕЛАТЬ!!!!----
create or replace trigger insert_incoming instead of insert on incoming_with_optional_info for each row
	execute procedure buy_product();
create or replace function buy_product() returns trigger as
	$$
		begin
			insert into Incoming(prod_id, tax_id, contr_id, man_id, inc_date, quantify, cost)
			values(new.Prod_ID, 1, GetContragentID(new.Contract_ID), GetManagerID(new.Contract_ID), new.Inc_Date, new.Quantity, CalculateTransactionCost(new.Prod_ID, new.Quantity, GetTaxValue(1), new.Inc_Date));
			
			return new;
		end;
	$$ language plpgsql;

--ПЕРЕДЕЛАТЬ!!!!----
create or replace trigger insert_outgoing instead of insert on outgoing_with_optional_info for each row
	execute procedure sell_product();
create or replace function sell_product() returns trigger as
	$$
		begin
			insert into Outgoing(prod_id, tax_id, contr_id, man_id, inc_date, quantify, cost)
			values(new.Prod_ID, 1, GetContragentID(new.Contract_ID), GetManagerID(new.Contract_ID), new.Inc_Date, new.Quantity, CalculateTransactionCost(new.Prod_ID, new.Quantity, GetTaxValue(1), new.Inc_Date));
			
			return new;
		end;
	$$ language plpgsql;