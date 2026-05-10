using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheCursedMod.TheCursedModCode.Powers;

/// <summary>
/// 고통 감내(Ignore Pain) - 이번 턴에 업보로 받는 피해량이 Amount% 감소합니다.
/// 중첩되며 최대 100까지 고정됩니다. 100이면 업보 피해가 0이 됩니다.
/// </summary>
public class IgnorePainPower : TheCursedModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(TheCursedModCode.Keywords.Karma)
    ];

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        // Karma 피해의 특징: target == dealer (자기 자신), cardSource 없음, props == Unpowered 단독
        if (target != base.Owner) return 1m;
        if (dealer != base.Owner) return 1m;
        if (cardSource != null) return 1m;
        if (props != ValueProp.Unpowered) return 1m;
        decimal reduction = Math.Min(Amount, 100m) / 100m;
        return 1m - reduction;
    }

    public void TriggerFlash() => Flash();

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == base.Owner.Side)
        {
            await PowerCmd.Remove(this);
        }
    }
}
