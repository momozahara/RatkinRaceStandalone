using RimWorld.QuestGen;
using RimWorld;
using Verse;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RatkinRaceStandalone
{
    public class QuestNode_RefugeePodCrash_Baby : QuestNode_RefugeePodCrash
    {
        protected override void AddSpawnPawnQuestParts(Quest quest, Map map, Pawn pawn)
        {
            Slate slate = QuestGen.slate;
            List<Thing> list = new List<Thing>
            {
                pawn
            };
            if (Rand.Value < 0.5f)
            {
                Pawn mother = pawn.GetMother();
                bool flag = mother == null || Find.WorldPawns.GetSituation(mother) == WorldPawnSituation.None;
                Pawn father = pawn.GetFather();
                bool flag2 = father == null || Find.WorldPawns.GetSituation(father) == WorldPawnSituation.None;
                if (flag || flag2)
                {
                    slate.Set("hasParent", true);
                    PawnGenerationRequest request = new PawnGenerationRequest(pawn.kindDef, pawn.Faction, PawnGenerationContext.NonPlayer, -1, true, true, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, true, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
                    if (flag && !flag2)
                    {
                        request.FixedGender = new Gender?(Gender.Female);
                    }
                    else if (flag2 && !flag)
                    {
                        request.FixedGender = new Gender?(Gender.Male);
                    }
                    else if (Rand.Value < 0.5f)
                    {
                        request.FixedGender = new Gender?(Gender.Female);
                    }
                    else
                    {
                        request.FixedGender = new Gender?(Gender.Male);
                    }
                    Pawn pawn2 = PawnGenerator.GeneratePawn(request);
                    pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, pawn2);
                    list.Add(pawn2.Corpse);
                }
            }
            else
            {
                slate.Set("hasParent", false);
            }

            quest.DropPods(mapParent: map.Parent, contents: list, faction: pawn.Faction, dropAllInSamePod: true, sendStandardLetter: false);
        }

        protected override void SendLetter(Pawn pawn)
        {
            TaggedString taggedLabel = "RK_LetterLabelRefugeePodCrash".Translate();
            TaggedString taggedText = "RK_RefugeePodCrashBaby".Translate(pawn.Named("PAWN")).AdjustedFor(pawn);
            if (QuestGen.slate.Get("hasParent", false))
            {
                taggedText += "\n\n" + "RK_RefugeePodCrashBabyHasParent".Translate(pawn.Named("PAWN")).AdjustedFor(pawn);
            }
            QuestNode_WandererJoin.AppendCharityInfoToLetter("RK_JoinerCharityInfo".Translate(pawn), ref taggedText);
            PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedText, ref taggedLabel, pawn);
            Find.LetterStack.ReceiveLetter(label: taggedLabel, text: taggedText, textLetterDef: LetterDefOf.NeutralEvent, lookTargets: new TargetInfo(pawn));
        }
    }
}
