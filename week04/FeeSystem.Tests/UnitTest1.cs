using System;
using System.Collections.Generic;
using FeeSystem;

namespace FeeSystem.Tests;

[TestFixture]
public class FeeCalculatorTests 
{
    private FeeCalculator _calc;

    [SetUp]
    public void Setup()
    {
        _calc = new FeeCalculator();
    }

    // Checklist 1: No payments -> full fee outstanding
    [Test]
    public void OutstandingBalance_NoPayments_ReturnsFullFee() 
    {
        var payments = new List<decimal>();
        var result = _calc.OutstandingBalance(600m, payments);
        Assert.That(result, Is.EqualTo(600m));
    }

    // Checklist 2: One partial payment
    [Test]
    public void OutstandingBalance_OnePartialPayment_ReturnsRemainingBalance()
    {
        var payments = new List<decimal> { 200m };
        var result = _calc.OutstandingBalance(600m, payments);
        Assert.That(result, Is.EqualTo(400m));
    }

    // Checklist 3: Several instalments
    [Test]
    public void OutstandingBalance_SeveralInstalments_ReturnsRemainingBalance()
    {
        var payments = new List<decimal> { 200m, 200m, 100m };
        var result = _calc.OutstandingBalance(600m, payments);
        Assert.That(result, Is.EqualTo(100m));
    }

    // Checklist 4: Fee fully paid
    [Test]
    public void OutstandingBalance_FullyPaid_ReturnsZero()
    {
        var payments = new List<decimal> { 600m };
        var result = _calc.OutstandingBalance(600m, payments);
        Assert.That(result, Is.EqualTo(0m));
    }

    // Checklist 5: Overpayment
    [Test]
    public void OutstandingBalance_Overpayment_ReturnsNegativeBalance()
    {
        var payments = new List<decimal> { 700m };
        var result = _calc.OutstandingBalance(600m, payments);
        Assert.That(result, Is.EqualTo(-100m));
    }

    // Checklist 6: Negative fee throws exception
    [Test]
    public void OutstandingBalance_NegativeFee_ThrowsArgumentException()
    {
        var payments = new List<decimal>();
        Assert.That(() => _calc.OutstandingBalance(-1m, payments), Throws.ArgumentException);
    }

    // Checklist 7: Exactly half paid -> cleared true
    [Test]
    public void IsClearedForExams_ExactlyHalfPaid_ReturnsTrue()
    {
        var payments = new List<decimal> { 300m };
        var result = _calc.IsClearedForExams(600m, payments);
        Assert.That(result, Is.True);
    }

    // Checklist 8: One toea under half -> cleared false
    [Test]
    public void IsClearedForExams_OneToeaUnderHalf_ReturnsFalse()
    {
        var payments = new List<decimal> { 299.99m };
        var result = _calc.IsClearedForExams(600m, payments);
        Assert.That(result, Is.False);
    }
}
