using System.Linq;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using TheCursedMod.TheCursedModCode.Character;
using TheCursedMod.TheCursedModCode.Extensions;
using TheCursedMod.TheCursedModCode.Powers;
using TheCursedMod.TheCursedModCode.Relics;

namespace TheCursedMod.TheCursedModCode.Cards;

[Pool(typeof(TheCursedModCardPool))]
public abstract class TheCursedModCard(
    int cost,
    CardType type,
    CardRarity rarity,
    TargetType target,
    bool showInCardLibrary = true,
    bool autoAdd = true)
    : CustomCardModel(cost, type, rarity, target, showInCardLibrary, autoAdd)
{
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();

    //Smaller variants: fullart 250x350, normalart 250x190
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

    /// <summary>
    /// 카드가 "사용불가"인지 확인합니다.
    /// HasUnplayableKeyword 이거나 마법진(CircleCard) 카드인 경우 사용불가로 간주합니다.
    /// </summary>
    public static bool IsUnplayableCard(CardModel c) =>
        c.Keywords.Contains(CardKeyword.Unplayable);


    /// <summary>
    /// 손에 사용불가 카드가 있는지 확인합니다.
    /// </summary>
    protected bool HasUnplayableInHand() =>
        PileType.Hand.GetPile(Owner).Cards.Any(IsUnplayableCard);

    /// <summary>
    /// 무작위 저주 카드를 지정한 파일에 추가합니다.
    /// 네잎클로버 부적이 있다면 대신 찌꺼기를 패에 추가합니다.
    /// </summary>
    protected Task GainRandomCurse(PileType pile, Player? targetPlayer = null)
        => GainRandomCurse(targetPlayer ?? Owner, Owner, CombatState, pile, addedByPlayer: false);

    /// <summary>
    /// 무작위 저주 카드를 지정한 파일에 추가합니다. (카드 외부에서도 호출 가능한 static 버전)
    /// 네잎클로버 부적이 있다면 대신 찌꺼기를 패에 추가합니다.
    /// </summary>
    public static async Task GainRandomCurse(Player target, Player rngSource, CombatState? combatState, PileType pile, bool addedByPlayer = false)
    {
        var baseCurses = ModelDb.CardPool<CurseCardPool>()
            .GetUnlockedCards(target.UnlockState, target.RunState.CardMultiplayerConstraint)
            .Where(c => c.CanBeGeneratedByModifiers && c is not Guilty)  // Guilty is meaningless in combat
            .Where(c => !c.Id.Entry.Contains('-'))  // Exclude curses from Mods
            .ToList();
        var curseCandidates = baseCurses
            .Concat(baseCurses)  // Double the chances for base curses to be selected
            .Append(ModelDb.Card<Enthralled>())
            .Append(ModelDb.Card<BadLuck>())
            .Append(ModelDb.Card<PoorSleep>())
            .ToList();

        if (curseCandidates.Count == 0) return;

        // RNG는 relic 여부와 무관하게 항상 소비하여 multiplayer desync 방지
        var randomCurse = rngSource.RunState.Rng.CombatCardGeneration.NextItem(curseCandidates)!;

        var cloverRelic = target.Relics.OfType<FourLeafCloverCharmRelic>().FirstOrDefault();
        CardModel card;
        if (cloverRelic != null)
        {
            cloverRelic.Flash();
            card = combatState!.CreateCard<Dregs>(target);
        }
        else
        {
            card = combatState!.CreateCard(randomCurse, target);
        }

        // Dregs가 Hand에 생성될 시 RecyclableWastePower 여부에 따라 특별한 처리가 필요.
        if (card is Dregs dregs && pile == PileType.Hand)
        {
            await dregs.AddToHand(addedByPlayer: addedByPlayer);
        }
        else if (pile == PileType.Draw)
        {
            CardCmd.PreviewCardPileAdd(
                await CardPileCmd.AddGeneratedCardToCombat(
                    card, pile, addedByPlayer: addedByPlayer, position: CardPilePosition.Random));
            await Cmd.Wait(0.5f);
        }
        else
        {
            await CardPileCmd.AddGeneratedCardToCombat(card, pile, addedByPlayer: addedByPlayer);
        }
    }

    /// <summary>
    /// GracePower가 활성화된 경우 KarmaTurn3Power를, 그렇지 않으면 KarmaTurn2Power를 적용합니다.
    /// </summary>
    protected Task ApplyKarma(decimal amount)
    {
        if (Owner.Creature.HasPower<GracePower>()) {
            Owner.Creature.GetPower<GracePower>()!.TriggerFlash();
            return PowerCmd.Apply<KarmaTurn3Power>(Owner.Creature, amount, Owner.Creature, this);
        }
        return PowerCmd.Apply<KarmaTurn2Power>(Owner.Creature, amount, Owner.Creature, this);
    }
}
