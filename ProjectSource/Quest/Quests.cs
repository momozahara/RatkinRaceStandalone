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
            PawnsArrivalModeDef arrivealMode = this.arrivalMode.GetValue(slate) ?? PawnsArrivalModeDefOf.EdgeWalkIn;
            Pawn pawn = this.pawn.GetValue(slate);
            if (!slate.TryGet<Map>("map", out Map map, false))
            {
                map = QuestGen_Get.GetMap(false, null);
            }

            string signalAccept = QuestGenUtility.HardcodedSignalWithQuestID("Accept");
            quest.Signal(signalAccept, delegate
            {
                quest.SetFaction(Gen.YieldSingle<Pawn>(pawn), Faction.OfPlayer, null);
                quest.PawnsArrive(Gen.YieldSingle<Pawn>(pawn), null, map.Parent, arrivealMode, true, null, null, null, null, null, false, false, true);
                quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
            }, null, QuestPart.SignalListenMode.OngoingOnly);

            string signalReject = QuestGenUtility.HardcodedSignalWithQuestID("Reject");
            quest.Signal(signalReject, delegate
            {
                quest.GiveDiedOrDownedThoughts(pawn, PawnDiedOrDownedThoughtsKind.DeniedJoining, null);
                quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
            }, null, QuestPart.SignalListenMode.OngoingOnly);

            TaggedString taggedLabel = "label";
            if (this.labelTag.TryGetValue(slate, out string label) && !label.NullOrEmpty())
            {
                taggedLabel = label.Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
            }

            TaggedString taggedText = "text";
            if (this.textTag.TryGetValue(slate, out string text) && !text.NullOrEmpty())
            {
                taggedText = text.Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
            }

            PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedText, ref taggedLabel, pawn);
            ChoiceLetter_AcceptJoiner letter = (ChoiceLetter_AcceptJoiner)LetterMaker.MakeLetter(taggedLabel, taggedText, LetterDefOf.AcceptJoiner, null, quest);
            letter.signalAccept = signalAccept;
            letter.signalReject = signalReject;
            letter.StartTimeout(6000);

            Find.LetterStack.ReceiveLetter(letter, null, 0, true);
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
