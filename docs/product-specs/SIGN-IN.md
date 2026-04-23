# Sign In And Session

## Flow

1. User submits email and password from the Angular sign-in page.
2. `AuthService.signIn()` posts to `POST /api/signin`.
3. The API validates credentials and returns a JWT.
4. The client stores the token in `localStorage`.
5. Future HTTP requests include the token through `authInterceptor`.

## Session Behavior

- The client considers the user logged in when a token exists.
- Route access can also depend on decoded claims.
- On `401`, the client removes the token, shows a session-expired toast, and redirects to `/signin`.
- On `403`, the client shows an authorization error toast.
