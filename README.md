# AI SOC Investigator

AI SOC Investigator is an AI-powered Security Operations Center platform
built on Microsoft Azure. It uses a multi-agent workflow to route
security requests between live investigation and knowledge retrieval
capabilities, integrates enterprise security systems through Model
Context Protocol (MCP), and supports controlled security actions through
Human-in-the-Loop approval.

## Core Capabilities

### Agentic Workflow

-   Intake Agent routes requests to the appropriate specialist.
-   MCP Investigation Agent works with live security data.
-   Knowledge Agent answers security and architecture questions using
    the SOC knowledge layer.
-   Workflow sessions preserve agent context across interactions.
-   Human-in-the-Loop workflow control pauses execution before security
    actions and resumes the pending workflow after approval.
-   Unrelated requests can be processed independently while an approval
    remains pending.

### Security Investigation

-   Live failed and successful login telemetry.
-   Microsoft Sentinel incident access.
-   Microsoft Defender alert retrieval.
-   Microsoft Defender manual alert creation.
-   Multi-tool investigations using MCP.
-   Duplicate-aware alert recommendations based on existing Defender
    state.
-   Investigation scope remains focused on the user's current request.

### Knowledge and Agentic Retrieval

-   Azure AI Search Knowledge Source over SOC security documentation.
-   Vectorized Azure AI Search index using `text-embedding-3-large`.
-   SOC Security Knowledge Base.
-   GPT-5.4 based query planning and answer synthesis.
-   Agentic Retrieval with multi-query evidence retrieval.
-   Evidence-based responses with source citations.
-   Knowledge Base exposed to the Knowledge Agent through MCP.

### Human-in-the-Loop Security Actions

-   Security actions require explicit human approval before execution.
-   Pending workflows can pause and resume after approval.
-   Approved actions are executed through the secured MCP layer.

### Observability and AI Security

-   End-to-end distributed tracing with Application Insights and
    OpenTelemetry.
-   Azure AI Foundry Continuous Evaluation.
-   Azure AI Foundry Guardrails for jailbreak and prompt injection
    protection.

## Technology Stack

-   .NET 10
-   ASP.NET Core Web API
-   Microsoft Agent Framework
-   Wolverine
-   Azure AI Foundry Agents
-   GPT-5.4
-   Azure AI Projects SDK
-   Model Context Protocol (MCP)
-   Azure AI Search
-   Azure AI Search Knowledge Sources and Knowledge Bases
-   Foundry IQ / Agentic Retrieval
-   Azure Blob Storage
-   Microsoft Sentinel
-   Microsoft Defender XDR
-   Microsoft Graph Beta API
-   Microsoft Entra ID
-   Managed Identity / Agent Identity / Project Identity
-   Azure Key Vault
-   Azure App Service
-   Azure Monitor OpenTelemetry
-   Application Insights

## Architecture

``` text
User
 |
 v
ASP.NET Core API
 |
 v
Workflow Orchestrator
 |
 +-- Intake Agent
 |     |
 |     +-- Investigation
 |     |     |
 |     |     +--> MCP Investigation Agent
 |     |             |
 |     |             +--> Security MCP Server
 |     |                     |
 |     |                     +--> Login Telemetry
 |     |                     +--> Microsoft Sentinel
 |     |                     +--> Microsoft Defender / Microsoft Graph
 |     |
 |     +-- Knowledge
 |           |
 |           +--> Knowledge Agent
 |                   |
 |                   +--> Knowledge Base MCP
 |                           |
 |                           +--> SOC Security Knowledge Base
 |                                   |
 |                                   +--> Agentic Retrieval
 |                                   +--> Azure AI Search
 |
 +-- Human-in-the-Loop
 |     |
 |     +--> Proposed Security Action
 |     +--> RequestPort
 |     +--> Pending Workflow Run
 |     +--> User Approval / Rejection
 |     +--> Resume Workflow
 |     +--> Approved Action Execution
 |
 +--> Application Insights
 +--> Azure Monitor OpenTelemetry
```

## Security and Authentication

The platform uses identity-based authentication instead of embedded
application secrets wherever supported.

-   Azure Key Vault is used for configuration and secret management.
-   Azure resources use `DefaultAzureCredential` and Managed Identity.
-   The Security MCP Server is protected with Microsoft Entra ID and
    Azure App Service Authentication.
-   Azure AI Foundry connects to the Security MCP Server using Agent
    Identity.
-   The Knowledge Base MCP connection uses Project Identity and Azure
    RBAC.
-   Microsoft Defender access uses the MCP Server Managed Identity with
    Microsoft Graph application permissions.
-   `SecurityAlert.ReadWrite.All` supports Defender alert read and
    create operations used by the platform.

## Workflow Behavior

The Intake Agent uses two routing paths:

-   **Investigation** --- selected when answering requires live security
    environment data.
-   **Knowledge** --- selected when the request can be answered from
    security knowledge, documentation, or the knowledge base.

The Investigation Agent dynamically selects the MCP tools required for
the current request. Security actions are proposed only when relevant to
the investigated activity and require explicit human approval before
execution.

For failed-login and anomaly scenarios, the Investigator can recommend
creating a Microsoft Defender alert when suspicious activity meets the
configured investigation criteria and no corresponding alert already
exists.

## Solution Structure

``` text
AISocInvestigator
|
+-- src
|   +-- AISocInvestigator.Api
|   +-- AISocInvestigator.Application
|   +-- AISocInvestigator.Domain
|   +-- AISocInvestigator.Infrastructure
|   +-- AISocInvestigator.Functions
|   +-- AISocInvestigator.SecurityMcpServer
|
+-- docs
|
+-- tests
```

## Documentation

Detailed implementation reports are available in the `docs` directory:

-   Week 1 --- Azure AI Foundry foundation and SOC assistant.
-   Week 2 --- Specialized agents, Microsoft Sentinel integration, and
    RAG.
-   Week 3 --- Agentic Workflow, observability, evaluation, and
    guardrails.
-   Week 4 --- Secured MCP investigation layer.
-   Week 5 --- Knowledge Base, Foundry IQ, and Agentic Retrieval.
-   Week 6 --- Human-in-the-Loop workflow control and Microsoft Defender
    integration.
