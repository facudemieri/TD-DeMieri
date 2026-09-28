USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'INGSW_23DB')
    CREATE DATABASE INGSW_23DB;
GO

USE INGSW_23DB;
GO

-- Tablas
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Rol_23DB')
CREATE TABLE Rol_23DB (
    IdRol INT PRIMARY KEY,
    NombreRol VARCHAR(20) NOT NULL
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Usuario_23DB')
CREATE TABLE Usuario_23DB (
    DNI VARCHAR(8) PRIMARY KEY,
    Apellido VARCHAR(50) NOT NULL,
    Nombre VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Login VARCHAR(20) NOT NULL,
    Password VARCHAR(256) NOT NULL,
    IdRol INT NOT NULL FOREIGN KEY REFERENCES Rol_23DB(IdRol),
    Bloqueado BIT DEFAULT 0,
    Activo BIT DEFAULT 1,
    IntentosFallidos INT DEFAULT 0,
    FechaUltimoIntento DATETIME NULL,
    UltimoIdioma NVARCHAR(50) NULL
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Patente_23DB')
CREATE TABLE Patente_23DB (
    IdPatente INT PRIMARY KEY,
    NombrePatente VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(100) NOT NULL
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Familia_23DB')
CREATE TABLE Familia_23DB (
    IdFamilia INT PRIMARY KEY,
    NombreFamilia VARCHAR(50) NOT NULL
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RolPat_23DB')
CREATE TABLE RolPat_23DB (
    IdRol INT NOT NULL FOREIGN KEY REFERENCES Rol_23DB(IdRol),
    IdPatente INT NOT NULL FOREIGN KEY REFERENCES Patente_23DB(IdPatente),
    PRIMARY KEY (IdRol, IdPatente)
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RolFam_23DB')
CREATE TABLE RolFam_23DB (
    IdRol INT NOT NULL FOREIGN KEY REFERENCES Rol_23DB(IdRol),
    IdFamilia INT NOT NULL FOREIGN KEY REFERENCES Familia_23DB(IdFamilia),
    PRIMARY KEY (IdRol, IdFamilia)
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FamPat_23DB')
CREATE TABLE FamPat_23DB (
    IdFamilia INT NOT NULL FOREIGN KEY REFERENCES Familia_23DB(IdFamilia),
    IdPatente INT NOT NULL FOREIGN KEY REFERENCES Patente_23DB(IdPatente),
    PRIMARY KEY (IdFamilia, IdPatente)
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FamFam_23DB')
CREATE TABLE FamFam_23DB (
    IdFamiliaPadre INT NOT NULL FOREIGN KEY REFERENCES Familia_23DB(IdFamilia),
    IdFamiliaHija INT NOT NULL FOREIGN KEY REFERENCES Familia_23DB(IdFamilia),
    PRIMARY KEY (IdFamiliaPadre, IdFamiliaHija)
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Eventos_23DB')
CREATE TABLE Eventos_23DB (
    Id_Evento INT PRIMARY KEY,
    DNI VARCHAR(8) NOT NULL FOREIGN KEY REFERENCES Usuario_23DB(DNI),
    Fecha DATE NOT NULL,
    Hora TIME NOT NULL,
    Modulo VARCHAR(50) NOT NULL,
    Evento VARCHAR(50) NOT NULL,
    Criticidad INT NOT NULL
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DV_23DB')
CREATE TABLE DV_23DB (
    IdTabla INT PRIMARY KEY,
    NombreTabla VARCHAR(50) NOT NULL,
    DVH BIGINT NOT NULL DEFAULT 0,
    DVV BIGINT NOT NULL DEFAULT 0
);
GO

-- Datos iniciales
IF NOT EXISTS (SELECT * FROM Rol_23DB WHERE IdRol = 1)
    INSERT INTO Rol_23DB VALUES (1, 'Administrador');
IF NOT EXISTS (SELECT * FROM Rol_23DB WHERE IdRol = 2)
    INSERT INTO Rol_23DB VALUES (2, 'Basico');

IF NOT EXISTS (SELECT * FROM Patente_23DB WHERE IdPatente = 1)
    INSERT INTO Patente_23DB VALUES (1, 'Gestion de Usuarios', 'Acceso al modulo de gestion de usuarios');
IF NOT EXISTS (SELECT * FROM Patente_23DB WHERE IdPatente = 2)
    INSERT INTO Patente_23DB VALUES (2, 'Gestion de Perfiles', 'Acceso al modulo de gestion de perfiles');
IF NOT EXISTS (SELECT * FROM Patente_23DB WHERE IdPatente = 3)
    INSERT INTO Patente_23DB VALUES (3, 'Cambio de Clave', 'Acceso al modulo de cambio de clave');
IF NOT EXISTS (SELECT * FROM Patente_23DB WHERE IdPatente = 4)
    INSERT INTO Patente_23DB VALUES (4, 'Gestion de Bitacora', 'Acceso al modulo de bitacora de eventos');
IF NOT EXISTS (SELECT * FROM Patente_23DB WHERE IdPatente = 5)
    INSERT INTO Patente_23DB VALUES (5, 'Gestion de Respaldo', 'Acceso al modulo de gestion de respaldos');

IF NOT EXISTS (SELECT * FROM Usuario_23DB WHERE DNI = '12345678')
    INSERT INTO Usuario_23DB (DNI, Apellido, Nombre, Email, Login, Password, IdRol, Bloqueado, Activo)
    VALUES ('12345678', 'Garcia', 'Juan', 'juan.garcia@mail.com', 'Juan.Garcia',
    '35b858194a2d35fbcc5e4dfd32c5cbd1f9212d0d870b6a5d9d297c13569a1456', 1, 0, 1);

-- Patentes del Administrador
IF NOT EXISTS (SELECT * FROM RolPat_23DB WHERE IdRol = 1 AND IdPatente = 1)
    INSERT INTO RolPat_23DB VALUES (1, 1);
IF NOT EXISTS (SELECT * FROM RolPat_23DB WHERE IdRol = 1 AND IdPatente = 2)
    INSERT INTO RolPat_23DB VALUES (1, 2);
IF NOT EXISTS (SELECT * FROM RolPat_23DB WHERE IdRol = 1 AND IdPatente = 3)
    INSERT INTO RolPat_23DB VALUES (1, 3);
IF NOT EXISTS (SELECT * FROM RolPat_23DB WHERE IdRol = 1 AND IdPatente = 4)
    INSERT INTO RolPat_23DB VALUES (1, 4);
IF NOT EXISTS (SELECT * FROM RolPat_23DB WHERE IdRol = 1 AND IdPatente = 5)
    INSERT INTO RolPat_23DB VALUES (1, 5);

-- Patentes del Basico
IF NOT EXISTS (SELECT * FROM RolPat_23DB WHERE IdRol = 2 AND IdPatente = 3)
    INSERT INTO RolPat_23DB VALUES (2, 3);

-- DV
IF NOT EXISTS (SELECT * FROM DV_23DB WHERE IdTabla = 1)
    INSERT INTO DV_23DB VALUES (1, 'Usuario_23DB', 0, 0);
IF NOT EXISTS (SELECT * FROM DV_23DB WHERE IdTabla = 2)
    INSERT INTO DV_23DB VALUES (2, 'Rol_23DB', 0, 0);
IF NOT EXISTS (SELECT * FROM DV_23DB WHERE IdTabla = 3)
    INSERT INTO DV_23DB VALUES (3, 'Eventos_23DB', 0, 0);
IF NOT EXISTS (SELECT * FROM DV_23DB WHERE IdTabla = 4)
    INSERT INTO DV_23DB VALUES (4, 'Patente_23DB', 0, 0);
IF NOT EXISTS (SELECT * FROM DV_23DB WHERE IdTabla = 5)
    INSERT INTO DV_23DB VALUES (5, 'Familia_23DB', 0, 0);
IF NOT EXISTS (SELECT * FROM DV_23DB WHERE IdTabla = 6)
    INSERT INTO DV_23DB VALUES (6, 'RolPat_23DB', 0, 0);
IF NOT EXISTS (SELECT * FROM DV_23DB WHERE IdTabla = 7)
    INSERT INTO DV_23DB VALUES (7, 'RolFam_23DB', 0, 0);
IF NOT EXISTS (SELECT * FROM DV_23DB WHERE IdTabla = 8)
    INSERT INTO DV_23DB VALUES (8, 'FamPat_23DB', 0, 0);
IF NOT EXISTS (SELECT * FROM DV_23DB WHERE IdTabla = 9)
    INSERT INTO DV_23DB VALUES (9, 'FamFam_23DB', 0, 0);
GO

-- Tablas del modulo de negocio (_62FD)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Vuelo_62FD')
CREATE TABLE Vuelo_62FD (
    NroVuelo_62FD VARCHAR(10) NOT NULL CONSTRAINT PK_Vuelo_62FD PRIMARY KEY,
    Origen_62FD VARCHAR(50) NOT NULL,
    Destino_62FD VARCHAR(50) NOT NULL,
    FechaHoraSalida_62FD DATETIME NOT NULL,
    Matricula_62FD VARCHAR(10) NOT NULL,
    Precio_62FD DECIMAL(10,2) NOT NULL,
    EstadoVuelo_62FD VARCHAR(20) NOT NULL
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Pasajero_62FD')
CREATE TABLE Pasajero_62FD (
    numeroDocumento_62FD VARCHAR(8) NOT NULL CONSTRAINT PK_Pasajero_62FD PRIMARY KEY,
    Apellido_62FD VARCHAR(50) NOT NULL,
    Nombre_62FD VARCHAR(50) NOT NULL,
    FechaNacimiento_62FD DATE NOT NULL,
    CorreoElectronico_62FD VARCHAR(100) NOT NULL,
    Telefono_62FD VARCHAR(20) NOT NULL,
    ContactoEmergencia_62FD VARCHAR(100) NOT NULL
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Asiento_62FD')
CREATE TABLE Asiento_62FD (
    IdAsiento_62FD INT NOT NULL CONSTRAINT PK_Asiento_62FD PRIMARY KEY,
    NroVuelo_62FD VARCHAR(10) NOT NULL CONSTRAINT FK_Asiento_62FD_Vuelo_62FD FOREIGN KEY REFERENCES Vuelo_62FD(NroVuelo_62FD),
    NroAsiento_62FD VARCHAR(5) NOT NULL,
    Clase_62FD VARCHAR(20) NOT NULL,
    EstadoAsiento_62FD VARCHAR(20) NOT NULL
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Reserva_62FD')
CREATE TABLE Reserva_62FD (
    CodReserva_62FD VARCHAR(10) NOT NULL CONSTRAINT PK_Reserva_62FD PRIMARY KEY,
    NroVuelo_62FD VARCHAR(10) NOT NULL CONSTRAINT FK_Reserva_62FD_Vuelo_62FD FOREIGN KEY REFERENCES Vuelo_62FD(NroVuelo_62FD),
    FechaReserva_62FD DATETIME NOT NULL,
    MontoTotal_62FD DECIMAL(10,2) NOT NULL,
    EstadoReserva_62FD VARCHAR(20) NOT NULL
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ReservaDetalle_62FD')
CREATE TABLE ReservaDetalle_62FD (
    IdDetalle_62FD INT NOT NULL CONSTRAINT PK_ReservaDetalle_62FD PRIMARY KEY,
    CodReserva_62FD VARCHAR(10) NOT NULL CONSTRAINT FK_ReservaDetalle_62FD_Reserva_62FD FOREIGN KEY REFERENCES Reserva_62FD(CodReserva_62FD),
    numeroDocumento_62FD VARCHAR(8) NOT NULL CONSTRAINT FK_ReservaDetalle_62FD_Pasajero_62FD FOREIGN KEY REFERENCES Pasajero_62FD(numeroDocumento_62FD),
    IdAsiento_62FD INT NOT NULL CONSTRAINT FK_ReservaDetalle_62FD_Asiento_62FD FOREIGN KEY REFERENCES Asiento_62FD(IdAsiento_62FD),
    Precio_62FD DECIMAL(10,2) NOT NULL
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Pago_62FD')
CREATE TABLE Pago_62FD (
    CodPago_62FD VARCHAR(10) NOT NULL CONSTRAINT PK_Pago_62FD PRIMARY KEY,
    CodReserva_62FD VARCHAR(10) NOT NULL CONSTRAINT FK_Pago_62FD_Reserva_62FD FOREIGN KEY REFERENCES Reserva_62FD(CodReserva_62FD),
    MedioPago_62FD VARCHAR(20) NOT NULL,
    EntidadFinanciera_62FD VARCHAR(50) NULL,
    UltimosDigitos_62FD VARCHAR(4) NULL,
    CodigoAutorizacion_62FD VARCHAR(20) NULL,
    FechaOperacion_62FD DATETIME NOT NULL,
    MontoTotal_62FD DECIMAL(10,2) NOT NULL,
    EstadoPago_62FD VARCHAR(20) NOT NULL
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Pasaje_62FD')
CREATE TABLE Pasaje_62FD (
    CodPasaje_62FD VARCHAR(10) NOT NULL CONSTRAINT PK_Pasaje_62FD PRIMARY KEY,
    IdDetalle_62FD INT NOT NULL CONSTRAINT FK_Pasaje_62FD_ReservaDetalle_62FD FOREIGN KEY REFERENCES ReservaDetalle_62FD(IdDetalle_62FD),
    FechaEmision_62FD DATETIME NOT NULL,
    EstadoPasaje_62FD VARCHAR(20) NOT NULL
);
GO

-- Restriccion de integridad: un mismo asiento no puede figurar en dos detalles
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ReservaDetalle_62FD_IdAsiento_62FD' AND object_id = OBJECT_ID('ReservaDetalle_62FD'))
    CREATE UNIQUE NONCLUSTERED INDEX IX_ReservaDetalle_62FD_IdAsiento_62FD ON ReservaDetalle_62FD(IdAsiento_62FD);
GO