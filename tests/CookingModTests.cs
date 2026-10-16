using System.Text.Json;
using System.Threading.Tasks;
using Cthangover.Core.Items;
using Cthangover.Core.Mods.Testing;
using Cthangover.Core.Settings;

namespace Cthangover.Cooking.Tests
{
    /// <summary>
    /// Unit tests of the cooking mod's pure logic (plan-mod-testing.md §4.2).
    /// Compiled by ModTestEngine into a separate {modId}__tests assembly,
    /// only in the explicit test mode. No scene-tree interaction: anything
    /// requiring a live scene belongs to the integration contour.
    /// </summary>
    public class CookingModTests
    {
        [ModTest("recipe/wolf_meat_data_shape")]
        static void WolfMeatRecipeDataShape(ModTestContext ctx)
        {
            // The recipe JSON declared by the mod must keep the exact shape
            // RecipeFactory consumes: single recipe, cooking type, inputs/outputs.
            var raw = ctx.ReadFileText("recipes/cooking.json");
            ModAssert.NotNull(raw, "recipes/cooking.json exists");
            using var doc = JsonDocument.Parse(raw);
            var items = doc.RootElement.GetProperty("Items");
            ModAssert.Equal(1, items.GetArrayLength(), "recipe count");

            var recipe = items[0];
            ModAssert.Equal("recipe/wolf_meat_to_ration", recipe.GetProperty("Id").GetString(), "recipe id");
            ModAssert.Equal("Cooking", recipe.GetProperty("Type").GetString(), "workbench type");
            ModAssert.Equal(15, recipe.GetProperty("Time").GetInt32(), "crafting time");
            ModAssert.Equal(2, recipe.GetProperty("Input").GetArrayLength(), "input ingredient count");
            ModAssert.Equal(1, recipe.GetProperty("Output").GetArrayLength(), "output ingredient count");

            var meat = recipe.GetProperty("Input")[0];
            ModAssert.Equal("food/wolf_meat", meat.GetProperty("Id").GetString(), "input meat id");
            ModAssert.Equal(1, meat.GetProperty("Count").GetInt32(), "input meat count");
            var branches = recipe.GetProperty("Input")[1];
            ModAssert.Equal("resource/branches", branches.GetProperty("Id").GetString(), "input branches id");
            ModAssert.Equal(2, branches.GetProperty("Count").GetInt32(), "input branches count");
        }

        [ModTest("recipe/recipe_data_unlock_flow")]
        static void RecipeDataUnlockFlow(ModTestContext ctx)
        {
            // The unlock contract WolfMeatToRation relies on: Add is idempotent
            // and Has mirrors membership. Exercises the same flow the item
            // action performs, through the public RecipeData API.
            var data = new RecipeData();
            ModAssert.False(data.Has("recipe/wolf_meat_to_ration"), "fresh state does not know the recipe");
            ModAssert.True(data.Add("recipe/wolf_meat_to_ration"), "first Add unlocks");
            ModAssert.False(data.Add("recipe/wolf_meat_to_ration"), "second Add is a no-op");
            ModAssert.True(data.Has("recipe/wolf_meat_to_ration"), "recipe stays unlocked");

            // Round-trip through JSON: Data is a public property, the save
            // system serializes RecipeData as a whole.
            var json = JsonSerializer.Serialize(data);
            var restored = JsonSerializer.Deserialize<RecipeData>(json);
            ModAssert.True(restored.Has("recipe/wolf_meat_to_ration"), "unlock survives JSON round-trip");
        }

        [ModTest("settings/rations_per_character_clamp")]
        static void SettingsClamp(ModTestContext ctx)
        {
            // CookingSettings.ReadValues clamps below-minimum input to 1 —
            // verify through a real DataBlob key-value round-trip.
            var blob = new DataBlob();
            blob.SetInt("rations_per_character", -5);
            new global::Cthangover.Cooking.CookingSettings().ReadValues(blob);
            ModAssert.Equal(1, global::Cthangover.Cooking.CookingSettingsForTests.RationsPerCharacter, "negative value clamps to 1");

            var blob2 = new DataBlob();
            blob2.SetInt("rations_per_character", 4);
            new global::Cthangover.Cooking.CookingSettings().ReadValues(blob2);
            ModAssert.Equal(4, global::Cthangover.Cooking.CookingSettingsForTests.RationsPerCharacter, "valid value passes through");

            // Write/read round-trip through the blob's own serialization.
            new global::Cthangover.Cooking.CookingSettings().WriteValues(blob2);
            ModAssert.Equal(4, blob2.GetInt("rations_per_character", 0), "WriteValues stores the value");
        }

        [ModTest("manifest/declares_required_dependencies")]
        static void ManifestDependencies(ModTestContext ctx)
        {
            // The cooking UI binds to interface widget types and item/time
            // subsystems: the manifest must keep these declared, otherwise
            // compilation against the dependencies breaks silently.
            var raw = ctx.ReadFileText("manifest.json");
            ModAssert.NotNull(raw, "manifest.json exists");
            using var doc = JsonDocument.Parse(raw);
            var depends = doc.RootElement.GetProperty("depends");
            var found = new[] { "visual_novel_core", "interface", "items_base", "time_base" };
            foreach (var dep in found)
            {
                var ok = false;
                foreach (var d in depends.EnumerateArray())
                {
                    var s = d.GetString() ?? "";
                    var id = s.Split('>', '<', '=', '?')[0];
                    if (id == dep) { ok = true; break; }
                }
                ModAssert.True(ok, $"dependency '{dep}' declared");
            }
        }

        [ModTest("frames/async_test_spans_frames", TimeoutSeconds = 10)]
        static async Task AsyncTestSpansFrames(ModTestContext ctx)
        {
            // Async tests may wait for engine frames (deferred UI rebuilds,
            // signals). The recovery cycle between tests must keep the scene
            // tree intact across those frames: core autoloads and the running
            // scene survive, and the run still finishes with a report.
            var tree = (Godot.SceneTree)Godot.Engine.GetMainLoop();
            for (var i = 0; i < 3; i++)
                await tree.ToSignal(tree, Godot.SceneTree.SignalName.ProcessFrame);

            ModAssert.True(tree.Root.HasNode("GameData"), "/root/GameData survives frames between tests");
            ModAssert.NotNull(tree.CurrentScene, "current scene survives frames between tests");
            ModAssert.NotNull(ctx.ReadFileText("recipes/cooking.json"), "mod files stay readable after awaiting frames");
        }
    }
}
