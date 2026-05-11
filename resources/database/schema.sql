-- Esquema inicial do banco de dados. Deve ser alterado enquanto o banco de dados
-- não estiver em produção, para evitar problemas de migração.
-- Após a fase de entrega, deverão ser criadas migrações para alterações futuras,
-- e este arquivo deverá ser mantido apenas como referência.
BEGIN;

CREATE TABLE tb_cliente
(
    id uuid NOT NULL,
    nome character varying NOT NULL,
    telefone character varying(13) NOT NULL,
    email character varying,
    cpf character varying(11),
    dt_nasc date,
    CONSTRAINT tb_cliente_pkey PRIMARY KEY (id),
    CONSTRAINT tb_cliente_ak01 UNIQUE (cpf)
);

CREATE TABLE tb_funcionario
(
    id uuid NOT NULL,
    nome character varying NOT NULL,
    cpf character varying(11) NOT NULL,
    email character varying NOT NULL,
    endereco character varying NOT NULL,
    CONSTRAINT tb_funcionario_pkey PRIMARY KEY (id),
    CONSTRAINT tb_funcionario_ak01 UNIQUE (cpf),
    CONSTRAINT tb_funcionario_ak02 UNIQUE (email)
);

CREATE TABLE tb_servico
(
    id uuid NOT NULL,
    nome character varying NOT NULL,
    descricao character varying,
    valor numeric(6, 2) NOT NULL,
    CONSTRAINT tb_servico_pkey PRIMARY KEY (id),
    CONSTRAINT tb_servico_ak01 UNIQUE (nome)
);

CREATE TABLE tb_agendamento
(
    id uuid NOT NULL,
    id_cliente uuid NOT NULL,
    id_funcionario uuid NOT NULL,
    dt_agendamento timestamp with time zone NOT NULL,
    dt_conclusao timestamp with time zone,
    status character varying(9) NOT NULL,
    total numeric(6, 2),
    CONSTRAINT tb_agendamento_pkey PRIMARY KEY (id),
    CONSTRAINT tb_agendamento_fk01 FOREIGN KEY (id_cliente)
        REFERENCES tb_cliente (id) MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    CONSTRAINT tb_agendamento_fk02 FOREIGN KEY (id_funcionario)
        REFERENCES tb_funcionario (id) MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

COMMENT ON COLUMN public.tb_agendamento.status
    IS 'agendado, concluido, cancelado';

CREATE TABLE tb_usuario
(
    id_funcionario uuid NOT NULL,
    login character varying NOT NULL,
    senha character varying NOT NULL,
    CONSTRAINT tb_usuario_pkey PRIMARY KEY (id_funcionario),
    CONSTRAINT tb_usuario_ak01 UNIQUE (login),
    CONSTRAINT tb_usuario_funcionario_fk01 FOREIGN KEY (id_funcionario)
        REFERENCES tb_funcionario (id) MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

COMMENT ON COLUMN tb_usuario.senha
    IS 'Tipo deveria ser bytes, mas será alterado posteriormente.';

CREATE TABLE tb_servico_realizado
(
    id_agendamento uuid NOT NULL,
    id_servico uuid NOT NULL,
    valor numeric(6, 2) NOT NULL,
    CONSTRAINT tb_servico_realizado_pkey PRIMARY KEY (id_servico, id_agendamento),
    CONSTRAINT tb_servico_realizado_fk01 FOREIGN KEY (id_agendamento)
        REFERENCES tb_agendamento (id) MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    CONSTRAINT tb_servico_realizado_fk02 FOREIGN KEY (id_servico)
        REFERENCES tb_servico (id) MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

END;
