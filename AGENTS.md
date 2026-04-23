# AGENTS.md

Start here, then load only the docs needed for the task.

## Repo Map

- `AuthECClient/`: Angular 19 frontend.
- `AuthECAPI/`: ASP.NET Core API (`net8.0`) plus tests.
- `docs/`: source-of-truth docs and execution plans.

## Read This First

- Setup or local run issue: `docs/SETUP.md`
- Architecture or data flow: `docs/ARCHITECTURE.md`
- Angular behavior or routes: `docs/FRONTEND.md`
- API bootstrap or auth internals: `docs/BACKEND.md`
- Endpoint contract or auth requirements: `docs/API.md`
- Roles, claims, or policies: `docs/AUTHORIZATION.md`
- Validation strategy: `docs/TESTING.md`
- Planning work: `docs/PLANS.md`

## Working Rules

- Treat `docs/` as the source of truth for repo behavior.
- Prefer repo-local docs over chat history when they conflict.
- Update the relevant doc in the same change when behavior, setup, routes, endpoints, or auth rules change.
- For multi-file or non-trivial work, add or update a plan under `docs/exec-plans/`.
- Keep this file short; move detailed explanations into `docs/`.

## Fast Navigation

- Product flows: `docs/product-specs/`
- Compact references: `docs/references/`
- Active plans: `docs/exec-plans/active/`
- Completed plans: `docs/exec-plans/completed/`

## Key Runtime Facts

- Angular app runs on `http://localhost:4200`.
- API dev profile runs on `http://localhost:5181`.
- Frontend base URL is `http://localhost:5181/api`.
- JWT bearer auth is attached by the Angular HTTP interceptor.
- Server auth rules are mirrored in Angular route guard claim checks.

## When Unsure

1. Confirm setup in `docs/SETUP.md`.
2. Confirm behavior in `docs/API.md`, `docs/FRONTEND.md`, or `docs/BACKEND.md`.
3. If the change spans multiple areas, record intent in `docs/exec-plans/` before implementation.
