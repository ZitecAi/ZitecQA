# ZiTec · Automação de Testes E2E com Playwright + C# + NUnit  

Status: Ativo  
Tecnologias: C#, Playwright, NUnit  

---

## FAQ – Perguntas Frequentes

### 1. Qual o objetivo deste projeto?
Fornecer uma arquitetura de automação de testes E2E (end-to-end) para aplicação Web PortalIDSF utilizando Playwright + NUnit + Page Object Model (POM), priorizando:  
- Redução de retrabalho  
- Reutilização de código  
- Relatórios claros e acessíveis  
- Escalabilidade para novas features  

---

### 2. Como a arquitetura do projeto está organizada?
O projeto é dividido em 5 camadas principais:

1. Runner → Gerencia o ambiente de testes (abrir/fechar navegador).  
2. Elementos → Mapeamento dos elementos Web (LoginPage → LoginElements).  
3. Metodos → Métodos genéricos reutilizáveis (cliques, preenchimentos, uploads, validações).  
4. Pages → Implementa a lógica de interação com as páginas (equivalente ao Service em MVC).  
5. Testes → Onde os cenários são executados (equivalente ao Controller em MVC).  

---

### 4. Como rodar o projeto localmente?

Clone o repositório:
```bash
git clone https://github.com/seu-usuario/seu-repo.git
cd seu-repo
