--Триггер для регистрации менеджера. Проверятся:
-- - валидны ли введённые данные;
-- - зарегистрирована ли введённая почта;
-- - если процент с продаж был введён больше 50%, то мягко намекнуть неофиту, что он слишком много берёт;
-- - если был введён желаемый руководитель, то необходимо проверить: сможет ли данный менеджер принять нового подчинённого. В случае, если не может, отправить соответствующее сообщение, чтобы был выбран другой или не выбран вовсе.
create or replace trigger InsertManager before insert on Managers for each row
	execute procedure AddNewManager();
create or replace function AddNewManager() returns trigger as 
	$$
		begin
			delete from Messages;
		--Проверка на валидность введённой фио
			call CheckFullName(new.fullname);
		--Проверка на валидность введённой почты
			call CheckEmail(new.email);
		--Проверка на валидность введённого пароля
			call CheckPassword(new.password);
		--Проверка на валидность введённого процента
			call CheckPercent(new.percent);
		--Проверка на валидность введённого руководителя
			if new.parent_id is not null then
				call CheckLeader(new.parent_id);
			else
				new.comments = 'Отсутствует руководитель';
			end if;
		--Прочее
			new.hire_day = now();
			
			if (select count(*) from Messages where type = 'fail') > 0 then
				raise notice 'Невозможно добавить нового менеджера! См. логи';
				return null;
			end if;
			return new;
		end;
	$$ language plpgsql;
	
--Триггер для подписания контракта между менеджером и контрагентом. Проверяется:
-- - существует ли такой контрагент (вводится ФИО + идентификатор);
-- - есть ли действующий банковский аккаунт у введённого контрагента;
-- - контракт должен подписываться МИНИМУМ на неделю.
create or replace trigger InsertContract instead of insert on ContractsWithOptionalInfo for each row
	execute procedure AddNewContract()
create or replace function AddNewContract() returns trigger as 
	$$
		begin
			delete from Messages;
		--Проверка на валидность введённой ФИО контрагента, идентификатора и на существование активного банковского аккаунта контрагента
			call CheckFullName(new.ContrFullName);
			call CheckContragent(new.ContrFullName, new.ContrID);
		--Проверка на валидность даты подписания контракта
			if new.DayTo is null then
				raise exception 'Пожалуйста, введите дату оформление контракта!';
			end if;
			if (extract('epoch' from new.DayTo) - extract('epoch' from now()))/3600.0/24.0 < 7 then
				raise exception 'Контракт должен оформляться минимум на неделю!';
			end if;
		--Прочее
			new.DayFrom = now();
			if (select count(*) from Messages where type = 'fail') > 0 then
				raise notice 'Невозможно подписать новый контракт! См. логи';
				return null;
			end if;
			
			insert into Contracts values(new.ID, new.ContrID, new.ManID, new.DayFrom, new.DayTo);
			return new;
		end;
	$$ language plpgsql;

--Триггер для покупки товаров менеджером. Проверяется:
-- - если дата транзакции откладывается на некоторую дату, то проверить, что контракт на тот момент времени не будет просрочен (также касается случая, если дата не будет введена, что подразумевает выполнение транзакции сегодня);
-- - введённый идентификатор товара валиден;
-- - введённое количество товара возможно к выполнению транзакции (проверять остатки на складе);
-- - валидность введённого идентификатора контрагента.
create or replace trigger InsertIncoming instead of insert on IncomingWithOptionalInfo for each row
	execute procedure BuyProducts()
create or replace function BuyProducts() returns trigger as
	$$
		begin		
			delete from Messages;
		--Проверка на валидность введённого идентификатора товара и его кол-ва
			call CheckProductQuantity(new.ProdID, new.Quantity);
		--Проверка на валидность введённой даты покупки (если была указана) и идентификатора контракта
			if new.IncDate is not null then
				call CheckContract(new.ContractID, new.IncDate);
			else
				call CheckContract(new.ContractID);
				new.IncDate = now();
			end if;
		--Прочее
			if (select count(*) from Messages where type = 'fail') > 0 then
				raise exception 'Невозможно приобрести товары! См. логи';
				return null;
			end if;
			
			insert into Incoming(prod_id, tax_id, contr_id, man_id, inc_date, quantify, cost)
			values(new.Prod_ID, 1, GetContragentID(new.ContractID), GetManagerID(new.ContractID), new.IncDate, new.Quantity, CalculateTransactionCost(new.ProdID, new.Quantity, GetTaxValue(1), new.IncDate));
			return new;
		end;
	$$ language plpgsql;

--Триггер для продажи товаров менеджером. Проверяется:
-- - если дата транзакции откладывается на некоторую дату, то проверить, что контракт на тот момент времени не будет просрочен (также касается случая, если дата не будет введена, что подразумевает выполнение транзакции сегодня);
-- - введённый идентификатор товара валиден;
-- - введённое количество товара возможно к выполнению транзакции (проверять остатки на складе);
-- - валидность введённого идентификатора контрагента.
create or replace trigger InsertOutgoing instead of insert on OutgoingWithOptionalInfo for each row
	execute procedure SellProducts();
create or replace function SellProducts() returns trigger as
	$$
		begin
			delete from Messages;
		--Проверка на валидность введённого идентификатора товара и его кол-ва
			call CheckProductQuantity(new.ProdID, new.Quantity);
		--Проверка на валидность введённой даты покупки (если была указана) и идентификатора контракта
			if new.IncDate is not null then
				call CheckContract(new.ContractID, new.IncDate);
			else
				call CheckContract(new.ContractID);
				new.IncDate = now();
			end if;
		--Прочее
			if (select count(*) from Messages where type = 'fail') > 0 then
				raise exception 'Невозможно продать товары! См. логи';
				return null;
			end if;
			
			insert into Outgoing(prod_id, tax_id, contr_id, man_id, inc_date, quantify, cost)
			values(new.Prod_ID, 1, GetContragentID(new.ContractID), GetManagerID(new.ContractID), new.IncDate, new.Quantity, CalculateTransactionCost(new.ProdID, new.Quantity, GetTaxValue(1), new.IncDate));
			return new;
		end;
	$$ language plpgsql;