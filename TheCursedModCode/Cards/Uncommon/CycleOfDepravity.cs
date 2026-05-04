using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheCursedMod.TheCursedModCode.Powers;

namespace TheCursedMod.TheCursedModCode.Cards;

/// <summary>
/// 타락의 굴레(Cycle of Depravity) - 내 턴 시작 시, 업보가 있다면 카드를 2장 뽑습니다.
/// 강화 시 카드 3장.
/// </summary>
public sealed class CycleOfDepravity() : TheCursedModCard(0, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<CycleOfDepravityPower>(2m)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(TheCursedModCode.Keywords.Karma)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner!.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<CycleOfDepravityPower>(Owner.Creature, DynamicVars["CycleOfDepravityPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["CycleOfDepravityPower"].UpgradeValueBy(1m);
    }
}
