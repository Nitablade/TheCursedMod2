using BaseLib.Abstracts;
using TheCursedMod.TheCursedModCode.Extensions;
using TheCursedMod.TheCursedModCode.Cards;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using TheCursedMod.TheCursedModCode.Relics;

namespace TheCursedMod.TheCursedModCode.Character;

public class TheCursedMod : PlaceholderCharacterModel
{
    public const string CharacterId = "CharMod";
    
    public static readonly Color Color = new("5757d4");

    public override string CustomEnergyCounterPath =>
        "res://TheCursedMod/scenes/the_cursed_energy_counter.tscn";

    public override Color NameColor => Color;
    public override Color MapDrawingColor => Color;
    public override CharacterGender Gender => CharacterGender.Masculine;
    public override int StartingHp => 75;
    
    public override IEnumerable<CardModel> StartingDeck => [
        ModelDb.Card<StrikeTheCursed>(),
        ModelDb.Card<StrikeTheCursed>(),
        ModelDb.Card<StrikeTheCursed>(),
        ModelDb.Card<StrikeTheCursed>(),
        ModelDb.Card<DefendTheCursed>(),
        ModelDb.Card<DefendTheCursed>(),
        ModelDb.Card<DefendTheCursed>(),
        ModelDb.Card<DefendTheCursed>(),
        ModelDb.Card<CursedWand>(),
        ModelDb.Card<CleanUpWorkshop>(),
        ModelDb.Card<Dregs>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<BlackMagic101Relic>()
    ];
    
    public override CardPoolModel CardPool => ModelDb.CardPool<TheCursedModCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<TheCursedModRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<TheCursedModPotionPool>();
    
    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets. 
        These are just some of the simplest assets, given some placeholders to differentiate your character with. 
        You don't have to, but you're suggested to rename these images. */
    public override string CustomTrailPath => "res://TheCursedMod/scenes/card_trail_the_cursed.tscn";
    public override string CustomVisualPath => "res://TheCursedMod/scenes/the_cursed_visual.tscn";
    public override string CustomRestSiteAnimPath => "res://TheCursedMod/scenes/the_cursed_rest_site.tscn";
    public override string CustomMerchantAnimPath => "res://TheCursedMod/scenes/the_cursed_merchant.tscn";
    public override string CustomCharacterSelectBg => "char_select_bg_the_cursed.tscn".ScenePath();
    public override string CustomIconTexturePath => $"res://{TheCursedModMainFile.ModId}/images/ui/top_panel/character_icon_the_cursed.png";
    public override string CustomIconPath => "res://TheCursedMod/scenes/the_cursed_icon.tscn";
    public override string CustomCharacterSelectIconPath => "char_select_the_cursed.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_the_cursed_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_the_cursed.png".CharacterUiPath();

    public override string CustomArmPointingTexturePath =>
        $"res://{TheCursedModMainFile.ModId}/images/thecursed/hands/multiplayer_hand_the_cursed_point.png";
    public override string CustomArmRockTexturePath =>
        $"res://{TheCursedModMainFile.ModId}/images/thecursed/hands/multiplayer_hand_the_cursed_rock.png";
    public override string CustomArmPaperTexturePath =>
        $"res://{TheCursedModMainFile.ModId}/images/thecursed/hands/multiplayer_hand_the_cursed_paper.png";
    public override string CustomArmScissorsTexturePath =>
        $"res://{TheCursedModMainFile.ModId}/images/thecursed/hands/multiplayer_hand_the_cursed_scissor.png";
}