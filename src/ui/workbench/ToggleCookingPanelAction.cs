using Cthangover.Core.Actions;
using Cthangover.Core.Actions.Atomic;
using Cthangover.Core.UI;
using Cthangover.Core.Utils;

namespace Mods.Cooking.Workbench
{
    /// <summary>
    /// Scenario action registered under <c>toggle_cooking_workbench</c>.
    /// Delegates to <see cref="UiRegistry.Toggle"/>, which owns the workbench
    /// panel instance and its visibility state — the action no longer searches
    /// the tree or builds the scene itself, so the panel survives scene changes.
    /// </summary>
    public class ToggleCookingPanelAction : IScenarioAction
    {
        /// <summary>
        /// Unique action name used in scenario script commands
        /// (e.g. <c>action toggle_cooking_workbench</c>).
        /// </summary>
        public string Name => "toggle_cooking_workbench";

        /// <summary>
        /// Toggles the <c>cooking_workbench</c> entry through the UI registry.
        /// </summary>
        public void Run(IActionContext ctx)
        {
            GameLogger.Log("COOKING", "ToggleCookingPanelAction.Run: toggling 'cooking_workbench'");
            UiRegistry.Toggle("cooking_workbench");
        }
    }
}
