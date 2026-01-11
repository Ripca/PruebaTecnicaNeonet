-- PROCEDIMIENTOS ALMACENADOS 
USE EMPRESA_NEO;
GO

-- QUERIES SOLICITADOS HECHOS EN SP PARA LUEO REPRESARLOS EN LA API*

-- 1. Total Vendido por Cliente
CREATE OR ALTER PROCEDURE sp_ReporteTotalVendidoPorCliente
AS
BEGIN
    SELECT 
        C.Nombre AS Cliente,
        SUM(D.Cantidad * D.PrecioUnitario) AS TotalVendido
    FROM Clientes C
    INNER JOIN Ventas V ON C.Id = V.ClienteId
    INNER JOIN DetalleVenta D ON V.Id = D.VentaId
    WHERE C.Estado = 1 AND V.Estado = 1 AND D.Estado = 1
    GROUP BY C.Nombre;
END
GO

-- 2. Productos Más Vendidos
CREATE OR ALTER PROCEDURE sp_ReporteProductosMasVendidos
AS
BEGIN
    SELECT TOP 5
        P.Nombre AS Producto,
        SUM(D.Cantidad) AS CantidadTotal
    FROM Productos P
    INNER JOIN DetalleVenta D ON P.Id = D.ProductoId
    WHERE P.Estado = 1 AND D.Estado = 1
    GROUP BY P.Nombre
    ORDER BY CantidadTotal DESC;
END
GO

--- 3. EL DE OBTENER EL STOCK DE PRODUCTOS SE OBTIENE SOLO HACIENDO EL SIGUIENTE QUERY. QUE TAMBIEN ES EL QUE UTILIZO PARA LISTAR LOS PRODUCTOS
-- Listar Productos
 --   SELECT Id, Nombre, Precio, Stock, Estado, FechaRegistro 
 --   FROM Productos;
---

-- Validar Usuario 
CREATE OR ALTER PROCEDURE sp_ValidarUsuario
    @Email VARCHAR(100),
    @Password VARCHAR(MAX)
AS
BEGIN
    SELECT 
        U.Id,
        U.Nombre,
        U.Email,
        U.Estado,
        U.FechaRegistro,
        STUFF((
            SELECT ',' + CAST(UR.RolId AS VARCHAR)
            FROM UsuarioRol UR
            INNER JOIN Roles R ON UR.RolId = R.Id
            WHERE UR.UsuarioId = U.Id AND UR.Estado = 1 AND R.Estado = 1
            FOR XML PATH('')), 1, 1, '') AS RolesIds,
        STUFF((
            SELECT ', ' + R.Nombre
            FROM UsuarioRol UR
            INNER JOIN Roles R ON UR.RolId = R.Id
            WHERE UR.UsuarioId = U.Id AND UR.Estado = 1 AND R.Estado = 1
            FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS RolNombre
    FROM Usuarios U
    WHERE U.Email = @Email 
        AND U.Password = @Password 
        AND U.Estado = 1
        AND EXISTS (
            SELECT 1 FROM UsuarioRol UR 
            INNER JOIN Roles R ON UR.RolId = R.Id
            WHERE UR.UsuarioId = U.Id AND UR.Estado = 1 AND R.Estado = 1
        );
END
GO

-- Obtener Menús por Usuario
CREATE OR ALTER PROCEDURE sp_ObtenerMenusPorUsuario
    @UsuarioId INT
AS
BEGIN
    SELECT DISTINCT
        M.Id,
        M.MenuPadreId,
        M.Nombre,
        M.Url,
        M.Icono,
        M.Orden,	
        M.Estado,
        M.FechaRegistro
    FROM Usuarios U
    INNER JOIN UsuarioRol UR ON U.Id = UR.UsuarioId
    INNER JOIN MenuRol MR ON UR.RolId = MR.RolId
    INNER JOIN Menus M ON MR.MenuId = M.Id
    WHERE U.Id = @UsuarioId 
        AND U.Estado = 1
        AND UR.Estado = 1
        AND MR.Estado = 1
        AND M.Estado = 1
    ORDER BY M.Orden;
END
GO

-- Listar Usuarios
CREATE OR ALTER PROCEDURE sp_ListarUsuarios
AS
BEGIN
    SELECT 
        U.Id,
        U.Nombre,
        U.Email,
        U.Estado,
        U.FechaRegistro,
        STUFF((
            SELECT ',' + CAST(UR.RolId AS VARCHAR)
            FROM UsuarioRol UR
            INNER JOIN Roles R ON UR.RolId = R.Id
            WHERE UR.UsuarioId = U.Id AND UR.Estado = 1 AND R.Estado = 1
            FOR XML PATH('')), 1, 1, '') AS RolesIds,
        STUFF((
            SELECT ', ' + R.Nombre
            FROM UsuarioRol UR
            INNER JOIN Roles R ON UR.RolId = R.Id
            WHERE UR.UsuarioId = U.Id AND UR.Estado = 1 AND R.Estado = 1
            FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS RolNombre
    FROM Usuarios U
    ORDER BY U.FechaRegistro DESC;
END
GO

-- Obtener Usuario por id
CREATE OR ALTER PROCEDURE sp_ObtenerUsuarioPorId
    @Id INT
AS
BEGIN
    SELECT 
        U.Id,
        U.Nombre,
        U.Email,
        U.Estado,
        U.FechaRegistro,
        STUFF((
            SELECT ',' + CAST(UR.RolId AS VARCHAR)
            FROM UsuarioRol UR
            INNER JOIN Roles R ON UR.RolId = R.Id
            WHERE UR.UsuarioId = U.Id AND UR.Estado = 1 AND R.Estado = 1
            FOR XML PATH('')), 1, 1, '') AS RolesIds,
        STUFF((
            SELECT ', ' + R.Nombre
            FROM UsuarioRol UR
            INNER JOIN Roles R ON UR.RolId = R.Id
            WHERE UR.UsuarioId = U.Id AND UR.Estado = 1 AND R.Estado = 1
            FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS RolNombre
    FROM Usuarios U
    WHERE U.Id = @Id;
END
GO

-- Crear Usuario
CREATE OR ALTER PROCEDURE sp_CrearUsuario
    @Nombre VARCHAR(100),
    @Email VARCHAR(100),
    @Password VARCHAR(MAX),
    @RolesIds VARCHAR(MAX),
    @NuevoId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS(SELECT 1 FROM Usuarios WHERE Email = @Email)
    BEGIN
        SET @NuevoId = -1;
        RETURN;
    END
    
    INSERT INTO Usuarios(Nombre, Email, Password)
    VALUES (@Nombre, @Email, @Password);
    
    SET @NuevoId = SCOPE_IDENTITY();
    
    IF @RolesIds IS NOT NULL AND @RolesIds != ''
    BEGIN
        INSERT INTO UsuarioRol(UsuarioId, RolId)
        SELECT @NuevoId, value
        FROM STRING_SPLIT(@RolesIds, ',')
        WHERE RTRIM(value) != '';
    END
END
GO

-- Actualizar Usuario
CREATE OR ALTER PROCEDURE sp_ActualizarUsuario
    @Id INT,
    @Nombre VARCHAR(100),
    @Email VARCHAR(100),
    @Password VARCHAR(MAX) = NULL,
    @RolesIds VARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @Password IS NOT NULL AND @Password != ''
    BEGIN
        UPDATE Usuarios
        SET Nombre = @Nombre,
            Email = @Email,
            Password = @Password
        WHERE Id = @Id;
    END
    ELSE
    BEGIN
        UPDATE Usuarios
        SET Nombre = @Nombre,
            Email = @Email
        WHERE Id = @Id;
    END
    
    IF @RolesIds IS NOT NULL
    BEGIN
        DELETE FROM UsuarioRol WHERE UsuarioId = @Id;
        
        IF @RolesIds != ''
        BEGIN
            INSERT INTO UsuarioRol(UsuarioId, RolId)
            SELECT @Id, value
            FROM STRING_SPLIT(@RolesIds, ',')
            WHERE RTRIM(value) != '';
        END
    END
END
GO

-- Cambiar Estado Usuario
CREATE OR ALTER PROCEDURE sp_CambiarEstadoUsuario
    @Id INT,
    @Estado INT
AS
BEGIN
    UPDATE Usuarios
    SET Estado = @Estado
    WHERE Id = @Id;
END
GO

-- Listar Menús
CREATE OR ALTER PROCEDURE sp_ListarMenus
AS
BEGIN
    SELECT 
        Id,
        MenuPadreId,
        Nombre,
        Url,
        Icono,
        Orden,
        Estado,
        FechaRegistro
    FROM Menus
    ORDER BY Orden;
END
GO

-- Obtener Menú por id
CREATE OR ALTER PROCEDURE sp_ObtenerMenuPorId
    @Id INT
AS
BEGIN
    SELECT 
        Id,
        MenuPadreId,
        Nombre,
        Url,
        Icono,
        Orden,
        Estado,
        FechaRegistro
    FROM Menus
    WHERE Id = @Id;
END
GO

-- Listar Productos
CREATE OR ALTER PROCEDURE sp_ListarProductos
AS
BEGIN
    SELECT Id, Nombre, Precio, Stock, Estado, FechaRegistro 
    FROM Productos;
END
GO

-- Obtener Producto por id
CREATE OR ALTER PROCEDURE sp_ObtenerProductoPorId
    @Id INT
AS
BEGIN
    SELECT Id, Nombre, Precio, Stock, Estado, FechaRegistro 
    FROM Productos 
    WHERE Id = @Id;
END
GO

-- Crear Producto
CREATE OR ALTER PROCEDURE sp_CrearProducto
    @Nombre VARCHAR(50),
    @Precio FLOAT,
    @Stock INT,
    @NuevoId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Productos(Nombre, Precio, Stock)
    VALUES (@Nombre, @Precio, @Stock);
    
    SET @NuevoId = SCOPE_IDENTITY();
END
GO

-- Actualizar Producto
CREATE OR ALTER PROCEDURE sp_ActualizarProducto
    @Id INT,
    @Nombre VARCHAR(50),
    @Precio FLOAT,
    @Stock INT
AS
BEGIN
    UPDATE Productos
    SET Nombre = @Nombre,
        Precio = @Precio,
        Stock = @Stock
    WHERE Id = @Id;
END
GO

-- Cambiar Estado Producto
CREATE OR ALTER PROCEDURE sp_CambiarEstadoProducto
    @Id INT,
    @Estado INT
AS
BEGIN
    UPDATE Productos
    SET Estado = @Estado
    WHERE Id = @Id;
END
GO

-- Listar Clientes
CREATE OR ALTER PROCEDURE sp_ListarClientes
AS
BEGIN
    SELECT Id, Nombre, Email, Estado, FechaRegistro 
    FROM Clientes;
END
GO

-- Obtener Cliente por id
CREATE OR ALTER PROCEDURE sp_ObtenerClientePorId
    @Id INT
AS
BEGIN
    SELECT Id, Nombre, Email, Estado, FechaRegistro 
    FROM Clientes 
    WHERE Id = @Id;
END
GO

-- Obtener Cliente por Email
CREATE OR ALTER PROCEDURE sp_ObtenerClientePorEmail
    @Email VARCHAR(100)
AS
BEGIN
    SELECT Id, Nombre, Email, Estado, FechaRegistro 
    FROM Clientes 
    WHERE Email = @Email;
END
GO

-- Crear Cliente
CREATE OR ALTER PROCEDURE sp_CrearCliente
    @Nombre VARCHAR(50),
    @Email VARCHAR(50),
    @NuevoId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Clientes(Nombre, Email)
    VALUES (@Nombre, @Email);
    
    SET @NuevoId = SCOPE_IDENTITY();
END
GO

-- Actualizar Cliente
CREATE OR ALTER PROCEDURE sp_ActualizarCliente
    @Id INT,
    @Nombre VARCHAR(50),
    @Email VARCHAR(50)
AS
BEGIN
    UPDATE Clientes
    SET Nombre = @Nombre,
        Email = @Email
    WHERE Id = @Id;
END
GO

-- Cambiar Estado Cliente
CREATE OR ALTER PROCEDURE sp_CambiarEstadoCliente
    @Id INT,
    @Estado INT
AS
BEGIN
    UPDATE Clientes
    SET Estado = @Estado
    WHERE Id = @Id;
END
GO


-- Ingresar Venta
CREATE OR ALTER PROCEDURE sp_RegistrarVenta
    @ClienteId INT,
    @NuevoId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Ventas(Fecha, ClienteId)
    VALUES (GETDATE(), @ClienteId);
    
    SET @NuevoId = SCOPE_IDENTITY();
END
GO

-- Ingresar detalle de venta
CREATE OR ALTER PROCEDURE sp_RegistrarDetalleVenta
    @VentaId INT,
    @ProductoId INT,
    @Cantidad INT,
    @PrecioUnitario FLOAT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO DetalleVenta(VentaId, ProductoId, Cantidad, PrecioUnitario)
    VALUES (@VentaId, @ProductoId, @Cantidad, @PrecioUnitario);
    
    UPDATE Productos
    SET Stock = Stock - @Cantidad
    WHERE Id = @ProductoId;
END
GO

-- Listar Todas las Ventas calculando total
CREATE OR ALTER PROCEDURE sp_ListarTodasLasVentas
AS
BEGIN
    SELECT 
        V.Id AS VentaId,
        V.Fecha,
        C.Nombre AS Cliente,
        SUM(D.Cantidad * D.PrecioUnitario) AS Total
    FROM Ventas V
    INNER JOIN Clientes C ON V.ClienteId = C.Id
    LEFT JOIN DetalleVenta D ON V.Id = D.VentaId
    WHERE V.Estado = 1
    GROUP BY V.Id, V.Fecha, C.Nombre
    ORDER BY V.Fecha DESC;
END
GO

-- Listar Ventas 
CREATE OR ALTER PROCEDURE sp_ListarVentas
AS
BEGIN
    SELECT 
        V.Id, 
        V.Fecha, 
        V.ClienteId, 
        C.Nombre AS ClienteNombre,
        V.Estado, 
        V.FechaRegistro
    FROM Ventas V
    INNER JOIN Clientes C ON V.ClienteId = C.Id
    WHERE V.Estado = 1
    ORDER BY V.Fecha DESC;
END
GO

-- Obtener Ventas por Cliente
CREATE OR ALTER PROCEDURE usp_ObtenerVentasPorCliente
    @ClienteId INT
AS
BEGIN
    SELECT 
        v.Id,
        v.ClienteId,
        v.Fecha,
        c.Nombre AS ClienteNombre,
        v.Estado,
        v.FechaRegistro
    FROM Ventas v
    INNER JOIN Clientes c ON v.ClienteId = c.Id
    WHERE v.ClienteId = @ClienteId
    ORDER BY v.Fecha DESC;
END
GO

-- Detalle de Venta 
CREATE OR ALTER PROCEDURE sp_DetalleVenta
    @VentaId INT
AS
BEGIN
    SELECT 
        V.Id AS VentaId,
        V.Fecha,
        C.Nombre AS Cliente,
        P.Nombre AS Producto,
        D.Cantidad,
        D.PrecioUnitario,
        (D.Cantidad * D.PrecioUnitario) AS Subtotal
    FROM Ventas V
    INNER JOIN Clientes C ON V.ClienteId = C.Id
    INNER JOIN DetalleVenta D ON V.Id = D.VentaId
    INNER JOIN Productos P ON D.ProductoId = P.Id
    WHERE V.Id = @VentaId
    ORDER BY P.Nombre ASC;
END
GO

-- Listar Roles
CREATE OR ALTER PROCEDURE sp_ListarRoles
AS
BEGIN
    SELECT Id, Nombre, Estado, FechaRegistro
    FROM Roles
    WHERE Estado = 1;
END
GO

-- Mostrar imagenes
CREATE OR ALTER PROCEDURE sp_ListarImagenesDashboard
AS
BEGIN
    SELECT Id, Url, Estado, FechaRegistro, Correlativo
    FROM ImagenesDashboard
    WHERE Estado = 1
    ORDER BY Correlativo ASC;
END
GO

-- Ingresar imange
CREATE OR ALTER PROCEDURE sp_RegistrarImagenDashboard
    @Url VARCHAR(MAX),
    @Correlativo INT
AS
BEGIN
    INSERT INTO ImagenesDashboard (Url, Correlativo)
    VALUES (@Url, @Correlativo);
END
GO

-- Cambiar estado imagen 
CREATE OR ALTER PROCEDURE sp_CambiarEstadoImagenDashboard
    @Id INT,
    @Estado INT
AS
BEGIN
    UPDATE ImagenesDashboard
    SET Estado = @Estado
    WHERE Id = @Id;
END
GO
