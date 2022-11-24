create or replace trigger InsertManager instead of insert on ManagersWithOptionalInfo for each row
	execute procedure AddNewManager();
create or replace function AddNewManager() returns trigger as 
	$$
		begin
			insert into Managers(email,password,fullname,percent,hire_day,comments,parent_id)
			values(new.email, new.password, new.fullname, new.percent, new.hire_day, new.comments, new.parent_id);
			
			return new;
		end;
	$$ language plpgsql;
	
	
create or replace trigger DeleteManager instead of delete on ManagersWithOptionalInfo for each row
	execute procedure DeleteCurrentManager();
create or replace function DeleteCurrentManager() returns trigger as
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
	
	
create or replace trigger InsertContract instead of insert on ContractsWithOptionalInfo for each row
	execute procedure AddNewContract();
create or replace function AddNewContract() returns trigger as 
	$$
		begin
			insert into Contracts(contr_id, man_id, dayfrom, dayto)
			values(new.Contr_ID, new.Man_ID, new.DayFrom, new.DayTo);
			
			return new;
		end;
	$$ language plpgsql;



create or replace trigger InsertIncoming instead of insert on IncomingWithOptionalInfo for each row
	execute procedure BuyProducts();
create or replace function BuyProducts() returns trigger as
	$$
		begin
			insert into Incoming(prod_id, tax_id, contr_id, man_id, inc_date, quantify, cost)
			values(new.Prod_ID, 1, GetContragentID(new.ContractID), GetManagerID(new.ContractID), new.IncDate, new.Quantity, CalculateTransactionCost(new.ProdID, new.Quantity, GetTaxValue(1), new.IncDate));
			
			return new;
		end;
	$$ language plpgsql;



create or replace trigger InsertOutgoing instead of insert on OutgoingWithOptionalInfo for each row
	execute procedure SellProducts();
create or replace function SellProducts() returns trigger as
	$$
		begin
			insert into Outgoing(prod_id, tax_id, contr_id, man_id, inc_date, quantify, cost)
			values(new.Prod_ID, 1, GetContragentID(new.ContractID), GetManagerID(new.ContractID), new.IncDate, new.Quantity, CalculateTransactionCost(new.ProdID, new.Quantity, GetTaxValue(1), new.IncDate));
			
			return new;
		end;
	$$ language plpgsql;