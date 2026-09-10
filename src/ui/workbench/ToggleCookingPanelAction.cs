using Cthangover.Core.Scenes;
using Cthangover.Core.Actions.Atomic;
using Cthangover.Core.Actions;
using Cthangover.Core.Utils;
using Godot;

namespace Mods.Cooking.Workbench
{
    /// <summary>
    /// Scenario action registered under <c>toggle_cooking_workbench</c>
    /// that toggles the cooking workbench UI panel on the scenario screen.
    /// Called from scenario script directives via the action system.
    /// Locates the <c>ModLastPanel</c> root node in the scene tree,
    /// finds the <c>CookingWorkbenchPanel</c> child (a <see cref="WorkbenchPanel"/>),
    /// and calls <c>Switch()</c> to show or hide it. No-op if the root
    /// or panel node is missing.
    /// </summary>
    public class ToggleCookingPanelAction : IScenarioAction
    {
        /// <summary>
        /// Unique action name used in scenario script commands
        /// (e.g. <c>action toggle_cooking_workbench</c>).
        /// </summary>
        public string Name => "toggle_cooking_workbench";

        /// <summary>
        /// Executes the toggle: finds the <c>CookingWorkbenchPanel</c>
        /// under <c>ModLastPanel</c> and calls <c>Switch()</c> to
        /// alternate its visibility state.
        /// </summary>
        public void Run(IActionContext ctx)
        {
            GameLogger.Log("COOKING", "ToggleCookingPanelAction.Run: looking for CookingWorkbenchPanel...");
            var root = SceneContextNode.FindNode<Control>("ModLastPanel");
            if (root == null)
            {
                GameLogger.Log("COOKING", "ToggleCookingPanelAction.Run: ModLastPanel NOT FOUND", LogLevel.Error);
                return;
            }

            var panel = root.GetNodeOrNull<WorkbenchPanel>("CookingWorkbenchPanel");
            if (panel == null)
            {
                GameLogger.Log("COOKING", "ToggleCookingPanelAction.Run: creating panel via TscnScenes.LoadAndBuild...");
                var loaded = TscnScenes.LoadAndBuild("scenes/cooking_panel.tscn");
                if (loaded == null)
                {
                    GameLogger.Log("COOKING", "ToggleCookingPanelAction.Run: LoadAndBuild returned NULL", LogLevel.Error);
                    return;
                }
                var control = loaded as Control;
                if (control == null) return;
                control.Name = "CookingWorkbenchPanel";
                control.Visible = false;
                control.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
                root.AddChild(control);
                panel = control as WorkbenchPanel;
            }

            GameLogger.Log("COOKING", $"ToggleCookingPanelAction.Run: switching panel (currently Visible={panel?.Visible})");
            panel?.Switch();
        }
    }
}
