using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.CardPools;
using TheCursedMod.TheCursedModCode.Extensions;
using TheCursedMod.TheCursedModCode.Powers;

namespace TheCursedMod.TheCursedModCode.Cards;

/// <summary>
/// 찌꺼기(Dregs) - 사용불가 저주 카드.
/// 의례 카드의 재료로 사용되는 저주 카드.
/// CurseCardPool에 추가하여 보라색 모드 프레임 대신 기본 저주 카드 프레임(검은색)을 사용.
/// </summary>
[Pool(typeof(CurseCardPool))]
public sealed class Dregs() : CustomCardModel(-1, CardType.Curse, CardRarity.Curse, TargetType.None, showInCardLibrary: false)
{
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override bool CanBeGeneratedByModifiers => false;
    public override int MaxUpgradeLevel => 0;

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];

    /// <summary>
    /// target 카드를 Dregs로 변환합니다.
    /// RecyclableWastePower가 활성화된 경우 Flash()를 실행합니다.
    /// </summary>
    public static async Task TransformToDregs(CardModel source, CardModel target)
    {
        var power = source.Owner?.Creature.GetPower<RecyclableWastePower>();
        var newDregs = target.CombatState!.CreateCard<Dregs>(target.Owner);
        if (power != null) CardCmd.ApplyKeyword(newDregs, CardKeyword.Retain);
        await CardCmd.Transform(target, newDregs);
        power?.OnDregsCreated();
    }

    /// <summary>
    /// 이미 생성된 Dregs 카드를 패에 추가합니다.
    /// RecyclableWastePower가 활성화된 경우 Retain을 부여하고 OnDregsCreated를 호출합니다.
    /// </summary>
    public async Task AddToHand(bool addedByPlayer = false)
    {
        var power = Owner?.Creature?.GetPower<RecyclableWastePower>();
        if (power != null) CardCmd.ApplyKeyword(this, CardKeyword.Retain);
        await CardPileCmd.AddGeneratedCardToCombat(this, PileType.Hand, addedByPlayer: addedByPlayer);
        power?.OnDregsCreated();
    }

    /// <summary>
    /// Dregs를 numCards장 생성하여 패에 추가합니다.
    /// RecyclableWastePower가 활성화된 경우 Flash()를 한 번만 실행합니다.
    /// </summary>
    public static async Task CreateAndAddToHand(Player owner, int numCards)
    {
        var combatState = owner.Creature?.CombatState;
        if (combatState == null) return;

        for (int i = 0; i < numCards; i++)
        {
            var dregs = combatState.CreateCard<Dregs>(owner);
            await dregs.AddToHand(addedByPlayer: true);
        }
    }
}
