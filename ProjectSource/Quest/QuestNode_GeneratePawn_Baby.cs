using RimWorld.Planet;
using RimWorld;
using RimWorld.QuestGen;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RatkinRaceStandalone
{
    public class QuestNode_GeneratePawn_Baby : QuestNode
    {
        protected override bool TestRunInt(Slate slate)
        {
            return Find.Storyteller.difficulty.ChildrenAllowed;
        }

        // TODO: I'll clean this later beacuse too tired right now.
        protected override void RunInt()
        {
            Slate slate = QuestGen.slate;
            Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kindDef.GetValue(slate), null, PawnGenerationContext.NonPlayer, -1, true, false, true, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, true, false, false, -1, 0, false)
            {
                AllowedDevelopmentalStages = DevelopmentalStage.Baby
            });
            pawn.ageTracker.AgeChronologicalTicks = pawn.ageTracker.AgeBiologicalTicks;
            slate.Set(storeAs.GetValue(slate), pawn, false);
        }

        public SlateRef<string> storeAs;

        public SlateRef<PawnKindDef> kindDef;
    }
}
