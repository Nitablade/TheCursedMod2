using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheCursedMod.TheCursedModCode.Cards;

/// <summary>
/// 집중의 마법진(Circle of Focus) - 마법진 : 비용이 3 이상인 카드를 사용할 때 마다 에너지를 1 얻습니다.
/// 강화 시 비용 2 이상으로 조건 완화.
/// </summary>
public sealed class CircleOfFocus() : CircleCard(CardRarity.Uncommon)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new EnergyVar(1),
        new EnergyVar("CostThreshold", 3)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(TheCursedModCode.Keywords.Circle),
        EnergyHoverTip
    ];

    protected override bool ShouldTrigger(CardPlay cardPlay) =>
        cardPlay.Card.EnergyCost.GetWithModifiers(CostModifiers.Local) >= (int)DynamicVars["CostThreshold"].BaseValue;

    protected override async Task OnCircleEffect(PlayerChoiceContext choiceContext)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["CostThreshold"].UpgradeValueBy(-1m);
    }
}
