using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSD_Lab1.Models;

namespace SSD_Lab1.Data;

public static class SeedData
{
    public const string SupervisorRole = "Supervisor";
    public const string EmployeeRole = "Employee";

    public static async Task InitializeAsync(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        // The SQL Server migration is the submission schema. SQLite is an optional
        // cross-platform preview provider and creates the same model without using
        // SQL Server-specific migration operations.
        if (context.Database.IsSqlite())
        {
            await context.Database.EnsureCreatedAsync();
        }
        else
        {
            await context.Database.MigrateAsync();
        }

        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var roleName in new[] { SupervisorRole, EmployeeRole })
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException($"Could not create the {roleName} role: {string.Join("; ", roleResult.Errors.Select(error => error.Description))}");
                }
            }
        }

        await CreateConfiguredUserIfMissingAsync(
            userManager,
            configuration["InitialAccounts:Supervisor:Email"],
            configuration["InitialAccounts:Supervisor:Password"],
            "Sample",
            "Supervisor",
            "Hamilton",
            SupervisorRole);

        await CreateConfiguredUserIfMissingAsync(
            userManager,
            configuration["InitialAccounts:Employee:Email"],
            configuration["InitialAccounts:Employee:Password"],
            "Sample",
            "Employee",
            "Hamilton",
            EmployeeRole);

        if (!await context.Companies.AnyAsync())
        {
            context.Companies.AddRange(
                new Company { Name = "Maple Leaf Consulting", YearsInBusiness = 12, Website = "https://www.mapleleafconsulting.example", Province = "Ontario" },
                new Company { Name = "Harbour Technologies", YearsInBusiness = 7, Website = "https://www.harbourtechnologies.example", Province = "Nova Scotia" },
                new Company { Name = "Prairie Design Studio", YearsInBusiness = 4, Website = "https://www.prairiedesign.example", Province = "Saskatchewan" });
            await context.SaveChangesAsync();
        }

        if (!await context.Departments.AnyAsync())
        {
            context.Departments.AddRange(
                new Department { Name = "Engineering", Description = "Builds and maintains company software and systems.", ManagerName = "Jordan Lee", Email = "engineering@example.test", Phone = "905-555-0141", OfficeLocation = "Hamilton Office" },
                new Department { Name = "Customer Support", Description = "Helps customers resolve questions and service issues.", ManagerName = "Taylor Morgan", Email = "support@example.test", Phone = "905-555-0142", OfficeLocation = "Hamilton Office" });
            await context.SaveChangesAsync();
        }

        if (!await context.OfficeLocations.AnyAsync())
        {
            context.OfficeLocations.AddRange(
                new OfficeLocation { Name = "Hamilton Office", Address = "100 King Street West", City = "Hamilton", Province = "Ontario", PostalCode = "L8P 1A2", Phone = "905-555-0100" },
                new OfficeLocation { Name = "Toronto Office", Address = "200 Bay Street", City = "Toronto", Province = "Ontario", PostalCode = "M5J 2J2", Phone = "416-555-0100" });
            await context.SaveChangesAsync();
        }
    }

    private static async Task CreateConfiguredUserIfMissingAsync(
        UserManager<ApplicationUser> userManager,
        string? email,
        string? password,
        string firstName,
        string lastName,
        string city,
        string role)
    {
        if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                $"Both the {role} email and password must be configured under InitialAccounts:{role}.");
        }

        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = firstName,
                LastName = lastName,
                City = city
            };

            var createResult = await userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException($"Could not create {email}: {string.Join("; ", createResult.Errors.Select(error => error.Description))}");
            }
        }

        if (!await userManager.IsInRoleAsync(user, role))
        {
            var roleResult = await userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException($"Could not assign {role} to {email}: {string.Join("; ", roleResult.Errors.Select(error => error.Description))}");
            }
        }
    }
}
