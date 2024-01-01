using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using UnityEngine;
using System.Reflection;
using PersonalBoombox;

namespace YakuzaBoombox
{
    [BepInPlugin(PluginInfo.PluginGuid, PluginInfo.PluginName, PluginInfo.PluginVersion)]
    [BepInProcess("Lethal Company.exe")]
    [BepInDependency("ImoutoSama.PersonalBoombox", "1.3.0")]
    public class YakuzaBoomboxPlugin : BaseUnityPlugin
    {
        private ManualLogSource _logger;

        private ConfigEntry<bool> _overridePriceFlagConfig;
        private ConfigEntry<int> _overridePriceValueConfig;

        private void Awake(){
            _logger = BepInEx.Logging.Logger.CreateLogSource(PluginInfo.PluginGuid);
            _logger.LogInfo($"Plugin {PluginInfo.PluginName} has been added!");
 
            _overridePriceFlagConfig = Config.Bind("General", "Override Price Flag", false, "If you want to override price of the boomboxes, you MUST set this to true.");
            _overridePriceValueConfig = ConfigBindClamp("General", "Override Price Value", 30, "Overrides the price of the boomboxes by this value. Clamped from 0 to 1000.", 0, 1000);
 
            var request = PersonalBoomboxPlugin.AddFromAssemblyDll(Assembly.GetExecutingAssembly().Location);
            request.overridePriceFlag = _overridePriceFlagConfig.Value;
            request.overridePriceValue = _overridePriceValueConfig.Value;
        }

        private ConfigEntry<int> ConfigBindClamp(string section, string key, int defaultValue, string description, int min, int max){
            var config = Config.Bind(section, key, defaultValue, description);
            config.Value = Mathf.Clamp(config.Value, min, max);
            return config;
        }
    }
}