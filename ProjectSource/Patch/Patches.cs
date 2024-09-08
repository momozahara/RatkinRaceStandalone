using System.Xml;
using Verse;

namespace RatkinRaceStandalone.Patch
{
    public class RatkinPatchUseRatkinBodies : PatchOperationSequence
    {
        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (RatkinRaceStandaloneSettings.useRatkinBody)
            {
                Log.Message("RatkinPatchUseRatkinBodies Operation Apply");
                return base.ApplyWorker(xml);
            }
            Log.Message("RatkinPatchUseRatkinBodies Operation Skipped");
            return true;
        }
    }

    public class RatkinPatchAllowInCoreFactions : PatchOperationSequence
    {
        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (RatkinRaceStandaloneSettings.allowRatkinInCoreFaction)
            {
                Log.Message("RatkinPatchAllowInCoreFactions Operation Apply");
                return base.ApplyWorker(xml);
            }
            Log.Message("RatkinPatchAllowInCoreFactions Operation Skipped");
            return true;
        }
    }

    public class RatkinPatchAllowInEmpireFactions : PatchOperationSequence
    {
        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (RatkinRaceStandaloneSettings.allowRatkinInEmpireFaction)
            {
                Log.Message("RatkinPatchAllowInEmpireFactions Operation Apply");
                return base.ApplyWorker(xml);
            }
            Log.Message("RatkinPatchAllowInEmpireFactions Operation Skipped");
            return true;
        }
    }

    public class RatkinPatchAllowInBiotechFactions : PatchOperationSequence
    {
        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (RatkinRaceStandaloneSettings.allowRatkinInBiotechFaction)
            {
                Log.Message("RatkinPatchAllowInBiotechFactions Operation Apply");
                return base.ApplyWorker(xml);
            }
            Log.Message("RatkinPatchAllowInBiotechFactions Operation Skipped");
            return true;
        }
    }

    public class RatkinPatchAllowRatkinSlave : PatchOperationSequence
    {
        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (RatkinRaceStandaloneSettings.allowRatkinSlave)
            {
                Log.Message("RatkinPatchAllowRatkinSlave Operation Apply");
                return base.ApplyWorker(xml);
            }
            Log.Message("RatkinPatchAllowRatkinSlave Operation Skipped");
            return true;
        }
    }
}
