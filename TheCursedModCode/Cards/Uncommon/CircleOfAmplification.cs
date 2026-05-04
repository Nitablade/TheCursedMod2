using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheCursedMod.TheCursedModCode.Cards;

/// <summary>
/// 증폭의 마법진(Circle of Amplification) - 마법진 : 파워 카드를 사용할 때 마다 에너지를 1 얻습니다.
/// 강화 시 보존.
/// </summary>
public sealed class CircleOfAmplification() : CircleCard(CardRarity.Uncommon)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(TheCursedModCode.Keywords.Circle),
        EnergyHoverTip
    ];

    protected override bool ShouldTrigger(CardPlay cardPlay) =>
        cardPlay.Card.Type == CardType.Power;

    protected override async Task OnCircleEffect(PlayerChoiceContext choiceContext)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}
