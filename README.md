# 🏢 Rentify — Property Leasing & Maintenance Platform

> **IT8118 Advanced Programming** · Bahrain Polytechnic · Semester B, 2025–2026 · Group S6-G2

Rentify is a full-stack property leasing and maintenance management platform built to digitise the day-to-day operations of a residential property business. It replaces manual workflows — paper applications, phone calls, spreadsheets — with a centralised web-based system that keeps all property, tenant, and financial data in one place.

---

## 🚀 Live Deployments

| Application | URL |
|---|---|
| 🌐 MVC Application | https://mvc-advanced-s6g2-gzdzatc4g6hvguat.westeurope-01.azurewebsites.net |
| 🔌 Web API | https://api-advanced-s6g2-eab2e5cvgaeqd2cu.westeurope-01.azurewebsites.net |
| 📊 Reporting Application | https://reporting-advanced-s6g2-hsf7fnaegjeffeb4.westeurope-01.azurewebsites.net |

---

## 🔑 Demo / Test Credentials

| Role | Email | Password |
|---|---|---|
| 🏠 Property Manager | `manager@mail.com` | `Manager@123` |
| 👤 Tenant | `zahraa.hubail8@gmail.com` | `Zahraa.123` |
| 🔧 Maintenance Staff | `abbas@gmail.com` | `Abbas.123` |

> 💡 The same credentials work for both the local and deployed environments.

---

## 👥 Team Members

| # | Student ID | Name |
|---|---|---|
| 1 | 202305220 | [Zahraa Hubail](https://github.com/zahraa-hubail) |
| 2 | 202301089 | [Fatima Alaiwi](https://github.com/Fatima-Alaiwi) |
| 3 | 202301660 | [Norain Almajed](https://github.com/NorainAlmajed) *(Team Leader)* |
| 4 | 202302130 | [Raghad Aleskafi](https://github.com/RaghadAlesakfi) |
---

## 🛠️ Tech Stack

- **Framework:** ASP.NET Core 9 — MVC Web App + RESTful Web API + Reporting App
- **Database:** SQL Server / Azure SQL via Entity Framework Core 9
- **Authentication:** ASP.NET Core Identity + JWT Bearer Tokens
- **Real-Time:** SignalR (live updates across all user roles + public page)
- **API Docs:** Swagger / OpenAPI via Swashbuckle
- **Hosting:** Microsoft Azure — App Service (3 apps) + Azure SQL Database

---

## ✨ Features

### 🌐 Public (No Login Required)
- Landing page with property platform overview
- Maintenance request tracker — look up any ticket using its number and registered phone, no account needed
- Live status updates on the public tracking page via SignalR

### 👤 Tenant
- Self-registration and login
- Browse available properties and units with images, amenities, and pricing
- Submit, view, and cancel lease applications
- View active and historical leases; request lease termination
- Submit, edit, and track maintenance requests
- View personal payment history and overdue status
- Real-time in-app notifications (lease, payment, maintenance events)

### 🏠 Property Manager
- Full user management — tenants, maintenance staff, and other managers
- Create, edit, and soft-delete properties and units (with image upload)
- Review and approve or reject tenant lease applications (with tenant screening info)
- Create and manage leases directly
- Record and track rent payments; flag overdue balances
- Assign maintenance requests to staff and monitor the full lifecycle
- Real-time notifications for all key events across all roles

### 🔧 Maintenance Staff
- Personal dashboard with assigned job queue
- Update request status: Pending → Assigned → In Progress → Resolved → Closed
- Manage profile, skills, and availability status
- Real-time notifications when request statuses change

### 📊 Reporting Application *(Manager Portal)*
- Separate secured portal — managers only, with a 60-minute session timeout
- **Dashboard** — at-a-glance overview of occupancy, pending maintenance, revenue collected, and overdue payments
- **Occupancy Report** — per-building occupancy rates with interactive line and bar charts
- **Maintenance Report** — request status breakdown, average resolution times, and requests by skill type
- **Payment Report** — collected vs. outstanding revenue; overdue payments with Islamic profit rate *(TA'WIDH, 3% p.a.)*
- **PDF Export** — download any report as a timestamped, formatted PDF

### ⚡ SignalR — Real-Time Updates
- New lease applications appear instantly in the manager's list, no refresh needed
- Maintenance submissions and status changes pushed live to managers, tenants, and staff
- Payment creation and edits reflected in real time across all relevant views
- Lease terminations update unit availability without a page reload
- Public maintenance tracking page updates live when a staff member changes a request status

### 🎯 Enhancements & Edge Cases
- Interactive JavaScript charts throughout the reporting application
- Property listing images — uploaded and stored per property for a richer browsing experience
- Islamic overdue interest calculation — Bahraini TA'WIDH standard (3% per annum)
- Content pagination — 10 users per page on the Users management page

---

## 📡 API Endpoints Summary

| Controller | Route | Access |
|---|---|---|
| 🔐 Auth | `POST /api/auth/register` · `POST /api/auth/login` | Public |
| 🏢 Properties | `GET/POST/PUT/DELETE /api/properties` | Authenticated / Manager |
| 🏠 Units | `GET/POST/PUT/DELETE /api/units` | Authenticated / Manager |
| 👤 Tenants | `GET/POST/PUT/DELETE /api/tenants` | Manager / Tenant (own record) |
| 📋 Lease Applications | `GET/POST/PUT/DELETE /api/leaseapplications` | Manager / Tenant |
| 📄 Leases | `GET/POST/PUT/DELETE /api/leases` | Manager / Authenticated |
| 💳 Payments | `GET/POST/PUT/DELETE /api/payments` | Manager / Authenticated |
| 🔧 Maintenance | `GET/POST/PUT/DELETE /api/maintenance` | Manager / Staff / Tenant |
| 🔍 Ticket Lookup | `GET /api/maintenance/{ticketNumber}/{phone}` | **Public** |
| 📊 Reports | `GET /api/reports/occupancy` · `/maintenance` · `/payments` | Manager |

> Swagger UI available at `{API_URL}/swagger`

---

## 📦 NuGet Packages

| Package | Version | Project |
|---|---|---|
| Microsoft.AspNetCore.Authentication.JwtBearer | 9.0.0 | MVC + API |
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 9.0.0 | MVC + API |
| Microsoft.AspNetCore.Identity.UI | 9.0.0 | MVC |
| Microsoft.EntityFrameworkCore.SqlServer | 9.0.0 | MVC + API |
| Microsoft.EntityFrameworkCore.Tools | 9.0.12 | MVC + API |
| Microsoft.VisualStudio.Web.CodeGeneration.Design | 9.0.12 | MVC |
| Microsoft.AspNetCore.Mvc.NewtonsoftJson | 9.0.0 | API |
| Microsoft.AspNetCore.OpenApi | 9.0.11 | API |
| Swashbuckle.AspNetCore | 6.9.0 | API |

---

## 👩‍💻 Individual Contributions

### Fatima Alaiwi
- Authentication and Authorization (ASP.NET Core Identity — roles, login, registration)
- Password encryption
- SignalR — real-time notifications for payments, leases, and maintenance across all roles
- SignalR integration on the public maintenance tracking page (live status updates without login)
- Privacy Policy page
- Azure deployment — MVC app, Web API, and Reporting app
---

### Raghad Aleskafi
- Project scaffolding and initial database setup (migrations, schema generation)
- UI design and layout — card views, search bars, navigation, forms, and page enhancements across the app
- Property and Unit entity improvements (address fields, amenities, status display)
- Notifications UI
- Governorate field and filter

---

### Norain Almajed *(Team Leader)*
- JWT authentication setup and configuration (token generation, validation, expiry)
- Reporting application — separate secured portal for managers
- Interactive JavaScript charts (occupancy line/bar, maintenance donut, payment split)
- PDF export for all reports — Dashboard, Occupancy, Maintenance, and Payment — with timestamps
- Islamic overdue interest calculation (TA'WIDH, Bahraini standard — 3% per annum)
- Project documentation and cover sheet
---

### Zahraa Hubail
- Database creation and seed data
- RESTful Web API — all controllers and endpoint implementations
- Refactored MVC to call the API for all backend operations
- Full CRUD forms for all entities (Properties, Units, Leases, Applications, Payments, Maintenance Requests, Users)
- Business logic — lease renewal, termination, maintenance staff assignment, notifications, form validations, and success messages
---
---

## 🗃️ Repository

| Item | Details |
|---|---|
| Platform | GitHub |
| URL | https://github.com/NorainAlmajed/Advanced-Project-S6-G2 |
| Default Branch | `main` |

---

## 📚 Course Information

| Field | Details |
|---|---|
| Course Code | IT8118 — Advanced Programming |
| Institution | Bahrain Polytechnic — School of ICT |
| Semester | B, 2025–2026 |
| Lecturer | Mr. Ghassan AlShajjar |
| Submission Date | 03 June 2026 |

---

*© 2026 Rentify — S6-G2 · Bahrain Polytechnic*
