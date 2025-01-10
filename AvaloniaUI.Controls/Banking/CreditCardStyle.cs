using System.Collections.Generic;
using System.Text.RegularExpressions;
using Avalonia.Media;

namespace AvaloniaUI.Controls.Banking;

public static class CreditCardStyle
{
    public enum CreditCardType
    {
        Default,
        AmericanExpress,
        DinersClub,
        Discover,
        JCB,
        Visa,
        MasterCard,
        Maestro,
        UnionPay
    }

    public static Dictionary<Regex, CreditCardType> CreditCardTypeMatch = new()
    {
        { new Regex(@"^(5018|5020|5038|6304|6759|6761|6763)[0-9]{8,15}$"), CreditCardType.Maestro },
        { new Regex(@"^(62[0-9]{14,17})$"), CreditCardType.UnionPay },
        { new Regex(@"^3[47][0-9]{13}$"), CreditCardType.AmericanExpress },
        { new Regex(@"^3(?:0[0-5]|[68][0-9])[0-9]{11}$"), CreditCardType.DinersClub },
        { new Regex(@"^6(?:011|5[0-9]{2})[0-9]{12}$"), CreditCardType.Discover },
        { new Regex(@"^(?:2131|1800|35\d{3})\d{11}$"), CreditCardType.JCB },
        { new Regex(@"^4[0-9]{12}(?:[0-9]{3})?$"), CreditCardType.Visa },
        {
            new Regex(@"^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$"),
            CreditCardType.MasterCard
        }
    };

    public static Dictionary<CreditCardType, IBrush> BackgroundColor = new()
    {
        { CreditCardType.AmericanExpress, Brush.Parse("#3177CB") },
        { CreditCardType.DinersClub, Brush.Parse("#1B4F8F") },
        { CreditCardType.Discover, Brush.Parse("#E9752F") },
        { CreditCardType.JCB, Brush.Parse("#9E2911") },
        { CreditCardType.MasterCard, Brush.Parse("#394854") },
        { CreditCardType.Visa, Brush.Parse("#2867BA") },
        { CreditCardType.Default, Brush.Parse("#75849D") }
    };

    public static Dictionary<CreditCardType, string> Logo = new()
    {
        { CreditCardType.AmericanExpress, "\uf1f3" },
        { CreditCardType.DinersClub, "\uf24c" },
        { CreditCardType.Discover, "\uf1f2" },
        { CreditCardType.JCB, "\uf24b" },
        { CreditCardType.MasterCard, "\uf1f1" },
        { CreditCardType.Visa, "\uf1f0" },
        { CreditCardType.Default, "\uf09d" },
        { CreditCardType.UnionPay, "\uf642" }
    };
}