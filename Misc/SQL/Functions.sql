create or replace function GetProductQuantity(prodID integer) returns integer as
	$$
		declare
			amount integer;
		begin
			select Sum(Incoming.Quantify) - Sum(Outgoing.Quantify) into amount from Incoming
				left join Products on Incoming.Prod_ID = Products.Prod_ID
				right join Outgoing on Outgoing.Prod_ID = Products.Prod_ID
			where Products.Prod_ID = prodID;
			return amount;
		end;
	$$ language plpgsql;


create or replace function GetContragentID(contractID integer) returns integer as
	$$
		declare
			contrID integer;
		begin
			select contr_id into contrID from Contracts
			where id = contractID;
			return contrID;
		end;
	$$ language plpgsql;


create or replace function GetManagerID(contractID integer) returns integer as
	$$
		declare
			manID integer;
		begin
			select man_id into manID from Contracts
			where id = contractID;
			return manID;
		end;
	$$ language plpgsql;

	
create or replace function GetTaxValue(taxID integer) returns numeric(3,2) as
	$$
		declare
			taxValue numeric(3,2);
		begin
			select value into taxValue from Taxes
			where tax_id = taxID;
			return taxValue;
		end;
	$$ language plpgsql;
	
	
create or replace function GetProductPrice(productID integer, priceDate timestamp) returns numeric(10,2) as
	$$
		declare
			price numeric(10,2);
		begin
			select value into price from Prices
			where prod_id = productID
				and dayfrom <= priceDate and priceDate <= dateTo;
			return price;
		end;
	$$ language plpgsql;
	

create or replace function CalculateTransactionCost(productID integer, quantity integer, taxValue numeric(3,2), transactionDate timestamp) returns numeric(10,2) as
	$$
		declare
			summ numeric(10,2);
		begin
			summ = GetProductPrice(productID, transactionDate) * quantity * taxValue;
			return summ;
		end;
	$$ language plpgsql;