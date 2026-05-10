using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace TheCursedMod.TheCursedModCode.Powers;

/// <summary>
/// 함께 폭사하자 - 이번 전투 동안 업보로 인한 피해를 모든 적도 같이 받습니다.
/// 실제 피해는 KarmaTurn1Power가 처리합니다.
/// </summary>
public class DieTogetherPower : TheCursedModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(TheCursedModCode.Keywords.Karma)
    ];

    public void TriggerFlash() => Flash();
}
