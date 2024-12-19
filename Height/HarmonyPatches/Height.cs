using HarmonyLib;
using Height.Configuration;
using System;
using Unity.Mathematics;
using UnityEngine;

namespace PreciseHeight.HarmonyPatches
{
    [HarmonyPatch(typeof(PlayerHeightSettingsController), nameof(PlayerHeightSettingsController.AutoSetHeight))]
    internal class AutoSetHeight
    {
        static bool Prefix(ref PlayerHeightSettingsController __instance, ref Action<float> ___valueDidChangeEvent)
        {
            if (PluginConfig.Instance.Enabled)
            {
                __instance._value = PluginConfig.Instance.Height;
                __instance.RefreshUI();
                ___valueDidChangeEvent?.Invoke(__instance._value);

                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(RoomAdjustSettingsViewController), nameof(RoomAdjustSettingsViewController.Move))]
    internal class Move
    {
        static bool Prefix(Vector3 move, ref RoomAdjustSettingsViewController __instance)
        {
            if (PluginConfig.Instance.Enabled)
            {
                if (move.y != 0)
                {
                    move /= 5;
                }
                else
                {
                    move /= 10;
                }

                __instance._settingsManager.settings.room.center += (float3)move;
                __instance._settingsApplicator.NotifyRoomTransformOffsetWasUpdated();
                __instance.RefreshTexts();

                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(RoomAdjustSettingsViewController), nameof(RoomAdjustSettingsViewController.Rotate))]
    internal class Rotate
    {
        static bool Prefix(float rotation, ref RoomAdjustSettingsViewController __instance)
        {
            if (PluginConfig.Instance.Enabled)
            {
                rotation /= 5;
                __instance._settingsManager.settings.room.rotation = (rotation + __instance._settingsManager.settings.room.rotation) % 360f;
                __instance._settingsApplicator.NotifyRoomTransformOffsetWasUpdated();
                __instance.RefreshTexts();

                return false;
            }

            return true;
        }
    }
}
