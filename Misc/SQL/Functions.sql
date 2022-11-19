--Функция, к-рая возвращает кол-во товара на складе
create or replace function GetProductQuantity(prodID integer) returns integer as
	$$
		declare
			isValid integer;
			amount integer;
		begin
			if productID is null then
				raise exception 'Идентификатор товара не был определён!';
			end if;
			select count(*) into isValid from Products 
			where prod_id = productID;
			if isValid = 0 then
				raise exception 'Товара с идентификатором % не существует!', productID;
			end if;
		
			select Sum(Incoming.Quantify) - Sum(Outgoing.Quantify) into amount from Incoming
				left join Products on Incoming.Prod_ID = Products.Prod_ID
				right join Outgoing on Outgoing.Prod_ID = Products.Prod_ID
			where Products.Prod_ID = prodID;
			return amount;
		end;
	$$ language plpgsql;

--Функция, к-рая возвращает идентификатор контрагента по идентификатору контракта
create or replace function GetContragentID(contractID integer) returns integer as
	$$
		declare
			contrID integer;
		begin
			call CheckContract(contractID);
			select contr_id into contrID from Contracts
			where id = contractID;
			return contrID;
		end;
	$$ language plpgsql;

--Функция, к-рая возвращает идентификатор менеджера по идентификатору контракта
create or replace function GetManagerID(contractID integer) returns integer as
	$$
		declare
			manID integer;
		begin
			call CheckContract(contractID);
			select man_id into manID from Cntracts
			where id = contractID;
			return manID;
		end;
	$$ language plpgsql;
	
--Функция, к-рая возвращает процент налога по его идентификатору
create or replace function GetTaxValue(taxID integer) returns numeric(3,2) as
	$$
		declare
			isValid integer;
			taxValue integer;
		begin
			if taxID is null then
				raise exception 'Идентификатор налога не был определён!';
			end if;
			select count(*) into isValid from Taxes
			where tax_id = taxID;
			if isValid = 0 then
				raise exception 'Налога с идентификатором % не существует!', taxID;
			end if;
			
			select tax_id into taxID from Taxes
			where tax_id = taxID;
			return taxValue;
		end;
	$$ language plpgsql;
	
--Функция, к-рая возвращает цену товара по его идентификатору и дате
create or replace function GetProductPrice(productID integer, priceDate date) returns numeric(10,2) as
	$$
		declare
			price numeric(10,2);
			isValid integer;
		begin
			call CheckProductQuantity(productID);
			select count(*) into isValid from Prices
			where prod_id = productID
				and dayfrom <= priceDate and priceDate <= dateTo;
			if isValid = 0 then
				raise exception 'Отсутствует актуальный ценник на дату % для товара %!', priceDate, productID;
			end if;
			
			select value into price from Prices
			where prod_id = productID
				and dayfrom <= priceDate and priceDate <= dateTo;
			return price;
		end;
	$$ language plpgsql;

--Функция, к-рая подсчитывает сумму расходов при совершении транзакции (в рамках проекта рассматриваются только налоги)
create or replace function CalculateTransactionCost(productID integer, quantity integer, taxValue integer, transactionDate date) returns numeric(10,2) as
	$$
		declare
			summ numeric(10,2);
		begin
			summ = GetProductPrice(productID, transactionDate) * quantity * taxValue;
			return summ;
		end;
	$$ language plpgsql;