--Таблица курсов валют
create table Currencies(
    Cur_Id serial primary key,
    Name varchar(100),
    ShortName varchar(50)
);


--Таблица банков
create table Banks(
    Bank_Id serial primary key,
    Name varchar(100),
    Address varchar(100)
);


--Таблица контрагентов, с к-рыми подписывают контракты менеджеры
create table Contragents(
    Contr_Id serial primary key,
    Name varchar(100),
    Address varchar(150),
    Phone varchar(100),
    Comments varchar(150)
);


--Таблица групп товаров
create table Groups(
    Group_Id serial primary key,
    Name varchar(100),
    Comments varchar(150)
);


--Таблица налогов на транзакции
create table Taxes(
    Tax_Id serial primary key,
    Name varchar(100),
    Value numeric(3,2),
    Comments varchar(150)
);


--Таблица курсов валют
create table Cources(
    Cur_IdFrom integer not null,
    Cur_IdTo integer not null,
    DayFrom date not null,
    DayTo date,
    Value numeric(5,2) not null,
	primary key(Cur_IdFrom, Cur_IdTo, DayFrom),
    foreign key(Cur_IdFrom) references Currencies(Cur_Id),
    foreign key(Cur_IdTo) references Currencies(Cur_Id)
);


--Таблица банковских аккаунтов контрагентов
create table Accounts(
    Acc_Id serial primary key,
    Bank_Id integer,
    Contr_Id integer,
    DayFrom date,
    DayTo date,
    foreign key(Bank_Id) references Banks(Bank_Id),
    foreign key(Contr_Id) references Contragents(Contr_Id)
);


--Таблица менеджеров
create table Managers(
    Man_Id serial primary key,
	Email varchar(100),
	Password varchar(300),
    FullName varchar(100),
    Percent numeric(3,2),
    Hire_Day timestamp,
    Comments varchar(100),
    Parent_Id integer,
	unique(email),
    foreign key(Parent_Id) references Managers(Man_Id)
);


--Таблица контрактов между менеджерами и контрагентами
create table Contracts(
	ID serial primary key,
    Contr_Id integer not null,
    Man_Id integer not null,
    DayFrom date,
    DayTo date,
	Comments varchar(250),
    foreign key(Contr_Id) references Contragents(Contr_Id),
    foreign key(Man_Id) references Managers(Man_Id)
);


--Таблица товаров
create table Products(
    Prod_Id serial primary key,
    Group_Id integer,
    Name varchar(100),
    Description varchar(250),
    Expire_Time date,
    foreign key(Group_Id) references Groups(Group_Id)
);


--Таблица цен на товары
create table Prices(
    Prod_Id integer not null,
    DayFrom date not null,
    Cur_Id integer,
    DateTo date,
    Value numeric(10,2),
	primary key(Prod_Id, DayFrom),
    foreign key(Prod_Id) references Products(Prod_Id),
    foreign key(Cur_Id) references Currencies(Cur_Id)
);


--Таблица продаж товаров
create table Outgoing(
    Out_Id serial primary key,
    Prod_Id integer,
    Tax_Id integer,
    Contr_Id integer,
    Man_Id integer,
    Out_Date timestamp,
    Quantify integer,
    Cost numeric(10,2),
    foreign key(Prod_Id) references Products(Prod_Id),
    foreign key(Tax_Id) references Taxes(Tax_Id),
    foreign key(Contr_Id) references Contragents(Contr_Id),
    foreign key(Man_Id) references Managers(Man_Id)
);


--Таблица покупок товаров
create table Incoming(
    Inc_Id serial primary key,
    Prod_Id integer,
    Tax_Id integer,
    Contr_Id integer,
    Man_Id integer,
    Inc_Date timestamp,
    Quantify integer,
    Cost numeric(10,2),
    foreign key(Prod_Id) references Products(Prod_Id),
    foreign key(Tax_Id) references Taxes(Tax_Id),
    foreign key(Contr_Id) references Contragents(Contr_Id),
    foreign key(Man_Id) references Managers(Man_Id)
);