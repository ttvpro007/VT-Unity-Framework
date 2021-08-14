namespace VT.CodeGenerator.Enums
{
    public static class WorldFXModulesEnumGenerator
    {
        private const string MENU_ITEM_NAME = "VT Framework/Code Generator/Create World FX Modules Enum File";

        [UnityEditor.MenuItem(MENU_ITEM_NAME)]
        public static void Generate()
        {
            FXModulesEnumGenerator.Generate(typeof(Gameplay.HealthSystem.FXModules.WorldFXModule));
        }
    }
}