using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheCursedMod.TheCursedModCode.Powers;

namespace TheCursedMod.TheCursedModCode.Cards;

/// <summary>
/// 대악마의 검(Archdemon Sword) - 피해를 13 줍니다. 손에 있는 저주 카드를 전부 소멸시키고
/// 그 수만큼 힘을 1 얻습니다. 업보 13. 강화 시 힘 2.
/// 비용 1, Ancient, Attack.
/// </summary>
public sealed class ArchdemonSword() : TheCursedModCard(1, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy), IKarmaAttack
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(13, ValueProp.Move),
        new PowerVar<StrengthPower>("StrengthPower", 1m),
        new PowerVar<KarmaTurn2Power>("KarmaPower", 13m)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust),
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromKeyword(TheCursedModCode.Keywords.Karma)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);

        var curseCards = PileType.Hand.GetPile(Owner).Cards
            .Where(c => c.Type == CardType.Curse)
            .ToList();

        foreach (var card in curseCards)
        {
            await CardCmd.Exhaust(choiceContext, card);
            await CommonActions.Apply<StrengthPower>(choiceContext, Owner!.Creature, this, DynamicVars["StrengthPower"].IntValue);
        }

        await ApplyKarma(DynamicVars["KarmaPower"].IntValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["StrengthPower"].UpgradeValueBy(1m);
    }
}
