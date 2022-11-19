--Процедура, которая проверяет, просрочен ли контракт
create or replace procedure CheckContract(contractID integer) as
	$$
		declare
			isValid integer;
		begin
			if contractID is null then
				raise exception 'Идентификатор контракта не был определён!';
			end if;
			select count(*) into isValid from Contracts
			where id = contractID;
			if isValid = 0 then
				raise exception 'Контракта с идентификатором % не существует!', contractID;
			end if;
			select count(*) into isValid from Contracts
			where id = contractID
				and dayfrom <= now() and now() <= dayto;
			if isValid = 0 then
				raise exception 'Контракт с идентификатором % просрочен!', contractID;
			end if;
		end;
	$$ language plpgsql;

--Процедура, которая проверяет, будет ли просрочен контракт на заданную дату
create or replace procedure CheckContract(contractID integer, incomingDate date) as
	$$
		declare
			isValid integer;
		begin
			if contractID is null then
				raise exception 'Идентификатор контракта не был определён!';
			end if;
			select count(*) into isValid from Contracts
			where id = contractID;
			if isValid = 0 then
				raise exception 'Контракта с идентификатором % не существует!', contractID;
			end if;
			select count(*) into isValid from Contracts
			where id = contractID
				and dayfrom <= incomingDate and incomingDate <= dayto;
			if isValid = 0 then
				raise exception 'Контракт с идентификатором % на дату % будет просрочен!', contractID, incomingDate;
			end if;
		end;
	$$ language plpgsql;

--(!!!!ВОЗМОЖНО, ЧТО УЖЕ НЕ НУЖНА. ЧЕКНУТЬ ПРИ ТЕСТИРОВАНИИ!!!!!)
--Процедура, которая проверяет существование контрагента и наличие у него активного банковского аккаунта
create or replace procedure CheckContragent(contrId integer) as
	$$
		begin
			if contrID is null then
				raise exception 'Пожалуйста, введите идентификатор контрагента!';
			end if;
			if (select count(*) from Contragents where contr_id = contrID) = 0 then
				raise exception 'Контрагента с идентификатором % не существует!', contrID;
			end if;
			if (select count(*) from Accounts where contr_id = contrID and dayfrom <= now() and now() <= dayto) = 0 then
				raise exception 'У контрагента с идентификатором % отсутствует активный банковский аккаунт!', contrID;
			end if;
		end;
	$$ language plpgsql;	
	
--Процедура, которая проверяет существование контрагента и наличие у него активного банковского аккаунта
create or replace procedure CheckContragent(fullName varchar(150), contrId integer) as
	$$
		begin
			if fullName is null then
				raise exception 'Пожалуйста, введите ФИО контрагента!';
			end if;
			if contrID is null then
				raise exception 'Пожалуйста, введите идентификатор контрагента!';
			end if;
			if (select count(*) from Contragents where contr_id = contrID and name = fullName) = 0 then
				raise exception 'Контрагента %#% не существует!', fullName, contrID;
			end if;
			if (select count(*) from Accounts where contr_id = contrID and dayfrom <= now() and now() <= dayto) = 0 then
				raise exception 'У контрагента %#% отсутствует активный банковский аккаунт!', fullname, contrID;
			end if;
		end;
	$$ language plpgsql;	
	
--Процедура, которая проверяет валидность введённой ФИО
create or replace procedure CheckFullName(fullName varchar(100)) as
	$$
		declare
			isValid integer;
		begin
			if fullName is null then
				raise exception 'ФИО не было введено!';
			end if;
			select count(*) into isValid from regexp_matches(fullName, ' ', 'g');
			if isValid < 2 then
				raise exception 'ФИО введено некорректно: между фамилией, именем и отчеством необходим отступ в виде пробела!';
			elsif isValid > 2 then 
				raise exception 'ФИО введено некорректно: присутствуют лишние пробелы!';
			end if;
		end;
	$$ language plpgsql;

--Процедура, которая проверяет валидность введённой почты
create or replace procedure CheckEmail(email varchar(150)) as
	$$
		begin
			if email is null then
				raise exception 'Почта не была введена!';
			end if;
			if not email ~ '@' then
				raise exception 'Почта введена некорректно: отсутствует символ "@"!';
			end if;
			if (select count(*) from Managers m where m.email = email) != 0 then
				raise exception 'Почта уже занята другим пользователем!';
			end if;
		end;
	$$ language plpgsql;

--Процедура, которая проверяет валидность введённого пароля при регистрации менеджера
create or replace procedure CheckPassword(password varchar(300)) as
	$$
		begin
			if password is null then
				raise exception 'Не был введён пароль!';
			end if;
			if length(password) < 8 then
				raise exception 'Пароль слишком короткий: необходимо минимум 8 символов!';
			end if;	
		end;
	$$ language plpgsql;

--Процедура, которая проверяет валидность введённого процента при регистрации менеджера
create or replace procedure CheckPercent(percent numeric(4,2)) as
	$$
		begin
			if percent is null then
				raise exception 'Не был введён желаемый процент с продаж!';
			end if;
			if percent > 50 then
				raise exception 'Процент слишком высокий: необходимо указать меньше 50!';
			end if;
		end;
	$$ language plpgsql;

--Процедура, которая при регистрации менеджера проверяет возможность встать в подчинении некоторому менеджеру
create or replace procedure CheckLeader(leadID integer) as
	$$
		begin
			if (select count(*) from Managers where Man_ID = leadID) = 0 then
				raise exception 'Руководителя с идентификатором % не существует!', leadID;
			end if;
			if (select count(*) from Managers where Parent_ID = leadID) > 3 then
				raise exception 'Нельзя подписаться к руководителю с идентификатором %, пожалуйста, выберите другого или не выбирайте вовсе!', leadID;
			end if;
		end;
	$$ language plpgsql;
	
--Процедура, которая проверяет, существует ли товар с заданным идентификатором в базе данных
create or replace procedure CheckProductExist(productID integer) as
	$$
		declare
			isValid integer;
		begin
			if productID is null then
				raise exception 'Идентификатор товара не был определён!';
			end if;
			select count(*) into isValid from Products 
			where prod_id = productID;
			if isValid = 0 then
				raise exception 'Товара с идентификатором % не существует!', productID;
			end if;
		end;
	$$ language plpgsql;
	
--Процедура, которая проверяет остаток товара на складе и возможность выполнения транзакции для заданного кол-ва товаров
create or replace procedure CheckProductQuantity(prodID integer, quantity integer) as
	$$
		declare
			isValid integer;
			existProductQuantity integer;
		begin
			if productID is null then
				raise exception 'Идентификатор товара не был определён!';
			end if;
			select count(*) into isValid from Products 
			where prod_id = productID;
			if isValid = 0 then
				raise exception 'Товара с идентификатором % не существует!', productID;
			end if;
			
			existProductQuantity = GetProductQuantity(prodID);
			if quantity < 0 then
				raise exception 'Указанное кол-во товара % недопустимо: необходимо указать больше нуля!', quantity;
				--raise exception 'Указано невалидное кол-во товара: невозможно купить товара с идентификатором % в кол-ве %!', prodID, quantity;
			elsif existProductQuantity - quantity <= 0 then
				raise exception 'Указанное кол-во товара % недопустимо: на складе есть только % единиц товара!', quantity, existProductQuantity;
			end if;
		end;
	$$ language plpgsql;