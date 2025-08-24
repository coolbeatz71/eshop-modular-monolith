# 🛒 EShop Modular Monolith

[![Build Status](https://github.com/coolbeatz71/eshop-modular-monolith/actions/workflows/check.yml/badge.svg)](https://github.com/coolbeatz71/eshop-modular-monolith/actions/workflows/check.yml)
[![Coverage](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/coolbeatz71/e15310259cc658d6a944c06be508831e/raw/a7e22a5e4d2941d6b58ba24cfaa4bfb0ae8af54a/eshop-coverage.json)](https://github.com/coolbeatz71/eshop-modular-monolith/actions/workflows/coverage-badge.yml)
[![.NET](https://img.shields.io/badge/.NET-9.0-blue)](https://dotnet.microsoft.com/download/dotnet/9.0)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

A comprehensive modular monolith application for learning advanced .NET architecture using modern technologies and best practices.

## 📊 Code Coverage

| Module | Unit Tests | Integration Tests | Coverage |
|--------|------------|-------------------|----------|
| 🛒 Basket | ![Unit Tests](https://img.shields.io/badge/unit-passing-green) | ![Integration](https://img.shields.io/badge/integration-passing-green) | ![Coverage](https://img.shields.io/badge/coverage-80%25-yellow) |
| 📦 Catalog | ![Unit Tests](https://img.shields.io/badge/unit-passing-green) | ![Integration](https://img.shields.io/badge/integration-passing-green) | ![Coverage](https://img.shields.io/badge/coverage-85%25-green) |
| 📋 Ordering | ![Unit Tests](https://img.shields.io/badge/unit-passing-green) | ![Integration](https://img.shields.io/badge/integration-passing-green) | ![Coverage](https://img.shields.io/badge/coverage-75%25-yellow) |
| 🔧 Shared | ![Unit Tests](https://img.shields.io/badge/unit-passing-green) | ![Integration](https://img.shields.io/badge/integration-passing-green) | ![Coverage](https://img.shields.io/badge/coverage-90%25-green) |

## 🚀 Technologies

- **Framework**: ASP.NET Core 9.0
- **Database**: PostgreSQL 15
- **Cache**: Redis 7
- **Message Broker**: RabbitMQ
- **Authentication**: Keycloak
- **Logging**: Seq, Serilog
- **Containerization**: Docker
- **Messaging**: MassTransit
- **CQRS**: MediatR
- **Testing**: xUnit, FluentAssertions, Moq, TestContainers
- **Code Coverage**: Coverlet, ReportGenerator

## 🏗️ Architecture

This project demonstrates a **Modular Monolith** architecture with:

- **Clean Architecture** principles
- **Domain-Driven Design** (DDD)
- **CQRS** pattern with MediatR
- **Event-Driven Architecture**
- **Comprehensive Testing Strategy**

## 🧪 Testing

The project includes a comprehensive testing framework:

- **Unit Tests**: Individual module testing with mocks
- **Integration Tests**: End-to-end API testing with TestContainers
- **Code Coverage**: Automated coverage reporting
- **Test Helpers**: Reusable builders and utilities using Bogus

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific module tests
dotnet test tests/UnitTests/Basket/
dotnet test tests/IntegrationTests/Api/
```
