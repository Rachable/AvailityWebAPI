# Datastore Info

MySQL is the datastore used for this API.  The database connection will need to be modified in `appsettings.Development.json`

To create the customer database (or update the database reference)
```sql
create database customerdb;
show create database customerdb;
```

To create the customer table
```sql
create table customers(
id mediumint not null auto_increment,
first_name varchar(60) not null,
last_name varchar(80) not null,
date_created datetime not null,
date_last_updated datetime,
primary key (id)
);
```

To create the triggers that update the date columns
```sql
create trigger before_customers_add before insert on customers for each row set new.date_created = now();
create trigger before_customers_update before update on customers for each row set new.date_last_updated = now());
```