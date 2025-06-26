create database Tienda_en_linea

use [Tienda_en_linea]

CREATE TABLE Usuarios (
    UsuarioID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Contrasena NVARCHAR(255) NOT NULL,
    FechaRegistro DATETIME NOT NULL
);
ALTER TABLE Usuarios
ADD Apellido NVARCHAR(100) NULL;
UPDATE Usuarios SET Apellido = 'Choque' WHERE Apellido IS NULL;

ALTER TABLE Usuarios
ALTER COLUMN Apellido NVARCHAR(100) NOT NULL;


CREATE TABLE Direcciones (
    DireccionID INT PRIMARY KEY IDENTITY(1,1),
    UsuarioID INT FOREIGN KEY REFERENCES Usuarios(UsuarioID),
    Pais NVARCHAR(100) NOT NULL,
    Ciudad NVARCHAR(100) NOT NULL,
    CodigoPostal NVARCHAR(20),
    DireccionDetalle NVARCHAR(255) NOT NULL
);

CREATE TABLE Tiendas (
    TiendaID INT PRIMARY KEY IDENTITY(1,1),
    UsuarioID INT FOREIGN KEY REFERENCES Usuarios(UsuarioID),
    NombreTienda NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255),
    FechaCreacion DATETIME NOT NULL
);

CREATE TABLE Categorias (
    CategoriaID INT PRIMARY KEY IDENTITY(1,1),
    NombreCategoria NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255)
);

CREATE TABLE Productos (
    ProductoID INT PRIMARY KEY IDENTITY(1,1),
    TiendaID INT FOREIGN KEY REFERENCES Tiendas(TiendaID),
    CategoriaID INT FOREIGN KEY REFERENCES Categorias(CategoriaID),
    NombreProducto NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255),
    Precio DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL,
    Estado NVARCHAR(50) NOT NULL,
    FechaPublicacion DATETIME NOT NULL
);

CREATE TABLE Pedidos (
    PedidoID INT PRIMARY KEY IDENTITY(1,1),
    UsuarioID INT FOREIGN KEY REFERENCES Usuarios(UsuarioID),
    DireccionID INT FOREIGN KEY REFERENCES Direcciones(DireccionID),
    FechaPedido DATETIME NOT NULL,
    Estado NVARCHAR(50) NOT NULL
);

CREATE TABLE DetallePedido (
    DetalleID INT PRIMARY KEY IDENTITY(1,1),
    PedidoID INT FOREIGN KEY REFERENCES Pedidos(PedidoID),
    ProductoID INT FOREIGN KEY REFERENCES Productos(ProductoID),
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10,2) NOT NULL
);
--Indices
CREATE NONCLUSTERED INDEX IX_Usuarios_Email ON Usuarios(Email)
CREATE NONCLUSTERED INDEX IX_Productos_NombreProducto ON Productos(NombreProducto)
CREATE NONCLUSTERED INDEX IX_Productos_Estado_Fecha ON Productos(Estado)
CREATE NONCLUSTERED INDEX IX_Pedidos_UsuarioID ON Pedidos(UsuarioID)
CREATE NONCLUSTERED INDEX IX_Pedidos_Estado_Fecha ON Pedidos(Estado, FechaPedido)
CREATE NONCLUSTERED INDEX IX_DetallePedido_PedidoID ON DetallePedido(PedidoID)
CREATE NONCLUSTERED INDEX IX_DetallePedido_ProductoID ON DetallePedido(ProductoID)
CREATE NONCLUSTERED INDEX IX_Tiendas_UsuarioID ON Tiendas(UsuarioID)







--Poblacion

INSERT INTO Usuarios (Nombre, Email, Contrasena, FechaRegistro)
VALUES 
('Carlos Méndez', 'carlos@mail.com', 'clave123', '2025-05-29'),
('Laura Gómez', 'laura@mail.com', 'clave456', '2025-05-29');

INSERT INTO Direcciones (UsuarioID, Pais, Ciudad, CodigoPostal, DireccionDetalle)
VALUES 
(1, 'Bolivia', 'La Paz', '1234', 'Av. Busch #123, frente al colegio Don Bosco'),
(2, 'Bolivia', 'Cochabamba', '5678', 'Calle Sucre #456, zona norte');

INSERT INTO Tiendas (UsuarioID, NombreTienda, Descripcion, FechaCreacion)
VALUES 
(1, 'TecnoWorld', 'Tienda de tecnología y accesorios', '2025-05-29'),
(2, 'ModaCool', 'Ropa juvenil y moderna', '2025-05-29');

INSERT INTO Categorias (NombreCategoria, Descripcion)
VALUES 
('Electrónica', 'Dispositivos y accesorios tecnológicos'),
('Ropa', 'Prendas de vestir para todas las edades');

INSERT INTO Productos (TiendaID, CategoriaID, NombreProducto, Descripcion, Precio, Stock, Estado, FechaPublicacion)
VALUES 
(1, 1, 'Mouse Gamer RGB', 'Mouse ergonómico con luces LED', 150.00, 10, 'Activo', '2025-05-29'),
(2, 2, 'Polera Oversize', 'Polera de algodón talla única', 90.00, 25, 'Activo', '2025-05-29');

INSERT INTO Pedidos (UsuarioID, DireccionID, FechaPedido, Estado)
VALUES 
(2, 2, '2025-05-29', 'Pendiente');

INSERT INTO DetallePedido (PedidoID, ProductoID, Cantidad, PrecioUnitario)
VALUES 
(1, 1, 1, 150.00);

INSERT INTO Usuarios (Nombre, Email, Contrasena, FechaRegistro)
VALUES 
('Carlos Méndez', 'carlos@mail.com', 'clave123', '2025-05-29'),
('Laura Gómez', 'laura@mail.com', 'clave456', '2025-05-29'),
('Ana Rodríguez', 'ana@mail.com', 'clave789', '2025-05-29'),
('Luis Pérez', 'luis@mail.com', 'clave321', '2025-05-29');

INSERT INTO Direcciones (UsuarioID, Pais, Ciudad, CodigoPostal, DireccionDetalle)
VALUES 
(1, 'Bolivia', 'La Paz', '1234', 'Av. Busch #123'),
(2, 'Bolivia', 'Cochabamba', '5678', 'Calle Sucre #456'),
(3, 'Bolivia', 'Santa Cruz', '9012', 'Av. Cristo Redentor #789'),
(4, 'Bolivia', 'Oruro', '3456', 'Av. Villarroel #101');

INSERT INTO Tiendas (UsuarioID, NombreTienda, Descripcion, FechaCreacion)
VALUES 
(1, 'TecnoWorld', 'Tecnología avanzada', '2025-05-29'),
(2, 'ModaCool', 'Moda juvenil', '2025-05-29'),
(3, 'LibroManía', 'Librería virtual', '2025-05-29');

INSERT INTO Categorias (NombreCategoria, Descripcion)
VALUES 
('Electrónica', 'Dispositivos electrónicos'),
('Ropa', 'Ropa para todas las edades'),
('Libros', 'Todo tipo de libros y novelas');

INSERT INTO Productos (TiendaID, CategoriaID, NombreProducto, Descripcion, Precio, Stock, Estado, FechaPublicacion)
VALUES 
(1, 1, 'Mouse Gamer RGB', 'Mouse ergonómico con luces LED', 150.00, 10, 'Activo', '2025-05-29'),
(1, 1, 'Teclado Mecánico', 'Teclado retroiluminado con switches azules', 250.00, 5, 'Activo', '2025-05-29'),
(2, 2, 'Polera Oversize', 'Polera de algodón talla única', 90.00, 25, 'Activo', '2025-05-29'),
(2, 2, 'Jean Azul', 'Jean clásico corte slim', 180.00, 15, 'Activo', '2025-05-29'),
(3, 3, 'Cien Años de Soledad', 'Novela de Gabriel García Márquez', 120.00, 20, 'Activo', '2025-05-29'),
(3, 3, 'El Principito', 'Libro ilustrado para todas las edades', 80.00, 30, 'Activo', '2025-05-29');

INSERT INTO Pedidos (UsuarioID, DireccionID, FechaPedido, Estado)
VALUES 
(2, 2, '2025-05-29', 'Pendiente');

INSERT INTO Pedidos (UsuarioID, DireccionID, FechaPedido, Estado)
VALUES 
(3, 3, '2025-05-29', 'Pendiente');

INSERT INTO Pedidos (UsuarioID, DireccionID, FechaPedido, Estado)
VALUES 
(4, 4, '2025-05-29', 'Pendiente');

INSERT INTO DetallePedido (PedidoID, ProductoID, Cantidad, PrecioUnitario)
VALUES 
(1, 1, 1, 150.00), -- Mouse
(1, 5, 1, 120.00);  -- Cien Años de Soledad

INSERT INTO DetallePedido (PedidoID, ProductoID, Cantidad, PrecioUnitario)
VALUES 
(2, 3, 2, 90.00); -- 2 Poleras

INSERT INTO DetallePedido (PedidoID, ProductoID, Cantidad, PrecioUnitario)
VALUES 
(3, 2, 1, 250.00), -- Teclado
(3, 6, 2, 80.00);  -- 2 libros El Principito


--PRUEBAS VISTAS
	--mostrar datos pedido
	select *
	from mostrar_detalle_venta
	where Nro_factura = 1

	--mostrar productos por tienda
	select *
	from MostrarProductosPorTienda
	where TiendaID = 1

	--mostrar Pedidos por tienda
	select *
	from PedidosPorTienda
	where TiendaID = 1

--PRUEBAS SP
	--confirmar compra
	DECLARE @Carrito Carrito
	INSERT INTO @Carrito (ProductoID, Cantidad, PrecioUnitario)
	VALUES 
	(1, 6, 150.00)  
	EXEC ConfirmarCompra 
		@UsuarioID = 1, 
		@DireccionID = 1, 
		@Carrito = @Carrito;

	select *
	from Productos

	select *
	from DetallePedido



	select *
	from Pedidos as p
	inner join DetallePedido as dp
	on p.PedidoID = dp.PedidoID
	where p.PedidoID = 8
	--agregar producto
	EXEC AgregarProducto
    @TiendaID = 1,
    @CategoriaID = 2,
    @NombreProducto = 'Teclado inalambrico',
    @Descripcion = 'teclado óptico con conectividad Bluetooth y sensor de alta precisión',
    @Precio = 99.99,
    @Stock = 10,
    @Estado = 'Activo';

	select *
	from Productos
	--editar producto
	EXEC editarproducto
    @ProductoID = 5,
    @NombreProducto = 'Mouse ergonómico',
    @Descripcion = 'Mouse con diseño ergonómico y batería recargable',
    @Precio = 99.99,
    @Stock = 25,
    @Estado = 'Activo',
    @CategoriaID = 2;
	--inactivar producto
	EXEC inactivarproducto @ProductoID = 5;
	--cambiar estado pedido
	EXEC cambiarestadopedido
    @PedidoID = 8,
    @Estadonuevo = 'Cancelado'; --Entregado Pendiente Cancelado
	
	select *
	from Pedidos
	--agregar usuario
	EXEC RegistrarUsuario
    @Nombre = 'Marco',
    @Apellido = 'Rodríguez',
    @Email = 'marco.rodriguez@example.com',
    @Contrasena = 'claveSegura123';
	
	select *
	from Usuarios
	--agregar direccion
	EXEC AgregarDireccion
    @UsuarioID = 1,
    @Pais = 'Bolivia',
    @Ciudad = 'Cochabamba',
    @CodigoPostal = '591CBBA',
    @DireccionDetalle = 'Av. América, Edificio El Prado, piso 3';

	select *
	from Direcciones
	--Buscar producto por categoria y nombre
	EXEC BuscarProducto
	@BUSQUEDA = 'elec'

	select *
	from Categorias

	--Agregar tienda
	
	select *
	from Tiendas

	EXEC CrearTienda
	@UsuarioID = 1, 
	@NombreTienda = 'Tienda Amiga',
	@Descripcion = 'venta de articulos para el hogar'

	EXEC CrearTienda
	@UsuarioID = 3,
	@NombreTienda = 'TecnoWorld',
	@Descripcion = 'venta de articulos para el hogar'

--PRUEBAS TRIGGERS
	SELECT ProductoID, NombreProducto, Stock
	FROM Productos
	WHERE ProductoID = 1; -- Usa el ID del producto que vas a probar
	
	select *
	from Pedidos
	--6

	DECLARE @Carrito Carrito;
	INSERT INTO @Carrito (ProductoID, Cantidad, PrecioUnitario)
	VALUES (1, 2, 100.00);

	EXEC confirmarcompra @UsuarioID = 1, @DireccionID = 1, @Carrito = @Carrito;

	UPDATE Pedidos
	SET Estado = 'Cancelado'
	WHERE PedidoID = 7;


--EXECUTION PLAN
-- Query para evaluar eficiencia
SELECT 
    p.PedidoID,
    p.FechaPedido,
    p.Estado,
    u.Nombre + ' ' + u.Apellido AS Comprador,
    pr.NombreProducto,
    dp.Cantidad,
    dp.PrecioUnitario,
    t.NombreTienda
FROM Pedidos p
JOIN Usuarios u ON p.UsuarioID = u.UsuarioID
JOIN DetallePedido dp ON p.PedidoID = dp.PedidoID
JOIN Productos pr ON dp.ProductoID = pr.ProductoID
JOIN Tiendas t ON pr.TiendaID = t.TiendaID
WHERE u.Nombre LIKE 'L%' AND p.Estado = 'Pendiente';



