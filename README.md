## :bookmark_tabs: Вкратце о проекте

<div align="center">

![image](https://user-images.githubusercontent.com/86602542/207268705-06f60201-dbba-497e-bdb0-e5b0df344aad.png)

</div>

Проект представляет собой веб-приложение, имитирующее работу менеджеров на торговом предприятии.
## :keyboard: Возможности приложения
| Менеджеры могут |  |
|--|--|
| Регистрироваться на платформе.  | ![auth](https://user-images.githubusercontent.com/86602542/221241410-dd0684c4-702a-4de3-a731-c9383bf4567f.jpg)  
| Посмотреть свой профиль, где находится некоторая информация о них и кнопки для взаимодействия с предприятием. | ![image](https://user-images.githubusercontent.com/86602542/207268705-06f60201-dbba-497e-bdb0-e5b0df344aad.png) | 
| Подписать контракт с контрагентом. | ![newContract](https://user-images.githubusercontent.com/86602542/221242900-29cb29b2-e81f-4511-88c8-1b3afdce4950.jpg) |
| Купить или продать товары. | ![doingTransaction](https://user-images.githubusercontent.com/86602542/221242955-8cc0230e-a7d0-4d4a-86f2-3a136d706207.jpg) |

| Посмотреть статистику | |
| -- | -- |
| Какие товары есть на складе предприятия на текущий момент в виде списка и диаграммы. | ![warehouse](https://user-images.githubusercontent.com/86602542/221243115-519b589d-46f1-46e5-ae19-58d553bcb91b.jpg) |
| Список контрактов текущего менеджера, причём как действующих, так и уже просроченных. | ![contractList](https://user-images.githubusercontent.com/86602542/221243174-48921ba1-b413-473f-82ee-4bfb3f551a61.jpg)|
| Список всех совершённых транзакций менеджера в виде списка. | ![transactionList](https://user-images.githubusercontent.com/86602542/221243241-dea2d5e8-402b-4c4c-9140-849df16680b1.jpg) |
| Динамику курса выбранной валюты по отношению к рублю в виде списка. | ![courceList](https://user-images.githubusercontent.com/86602542/221243293-4e1b9732-23f5-466f-972f-70411aeca8b2.jpg) |
| Динамику цены выбранного товар в виде списка. | ![priceList](https://user-images.githubusercontent.com/86602542/221243359-4a92b084-8e78-4664-aa15-358f40999861.jpg) |

Также на стороне БД настроена автоматическая генерация ценников (еженедельно) и курсов валют (ежедневно) по определённой траектории (вызовы процедур *UpdatePrices*() и *UpdateCources*() соответственно). Для этого использовался планировщик задач *pgAgent*.

**Подробнее о проекте расписано непосредственно в приложении на странице "Инструкция к работе".**
## :page_with_curl: Архитектура приложения
```
App
 ⸠⸺> Areas
 ⸠      ⸠⸺> Contract
 ⸠      ⸠       ⸠⸺> Controllers
 ⸠      ⸠       ⸠⸺> Models
 ⸠      ⸠       ⸠⸺> ViewModels
 ⸠      ⸠       ⸠⸺> Views
 ⸠      ⸠⸺> Course
 ⸠      ⸠       ⸠⸺> Controllers
 ⸠      ⸠       ⸠⸺> Models
 ⸠      ⸠       ⸠⸺> Views
 ⸠      ⸠⸺> Manager
 ⸠      ⸠       ⸠⸺> Controllers
 ⸠      ⸠       ⸠⸺> ViewModels
 ⸠      ⸠       ⸠⸺> Views
 ⸠      ⸠⸺> Price
 ⸠      ⸠       ⸠⸺> Controllers
 ⸠      ⸠       ⸠⸺> Models
 ⸠      ⸠       ⸠⸺> Views
 ⸠      ⸠⸺> Transaction
 ⸠      ⸠       ⸠⸺> Controllers
 ⸠      ⸠       ⸠⸺> Models
 ⸠      ⸠       ⸠⸺> ViewModels
 ⸠      ⸠       ⸠⸺> Views
 ⸠      ⸠⸺> Warehouse
 ⸠             ⸠⸺> Controllers
 ⸠             ⸠⸺> Models
 ⸠             ⸠⸺> Views
 ⸠⸺> Models
 ⸠⸺> Views
 ⸠⸺> Controllers
 ⸠⸺> Hubs
 ⸠⸺> Misc
       ⸠⸺> SQL
```
Приложение было поделено на области (*Areas*). 

В папках, находящихся вне областей такие, как *Models*, *Views*, *Controllers* и *Hubs* присутствуют базовые сущности, которые используются во всех областях. 

В папке *Misc* определены сущности, которые отвечают за кодирование и декодирование информации (*Security.cs*) и проверку активности сессии (*Extensions.cs*)

В папке *Misc/SQL* лежат скрипты на создание таблиц, представлений, триггеров и прочего в базе данных.
## :floppy_disk: Схема БД

<div align="center">

![image](https://user-images.githubusercontent.com/86602542/209706796-acb6267d-24ff-4420-b8ec-f9bb04e2f908.png)

</div>

| Наименование таблицы | Краткое описание |
|--|--|
| Cources | Таблица отношений курсов валют |
| Currencies | Таблица курсов валют |
| Prices | Таблица ценников на товары (*товары могут продаваться в разных валютах*) |
| Products  | Таблица товаров |
| Groups | Таблица групп товаров (*медицина, продовольствие, ...*) |
| Managers | Таблица менеджеров |
| Contragents | Таблица контрагентов (*менеджеры ведут торговлю через них*) |
| Banks | Таблица банков |
| Accounts | Таблица банковских аккаунтов (БА) контрагентов (*без них менеджер не имеет возможности подписывать контракты с контрагентами, у к-рых просроченный или отсутствующий БА!*) |
| Contracts | Таблица контрактов между контрагентом и менеджером |
| Incoming | Таблица покупок товаров менеджеров |
| Outgoing | Таблица продаж товаров менеджеров |
| Taxes | Налоги, списываемые при совершении транзакции |
## :computer: Стек технологий
### :earth_asia: Языки
```
C# 11
JavaScript
Pl/pgSQL
```
### :desktop_computer: Фронтенд
```
Razor Pages
```
### :hammer_and_wrench: Бекэнд
```
ASP.NET Core 7
```
### :floppy_disk: База данных
```
PostgreSQL 15
```
### :scroll: Прочее
```
Entity Framework 7
pgAgent
SignalR
Bootstrap
```