using HarmonyLib;
using Height.Configuration;
using System;
using UnityEngine;

namespace PreciseHeight.HarmonyPatches
{
    [HarmonyPatch(typeof(PlayerHeightSettingsController), nameof(PlayerHeightSettingsController.AutoSetHeight))]
    internal class AutoSetHeight
    {
        static bool Prefix(ref float ____value, ref Vector3SO ____roomCenter, ref PlayerHeightSettingsController __instance, ref Action<float> ___valueDidChangeEvent)
        {
            if (PluginConfig.Instance.Enabled)
            {
                ____value = PluginConfig.Instance.Height;
                __instance.RefreshUI();
                ___valueDidChangeEvent?.Invoke(____value);

                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(RoomAdjustSettingsViewController), nameof(RoomAdjustSettingsViewController.Move))]
    internal class Move
    {
        static bool Prefix(Vector3 move, ref Vector3SO ____roomCenter, ref RoomAdjustSettingsViewController __instance)
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

                ____roomCenter.value += move;
                __instance.RefreshTexts();

                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(RoomAdjustSettingsViewController), nameof(RoomAdjustSettingsViewController.Rotate))]
    internal class Rotate
    {
        static bool Prefix(float rotation, ref FloatSO ____roomRotation, ref RoomAdjustSettingsViewController __instance)
        {
            if (PluginConfig.Instance.Enabled)
            {
                rotation /= 5;
                ____roomRotation.value = (rotation + ____roomRotation.value) % 360f;
                __instance.RefreshTexts();

                return false;
            }

            return true;
        }
    }
}
