using UnityEngine;
using Verse;

namespace RatkinRaceStandalone
{
    public class RatkinRaceStandaloneSettings : ModSettings
    {
        public static bool useRatkinBody = true;
        public static bool allowRatkinInCoreFaction = true;
        public static bool allowRatkinInEmpireFaction = true;
        public static bool allowRatkinInBiotechFaction = true;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref useRatkinBody, "useRatkinBody", true);
            Scribe_Values.Look(ref allowRatkinInCoreFaction, "allowRatkinInCoreFaction", true);
            Scribe_Values.Look(ref allowRatkinInEmpireFaction, "allowRatkinInEmpireFaction", true);
            Scribe_Values.Look(ref allowRatkinInBiotechFaction, "allowRatkinInBiotechFaction", true);
            base.ExposeData();
        }

        public static void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();

            listingStandard.Begin(inRect);
            listingStandard.CheckboxLabeled("Use Ratkin body", ref useRatkinBody, "required restart to make change");
            listingStandard.CheckboxLabeled("Allow Ratkin to spawn in core factions", ref allowRatkinInCoreFaction, "required restart to make change");
            listingStandard.CheckboxLabeled("Allow Ratkin to spawn in empire factions", ref allowRatkinInEmpireFaction, "required restart to make change");
            listingStandard.CheckboxLabeled("Allow Ratkin to spawn in biotech factions", ref allowRatkinInBiotechFaction, "required restart to make change");
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
