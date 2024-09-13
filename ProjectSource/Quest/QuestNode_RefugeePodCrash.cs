using RimWorld.QuestGen;
using RimWorld;
using Verse;
using System;

namespace RatkinRaceStandalone
{
    public class QuestNode_RefugeePodCrash : QuestNode_WandererBase
    {
        protected virtual void AddSpawnPawnQuestParts(Quest quest, Map map, Pawn pawn)
        {
            QuestGen_Misc.DropPods(quest, map.Parent, Gen.YieldSingle(pawn), null, null, null, null, false, false, false, false, null, null, QuestPart.SignalListenMode.OngoingOnly, null, true, true, false, pawn.Faction);
        }

        protected override void AfterRunRunInit(Slate slate, Quest quest, PawnsArrivalModeDef arrivalMode, Pawn pawn, Map map)
        {
            HealthUtility.DamageUntilDowned(pawn, false, null, null, null);
            HealthUtility.DamageLegsUntilIncapableOfMoving(pawn, false);
            AddSpawnPawnQuestParts(quest, map, pawn);
            slate.Set("pawn", pawn);

            SendLetter(pawn);
            
            string pawnKilled = QuestGenUtility.HardcodedSignalWithQuestID("pawn.Killed");
            string pawnTended = QuestGenUtility.HardcodedSignalWithQuestID("pawn.PlayerTended");
            string pawnLeftMap = QuestGenUtility.HardcodedSignalWithQuestID("pawn.LeftMap");
            string pawnRecruited = QuestGenUtility.HardcodedSignalWithQuestID("pawn.Recruited");

            QuestGen_End.End(quest, QuestEndOutcome.Success, 0, null, pawnTended, QuestPart.SignalListenMode.OngoingOnly, false, false);

            QuestGen_Signal.Signal(quest, pawnKilled, delegate
            {
                Quest _quest = quest;
                Action action = delegate
                {
                    Quest __quest = _quest;
                    Action _action = delegate
                    {
                        QuestGen_Misc.Message(_quest, "MessageCharityEventRefused".Translate() + ": " + "MessageWandererLeftToDie".Translate(pawn),
                            MessageTypeDefOf.NegativeEvent, false, null, pawn);

                    };

                    QuestGen_Filter.AnyColonistWithCharityPrecept(__quest, action: _action);
                    QuestGen_End.End(_quest, QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
                };

                Action elseAction = delegate
                {
                    QuestGen_End.End(_quest, QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
                };

                int allowKilledBeforeTicks = AllowKilledBeforeTicks;
                QuestGen_Filter.AcceptedAfterTicks(_quest, allowKilledBeforeTicks, action, elseAction, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
            }, null, QuestPart.SignalListenMode.OngoingOnly);

            QuestGen_Filter.AnyColonistWithCharityPrecept(quest, delegate
            {
                QuestGen_Misc.Message(quest, "MessageCharityEventFulfilled".Translate() + ": " + "MessageWandererRecruited".Translate(pawn),
                    MessageTypeDefOf.PositiveEvent, false, null, pawn);

            }, null, pawnRecruited, null, null, QuestPart.SignalListenMode.OngoingOnly);
            QuestGen_End.End(quest, QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);

            QuestGen_Signal.Signal(quest, pawnLeftMap, delegate
            {
                Action action = delegate
                {
                    QuestGen_End.End(quest, QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
                };

                Action elseAction = delegate
                {
                    Quest _quest = quest;
                    Action _action = delegate
                    {
                        QuestGen_Misc.Message(quest, "MessageCharityEventFulfilled".Translate() + ": " + "MessageWandererLeftHealthy".Translate(pawn),
                            MessageTypeDefOf.PositiveEvent, false, null, pawn);

                    };
                    QuestGen_Filter.AnyColonistWithCharityPrecept(_quest, _action, null, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
                    QuestGen_End.End(quest, QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
                };

                QuestGen_Filter.AnyPawnUnhealthy(quest, Gen.YieldSingle(pawn), action, elseAction, null, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
            }, null, QuestPart.SignalListenMode.OngoingOnly);
        }

        protected virtual void SendLetter(Pawn pawn)
        {
            TaggedString taggedLabel = "RK_LetterLabelRefugeePodCrash".Translate();
            TaggedString taggedText = "RK_RefugeePodCrash".Translate(pawn.Named("PAWN")).AdjustedFor(pawn);
            taggedText += "\n\n";
            if (pawn.Faction == null)
            {
                taggedText += "RK_RefugeePodCrash_Factionless".Translate(pawn.Named("PAWN")).AdjustedFor(pawn);
            }
            else if (pawn.Faction.HostileTo(Faction.OfPlayer))
            {
                taggedText += "RK_RefugeePodCrash_Hostile".Translate(pawn.Named("PAWN")).AdjustedFor(pawn);
            }
            else
            {
                taggedText += "RK_RefugeePodCrash_NonHostile".Translate(pawn.Named("PAWN")).AdjustedFor(pawn);
            }
            if (pawn.DevelopmentalStage.Juvenile())
            {
                string arg = (pawn.ageTracker.AgeBiologicalYears * 3600000).ToStringTicksToPeriod();
                taggedText += "\n\n" + "RK_RefugeePodCrash_Child".Translate(pawn.Named("PAWN"), arg.Named("AGE"));
            }

            QuestUtility.AppendCharityInfoToLetter("JoinerCharityInfo".Translate(pawn), ref taggedText);
            PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedText, ref taggedLabel, pawn);
            Find.LetterStack.ReceiveLetter(taggedLabel, taggedText, LetterDefOf.NeutralEvent, new TargetInfo(pawn), null, null, null, null, 0, true);
        }

        private int AllowKilledBeforeTicks = 15000;
    }
}