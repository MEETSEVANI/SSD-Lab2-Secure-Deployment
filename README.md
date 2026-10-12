# SSD Lab 1 company directory

An ASP.NET Core MVC company directory with ASP.NET Core Identity. Signed-in Employees can view company records. Supervisors can create, edit, and delete records. The app enforces role checks on the server and stores passwords as Identity hashes.

## Requirements

- .NET 10 SDK
- SQL Server for the standard configuration, or SQLite for a local preview

The project targets .NET 10 LTS and uses the current servicing packages available when this lab was prepared. .NET 10 is supported through November 14, 2028.

## Run locally

Create local demo accounts in User Secrets before starting the app. Replace the example addresses and placeholder passwords with your own values; use unique passwords that you do not use elsewhere:

```bash
dotnet user-secrets set "InitialAccounts:Supervisor:Email" "supervisor@example.test"
dotnet user-secrets set "InitialAccounts:Supervisor:Password" "<strong unique password>"
dotnet user-secrets set "InitialAccounts:Employee:Email" "employee@example.test"
dotnet user-secrets set "InitialAccounts:Employee:Password" "<strong unique password>"
```

The project has a User Secrets ID configured. On Windows with SQL Server LocalDB, run the app in Development. The LocalDB connection string is in `appsettings.Development.json` and contains no password.

For a cross-platform SQLite preview, set the provider and database path before starting the app:

```bash
ASPNETCORE_ENVIRONMENT=Development \
Database__Provider=Sqlite \
ConnectionStrings__Sqlite='Data Source=/tmp/SSD_Lab1.preview.db' \
dotnet run --no-launch-profile --urls http://localhost:5165
```

The SQLite preview uses `EnsureCreated`; the checked-in Entity Framework migration targets SQL Server. SQL Server deployments apply the migrations at startup.

## Initial account settings

The repository does not contain initial account passwords. Set both an email and a strong password for each role in local User Secrets or in the hosting provider's private environment settings:

```text
InitialAccounts__Supervisor__Email
InitialAccounts__Supervisor__Password
InitialAccounts__Employee__Email
InitialAccounts__Employee__Password
```

When present, these settings create the initial accounts and assign their roles. When omitted, the app still creates the roles and sample company records, but it does not create login accounts. The initial password is only used when that account is first created; later password changes must be made through Identity.

Do not put hosting connection strings, account passwords, publish profiles, or local environment files in Git. The app reads its SQL Server connection string from `ConnectionStrings__DefaultConnection` in the host's private settings. `Database__Provider` defaults to `SqlServer`.

## Publish a Release build

```bash
dotnet restore SSD_Lab1.sln
dotnet publish SSD_Lab1.csproj --configuration Release --output ./publish
```

Upload the contents of `publish` using the host's supported ASP.NET Core deployment method. Set `ASPNETCORE_ENVIRONMENT` to `Production`, configure the SQL Server connection string and the two initial accounts in private host settings, and require HTTPS. The Development settings file is excluded from publish output. Do not upload local database files.

For a no-charge Azure deployment, use Azure for Students, an App Service F1 Free plan, and the Azure SQL Database free offer. In the SQL offer settings, select **Auto-pause the database until next month** when the free allowance is reached. This pauses database access at the monthly limit instead of opting into overage charges. Do not select a paid App Service plan or the SQL option that continues for additional charges.

## Manual functionality test plan

Run these checks against the deployed HTTPS site after deployment:

1. While signed out, open `/Companies` and `/Companies/Create`. Both should direct the visitor to the Identity login page.
2. Sign in as Employee. Confirm the company list and Details page work. Open `/Companies/Create`, `/Companies/Edit/1`, and `/Companies/Delete/1`; write actions should return Access Denied.
3. Sign in as Supervisor. Confirm the company list provides Create, Edit, and Delete actions.
4. Create a company with valid values. Confirm it appears in the list and its Details page opens.
5. Submit an empty Create form. Confirm validation errors appear and no company is saved.
6. Edit the test company and confirm the changes appear. Delete it and confirm it no longer appears in the list.
7. Sign out and confirm a protected company URL redirects to login. Confirm the site uses HTTPS and does not display development exception details.

## Database schema

The SQL Server migration is in `Migrations/`. The app applies pending SQL Server migrations at startup before seeding roles and company sample data. The production database account must have permission to apply these migrations.
