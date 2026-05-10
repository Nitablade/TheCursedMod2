using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheCursedMod.TheCursedModCode.Powers;

/// <summary>
/// 금단의 형상 - 의례로 저주를 소멸시킬 때마다 다음 업보 공격의 피해량이 Amount% 증가합니다.
/// Amount = 강화 여부에 따라 25 or 50 (ForbiddenForm.OnPlay에서 설정).
/// RiteCard에서 MultiplyNextKarmaAttackPower를 더하도록 처리
/// </summary>
public class ForbiddenFormPower : TheCursedModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(TheCursedModCode.Keywords.Rite),
        HoverTipFactory.FromKeyword(TheCursedModCode.Keywords.Karma)
    ];
}
