namespace EShop.Basket.Domain.Basket.Dtos;

/// <summary>
/// Data Transfer Object representing the checkout information for a shopping basket.
/// </summary>
public record BasketCheckoutDto
{
    /// <summary>
    /// The username of the customer performing the checkout.
    /// </summary>
    public string UserName { get; init; } = null!;

    /// <summary>
    /// Unique identifier of the customer.
    /// </summary>
    public Guid CustomerId { get; init; }

    /// <summary>
    /// Total price of all items in the basket.
    /// </summary>
    public decimal TotalPrice { get; init; }

    // Shipping and Billing Address

    /// <summary>
    /// First name of the customer.
    /// </summary>
    public string FirstName { get; init; } = null!;

    /// <summary>
    /// Last name of the customer.
    /// </summary>
    public string LastName { get; init; } = null!;

    /// <summary>
    /// Email address of the customer.
    /// </summary>
    public string EmailAddress { get; init; } = null!;

    /// <summary>
    /// Street address or address line of the customer.
    /// </summary>
    public string AddressLine { get; init; } = null!;

    /// <summary>
    /// Country of residence.
    /// </summary>
    public string Country { get; init; } = null!;

    /// <summary>
    /// State or province of residence.
    /// </summary>
    public string State { get; init; } = null!;

    /// <summary>
    /// ZIP or postal code.
    /// </summary>
    public string ZipCode { get; init; } = null!;

    // Payment Details

    /// <summary>
    /// Name on the payment card.
    /// </summary>
    public string CardName { get; init; } = null!;

    /// <summary>
    /// Credit or debit card number.
    /// </summary>
    public string CardNumber { get; init; } = null!;

    /// <summary>
    /// Card expiration date in MM/YY format.
    /// </summary>
    public string Expiration { get; init; } = null!;

    /// <summary>
    /// Card verification value (CVV or CVC).
    /// </summary>
    public string Cvv { get; init; } = null!;

    /// <summary>
    /// Identifier for the selected payment method. 
    /// Example: 1 = Credit Card, 2 = PayPal, etc.
    /// </summary>
    public int PaymentMethod { get; init; }
}
