# Plans

## Purpose

Use repo-local execution plans to record intent, scope, decisions, and validation steps for work that spans multiple files or subsystems.

## When To Create A Plan

Create or update a plan when the work:

- changes behavior across frontend and backend
- introduces or changes auth rules, routes, or API contracts
- restructures architecture, setup, or testing
- is large enough that another agent or engineer should be able to continue it without chat history

## Locations

- Active work: `docs/exec-plans/active/`
- Completed work: `docs/exec-plans/completed/`

## Naming Convention

Use a date-prefixed kebab-case name:

```text
YYYY-MM-DD-short-topic.md
```

Example:

```text
2026-04-13-agent-first-docs.md
```

## Minimum Plan Contents

- goal and summary
- key implementation decisions
- affected systems or interfaces
- validation steps
- assumptions or follow-up items

## Maintenance Rule

If a feature or behavior change updates code, update the relevant source-of-truth docs in the same change.

If a multi-file task starts without an existing plan, add one before or during the implementation so later agents can resume without reconstructing context from commits or chat.
