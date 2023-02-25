--Генерация случайного значения на некотором интервале
create or replace function RandomBetween(low int, high int) returns numeric(5,2) as
	$$
		begin
		   return random() * (high - low + 1) + low;
		end;
	$$ language plpgsql;


--Получение идентификатора контрагента по идентификатору контракта
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


--Получение идентификатора менеджера по идентификатору контракта
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


--Получение величины налога по его идентификатору
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
	

--Получение цены товара по его идентификатору и указанной дате
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
	

--Подсчёт стоимости совершения транзакции
create or replace function CalculateTransactionCost(productID integer, quantity integer, taxValue numeric(3,2), transactionDate timestamp) returns numeric(10,2) as
	$$
		declare
			summ numeric(10,2);
		begin
			summ = GetProductPrice(productID, transactionDate) * quantity * taxValue;
			return summ;
		end;
	$$ language plpgsql;