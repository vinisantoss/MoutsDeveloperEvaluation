using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation.Sales;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation.Sales;

public class CustomerValidatorTests
{
    private readonly CustomerValidator _validator;

    public CustomerValidatorTests()
    {
        _validator = new CustomerValidator();
    }

    [Fact(DisplayName = "Valid customer should pass validation")]
    public void Given_ValidCustomer_When_Validated_Then_ShouldNotHaveErrors()
    {
        var customer = CustomerTestData.GenerateValidCustomer();

        var result = _validator.TestValidate(customer);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Customer without external ID should fail validation")]
    public void Given_CustomerWithoutExternalId_When_Validated_Then_ShouldHaveError()
    {
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.ExternalId = string.Empty;

        var result = _validator.TestValidate(customer);

        result.ShouldHaveValidationErrorFor(x => x.ExternalId)
            .WithErrorMessage("External ID is required");
    }

    [Fact(DisplayName = "Customer with external ID exceeding 50 characters should fail validation")]
    public void Given_CustomerWithExternalIdExceeding50Characters_When_Validated_Then_ShouldHaveError()
    {
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.ExternalId = new string('A', 51);

        var result = _validator.TestValidate(customer);

        result.ShouldHaveValidationErrorFor(x => x.ExternalId)
            .WithErrorMessage("External ID must not exceed 50 characters");
    }

    [Fact(DisplayName = "Customer without name should fail validation")]
    public void Given_CustomerWithoutName_When_Validated_Then_ShouldHaveError()
    {
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.Name = string.Empty;

        var result = _validator.TestValidate(customer);

        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Name is required");
    }

    [Fact(DisplayName = "Customer with name exceeding 100 characters should fail validation")]
    public void Given_CustomerWithNameExceeding100Characters_When_Validated_Then_ShouldHaveError()
    {
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.Name = new string('A', 101);

        var result = _validator.TestValidate(customer);

        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Name must not exceed 100 characters");
    }

    [Fact(DisplayName = "Customer without email should fail validation")]
    public void Given_CustomerWithoutEmail_When_Validated_Then_ShouldHaveError()
    {
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.Email = string.Empty;

        var result = _validator.TestValidate(customer);

        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email is required");
    }

    [Fact(DisplayName = "Customer with invalid email format should fail validation")]
    public void Given_CustomerWithInvalidEmail_When_Validated_Then_ShouldHaveError()
    {
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.Email = "invalid-email";

        var result = _validator.TestValidate(customer);

        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email must be a valid email address");
    }

    [Fact(DisplayName = "Customer with email exceeding 100 characters should fail validation")]
    public void Given_CustomerWithEmailExceeding100Characters_When_Validated_Then_ShouldHaveError()
    {
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.Email = $"{"a".PadLeft(90, 'a')}@example.com";

        var result = _validator.TestValidate(customer);

        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email must not exceed 100 characters");
    }

    [Fact(DisplayName = "Customer without document should fail validation")]
    public void Given_CustomerWithoutDocument_When_Validated_Then_ShouldHaveError()
    {
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.Document = string.Empty;

        var result = _validator.TestValidate(customer);

        result.ShouldHaveValidationErrorFor(x => x.Document)
            .WithErrorMessage("Document is required");
    }

    [Fact(DisplayName = "Customer with document less than 11 characters should fail validation")]
    public void Given_CustomerWithDocumentLessThan11Characters_When_Validated_Then_ShouldHaveError()
    {
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.Document = "12345";

        var result = _validator.TestValidate(customer);

        result.ShouldHaveValidationErrorFor(x => x.Document)
            .WithErrorMessage("Document must have at least 11 characters");
    }

    [Fact(DisplayName = "Customer with document exceeding 18 characters should fail validation")]
    public void Given_CustomerWithDocumentExceeding18Characters_When_Validated_Then_ShouldHaveError()
    {
        var customer = CustomerTestData.GenerateValidCustomer();
        customer.Document = new string('1', 19);

        var result = _validator.TestValidate(customer);

        result.ShouldHaveValidationErrorFor(x => x.Document)
            .WithErrorMessage("Document must not exceed 18 characters");
    }
}