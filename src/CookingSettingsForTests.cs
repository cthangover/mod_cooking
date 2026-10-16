using Cthangover.Core.Settings;

namespace Cthangover.Cooking
{
    /// <summary>
    /// Test accessor over <see cref="CookingSettings"/>: the real class exposes
    /// <c>RationsPerCharacter</c> with a private setter because only the core
    /// settings pipeline may write it, and the getter is static — mod unit
    /// tests exercise the clamp through the public <c>ReadValues</c> contract
    /// and assert via this static passthrough, so no reflection into private
    /// members is needed. Stateless and harmless in production builds.
    /// </summary>
    public static class CookingSettingsForTests
    {
        /// <summary>Current parsed value, for test assertions.</summary>
        public static int RationsPerCharacter => CookingSettings.RationsPerCharacter;
    }
}
