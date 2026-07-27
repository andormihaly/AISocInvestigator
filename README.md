# AI SOC Investigator

An AI-103 learning project focused on building a modern AI-powered Security Operations Center (SOC) assistant using Microsoft Azure AI Foundry.

The goal of this project is to progressively build an intelligent security investigator capable of analyzing incidents, retrieving evidence, orchestrating multiple AI agents, and supporting security analysts throughout the investigation process.

The project follows a six-week implementation plan that aligns with the Microsoft AI-103 certification topics while producing a real-world portfolio application.

---

## Technology Stack

- .NET 10
- ASP.NET Core Web API
- Azure AI Foundry
- GPT-5.4
- Azure AI Search
- Azure Functions
- Azure Blob Storage
- LangGraph
- Model Context Protocol (MCP)
- Application Insights

---

## Architecture

```
Client
    │
    ▼
ASP.NET Core API
    │
    ▼
Application Layer
    │
    ▼
Infrastructure Layer
    │
    ▼
Azure AI Foundry
```

Current solution structure:

```
AISocInvestigator
│
├── src
│   ├── AISocInvestigator.Api
│   ├── AISocInvestigator.Application
│   └── AISocInvestigator.Infrastructure
│
└── tests
```

---

## Project Roadmap

- [x] Project initialization
- [x] Azure AI Foundry project
- [x] GPT-5.4 deployment
- [x] Clean Architecture solution

### Week 1
- [ ] Azure AI Foundry integration
- [ ] Basic Security Assistant
- [ ] Secure configuration
- [ ] Initial architecture documentation

### Week 2
- [ ] AI Agents
- [ ] Tool Calling
- [ ] Azure AI Search
- [ ] RAG

### Week 3
- [ ] Memory
- [ ] Investigation Workflow
- [ ] Observability

### Week 4
- [ ] LangGraph
- [ ] MCP
- [ ] Security Tools

### Week 5
- [ ] Content Understanding
- [ ] Advanced RAG
- [ ] Evidence Platform

### Week 6
- [ ] Multi-Agent Architecture
- [ ] Evaluation
- [ ] Production Readiness

---

## Current Status

**Version:** Pre v0.1

The application currently contains the project structure and Azure AI Foundry configuration.

The first milestone (v0.1) will provide a basic AI-powered SOC assistant capable of answering security-related questions.

---

## Learning Objectives

- Microsoft AI-103
- Azure AI Foundry
- AI Agents
- Tool Calling
- Model Context Protocol
- Retrieval-Augmented Generation
- Multi-Agent Systems
- AI Evaluation
- AI Observability

---

## License

This project is created for learning and portfolio purposes.