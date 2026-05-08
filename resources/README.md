# Resources

Este diretório contém materiais de apoio para o projeto que não fazem parte do runtime principal da aplicação. Esses recursos ajudam no desenvolvimento, documentação, design e gerenciamento de banco de dados.

---

## Estrutura

```
resources/
├── database/
├── designs/
└── scripts/
```

---

## `database/`

Contém arquivos relacionados ao banco de dados, usados para definição de esquema, configuração e testes.

**Conteúdo:**

- `schema.sql` — Define a estrutura do banco de dados, incluindo tabelas, relacionamentos e restrições.
- `test_seed.sql` — Fornece dados de exemplo para testes e desenvolvimento.

---

## `designs/`

Contém materiais de design visual e estrutural, incluindo diagramas e outras representações.

```
designs/
├── images/
└── sources/
```

### `images/`

Contém arquivos de imagem exportados a partir dos arquivos de origem. Esses são os ativos finais prontos para visualização.
São exatamente os arquivos que aparecem no documento principal do projeto.

---

### `sources/`

Contém os arquivos de origem usados para geras as imagens. Esses arquivos são editáveis e devem ser mantidos para permitir atualizações e regeneração das imagens quando necessário.

**Tipos:**

- `.mmd` — Diagramas mermaid. Renderizar para SVG usando [draw.io](https://draw.io).
- `.dbml` — Diagrama de banco de dados. Renderizar para SVG usando [dbdiagram.io](https://dbdiagram.io).
- `.excalidraw` — Wireframes. Renderizar para PNG usando [Excalidraw](https://excalidraw.com).

> **Nota:** Sempre que um arquivo de origem for atualizado, a imagem correspondente deve ser regenerada para garantir que o design visual esteja alinhado com as fontes de verdade.
>
> Os arquivos SVG deve ser convertido para PNG usando [svgviewer.dev](https://www.svgviewer.dev/svg-to-png) para garantir compatibilidade e qualidade visual.

---

## `nginx/`

Contém arquivos de configuração para o servidor Nginx, usado para servir a aplicação frontend.

\*_Conteúdo:_

- `default.conf` — Configuração padrão do Nginx para servir a aplicação frontend e redirecionar as requisições para a API.

---

## `scripts/`

Scripts utilitários usados para apoiar os fluxos de trabalho de desenvolvimento.

**Conteúdo:**

- `patch_dbdiagram_svg.py` — Script para alterar o SVG gerado pelo dbdiagram, mudando a cor de destaque para melhor visibilidade.

---
