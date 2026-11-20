using System;
using MelonLoader;

[assembly: MelonInfo(typeof(CART3R.WeaponOverheat.WeaponOverheatMod), "WeaponOverheat", "1.0.3", "CART3R")]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]

namespace CART3R.WeaponOverheat
{
    public class WeaponOverheatMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("================================");
            MelonLogger.Msg("CART3R WEAPON OVERHEAT");
            MelonLogger.Msg("BONELAB QUEST PATCH 6");
            MelonLogger.Msg("MOD INITIALIZED SUCCESSFULLY");
            MelonLogger.Msg("================================");
        }

        public override void OnUpdate()
        {
            // Overheating effects will be added after
            // the mod successfully initializes.
        }
    }
}
