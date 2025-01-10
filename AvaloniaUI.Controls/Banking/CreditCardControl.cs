using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace AvaloniaUI.Controls.Banking;

public class CreditCardControl : TemplatedControl
{
    // Font Family Property
    public static readonly StyledProperty<FontFamily> CardFontFamilyProperty =
        AvaloniaProperty.Register<CreditCardControl, FontFamily>(
            nameof(CardFontFamily)
        );

    // CardType Property
    public static readonly StyledProperty<CreditCardStyle.CreditCardType> CardTypeProperty =
        AvaloniaProperty.Register<CreditCardControl, CreditCardStyle.CreditCardType>(
            nameof(CardType)
        );

    // BackgroundColor Property
    public static readonly StyledProperty<IBrush> BackgroundColorProperty =
        AvaloniaProperty.Register<CreditCardControl, IBrush>(
            nameof(BackgroundColor),
            CreditCardStyle.BackgroundColor[CreditCardStyle.CreditCardType.Default]
        );

    // ExpirationDate Property
    public static readonly StyledProperty<string> LogoTextProperty =
        AvaloniaProperty.Register<CreditCardControl, string>(
            nameof(LogoText), "\uf791");

    // CardNumber Property
    public static readonly StyledProperty<string> CardNumberProperty =
        AvaloniaProperty.Register<CreditCardControl, string>(
            nameof(CardNumber),
            string.Empty
            //coerce: (_, value) => SecureDataHelper.Encrypt(value)//, // Encrypt on set
            //validate: ValidateCardNumber
        );

    // ExpirationDate Property
    public static readonly StyledProperty<string> ExpirationDateProperty =
        AvaloniaProperty.Register<CreditCardControl, string>(
            nameof(ExpirationDate),
            string.Empty
        );

    // CVV Property
    public static readonly StyledProperty<string> CVVProperty =
        AvaloniaProperty.Register<CreditCardControl, string>(
            nameof(CVV),
            string.Empty
        );

    // CardholderName Property
    public static readonly StyledProperty<string> CardholderNameProperty =
        AvaloniaProperty.Register<CreditCardControl, string>(
            nameof(CardholderName),
            string.Empty
        );

    static CreditCardControl()
    {
        // Subscribe to changes in CardNumberProperty
        CardNumberProperty.Changed.AddClassHandler<CreditCardControl>(CardNumberPropertyChanged);
    }

    public string CardNumber
    {
        get => SecureDataHelper.Decrypt(GetValue(CardNumberProperty));
        set => SetValue(CardNumberProperty, SecureDataHelper.Encrypt(value.Replace(" ", "")));
    }

    public string ExpirationDate
    {
        get => SecureDataHelper.Decrypt(GetValue(ExpirationDateProperty));
        set => SetValue(ExpirationDateProperty, SecureDataHelper.Encrypt(value));
    }

    public string CVV
    {
        get => SecureDataHelper.Decrypt(GetValue(CVVProperty));
        set => SetValue(CVVProperty, SecureDataHelper.Encrypt(value));
    }

    public string CardholderName
    {
        get => SecureDataHelper.Decrypt(GetValue(CardholderNameProperty));
        set => SetValue(CardholderNameProperty, SecureDataHelper.Encrypt(value));
    }

    public CreditCardStyle.CreditCardType CardType
    {
        get => GetValue(CardTypeProperty);
        set => SetValue(CardTypeProperty, value);
    }

    public IBrush BackgroundColor
    {
        get => GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    public string LogoText
    {
        get => GetValue(LogoTextProperty);
        set => SetValue(LogoTextProperty, value);
    }

    public FontFamily CardFontFamily
    {
        get => GetValue(CardFontFamilyProperty);
        set => SetValue(CardFontFamilyProperty, value);
    }

    /// <summary>
    ///     Handle Card Type Change
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private static void CardNumberPropertyChanged(CreditCardControl sender, AvaloniaPropertyChangedEventArgs args)
    {
        sender.UpdateCardTypeAndBackground();
    }

    private void UpdateCardTypeAndBackground()
    {
        var decryptedCardNumber = GetValue(CardNumberProperty)?.Replace(" ", "");
        if (!string.IsNullOrEmpty(decryptedCardNumber))
            foreach (var match in CreditCardStyle.CreditCardTypeMatch)
                if (match.Key.IsMatch(decryptedCardNumber))
                {
                    CardType = match.Value;
                    BackgroundColor = CreditCardStyle.BackgroundColor[match.Value];
                    LogoText = CreditCardStyle.Logo[match.Value];
                    return;
                }

        // Default to Default card type if no match is found
        CardType = CreditCardStyle.CreditCardType.Default;
        LogoText = CreditCardStyle.Logo[CreditCardStyle.CreditCardType.Default];
        BackgroundColor = CreditCardStyle.BackgroundColor[CreditCardStyle.CreditCardType.Default];
    }

    private static bool ValidateCardNumber(string value)
    {
        // You can add custom validation for the card number here if needed
        return !string.IsNullOrWhiteSpace(value);
    }
}