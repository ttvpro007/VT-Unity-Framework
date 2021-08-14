namespace VT.CodeGenerator.Enums
{
    public static class EnumGenerator
    {
        private static readonly string ENUM_TEMPLATE_NAME = "EnumTemplate";
        private static readonly string RELATIVE_ENUM_TEMPLATE_FOLDER_PATH = "Assets/VT-Framework-v1.0/Scripts/Code Generator/Templates/Enum";
        private static readonly string FULL_ENUM_TEMPLATE_FILE_PATH = $"{Utilities.PathUtils.ConvertRelativeToAbsolutePath(RELATIVE_ENUM_TEMPLATE_FOLDER_PATH)}/{ENUM_TEMPLATE_NAME}.txt";
        private static readonly string NAMESPACE_PLACE_HOLDER = "##NAMESPACE##";
        private static readonly string ENUM_PLACE_HOLDER = "##ENUM##";
        private static readonly string CONTENT_PLACE_HOLDER = "##CONTENT##";

        /// <summary>
        /// Generating a class file
        /// </summary>
        public static void Generate(string nameSpace, string enumName, string enumContent, string savePath)
        {
            // Create template file if not exist
            if (!System.IO.File.Exists(FULL_ENUM_TEMPLATE_FILE_PATH))
            {
                CodeGeneratorUtils.CreateTemplateTextFile(GetEnumTemplateContent(), FULL_ENUM_TEMPLATE_FILE_PATH);
            }
            
            string fullFolderPath = Utilities.PathUtils.ConvertRelativeToAbsolutePath(savePath);

            // Read from template file and replace place holders with appropriate contents
            string fileContent = CodeGeneratorUtils.ReadFromFile(FULL_ENUM_TEMPLATE_FILE_PATH)
            .MassReplace(new (string, string)[3]
            {
                (NAMESPACE_PLACE_HOLDER, nameSpace),
                (ENUM_PLACE_HOLDER, enumName),
                (CONTENT_PLACE_HOLDER, enumContent)
            });

            // Import file into Unity and ping
            CodeGeneratorUtils.CreateAndImportCSFile(enumName, fileContent, fullFolderPath, true);
        }

        private static string GetEnumTemplateContent()
        {
            string content = string.Empty;
            content += "///This file is auto generated. Do not edit.";
            content += Utilities.Utils.LineBreak(1);
            content += $"namespace {NAMESPACE_PLACE_HOLDER}";
            content += Utilities.Utils.LineBreak(1);
            content += "{";
            content += Utilities.Utils.LineBreak(1);
            content += $"{Utilities.Utils.Tab(1)}public enum {ENUM_PLACE_HOLDER}";
            content += Utilities.Utils.LineBreak(1);
            content += $"{Utilities.Utils.Tab(1)}{{";
            content += Utilities.Utils.LineBreak(1);
            content += $"{Utilities.Utils.Tab(2)}{CONTENT_PLACE_HOLDER}";
            content += Utilities.Utils.LineBreak(1);
            content += $"{Utilities.Utils.Tab(1)}}}";
            content += Utilities.Utils.LineBreak(1);
            content += "}";
            content += Utilities.Utils.LineBreak(1);
            return content;
        }
    }
}