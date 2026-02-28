using System.Diagnostics.CodeAnalysis;
using Content.Client.UserInterface.ControlExtensions;
using Content.Shared._Stalker_EN.MercBoard;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.RichText;
using Robust.Shared.Input;
using Robust.Shared.Utility;

namespace Content.Client._Stalker_EN.PdaMessenger;

/// <summary>
/// Markup tag handler for clickable offer references in messenger messages.
/// Renders <c>[offerlink=3][/offerlink]</c> as a blue clickable "[MB#3]" label.
/// Click handling is delegated to the nearest parent implementing <see cref="IOfferLinkClickHandler"/>.
/// </summary>
public sealed class OfferLinkTag : IMarkupTagHandler
{
    public string Name => "offerlink";

    public bool TryCreateControl(MarkupNode node, [NotNullWhen(true)] out Control? control)
    {
        if (!node.Value.TryGetLong(out var longId))
        {
            control = null;
            return false;
        }

        var id = (uint) longId;
        var label = new Label
        {
            Text = $"[{STMercBoardOffer.OfferRefPrefix}{id}]",
            MouseFilter = Control.MouseFilterMode.Stop,
            FontColorOverride = Color.CornflowerBlue,
            DefaultCursorShape = Control.CursorShape.Hand,
        };

        label.OnMouseEntered += _ => label.FontColorOverride = Color.LightSkyBlue;
        label.OnMouseExited += _ => label.FontColorOverride = Color.CornflowerBlue;
        label.OnKeyBindDown += args =>
        {
            if (args.Function != EngineKeyFunctions.UIClick)
                return;

            if (label.TryGetParentHandler<IOfferLinkClickHandler>(out var handler))
                handler.HandleOfferLinkClick(id);
        };

        control = label;
        return true;
    }
}

/// <summary>
/// Interface for controls that handle offer link clicks from <see cref="OfferLinkTag"/>.
/// </summary>
public interface IOfferLinkClickHandler
{
    void HandleOfferLinkClick(uint offerId);
}
