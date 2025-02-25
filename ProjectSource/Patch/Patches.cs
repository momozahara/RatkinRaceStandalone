﻿using RimWorld;
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

    public class RatkinPatchEarUseHairColorChannel : PatchOperationSequence
    {
        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (RatkinRaceStandaloneSettings.useHairColorChannel)
            {
                Log.Message("RatkinPatchEarUseHairColorChannel Operation Apply");
                return base.ApplyWorker(xml);
            }
            Log.Message("RatkinPatchEarUseHairColorChannel Operation Skipped");
            return true;
        }
    }

    public class RatkinPatchAllowDarkerColor : PatchOperationSequence
    {
        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (RatkinRaceStandaloneSettings.allowDarkerRatkinSkinColor)
            {
                Log.Message("RatkinPatchAllowDarkerColor Operation Apply");
                return base.ApplyWorker(xml);
            }
            Log.Message("RatkinPatchAllowDarkerColor Operation Skipped");
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
            if (ModsConfig.RoyaltyActive && RatkinRaceStandaloneSettings.allowRatkinInEmpireFaction)
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
            if (ModsConfig.BiotechActive && RatkinRaceStandaloneSettings.allowRatkinInBiotechFaction)
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

    public class RatkinPatchAllowRatkinMO : PatchOperationSequence
    {
        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (ModsConfig.IsActive(PackagesId.MedievalOverhaul) && RatkinRaceStandaloneSettings.allowRatkinMO)
            {
                Log.Message("RatkinPatchAllowRatkinMO Operation Apply");
                return base.ApplyWorker(xml);
            }
            Log.Message("RatkinPatchAllowRatkinMO Operation Skipped");
            return true;
        }
    }
}
