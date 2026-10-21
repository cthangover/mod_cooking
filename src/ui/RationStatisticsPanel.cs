using Cthangover.Cooking;

using System;
using Cthangover.Core.Settings;
using Cthangover.Core.UI;
using Godot;

using Cthangover.Mods.Interface.Base.Lists;
namespace Mods.Cooking.Rations
{
    /// <summary>
    /// Displays live ration statistics on the dinner/ration UI screen.
    /// Shows the player the current ration count, how many rations are
    /// consumed daily (one per character), and the maximum number of days
    /// the party can survive on available rations. Listens to
    /// <see cref="Core.Items.InventoryBag.Change"/> events to refresh
    /// automatically whenever the inventory is modified.
    /// </summary>
    public class RationStatisticsPanel : Widget
    {
        private RichTextLabel label;
        private Action<string, int> _onInventoryChange;

        protected override void OnceConstruct()
        {
            label = GetNode<RichTextLabel>("RationMargin/RationLabel");

            _onInventoryChange = (_, _) => UpdateInfo();
            GameData.Instance.Runtime.Inventory.Change += _onInventoryChange;
        }

        protected override void ShowConstruct()
        {
            UpdateInfo();
        }
        
        protected override void OnceDestruct()
        {
            GameData.Instance.Runtime.Inventory.Change -= _onInventoryChange;
        }

        /// <summary>
        /// Recomputes and refreshes the ration-statistics text.
        /// Reads current ration inventory via <see cref="Core.Items.InventoryBag.CheckCount"/>
        /// for <c>"food/ration"</c> and the total character count from
        /// <see cref="CharacterData"/>. Formats three localized lines:
        /// total rations, daily consumption, and remaining days
        /// (rations divided by character count, clamped to 1 minimum).
        /// </summary>
        public void UpdateInfo()
        {
            if (label == null)
                return;
            
            var rationCount = GameData.Instance.Runtime.Inventory.CheckCount("food/ration");
            var characterCount = GameData.Instance.Runtime.CharacterData.Characters.Count;
            var perChar = CookingSettings.RationsPerCharacter;
            var dailyConsumption = characterCount * perChar;

            label.Text = string.Format(TranslationServer.Translate("ui/diner/rationcount"), rationCount) +
                         "\n" + string.Format(TranslationServer.Translate("ui/diner/rationday"), dailyConsumption) +
                         "\n" + string.Format(TranslationServer.Translate("ui/diner/rationmax"), rationCount / Mathf.Max(1, dailyConsumption));
        }
    }
}
