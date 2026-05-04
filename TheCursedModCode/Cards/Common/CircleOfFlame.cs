using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheCursedMod.TheCursedModCode.Cards;

/// <summary>
/// 불 뿜는 마법진(Circle of Flame) - 마법진 : 공격 카드를 사용할 때 마다 모든 적에게 피해를 5 줍니다.
/// 강화 시 피해 8.
/// </summary>
public sealed class CircleOfFlame() : CircleCard(CardRarity.Common)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(5, ValueProp.Unpowered)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(TheCursedModCode.Keywords.Circle)
    ];

    protected override bool ShouldTrigger(CardPlay cardPlay) =>
        cardPlay.Card.Type == CardType.Attack;

    protected override async Task OnCircleEffect(PlayerChoiceContext choiceContext)
    {
        var enemies = CombatState!.HittableEnemies;
        var combatRoom = NCombatRoom.Instance;
        if (combatRoom != null && GodotObject.IsInstanceValid(combatRoom))
        {
            foreach (var enemy in enemies)
                combatRoom.CombatVfxContainer?.AddChildSafely(NFireBurstVfx.Create(enemy, 0.75f));
        }
        await CreatureCmd.Damage(choiceContext, enemies,
            DynamicVars.Damage.IntValue, ValueProp.Unpowered, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
