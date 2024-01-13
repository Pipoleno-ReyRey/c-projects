create database TiendaDeRopa;
use TiendaDeRopa;

create table Ropa(
id int primary key auto_increment,
nombre varchar(100),
precio float,
tallas varchar(50),
cantidad int
);

insert into ropa(nombre,precio, tallas, cantidad) values ('basic t-shirt',130, 'S,M,L', 140),
('dress shirts', 200, 'S,M,L,XL', 120),
('elegant blouses', 235, 'M,L,XL', 44),
('jeans', 205, 'S,M,L,XL,XXL', 110),
('dress pants', 235, 'S,M,L', 90),
('skirts', 100, 'S,M,L', 200),
('coats', 120, 'M,L', 50),
('underwear', 30, 'M,L,XL', 222),
('sleepwear', 75, 'M,L,XL', 143),
('sports sets', 500, 'S,M,L,XL,XXL', 213),
('swimsuits', 200, 'S,M,L', 44),
('hats', 65, 'S,M', 210);

select * from ropa;

create table VentaRecibo(
id int primary key auto_increment,
nombreComprador varchar(100),
prenda varchar(100),
talla varchar(65),
ropa int,
cantidadPrendas int,
foreign key (ropa) references ropa(id),
numRecibo long
);

select * from ventarecibo;
truncate VentaRecibo;

create table Administrador(
id int primary key auto_increment,
usuario varchar(100),
clave varchar(50)
);

insert into Administrador(usuario, clave) values ('administrator', '123456789');