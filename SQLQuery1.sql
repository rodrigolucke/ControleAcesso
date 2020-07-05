

-- drop table pessoa
CREATE TABLE pessoa(
    id_pessoa INT IDENTITY PRIMARY KEY NOT NULL,
    nome VARCHAR(100) NOT NULL,
    sobrenome VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL,
    cpf VARCHAR(100) NOT NULL
);

-- drop table usuario
CREATE TABLE usuario(
    id_usuario INT IDENTITY PRIMARY KEY NOT NULL,
    login_usuario VARCHAR(100) NOT NULL,
    senha VARCHAR(100) NOT NULL,
    pessoa_id_pessoa INT NOT NULL
)

-- drop table grupo
CREATE TABLE grupo(
    id_grupo INT IDENTITY PRIMARY KEY NOT NULL,
    nome_grupo VARCHAR(100) NOT NULL,    
)

-- drop table usuario_grupo
CREATE TABLE usuario_grupo(
	seq INT IDENTITY PRIMARY KEY NOT NULL,
    id_usuario INT NOT NULL,
    id_grupo INT NOT NULL
)


ALTER TABLE usuario_grupo
   ADD CONSTRAINT FK_grupo_usuario_usuario FOREIGN KEY (id_usuario)
      REFERENCES usuario (id_usuario)
      ON DELETE CASCADE
      ON UPDATE CASCADE
;


ALTER TABLE usuario_grupo
   ADD CONSTRAINT FK_grupo_usuario_grupo FOREIGN KEY (id_grupo)
      REFERENCES grupo (id_grupo)
      ON DELETE CASCADE
      ON UPDATE CASCADE
;

-- drop FK_PESSOA_ID_GRUPO
ALTER TABLE usuario
   ADD CONSTRAINT FK_PESSOA_ID_GRUPO FOREIGN KEY (pessoa_id_pessoa)
      REFERENCES pessoa (id_pessoa)
      ON DELETE CASCADE
      ON UPDATE CASCADE
;


INSERT INTO pessoa (nome, sobrenome, email, cpf)
VALUES ('Rodrigo', 'Lucke', 'rodrigo.lucke@outlook.com', '01964805317')

INSERT INTO usuario (login_usuario, senha, pessoa_id_pessoa)
VALUES ('lucke', 'lucke', 1)


INSERT INTO grupo (nome_grupo)
VALUES ('admin')


INSERT INTO usuario_grupo (id_usuario, id_grupo)
VALUES (1,1)