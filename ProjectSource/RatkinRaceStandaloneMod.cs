using UnityEngine;
using Verse;

namespace RatkinRaceStandalone
{
    public class RatkinRaceStandaloneSettings : ModSettings
    {
        public static bool useRatkinBody;
        public static bool useHairColorChannel;
        public static bool allowDarkerRatkinSkinColor;
        public static bool allowRatkinInCoreFaction;
        public static bool allowRatkinInEmpireFaction;
        public static bool allowRatkinInBiotechFaction;
        public static bool allowRatkinSlave;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref useRatkinBody, "useRatkinBody", true);
            Scribe_Values.Look(ref useHairColorChannel, "useHairColorChannel", false);
            Scribe_Values.Look(ref allowDarkerRatkinSkinColor, "allowDarkerRatkinSkinColor", true);
            Scribe_Values.Look(ref allowRatkinInCoreFaction, "allowRatkinInCoreFaction", true);
            Scribe_Values.Look(ref allowRatkinInEmpireFaction, "allowRatkinInEmpireFaction", true);
            Scribe_Values.Look(ref allowRatkinInBiotechFaction, "allowRatkinInBiotechFaction", true);
            Scribe_Values.Look(ref allowRatkinSlave, "allowRatkinSlave", true);
            base.ExposeData();
        }

        public static void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();

            listingStandard.Begin(inRect);
            listingStandard.CheckboxLabeled("Use Ratkin body", ref useRatkinBody, "required restart to make change");
            listingStandard.CheckboxLabeled("Make ears color sync with hair", ref useHairColorChannel, "required restart to make change");
            listingStandard.CheckboxLabeled("Allow ratkin with darker skin color", ref allowDarkerRatkinSkinColor, "required restart to make change");
            listingStandard.CheckboxLabeled("Allow Ratkin to spawn in core factions", ref allowRatkinInCoreFaction, "required restart to make change");
            listingStandard.CheckboxLabeled("Allow Ratkin to spawn in empire factions", ref allowRatkinInEmpireFaction, "required restart to make change");
            listingStandard.CheckboxLabeled("Allow Ratkin to spawn in biotech factions", ref allowRatkinInBiotechFaction, "required restart to make change");
            listingStandard.CheckboxLabeled("Allow Ratkin to spawn as slave", ref allowRatkinSlave, "required restart to make change");
            listingStandard.End();
        }
    }

    public class RatkinRaceStandaloneMod : Mod
    {
        RatkinRaceStandaloneSettings settings;

        public RatkinRaceStandaloneMod(ModContentPack content) : base(content)
        {
            Log.Message("Ratkin Race Standalone loaded");

            this.settings = GetSettings<RatkinRaceStandaloneSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            RatkinRaceStandaloneSettings.DoSettingsWindowContents(inRect);
        }
        public override string SettingsCategory()
        {
            return "Ratkin Race Standalone";
        }
    }
}
