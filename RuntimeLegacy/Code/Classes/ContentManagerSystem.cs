﻿using Moonstorm.Components;
using R2API;
using RoR2;
using System;
using UnityEngine;

namespace Moonstorm
{
    internal static class ContentManagerSystem
    {
        [SystemInitializer(new Type[] { typeof(BodyCatalog), typeof(ItemCatalog), typeof(EquipmentCatalog), typeof(BuffCatalog) })]
        private static void SystemInit()
        {
            MSULog.Info($"Initializing Content Manager System...");
            for (int i = 0; i < BodyCatalog.bodyPrefabs.Length; i++)
            {
                try
                {
                    GameObject bodyPrefab = BodyCatalog.bodyPrefabs[i];

                    var manager = bodyPrefab.AddComponent<MoonstormContentManager>();
                    manager.body = bodyPrefab.GetComponent<CharacterBody>();

                    var modelLocator = bodyPrefab.GetComponent<ModelLocator>();
                    if (!modelLocator)
                        continue;
                    if (!modelLocator.modelTransform)
                        continue;
                    if (!modelLocator.modelTransform.GetComponent<CharacterModel>())
                        continue;

                    var eliteBehavior = bodyPrefab.AddComponent<MoonstormEliteBehavior>();
                    eliteBehavior.characterModel = modelLocator.modelTransform.GetComponent<CharacterModel>();
                    eliteBehavior.body = bodyPrefab.GetComponent<CharacterBody>();
                    manager.eliteBehavior = eliteBehavior;

                    BodyCatalog.bodyPrefabs[i] = bodyPrefab;
                }
                catch (Exception ex)
                {
                    MSULog.Error(ex);
                }
            }
            CharacterBody.onBodyStartGlobal += OnBodyStart;

            if (MSUtil.HolyDLLInstalled)
            {
#if DEBUG
                MSULog.Info("Holy installed, using custom RecalculateStats support");
#endif
                On.RoR2.CharacterBody.RecalculateStats += HolyfiedRecalculateStats;
                R2API.RecalculateStatsAPI.GetStatCoefficients += HolyfiedGetStatCoefficients;
            }
            else
            {
                On.RoR2.CharacterBody.RecalculateStats += OnRecaluclateStats;
                R2API.RecalculateStatsAPI.GetStatCoefficients += OnGetStatCoefficients;
            }
        }

        //Has master needs to be set here because we cant guarantee a body has a singular master (A body can have multiple possible masters)
        private static void OnBodyStart(CharacterBody body)
        {
            var manager = body.GetComponent<MoonstormContentManager>();
            if (!manager)
                return;

            if (body.master)
            {
                manager.hasMaster = true;
            }

            manager.StartGetInterfaces();
        }

        private static void OnRecaluclateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            var manager = self.GetComponent<MoonstormContentManager>();
            manager.AsValidOrNull()?.RunStatRecalculationsStart();
            orig(self);
            manager.AsValidOrNull()?.RunStatRecalculationsEnd();
        }

        private static void HolyfiedRecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            var manager = self.GetComponent<MoonstormContentManager>();
            manager.AsValidOrNull()?.RunStatRecalculationsEnd();
        }

        private static void OnGetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            var manager = sender.GetComponent<MoonstormContentManager>();
            manager.AsValidOrNull()?.RunStatHookEventModifiers(args);
        }

        private static void HolyfiedGetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs
             args)
        {
            var manager = sender.GetComponent<MoonstormContentManager>();

            manager.AsValidOrNull()?.RunStatRecalculationsStart();
            manager.AsValidOrNull()?.RunStatHookEventModifiers(args);
        }
    }
}
