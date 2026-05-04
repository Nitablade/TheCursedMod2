using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheCursedMod.TheCursedModCode.Powers;

namespace TheCursedMod.TheCursedModCode.Cards;

/// <summary>
/// 급소 타격(Vital Strike) - 피해를 6 줍니다. 업보가 있다면, 취약을 2 부여합니다. (강화 시 피해 9, 취약 3)
/// </summary>
public sealed class VitalStrike() : TheCursedModCard(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(6, ValueProp.Move),
        new PowerVar<VulnerablePower>("VulnerablePower", 2),
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<VulnerablePower>(),
        HoverTipFactory.FromKeyword(TheCursedModCode.Keywords.Karma)
    ];

    protected override bool ShouldGlowGoldInternal => HasKarma();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        if (play.Target != null && HasKarma())
            await CommonActions.Apply<VulnerablePower>(choiceContext, play.Target, this, DynamicVars.Vulnerable.BaseValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars.Vulnerable.UpgradeValueBy(1m);
    }

    private bool HasKarma() =>
        Owner?.Creature.HasPower<KarmaTurn1Power>() == true ||
        Owner?.Creature.HasPower<KarmaTurn2Power>() == true ||
        Owner?.Creature.HasPower<KarmaTurn3Power>() == true;
}
