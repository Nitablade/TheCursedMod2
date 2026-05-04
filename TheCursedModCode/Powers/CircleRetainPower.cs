using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using TheCursedMod.TheCursedModCode.Cards;

namespace TheCursedMod.TheCursedModCode.Powers;

/// <summary>
/// 마법진 보존 - Amount 턴 동안 손에 있는 마법진 카드를 버리지 않습니다.
/// 매 턴 종료 시 카운터가 1 감소하며, 0이 되면 모든 파일의 마법진에서 Retain 키워드를 제거하고 파워가 소멸합니다.
/// </summary>
public class CircleRetainPower : TheCursedModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        Flash();

        var hand = Owner.Player?.PlayerCombatState?.Hand;
        if (hand != null)
            foreach (var card in hand.Cards.OfType<CircleCard>())
                CardCmd.ApplyKeyword(card, CardKeyword.Retain);

        return Task.CompletedTask;
    }

    public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card.Owner.Creature == Owner && card is CircleCard)
            CardCmd.ApplyKeyword(card, CardKeyword.Retain);

        return Task.CompletedTask;
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Side) return;

        if (Amount == 1)
        {
            // 마지막 턴 — Retain 제거 후 파워 소멸
            Flash();
            var pcs = Owner.Player?.PlayerCombatState;
            if (pcs != null)
                foreach (var card in pcs.AllCards.OfType<CircleCard>())
                    if (card.Enchantment is not Steady && !card.CanonicalKeywords.Contains(CardKeyword.Retain))
                        CardCmd.RemoveKeyword(card, CardKeyword.Retain);
        }

        await PowerCmd.Decrement(this);
    }
}
