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
            quest.DropPods(mapParent: map.Parent, contents: Gen.YieldSingle(pawn), faction: pawn.Faction, sendStandardLetter: false);
        }

        protected override void AfterRunRunInit(Slate slate, Quest quest, PawnsArrivalModeDef arrivalMode, Pawn pawn, Map map)
        {
            HealthUtility.DamageUntilDowned(p: pawn, allowBleedingWounds: false);
            HealthUtility.DamageLegsUntilIncapableOfMoving(p: pawn, allowBleedingWounds: false);
            AddSpawnPawnQuestParts(quest, map, pawn);
            slate.Set("pawn", pawn);

            SendLetter(pawn);
            
            string pawnKilled = QuestGenUtility.HardcodedSignalWithQuestID("pawn.Killed");
            string pawnTended = QuestGenUtility.HardcodedSignalWithQuestID("pawn.PlayerTended");
            string pawnLeftMap = QuestGenUtility.HardcodedSignalWithQuestID("pawn.LeftMap");
            string pawnRecruited = QuestGenUtility.HardcodedSignalWithQuestID("pawn.Recruited");

            quest.End(outcome: QuestEndOutcome.Success, inSignal: pawnTended, sendStandardLetter: false, playSound: false);

            quest.Signal(action: delegate
            {
                Quest _quest = quest;
                Action action = delegate
                {
                    Quest __quest = _quest;
                    Action _action = delegate
                    {
                        _quest.Message(message: "MessageCharityEventRefused".Translate() + ": " + "MessageWandererLeftToDie".Translate(pawn),
                            messageType: MessageTypeDefOf.NegativeEvent, lookTargets: pawn);

                    };

                    __quest.AnyColonistWithCharityPrecept(action: _action);
                    _quest.End(outcome: QuestEndOutcome.Fail, sendStandardLetter: false, playSound: false);
                };

                Action elseAction = delegate
                {
                    _quest.End(outcome: QuestEndOutcome.Fail, sendStandardLetter: false, playSound: false);
                };

                int allowKilledBeforeTicks = AllowKilledBeforeTicks;
                _quest.AcceptedAfterTicks(action: action, elseAction: elseAction, ticks: allowKilledBeforeTicks);
            }, inSignal: pawnKilled);

            quest.AnyColonistWithCharityPrecept(action: delegate
            {
                quest.Message(message: "MessageCharityEventFulfilled".Translate() + ": " + "MessageWandererRecruited".Translate(pawn),
                    messageType: MessageTypeDefOf.PositiveEvent, lookTargets: pawn);

            }, inSignal: pawnRecruited);
            quest.End(outcome: QuestEndOutcome.Success, inSignal: pawnRecruited, sendStandardLetter: false, playSound: false);

            quest.Signal(action: delegate
            {
                Action action = delegate
                {
                    quest.End(outcome: QuestEndOutcome.Fail, sendStandardLetter: false, playSound: false);
                };

                Action elseAction = delegate
                {
                    Quest _quest = quest;
                    Action _action = delegate
                    {
                        quest.Message(message: "MessageCharityEventFulfilled".Translate() + ": " + "MessageWandererLeftHealthy".Translate(pawn),
                            messageType: MessageTypeDefOf.PositiveEvent, lookTargets: pawn);

                    };
                    _quest.AnyColonistWithCharityPrecept(action: _action);
                    quest.End(outcome: QuestEndOutcome.Fail, sendStandardLetter: false, playSound: false);
                };

                quest.AnyPawnUnhealthy(action: action, elseAction: elseAction, pawns: Gen.YieldSingle<Pawn>(pawn));
            }, inSignal: pawnLeftMap);
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

            QuestNode_WandererJoin.AppendCharityInfoToLetter("JoinerCharityInfo".Translate(pawn), ref taggedText);
            PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedText, ref taggedLabel, pawn);
            Find.LetterStack.ReceiveLetter(label: taggedLabel, text: taggedText, textLetterDef: LetterDefOf.NeutralEvent, lookTargets: new TargetInfo(pawn));
        }

        private int AllowKilledBeforeTicks = 15000;
    }
}