using FluentAssertions;
using SmartLedger.Domain.ValueObjects;

namespace SmartLedger.Domain.Tests.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Constructor_WithValidAmount_ShouldCreateMoney()
    {
        var money = new Money(100.50m, "EUR");
        money.Amount.Should().Be(100.50m);
        money.Currency.Should().Be("EUR");
    }

    [Fact]
    public void Constructor_WithNegativeAmount_ShouldThrow()
    {
        var act = () => new Money(-1m, "EUR");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_WithInvalidCurrency_ShouldThrow()
    {
        var act = () => new Money(100m, "EU");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Add_SameCurrency_ShouldReturnCorrectSum()
    {
        var a = new Money(100m, "EUR");
        var b = new Money(50.75m, "EUR");
        a.Add(b).Amount.Should().Be(150.75m);
    }

    [Fact]
    public void Add_DifferentCurrency_ShouldThrow()
    {
        var a = new Money(100m, "EUR");
        var b = new Money(100m, "USD");
        var act = () => a.Add(b);
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Constructor_ShouldRoundToTwoDecimalPlaces()
    {
        var money = new Money(100.999m, "EUR");
        money.Amount.Should().Be(101.00m);
    }
}