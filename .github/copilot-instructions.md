# eForm Angular Frontend Base

The eForm Angular Frontend Base is a .NET 9.0 C# library by Microting that provides base functionality for eForm Angular frontend applications. It includes Entity Framework database models, migrations, and core infrastructure components.

Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.

## Working Effectively

- Bootstrap, build, and test the repository:
  - Install .NET 9.0 SDK: `wget https://dot.net/v1/dotnet-install.sh && chmod +x dotnet-install.sh && ./dotnet-install.sh --version 9.0.101`
  - Add .NET to PATH: `export PATH=$HOME/.dotnet:$PATH`
  - Restore packages: `dotnet restore` -- takes ~22 seconds. NEVER CANCEL. Set timeout to 60+ seconds.
  - Build solution: `dotnet build --configuration Release --no-restore` -- takes ~12 seconds. NEVER CANCEL. Set timeout to 60+ seconds.
  - Run tests: `dotnet test --no-restore -c Release -v n Microting.EformAngularFrontendBase.Tests/Microting.EformAngularFrontendBase.Tests.csproj` -- takes ~2 seconds. NEVER CANCEL. Set timeout to 30+ seconds.

## Optional MariaDB Setup (CI Compatibility)

The CI workflows use MariaDB 10.8, but tests work without it. For full CI compatibility:
- Start MariaDB: `docker pull mariadb:10.8 && docker run --name mariadbtest -e MYSQL_ROOT_PASSWORD=secretpassword -p 3306:3306 -d mariadb:10.8`
- Clean up: `docker stop mariadbtest && docker rm mariadbtest`

## Validation

- ALWAYS run the bootstrapping steps first before making code changes.
- ALWAYS build and test your changes: `dotnet build --configuration Release --no-restore && dotnet test --no-restore -c Release -v n Microting.EformAngularFrontendBase.Tests/Microting.EformAngularFrontendBase.Tests.csproj`
- Current test suite contains only 1 canary test that validates basic functionality.
- The build succeeds and tests pass without external dependencies.

## Code Quality and Formatting

- Always run `dotnet format --verify-no-changes` to check code formatting before committing.
- Run `dotnet format` to fix formatting issues automatically.
- WARNING: The codebase currently has whitespace formatting issues that must be addressed before CI passes.

## Common Tasks

The following are outputs from frequently run commands. Reference them instead of viewing, searching, or running bash commands to save time.

### Repository Structure
```
eform-angular-frontend-base/
├── .github/workflows/          # CI/CD pipelines (dotnet-core*.yml)
├── Microting.EformAngularFrontendBase/    # Main library project (.NET 9.0)
│   ├── Core.cs                 # Main library entry point
│   ├── Infrastructure/         # Data models, entities, DbContext
│   │   ├── Const/             # Constants
│   │   └── Data/              # Entity Framework models and migrations
│   │       ├── Entities/       # Database entities (Users, Permissions, Reports, etc.)
│   │       ├── Seed/           # Database seed data
│   │       └── BaseDbContext.cs # EF DbContext
│   ├── Migrations/            # 70+ EF Core migrations (2018-2025)
│   └── Microting.EformAngularFrontendBase.csproj
├── Microting.EformAngularFrontendBase.Tests/  # Test project (NUnit)
│   ├── CanaryInACoalMine.cs   # Single canary test
│   └── Microting.EformAngularFrontendBase.Tests.csproj
├── DBMigrator/                # Database migration console app
└── Microting.EformAngularFrontendBase.sln    # Solution file
```

### Key Dependencies (from .csproj files)
- Microsoft.EntityFrameworkCore.Design v9.0.8
- Microting.eForm v9.0.48
- Microting.eFormApi.BasePn v9.0.45
- NUnit v4.3.2 (tests)
- Microsoft.NET.Test.Sdk v17.14.1 (tests)

### Package Upgrade Workflow
- Use `./upgradeeformnugets.sh` to automatically upgrade Microting NuGet packages
- Script requires clean git working tree and CHANGELOG_GITHUB_TOKEN environment variable
- Automatically creates GitHub issues and commits for each package upgrade

### CI/CD Pipelines
- **Pull Requests**: `.github/workflows/dotnet-core-pr.yml` - builds and tests on Ubuntu with MariaDB
- **Master Branch**: `.github/workflows/dotnet-core-master.yml` - same as PR + Slack notifications  
- **Releases**: `.github/workflows/dotnet-core.yml` - builds, tests, packs, and publishes to NuGet

### Common Commands Reference
```bash
# Full build and test cycle (measured timings)
dotnet restore                    # ~22 seconds
dotnet build --configuration Release --no-restore    # ~12 seconds  
dotnet test --no-restore -c Release -v n Microting.EformAngularFrontendBase.Tests/Microting.EformAngularFrontendBase.Tests.csproj    # ~2 seconds

# Code formatting
dotnet format --verify-no-changes    # Check formatting (currently fails)
dotnet format                         # Fix formatting issues (~21 seconds)

# Package listing
dotnet list package                   # Show all NuGet packages

# Migration commands (if working with database)
dotnet ef migrations add <MigrationName> --project Microting.EformAngularFrontendBase
dotnet ef database update --project Microting.EformAngularFrontendBase
```

## Important Notes for Agents

- **TIMING**: All build operations complete quickly (under 30 seconds), but ALWAYS set generous timeouts (60+ minutes) to avoid premature cancellation.
- **DATABASE**: Tests run without external database. MariaDB is only needed for full integration testing.
- **FORMATTING**: The project has existing whitespace formatting issues. Always run `dotnet format` before committing.
- **TESTING**: Only one canary test exists. When making changes, focus on compilation and basic functionality validation.
- **NUGET UPGRADES**: Use the upgrade script rather than manual package updates to maintain consistency with the project's workflow.
- **TARGET FRAMEWORK**: All projects target .NET 9.0. Ensure .NET 9.0 SDK is installed before working.

## Navigation Tips

- **Main Library Code**: `Microting.EformAngularFrontendBase/Core.cs` and `Infrastructure/` folder
- **Database Models**: `Infrastructure/Data/Entities/` contains all Entity Framework models  
- **Migrations**: `Migrations/` folder has 70+ migration files showing database evolution
- **Configuration**: Project files (*.csproj) contain all dependency and build configuration
- **CI Configuration**: `.github/workflows/` contains all build pipeline definitions
- **Documentation**: Minimal - mainly `README.md` and `CHANGELOG.md`