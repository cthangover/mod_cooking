using Cthangover.Core.Items;
using Cthangover.Core.Settings;
using Cthangover.Core.UI;
using Cthangover.Core.UI.Theming;
using Cthangover.Core.Utils;
using Godot;

using Cthangover.Mods.Interface.Base.Lists;
namespace Mods.Cooking.Workbench
{
    /// <summary>
    /// Renders a single ingredient row inside a <see cref="RecipeItemBehaviour"/>
    /// on the cooking workbench. Displays the ingredient's item icon and the
    /// required count. When <see cref="CheckAndUpdate"/> is called, compares
    /// inventory count against the recipe requirement and tints the icon and
    /// count text grey if the player lacks enough items.
    /// </summary>
    public class RecipeIconItemBehaviour : Widget
    {
        private static readonly Color NormalColor = UiTokens.InkBright;
        private static readonly Color NotItemsColor = UiTokens.Muted;

        private TextureRect img;
        private Label txt;
        private IIngredient ingredient;

        protected override void OnceConstruct()
        {
            img = GetNode<TextureRect>("Img");
            txt = GetNode<Label>("Txt");
        }

        /// <summary>
        /// Binds this icon widget to a recipe ingredient definition.
        /// Sets the icon texture from <c>ingredient.Item.Sprite</c> (logs an error
        /// if the item reference is null) and displays the required count as text.
        /// The <paramref name="hasItems"/> flag is currently unused but reserved
        /// for future visual pre-initialization.
        /// </summary>
        public void Init(IIngredient ingredient, bool hasItems)
        {
            EnsureConstructed();
            this.ingredient = ingredient;
            if (ingredient.Item != null)
            {
                img.Texture = ingredient.Item.Sprite;
            }
            else
            {
                GameLogger.Log("MOD_TEST", $"RecipeIconItemBehaviour::Init ingredient.Item is NULL", LogLevel.Error);
            }
            txt.Text = ingredient.Count.ToString();
        }

        /// <summary>
        /// Evaluates whether the player's inventory contains enough of the
        /// required ingredient. Tints the icon and count text to
        /// <c>NormalColor</c> (bright ink) when satisfied or <c>NotItemsColor</c>
        /// (muted) when insufficient. Called by the parent
        /// <see cref="RecipeItemBehaviour"/> during state refresh.
        /// </summary>
        /// <returns><c>true</c> if inventory meets or exceeds the required count.</returns>
        public bool CheckAndUpdate()
        {
            var result = GameData.Instance.Runtime.Inventory.CheckCount(ingredient.Item) >= ingredient.Count;
            txt.AddThemeColorOverride("font_color", result ? NormalColor : NotItemsColor);
            img.Modulate = result ? UiTokens.White : NotItemsColor;
            return result;
        }
    }
}
