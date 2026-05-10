using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheCursedMod.TheCursedModCode.Powers;

/// <summary>
/// 돌려 막기 - 업보가 있다면, 턴 종료 시 방어도를 Amount 얻습니다.
/// KarmaTurn1Power가 활성화된 턴에 StartPulsing, 아니면 StopPulsing.
/// </summary>
public class RollingOverDebtPower : TheCursedModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(TheCursedModCode.Keywords.Karma),
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != base.Owner.Player) return Task.CompletedTask;
        if (HasKarma())
            StartPulsing();
        else
            StopPulsing();
        return Task.CompletedTask;
    }
    
    public override async Task BeforeTurnEndVeryEarly(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != base.Owner.Side) return;
        if (HasKarma())
        {
            Flash();
            await CreatureCmd.GainBlock(base.Owner, Amount, ValueProp.Unpowered, null);
            await Cmd.Wait(0.5f);
        }
    }

    private bool HasKarma()
    {
        return base.Owner.HasPower<KarmaTurn1Power>()
                || base.Owner.HasPower<KarmaTurn2Power>()
                || base.Owner.HasPower<KarmaTurn3Power>();
    }
}
