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
    Nombre NVARCHAR(150) NOT NULL,
    Creditos INT NOT NULL DEFAULT 3
);
GO

CREATE TABLE Matricula (
    IdMatricula INT IDENTITY(1,1) PRIMARY KEY,
    IdAlumno INT NOT NULL,
    FechaMatricula DATETIME NOT NULL DEFAULT GETDATE(),
    Periodo NVARCHAR(50) NOT NULL,
    CONSTRAINT FK_Matricula_Alumno FOREIGN KEY (IdAlumno) REFERENCES Alumno(IdAlumno) ON DELETE CASCADE
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
('Matemáticas I', 4),
('Comunicación', 3),
('Programación I', 4);

INSERT INTO Alumno (Nombre, Apellido, DNI, FechaNacimiento, Direccion) VALUES
('Juan', 'Perez', '71717171', '2005-06-12', 'Av. Principal 100'),
('María', 'Gonzales', '81818181', '2006-03-01', 'Jr. Secundaria 45');
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
    @Nombre NVARCHAR(150),
    @Creditos INT,
    @NewId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Curso (Nombre, Creditos) VALUES (@Nombre, @Creditos);
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
    @Nombre NVARCHAR(150),
    @Creditos INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Curso SET Nombre = @Nombre, Creditos = @Creditos WHERE IdCurso = @IdCurso;
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

-- 6) Stored Procedures: MATRICULA
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