using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace TheCursedMod.TheCursedModCode.Powers;

/// <summary>
/// 타락의 굴레 - 내 턴 시작 시, KarmaTurn1Power가 활성화된 경우 카드를 Amount장 뽑습니다.
/// </summary>
public class CycleOfDepravityPower : TheCursedModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(TheCursedModCode.Keywords.Karma)
    ];
    
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (!base.Owner.HasPower<KarmaTurn1Power>()
            && !base.Owner.HasPower<KarmaTurn2Power>()
            && !base.Owner.HasPower<KarmaTurn3Power>())
        {
            return;
        }
        Flash();
        await CardPileCmd.Draw(choiceContext, Amount, player);
    }
}
