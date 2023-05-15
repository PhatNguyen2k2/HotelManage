create database HotelManage
use HotelManage
create table Employee(
	id int not null primary key identity(1,1),
	e_name nvarchar(50),
	phone varchar(10),
	e_address nvarchar(255),
)
create table Customer(
	id int not null primary key identity(1,1),
	c_name nvarchar(50),
	email varchar(50),
	phone varchar(10),
	c_address nvarchar(255)
)
create table Room(
	id int not null primary key identity(1,1),
	r_name nvarchar(50),
	price float,
	status nvarchar(20)
)
create table Booking (
	r_id int not null,
    c_id int not null,
	checkin date,
	checkout date,
	primary key(r_id, c_id),
	constraint fk_booking1 foreign key (r_id) references Room(id),
	constraint fk_booking2 foreign key (c_id) references Customer(id)
)
create table CusService(
	s_name nvarchar(50) not null primary key,
	price float
)
create table Employee_Service(
	e_id int not null,
	s_name nvarchar(50) not null,
	constraint fk_employeeService1 foreign key (e_id) references Employee(id),
	constraint fk_employeeService2 foreign key (s_name) references CusService(s_name),
	primary key (e_id, s_name)
)
create table Room_Service(
	r_id int not null,
	s_name nvarchar(50) not null,
	constraint fk_roomService1 foreign key (r_id) references Room(id),
	constraint fk_roomService2 foreign key (s_name) references CusService(s_name),
	primary key (r_id, s_name)
)
create table Bill(
	id int not null primary key identity(1,1),
	c_id int not null,
	e_id int not null,
	status nvarchar(20),
	total float default 0,
	constraint fk_bill1 foreign key (c_id) references Customer(id),
	constraint fk_bill3 foreign key (e_id) references Employee(id)
)
create table DetailBill(
	b_id int not null,
	s_name nvarchar(50) not null,
	amount int,
	primary key(b_id, s_name),
	constraint fk_detailbill1 foreign key (b_id) references Bill(id),
	constraint fk_detailbill2 foreign key (s_name) references CusService(s_name)
)
-- Nhân viên
insert into Employee(e_name,phone,e_address) values (N'Nguyễn Hoàng Phát','0362485798',N'Ninh Thuận');
insert into Employee(e_name,phone,e_address) values (N'Phạm Thu Thủy','0326541987',N'Nha Trang');
insert into Employee(e_name,phone,e_address) values (N'Lê Nguyễn Yến Ngọc','0635129861',N'Long An');
insert into Employee(e_name,phone,e_address) values (N'Nguyễn Phạm Lan Anh','0951362476',N'Biên Hòa');

-- Khách hàng
insert into Customer(c_name,email,phone,c_address) values (N'Trần Thiên Phú','phuthienla@gmail.com','0352146587',N'Đồng Nai');
insert into Customer(c_name,email,phone,c_address) values (N'Vạn Minh Ty','tyruby@gmail.com','0125436987',N'Ninh Thuận');
insert into Customer(c_name,email,phone,c_address) values (N'Trần Đức Anh','anhtranbo@gmail.com','0358496528',N'Nghệ An');
insert into Customer(c_name,email,phone,c_address) values (N'Nguyễn Thị Ngọc Linh','linhngoc@gmail.com','0321654987',N'Bình Định');

-- Dịch vụ
insert into CusService values (N'Mineral Water', 10000);
insert into CusService values (N'Sandwich', 50000);
insert into CusService values (N'Cold Towel', 2000);
insert into CusService values (N'Pepsi', 20000);

-- Nhân viên ql dịch vụ
insert into Employee_Service values (1, N'Sandwich');
insert into Employee_Service values (2, N'Cold Towel');
insert into Employee_Service values (3, N'Pepsi');
insert into Employee_Service values (3, N'Mineral Water');

--Phòng
insert into Room(r_name,price,status) values (N'One People',400000,N'ready');
insert into Room(r_name,price,status) values (N'Two people',600000,N'ready');
insert into Room(r_name,price,status) values (N'Three people',700000,N'ready');
insert into Room(r_name,price,status) values (N'Four people',1000000,N'ready');

-- Khách hàng đặt phòng
insert into Booking values (1,2,'2023-05-01','2023-05-05');
insert into Booking values (2,1,'2023-04-30','2023-05-04');
insert into Booking values (3,3,'2023-04-29','2023-05-03');

-- Phòng đặt dịch vụ
insert into Room_Service values (3, N'Mineral Water');
insert into Room_Service values (2, N'Sandwich');
insert into Room_Service values (1, N'Cold Towel');

--Hóa đơn
insert into Bill(c_id,e_id,status) values (3,3,'unpaid');
insert into Bill(c_id,e_id,status) values (1,1,'unpaid');
insert into Bill(c_id,e_id,status) values (2,2,'unpaid');

-- Chi tiết hóa đơn
insert into DetailBill values (1, N'Mineral Water',5);
insert into DetailBill values (2, N'Sandwich',10);
insert into DetailBill values (3, N'Cold Towel',4);


--Trigger
-- Hóa đơn với giá phòng
go
CREATE Trigger tr_hd On Bill
for insert
AS
	Begin
		Declare @sohd int
		select  @sohd = i.id from inserted i
		update Bill
		set total = (select r.price from Room r, inserted i, Booking b where i.c_id = b.c_id and b.r_id = r.id)
		where id = @sohd
	End

-- tính tổng tiền của hd khi nhập 1 cthd
go
CREATE Trigger tr_cthd2 On DetailBill
for insert
AS
	Begin
		Declare @sohd int, @total float, @tendv varchar(50)
		select @tendv = i.s_name, @sohd = i.b_id, @total = b.total
		from inserted i, CusService cus, Bill b where i.s_name = cus.s_name and i.b_id = b.id
		update Bill
		set total = @total + (select (cus.price * i.amount) from CusService cus, inserted i where cus.s_name = i.s_name)
		where id = @sohd
	End


select * from room;
select * from Customer;
select * from CusService;
select * from employee;
select * from Employee_Service;
select * from Booking;
select * from Room_Service;
select * from Bill;
delete from cusservice

drop table DetailBill
drop table Bill