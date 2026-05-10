using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace TheCursedMod.TheCursedModCode.Powers;

/// <summary>
/// 매 턴 업보 - 내 턴 시작 시 Amount만큼 업보를 얻습니다.
/// TearGrimoire, DieTogether 등 매 턴 업보를 부여하는 카드에서 공통 사용됩니다.
/// </summary>
public class KarmaEveryTurnPower : TheCursedModPower
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(TheCursedModCode.Keywords.Karma)
    ];

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != base.Owner.Player) return;
        Flash();
        if (base.Owner.HasPower<GracePower>())
            await PowerCmd.Apply<KarmaTurn3Power>(base.Owner, Amount, base.Owner, null);
        else
            await PowerCmd.Apply<KarmaTurn2Power>(base.Owner, Amount, base.Owner, null);
    }
}
