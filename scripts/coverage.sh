#!/bin/bash

# EShop Code Coverage Script
# This script runs all tests with code coverage and generates a report

echo "🧪 Running EShop Code Coverage..."
echo ""

# Clean previous results
if [ -d "TestResults" ]; then
    echo "🧹 Cleaning previous test results..."
    rm -rf TestResults
fi

if [ -d "coverage-report" ]; then
    echo "🧹 Cleaning previous coverage report..."
    rm -rf coverage-report
fi

# Run tests with coverage
echo "▶️  Running tests with coverage collection..."
dotnet test --configuration Release --collect:"XPlat Code Coverage" --settings coverlet.runsettings --results-directory TestResults/

# Check if dotnet-reportgenerator-globaltool is installed
if ! dotnet tool list -g | grep -q "dotnet-reportgenerator-globaltool"; then
    echo "📦 Installing ReportGenerator tool..."
    dotnet tool install -g dotnet-reportgenerator-globaltool
fi

# Generate HTML report
echo "📊 Generating coverage report..."
reportgenerator \
    "-reports:TestResults/**/coverage.cobertura.xml" \
    "-targetdir:coverage-report" \
    "-reporttypes:Html;Cobertura;Badges;JsonSummary" \
    "-title:EShop Coverage Report"

# Extract and display coverage percentage
if [ -f "coverage-report/Summary.json" ]; then
    coverage=$(grep -o '"lineCoveragePercent":[^,]*' coverage-report/Summary.json | grep -o '[0-9.]*')
    echo ""
    echo "📈 Code Coverage: ${coverage}%"
    
    # Determine coverage status
    if (( $(echo "$coverage >= 80" | bc -l) )); then
        echo "✅ Great coverage!"
    elif (( $(echo "$coverage >= 60" | bc -l) )); then
        echo "⚠️  Good coverage, but could be improved"
    else
        echo "❌ Coverage needs improvement"
    fi
fi

echo ""
echo "🎉 Coverage report generated!"
echo "📁 Open coverage-report/index.html in your browser to view the detailed report"
echo ""