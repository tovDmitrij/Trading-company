insert into Currencies values(1, 'Российский рубль', 'RUB');
insert into Currencies values(2, 'Доллар США', 'USD');
insert into Currencies values(3, 'Евро', 'EUR');

insert into Cources values(1, 2, now(), now() + interval '7' day, 0.016);
insert into Cources values(1, 3, now(), now() + interval '7' day, 0.016);
insert into Cources values(2, 1, now(), now() + interval '7' day, 60.79);
insert into Cources values(2, 3, now(), now() + interval '7' day, 0.96);
insert into Cources values(3, 1, now(), now() + interval '7' day, 63.24);
insert into Cources values(3, 2, now(), now() + interval '7' day, 1.04);
insert into Cources values(1, 1, now() - interval '7 year', now() + interval '7 year', 1);
insert into Cources values(2, 2, now() - interval '7 year', now() + interval '7 year', 1);
insert into Cources values(3, 3, now() - interval '7 year', now() + interval '7 year', 1);

insert into Groups(Name) values('Электроника');
insert into Groups(Name) values('Продовольствие');
insert into Groups(Name) values('Одежда');
insert into Groups(Name) values('Медицина');
insert into Groups(Name) values('Транспорт');
insert into Groups(Name) values('Мебель');
insert into Groups(Name) values('Канцелярия');

insert into Taxes(Name, Value) values('НДС', 0.18);
insert into Taxes(Name, Value) values('Подоходный налог', 0.13);
insert into Taxes(Name, Value) values('Налог на прибыль', 0.2);

insert into Banks(name,address) values('SberBank', '19 Vavilova St., Moscow 117312, Russia');
insert into Banks(name,address) values('Tinkoff', 'ул. Хуторская 2-я, д. 38А, стр. 26, Москва, 127287 Россия');
insert into Banks(name,address) values('Alpha-Bank', 'Some address');
insert into Banks(name,address) values('Rosselhozbank', 'Some address');

insert into Dealers(Name, Percent, Comments) values('Ситилинк', 0.22, 'Магазин электроники');
insert into Dealers(Name, Percent) values('М-Видео', 0.2);
insert into Dealers(Name, Percent) values('Пятерочка', 0.33);
insert into Dealers(Name, Percent) values('Wildberries', 0.18);

insert into Contragents(name,address,phone,comments) values('Балашов Илья Дамирович', 'Костромская область, город Дорохово, проезд Гагарина, 17', '+7(912)712-56-90', 'Физлицо');
insert into Contragents(name,address,phone,comments) values('Блинов Максим Иванович', 'Читинская область, город Ступино, пер. Ломоносова, 77', '+7(915)212-52-40', 'Физлицо');
insert into Contragents(name,address,phone,comments) values('Миронов Дмитрий Владиславович', 'Кемеровская область, город Ногинск, спуск Бухарестская, 14', '+7(911)762-16-12', 'Физлицо');
insert into Contragents(name,address,phone,comments) values('Бобров Александр Константинович', 'Калининградская область, город Дорохово, въезд Ленина, 68', '+7(915)717-58-56', 'Физлицо');

insert into Accounts(bank_id,contr_id,dayfrom,dayto) values(2, 1, now(), now() + interval '365' day);
insert into Accounts(bank_id,contr_id,dayfrom,dayto) values(4, 3, now(), now() + interval '365' day);
insert into Accounts(bank_id,contr_id,dayfrom,dayto) values(1, 2, now(), now() + interval '365' day);
insert into Accounts(bank_id,contr_id,dayfrom,dayto) values(3, 4, now(), now() + interval '365' day);

insert into Products(group_id,name,description,expire_time) values(1, 'Palit GeForce RTX 3070 Ti GamingPro', 'Видеокарта 8ГБ 1575МГц', now() + interval '365' day);
insert into Products(group_id,name,description,expire_time) values(1, 'Intel Core i5-11400F OEM', 'Процессор 6х2.6ГГц', now() + interval '365' day);
insert into Products(group_id,name,description,expire_time) values(1, 'GIGABYTE B560M H', 'Материнская плата LGA 1200 Intel B560', now() + interval '365' day);
insert into Products(group_id,name,description,expire_time) values(1, 'LG 24MK430H', 'Монитор 1920х1080@75Гц', now() + interval '365' day);
insert into Products(group_id,name,description,expire_time) values(2, 'КВАС ОЧАКОВСКИЙ П/Б 2Л', 'Двухлитровый квас', now() + interval '365' day);
insert into Products(group_id,name,description,expire_time) values(2, 'Пиво Золотая Бочка светлое', '4.7%, 0.45 л', now() + interval '365' day);
insert into Products(group_id,name,description,expire_time) values(2, 'Кофе Jacobs Gold', 'без кофеина растворимый, 95 г', now() + interval '365' day);
insert into Products(group_id,name,description,expire_time) values(2, 'Форель Гурман соленая филе-кусочки на коже', '200 г', now() + interval '365' day);
insert into Products(group_id,name,description,expire_time) values(3, 'Спецтекс 37', 'Полукомбинезон рабочий', now() + interval '365' day);
insert into Products(group_id,name,description,expire_time) values(3, 'DENIM APPARAL', 'Джинсы женские с высокой посадкой', now() + interval '365' day);
insert into Products(group_id,name,description,expire_time) values(3, 'BELFOR', 'Классический мужской костюм тройка', now() + interval '365' day);
insert into Products(group_id,name,description,expire_time) values(4, 'Термометр', 'Градусник медицинский в контейнере стеклянный', now() + interval '365' day);
insert into Products(group_id,name,description,expire_time) values(4, 'ACAT', 'Кровоостанавливающий жгут венозный тактический', now() + interval '365' day);
insert into Products(group_id,name,description,expire_time) values(5, 'Lada Granta', 'Автомобиль личного пользования', now() + interval '365' day);
insert into Products(group_id,name,description,expire_time) values(5, 'Cat 789D', 'Карьерный самосвал' , now() + interval '365' day);
insert into Products(group_id,name,description,expire_time) values(5, 'Cat 988K' , 'Колесный погрузчик большой мощности' , now() + interval '365' day);
insert into Products(group_id,name,description,expire_time) values(6, 'Кресло Сомерс 03.04' , 'Кресло на ножках' , now() + interval '365' day);
insert into Products(group_id,name,description,expire_time) values(6, 'Диван ОДОС' , 'Прямой нераскладной диван' , now() + interval '365' day);
insert into Products(group_id,name,description,expire_time) values(7, 'Ручка LazyDays' , 'Ручка шариковая' , now() + interval '365' day);
insert into Products(group_id,name,description,expire_time) values(7, 'Степлер OfficeSpace' , 'Степлер до 20 листов' , now() + interval '365' day);
insert into Products(group_id,name,description,expire_time) values(7, 'Линейка ErichKrause' , 'Линейка пластиковая 15см' , now() + interval '365' day);

insert into Prices values(1, now(), 2, now() + interval '7' day, 949);
insert into Prices values(2, now(), 2, now() + interval '7' day, 199);
insert into Prices values(3, now(), 2, now() + interval '7' day, 99);
insert into Prices values(4, now(), 2, now() + interval '7' day, 199);
insert into Prices values(5, now(), 1 , now() + interval '7' day, 81.99);
insert into Prices values(6, now(), 1, now() + interval '7' day, 59.99);
insert into Prices values(7, now(), 1, now() + interval '7' day, 259);
insert into Prices values(8, now(), 1, now() + interval '7' day, 490.90);
insert into Prices values(9, now(), 1, now() + interval '7' day, 1030);
insert into Prices values(10, now(), 3, now() + interval '7' day, 44.16);
insert into Prices values(11, now(), 1, now() + interval '7' day, 15990);
insert into Prices values(12, now(), 1, now() + interval '7' day, 370);
insert into Prices values(13, now(), 3, now() + interval '7' day, 129);
insert into Prices values(14, now(), 1, now() + interval '7' day, 678300);
insert into Prices values(15, now(), 2, now() + interval '7' day, 2467511);
insert into Prices values(16, now(), 3, now() + interval '7' day, 48360);
insert into Prices values(17, now(), 1, now() + interval '7' day, 11500);
insert into Prices values(18, now(), 1, now() + interval '7' day, 10801);
insert into Prices values(19, now(), 3, now() + interval '7' day, 2);
insert into Prices values(20, now(), 1, now() + interval '7' day, 102);
insert into Prices values(21, now(), 1, now() + interval '7' day, 26.20);