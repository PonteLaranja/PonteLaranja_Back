USE master
GO

DROP DATABASE PonteLaranjaDb
GO

CREATE DATABASE PonteLaranjaDb
GO

USE PonteLaranjaDb
GO

CREATE TABLE TipoUsuario(
    TipoUsuarioID UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    NomeTipoUsuario VARCHAR(25) UNIQUE NOT NULL
)
GO

CREATE TABLE Usuario(
    UsuarioID UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    Nome VARCHAR(50) NOT NULL,
    Email VARCHAR(150) UNIQUE,
    Senha VARBINARY(32),
    DataCriacao DATETIME2 DEFAULT SYSDATETIME(),
    Ativo BIT DEFAULT 1 NOT NULL,
    TipoUsuarioID UNIQUEIDENTIFIER NOT NULL,

CONSTRAINT Fk_TipoUsuario FOREIGN KEY (TipoUsuarioID)
    REFERENCES TipoUsuario(TipoUsuarioID)
)
GO

CREATE TABLE TipoUnidade(
    TipoUnidadeID UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    NomeTipoUnidade VARCHAR(25) UNIQUE NOT NULL
)
GO

CREATE TABLE Unidade(
    UnidadeID UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    NomeUnidade VARCHAR(75) UNIQUE NOT NULL,
    CEP VARCHAR(10) NOT NULL,
    Ativo BIT DEFAULT 1 NOT NULL,
    UsuarioResponsavelID UNIQUEIDENTIFIER NOT NULL,
    TipoUnidadeID UNIQUEIDENTIFIER NOT NULL,

CONSTRAINT Fk_UsuarioResponsavelID FOREIGN KEY (UsuarioResponsavelID)
    REFERENCES Usuario(UsuarioID),
CONSTRAINT Fk_TipoUnidadeID FOREIGN KEY (TipoUnidadeID)
    REFERENCES TipoUnidade(TipoUnidadeID)
)
GO

CREATE TABLE TipoItem(
    TipoItemID UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    NomeTipoItem VARCHAR(25) UNIQUE NOT NULL
)
GO

CREATE TABLE TipoMedida(
    TipoMedidaID UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    NomeTipoMedida VARCHAR(25) UNIQUE NOT NULL
)
GO

CREATE TABLE Item(
    ItemID UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    NomeItem VARCHAR(75) NOT NULL,
    DataRecebimento DATETIME2 DEFAULT SYSDATETIME(),
    DataDoacao DATETIME2,
    Medida DECIMAL (6,3) NOT NULL,
    Quantidade INT NOT NULL,
    Ativo BIT DEFAULT 1 NOT NULL,
    TipoItemID UNIQUEIDENTIFIER,
    TipoMedidaID UNIQUEIDENTIFIER,
    UsuarioID UNIQUEIDENTIFIER,
    UnidadeID UNIQUEIDENTIFIER,

CONSTRAINT Fk_TipoItemID FOREIGN KEY (TipoItemID)
    REFERENCES TipoItem(TipoItemID),
CONSTRAINT Fk_TipoMedidaID FOREIGN KEY (TipoMedidaID)
    REFERENCES TipoMedida(TipoMedidaID),
CONSTRAINT Fk_UnidadeID FOREIGN KEY (UnidadeID)
    REFERENCES Unidade(UnidadeID),
CONSTRAINT Fk_UsuarioID FOREIGN KEY (UsuarioID)
    REFERENCES Usuario(UsuarioID),
)
GO

CREATE TABLE EstadoTransferencia(
    EstadoTransID  UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    EstadoTransNome VARCHAR(25) UNIQUE NOT NULL
)
GO

CREATE TABLE Transferencia(
    TransferenciaID UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    DataEnvio DATETIME2 DEFAULT SYSDATETIME(),
    DataChegada DATETIME2,
    UnidadeOrigemID UNIQUEIDENTIFIER NOT NULL,
    UnidadeDestinoID UNIQUEIDENTIFIER NOT NULL,
    EstadoTransferenciaID UNIQUEIDENTIFIER NOT NULL,
    ItemID UNIQUEIDENTIFIER NOT NULL,

CONSTRAINT Fk_UnidadeOrigemID FOREIGN KEY (UnidadeOrigemID)
    REFERENCES Unidade(UnidadeID),
CONSTRAINT Fk_UnidadeDestinoID FOREIGN KEY (UnidadeDestinoID)
    REFERENCES Unidade(UnidadeID),
CONSTRAINT Fk_EstadoTransferenciaID FOREIGN KEY (EstadoTransferenciaID)
    REFERENCES EstadoTransferencia(EstadoTransID),
CONSTRAINT Fk_ItemID FOREIGN KEY (ItemID) REFERENCES Item(ItemID)
)
GO

/* =====================================================
   TRIGGERS
   ===================================================== */
/* =====================================================
   TRIGGER SOFT DELETE: USUARIO
   ===================================================== */
CREATE TRIGGER trg_SoftDelete_Usuario
ON Usuario
INSTEAD OF DELETE
AS BEGIN
    SET NOCOUNT ON;
    
    UPDATE Usuario
    SET Ativo = 0
    WHERE UsuarioID IN (SELECT UsuarioID FROM deleted);
END
GO

/* =====================================================
   TRIGGER SOFT DELETE: UNIDADE
   ===================================================== */
CREATE TRIGGER trg_SoftDelete_Unidade
ON Unidade
INSTEAD OF DELETE
AS BEGIN
    SET NOCOUNT ON;
    
    UPDATE Unidade
    SET Ativo = 0
    WHERE UnidadeID IN (SELECT UnidadeID FROM deleted);
END
GO

/* =====================================================
   TRIGGER SOFT DELETE: ITEM
   ===================================================== */
CREATE TRIGGER trg_SoftDelete_Item
ON Item
INSTEAD OF DELETE
AS BEGIN
    SET NOCOUNT ON;
    
    UPDATE Item
    SET Ativo = 0
    WHERE ItemID IN (SELECT ItemID FROM deleted);
END
GO

/* =====================================================
   INSERCAO DOS ATRIBUTOS INERENTES DO SISTEMA
   ===================================================== */

INSERT INTO TipoUsuario (NomeTipoUsuario) VALUES ('Administrador'), ('Responsavel'), ('Doador');
GO

INSERT INTO EstadoTransferencia (EstadoTransNome) VALUES ('Pendente'), ('Aprovado'), ('Reprovado'), ('Cancelado'), ('Recebido');
GO

INSERT INTO TipoUnidade (NomeTipoUnidade) VALUES ('Coleta'), ('Distribuicao');
GO

INSERT INTO TipoMedida (NomeTipoMedida) VALUES ('Kg'), ('Nulo'), ('Litro'), ('PP'), ('P'), ('M'), ('G'), ('GG');
GO


INSERT INTO TipoItem (NomeTipoItem) VALUES ('Alimento'), ('Bebida'), ('Roupa'), ('Brinquedo');
GO

SELECT * FROM TipoItem
SELECT * FROM EstadoTransferencia
SELECT * FROM TipoUnidade
SELECT * FROM TipoUsuario

/* =====================================================
   INSERCAO DE ATRIBUTOS INERENTES PARA TESTE
   ===================================================== */

/* =====================================================
   INSERCAO DE USUARIOS
   ===================================================== */
INSERT INTO Usuario (Nome, Email, Senha, TipoUsuarioID)
VALUES
('Administrador Geral', 'sudo@pontelaranja.org', HASHBYTES('MD5','123456'), (SELECT TipoUsuarioID FROM TipoUsuario WHERE NomeTipoUsuario = 'Administrador')),
('Maria Silva', 'maria@pontelaranja.org', HASHBYTES('MD5','123456'), (SELECT TipoUsuarioID FROM TipoUsuario WHERE NomeTipoUsuario = 'Responsavel')),
('Joao Pereira', 'joao@pontelaranja.org', HASHBYTES('MD5','123456'), (SELECT TipoUsuarioID FROM TipoUsuario WHERE NomeTipoUsuario = 'Responsavel')),
('Ana Souza', 'ana@email.com', HASHBYTES('MD5','123456'), (SELECT TipoUsuarioID FROM TipoUsuario WHERE NomeTipoUsuario = 'Doador')),
('Carlos Lima', 'carlos@email.com', HASHBYTES('MD5','123456'), (SELECT TipoUsuarioID FROM TipoUsuario WHERE NomeTipoUsuario = 'Doador'));
GO

/* =====================================================
   INSERCAO DE UNIDADES
   ===================================================== */

INSERT INTO Unidade (NomeUnidade, CEP, UsuarioResponsavelID, TipoUnidadeID)
VALUES
('Matriz Centro', '09000-000', (SELECT UsuarioID FROM Usuario WHERE Email = 'maria@pontelaranja.org'), (SELECT TipoUnidadeID FROM TipoUnidade WHERE NomeTipoUnidade = 'Coleta')),
('Unidade Leste', '09100-000', (SELECT UsuarioID FROM Usuario WHERE Email = 'joao@pontelaranja.org'), (SELECT TipoUnidadeID FROM TipoUnidade WHERE NomeTipoUnidade = 'Distribuicao'));
GO

/* =====================================================
   INSERCAO DE ITENS 
   ===================================================== */
INSERT INTO Item (
    NomeItem,
    DataRecebimento,
    Medida,
    Quantidade,
    TipoMedidaID,
    UnidadeID,
    UsuarioID,
    TipoItemID
)
VALUES
(
    'Arroz 5kg',
    GETDATE(),
    5,
    100,
    (SELECT TipoMedidaID FROM TipoMedida WHERE NomeTipoMedida = 'Kg'),
    (SELECT UnidadeID FROM Unidade WHERE NomeUnidade = 'Matriz Centro'),
    (SELECT UsuarioID FROM Usuario WHERE Email = 'maria@pontelaranja.org'),
    (SELECT TipoItemID FROM TipoItem WHERE NomeTipoItem = 'Alimento')
),
(
    'Camiseta Infantil',
    GETDATE(),
    1,
    80,
    (SELECT TipoMedidaID FROM TipoMedida WHERE NomeTipoMedida = 'Kg'),
    (SELECT UnidadeID FROM Unidade WHERE NomeUnidade = 'Matriz Centro'),
    (SELECT UsuarioID FROM Usuario WHERE Email = 'maria@pontelaranja.org'),
    (SELECT TipoItemID FROM TipoItem WHERE NomeTipoItem = 'Roupa')
),
(
    'Carrinho de Brinquedo',
    GETDATE(),
    1,
    30,
    (SELECT TipoMedidaID FROM TipoMedida WHERE NomeTipoMedida = 'Kg'),
    (SELECT UnidadeID FROM Unidade WHERE NomeUnidade = 'Matriz Centro'),
    (SELECT UsuarioID FROM Usuario WHERE Email = 'maria@pontelaranja.org'),
    (SELECT TipoItemID FROM TipoItem WHERE NomeTipoItem = 'Brinquedo')
);
GO

/* =====================================================
   INSERCAO DE TRANSFERENCIAS
   ===================================================== */
INSERT INTO Transferencia (DataEnvio, DataChegada, UnidadeOrigemID, UnidadeDestinoID, EstadoTransferenciaID, ItemID)
VALUES
(
    DATEADD(DAY,-3,GETDATE()),
    GETDATE(),
    (SELECT UnidadeID FROM Unidade WHERE NomeUnidade = 'Matriz Centro'),
    (SELECT UnidadeID FROM Unidade WHERE NomeUnidade = 'Unidade Leste'),
    (SELECT EstadoTransID FROM EstadoTransferencia WHERE EstadoTransNome = 'Recebido'),
    (SELECT ItemID FROM Item WHERE NomeItem = 'Arroz 5kg')
),
(
    GETDATE(),
    NULL,
    (SELECT UnidadeID FROM Unidade WHERE NomeUnidade = 'Matriz Centro'),
    (SELECT UnidadeID FROM Unidade WHERE NomeUnidade = 'Unidade Leste'),
    (SELECT EstadoTransID FROM EstadoTransferencia WHERE EstadoTransNome = 'Pendente'),
    (SELECT ItemID FROM Item WHERE NomeItem = 'Camiseta Infantil')
);
GO

/* =====================================================
   CONFERENCIA DE DADOS
   ===================================================== */
PRINT '--- TipoUsuario ---'; SELECT * FROM TipoUsuario;
PRINT '--- EstadoTransferencia ---'; SELECT * FROM EstadoTransferencia;
PRINT '--- TipoUnidade ---'; SELECT * FROM TipoUnidade;
PRINT '--- TipoMedida ---'; SELECT * FROM TipoMedida;
PRINT '--- TipoItem ---'; SELECT * FROM TipoItem;
PRINT '--- Usuario ---'; SELECT * FROM Usuario;
PRINT '--- Unidade ---'; SELECT * FROM Unidade;
PRINT '--- Item ---'; SELECT * FROM Item;
PRINT '--- Transferencia ---'; SELECT * FROM Transferencia;