using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace TheCursedMod.TheCursedModCode.Powers;

/// <summary>
/// 마나 요새 - 마법진의 효과가 발동될 때 마다, 방어도를 Amount 얻습니다.
/// CircleCard.TriggerEffect에서 효과 발동 직후 방어도를 적용합니다.
/// </summary>
public class ManaBastionPower : TheCursedModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(TheCursedModCode.Keywords.Circle),
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];

    public void TriggerFlash() => Flash();
}
