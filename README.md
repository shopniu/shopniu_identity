# shopniu-identity

Servicio de autenticación y usuarios: servidor de autorización **OpenIddict**, registro/login y administración de roles y permisos. ASP.NET Core Web API sobre .NET 10.

## Stack

- ASP.NET Core (`net10.0`), `Microsoft.NET.Sdk.Web`
- OpenIddict.AspNetCore 7.6.0
- EF Core + Npgsql (PostgreSQL) 10.0.0
- Microsoft.AspNetCore.OpenApi 10.0.10, Swashbuckle 10.2.3
- Shopniu.Shared 1.0.3 (paquete privado)

## Configuración

- **BD:** `ConnectionStrings:DefaultConnection` → base `shopniu_identity_db` (ver `appsettings.json` / `appsettings.Development.json`).
- **Migraciones/seeders:** `Database:Migration:RunOnStartup` y `Database:Seeding:RunOnStartup` = `true` en Development (se ejecutan al iniciar).
- **Repos:** https://github.com/DanielAmado11/shopniu_identity (rama `main`).

## Correr localmente

```powershell
dotnet restore
dotnet build -c Release
dotnet run
```

Con docker (orquestado desde el workspace raíz):

```powershell
docker compose up -d --build shopniu-identity
```

Puerto: **8081 (host) → 8080 (contenedor)**. En el docker-compose global levanta primero PostgreSQL.

### Paquete privado (auth)

El `nuget.config` de la raíz agrega el feed `nuget.pkg.github.com/DanielAmado11` y lee la contraseña desde `%GITHUB_PACKAGES_TOKEN%`. Para restaurar:

```powershell
[Environment]::SetEnvironmentVariable('GITHUB_PACKAGES_TOKEN', '<PAT>', 'User')
$env:GITHUB_PACKAGES_TOKEN = [Environment]::GetEnvironmentVariable('GITHUB_PACKAGES_TOKEN','User')
dotnet restore
```

El PAT necesita scope `read:packages`. El archivo `nuget.config` es seguro de commitear (solo contiene el placeholder).

## CI/CD

Workflow `.github/workflows/ci.yml` (push a `main` y PRs):

1. **build** — restore (con `GITHUB_PACKAGES_TOKEN: ${{ secrets.NUGET_PACKAGES_TOKEN }}`), `dotnet format --verify-no-changes`, build Release, test.
2. **docker-publish** — imagen a `ghcr.io/danielamado11/shopniu_identity` (tags `latest`/`sha`/semver). Build-args `NUGET_GITHUB_TOKEN` y `GITHUB_ACTOR` para el restore dentro del Dockerfile.
3. **deploy** — actualiza la container app en Azure.

## Deploy a Azure (Container Apps)

- **Container app:** `shopniu-identity`
- **Entorno:** `thankfulmushroom-4e17c339` (westus)
- **Resource group:** `shopniu`
- **Imagen:** `ghcr.io/danielamado11/shopniu_identity:<sha>`, `targetPort 8080`
- **Registry GHCR:** `ghcr.io`, usuario `DanielAmado11`, password `NUGET_PACKAGES_TOKEN`
- **Env vars/secrets** de la app (connection string de producción, `ASPNETCORE_ENVIRONMENT`): configuradas en el portal de Azure (Containers → Environment variables).
- **Secrets de GitHub (org):** `AZURE_CREDENTIALS` (login a Azure) y `NUGET_PACKAGES_TOKEN`.

## Troubleshooting

- **401 / NU1301 en restore:** la variable `GITHUB_PACKAGES_TOKEN` no está definida o el PAT expiró. Verificar con `[Environment]::GetEnvironmentVariable('GITHUB_PACKAGES_TOKEN','User')`.
- **`dotnet format --verify-no-changes` falla en CI:** correr `dotnet format` local y commitear.

## Convención de commits

Misma convención para todo el workspace (ver `AGENTS.md` en la raíz del proyecto):

```
tipo(scope): descripción en español
```

- **tipo** (obligatorio): `feat`, `fix`, `test`, `refactor`, `chore`, `docs`
- **scope** (opcional): área afectada, ej. `transactions`, `webhook`, `db`, `ci/cd`
- **descripción**: en español, minúsculas, concisa, en pasado o imperativo (ej. `se corrigió`)

Ejemplos:

```
feat(transactions): persistir user payment data y delivery al crear transacción
fix(cart): se corrigió el cálculo del subtotal
chore(db): migración AddDeliveryAndPaymentDataFlow
docs: documentar convención de commits
```

Antes de commitear: `dotnet build -c Release` sin errores y `dotnet format --verify-no-changes` en 0.
