create table Messages(
	ID serial primary key,
	Type varchar(100),
	Time timestamp,
	CauseOfError varchar(1000),
	Description varchar(1000)
);

create table Currencies(
    Cur_Id serial primary key,
    Name varchar(100) not null,
    ShortName varchar(50) not null
);
create table Banks(
    Bank_Id serial primary key,
    Name varchar(100) not null,
    Address varchar(100) not null 
);
create table Contragents(
    Contr_Id serial primary key,
    Name varchar(100) not null,
    Address varchar(150) not null,
    Phone varchar(100) not null,
    Comments varchar(150)
);
create table Dealers(
    D_Id serial primary key,
    Name varchar(100) not null,
    Percent numeric(3,2) not null,
    Comments varchar(150)
);
create table Groups(
    Group_Id serial primary key,
    Name varchar(100) not null,
    Comments varchar(150)
);
create table Taxes(
    Tax_Id serial primary key,
    Name varchar(100) not null,
    Value numeric(3,2) not null,
    Comments varchar(150)
);
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
create table Accounts(
    Acc_Id serial primary key,
    Bank_Id integer not null,
    Contr_Id integer not null,
    DayFrom date not null,
    DayTo date not null,
    foreign key(Bank_Id) references Banks(Bank_Id),
    foreign key(Contr_Id) references Contragents(Contr_Id)
);
create table Managers(
    Man_Id serial primary key,
	Email varchar(100) not null,
	Password varchar(300) not null,
    D_Id integer,
    FullName varchar(100) not null,
    Percent numeric(3,2) not null,
    Hire_Day date,
    Comments varchar(100),
    Parent_Id integer,
	unique(email),
    foreign key(D_Id) references Dealers(D_Id),
    foreign key(Parent_Id) references Managers(Man_Id)
);
create table Contracts(
	ID serial,
    Contr_Id integer not null,
    Man_Id integer not null,
    DayFrom date not null,
    DayTo date not null,
	primary key(Contr_Id, Man_Id),
    foreign key(Contr_Id) references Contragents(Contr_Id),
    foreign key(Man_Id) references Managers(Man_Id)
);
create table Products(
    Prod_Id serial primary key,
    Group_Id integer not null,
    Name varchar(100) not null,
    Description varchar(250) not null,
    Expire_Time date not null,
    foreign key(Group_Id) references Groups(Group_Id)
);
create table Prices(
    Prod_Id integer not null,
    DayFrom date not null,
    Cur_Id integer not null,
    DateTo date not null,
    Value numeric(10,2) not null,
	primary key(Prod_Id, DayFrom),
    foreign key(Prod_Id) references Products(Prod_Id),
    foreign key(Cur_Id) references Currencies(Cur_Id)
);
create table Outgoing(
    Out_Id serial primary key,
    Prod_Id integer not null,
    Tax_Id integer not null,
    Contr_Id integer not null,
    Man_Id integer not null,
    Out_Date date not null,
    Quantify integer not null,
    Cost numeric(10,2) not null,
    foreign key(Prod_Id) references Products(Prod_Id),
    foreign key(Tax_Id) references Taxes(Tax_Id),
    foreign key(Contr_Id) references Contragents(Contr_Id),
    foreign key(Man_Id) references Managers(Man_Id)
);
create table Incoming(
    Inc_Id serial primary key,
    Prod_Id integer not null,
    Tax_Id integer not null,
    Contr_Id integer not null,
    Man_Id integer not null,
    Inc_Date date not null,
    Quantify integer not null,
    Cost numeric(10,2) not null,
    foreign key(Prod_Id) references Products(Prod_Id),
    foreign key(Tax_Id) references Taxes(Tax_Id),
    foreign key(Contr_Id) references Contragents(Contr_Id),
    foreign key(Man_Id) references Managers(Man_Id)
);