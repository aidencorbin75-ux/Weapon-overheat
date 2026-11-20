using MelonLoader;
using System;

namespace CART3R.WeaponOverheat
{
    public class WeaponOverheatMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("CART3R Weapon Overheat Loaded!");
            MelonLogger.Msg("BONELAB Quest Patch 6");
            MelonLogger.Msg("[WOH] Initialization successful!");
        }
    }
}
