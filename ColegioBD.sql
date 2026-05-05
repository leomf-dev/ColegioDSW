-- Base de Datos: ColegioDB
-- 1) Crear BD
IF DB_ID('ColegioDB') IS NULL
BEGIN
    CREATE DATABASE ColegioDB;
END
GO

USE ColegioDB;
GO

-- 2) Tablas
CREATE TABLE Alumno (
    IdAlumno INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    DNI NVARCHAR(20) NOT NULL UNIQUE,
    FechaNacimiento DATE NULL,
    Direccion NVARCHAR(250) NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE Curso (
    IdCurso INT IDENTITY(1,1) PRIMARY KEY,
    Codigo NVARCHAR(50) NULL,
    Nombre NVARCHAR(150) NOT NULL,
    Creditos INT NOT NULL DEFAULT 3,
    HorasSemanales INT NOT NULL DEFAULT 0,
    Estado NVARCHAR(20) NOT NULL DEFAULT 'Activo'
);
GO

CREATE TABLE Docente (
    IdDocente INT IDENTITY(1,1) PRIMARY KEY,
    Nombres NVARCHAR(100) NOT NULL,
    Apellidos NVARCHAR(100) NOT NULL,
    Especialidad NVARCHAR(100) NOT NULL,
    Estado NVARCHAR(20) NOT NULL DEFAULT 'Activo'
);
GO

CREATE TABLE Seccion (
    IdSeccion INT IDENTITY(1,1) PRIMARY KEY,
    IdCurso INT NOT NULL,
    IdDocente INT NOT NULL,
    CodigoSeccion NVARCHAR(20) NOT NULL UNIQUE,
    PeriodoAcademico NVARCHAR(50) NOT NULL,
    CapacidadMaxima INT NOT NULL DEFAULT 30,
    CupoDisponible INT NOT NULL DEFAULT 0,
    Estado NVARCHAR(20) NOT NULL DEFAULT 'Activa' CHECK (Estado IN ('Activa', 'Activo', 'Cerrada', 'Cancelada')),
    CONSTRAINT FK_Seccion_Curso FOREIGN KEY (IdCurso) REFERENCES Curso(IdCurso) ON DELETE CASCADE,
    CONSTRAINT FK_Seccion_Docente FOREIGN KEY (IdDocente) REFERENCES Docente(IdDocente) ON DELETE NO ACTION
);
GO

CREATE TABLE Horario (
    IdHorario INT IDENTITY(1,1) PRIMARY KEY,
    IdSeccion INT NOT NULL,
    Dia NVARCHAR(20) NOT NULL CHECK (Dia IN ('Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado', 'Domingo')),
    HoraInicio TIME NOT NULL,
    HoraFin TIME NOT NULL,
    CONSTRAINT FK_Horario_Seccion FOREIGN KEY (IdSeccion) REFERENCES Seccion(IdSeccion) ON DELETE CASCADE,
    CONSTRAINT CHK_Horario_Horas CHECK (HoraInicio < HoraFin)
);
GO

CREATE TABLE Matricula (
    IdMatricula INT IDENTITY(1,1) PRIMARY KEY,
    IdAlumno INT NOT NULL,
    FechaMatricula DATETIME NOT NULL DEFAULT GETDATE(),
    Periodo NVARCHAR(50) NOT NULL,
    Estado NVARCHAR(20) NOT NULL DEFAULT 'Activa' CHECK (Estado IN ('Activa', 'Anulada', 'Cancelada')),
    FechaEstado DATETIME NULL,
    Observacion NVARCHAR(500) NULL,
    CONSTRAINT FK_Matricula_Alumno FOREIGN KEY (IdAlumno) REFERENCES Alumno(IdAlumno) ON DELETE CASCADE
);
GO

CREATE TABLE DetalleMatricula (
    IdDetalleMatricula INT IDENTITY(1,1) PRIMARY KEY,
    IdMatricula INT NOT NULL,
    IdSeccion INT NOT NULL,
    Estado NVARCHAR(20) NOT NULL DEFAULT 'Activo' CHECK (Estado IN ('Activo', 'Retirado')),
    CONSTRAINT FK_DetalleMatricula_Matricula FOREIGN KEY (IdMatricula) REFERENCES Matricula(IdMatricula) ON DELETE CASCADE,
    CONSTRAINT FK_DetalleMatricula_Seccion FOREIGN KEY (IdSeccion) REFERENCES Seccion(IdSeccion) ON DELETE NO ACTION
);
GO

CREATE TABLE Nota (
    IdNota INT IDENTITY(1,1) PRIMARY KEY,
    IdMatricula INT NOT NULL,
    IdCurso INT NOT NULL,
    Calificacion DECIMAL(5,2) NOT NULL CHECK (Calificacion >= 0 AND Calificacion <= 20),
    CONSTRAINT FK_Nota_Matricula FOREIGN KEY (IdMatricula) REFERENCES Matricula(IdMatricula) ON DELETE CASCADE,
    CONSTRAINT FK_Nota_Curso FOREIGN KEY (IdCurso) REFERENCES Curso(IdCurso) ON DELETE NO ACTION
);
GO

-- 3) Datos de ejemplo
INSERT INTO Curso (Nombre, Creditos) VALUES
('Matem�ticas I', 4),
('Comunicaci�n', 3),
('Programaci�n I', 4);

INSERT INTO Alumno (Nombre, Apellido, DNI, FechaNacimiento, Direccion) VALUES
('Juan', 'Perez', '71717171', '2005-06-12', 'Av. Principal 100'),
('Mar�a', 'Gonzales', '81818181', '2006-03-01', 'Jr. Secundaria 45');
GO

-- Matriculas ejemplo
INSERT INTO Matricula (IdAlumno, Periodo) VALUES (1, '2025-1'), (2, '2025-1');
GO

-- Notas ejemplo
INSERT INTO Nota (IdMatricula, IdCurso, Calificacion) VALUES (1, 1, 15.5), (1, 2, 18.0), (2, 1, 12.0);
GO

-- 4) Stored Procedures: ALUMNO
-- Create
CREATE PROCEDURE sp_Alumno_Insert
    @Nombre NVARCHAR(100),
    @Apellido NVARCHAR(100),
    @DNI NVARCHAR(20),
    @FechaNacimiento DATE = NULL,
    @Direccion NVARCHAR(250) = NULL,
    @NewId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Alumno (Nombre, Apellido, DNI, FechaNacimiento, Direccion)
    VALUES (@Nombre, @Apellido, @DNI, @FechaNacimiento, @Direccion);

    SET @NewId = SCOPE_IDENTITY();
END
GO

-- Read all
CREATE PROCEDURE sp_Alumno_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Alumno ORDER BY Apellido, Nombre;
END
GO

-- Read by id
CREATE PROCEDURE sp_Alumno_GetById
    @IdAlumno INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Alumno WHERE IdAlumno = @IdAlumno;
END
GO

-- Update
CREATE PROCEDURE sp_Alumno_Update
    @IdAlumno INT,
    @Nombre NVARCHAR(100),
    @Apellido NVARCHAR(100),
    @DNI NVARCHAR(20),
    @FechaNacimiento DATE = NULL,
    @Direccion NVARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Alumno
    SET Nombre = @Nombre, Apellido = @Apellido, DNI = @DNI, FechaNacimiento = @FechaNacimiento, Direccion = @Direccion
    WHERE IdAlumno = @IdAlumno;
END
GO

-- Delete
CREATE PROCEDURE sp_Alumno_Delete
    @IdAlumno INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Alumno WHERE IdAlumno = @IdAlumno;
END
GO

-- 5) Stored Procedures: CURSO
CREATE PROCEDURE sp_Curso_Insert
    @Codigo NVARCHAR(50),
    @Nombre NVARCHAR(150),
    @Creditos INT,
    @HorasSemanales INT,
    @Estado NVARCHAR(20),
    @NewId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Curso (Codigo, Nombre, Creditos, HorasSemanales, Estado)
    VALUES (@Codigo, @Nombre, @Creditos, @HorasSemanales, @Estado);
    SET @NewId = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE sp_Curso_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Curso ORDER BY Nombre;
END
GO

CREATE PROCEDURE sp_Curso_GetById
    @IdCurso INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Curso WHERE IdCurso = @IdCurso;
END
GO

CREATE PROCEDURE sp_Curso_Update
    @IdCurso INT,
    @Codigo NVARCHAR(50),
    @Nombre NVARCHAR(150),
    @Creditos INT,
    @HorasSemanales INT,
    @Estado NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Curso
    SET Codigo = @Codigo,
        Nombre = @Nombre,
        Creditos = @Creditos,
        HorasSemanales = @HorasSemanales,
        Estado = @Estado
    WHERE IdCurso = @IdCurso;
END
GO

CREATE PROCEDURE sp_Curso_Delete
    @IdCurso INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Curso WHERE IdCurso = @IdCurso;
END
GO

-- 6) Stored Procedures: DOCENTE
CREATE PROCEDURE sp_Docente_Insert
    @Nombres NVARCHAR(100),
    @Apellidos NVARCHAR(100),
    @Especialidad NVARCHAR(100),
    @Estado NVARCHAR(20),
    @NewId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Docente (Nombres, Apellidos, Especialidad, Estado)
    VALUES (@Nombres, @Apellidos, @Especialidad, @Estado);
    SET @NewId = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE sp_Docente_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Docente ORDER BY Apellidos, Nombres;
END
GO

CREATE PROCEDURE sp_Docente_GetById
    @IdDocente INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Docente WHERE IdDocente = @IdDocente;
END
GO

CREATE PROCEDURE sp_Docente_Update
    @IdDocente INT,
    @Nombres NVARCHAR(100),
    @Apellidos NVARCHAR(100),
    @Especialidad NVARCHAR(100),
    @Estado NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Docente
    SET Nombres = @Nombres,
        Apellidos = @Apellidos,
        Especialidad = @Especialidad,
        Estado = @Estado
    WHERE IdDocente = @IdDocente;
END
GO

CREATE PROCEDURE sp_Docente_Delete
    @IdDocente INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Docente WHERE IdDocente = @IdDocente;
END
GO

-- 7) Stored Procedures: SECCION
CREATE PROCEDURE sp_Seccion_Insert
    @IdCurso INT,
    @IdDocente INT,
    @CodigoSeccion NVARCHAR(20),
    @PeriodoAcademico NVARCHAR(50),
    @CapacidadMaxima INT,
    @CupoDisponible INT = NULL,
    @Estado NVARCHAR(20),
    @NewId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Seccion (IdCurso, IdDocente, CodigoSeccion, PeriodoAcademico, CapacidadMaxima, CupoDisponible, Estado)
    VALUES (@IdCurso, @IdDocente, @CodigoSeccion, @PeriodoAcademico, @CapacidadMaxima, ISNULL(@CupoDisponible, @CapacidadMaxima), @Estado);
    SET @NewId = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE sp_Seccion_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT s.*, c.Nombre as NombreCurso, c.Codigo as CodigoCurso,
           d.Nombres + ' ' + d.Apellidos as NombreDocente
    FROM Seccion s
    INNER JOIN Curso c ON s.IdCurso = c.IdCurso
    INNER JOIN Docente d ON s.IdDocente = d.IdDocente
    ORDER BY s.CodigoSeccion;
END
GO

CREATE PROCEDURE sp_Seccion_GetById
    @IdSeccion INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT s.*, c.Nombre as NombreCurso, c.Codigo as CodigoCurso,
           d.Nombres + ' ' + d.Apellidos as NombreDocente
    FROM Seccion s
    INNER JOIN Curso c ON s.IdCurso = c.IdCurso
    INNER JOIN Docente d ON s.IdDocente = d.IdDocente
    WHERE s.IdSeccion = @IdSeccion;
END
GO

CREATE PROCEDURE sp_Seccion_Update
    @IdSeccion INT,
    @IdCurso INT,
    @IdDocente INT,
    @CodigoSeccion NVARCHAR(20),
    @PeriodoAcademico NVARCHAR(50),
    @CapacidadMaxima INT,
    @CupoDisponible INT = NULL,
    @Estado NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Seccion
    SET IdCurso = @IdCurso,
        IdDocente = @IdDocente,
        CodigoSeccion = @CodigoSeccion,
        PeriodoAcademico = @PeriodoAcademico,
        CapacidadMaxima = @CapacidadMaxima,
        CupoDisponible = ISNULL(@CupoDisponible, CupoDisponible),
        Estado = @Estado
    WHERE IdSeccion = @IdSeccion;
END
GO

CREATE PROCEDURE sp_Seccion_Delete
    @IdSeccion INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Seccion WHERE IdSeccion = @IdSeccion;
END
GO

CREATE PROCEDURE sp_Horario_Insert
    @IdSeccion INT,
    @Dia NVARCHAR(20),
    @HoraInicio TIME,
    @HoraFin TIME,
    @NewId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Horario (IdSeccion, Dia, HoraInicio, HoraFin)
    VALUES (@IdSeccion, @Dia, @HoraInicio, @HoraFin);
    SET @NewId = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE sp_Horario_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT h.*, s.CodigoSeccion AS NombreSeccion
    FROM Horario h
    INNER JOIN Seccion s ON h.IdSeccion = s.IdSeccion
    ORDER BY s.CodigoSeccion, h.Dia, h.HoraInicio;
END
GO

CREATE PROCEDURE sp_Horario_GetById
    @IdHorario INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT h.*, s.CodigoSeccion AS NombreSeccion
    FROM Horario h
    INNER JOIN Seccion s ON h.IdSeccion = s.IdSeccion
    WHERE h.IdHorario = @IdHorario;
END
GO

CREATE PROCEDURE sp_Horario_GetBySeccion
    @IdSeccion INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT h.*, s.CodigoSeccion AS NombreSeccion
    FROM Horario h
    INNER JOIN Seccion s ON h.IdSeccion = s.IdSeccion
    WHERE h.IdSeccion = @IdSeccion
    ORDER BY h.Dia, h.HoraInicio;
END
GO

CREATE PROCEDURE sp_Horario_Update
    @IdHorario INT,
    @IdSeccion INT,
    @Dia NVARCHAR(20),
    @HoraInicio TIME,
    @HoraFin TIME
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Horario SET
        IdSeccion = @IdSeccion,
        Dia = @Dia,
        HoraInicio = @HoraInicio,
        HoraFin = @HoraFin
    WHERE IdHorario = @IdHorario;
END
GO

CREATE PROCEDURE sp_Horario_Delete
    @IdHorario INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Horario WHERE IdHorario = @IdHorario;
END
GO

-- 8) Stored Procedures: MATRICULA
CREATE PROCEDURE sp_Matricula_Insert
    @IdAlumno INT,
    @Periodo NVARCHAR(50),
    @NewId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Matricula (IdAlumno, Periodo) VALUES (@IdAlumno, @Periodo);
    SET @NewId = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE sp_Matricula_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT m.*, a.Nombre, a.Apellido, a.DNI
    FROM Matricula m
    INNER JOIN Alumno a ON m.IdAlumno = a.IdAlumno
    ORDER BY m.FechaMatricula DESC;
END
GO

CREATE PROCEDURE sp_Matricula_GetById
    @IdMatricula INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Matricula WHERE IdMatricula = @IdMatricula;
END
GO

CREATE PROCEDURE sp_Matricula_Delete
    @IdMatricula INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Matricula WHERE IdMatricula = @IdMatricula;
END
GO

-- 8) Stored Procedures: DETALLEMATRICULA
CREATE PROCEDURE sp_DetalleMatricula_Insert
    @IdMatricula INT,
    @IdSeccion INT,
    @Estado NVARCHAR(20),
    @NewId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO DetalleMatricula (IdMatricula, IdSeccion, Estado)
    VALUES (@IdMatricula, @IdSeccion, @Estado);
    SET @NewId = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE sp_DetalleMatricula_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT dm.*, m.IdAlumno, s.CodigoSeccion, c.Nombre AS NombreCurso,
           d.Nombres + ' ' + d.Apellidos AS NombreDocente
    FROM DetalleMatricula dm
    INNER JOIN Matricula m ON dm.IdMatricula = m.IdMatricula
    INNER JOIN Seccion s ON dm.IdSeccion = s.IdSeccion
    INNER JOIN Curso c ON s.IdCurso = c.IdCurso
    INNER JOIN Docente d ON s.IdDocente = d.IdDocente
    ORDER BY dm.IdMatricula, s.CodigoSeccion;
END
GO

CREATE PROCEDURE sp_DetalleMatricula_GetById
    @IdDetalleMatricula INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT dm.*, m.IdAlumno, s.CodigoSeccion, c.Nombre AS NombreCurso,
           d.Nombres + ' ' + d.Apellidos AS NombreDocente
    FROM DetalleMatricula dm
    INNER JOIN Matricula m ON dm.IdMatricula = m.IdMatricula
    INNER JOIN Seccion s ON dm.IdSeccion = s.IdSeccion
    INNER JOIN Curso c ON s.IdCurso = c.IdCurso
    INNER JOIN Docente d ON s.IdDocente = d.IdDocente
    WHERE dm.IdDetalleMatricula = @IdDetalleMatricula;
END
GO

CREATE PROCEDURE sp_DetalleMatricula_GetByMatricula
    @IdMatricula INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT dm.*, s.CodigoSeccion, c.Nombre AS NombreCurso,
           d.Nombres + ' ' + d.Apellidos AS NombreDocente
    FROM DetalleMatricula dm
    INNER JOIN Seccion s ON dm.IdSeccion = s.IdSeccion
    INNER JOIN Curso c ON s.IdCurso = c.IdCurso
    INNER JOIN Docente d ON s.IdDocente = d.IdDocente
    WHERE dm.IdMatricula = @IdMatricula
    ORDER BY s.CodigoSeccion;
END
GO

CREATE PROCEDURE sp_DetalleMatricula_Update
    @IdDetalleMatricula INT,
    @IdMatricula INT,
    @IdSeccion INT,
    @Estado NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE DetalleMatricula
    SET IdMatricula = @IdMatricula,
        IdSeccion = @IdSeccion,
        Estado = @Estado
    WHERE IdDetalleMatricula = @IdDetalleMatricula;
END
GO

CREATE PROCEDURE sp_DetalleMatricula_UpdateEstado
    @IdDetalleMatricula INT,
    @Estado NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE DetalleMatricula SET Estado = @Estado WHERE IdDetalleMatricula = @IdDetalleMatricula;
END
GO

CREATE PROCEDURE sp_DetalleMatricula_Delete
    @IdDetalleMatricula INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM DetalleMatricula WHERE IdDetalleMatricula = @IdDetalleMatricula;
END
GO

-- 7) Stored Procedures: NOTA
CREATE PROCEDURE sp_Nota_Insert
    @IdMatricula INT,
    @IdCurso INT,
    @Calificacion DECIMAL(5,2),
    @NewId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Nota (IdMatricula, IdCurso, Calificacion) VALUES (@IdMatricula, @IdCurso, @Calificacion);
    SET @NewId = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE sp_Nota_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT n.*, m.IdAlumno, c.Nombre AS CursoNombre
    FROM Nota n
    INNER JOIN Matricula m ON n.IdMatricula = m.IdMatricula
    INNER JOIN Curso c ON n.IdCurso = c.IdCurso
    ORDER BY n.IdNota DESC;
END
GO

CREATE PROCEDURE sp_Nota_GetById
    @IdNota INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Nota WHERE IdNota = @IdNota;
END
GO

CREATE PROCEDURE sp_Nota_Update
    @IdNota INT,
    @Calificacion DECIMAL(5,2)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Nota SET Calificacion = @Calificacion WHERE IdNota = @IdNota;
END
GO

CREATE PROCEDURE sp_Nota_Delete
    @IdNota INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Nota WHERE IdNota = @IdNota;
END
GO