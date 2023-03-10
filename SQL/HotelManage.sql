create table Employee(
	id varchar(20) not null primary key,
	e_name nvarchar(50),
	phone varchar(10),
	e_address nvarchar(255),
)
create table Customer(
	id varchar(20) not null primary key,
	c_name nvarchar(50),
	email varchar(50),
	phone varchar(10),
	c_address nvarchar(255)
)
create table CusService(
	s_name nvarchar(50) not null primary key,
	price float
)
create table Employee_Service(
	e_id varchar(20) not null,
	s_name nvarchar(50) not null,
	constraint fk_employeeService1 foreign key (e_id) references Employee(id),
	constraint fk_employeeService2 foreign key (s_name) references CusService(s_name),
	primary key (e_id, s_name)
)
create table Customer_Service(
	c_id varchar(20) not null,
	s_name nvarchar(50) not null,
	constraint fk_customerService1 foreign key (c_id) references Customer(id),
	constraint fk_customerService2 foreign key (s_name) references CusService(s_name),
	primary key (c_id, s_name)
)
create table Room(
	id varchar(20) not null primary key,
	r_name nvarchar(50),
	price float,
	status nvarchar(20)
)