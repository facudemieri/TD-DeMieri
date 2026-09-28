-- ============================================================
-- INGSW_23DB_SPs.sql
-- Stored procedures consumidos por los mappers:
--   mapperPatente_23DB, mapperUsuario_23DB, mapperEvento_23DB, mapperDV_23DB,
--   mapperRol_23DB, mapperFamilia_23DB, mapperRespaldo_23DB
--
-- SET NOCOUNT ON solo va en los SPs que se leen con Acceso_23DB.Leer_23DB
-- (incluidos InsertarRol e InsertarFamilia, que devuelven el Id generado) y en
-- GenerarBackup. Los que se invocan con Escribir_23DB NO lo llevan: con NOCOUNT
-- activo ExecuteNonQuery devuelve -1 y la BLL lo interpreta como error.
--
-- Script idempotente: cada SP se elimina antes de recrearse.
-- No reemplaza a INGSW_23DB_Setup.sql (tablas y datos iniciales).
-- ============================================================

USE INGSW_23DB;
GO

-- ============================================================
-- mapperPatente_23DB
-- ============================================================

-- ObtenerPatentes_23DB()
IF OBJECT_ID('dbo.ObtenerPatentes', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerPatentes;
GO
CREATE PROCEDURE dbo.ObtenerPatentes
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdPatente, NombrePatente, Descripcion
    FROM Patente_23DB;
END
GO

-- ============================================================
-- mapperUsuario_23DB
-- ============================================================

-- ObtenerTodos_23DB(filtro): 'Activos' | 'Inactivos' | cualquier otro valor = todos
IF OBJECT_ID('dbo.ObtenerTodos', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerTodos;
GO
CREATE PROCEDURE dbo.ObtenerTodos
    @Filtro VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DNI, Apellido, Nombre, Email, [Login], IdRol, Bloqueado, Activo,
           IntentosFallidos, FechaUltimoIntento, UltimoIdioma
    FROM Usuario_23DB
    WHERE (@Filtro = 'Activos'   AND Activo = 1)
       OR (@Filtro = 'Inactivos' AND Activo = 0)
       OR (@Filtro IS NULL OR @Filtro NOT IN ('Activos', 'Inactivos'))
    ORDER BY Apellido, Nombre;
END
GO

-- Insertar_23DB(usuario)
IF OBJECT_ID('dbo.Insertar', 'P') IS NOT NULL DROP PROCEDURE dbo.Insertar;
GO
CREATE PROCEDURE dbo.Insertar
    @DNI        VARCHAR(8),
    @Apellido   VARCHAR(50),
    @Nombre     VARCHAR(50),
    @Email      VARCHAR(100),
    @Login      VARCHAR(20),
    @Password   VARCHAR(256),
    @IdRol      INT,
    @Bloqueado  BIT,
    @Activo     BIT
AS
BEGIN
    INSERT INTO Usuario_23DB (DNI, Apellido, Nombre, Email, [Login], [Password], IdRol, Bloqueado, Activo)
    VALUES (@DNI, @Apellido, @Nombre, @Email, @Login, @Password, @IdRol, @Bloqueado, @Activo);
END
GO

-- Modificar_23DB(usuario)
IF OBJECT_ID('dbo.Modificar', 'P') IS NOT NULL DROP PROCEDURE dbo.Modificar;
GO
CREATE PROCEDURE dbo.Modificar
    @DNI    VARCHAR(8),
    @Email  VARCHAR(100),
    @IdRol  INT
AS
BEGIN
    UPDATE Usuario_23DB
    SET Email = @Email,
        IdRol = @IdRol
    WHERE DNI = @DNI;
END
GO

-- CambiarEstado_23DB(dni, activo)
IF OBJECT_ID('dbo.CambiarEstado', 'P') IS NOT NULL DROP PROCEDURE dbo.CambiarEstado;
GO
CREATE PROCEDURE dbo.CambiarEstado
    @DNI     VARCHAR(8),
    @Activo  BIT
AS
BEGIN
    UPDATE Usuario_23DB
    SET Activo = @Activo
    WHERE DNI = @DNI;
END
GO

-- Desbloquear_23DB(dni, passwordInicial)
IF OBJECT_ID('dbo.Desbloquear', 'P') IS NOT NULL DROP PROCEDURE dbo.Desbloquear;
GO
CREATE PROCEDURE dbo.Desbloquear
    @DNI       VARCHAR(8),
    @Password  VARCHAR(256)
AS
BEGIN
    UPDATE Usuario_23DB
    SET Bloqueado = 0,
        [Password] = @Password,
        IntentosFallidos = 0
    WHERE DNI = @DNI;
END
GO

-- ActualizarPassword_23DB(dni, passwordEncriptado)
IF OBJECT_ID('dbo.ActualizarPassword', 'P') IS NOT NULL DROP PROCEDURE dbo.ActualizarPassword;
GO
CREATE PROCEDURE dbo.ActualizarPassword
    @DNI       VARCHAR(8),
    @Password  VARCHAR(256)
AS
BEGIN
    UPDATE Usuario_23DB
    SET [Password] = @Password
    WHERE DNI = @DNI;
END
GO

-- ObtenerUsuario_23DB(login, password): valida credenciales
IF OBJECT_ID('dbo.ObtenerUsuario', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerUsuario;
GO
CREATE PROCEDURE dbo.ObtenerUsuario
    @Login     VARCHAR(20),
    @Password  VARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DNI, Apellido, Nombre, Email, [Login], IdRol, Bloqueado, Activo,
           IntentosFallidos, FechaUltimoIntento, UltimoIdioma
    FROM Usuario_23DB
    WHERE [Login] = @Login
      AND [Password] = @Password;
END
GO

-- ObtenerUsuarioPorLogin_23DB(login)
IF OBJECT_ID('dbo.ObtenerUsuarioPorLogin', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerUsuarioPorLogin;
GO
CREATE PROCEDURE dbo.ObtenerUsuarioPorLogin
    @Login VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DNI, Apellido, Nombre, Email, [Login], IdRol, Bloqueado, Activo,
           IntentosFallidos, FechaUltimoIntento, UltimoIdioma
    FROM Usuario_23DB
    WHERE [Login] = @Login;
END
GO

-- ObtenerUsuarioPorDNI_23DB(dni, password): para cambiar clave
IF OBJECT_ID('dbo.ObtenerUsuarioPorDNI', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerUsuarioPorDNI;
GO
CREATE PROCEDURE dbo.ObtenerUsuarioPorDNI
    @DNI       VARCHAR(8),
    @Password  VARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DNI, Apellido, Nombre, Email, [Login], IdRol, Bloqueado, Activo,
           IntentosFallidos, FechaUltimoIntento, UltimoIdioma
    FROM Usuario_23DB
    WHERE DNI = @DNI
      AND [Password] = @Password;
END
GO

-- ObtenerUsuarioDNI_23DB(dni)
-- Tambien lo consume mapperEvento_23DB.ObtenerUsuarioPorDNI_23DB(dni),
-- que solo lee DNI, Nombre y Apellido de este mismo resultset.
IF OBJECT_ID('dbo.ObtenerUsuarioDNI', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerUsuarioDNI;
GO
CREATE PROCEDURE dbo.ObtenerUsuarioDNI
    @DNI VARCHAR(8)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DNI, Apellido, Nombre, Email, [Login], IdRol, Bloqueado, Activo,
           IntentosFallidos, FechaUltimoIntento, UltimoIdioma
    FROM Usuario_23DB
    WHERE DNI = @DNI;
END
GO

-- BloquearUsuario_23DB(dni)
IF OBJECT_ID('dbo.BloquearUsuario', 'P') IS NOT NULL DROP PROCEDURE dbo.BloquearUsuario;
GO
CREATE PROCEDURE dbo.BloquearUsuario
    @DNI VARCHAR(8)
AS
BEGIN
    UPDATE Usuario_23DB
    SET Bloqueado = 1
    WHERE DNI = @DNI;
END
GO

-- IncrementarIntentos_23DB(dni)
IF OBJECT_ID('dbo.IncrementarIntentos', 'P') IS NOT NULL DROP PROCEDURE dbo.IncrementarIntentos;
GO
CREATE PROCEDURE dbo.IncrementarIntentos
    @DNI    VARCHAR(8),
    @Fecha  DATETIME
AS
BEGIN
    UPDATE Usuario_23DB
    SET IntentosFallidos = IntentosFallidos + 1,
        FechaUltimoIntento = @Fecha
    WHERE DNI = @DNI;
END
GO

-- ResetearIntentos_23DB(dni)
IF OBJECT_ID('dbo.ResetearIntentos', 'P') IS NOT NULL DROP PROCEDURE dbo.ResetearIntentos;
GO
CREATE PROCEDURE dbo.ResetearIntentos
    @DNI VARCHAR(8)
AS
BEGIN
    UPDATE Usuario_23DB
    SET IntentosFallidos = 0,
        FechaUltimoIntento = NULL
    WHERE DNI = @DNI;
END
GO

-- ActualizarUltimoIdioma_23DB(dni, idioma)
IF OBJECT_ID('dbo.ActualizarUltimoIdioma', 'P') IS NOT NULL DROP PROCEDURE dbo.ActualizarUltimoIdioma;
GO
CREATE PROCEDURE dbo.ActualizarUltimoIdioma
    @DNI     VARCHAR(8),
    @Idioma  NVARCHAR(50)
AS
BEGIN
    UPDATE Usuario_23DB
    SET UltimoIdioma = @Idioma
    WHERE DNI = @DNI;
END
GO

-- ============================================================
-- mapperEvento_23DB
-- ============================================================

-- InsertarEvento_23DB(dni, modulo, evento, criticidad)
-- El Id_Evento se calcula adentro del SP: el SELECT MAX y el INSERT van
-- en una sola sentencia INSERT ... SELECT, sin manejo transaccional explicito.
-- El UPDLOCK/HOLDLOCK sobre el SELECT MAX(Id_Evento) serializa el calculo para
-- que dos sesiones concurrentes no obtengan el mismo Id. Al ser una unica
-- sentencia, el bloqueo se sostiene desde la lectura del maximo hasta que la
-- fila queda insertada.
IF OBJECT_ID('dbo.InsertarEvento', 'P') IS NOT NULL DROP PROCEDURE dbo.InsertarEvento;
GO
CREATE PROCEDURE dbo.InsertarEvento
    @DNI         VARCHAR(8),
    @Fecha       DATE,
    @Hora        TIME,
    @Modulo      VARCHAR(50),
    @Evento      VARCHAR(50),
    @Criticidad  INT
AS
BEGIN
    INSERT INTO Eventos_23DB (Id_Evento, DNI, Fecha, Hora, Modulo, Evento, Criticidad)
    SELECT ISNULL(MAX(Id_Evento), 0) + 1, @DNI, @Fecha, @Hora, @Modulo, @Evento, @Criticidad
    FROM Eventos_23DB WITH (UPDLOCK, HOLDLOCK);
END
GO

-- ObtenerEventos_23DB(fechaInicio, fechaFin)
IF OBJECT_ID('dbo.ObtenerEventos', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerEventos;
GO
CREATE PROCEDURE dbo.ObtenerEventos
    @FechaInicio  DATE,
    @FechaFin     DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id_Evento, DNI, Fecha, Hora, Modulo, Evento, Criticidad
    FROM Eventos_23DB
    WHERE Fecha BETWEEN @FechaInicio AND @FechaFin
    ORDER BY Fecha DESC, Hora DESC;
END
GO

-- FiltrarEventos_23DB(dni, fechaInicio, fechaFin, modulo, evento, criticidad)
-- Parametros opcionales: el mapper manda DBNull.Value en los filtros que no aplican.
IF OBJECT_ID('dbo.FiltrarEventos', 'P') IS NOT NULL DROP PROCEDURE dbo.FiltrarEventos;
GO
CREATE PROCEDURE dbo.FiltrarEventos
    @FechaInicio  DATE,
    @FechaFin     DATE,
    @DNI          VARCHAR(8)  = NULL,
    @Modulo       VARCHAR(50) = NULL,
    @Evento       VARCHAR(50) = NULL,
    @Criticidad   INT         = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id_Evento, DNI, Fecha, Hora, Modulo, Evento, Criticidad
    FROM Eventos_23DB
    WHERE Fecha BETWEEN @FechaInicio AND @FechaFin
      AND (@DNI        IS NULL OR DNI        = @DNI)
      AND (@Modulo     IS NULL OR Modulo     = @Modulo)
      AND (@Evento     IS NULL OR Evento     = @Evento)
      AND (@Criticidad IS NULL OR Criticidad = @Criticidad)
    ORDER BY Fecha DESC, Hora DESC;
END
GO

-- ObtenerLogins_23DB()
IF OBJECT_ID('dbo.ObtenerLogins', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerLogins;
GO
CREATE PROCEDURE dbo.ObtenerLogins
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DNI, [Login]
    FROM Usuario_23DB
    ORDER BY [Login];
END
GO

-- ============================================================
-- mapperDV_23DB
-- ============================================================

-- ActualizarDV_23DB(idTabla, dvh, dvv)
IF OBJECT_ID('dbo.ActualizarDV', 'P') IS NOT NULL DROP PROCEDURE dbo.ActualizarDV;
GO
CREATE PROCEDURE dbo.ActualizarDV
    @IdTabla  INT,
    @DVH      BIGINT,
    @DVV      BIGINT
AS
BEGIN
    UPDATE DV_23DB
    SET DVH = @DVH,
        DVV = @DVV
    WHERE IdTabla = @IdTabla;
END
GO

-- ObtenerDV_23DB(idTabla)
IF OBJECT_ID('dbo.ObtenerDV', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerDV;
GO
CREATE PROCEDURE dbo.ObtenerDV
    @IdTabla INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdTabla, NombreTabla, DVH, DVV
    FROM DV_23DB
    WHERE IdTabla = @IdTabla;
END
GO

-- ObtenerTodosDV_23DB()
IF OBJECT_ID('dbo.ObtenerTodosDV', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerTodosDV;
GO
CREATE PROCEDURE dbo.ObtenerTodosDV
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdTabla, NombreTabla, DVH, DVV
    FROM DV_23DB;
END
GO

-- ObtenerDatosTabla_23DB(nombreTabla)
-- @NombreTabla se valida contra DV_23DB (lista blanca) y contra sys.tables
-- ANTES de ejecutar nada. Si no pasa la validacion el SP retorna sin
-- devolver resultset, y el DataTable del mapper queda vacio.
-- Recien despues se arma el SQL dinamico con QUOTENAME + sp_executesql.
IF OBJECT_ID('dbo.ObtenerDatosTabla', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerDatosTabla;
GO
CREATE PROCEDURE dbo.ObtenerDatosTabla
    @NombreTabla VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM DV_23DB WHERE NombreTabla = @NombreTabla)
        RETURN;

    IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = @NombreTabla AND SCHEMA_NAME(schema_id) = 'dbo')
        RETURN;

    DECLARE @Sql NVARCHAR(MAX) = N'SELECT * FROM dbo.' + QUOTENAME(@NombreTabla) + N';';

    EXEC sys.sp_executesql @Sql;
END
GO

-- ============================================================
-- mapperRol_23DB
-- ============================================================

-- ObtenerRoles_23DB()
IF OBJECT_ID('dbo.ObtenerRoles', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerRoles;
GO
CREATE PROCEDURE dbo.ObtenerRoles
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdRol, NombreRol
    FROM Rol_23DB;
END
GO

-- ObtenerRol_23DB(idRol)
IF OBJECT_ID('dbo.ObtenerRol', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerRol;
GO
CREATE PROCEDURE dbo.ObtenerRol
    @IdRol INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdRol, NombreRol
    FROM Rol_23DB
    WHERE IdRol = @IdRol;
END
GO

-- InsertarRol_23DB(nombreRol, componentes): inserta solo la fila de Rol_23DB.
-- El IdRol se calcula adentro con ISNULL(MAX(IdRol),0)+1 y el hint
-- WITH (UPDLOCK, HOLDLOCK), en una unica sentencia INSERT ... SELECT para que
-- el bloqueo se sostenga desde la lectura del maximo hasta la insercion.
-- El OUTPUT deja el Id en una tabla de paso y el SELECT final se lo devuelve
-- al mapper, que lo necesita para InsertarRolPat / InsertarRolFam.
IF OBJECT_ID('dbo.InsertarRol', 'P') IS NOT NULL DROP PROCEDURE dbo.InsertarRol;
GO
CREATE PROCEDURE dbo.InsertarRol
    @NombreRol VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Generado TABLE (IdRol INT);

    INSERT INTO Rol_23DB (IdRol, NombreRol)
    OUTPUT INSERTED.IdRol INTO @Generado
    SELECT ISNULL(MAX(IdRol), 0) + 1, @NombreRol
    FROM Rol_23DB WITH (UPDLOCK, HOLDLOCK);

    SELECT IdRol FROM @Generado;
END
GO

-- InsertarRol_23DB / ModificarRol_23DB: un componente Patente por llamada
IF OBJECT_ID('dbo.InsertarRolPat', 'P') IS NOT NULL DROP PROCEDURE dbo.InsertarRolPat;
GO
CREATE PROCEDURE dbo.InsertarRolPat
    @IdRol     INT,
    @IdPatente INT
AS
BEGIN
    INSERT INTO RolPat_23DB (IdRol, IdPatente)
    VALUES (@IdRol, @IdPatente);
END
GO

-- InsertarRol_23DB / ModificarRol_23DB: un componente Familia por llamada
IF OBJECT_ID('dbo.InsertarRolFam', 'P') IS NOT NULL DROP PROCEDURE dbo.InsertarRolFam;
GO
CREATE PROCEDURE dbo.InsertarRolFam
    @IdRol     INT,
    @IdFamilia INT
AS
BEGIN
    INSERT INTO RolFam_23DB (IdRol, IdFamilia)
    VALUES (@IdRol, @IdFamilia);
END
GO

-- ModificarRol_23DB(idRol, nombreRol, componentes): solo el UPDATE de la cabecera
IF OBJECT_ID('dbo.ModificarRol', 'P') IS NOT NULL DROP PROCEDURE dbo.ModificarRol;
GO
CREATE PROCEDURE dbo.ModificarRol
    @IdRol      INT,
    @NombreRol  VARCHAR(20)
AS
BEGIN
    UPDATE Rol_23DB
    SET NombreRol = @NombreRol
    WHERE IdRol = @IdRol;
END
GO

-- ModificarRol_23DB / EliminarRol_23DB: limpia las patentes del rol
IF OBJECT_ID('dbo.EliminarRolPatPorRol', 'P') IS NOT NULL DROP PROCEDURE dbo.EliminarRolPatPorRol;
GO
CREATE PROCEDURE dbo.EliminarRolPatPorRol
    @IdRol INT
AS
BEGIN
    DELETE FROM RolPat_23DB
    WHERE IdRol = @IdRol;
END
GO

-- ModificarRol_23DB / EliminarRol_23DB: limpia las familias del rol
IF OBJECT_ID('dbo.EliminarRolFamPorRol', 'P') IS NOT NULL DROP PROCEDURE dbo.EliminarRolFamPorRol;
GO
CREATE PROCEDURE dbo.EliminarRolFamPorRol
    @IdRol INT
AS
BEGIN
    DELETE FROM RolFam_23DB
    WHERE IdRol = @IdRol;
END
GO

-- EliminarRol_23DB(idRol): borra la cabecera, despues de las dos anteriores
IF OBJECT_ID('dbo.EliminarRol', 'P') IS NOT NULL DROP PROCEDURE dbo.EliminarRol;
GO
CREATE PROCEDURE dbo.EliminarRol
    @IdRol INT
AS
BEGIN
    DELETE FROM Rol_23DB
    WHERE IdRol = @IdRol;
END
GO

-- ObtenerRolCompleto_23DB(idRol): patentes asignadas directamente al rol
IF OBJECT_ID('dbo.ObtenerPatentesDeRolDirectas', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerPatentesDeRolDirectas;
GO
CREATE PROCEDURE dbo.ObtenerPatentesDeRolDirectas
    @IdRol INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT P.IdPatente, P.NombrePatente, P.Descripcion
    FROM Patente_23DB P
    INNER JOIN RolPat_23DB RP ON P.IdPatente = RP.IdPatente
    WHERE RP.IdRol = @IdRol;
END
GO

-- ObtenerRolCompleto_23DB(idRol): ids de las familias asignadas al rol
IF OBJECT_ID('dbo.ObtenerFamiliasDeRol', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerFamiliasDeRol;
GO
CREATE PROCEDURE dbo.ObtenerFamiliasDeRol
    @IdRol INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT RF.IdFamilia
    FROM RolFam_23DB RF
    WHERE RF.IdRol = @IdRol;
END
GO

-- RolEstaAsignado_23DB(idRol)
IF OBJECT_ID('dbo.RolEstaAsignado', 'P') IS NOT NULL DROP PROCEDURE dbo.RolEstaAsignado;
GO
CREATE PROCEDURE dbo.RolEstaAsignado
    @IdRol INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS Cantidad
    FROM Usuario_23DB
    WHERE IdRol = @IdRol;
END
GO

-- ObtenerPatentesDeRol_23DB(idRol): nombres de las patentes directas del rol
IF OBJECT_ID('dbo.ObtenerNombresPatentesDeRol', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerNombresPatentesDeRol;
GO
CREATE PROCEDURE dbo.ObtenerNombresPatentesDeRol
    @IdRol INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT P.NombrePatente
    FROM Patente_23DB P
    INNER JOIN RolPat_23DB RP ON P.IdPatente = RP.IdPatente
    WHERE RP.IdRol = @IdRol;
END
GO

-- ObtenerPatentesDeRol_23DB(idRol): patentes que llegan por las familias del rol
-- y por las familias de esas familias. CTE recursiva trasladada tal cual
-- desde el mapper, sin reescribir.
IF OBJECT_ID('dbo.ObtenerNombresPatentesDeFamiliasDeRol', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerNombresPatentesDeFamiliasDeRol;
GO
CREATE PROCEDURE dbo.ObtenerNombresPatentesDeFamiliasDeRol
    @IdRol INT
AS
BEGIN
    SET NOCOUNT ON;

    WITH FamiliasRecursivas AS (SELECT IdFamilia FROM RolFam_23DB WHERE IdRol = @IdRol UNION ALL
            SELECT FF.IdFamiliaHija
            FROM FamFam_23DB FF
            INNER JOIN FamiliasRecursivas FR ON FF.IdFamiliaPadre = FR.IdFamilia)
            SELECT DISTINCT P.NombrePatente
            FROM Patente_23DB P
            INNER JOIN FamPat_23DB FP ON P.IdPatente = FP.IdPatente
            INNER JOIN FamiliasRecursivas FR ON FP.IdFamilia = FR.IdFamilia;
END
GO

-- ============================================================
-- mapperFamilia_23DB
-- ============================================================

-- ObtenerFamilias_23DB()
IF OBJECT_ID('dbo.ObtenerFamilias', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerFamilias;
GO
CREATE PROCEDURE dbo.ObtenerFamilias
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdFamilia, NombreFamilia
    FROM Familia_23DB;
END
GO

-- ObtenerFamiliaRecursiva_23DB(idFamilia), consulta 1 de 3: la cabecera.
-- La recursion se mantiene en C#; cada nivel del Composite hace estas tres.
IF OBJECT_ID('dbo.ObtenerFamilia', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerFamilia;
GO
CREATE PROCEDURE dbo.ObtenerFamilia
    @IdFamilia INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdFamilia, NombreFamilia
    FROM Familia_23DB
    WHERE IdFamilia = @IdFamilia;
END
GO

-- ObtenerFamiliaRecursiva_23DB(idFamilia), consulta 2 de 3: patentes hoja
IF OBJECT_ID('dbo.ObtenerPatentesDeFamilia', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerPatentesDeFamilia;
GO
CREATE PROCEDURE dbo.ObtenerPatentesDeFamilia
    @IdFamilia INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT P.IdPatente, P.NombrePatente, P.Descripcion
    FROM Patente_23DB P
    INNER JOIN FamPat_23DB FP ON P.IdPatente = FP.IdPatente
    WHERE FP.IdFamilia = @IdFamilia;
END
GO

-- ObtenerFamiliaRecursiva_23DB(idFamilia), consulta 3 de 3: familias hijas
IF OBJECT_ID('dbo.ObtenerSubFamilias', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerSubFamilias;
GO
CREATE PROCEDURE dbo.ObtenerSubFamilias
    @IdFamilia INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdFamiliaHija
    FROM FamFam_23DB
    WHERE IdFamiliaPadre = @IdFamilia;
END
GO

-- InsertarFamilia_23DB(nombreFamilia, componentes): inserta solo la cabecera.
-- Mismo criterio que InsertarRol: el IdFamilia se calcula adentro con
-- ISNULL(MAX(IdFamilia),0)+1 y WITH (UPDLOCK, HOLDLOCK) en una unica sentencia
-- INSERT ... SELECT, y el SELECT final devuelve el Id al mapper.
IF OBJECT_ID('dbo.InsertarFamilia', 'P') IS NOT NULL DROP PROCEDURE dbo.InsertarFamilia;
GO
CREATE PROCEDURE dbo.InsertarFamilia
    @NombreFamilia VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Generado TABLE (IdFamilia INT);

    INSERT INTO Familia_23DB (IdFamilia, NombreFamilia)
    OUTPUT INSERTED.IdFamilia INTO @Generado
    SELECT ISNULL(MAX(IdFamilia), 0) + 1, @NombreFamilia
    FROM Familia_23DB WITH (UPDLOCK, HOLDLOCK);

    SELECT IdFamilia FROM @Generado;
END
GO

-- InsertarFamilia_23DB / ModificarFamilia_23DB: un componente Patente por llamada
IF OBJECT_ID('dbo.InsertarFamPat', 'P') IS NOT NULL DROP PROCEDURE dbo.InsertarFamPat;
GO
CREATE PROCEDURE dbo.InsertarFamPat
    @IdFamilia INT,
    @IdPatente INT
AS
BEGIN
    INSERT INTO FamPat_23DB (IdFamilia, IdPatente)
    VALUES (@IdFamilia, @IdPatente);
END
GO

-- InsertarFamilia_23DB / ModificarFamilia_23DB: una subfamilia por llamada
IF OBJECT_ID('dbo.InsertarFamFam', 'P') IS NOT NULL DROP PROCEDURE dbo.InsertarFamFam;
GO
CREATE PROCEDURE dbo.InsertarFamFam
    @IdFamiliaPadre INT,
    @IdFamiliaHija  INT
AS
BEGIN
    INSERT INTO FamFam_23DB (IdFamiliaPadre, IdFamiliaHija)
    VALUES (@IdFamiliaPadre, @IdFamiliaHija);
END
GO

-- ModificarFamilia_23DB(idFamilia, nombreFamilia, componentes): solo la cabecera
IF OBJECT_ID('dbo.ModificarFamilia', 'P') IS NOT NULL DROP PROCEDURE dbo.ModificarFamilia;
GO
CREATE PROCEDURE dbo.ModificarFamilia
    @IdFamilia      INT,
    @NombreFamilia  VARCHAR(50)
AS
BEGIN
    UPDATE Familia_23DB
    SET NombreFamilia = @NombreFamilia
    WHERE IdFamilia = @IdFamilia;
END
GO

-- ModificarFamilia_23DB / EliminarFamilia_23DB: limpia las patentes de la familia
IF OBJECT_ID('dbo.EliminarFamPatPorFamilia', 'P') IS NOT NULL DROP PROCEDURE dbo.EliminarFamPatPorFamilia;
GO
CREATE PROCEDURE dbo.EliminarFamPatPorFamilia
    @IdFamilia INT
AS
BEGIN
    DELETE FROM FamPat_23DB
    WHERE IdFamilia = @IdFamilia;
END
GO

-- ModificarFamilia_23DB: saca solo las subfamilias colgadas de esta familia
IF OBJECT_ID('dbo.EliminarFamFamPorPadre', 'P') IS NOT NULL DROP PROCEDURE dbo.EliminarFamFamPorPadre;
GO
CREATE PROCEDURE dbo.EliminarFamFamPorPadre
    @IdFamilia INT
AS
BEGIN
    DELETE FROM FamFam_23DB
    WHERE IdFamiliaPadre = @IdFamilia;
END
GO

-- EliminarFamilia_23DB: saca la familia de cualquier rol que la tenga asignada.
-- RolFam_23DB pertenece al agregado de Rol; se conserva el comportamiento actual.
IF OBJECT_ID('dbo.EliminarRolFamPorFamilia', 'P') IS NOT NULL DROP PROCEDURE dbo.EliminarRolFamPorFamilia;
GO
CREATE PROCEDURE dbo.EliminarRolFamPorFamilia
    @IdFamilia INT
AS
BEGIN
    DELETE FROM RolFam_23DB
    WHERE IdFamilia = @IdFamilia;
END
GO

-- EliminarFamilia_23DB: corta los vinculos en los dos sentidos, padre e hija
IF OBJECT_ID('dbo.EliminarFamFamPorFamilia', 'P') IS NOT NULL DROP PROCEDURE dbo.EliminarFamFamPorFamilia;
GO
CREATE PROCEDURE dbo.EliminarFamFamPorFamilia
    @IdFamilia INT
AS
BEGIN
    DELETE FROM FamFam_23DB
    WHERE IdFamiliaPadre = @IdFamilia
       OR IdFamiliaHija = @IdFamilia;
END
GO

-- EliminarFamilia_23DB(idFamilia): borra la cabecera, despues de las anteriores
IF OBJECT_ID('dbo.EliminarFamilia', 'P') IS NOT NULL DROP PROCEDURE dbo.EliminarFamilia;
GO
CREATE PROCEDURE dbo.EliminarFamilia
    @IdFamilia INT
AS
BEGIN
    DELETE FROM Familia_23DB
    WHERE IdFamilia = @IdFamilia;
END
GO

-- FamiliaEstaEnRol_23DB(idFamilia)
IF OBJECT_ID('dbo.FamiliaEstaEnRol', 'P') IS NOT NULL DROP PROCEDURE dbo.FamiliaEstaEnRol;
GO
CREATE PROCEDURE dbo.FamiliaEstaEnRol
    @IdFamilia INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS Cantidad
    FROM RolFam_23DB
    WHERE IdFamilia = @IdFamilia;
END
GO

-- FamiliaEstaEnFamilia_23DB(idFamilia)
IF OBJECT_ID('dbo.FamiliaEstaEnFamilia', 'P') IS NOT NULL DROP PROCEDURE dbo.FamiliaEstaEnFamilia;
GO
CREATE PROCEDURE dbo.FamiliaEstaEnFamilia
    @IdFamilia INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS Cantidad
    FROM FamFam_23DB
    WHERE IdFamiliaHija = @IdFamilia;
END
GO

-- ============================================================
-- mapperRespaldo_23DB
-- ============================================================

-- GenerarBackup_23DB(rutaCompleta)
-- @Ruta se escapa con QUOTENAME(@Ruta, '''') antes de armar el lote dinamico:
-- eso duplica cualquier comilla simple embebida y cierra la inyeccion que habia
-- al interpolar la ruta directamente en el BACKUP.
-- RestaurarBackup_23DB no tiene SP equivalente y queda con acceso directo: el
-- lote de restauracion corre contra master, y un SP alojado en INGSW_23DB no
-- puede restaurar la base que lo contiene.
IF OBJECT_ID('dbo.GenerarBackup', 'P') IS NOT NULL DROP PROCEDURE dbo.GenerarBackup;
GO
CREATE PROCEDURE dbo.GenerarBackup
    @Ruta NVARCHAR(260)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Sql NVARCHAR(MAX) =
        N'BACKUP DATABASE INGSW_23DB TO DISK = ' + QUOTENAME(@Ruta, '''') + N';';

    EXEC sys.sp_executesql @Sql;
END
GO
