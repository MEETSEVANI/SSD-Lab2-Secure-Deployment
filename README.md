# SSD Lab 1 Company Directory

ASP.NET Core MVC application for a small company directory. Employees can read company, department, and office information. Supervisors can manage those records. Signed-in users can post to the message board; messages are append-only in the web interface.

## Group

- Priyank Vora
- Dhruv Patel
- Meet Sevani

## Features and access rules

| Feature | Visitor | Signed-in user | Supervisor |
| --- | --- | --- | --- |
| Companies, departments, and office locations | Sign-in required | View and read details | View, create, edit, and delete |
| Message board | Sign-in required | View and post | View and post |
| Edit or delete a message | Not available | Not available | Not available |

Authorization is enforced on controller actions. The CRUD forms use server-side model validation and anti-forgery tokens. Razor encodes displayed message content. User passwords are managed by ASP.NET Core Identity.

The Department and OfficeLocation CRUD controllers and views were created with the ASP.NET Core MVC scaffolder using `ApplicationDbContext`, then customized for Supervisor-only writes. The message board is a separate append-only controller.

## Data models

- `Company`: company name, years in business, website, and province.
- `Department`: name, description, manager, email, phone, and office location.
- `OfficeLocation`: name, street address, city, province, postal code, and phone.
- `BoardMessage`: message content, author identity and display name, and UTC creation time.

## Requirements

- .NET 10 SDK
- SQL Server for the standard deployment, or SQLite for a local preview

## Run locally

Set unique development-only account values in User Secrets before starting the app. Do not use production passwords for local development.

```bash
dotnet user-secrets set "InitialAccounts:Supervisor:Email" "supervisor@example.test"
dotnet user-secrets set "InitialAccounts:Supervisor:Password" "<strong unique password>"
dotnet user-secrets set "InitialAccounts:Employee:Email" "employee@example.test"
dotnet user-secrets set "InitialAccounts:Employee:Password" "<strong unique password>"
```

On Windows with SQL Server LocalDB, run in Development using `appsettings.Development.json`. For a cross-platform SQLite preview:

```bash
ASPNETCORE_ENVIRONMENT=Development \
Database__Provider=Sqlite \
ConnectionStrings__Sqlite='Data Source=/tmp/SSD_Lab1.preview.db' \
dotnet run --no-launch-profile --urls http://localhost:5165
```

The SQLite preview uses `EnsureCreated`; the checked-in SQL Server migrations are used by the standard SQL Server configuration.

## Build and publish

```bash
dotnet restore SSD_Lab1.sln
dotnet publish SSD_Lab1.csproj --configuration Release --output ./publish
```

The SQL Server migration runs at application startup. For the existing Somee staging site, select the .NET 10 ASP.NET Core version in the site dashboard, publish the app, then upload the contents of `publish` to the site's root using FTP or Somee's File Manager. The publish output includes the `web.config` required by IIS. [Somee deployment instructions](https://somee.com/Doka/Help/Article/219/How_do_I_deploy_my_application%3F) · [Somee ASP.NET Core guidance](https://somee.com/DOKA/Help/Article/203/Deploy_ASP.Net_Core_application)

After uploading, place a server-only `appsettings.Production.json` beside the application DLL using the Somee File Manager. Use `appsettings.Production.json.example` as a shape guide and replace every placeholder with values from the host dashboard. The publish excludes both production settings files so later uploads will not overwrite the host's secret values; do not delete the server-only settings file during deployment. It is also ignored by Git and must never be added to the repository. Set:

```text
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=<SQL Server connection string>
InitialAccounts__Supervisor__Email=<supervisor test account email>
InitialAccounts__Supervisor__Password=<unique strong password>
InitialAccounts__Employee__Email=<employee test account email>
InitialAccounts__Employee__Password=<unique strong password>
```

Initial account settings create the matching users only if they do not already exist. They do not reset existing passwords. Change existing passwords through Identity before sharing test accounts. Never commit connection strings, account passwords, publish profiles, or production settings files. Require HTTPS at the host and confirm the production site does not expose development exception details.

## Staging access

- Existing staging address: [https://ssd-lab2-meet.somee.com](https://ssd-lab2-meet.somee.com)
- The updated build was deployed on October 22, 2026. The HTTPS home page loads, and signed-out requests to `/Departments`, `/OfficeLocations`, and `/MessageBoard` redirect to Identity sign-in.
- Visitors can select Register or visit `/Identity/Account/Register`, enter their name and email, and create a password. They can then sign in at `/Identity/Account/Login` to view records and post to the message board. The group should share Supervisor test credentials privately with the instructor and authorized peer reviewers only.
- Do not use real personal or confidential information. Limit security testing to this staging application and normal course review; do not attempt denial of service, access to the hosting account, or testing of other Somee customers.

## Manual acceptance checklist

1. Signed-out visitors who open `/Companies`, `/Departments`, `/OfficeLocations`, or `/MessageBoard` are redirected to Identity sign-in.
2. A registered signed-in user can view company, department, and office records and open their detail pages.
3. A regular user cannot create, edit, or delete a company, department, or office location; direct URLs return Access Denied.
4. A Supervisor can create, edit, and delete records in each of the three CRUD sections.
5. Invalid or empty CRUD form submissions show validation errors and do not save data.
6. A signed-in user can post a message. The board displays its author and UTC time. Script-like text is displayed as text.
7. There are no edit or delete controls or actions for messages, including when requested by a Supervisor.
8. Sign-out prevents access to protected routes; the deployed site uses HTTPS and does not expose development exception details.

## Local verification

On October 22, 2026, the project built with .NET SDK 10.0.402 with zero warnings and zero errors. A manual local acceptance review using a temporary SQLite database confirmed:

- Signed-out requests to all protected controllers redirect to Identity sign-in.
- Employees can read the directory, are denied write routes, and can post a message.
- Supervisors can create, edit, and delete company, department, and office-location records.
- Empty department and office forms and empty messages are rejected by server-side validation.
- Message text is HTML-encoded in the page; message edit and delete routes return 404.

Staging verification on October 22, 2026 confirmed the live HTTPS home page and sign-in redirects for the new protected routes. Signed-in CRUD and message-posting checks still require the group's test accounts and should be completed before peer review.

The SQL Server provider brings in `System.Security.Cryptography.Pkcs` through Microsoft.Data.SqlClient. The project pins the dependency to the patched 9.0.21 release.

## Source repository

The repository contains the complete buildable source and migrations: [MEETSEVANI/SSD-Lab2-Secure-Deployment](https://github.com/MEETSEVANI/SSD-Lab2-Secure-Deployment). Keep screenshots, database files, publish output, personal reflections, connection strings, and passwords out of the repository.
