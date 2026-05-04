using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheCursedMod.TheCursedModCode.Cards;

/// <summary>
/// 질풍의 마법진(Circle of Windfury) - 마법진 : 비용이 0인 카드를 사용할 때 마다 카드를 1장 뽑습니다. 휘발성.
/// 강화 시 휘발성 제거.
/// </summary>
public sealed class CircleOfWindfury() : CircleCard(CardRarity.Rare)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable, CardKeyword.Ethereal];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(1),
        new EnergyVar(1)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(TheCursedModCode.Keywords.Circle),
        EnergyHoverTip
    ];

    protected override bool ShouldTrigger(CardPlay cardPlay) =>
        cardPlay.Card.EnergyCost.GetWithModifiers(CostModifiers.Local) == 0;

    protected override async Task OnCircleEffect(PlayerChoiceContext choiceContext)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Ethereal);
    }
}
