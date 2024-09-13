using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RatkinRaceStandalone
{
    public static class QuestUtility
    {
        public static void AppendCharityInfoToLetter(TaggedString charityInfo, ref TaggedString taggedText)
        {
            if (ModsConfig.IdeologyActive)
            {
                IEnumerable<Pawn> source = IdeoUtility.AllColonistsWithCharityPrecept();
                if (source.Any())
                {
                    taggedText += "\n\n" + charityInfo + "\n\n" + "PawnsHaveCharitableBeliefs".Translate() + ":";
                    foreach (IGrouping<Ideo, Pawn> grouping in from c in source group c by c.Ideo)
                    {
                        taggedText += "\n  - " + "BelieversIn".Translate(grouping.Key.name.Colorize(grouping.Key.TextColor), grouping.Select((Pawn f) => f.NameShortColored.Resolve()).ToCommaList(false, false));
                    }
                }
            }
        }
    }
}
