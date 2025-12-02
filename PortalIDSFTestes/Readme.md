# ZiTec · Automação de Testes E2E com Playwright + C# + NUnit  

Acesse os relatórios de execução dos testes em: https://zitecai.github.io/ZitecQA/#

Tecnologias: C#, Playwright, NUnit  

---

## Arquitetura do Projeto

Este projeto implementa uma arquitetura de automação de testes E2E (end-to-end) para a aplicação Web PortalIDSF utilizando Playwright + NUnit + Page Object Model (POM). A estrutura foi desenhada para garantir escalabilidade, reutilização de código e manutenibilidade.

### Estrutura de Diretórios

O projeto é organizado em 5 camadas principais:

1. **Runner** → Gerencia o ambiente de testes (abrir/fechar navegador)
2. **Elementos** → Mapeamento dos elementos Web (LoginPage → LoginElements)
3. **Metodos** → Métodos genéricos reutilizáveis (cliques, preenchimentos, uploads, validações)
4. **Pages** → Implementa a lógica de interação com as páginas (centralização de validações)
5. **Testes** → Onde os cenários são executados (Faz as chamadas das pages)


### Relatórios

