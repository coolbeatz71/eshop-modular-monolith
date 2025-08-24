# GitHub Actions Configuration

This repository uses GitHub Actions for continuous integration and testing. The workflow requires certain secrets to be configured in your GitHub repository settings.

## Required GitHub Secrets

To run the CI/CD pipeline successfully, you need to configure the following secrets in your GitHub repository:

### Repository Settings > Secrets and variables > Actions

1. **POSTGRES_PASSWORD**
   - Description: Password for the PostgreSQL database used in testing
   - Example value: `your-secure-postgres-password`
   - Used by: Test services (PostgreSQL container)

2. **TEST_DATABASE_CONNECTION_STRING**
   - Description: Full connection string for the test database
   - Example value: `Host=localhost;Port=5432;Database=eshop_test;Username=postgres;Password=your-secure-postgres-password`
   - Used by: Integration tests

3. **TEST_REDIS_CONNECTION_STRING**
   - Description: Connection string for Redis cache used in testing
   - Example value: `localhost:6379`
   - Used by: Integration tests

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
- Sets up PostgreSQL and Redis services
- Runs all unit tests for each module
- Runs integration tests for the API
- Uploads test results as artifacts

## Local Development

For local development, you can use the same connection strings by setting them in your local environment or `appsettings.Development.json` file (make sure not to commit sensitive values).