using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheCursedMod.TheCursedModCode.Powers;

/// <summary>
/// 세계선 비틀기 - 이번 턴에 업보로 인한 피해를 받지 않습니다.
/// 턴 종료 시 소멸합니다.
/// </summary>
public class WorldlineTwistPower : TheCursedModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(TheCursedModCode.Keywords.Karma)
    ];

    public override decimal ModifyDamageMultiplicative(
        Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        // 업보 피해의 특징: target == dealer (자기 자신), cardSource 없음, props == Unpowered 단독
        if (target != Owner) return 1m;
        if (dealer != Owner) return 1m;
        if (cardSource != null) return 1m;
        if (props != ValueProp.Unpowered) return 1m;
        return 0m;
    }

    public void TriggerFlash() => Flash();

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == Owner.Side)
            await PowerCmd.Remove(this);
    }
}
