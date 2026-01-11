-- SCRIPT DE BASE DE DATOS 

DROP DATABASE IF EXISTS EMPRESA_NEO;
CREATE DATABASE EMPRESA_NEO;
GO

USE EMPRESA_NEO;
GO

-- TABLAS PRINCIPALES

-- Productos
CREATE TABLE Productos(
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Nombre VARCHAR(50),
	Precio FLOAT,
	Stock INT,
	Estado INT DEFAULT 1,
	FechaRegistro DATETIME DEFAULT GETDATE()
);

-- Clientes
CREATE TABLE Clientes(
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Nombre VARCHAR(50),
	Email VARCHAR(50),
	Estado INT DEFAULT 1,
	FechaRegistro DATETIME DEFAULT GETDATE()
);

-- Ventas
CREATE TABLE Ventas(
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Fecha DATE,
	ClienteId INT NOT NULL,
	Estado INT DEFAULT 1,
	FechaRegistro DATETIME DEFAULT GETDATE(),
	CONSTRAINT FK_CLIENTE FOREIGN KEY (ClienteId) REFERENCES Clientes(Id)
);

-- Detalle de Ventas
CREATE TABLE DetalleVenta(
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	VentaId INT NOT NULL,
	ProductoId INT NOT NULL,
	Cantidad INT,
	PrecioUnitario FLOAT,
	Estado INT DEFAULT 1,
	FechaRegistro DATETIME DEFAULT GETDATE(),
	CONSTRAINT FK_VENTA FOREIGN KEY (VentaId) REFERENCES Ventas(Id),
	CONSTRAINT FK_PRODUCTO FOREIGN KEY (ProductoId) REFERENCES Productos(Id)
);

-- TABLAS DE SEGURIDAD Y ADMINISTRACIÓN

-- Roles
CREATE TABLE Roles(
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Nombre VARCHAR(50) NOT NULL,
	Estado INT DEFAULT 1,
	FechaRegistro DATETIME DEFAULT GETDATE()
);

-- Usuarios
CREATE TABLE Usuarios(
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Nombre VARCHAR(100) NOT NULL,
	Email VARCHAR(100) NOT NULL UNIQUE,
	Password VARCHAR(MAX) NOT NULL,
	Estado INT DEFAULT 1,
	FechaRegistro DATETIME DEFAULT GETDATE()
);

--Roles por usuario
CREATE TABLE UsuarioRol(
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	UsuarioId INT NOT NULL,
	RolId INT NOT NULL,
	Estado INT DEFAULT 1,
	FechaRegistro DATETIME DEFAULT GETDATE(),
	CONSTRAINT FK_USUARIOROL_USUARIO FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id),
	CONSTRAINT FK_USUARIOROL_ROL FOREIGN KEY (RolId) REFERENCES Roles(Id)
);

-- Menus
CREATE TABLE Menus(
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	MenuPadreId INT NULL,
	Nombre VARCHAR(50) NOT NULL,
	Url VARCHAR(200) NULL,
	Icono VARCHAR(50) NULL,
	Orden INT DEFAULT 0,
	Estado INT DEFAULT 1,
	FechaRegistro DATETIME DEFAULT GETDATE(),
	CONSTRAINT FK_MENU_PADRE FOREIGN KEY (MenuPadreId) REFERENCES Menus(Id)
);

-- Menus por roles
CREATE TABLE MenuRol(
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	MenuId INT NOT NULL,
	RolId INT NOT NULL,
	Estado INT DEFAULT 1,
	FechaRegistro DATETIME DEFAULT GETDATE(),
	CONSTRAINT FK_MENUROL_MENU FOREIGN KEY (MenuId) REFERENCES Menus(Id),
	CONSTRAINT FK_MENUROL_ROL FOREIGN KEY (RolId) REFERENCES Roles(Id)
);

-- Imagenes carrusel
CREATE TABLE ImagenesDashboard (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Url VARCHAR(MAX) NOT NULL,
    Estado INT DEFAULT 1,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    Correlativo INT
);


-- INSERTS

-- Roles 
INSERT INTO Roles(Nombre) VALUES ('Administrador');
INSERT INTO Roles(Nombre) VALUES ('Vendedor');
INSERT INTO Roles(Nombre) VALUES ('Supervisor');

-- Usuarios 
INSERT INTO Usuarios(Nombre, Email, Password) VALUES ('Admin', 'admin@neonet.com', '123');
INSERT INTO Usuarios(Nombre, Email, Password) VALUES ('Vendedor 1', 'vendedor@neonet.com', '123');
INSERT INTO Usuarios(Nombre, Email, Password) VALUES ('Usuario 2', 'multirol@neonet.com', '123');

-- Roles por usuarios
INSERT INTO UsuarioRol(UsuarioId, RolId) VALUES (1, 1); 
INSERT INTO UsuarioRol(UsuarioId, RolId) VALUES (2, 2); 
INSERT INTO UsuarioRol(UsuarioId, RolId) VALUES (3, 3); 

-- Productos
INSERT INTO Productos (Nombre, Precio, Stock, Estado, FechaRegistro)
VALUES ('Agua', 5, 0, 1, '2026-01-10');

INSERT INTO Productos (Nombre, Precio, Stock, Estado, FechaRegistro)
VALUES ('Arroz', 10, 17, 1, '2026-01-10');

INSERT INTO Productos ( Nombre, Precio, Stock, Estado, FechaRegistro)
VALUES ('Frijol', 9, 0, 1, '2026-01-10');

INSERT INTO Productos ( Nombre, Precio, Stock, Estado, FechaRegistro)
VALUES ('Azucar', 6, 12, 1, '2026-01-10');

INSERT INTO Productos (Nombre, Precio, Stock, Estado, FechaRegistro)
VALUES ('Coca Cola', 10, 4, 1, '2026-01-10');

-- Clientes
INSERT INTO Clientes(Nombre,Email) VALUES('Ricardo Arrecis','rarrecis@gmail.com');
INSERT INTO Clientes(Nombre,Email) VALUES('Ricardo Rivera','rarrecis2@gmail.com');
INSERT INTO Clientes(Nombre,Email) VALUES('Enrique Arrecis','rarreci3@gmail.com');
INSERT INTO Clientes(Nombre,Email) VALUES('Enrique Rivera','rarrecis4@gmail.com');

-- Ventas 
INSERT INTO Ventas (Fecha, ClienteId, Estado, FechaRegistro)
VALUES ('2025-01-04', 1, 1, '2026-01-10');

INSERT INTO Ventas (Fecha, ClienteId, Estado, FechaRegistro)
VALUES ('2025-01-03', 1, 1, '2026-01-10');

INSERT INTO Ventas (Fecha, ClienteId, Estado, FechaRegistro)
VALUES ('2025-01-02', 1, 1, '2026-01-10');

INSERT INTO Ventas (Fecha, ClienteId, Estado, FechaRegistro)
VALUES ('2025-01-01', 1, 1, '2026-01-10');

INSERT INTO Ventas (Fecha, ClienteId, Estado, FechaRegistro)
VALUES ('2025-01-04', 2, 1, '2026-01-10');

INSERT INTO Ventas (Fecha, ClienteId, Estado, FechaRegistro)
VALUES ('2025-01-03', 2, 1, '2026-01-10');

INSERT INTO Ventas (Fecha, ClienteId, Estado, FechaRegistro)
VALUES ('2025-01-02', 3, 1, '2026-01-10');

INSERT INTO Ventas (Fecha, ClienteId, Estado, FechaRegistro)
VALUES ('2025-01-01', 4, 1, '2026-01-10');

INSERT INTO Ventas (Fecha, ClienteId, Estado, FechaRegistro)
VALUES ('2026-01-10', 2, 1, '2026-01-10');

INSERT INTO Ventas (Fecha, ClienteId, Estado, FechaRegistro)
VALUES ('2026-01-10', 2, 1, '2026-01-10');

INSERT INTO Ventas (Fecha, ClienteId, Estado, FechaRegistro)
VALUES ('2026-01-10', 2, 1, '2026-01-10');

INSERT INTO Ventas (Fecha, ClienteId, Estado, FechaRegistro)
VALUES ('2026-01-10', 2, 1, '2026-01-10');


-- Detalle de Ventas
INSERT INTO DetalleVenta (VentaId, ProductoId, Cantidad, PrecioUnitario, Estado, FechaRegistro)
VALUES (1, 1, 2, 6, 1, '2026-01-10');

INSERT INTO DetalleVenta (VentaId, ProductoId, Cantidad, PrecioUnitario, Estado, FechaRegistro)
VALUES (1, 2, 5, 8, 1, '2026-01-10');

INSERT INTO DetalleVenta (VentaId, ProductoId, Cantidad, PrecioUnitario, Estado, FechaRegistro)
VALUES (1, 3, 2, 9, 1, '2026-01-10');

INSERT INTO DetalleVenta (VentaId, ProductoId, Cantidad, PrecioUnitario, Estado, FechaRegistro)
VALUES (1, 4, 5, 10, 1, '2026-01-10');

INSERT INTO DetalleVenta (VentaId, ProductoId, Cantidad, PrecioUnitario, Estado, FechaRegistro)
VALUES (2, 2, 2, 7, 1, '2026-01-10');

INSERT INTO DetalleVenta (VentaId, ProductoId, Cantidad, PrecioUnitario, Estado, FechaRegistro)
VALUES (3, 3, 2, 9, 1, '2026-01-10');

INSERT INTO DetalleVenta (VentaId, ProductoId, Cantidad, PrecioUnitario, Estado, FechaRegistro)
VALUES (5, 4, 2, 5, 1, '2026-01-10');

INSERT INTO DetalleVenta (VentaId, ProductoId, Cantidad, PrecioUnitario, Estado, FechaRegistro)
VALUES (6, 1, 2, 10, 1, '2026-01-10');

INSERT INTO DetalleVenta (VentaId, ProductoId, Cantidad, PrecioUnitario, Estado, FechaRegistro)
VALUES (9, 1, 10, 5, 1, '2026-01-10');

INSERT INTO DetalleVenta (VentaId, ProductoId, Cantidad, PrecioUnitario, Estado, FechaRegistro)
VALUES (9, 4, 27, 6, 1, '2026-01-10');

INSERT INTO DetalleVenta (VentaId, ProductoId, Cantidad, PrecioUnitario, Estado, FechaRegistro)
VALUES (9, 3, 25, 9, 1, '2026-01-10');

INSERT INTO DetalleVenta (VentaId, ProductoId, Cantidad, PrecioUnitario, Estado, FechaRegistro)
VALUES (9, 2, 1, 10, 1, '2026-01-10');

INSERT INTO DetalleVenta (VentaId, ProductoId, Cantidad, PrecioUnitario, Estado, FechaRegistro)
VALUES (10, 2, 1, 10, 1, '2026-01-10');

INSERT INTO DetalleVenta (VentaId, ProductoId, Cantidad, PrecioUnitario, Estado, FechaRegistro)
VALUES (11, 2, 1, 10, 1, '2026-01-10');

INSERT INTO DetalleVenta (VentaId, ProductoId, Cantidad, PrecioUnitario, Estado, FechaRegistro)
VALUES (12, 4, 1, 6, 1, '2026-01-10');

-- Menus 

INSERT INTO Menus (MenuPadreId, Nombre, Url, Icono, Orden, Estado, FechaRegistro)
VALUES (NULL, 'Inicio', '/Home/Index', 'home', 1, 1, '2026-01-10');

INSERT INTO Menus (MenuPadreId, Nombre, Url, Icono, Orden, Estado, FechaRegistro)
VALUES (NULL, 'Ventas', NULL, 'shopping-cart', 2, 1, '2026-01-10');

INSERT INTO Menus (MenuPadreId, Nombre, Url, Icono, Orden, Estado, FechaRegistro)
VALUES (2, 'Nueva Venta', '/Ventas/Nueva', 'plus-circle', 1, 1, '2026-01-10');

INSERT INTO Menus (MenuPadreId, Nombre, Url, Icono, Orden, Estado, FechaRegistro)
VALUES (2, 'Historial Ventas', '/Ventas/Index', 'list', 2, 1, '2026-01-10');

INSERT INTO Menus (MenuPadreId, Nombre, Url, Icono, Orden, Estado, FechaRegistro)
VALUES (NULL, 'Mantenimiento', NULL, 'tools', 3, 1, '2026-01-10');

INSERT INTO Menus (MenuPadreId, Nombre, Url, Icono, Orden, Estado, FechaRegistro)
VALUES (5, 'Usuarios', '/Usuarios/Index', 'users', 4, 1, '2026-01-10');

INSERT INTO Menus (MenuPadreId, Nombre, Url, Icono, Orden, Estado, FechaRegistro)
VALUES (2, 'Ventas Cliente', '/Ventas/VentasCliente', 'users', 3, 1, '2026-01-10');

INSERT INTO Menus (MenuPadreId, Nombre, Url, Icono, Orden, Estado, FechaRegistro)
VALUES (5, 'Productos', '/Productos/Index', 'box', 2, 1, '2026-01-10');


-- Menus por roles
INSERT INTO MenuRol (MenuId, RolId, Estado, FechaRegistro)
VALUES (1, 1, 1, '2026-01-10');

INSERT INTO MenuRol (MenuId, RolId, Estado, FechaRegistro)
VALUES (2, 1, 1, '2026-01-10');

INSERT INTO MenuRol (MenuId, RolId, Estado, FechaRegistro)
VALUES (3, 1, 1, '2026-01-10');

INSERT INTO MenuRol (MenuId, RolId, Estado, FechaRegistro)
VALUES (4, 1, 1, '2026-01-10');

INSERT INTO MenuRol (MenuId, RolId, Estado, FechaRegistro)
VALUES (5, 1, 1, '2026-01-10');

INSERT INTO MenuRol (MenuId, RolId, Estado, FechaRegistro)
VALUES (6, 1, 1, '2026-01-10');

INSERT INTO MenuRol (MenuId, RolId, Estado, FechaRegistro)
VALUES (1, 2, 1, '2026-01-10');

INSERT INTO MenuRol (MenuId, RolId, Estado, FechaRegistro)
VALUES (2, 2, 1, '2026-01-10');

INSERT INTO MenuRol (MenuId, RolId, Estado, FechaRegistro)
VALUES (3, 2, 1, '2026-01-10');

INSERT INTO MenuRol (MenuId, RolId, Estado, FechaRegistro)
VALUES (4, 2, 1, '2026-01-10');

INSERT INTO MenuRol (MenuId, RolId, Estado, FechaRegistro)
VALUES (7, 1, 1, '2026-01-10');

INSERT INTO MenuRol (MenuId, RolId, Estado, FechaRegistro)
VALUES (8, 1, 1, '2026-01-10');

INSERT INTO MenuRol (MenuId, RolId, Estado, FechaRegistro)
VALUES (7, 2, 1, '2026-01-10');


-- Imagenes 
INSERT INTO ImagenesDashboard (Url, Estado, FechaRegistro, Correlativo)
VALUES ('https://images.unsplash.com/photo-1460925895917-afdab827c52f?ixlib=rb-1.2.1&auto=format&fit=crop&w=1350&q=80', 1, '2026-01-10', 1);

INSERT INTO ImagenesDashboard (Url, Estado, FechaRegistro, Correlativo)
VALUES ('https://images.unsplash.com/photo-1557804506-669a67965ba0?ixlib=rb-1.2.1&auto=format&fit=crop&w=1350&q=80', 1, '2026-01-10', 2);

INSERT INTO ImagenesDashboard (Url, Estado, FechaRegistro, Correlativo)
VALUES ('https://images.unsplash.com/photo-1542744173-8e7e53415bb0?ixlib=rb-1.2.1&auto=format&fit=crop&w=1350&q=80', 1, '2026-01-10', 3);

INSERT INTO ImagenesDashboard (Url, Estado, FechaRegistro, Correlativo)
VALUES ('https://neonet.com.gt/img/web_site/logos/neopagos_purple_left.png', 1, '2026-01-10', 4);
