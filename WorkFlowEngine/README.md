# Infonetica – Configurable Workflow Engine (State-Machine API)

##  Overview

This project is a simple backend API that allows users to:
- Define state-machine workflows
- Start workflow instances
- Execute actions to transition states
- View instance state and history

Built using **.NET 8 / C#**, with in-memory persistence.

---

**Assumptions Made**
- Each workflow must contain exactly one initial state

- All state and action IDs must be unique

- Action transitions are strictly validated against the definition

- Disabled actions or transitions from final states are blocked

- Data is stored in-memory (no persistence on restart)




## ▶️ How to Run


Make sure you have the **.NET 8 SDK** installed:  
 https://aka.ms/dotnet-download



```bash
git clone https://github.com/Sarthak-Patel15/Infotica-Task-by-Sarthak-R-Patel
cd workflow-engine
dotnet run



    
