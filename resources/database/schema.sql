-- Esquema inicial do banco de dados. Deve ser alterado enquanto o banco de dados
-- não estiver em produção, para evitar problemas de migração.
-- Após a fase de entrega, deverão ser criadas migrações para alterações futuras,
-- e este arquivo deverá ser mantido apenas como referência.
BEGIN;

CREATE TABLE public.tb_cliente
(
    id uuid NOT NULL,
    nome character varying COLLATE pg_catalog."default" NOT NULL,
    telefone character varying(13) COLLATE pg_catalog."default" NOT NULL,
    email character varying COLLATE pg_catalog."default",
    cpf character varying(11) COLLATE pg_catalog."default",
    dt_nasc date,
    CONSTRAINT tb_cliente_pkey PRIMARY KEY (id),
    CONSTRAINT tb_cliente_ak01 UNIQUE (cpf)
);

CREATE TABLE public.tb_funcionario
(
    id uuid NOT NULL,
    nome character varying COLLATE pg_catalog."default" NOT NULL,
    cpf character varying(11) COLLATE pg_catalog."default" NOT NULL,
    email character varying COLLATE pg_catalog."default" NOT NULL,
    endereco character varying COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT tb_funcionario_pkey PRIMARY KEY (id),
    CONSTRAINT tb_funcionario_ak01 UNIQUE (cpf),
    CONSTRAINT tb_funcionario_ak02 UNIQUE (email)
);

CREATE TABLE public.tb_servico
(
    id uuid NOT NULL,
    nome character varying COLLATE pg_catalog."default" NOT NULL,
    descricao character varying COLLATE pg_catalog."default",
    valor numeric(6, 2) NOT NULL,
    CONSTRAINT tb_servico_pkey PRIMARY KEY (id),
    CONSTRAINT tb_servico_ak01 UNIQUE (nome)
);

CREATE TABLE public.tb_agendamento
(
    id uuid NOT NULL,
    id_cliente uuid NOT NULL,
    id_funcionario uuid NOT NULL,
    dt_agendamento timestamp with time zone NOT NULL,
    dt_conclusao timestamp with time zone,
    status character varying(9) COLLATE pg_catalog."default" NOT NULL,
    total numeric(6, 2),
    CONSTRAINT tb_agendamento_pkey PRIMARY KEY (id)
);

COMMENT ON COLUMN public.tb_agendamento.status 
    IS 'agendado, concluido, cancelado';

CREATE TABLE public.tb_usuario
(
    id_funcionario uuid NOT NULL,
    login character varying COLLATE pg_catalog."default" NOT NULL,
    senha character varying COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT tb_usuario_pkey PRIMARY KEY (id_funcionario),
    CONSTRAINT tb_usuario_ak01 UNIQUE (login)
);

COMMENT ON COLUMN public.tb_usuario.senha
    IS 'Tipo deveria ser bytes, mas será alterado posteriormente.';

CREATE TABLE public.tb_servico_realizado
(
    id_agendamento uuid NOT NULL,
    id_servico uuid NOT NULL,
    valor numeric(6, 2) NOT NULL,
    CONSTRAINT tb_servico_realizado_pkey PRIMARY KEY (id_servico, id_agendamento)
);

ALTER TABLE public.tb_agendamento
    ADD CONSTRAINT tb_agendamento_fk01 FOREIGN KEY (id_cliente)
    REFERENCES public.tb_cliente (id) MATCH SIMPLE
    ON UPDATE CASCADE
    ON DELETE RESTRICT;

ALTER TABLE public.tb_agendamento
    ADD CONSTRAINT tb_agendamento_fk02 FOREIGN KEY (id_funcionario)
    REFERENCES public.tb_funcionario (id) MATCH SIMPLE
    ON UPDATE CASCADE
    ON DELETE RESTRICT;

ALTER TABLE public.tb_usuario
    ADD CONSTRAINT tb_usuario_funcionario_fk01 FOREIGN KEY (id_funcionario)
    REFERENCES public.tb_funcionario (id) MATCH SIMPLE
    ON UPDATE CASCADE
    ON DELETE CASCADE;

ALTER TABLE public.tb_servico_realizado
    ADD CONSTRAINT tb_servico_realizado_fk01 FOREIGN KEY (id_agendamento)
    REFERENCES public.tb_agendamento (id) MATCH SIMPLE
    ON UPDATE CASCADE
    ON DELETE CASCADE;

ALTER TABLE public.tb_servico_realizado
    ADD CONSTRAINT tb_servico_realizado_fk02 FOREIGN KEY (id_servico)
    REFERENCES public.tb_servico (id) MATCH SIMPLE
    ON UPDATE CASCADE
    ON DELETE RESTRICT;

END;