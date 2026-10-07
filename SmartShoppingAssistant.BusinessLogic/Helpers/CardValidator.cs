using SmartShoppingAssistant.BusinessLogic.DTOs.Order;

namespace SmartShoppingAssistant.BusinessLogic.Helpers;

// Simulated card payment: checks the card the way a payment form would, nothing is charged or stored
public static class CardValidator
{
    // Same idea as the test cards of real payment providers: this number is always declined
    public const string DeclinedTestCard = "4000000000000002";

    public static string Validate(CardPaymentDTO? card, DateTime now)
    {
        if (card is null)
            throw new BusinessException("Please enter your card details.");

        var number = new string(card.Number.Where(char.IsDigit).ToArray());
        if (number.Length is < 13 or > 19 || !PassesLuhn(number))
            throw new BusinessException("The card number is not valid.");

        var lastDayOfExpiry = new DateTime(card.ExpiryYear, card.ExpiryMonth, 1).AddMonths(1);
        if (lastDayOfExpiry <= now.Date)
            throw new BusinessException("The card has expired.");

        if (card.Cvv.Length is < 3 or > 4 || !card.Cvv.All(char.IsDigit))
            throw new BusinessException("The CVV is the 3 digit code on the back of the card.");

        if (number == DeclinedTestCard)
            throw new BusinessException("Your card was declined by the bank. Please use another card.");

        return number[^4..];
    }

    private static bool PassesLuhn(string digits)
    {
        var sum = 0;
        var doubleIt = false;
        for (var i = digits.Length - 1; i >= 0; i--)
        {
            var d = digits[i] - '0';
            if (doubleIt)
            {
                d *= 2;
                if (d > 9) d -= 9;
            }
            sum += d;
            doubleIt = !doubleIt;
        }
        return sum % 10 == 0;
    }
}
