using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EShop.TestHelpers.Extensions;

public static class TestExtensions
{
    public static void ShouldBeEquivalentTo<T>(this T actual, T expected, string because = "")
    {
        actual.Should().BeEquivalentTo(expected, because);
    }

    public static void ShouldBe<T>(this T actual, T expected, string because = "")
    {
        actual.Should().Be(expected, because);
    }

    public static void ShouldNotBe<T>(this T actual, T unexpected, string because = "")
    {
        actual.Should().NotBe(unexpected, because);
    }

    public static void ShouldBeNull<T>(this T? actual, string because = "")
    {
        actual.Should().BeNull(because);
    }

    public static void ShouldNotBeNull<T>(this T? actual, string because = "")
    {
        actual.Should().NotBeNull(because);
    }

    public static void ShouldBeTrue(this bool actual, string because = "")
    {
        actual.Should().BeTrue(because);
    }

    public static void ShouldBeFalse(this bool actual, string because = "")
    {
        actual.Should().BeFalse(because);
    }

    public static void ShouldBeEmpty<T>(this IEnumerable<T> actual, string because = "")
    {
        actual.Should().BeEmpty(because);
    }

    public static void ShouldNotBeEmpty<T>(this IEnumerable<T> actual, string because = "")
    {
        actual.Should().NotBeEmpty(because);
    }

    public static void ShouldHaveCount<T>(this IEnumerable<T> actual, int expectedCount, string because = "")
    {
        actual.Should().HaveCount(expectedCount, because);
    }

    public static void ShouldContain<T>(this IEnumerable<T> actual, T expected, string because = "")
    {
        actual.Should().Contain(expected, because);
    }

    public static void ShouldNotContain<T>(this IEnumerable<T> actual, T unexpected, string because = "")
    {
        actual.Should().NotContain(unexpected, because);
    }

    public static async Task<T> ShouldThrowAsync<T>(this Func<Task> action, string because = "") where T : Exception
    {
        var exception = await action.Should().ThrowAsync<T>(because);
        return exception.Which;
    }

    public static T ShouldThrow<T>(this Action action, string because = "") where T : Exception
    {
        var exception = action.Should().Throw<T>(because);
        return exception.Which;
    }

    public static void ShouldNotThrow(this Action action, string because = "")
    {
        action.Should().NotThrow(because);
    }

    public static async Task ShouldNotThrowAsync(this Func<Task> action, string because = "")
    {
        await action.Should().NotThrowAsync(because);
    }

    public static void ShouldBeGreaterThan<T>(this T actual, T expected, string because = "") where T : IComparable<T>
    {
        actual.Should().BeGreaterThan(expected, because);
    }

    public static void ShouldBeLessThan<T>(this T actual, T expected, string because = "") where T : IComparable<T>
    {
        actual.Should().BeLessThan(expected, because);
    }

    public static void ShouldBeGreaterOrEqualTo<T>(this T actual, T expected, string because = "") where T : IComparable<T>
    {
        actual.Should().BeGreaterOrEqualTo(expected, because);
    }

    public static void ShouldBeLessOrEqualTo<T>(this T actual, T expected, string because = "") where T : IComparable<T>
    {
        actual.Should().BeLessOrEqualTo(expected, because);
    }

    public static void ShouldMatch(this string actual, string pattern, string because = "")
    {
        actual.Should().MatchRegex(pattern, because);
    }

    public static void ShouldStartWith(this string actual, string expected, string because = "")
    {
        actual.Should().StartWith(expected, because);
    }

    public static void ShouldEndWith(this string actual, string expected, string because = "")
    {
        actual.Should().EndWith(expected, because);
    }

    public static void ShouldContain(this string actual, string expected, string because = "")
    {
        actual.Should().Contain(expected, because);
    }
}

public static class DbContextTestExtensions
{
    public static async Task<T?> FindByIdAsync<T>(this DbContext context, Guid id) where T : class
    {
        return await context.Set<T>().FindAsync(id);
    }

    public static async Task<bool> ExistsAsync<T>(this DbContext context, Guid id) where T : class
    {
        return await context.FindByIdAsync<T>(id) != null;
    }

    public static async Task<int> CountAsync<T>(this DbContext context) where T : class
    {
        return await context.Set<T>().CountAsync();
    }

    public static void ClearAllEntities(this DbContext context)
    {
        var entityTypes = context.Model.GetEntityTypes();
        foreach (var entityType in entityTypes)
        {
            var entities = context.Entry(entityType.ClrType).Collection("").CurrentValue;
            if (entities != null)
            {
                context.RemoveRange(entities);
            }
        }
        context.SaveChanges();
    }
}