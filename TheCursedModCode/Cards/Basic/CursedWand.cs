using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;
using TheCursedMod.TheCursedModCode;

namespace TheCursedMod.TheCursedModCode.Cards;

/// <summary>
/// 저주받은 마법봉(Cursed Wand) - 방어도를 4 얻습니다. 피해를 8 줍니다. 찌꺼기를 얻습니다. (강화 시 방어도 6, 피해 10)
/// </summary>
public sealed class CursedWand() : TheCursedModCard(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<Dregs>(false)
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(4, ValueProp.Move),
        new DamageVar(8, ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        await Dregs.CreateAndAddToHand(Owner, 1);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2m);
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
