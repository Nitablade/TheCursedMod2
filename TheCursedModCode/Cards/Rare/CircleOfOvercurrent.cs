using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheCursedMod.TheCursedModCode.Cards;

/// <summary>
/// 과전류의 마법진(Circle of Overcurrent) - 마법진 : 스킬 카드를 사용할 때 마다 무작위 적에게 피해를 8 주고, 이번 전투 동안 피해량이 1 증가합니다.
/// 강화 시 피해 증가량 2.
/// </summary>
public sealed class CircleOfOvercurrent() : CircleCard(CardRarity.Rare)
{
    private decimal _extraDamageFromTriggers;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(8, ValueProp.Unpowered),
        new DynamicVar("DamageIncrement", 1m)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(TheCursedModCode.Keywords.Circle)
    ];

    protected override bool ShouldTrigger(CardPlay cardPlay) =>
        cardPlay.Card.Type == CardType.Skill;

    protected override async Task OnCircleEffect(PlayerChoiceContext choiceContext)
    {
        var target = Owner.RunState.Rng.CombatTargets.NextItem(CombatState!.HittableEnemies);
        if (target == null) return;

        VfxCmd.PlayOnCreature(target, "vfx/vfx_attack_lightning");
        SfxCmd.Play("event:/sfx/characters/defect/defect_lightning_evoke");
        await CreatureCmd.Damage(choiceContext, target, DynamicVars.Damage.IntValue, ValueProp.Unpowered, Owner.Creature, this);
        var increment = DynamicVars["DamageIncrement"].BaseValue;
        DynamicVars.Damage.BaseValue += increment;
        _extraDamageFromTriggers += increment;
    }

    protected override void AfterDowngraded()
    {
        base.AfterDowngraded();
        DynamicVars.Damage.BaseValue += _extraDamageFromTriggers;
    }

    protected override void OnUpgrade()
    {
        DynamicVars["DamageIncrement"].UpgradeValueBy(1m);
    }
}
