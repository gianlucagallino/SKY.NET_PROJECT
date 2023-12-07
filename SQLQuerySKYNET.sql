drop DATABASE SKYNET
USE SKYNET

CREATE TABLE Partidas (
    PartidaID INT IDENTITY (1,1),

	PRIMARY KEY (PartidaID)
);

CREATE TABLE Operadores (
    OperadorID INT IDENTITY (1,1),
    PartidaID INT, 
    Nombre VARCHAR(50),
    KilometrosRecorridos FLOAT,
    EnergiaConsumida FLOAT,
    CargaTransportada FLOAT,
    InstruccionesEjecutadas INT,
    DaniosRecibidos INT,
    UltimoLugar1 INT,
    UltimoLugar2 INT,
    UltimoLugar3 INT,
    
    PRIMARY KEY (OperadorID),
    FOREIGN KEY (PartidaID) REFERENCES Partidas(PartidaID)
);


CREATE PROCEDURE InsertPartida
AS
BEGIN
    INSERT INTO Partidas DEFAULT VALUES
END;

EXEC InsertPartida


CREATE PROCEDURE InsertOperator
    @Nombre VARCHAR(50),
    @KilometrosRecorridos FLOAT,
    @EnergiaConsumida FLOAT,
    @CargaTransportada FLOAT,
    @InstruccionesEjecutadas INT,
    @DaniosRecibidos INT,
    @UltimoLugar1 INT,
    @UltimoLugar2 INT,
    @UltimoLugar3 INT,
	@PartidaID INT
AS
BEGIN
    INSERT INTO Operators (Nombre, KilometrosRecorridos, EnergiaConsumida, CargaTransportada, InstruccionesEjecutadas, DaniosRecibidos, UltimoLugar1, UltimoLugar2, UltimoLugar3, PartidaID)
    VALUES (@Nombre, @KilometrosRecorridos, @EnergiaConsumida, @CargaTransportada, @InstruccionesEjecutadas, @DaniosRecibidos, @UltimoLugar1, @UltimoLugar2, @UltimoLugar3, @PartidaID);

END;

CREATE PROCEDURE GetNextGame
AS
BEGIN
    DECLARE @NextGameID INT;

    SELECT @NextGameID = ISNULL(MAX(PartidaID), 0) + 1
    FROM Partidas;

    RETURN @NextGameID;
END;

create procedure GetGame
@next int output
as
begin
set @next = (select max(PartidaID)+1 from Partidas);
end

CREATE PROCEDURE InsertOperatorAndGame
    @Nombre VARCHAR(50),
    @KilometrosRecorridos FLOAT,
    @EnergiaConsumida FLOAT,
    @CargaTransportada FLOAT,
    @InstruccionesEjecutadas INT,
    @DaniosRecibidos INT,
    @UltimoLugar1 INT,
    @UltimoLugar2 INT,
    @UltimoLugar3 INT
AS
BEGIN
    DECLARE @PartidaID INT;

    -- Obtener el próximo número de partida
    EXEC GetNextGame @PartidaID OUTPUT;

    -- Insertar en la tabla Partidas
    INSERT INTO Partidas (PartidaID) VALUES (@PartidaID);

    -- Insertar en la tabla Operators
    INSERT INTO Operators (Nombre, KilometrosRecorridos, EnergiaConsumida, CargaTransportada, InstruccionesEjecutadas, DaniosRecibidos, UltimoLugar1, UltimoLugar2, UltimoLugar3, PartidaID)
    VALUES (@Nombre, @KilometrosRecorridos, @EnergiaConsumida, @CargaTransportada, @InstruccionesEjecutadas, @DaniosRecibidos, @UltimoLugar1, @UltimoLugar2, @UltimoLugar3, @PartidaID);
END;