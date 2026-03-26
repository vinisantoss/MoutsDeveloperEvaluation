using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class CustomerTests
{
    [Fact(DisplayName = "Customer should be created with valid data")]
    public void Given_ValidData_When_Creating_Then_CustomerShouldBeCreated()
    {
        var customer = CustomerTestData.GenerateValidCustomer();

        customer.Id.Should().NotBe(Guid.Empty);
        customer.ExternalId.Should().NotBeEmpty();
        customer.Name.Should().NotBeEmpty();
        customer.Email.Should().NotBeEmpty();
        customer.Document.Should().NotBeEmpty();
    }

    [Fact(DisplayName = "Validation should pass for valid customer")]
    public void Given_ValidCustomer_When_Validated_Then_ShouldReturnValid()
    {
        var customer = CustomerTestData.GenerateValidCustomer();

        var result = customer.Validate();

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact(DisplayName = "Validation should fail for invalid customer data")]
    public void Given_InvalidCustomer_When_Validated_Then_ShouldReturnInvalid()
    {
        var customer = new Customer
        {
            ExternalId = "",
            Name = "",
            Email = "invalid-email",
            Document = ""
        };

        var result = customer.Validate();

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }
}