create user managerRole with password 'jwu7iSQ';
grant all on all tables in schema public to managerRole;
grant usage, select on all sequences in schema public to managerRole;

revoke insert on banks from managerRole;
revoke delete on banks from managerRole;
revoke update on banks from managerRole;

revoke insert on accounts from managerRole;
revoke delete on accounts from managerRole;
revoke update on accounts from managerRole;

revoke insert on taxes from managerRole;
revoke delete on taxes from managerRole;
revoke update on taxes from managerRole;

revoke insert on contragents from managerRole;
revoke delete on contragents from managerRole;
revoke update on contragents from managerRole;

revoke insert on products from managerRole;
revoke delete on products from managerRole;
revoke update on products from managerRole;

revoke insert on groups from managerRole;
revoke delete on groups from managerRole;
revoke update on groups from managerRole;

revoke insert on prices from managerRole;
revoke delete on prices from managerRole;
revoke update on prices from managerRole;

revoke insert on currencies from managerRole;
revoke delete on currencies from managerRole;
revoke update on currencies from managerRole;

revoke insert on cources from managerRole;
revoke delete on cources from managerRole;
revoke update on cources from managerRole;