using RimWorld.QuestGen;
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;
using RuntimeAudioClipLoader;
using UnityEngine;

namespace RatkinRaceStandalone
{
    public abstract class QuestNode_WandererBase : QuestNode
    {
        protected abstract void AfterRunRunInit(Slate slate, Quest quest, PawnsArrivalModeDef arrivalMode, Pawn pawn, Map map);

        protected override void RunInt()
        {
            Slate slate = QuestGen.slate;
            Quest quest = QuestGen.quest;
            PawnsArrivalModeDef arrivalMode = this.arrivalMode.GetValue(slate) ?? PawnsArrivalModeDefOf.EdgeWalkIn;
            Pawn pawn = this.pawn.GetValue(slate);
            Faction faction = GenPawnFaction(slate);
            if (pawn.Faction != faction)
            {
                pawn.SetFaction(faction);
            }
            pawn.guest.Recruitable = true;
            if (!slate.TryGet("map", out Map map, false))
            {
                map = QuestGen_Get.GetMap(false, null);
            }

            AfterRunRunInit(slate, quest, arrivalMode, pawn, map);
        }

        protected virtual Faction GenPawnFaction(Slate slate)
        {
            if (Rand.Chance(0.6f))
            {
                return null;
            }

            FactionManager manager = Find.FactionManager;
            IEnumerable<string> allowFactionDefs = this.allowFactionDefs.GetValue(slate);
            if (allowFactionDefs.EnumerableNullOrEmpty())
            {
                allowFactionDefs = new List<string>
                    {
                        // In my opinion tribes are not supposed to be in transport pod

                        // Core
                        "OutlanderCivil",
                        "OutlanderRough",
                        "Pirate",

                        // Ideology

                        // Royalty
                        "OutlanderRefugee",
                        "Empire",

                        // Biotech
                        "PirateWaster"
                    };
            }

            List<Faction> factions = new List<Faction>();
            foreach (Faction faction in manager.GetFactions(allowNonHumanlike: false))
            {
                if (allowFactionDefs.Contains(faction.def.defName))
                {
                    factions.Add(faction);
                }

            }

            factions.Shuffle();
            return factions.First();
        }

        protected override bool TestRunInt(Slate slate)
        {
            return true;
        }

        public SlateRef<Pawn> pawn;

        public SlateRef<PawnsArrivalModeDef> arrivalMode;

        public SlateRef<IEnumerable<string>> allowFactionDefs;
    }
}
