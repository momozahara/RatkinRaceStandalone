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
            quest.Signal(signalAccept, delegate
            {
                quest.SetFaction(Gen.YieldSingle(pawn), Faction.OfPlayer);
                quest.PawnsArrive(pawns: Gen.YieldSingle(pawn), mapParent: map.Parent, arrivalMode: arrivalMode);
                quest.End(outcome: QuestEndOutcome.Success, sendStandardLetter: false, playSound: false);
            });

            string signalReject = QuestGenUtility.HardcodedSignalWithQuestID("Reject");
            quest.Signal(signalReject, delegate
            {
                quest.GiveDiedOrDownedThoughts(aboutPawn: pawn, thoughtsKind: PawnDiedOrDownedThoughtsKind.DeniedJoining);
                quest.End(outcome: QuestEndOutcome.Fail, sendStandardLetter: false, playSound: false);
            });

            TaggedString taggedLabel = "label";
            if (labelTag.TryGetValue(slate, out string label) && !label.NullOrEmpty())
            {
                taggedLabel = label.Translate(pawn.Named("PAWN")).AdjustedFor(pawn);
            }

            TaggedString taggedText = "text";
            if (textTag.TryGetValue(slate, out string text) && !text.NullOrEmpty())
            {
                taggedText = text.Translate(pawn.Named("PAWN")).AdjustedFor(pawn);
            }

            if (ModsConfig.IdeologyActive)
            {
                IEnumerable<Pawn> source = IdeoUtility.AllColonistsWithCharityPrecept();
                if (source.Any<Pawn>())
                {
                    taggedText += "\n\n" + "JoinerCharityInfo".Translate(pawn) + "\n\n" + "PawnsHaveCharitableBeliefs".Translate() + ":";
                    foreach (IGrouping<Ideo, Pawn> grouping in from c in source
                                                               group c by c.Ideo)
                    {
                        taggedText += "\n  - " + "BelieversIn".Translate(grouping.Key.name.Colorize(grouping.Key.TextColor), grouping.Select((Pawn f) => f.NameShortColored.Resolve()).ToCommaList(false, false));
                    }
                }
            }

            PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedText, ref taggedLabel, pawn);
            ChoiceLetter_AcceptJoiner letter = (ChoiceLetter_AcceptJoiner)LetterMaker.MakeLetter(taggedLabel, taggedText, LetterDefOf.AcceptJoiner, quest: quest);
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
