# GitHub Actions Configuration

This repository uses GitHub Actions for continuous integration and testing. The workflow requires certain secrets to be configured in your GitHub repository settings.

## Required GitHub Secrets

To run the CI/CD pipeline successfully, you need to configure the following secrets in your GitHub repository:

### Repository Settings > Secrets and variables > Actions

**Note**: The integration tests now use TestContainers which manage their own database and Redis containers automatically. No database connection secrets are required for basic CI/CD functionality.

### Optional: Coverage Badge Secrets (for automatic badge generation)

4. **GIST_SECRET**
   - Description: Personal Access Token for updating GitHub Gist (for coverage badges)
   - Scopes: `gist`
   - Used by: Coverage badge workflow

5. **GIST_ID**
   - Description: GitHub Gist ID where coverage badge data will be stored
   - Example value: `abcd1234efgh5678ijkl9012mnop3456`
   - Used by: Coverage badge workflow

## How to Add Secrets

1. Go to your GitHub repository
2. Navigate to **Settings** > **Secrets and variables** > **Actions**
3. Click **New repository secret**
4. Add each secret with the name and value as specified above

## Workflow Overview

The GitHub Actions workflow consists of two main jobs:

### Build Job
- Restores NuGet dependencies
- Builds the solution in Release configuration

### Test Job
- Sets up Docker for TestContainers
- Runs all unit tests for each module with code coverage
- Runs integration tests using TestContainers (PostgreSQL + Redis)
- Generates comprehensive coverage reports
- Uploads test results and coverage reports as artifacts

## Local Development

For local development, you can use the same connection strings by setting them in your local environment or `appsettings.Development.json` file (make sure not to commit sensitive values).