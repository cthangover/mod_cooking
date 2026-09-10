var modPanel = FindNode<Control>("ModLastPanel");
if (modPanel == null)
    return;

if (modPanel.GetNodeOrNull("CookingWorkbenchPanel") != null)
    return;

var panel = Cthangover.Core.Scenes.TscnScenes.LoadAndBuild("scenes/cooking_panel.tscn");
if (panel is Control control)
{
    control.Name = "CookingWorkbenchPanel";
    control.Visible = false;
    control.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
    modPanel.AddChild(control);
}