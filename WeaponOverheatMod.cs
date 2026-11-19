\
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MelonLoader;

namespace CART3R.WeaponOverheat
{
    // Patch-6 runtime reconnaissance build. It deliberately does not claim to
    // jam or explode guns until the live firearm firing API is verified.
    public sealed class WeaponOverheatMod : MelonMod
    {
        private bool _scanned;
        private DateTime _scanAt;

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("==============================================");
            MelonLogger.Msg("CART3R Weapon Overheat - Quest Patch 6 probe");
            MelonLogger.Msg("Planned: barrel orange/red heat, firing jam, critical overheat explosion.");
            MelonLogger.Warning("This build discovers live firearm APIs only; gameplay effects are not enabled yet.");
            MelonLogger.Msg("==============================================");
            _scanAt = DateTime.UtcNow.AddSeconds(8);
        }

        public override void OnUpdate()
        {
            if (_scanned || DateTime.UtcNow < _scanAt) return;
            _scanned = true;
            ScanFirearmApis();
        }

        private static void ScanFirearmApis()
        {
            string[] typeTerms = { "firearm", "gun", "weapon", "barrel", "chamber", "trigger" };
            string[] methodTerms = { "fire", "shoot", "trigger", "shot", "chamber", "muzzle", "barrel", "bolt" };
            int typeCount = 0;
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                string an;
                try { an = assembly.GetName().Name ?? ""; } catch { continue; }
                if (!(an.Contains("Marrow", StringComparison.OrdinalIgnoreCase) ||
                      an.Contains("Assembly-CSharp", StringComparison.OrdinalIgnoreCase) ||
                      an.Contains("Bone", StringComparison.OrdinalIgnoreCase))) continue;
                Type[] types;
                try { types = assembly.GetTypes(); }
                catch (ReflectionTypeLoadException ex) { types = ex.Types.Where(t => t != null).Cast<Type>().ToArray(); }
                catch { continue; }
                foreach (Type type in types)
                {
                    string tn;
                    try { tn = type.FullName ?? type.Name; } catch { continue; }
                    if (!typeTerms.Any(t => tn.Contains(t, StringComparison.OrdinalIgnoreCase))) continue;
                    typeCount++;
                    MelonLogger.Msg($"[WOH][TYPE] {tn}");
                    try
                    {
                        const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
                        foreach (MethodInfo m in type.GetMethods(flags).Take(80))
                        {
                            if (!methodTerms.Any(t => m.Name.Contains(t, StringComparison.OrdinalIgnoreCase))) continue;
                            string args = string.Join(", ", m.GetParameters().Select(p => p.ParameterType.FullName + " " + p.Name));
                            MelonLogger.Msg($"[WOH][METHOD] {tn}.{m.Name}({args}) -> {m.ReturnType.FullName}");
                        }
                        foreach (PropertyInfo p in type.GetProperties(flags).Take(40))
                            if (p.Name.Contains("barrel", StringComparison.OrdinalIgnoreCase) || p.Name.Contains("muzzle", StringComparison.OrdinalIgnoreCase) || p.Name.Contains("fire", StringComparison.OrdinalIgnoreCase))
                                MelonLogger.Msg($"[WOH][PROPERTY] {tn}.{p.PropertyType.FullName} {p.Name}");
                    }
                    catch (Exception ex) { MelonLogger.Warning($"[WOH] Scan issue {tn}: {ex.GetType().Name}"); }
                }
            }
            MelonLogger.Msg($"[WOH] Firearm candidate types found: {typeCount}");
            MelonLogger.Msg("[WOH] Scan complete. Share Latest.log entries beginning [WOH].");
        }
    }
}
