using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheCursedMod.TheCursedModCode.Cards;

namespace TheCursedMod.TheCursedModCode.Powers;

/// <summary>
/// 다음 업보 공격 강화 - 업보를 부여하는 다음 공격의 피해량이 Amount% 증가합니다.
/// 의례로 저주를 소멸시킬 때마다 ForbiddenFormPower.Amount만큼 누적됩니다.
/// 업보 공격 카드 사용 시 소멸합니다.
/// </summary>
public class MultiplyNextKarmaAttackPower : TheCursedModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(TheCursedModCode.Keywords.Karma)
    ];

    public override decimal ModifyDamageMultiplicative(
        Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (Amount > 0 && cardSource is IKarmaAttack && cardSource.Owner?.Creature == Owner)
            return 1m + Amount / 100m;

        return 1m;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Card is IKarmaAttack && play.Card.Owner?.Creature == Owner && Amount > 0)
        {
            Flash();
            await PowerCmd.Remove(this);
        }
    }
}
