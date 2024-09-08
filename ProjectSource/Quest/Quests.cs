using RimWorld.QuestGen;
using RimWorld;
using Verse;

namespace RatkinRaceStandalone
{
    public class QuestNode_WandererJoin : QuestNode
    {
        protected override void RunInt()
        {
            Slate slate = QuestGen.slate;
            Quest quest = QuestGen.quest;
            PawnsArrivalModeDef arrivealMode = arrivalMode.GetValue(slate) ?? PawnsArrivalModeDefOf.EdgeWalkIn;
            Pawn pawn = this.pawn.GetValue(slate);
            if (!slate.TryGet("map", out Map map, false))
            {
                map = QuestGen_Get.GetMap(false, null);
            }

            string signalAccept = QuestGenUtility.HardcodedSignalWithQuestID("Accept");
            quest.Signal(signalAccept, delegate
            {
                quest.SetFaction(Gen.YieldSingle(pawn), Faction.OfPlayer);
                quest.PawnsArrive(pawns: Gen.YieldSingle(pawn), mapParent: map.Parent, arrivalMode: arrivealMode, joinPlayer: true, sendStandardLetter: true);
                quest.End(outcome: QuestEndOutcome.Success,signalListenMode: QuestPart.SignalListenMode.OngoingOnly);
            }, null, QuestPart.SignalListenMode.OngoingOnly);

            string signalReject = QuestGenUtility.HardcodedSignalWithQuestID("Reject");
            quest.Signal(signalReject, delegate
            {
                quest.GiveDiedOrDownedThoughts(aboutPawn: pawn,thoughtsKind: PawnDiedOrDownedThoughtsKind.DeniedJoining);
                quest.End(outcome: QuestEndOutcome.Fail, signalListenMode: QuestPart.SignalListenMode.OngoingOnly);
            }, null, QuestPart.SignalListenMode.OngoingOnly);

            TaggedString taggedLabel = "label";
            if (labelTag.TryGetValue(slate, out string label) && !label.NullOrEmpty())
            {
                taggedLabel = label.Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
            }

            TaggedString taggedText = "text";
            if (textTag.TryGetValue(slate, out string text) && !text.NullOrEmpty())
            {
                taggedText = text.Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
            }

            PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedText, ref taggedLabel, pawn);
            ChoiceLetter_AcceptJoiner letter = (ChoiceLetter_AcceptJoiner)LetterMaker.MakeLetter(taggedLabel, taggedText, LetterDefOf.AcceptJoiner, quest: quest);
            letter.signalAccept = signalAccept;
            letter.signalReject = signalReject;
            letter.StartTimeout(6000);

            Find.LetterStack.ReceiveLetter(letter);
        }

        protected override bool TestRunInt(Slate slate)
        {
            return true;
        }

        [NoTranslate]
        public SlateRef<string> labelTag;

        [NoTranslate]
        public SlateRef<string> textTag;

        public SlateRef<Pawn> pawn;

        public SlateRef<PawnsArrivalModeDef> arrivalMode;
    }
}
