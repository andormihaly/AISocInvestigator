# AI SOC Investigator

An AI-103 learning project focused on building a modern AI-powered Security Operations Center (SOC) assistant using Microsoft Azure AI Foundry.

The project demonstrates how Large Language Models, AI Agents, Azure AI Search, Azure Functions, and Microsoft Sentinel can be combined to build an intelligent SOC investigation platform.

The long-term goal is to evolve the solution into a multi-agent security investigation platform capable of orchestrating specialized AI agents that assist security analysts throughout the incident response lifecycle.

---

## Technology Stack

- .NET 10
- ASP.NET Core Web API
- Azure AI Foundry
- GPT-5.4
- Azure AI Search
- Azure Functions
- Azure Blob Storage
- Microsoft Sentinel
- OpenAPI Tools
- LangGraph (planned)
- Model Context Protocol (planned)
- Application Insights

---

## Current Features

### AI Chat
- GPT-5.4 integration
- Multi-turn conversations using PreviousResponseId
- Clean Architecture implementation

### AI Agents

#### SOC Incident Agent
- Microsoft Sentinel integration
- Azure Function tool
- OpenAPI Tool Calling
- Multi-turn conversations

#### SOC Knowledge Agent
- Azure AI Search
- Retrieval-Augmented Generation (RAG)
- Security documentation knowledge base
- Semantic search over internal documentation

### Azure Integration

- Azure AI Foundry
- Azure AI Search
- Azure Blob Storage
- Azure Functions
- Microsoft Sentinel

---

## Architecture

```
                           User
                             │
                             ▼
                    ASP.NET Core API
                             │
                             ▼
                  Application Layer
                             │
                             ▼
                 Azure AI Foundry
                             │
          ┌──────────────────┴──────────────────┐
          │                                     │
          ▼                                     ▼
   SOC Incident Agent                 SOC Knowledge Agent
          │                                     │
          ▼                                     ▼
 Azure Function Tool                 Azure AI Search
          │                                     │
          ▼                                     ▼
 Microsoft Sentinel              Security Documentation
```

Current solution structure:

```
AISocInvestigator
│
├── src
│   ├── AISocInvestigator.Api
│   ├── AISocInvestigator.Application
│   ├── AISocInvestigator.Infrastructure
│   ├── AISocInvestigator.Functions
│   └── AISocInvestigator.Domain
│
├── docs
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
- [x] Azure AI Foundry integration
- [x] Basic Security Assistant
- [x] Secure configuration
- [x] Initial architecture documentation

### Week 2
- [x] AI Agents
- [x] OpenAPI Tool Calling
- [x] Azure Functions integration
- [x] Microsoft Sentinel integration
- [x] Azure AI Search
- [x] Retrieval-Augmented Generation (RAG)
- [x] Multi-turn conversations
- [x] Knowledge Agent
- [x] Incident Agent

### Week 3
- [ ] Agentic Workflow
- [ ] Memory Management
- [ ] Observability

### Week 4
- [ ] LangGraph
- [ ] Model Context Protocol (MCP)
- [ ] Security Tools

### Week 5
- [ ] Content Understanding
- [ ] Advanced RAG
- [ ] Evidence Platform

### Week 6
- [ ] Multi-Agent Orchestration
- [ ] Evaluation
- [ ] Production Readiness

---

## Current Status

**Version:** v0.2

### Completed

- Azure AI Foundry integration
- GPT-5.4 chat service
- AI Agent support
- Microsoft Sentinel integration
- Azure Function OpenAPI Tool
- Azure AI Search integration
- Retrieval-Augmented Generation (RAG)
- Security documentation knowledge base
- Multi-turn conversations
- Clean Architecture implementation

### Next Milestone

Implement an Agentic Workflow capable of orchestrating multiple specialized AI agents to solve complex security investigation tasks.

---

## Learning Objectives

- Microsoft AI-103
- Azure AI Foundry
- AI Agents
- Agentic Workflows
- OpenAPI Tool Calling
- Azure AI Search
- Retrieval-Augmented Generation (RAG)
- Microsoft Sentinel Integration
- Model Context Protocol
- LangGraph
- Multi-Agent Systems
- AI Evaluation
- AI Observability

---

## License

This project is created for learning and portfolio purposes.