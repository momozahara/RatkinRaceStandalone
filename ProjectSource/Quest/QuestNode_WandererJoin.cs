using RimWorld.QuestGen;
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;

namespace RatkinRaceStandalone
{
    public class QuestNode_WandererJoin : QuestNode_WandererBase
    {
        protected override void AfterRunRunInit(Slate slate, Quest quest, PawnsArrivalModeDef arrivalMode, Pawn pawn, Map map)
        {
            string signalAccept = QuestGenUtility.HardcodedSignalWithQuestID("Accept");
            QuestGen_Signal.Signal(quest, signalAccept, delegate
            {
                QuestGen_Misc.SetFaction(quest, Gen.YieldSingle(pawn), Faction.OfPlayer, null);
                QuestGen_Misc.PawnsArrive(quest, Gen.YieldSingle(pawn), null, map.Parent, arrivalMode, false, null, null, null, null, null, false, false, true);
                QuestGen_End.End(quest, QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
            }, null, QuestPart.SignalListenMode.OngoingOnly);

            string signalReject = QuestGenUtility.HardcodedSignalWithQuestID("Reject");
            QuestGen_Signal.Signal(quest, signalReject, delegate
            {
                QuestGen_Misc.GiveDiedOrDownedThoughts(quest, pawn, PawnDiedOrDownedThoughtsKind.DeniedJoining, null);
                QuestGen_End.End(quest, QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
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

            QuestUtility.AppendCharityInfoToLetter("JoinerCharityInfo".Translate(pawn), ref taggedText);

            PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedText, ref taggedLabel, pawn);
            ChoiceLetter_AcceptJoiner letter = (ChoiceLetter_AcceptJoiner)LetterMaker.MakeLetter(taggedLabel, taggedText, LetterDefOf.AcceptJoiner, null, quest);
            letter.signalAccept = signalAccept;
            letter.signalReject = signalReject;
            letter.StartTimeout(6000);

            Find.LetterStack.ReceiveLetter(letter);
        }

        [NoTranslate]
        public SlateRef<string> labelTag;

        [NoTranslate]
        public SlateRef<string> textTag;
    }
}
