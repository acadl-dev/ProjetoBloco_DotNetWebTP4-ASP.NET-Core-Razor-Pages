# 📦 Sistema de Acompanhamento de Entrega

> Projeto acadêmico desenvolvido em ASP.NET Core com Razor Pages, demonstrando competências em desenvolvimento web full-stack, manipulação de dados e design responsivo.

## 🎯 Sobre o Projeto

Sistema web robusto para acompanhamento de pedidos e entregas, desenvolvido como trabalho prático da faculdade. O projeto demonstra a aplicação de conceitos fundamentais de engenharia de software, arquitetura MVC e desenvolvimento web moderno.

## ✨ Funcionalidades

### 🔐 Sistema de Autenticação
- Login seguro com validação de credenciais
- Interface responsiva e moderna
- Proteção de rotas e sessões

### 📊 Dashboard Executivo
- **Cards de métricas em tempo real:**
  - Total de pedidos cadastrados
  - Pedidos em trânsito
  - Entregas concluídas
  - Valor total movimentado

### 📋 Gestão de Pedidos
- **Visualização completa de pedidos:**
  - Listagem ordenada por data
  - Status colorido por categoria (Em Trânsito, Entregue, Preparando, Cancelado)
  - Informações detalhadas do cliente
  - Valores e datas formatados

- **Detalhamento expansível:**
  - Lista completa de itens por pedido
  - Quantidade e preços unitários
  - Cálculo automático de subtotais
  - Valor total consolidado

## 🛠️ Tecnologias Utilizadas

### Backend
- **ASP.NET Core 8.0** - Framework web moderno
- **Razor Pages** - Arquitetura orientada a páginas
- **C#** - Linguagem principal
- **Manipulação de arquivos CSV** - Processamento de dados

### Frontend
- **HTML5 & CSS3** - Estrutura e estilização
- **Tailwind CSS** - Framework CSS utilitário
- **JavaScript** - Interatividade do cliente
- **Design Responsivo** - Compatível com dispositivos móveis

### Arquitetura
- **MVC Pattern** - Separação de responsabilidades
- **Model Binding** - Vinculação automática de dados
- **Dependency Injection** - Inversão de dependências
- **Logging** - Sistema de logs integrado

## 📁 Estrutura do Projeto

```
Sistema-de-Acompanhamento-de-Entrega/
├── Pages/
│   ├── Index.cshtml              # Página de login
│   ├── Index.cshtml.cs           # Model da página de login
│   ├── visualizar_pedidos.cshtml # Dashboard principal
│   └── visualizar_pedidos.cshtml.cs # Model do dashboard
├── wwwroot/
│   └── files/
│       └── pedidos.csv           # Base de dados dos pedidos
└── Models/
    ├── PedidoViewModel.cs        # Modelo de dados do pedido
    └── ItemPedidoViewModel.cs    # Modelo de itens do pedido
```

## 🔧 Como Executar

### Pré-requisitos
- .NET 8.0 SDK
- Visual Studio 2022 ou VS Code

### Instalação
```bash
# Clone o repositório
git clone https://github.com/acadl-dev/ProjetoBloco_DotNetWebTP4-ASP.NET-Core-Razor-Pages.git

# Navegue até o diretório
cd Sistema-de-Acompanhamento-de-Entrega

# Execute a aplicação
dotnet run
```

### Acesso
- **URL:** `https://localhost:5001`
- **Usuário:** `adm`
- **Senha:** `321`

## 💡 Destaques Técnicos

### Processamento de Dados
- **Parser CSV customizado** com tratamento de campos complexos
- **Agrupamento inteligente** de itens por pedido
- **Validação robusta** de dados com tratamento de exceções

### Interface do Usuário
- **Design system consistente** com Tailwind CSS
- **Componentes reutilizáveis** e modulares
- **Feedback visual imediato** em interações
- **Acessibilidade** seguindo padrões web

### Performance e Qualidade
- **Lazy loading** de detalhes de pedidos
- **Manipulação eficiente** de grandes datasets
- **Logging estruturado** para debugging
- **Código limpo** seguindo convenções .NET

## 📈 Métricas do Sistema

O dashboard apresenta métricas calculadas dinamicamente:
- Agregação de dados em tempo real
- Filtros por status automatizados
- Cálculos financeiros precisos
- Ordenação cronológica inteligente

## 🎓 Contexto Acadêmico

Este projeto foi desenvolvido como trabalho prático da disciplina de **Desenvolvimento Back-end**, demonstrando:

- **Aplicação prática** de conceitos teóricos
- **Resolução de problemas** do mundo real
- **Arquitetura escalável** e maintível
- **Boas práticas** de desenvolvimento

---

⭐ **Se este projeto foi útil, considere dar uma estrela no repositório!**