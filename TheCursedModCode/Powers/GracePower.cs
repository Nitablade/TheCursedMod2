using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace TheCursedMod.TheCursedModCode.Powers;

/// <summary>
/// 유예 - 업보 피해 타이밍을 1턴 연장합니다.
/// 적용 시: 현재 KarmaTurn1Power -> KarmaTurn2Power, KarmaTurn2Power -> KarmaTurn3Power로 변환.
/// 만일 K1, K2 동시에 갖고 있었다면, 둘 다 없앤 이후 K2, K3 순서대로 얻음
/// 이후 카드로 업보를 얻을 때 KarmaTurn2Power 대신 KarmaTurn3Power를 얻습니다 (TheCursedModCard.ApplyKarma 에서 처리).
/// </summary>
public class GracePower : TheCursedModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(TheCursedModCode.Keywords.Karma)
    ];

    public void TriggerFlash() => Flash();

    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        var k1 = base.Owner.GetPower<KarmaTurn1Power>();
        var k2 = base.Owner.GetPower<KarmaTurn2Power>();

        // The order of removing/applying powers matters!
        if (k1 != null && k2 != null) {
            // K1->K2, K2->K3
            int k1Amount = k1.Amount;
            int k2Amount = k2.Amount;
            await PowerCmd.Remove(k1);
            await PowerCmd.Remove(k2);

            // K2, K3 순으로 얻음 (그래야 2가 왼쪽으로 감)
            await PowerCmd.Apply<KarmaTurn2Power>(base.Owner, k1Amount, base.Owner, null);
            await PowerCmd.Apply<KarmaTurn3Power>(base.Owner, k2Amount, base.Owner, null);
        }
        else if (k1 != null) {
            // K1→K2 처리
            int k1Amount = k1.Amount;
            await PowerCmd.Remove(k1);
            await PowerCmd.Apply<KarmaTurn2Power>(base.Owner, k1Amount, base.Owner, null);
        }
        else if (k2 != null) {
            // K2->K3
            int k2Amount = k2.Amount;
            await PowerCmd.Remove(k2);
            await PowerCmd.Apply<KarmaTurn3Power>(base.Owner, k2Amount, base.Owner, null);
        }
    }
}
