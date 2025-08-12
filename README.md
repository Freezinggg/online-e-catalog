# Online E-Catalog v1

A simple, observable, clean architecture-based product catalog API built with ASP.NET Core (.NET 9).

This is a real-world project as part of my Back-End Developer journey — aiming to ship a clean backend system.

---

## 🔧 Tech Stack

- ASP.NET Core (.NET 9)
- Clean Architecture (API / Application / Domain / Infrastructure)
- Serilog + Seq (Structured Logging, easy to use)
- Docker & Docker Compose
- GitHub Actions
- Minimal React UI (Plan, idk if will use other)

---

## ✨ Features

- Product Catalog CRUD API
- Clean Architecture with separation of concerns
- Docker + PostgreSQL setup via Docker Compose
- Structured Logging with Serilog & Seq
- Health check endpoint (`/health`)
- Prometheus-compatible metrics endpoint (`/metrics`)
- Standardized API responses (`ApiResponse<T>`)
  
---

## 📌 Roadmap

- [x] Day 1 – Project setup, folder structure, README, git // Done at 30-July-2025
- [x] Day 2 – API skeleton & Clean Arch wiring // Done at 31-July-2025
- [x] Day 3 – Logging with Serilog + Seq // Done at 01-August-2025
- [x] Day 4 – Basic Product CRUD // Done at 02-August-2025
- [x] Day 5 – Dockerization // Done at 03-August-2025
- [x] Day 6 – Health Checks / Readiness //Done at 04-August-2025
- [x] Day 7 - Prometheus metrics + ApiResponse pattern //Done at 05-August-2025
- [x] Day 8 – GitHub Actions CI/CD //Done at 06-August-2025
- [x] Day 9 – Pagination //Done at 12-August-2025
- [ ] TBD (JWT, React UI, Auth, DDD refinements)
