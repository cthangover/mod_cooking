using System.Collections.Generic;
using Cthangover.Core.Mods;
using Cthangover.Core.Settings;

namespace Cthangover.Cooking
{
    /// <summary>
    /// Exposes a user-configurable setting for the number of rations
    /// consumed per character per in-game day. Registered automatically
    /// via <see cref="ModSettingsRegistry"/> when the cooking mod
    /// assembly loads.
    ///
    /// The static <see cref="RationsPerCharacter"/> property is the
    /// canonical source of truth shared with <c>RationStatisticsPanel</c>
    /// and any future consumption logic. It is updated by the core on
    /// boot (from persisted JSON) and on every UI change.
    /// </summary>
    public class CookingSettings : IModSettings
    {
        /// <summary>
        /// Current value for rations consumed per character per day.
        /// Default is 1. Updated by <see cref="ReadValues"/> on boot
        /// and whenever the player changes the setting in the menu.
        /// </summary>
        public static int RationsPerCharacter { get; private set; } = 1;

        public IReadOnlyList<ModSettingDefinition> GetDefinitions()
        {
            return new List<ModSettingDefinition>
            {
                new()
                {
                    Key = "rations_per_character",
                    Name = "mod_cooking/rations_per_character",
                    Type = SettingType.Slider,
                    DefaultValue = "1",
                    Min = 1f,
                    Max = 10f,
                    Step = 1f,
                },
            };
        }

        public void WriteValues(DataBlob blob)
        {
            blob.SetInt("rations_per_character", RationsPerCharacter);
        }

        public void ReadValues(DataBlob blob)
        {
            RationsPerCharacter = blob.GetInt("rations_per_character", 1);
            if (RationsPerCharacter < 1)
                RationsPerCharacter = 1;
        }
    }
}
