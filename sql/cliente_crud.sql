/* ==========================================================
   SCRIPT: CRUD Cliente - SQL Server
   ========================================================== */

-- A) Base de datos (opcional)
IF DB_ID('ClientesDb') IS NULL
BEGIN
    CREATE DATABASE ClientesDb;
END;
GO

USE ClientesDb;
GO

-- Eliminar objetos si existen (para re-ejecutar script)
IF OBJECT_ID('dbo.Cliente_SearchByNombre', 'P') IS NOT NULL DROP PROCEDURE dbo.Cliente_SearchByNombre;
IF OBJECT_ID('dbo.Cliente_GetAll', 'P') IS NOT NULL DROP PROCEDURE dbo.Cliente_GetAll;
IF OBJECT_ID('dbo.Cliente_GetById', 'P') IS NOT NULL DROP PROCEDURE dbo.Cliente_GetById;
IF OBJECT_ID('dbo.Cliente_Delete', 'P') IS NOT NULL DROP PROCEDURE dbo.Cliente_Delete;
IF OBJECT_ID('dbo.Cliente_Update', 'P') IS NOT NULL DROP PROCEDURE dbo.Cliente_Update;
IF OBJECT_ID('dbo.Cliente_Insert', 'P') IS NOT NULL DROP PROCEDURE dbo.Cliente_Insert;
GO

IF OBJECT_ID('dbo.Clientes', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.Clientes;
END;
GO

-- Tabla
CREATE TABLE dbo.Clientes
(
    clienteId       INT IDENTITY(1,1) NOT NULL,
    nombre          NVARCHAR(120) NOT NULL,
    edad            INT NOT NULL,
    fechaNacimiento DATE NOT NULL,
    salario         DECIMAL(18,2) NOT NULL,
    CONSTRAINT PK_Clientes PRIMARY KEY CLUSTERED (clienteId),
    CONSTRAINT CK_Clientes_Nombre_Len CHECK (LEN(LTRIM(RTRIM(nombre))) BETWEEN 2 AND 120),
    CONSTRAINT CK_Clientes_Edad CHECK (edad BETWEEN 0 AND 120),
    CONSTRAINT CK_Clientes_FechaNacimiento CHECK (fechaNacimiento <= CONVERT(date, GETDATE())),
    CONSTRAINT CK_Clientes_Salario CHECK (salario >= 0)
);
GO

-- Índices recomendados
CREATE INDEX IX_Clientes_Nombre ON dbo.Clientes(nombre);
CREATE INDEX IX_Clientes_FechaNacimiento ON dbo.Clientes(fechaNacimiento);
GO

/* ==========================================================
   B) Stored Procedures
   ========================================================== */

CREATE PROCEDURE dbo.Cliente_Insert
    @nombre NVARCHAR(120),
    @edad INT,
    @fechaNacimiento DATE,
    @salario DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Clientes (nombre, edad, fechaNacimiento, salario)
    VALUES (@nombre, @edad, @fechaNacimiento, @salario);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS clienteId;
END;
GO

CREATE PROCEDURE dbo.Cliente_Update
    @clienteId INT,
    @nombre NVARCHAR(120),
    @edad INT,
    @fechaNacimiento DATE,
    @salario DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Clientes
    SET nombre = @nombre,
        edad = @edad,
        fechaNacimiento = @fechaNacimiento,
        salario = @salario
    WHERE clienteId = @clienteId;

    SELECT @@ROWCOUNT AS affectedRows;
END;
GO

CREATE PROCEDURE dbo.Cliente_Delete
    @clienteId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.Clientes
    WHERE clienteId = @clienteId;

    SELECT @@ROWCOUNT AS affectedRows;
END;
GO

CREATE PROCEDURE dbo.Cliente_GetById
    @clienteId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT clienteId, nombre, edad, fechaNacimiento, salario
    FROM dbo.Clientes
    WHERE clienteId = @clienteId;
END;
GO

CREATE PROCEDURE dbo.Cliente_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT clienteId, nombre, edad, fechaNacimiento, salario
    FROM dbo.Clientes
    ORDER BY clienteId DESC;
END;
GO

CREATE PROCEDURE dbo.Cliente_SearchByNombre
    @q NVARCHAR(120)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT clienteId, nombre, edad, fechaNacimiento, salario
    FROM dbo.Clientes
    WHERE nombre LIKE CONCAT('%', @q, '%')
    ORDER BY nombre ASC;
END;
GO

/* ==========================================================
   Pruebas rápidas con EXEC
   ========================================================== */

-- Insertar
EXEC dbo.Cliente_Insert @nombre = N'Juan Pérez', @edad = 30, @fechaNacimiento = '1994-01-20', @salario = 2500.00;
EXEC dbo.Cliente_Insert @nombre = N'María Gómez', @edad = 40, @fechaNacimiento = '1984-05-12', @salario = 4200.75;

-- Listar todos
EXEC dbo.Cliente_GetAll;

-- Buscar por ID
EXEC dbo.Cliente_GetById @clienteId = 1;

-- Actualizar
EXEC dbo.Cliente_Update @clienteId = 1, @nombre = N'Juan Pablo Pérez', @edad = 31, @fechaNacimiento = '1993-01-20', @salario = 2800.00;

-- Buscar por nombre
EXEC dbo.Cliente_SearchByNombre @q = N'Juan';

-- Eliminar
EXEC dbo.Cliente_Delete @clienteId = 2;
