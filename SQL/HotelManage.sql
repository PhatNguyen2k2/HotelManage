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
	love int,
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
	e_id int not null,
	price float,
	constraint fk_cusService foreign key (e_id) references Employee(id)
)
create table RoomService(
	r_id int not null,
	s_name nvarchar(50) not null,
	amount int,
	primary key (r_id, s_name),
	constraint fk_roomService1 foreign key (r_id) references Room(id),
	constraint fk_roomService2 foreign key (s_name) references CusService(s_name)
)
create table Bill(
	id int not null primary key identity(1,1),
	c_id int not null,
	e_id int not null,
	status nvarchar(20),
	b_date date default GETDATE(),
	total float default 0,
	constraint fk_bill1 foreign key (c_id) references Customer(id),
	constraint fk_bill3 foreign key (e_id) references Employee(id)
)
create table Account(
	username nvarchar(50) not null primary key,
	password varchar(200),
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
insert into CusService values (N'Mineral Water', 1, 10000);
insert into CusService values (N'Sandwich', 2, 50000);
insert into CusService values (N'Cold Towel', 3, 2000);
insert into CusService values (N'Pepsi', 1, 20000);

--Phòng
insert into Room(r_name,price,love,status) values (N'One People',400000,0,N'ready');
insert into Room(r_name,price,love,status) values (N'Two people',600000,0,N'ready');
insert into Room(r_name,price,love,status) values (N'Three people',700000,0,N'ready');
insert into Room(r_name,price,love,status) values (N'Four people',1000000,0,N'ready');

-- Khách hàng đặt phòng
insert into Booking values (1,2,'2023-05-01','2023-05-05');
insert into Booking values (2,1,'2023-04-30','2023-05-04');
insert into Booking values (3,3,'2023-04-29','2023-05-03');

-- Phòng đặt dịch vụ
insert into RoomService values (3, N'Mineral Water', 5);
insert into RoomService values (2, N'Sandwich', 10);
insert into RoomService values (1, N'Cold Towel', 4);

--Hóa đơn
insert into Bill(c_id,e_id,status) values (3,3,'unpaid');
insert into Bill(c_id,e_id,status) values (1,1,'unpaid');
insert into Bill(c_id,e_id,status) values (2,2,'unpaid');

--Tài khoản
insert into Account values('admin','12345');
insert into Account values('emp','12345');
--Trigger
-- Hóa đơn với giá phòng
go
CREATE Trigger tr_hd On Bill
for insert
AS
	Begin
		Declare @sohd int, @price float
		select  @sohd = i.id, @price = (select sum((cs.price * rs.amount)) from CusService cs, RoomService rs, Booking b where b.c_id = i.c_id
										and rs.r_id = b.r_id and cs.s_name = rs.s_name) from inserted i
		update Bill
		set total = (select r.price from Room r, inserted i, Booking b where i.c_id = b.c_id and b.r_id = r.id) + @price
		where id = @sohd
	End
-- Cộng số lượng khi trùng tên dịch vụ
go
CREATE Trigger tr_rs On RoomService
for insert
AS
	Begin
		Declare @amount int, @id int, @name nvarchar(50)
		select @id = i.r_id, @name = i.s_name,  @amount = (select rs.amount from RoomService rs where i.r_id = rs.r_id and i.s_name = rs.s_name) from inserted i
		update RoomService		
		set amount = @amount + (select i.amount from inserted i, RoomService rs where i.r_id = rs.r_id and i.s_name = rs.s_name)
		where r_id = @id and s_name = @name
	End

select * from room;
select * from Customer;
select * from CusService;
select * from employee;
select * from Booking;
select * from RoomService;
select * from Bill;
delete from Bill
drop table Bill
update Room set love = 20 where id = 1
drop trigger tr_rs