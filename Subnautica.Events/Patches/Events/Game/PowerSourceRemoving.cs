﻿namespace Subnautica.Events.Patches.Events.Game
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::PowerRelay), nameof(global::PowerRelay.RemoveInboundPower))]
    public class PowerSourceRemoving
    {
        private static void Prefix(global::PowerRelay __instance, IPowerInterface powerInterface)
        {
            if (Network.IsMultiplayerActive)
            {
                if (__instance.inboundPowerSources.Contains(powerInterface))
                {
                    try
                    {
                        PowerSourceRemovingEventArgs args = new PowerSourceRemovingEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject, false), powerInterface);

                        Handlers.Game.OnPowerSourceRemoving(args);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"PowerSourceRemoving.Prefix: {e}\n{e.StackTrace}");
                    }
                }
            }
        }
    }
}