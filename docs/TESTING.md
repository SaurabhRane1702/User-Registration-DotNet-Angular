# Testing

## Current State

Backend test coverage exists in `AuthECAPI/AuthECAPI.test`.

- Framework: xUnit
- Current test file: `IdentityUserEndpointsTests.cs`
- Current strategy: in-memory EF Core plus reflection to invoke private endpoint handlers

The Angular client defines `npm test` through Karma/Jasmine tooling, but there are currently no frontend spec files in `src/`.

## Useful Commands

### Backend

From `AuthECAPI/AuthECAPI.test`:

```bash
dotnet test
```

### Frontend

From `AuthECClient`:

```bash
npm test
```

### Manual End-To-End Validation

1. Run the API.
2. Run the Angular client.
3. Register at least one user for each role you need to test.
4. Sign in and verify:
   - token is stored
   - guarded routes allow or reject navigation correctly
   - unauthorized API calls return `401` or `403`
   - book and timetable flows behave as documented

## Documentation Validation Checklist

When changing behavior, confirm the docs still answer:

- how to run the repo locally
- which endpoints exist and who can call them
- which client routes require which claims
- where auth is enforced
- how to test the changed flow

## Recommended Next Test Improvements

- Add frontend specs for `authGuard`, `authInterceptor`, and claim predicates.
- Add API tests that cover sign-in token claims and authorization-protected endpoints without reflection where possible.
