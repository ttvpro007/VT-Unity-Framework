using System.Linq;

namespace VT.CodeGenerator.Enums
{
    public static class FXModulesEnumGenerator
    {
        private const string SAVE_PATH = "Assets/VT-Framework-v1.0/Scripts/Gameplay/Health System/FX Modules/Enums";
        private const string FX_MODULES_FOLDER_PATH = "Assets/VT-Framework-v1.0/Scripts/Gameplay/Health System/FX Modules";

        public static void Generate(System.Type fxModuleType)
        {
            string content = "None,\n";
            System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(FX_MODULES_FOLDER_PATH);
            var fileInfos = directoryInfo.GetFiles("*.cs").Where(f => f.Name.Contains(fxModuleType.Name));

            int fileInfosCount = fileInfos.Count();
            int i = 0;
            foreach (var fileInfo in fileInfos)
            {
                string fxModule = fileInfo.Name.Split('.')[0].Replace(fxModuleType.Name, "");
                content += $"{Utilities.Utils.Tab(2)}{fxModule}" + (i < fileInfosCount - 1 ? "," + Utilities.Utils.LineBreak(1) : "");
                i++;
            }

            EnumGenerator.Generate($"{fxModuleType.Namespace}.Enums", "E" + fxModuleType.Name, content, SAVE_PATH);
        }
    }
}