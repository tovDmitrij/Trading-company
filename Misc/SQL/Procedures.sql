--Функция, пренназначенная для добавления сообщений о выполненных транзакциях
create or replace procedure AddMessage(type varchar(100), causeOfException varchar(1000), description varchar(1000)) as
	$$
		begin
			insert into Messages(type, time, causeoferror, description)
			values(type, now(), causeOfException, description);
			--if type = 'fail' then
				--commit;
				--raise exception '%', description;
			--end if;
		end;
	$$ language plpgsql;

--Процедура, которая проверяет, просрочен ли контракт
create or replace procedure CheckContract(contractID integer) as
	$$
		declare
			isValid integer;
		begin
			if contractID is null then
				call AddMessage('fail', current_query(), 'Идентификатор контракта не был определён!');
			end if;
			select count(*) into isValid from Contracts
			where id = contractID;
			if isValid = 0 then
				call AddMessage('fail', current_query(), 'Контракт №'||contractID||' не существует!');
			end if;
			select count(*) into isValid from Contracts
			where id = contractID
				and dayfrom <= now() and now() <= dayto;
			if isValid = 0 then
				call AddMessage('fail', current_query(), 'Контракт №'||contractID||' просрочен!');
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
				call AddMessage('fail', current_query(), 'Идентификатор контракта не был определён!');
			end if;
			select count(*) into isValid from Contracts
			where id = contractID;
			if isValid = 0 then
				call AddMessage('fail', current_query(), 'Контракта №'||contractID||' не существует!');
			end if;
			select count(*) into isValid from Contracts
			where id = contractID
				and dayfrom <= incomingDate and incomingDate <= dayto;
			if isValid = 0 then
				call AddMessage('fail', current_query(), 'Контракт №'||contractID||' на дату '||incomingDate||' будет просрочен!');
			end if;
		end;
	$$ language plpgsql;

--(!!!!ВОЗМОЖНО, ЧТО УЖЕ НЕ НУЖНА. ЧЕКНУТЬ ПРИ ТЕСТИРОВАНИИ!!!!!)
--Процедура, которая проверяет существование контрагента и наличие у него активного банковского аккаунта
create or replace procedure CheckContragent(contrId integer) as
	$$
		begin
			if contrID is null then
				call AddMessage('fail', current_query(), 'Пожалуйста, введите идентификатор контрагента!');
			end if;
			if (select count(*) from Contragents where contr_id = contrID) = 0 then
				call AddMessage('fail', current_query(), 'Контрагента №'|||contrID||' не существует!');
			end if;
			if (select count(*) from Accounts where contr_id = contrID and dayfrom <= now() and now() <= dayto) = 0 then
				call AddMessage('fail', current_query(), 'У контрагента №'||contrID||' отсутствует активный банковский аккаунт!');
			end if;
		end;
	$$ language plpgsql;
	
--Процедура, которая проверяет существование контрагента и наличие у него активного банковского аккаунта
create or replace procedure CheckContragent(fullName varchar(150), contrId integer) as
	$$
		begin
			if fullName is null then
				call AddMessage('fail', current_query(), 'Пожалуйста, введите ФИО контрагента!');
			end if;
			if contrID is null then
				call AddMessage('fail', current_query(), 'Пожалуйста, введите идентификатор контрагента!');
			end if;
			if (select count(*) from Contragents where contr_id = contrID and name = fullName) = 0 then
				call AddMessage('fail', current_query(), 'Контрагента '||fullname||'#'||contrID||' не существует!');
			end if;
			if (select count(*) from Accounts where contr_id = contrID and dayfrom <= now() and now() <= dayto) = 0 then
				call AddMessage('fail', current_query(), 'У контрагента '||fullname||'#'||contrID||' отсутствует активный банковский аккаунт!');
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
				call AddMessage('fail', current_query(), 'ФИО не было введено!');
			end if;
			select count(*) into isValid from regexp_matches(fullName, ' ', 'g');
			if isValid < 2 then
				call AddMessage('fail', current_query(), 'ФИО введено некорректно: между фамилией, именем и отчеством необходим отступ в виде пробела!');
			elsif isValid > 2 then 
				call AddMessage('fail', current_query(), 'ФИО введено некорректно: присутствуют лишние пробелы!');
			end if;
		end;
	$$ language plpgsql;

--Процедура, которая проверяет валидность введённой почты
create or replace procedure CheckEmail(newEmail varchar(150)) as
	$$
		begin
			if newEmail is null then
				call AddMessage('fail', current_query(), 'Почта не была введена!');
			end if;
			if not newEmail ~ '@' then
				call AddMessage('fail', current_query(), 'Почта введена некорректно: отсутствует символ "@"!');
			end if;
			if (select count(*) from Managers m where m.email = newEmail) != 0 then
				call AddMessage('fail', current_query(), 'Почта уже занята другим пользователем!');
			end if;
		end;
	$$ language plpgsql;
	
--Процедура, которая проверяет валидность введённого пароля при регистрации менеджера
create or replace procedure CheckPassword(password varchar(300)) as
	$$
		begin
			if password is null then
				call AddMessage('fail', current_query(), 'Не был введён пароль!');
			end if;
			if length(password) < 8 then
				call AddMessage('fail', current_query(), 'Пароль слишком короткий: необходимо минимум 8 символов!');
			end if;	
		end;
	$$ language plpgsql;

--Процедура, которая проверяет валидность введённого процента при регистрации менеджера
create or replace procedure CheckPercent(percent numeric(4,2)) as
	$$
		begin
			if percent is null or percent <= 0 then
				call AddMessage('fail', current_query(), 'Не был введён желаемый процент с продаж!');
			end if;
			if percent > 50 then
				call AddMessage('fail', current_query(), 'Процент слишком высокий: необходимо указать меньше 50!');
			end if;
		end;
	$$ language plpgsql;

--Процедура, которая при регистрации менеджера проверяет возможность встать в подчинении некоторому менеджеру
create or replace procedure CheckLeader(leadID integer) as
	$$
		begin
			if (select count(*) from Managers where Man_ID = leadID) = 0 then
				call AddMessage('fail', current_query(), 'Руководителя №'||leadID||' не существует!');
			end if;
			if (select count(*) from Managers where Parent_ID = leadID) > 3 then
				call AddMessage('fail', current_query(), 'Нельзя подписаться к руководителю №'||leadID||'. Пожалуйста, выберите другого или не выбирайте вовсе!');
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
				call AddMessage('fail', current_query(), 'Идентификатор товара не был определён!');
			end if;
			select count(*) into isValid from Products 
			where prod_id = productID;
			if isValid = 0 then
				call AddMessage('fail', current_query(), 'Товара №'||productID||' не существует!');
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
				call AddMessage('fail', current_query(), 'Идентификатор товара не был определён!');
			end if;
			select count(*) into isValid from Products 
			where prod_id = productID;
			if isValid = 0 then
				call AddMessage('fail', current_query(), 'Товара №'||productID||' не существует!');
			end if;
			
			existProductQuantity = GetProductQuantity(prodID);
			if quantity < 0 then
				call AddMessage('fail', current_query(), 'Указанное кол-во товара '||quantity||' недопустимо: необходимо указать больше нуля!');
			elsif existProductQuantity - quantity <= 0 then
				call AddMessage('fail', current_query(), 'Указанное кол-во товара '||quantity||' недопустимо: на складе есть только '||existProductQuantity||' единиц товара!');
			end if;
		end;
	$$ language plpgsql;