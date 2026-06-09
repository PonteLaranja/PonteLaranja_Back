CREATE DATABASE PonteLaranjaDb
GO

USE PonteLaranjaDb
GO

CREATE TABLE TipoUnidade(
TipoUnidadeID UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
NomeTipoUnidade VARCHAR(25) UNIQUE NOT NULL
)
GO

CREATE TABLE TipoUsuario(
TipoUsuarioID UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
NomeTipoUsuario VARCHAR(25) UNIQUE NOT NULL
)
GO

CREATE TABLE TipoEstadoTransferencia(
TipoEstadoTransferenciaID  UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
EstadoTransferencia VARCHAR(20) UNIQUE NOT NULL
)
GO

CREATE TABLE TipoMedida(
TipoMedidaID UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
NomeTipoMedida VARCHAR(25) UNIQUE NOT NULL
)
GO

CREATE TABLE Usuario(
UsuarioID UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
Nome VARCHAR(50) NOT NULL,
Email VARCHAR(150) UNIQUE NOT NULL, -- Blindado contra duplicidade
Senha VARBINARY(32) NOT NULL,
DataCriacao DATETIME2 DEFAULT GETDATE(),
TipoUsuarioID UNIQUEIDENTIFIER NOT NULL,

CONSTRAINT Fk_TipoUsuario FOREIGN KEY (TipoUsuarioID) REFERENCES TipoUsuario(TipoUsuarioID) ON DELETE CASCADE
)
GO

CREATE TABLE Unidade(
UnidadeID UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
NomeUnidade VARCHAR(50) UNIQUE NOT NULL,
CEP VARCHAR(10) NOT NULL,
UsuarioResponsavelID UNIQUEIDENTIFIER NOT NULL,

CONSTRAINT Fk_UsuarioResponsavelID FOREIGN KEY (UsuarioResponsavelID) REFERENCES Usuario(UsuarioID)
)
GO

CREATE TABLE TipoItem(
TipoItemID UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
NomeTipo VARCHAR(20) UNIQUE NOT NULL -- Adicionado UNIQUE para segurança das subconsultas
)
GO

CREATE TABLE Item(
ItemID UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
NomeItem VARCHAR(75) NOT NULL,
DataRecebimento DATETIME2 DEFAULT GETDATE(),
DataDoacao DATETIME2,
Medida INT NOT NULL,
Quantidade INT NOT NULL,
TipoMedidaID UNIQUEIDENTIFIER,
UnidadeID UNIQUEIDENTIFIER,
ResponsavelID UNIQUEIDENTIFIER,
TipoItemID UNIQUEIDENTIFIER,

CONSTRAINT Fk_TipoMedidaID FOREIGN KEY (TipoMedidaID) REFERENCES TipoMedida(TipoMedidaID),
CONSTRAINT Fk_UnidadeID FOREIGN KEY (UnidadeID) REFERENCES Unidade(UnidadeID),
CONSTRAINT Fk_ResponsavelID FOREIGN KEY (ResponsavelID) REFERENCES Usuario(UsuarioID),
CONSTRAINT Fk_TipoItemID FOREIGN KEY (TipoItemID) REFERENCES TipoItem(TipoItemID)
)
GO

CREATE TABLE Transferencia(
TransferenciaID UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
DataEnvio DATETIME2,
DataChegada DATETIME2,
UnidadeOrigemID UNIQUEIDENTIFIER NOT NULL,
UnidadeDestinoID UNIQUEIDENTIFIER NOT NULL,
TipoEstadoTransferenciaID UNIQUEIDENTIFIER NOT NULL,
ItemID UNIQUEIDENTIFIER NOT NULL,

CONSTRAINT Fk_UnidadeOrigemID FOREIGN KEY (UnidadeOrigemID) REFERENCES Unidade(UnidadeID),
CONSTRAINT Fk_UnidadeDestinoID FOREIGN KEY (UnidadeDestinoID) REFERENCES Unidade(UnidadeID),
CONSTRAINT Fk_TipoEstadoTransferenciaID FOREIGN KEY (TipoEstadoTransferenciaID) REFERENCES TipoEstadoTransferencia(TipoEstadoTransferenciaID) ON DELETE CASCADE,
CONSTRAINT Fk_ItemID FOREIGN KEY (ItemID) REFERENCES Item(ItemID) ON DELETE CASCADE
)
GO

/* =====================================================
   TRIGGER
   ===================================================== */
CREATE TRIGGER trg_Aprovacao_Item
ON Transferencia
AFTER Update 
AS BEGIN
    SET NOCOUNT ON
        UPDATE t
        SET DataChegada = GETDATE()
        FROM Transferencia t
        INNER JOIN inserted i
        ON t.TransferenciaID = i.TransferenciaID
        INNER JOIN deleted d
        ON d.TransferenciaID = i.TransferenciaID
        INNER JOIN TipoEstadoTransferencia novo
        ON i.TipoEstadoTransferenciaID = novo.TipoEstadoTransferenciaID
        INNER JOIN TipoEstadoTransferencia antigo
        ON d.TipoEstadoTransferenciaID = antigo.TipoEstadoTransferenciaID
        WHERE novo.EstadoTransferencia = 'Aprovado'
        AND antigo.EstadoTransferencia <> 'Aprovado';
END
GO

/* =====================================================
   INSERÇÃO DOS DADOS DE CONFIGURAÇÃO (TABELAS DE TIPOS)
   ===================================================== */

INSERT INTO TipoUsuario (NomeTipoUsuario) VALUES ('Sudo'), ('Responsavel'), ('Doador');
GO

INSERT INTO TipoEstadoTransferencia (EstadoTransferencia) VALUES ('Em andamento'), ('Aprovado'), ('Reprovado'), ('Cancelado'), ('Recebido');
GO

INSERT INTO TipoUnidade (NomeTipoUnidade) VALUES ('Coleta'), ('Distribuicao');
GO

INSERT INTO TipoMedida (NomeTipoMedida) VALUES ('Kg'), ('Unidade'), ('Caixa'), ('Pacote');
GO

-- Nova tabela de tipos populada aqui:
INSERT INTO TipoItem (NomeTipo) VALUES ('Alimentos'), ('Vestuário'), ('Brinquedos');
GO

/* =====================================================
   INSERÇÃO DE USUÁRIOS
   ===================================================== */
INSERT INTO Usuario (Nome, Email, Senha, TipoUsuarioID)
VALUES
('Administrador Geral', 'sudo@pontelaranja.org', HASHBYTES('MD5','123456'), (SELECT TipoUsuarioID FROM TipoUsuario WHERE NomeTipoUsuario = 'Sudo')),
('Maria Silva', 'maria@pontelaranja.org', HASHBYTES('MD5','123456'), (SELECT TipoUsuarioID FROM TipoUsuario WHERE NomeTipoUsuario = 'Responsavel')),
('Joao Pereira', 'joao@pontelaranja.org', HASHBYTES('MD5','123456'), (SELECT TipoUsuarioID FROM TipoUsuario WHERE NomeTipoUsuario = 'Responsavel')),
('Ana Souza', 'ana@email.com', HASHBYTES('MD5','123456'), (SELECT TipoUsuarioID FROM TipoUsuario WHERE NomeTipoUsuario = 'Doador')),
('Carlos Lima', 'carlos@email.com', HASHBYTES('MD5','123456'), (SELECT TipoUsuarioID FROM TipoUsuario WHERE NomeTipoUsuario = 'Doador'));
GO

/* =====================================================
   INSERÇÃO DE UNIDADES
   ===================================================== */
INSERT INTO Unidade (NomeUnidade, CEP, UsuarioResponsavelID)
VALUES
('Matriz Centro', '09000-000', (SELECT UsuarioID FROM Usuario WHERE Email = 'maria@pontelaranja.org')),
('Unidade Leste', '09100-000', (SELECT UsuarioID FROM Usuario WHERE Email = 'joao@pontelaranja.org'));
GO

/* =====================================================
   INSERÇÃO DE ITENS (Vinculados com os novos TipoItemID)
   ===================================================== */
INSERT INTO Item (NomeItem, DataDoacao, Medida, Quantidade, TipoMedidaID, UnidadeID, ResponsavelID, TipoItemID)
VALUES
(
    'Arroz 5kg',
    GETDATE(),
    5,
    100,
    (SELECT TipoMedidaID FROM TipoMedida WHERE NomeTipoMedida = 'Kg'),
    (SELECT UnidadeID FROM Unidade WHERE NomeUnidade = 'Matriz Centro'),
    (SELECT UsuarioID FROM Usuario WHERE Email = 'maria@pontelaranja.org'),
    (SELECT TipoItemID FROM TipoItem WHERE NomeTipo = 'Alimentos') -- ID de Alimentos incorporado
),
(
    'Camiseta Infantil',
    GETDATE(),
    1,
    80,
    (SELECT TipoMedidaID FROM TipoMedida WHERE NomeTipoMedida = 'Unidade'),
    (SELECT UnidadeID FROM Unidade WHERE NomeUnidade = 'Matriz Centro'),
    (SELECT UsuarioID FROM Usuario WHERE Email = 'maria@pontelaranja.org'),
    (SELECT TipoItemID FROM TipoItem WHERE NomeTipo = 'Vestuário') -- ID de Vestuário incorporado
),
(
    'Carrinho de Brinquedo',
    GETDATE(),
    1,
    30,
    (SELECT TipoMedidaID FROM TipoMedida WHERE NomeTipoMedida = 'Unidade'),
    (SELECT UnidadeID FROM Unidade WHERE NomeUnidade = 'Matriz Centro'),
    (SELECT UsuarioID FROM Usuario WHERE Email = 'maria@pontelaranja.org'),
    (SELECT TipoItemID FROM TipoItem WHERE NomeTipo = 'Brinquedos') -- ID de Brinquedos incorporado
);
GO

/* =====================================================
   INSERÇÃO DE TRANSFERÊNCIAS
   ===================================================== */
INSERT INTO Transferencia (DataEnvio, DataChegada, UnidadeOrigemID, UnidadeDestinoID, TipoEstadoTransferenciaID, ItemID)
VALUES
(
    DATEADD(DAY,-3,GETDATE()),
    GETDATE(),
    (SELECT UnidadeID FROM Unidade WHERE NomeUnidade = 'Matriz Centro'),
    (SELECT UnidadeID FROM Unidade WHERE NomeUnidade = 'Unidade Leste'),
    (SELECT TipoEstadoTransferenciaID FROM TipoEstadoTransferencia WHERE EstadoTransferencia = 'Recebido'),
    (SELECT ItemID FROM Item WHERE NomeItem = 'Arroz 5kg')
),
(
    GETDATE(),
    NULL,
    (SELECT UnidadeID FROM Unidade WHERE NomeUnidade = 'Matriz Centro'),
    (SELECT UnidadeID FROM Unidade WHERE NomeUnidade = 'Unidade Leste'),
    (SELECT TipoEstadoTransferenciaID FROM TipoEstadoTransferencia WHERE EstadoTransferencia = 'Em andamento'),
    (SELECT ItemID FROM Item WHERE NomeItem = 'Camiseta Infantil')
);
GO

/* =====================================================
   CONFERÊNCIA DOS DADOS (SELECT *)
   ===================================================== */
PRINT '--- TipoUsuario ---'; SELECT * FROM TipoUsuario;
PRINT '--- TipoEstadoTransferencia ---'; SELECT * FROM TipoEstadoTransferencia;
PRINT '--- TipoUnidade ---'; SELECT * FROM TipoUnidade;
PRINT '--- TipoMedida ---'; SELECT * FROM TipoMedida;
PRINT '--- TipoItem ---'; SELECT * FROM TipoItem;
PRINT '--- Usuario ---'; SELECT * FROM Usuario;
PRINT '--- Unidade ---'; SELECT * FROM Unidade;
PRINT '--- Item ---'; SELECT * FROM Item;
PRINT '--- Transferencia ---'; SELECT * FROM Transferencia;