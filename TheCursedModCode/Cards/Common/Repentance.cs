using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheCursedMod.TheCursedModCode.Powers;

namespace TheCursedMod.TheCursedModCode.Cards;

/// <summary>
/// 뉘우침(Repentance) - 방어도를 5 얻습니다.
/// 업보가 있다면, 대신 14 얻습니다.
/// 강화 시 5→8 / 14→18
/// </summary>
public sealed class Repentance() : TheCursedModCard(1, CardType.Skill, CardRarity.Common, TargetType.None)
{
    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(5, ValueProp.Move),
        new BlockVar("KarmaBlock", 14, ValueProp.Move)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(TheCursedModCode.Keywords.Karma)
    ];

    protected override bool ShouldGlowGoldInternal => HasKarma();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner!.Creature, "Cast", Owner.Character.CastAnimDelay);
        decimal blockAmount = HasKarma() ? DynamicVars["KarmaBlock"].PreviewValue : DynamicVars.Block.PreviewValue;
        await CreatureCmd.GainBlock(Owner.Creature, blockAmount, ValueProp.Move, play);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
        DynamicVars["KarmaBlock"].UpgradeValueBy(4m);
    }

    private bool HasKarma() =>
        Owner?.Creature.HasPower<KarmaTurn1Power>() == true ||
        Owner?.Creature.HasPower<KarmaTurn2Power>() == true ||
        Owner?.Creature.HasPower<KarmaTurn3Power>() == true;
}
