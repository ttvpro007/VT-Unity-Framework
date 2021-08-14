namespace VT.CodeGenerator
{
    public static class ClassGenerator
    {
        private static readonly string CLASS_TEMPLATE_NAME = "ClassTemplate";
        private static readonly string RELATIVE_CLASS_TEMPLATE_FOLDER_PATH = "Assets/VT-Framework-v1.0/Scripts/Code Generator/Templates/Class";
        private static readonly string FULL_CLASS_TEMPLATE_FILE_PATH = $"{Utilities.PathUtils.ConvertRelativeToAbsolutePath(RELATIVE_CLASS_TEMPLATE_FOLDER_PATH)}/{CLASS_TEMPLATE_NAME}.txt";
        private static readonly string USING_DIRECTIVES_PLACE_HOLDER = "##USING##";
        private static readonly string NAMESPACE_PLACE_HOLDER = "##NAMESPACE##";
        private static readonly string CLASS_PLACE_HOLDER = "##CLASS##";
        private static readonly string INHERIT_PLACE_HOLDER = "##INHERIT##";
        private static readonly string CONTENT_PLACE_HOLDER = "##CONTENT##";

        /// <summary>
        /// Generating an class file
        /// </summary>
        public static void Generate(string nameSpace, System.Type[] inheritFrom, string className, string classContent, string savePath)
        {
            // Create template file if not exist
            if (!System.IO.File.Exists(FULL_CLASS_TEMPLATE_FILE_PATH))
            {
                CodeGeneratorUtils.CreateTemplateTextFile(GetClassTemplateContent(), FULL_CLASS_TEMPLATE_FILE_PATH);
            }

            string fullFolderPath = $"{UnityEngine.Application.dataPath}/{savePath}";
            string allUsingDirective = string.Empty;
            string allInheritances = inheritFrom != null && inheritFrom.Length > 0 ? ": " : string.Empty;

            // Get appropriate using directives and inherit classes name
            foreach (var inheritance in inheritFrom)
            {
                if (nameSpace != inheritance.Namespace)
                {
                    allUsingDirective += $"using {inheritance.Namespace};{Utilities.Utils.LineBreak(1)}";
                }

                allInheritances += inheritance.Name + ", ";
            }
            
            // Read from template file and replace place holders with appropriate contents
            string fileContent = CodeGeneratorUtils.ReadFromFile(FULL_CLASS_TEMPLATE_FILE_PATH)
            .MassReplace(new (string, string)[5]
            {
                (USING_DIRECTIVES_PLACE_HOLDER, allUsingDirective.Remove(allUsingDirective.Length - 1, 1)),
                (NAMESPACE_PLACE_HOLDER, nameSpace),
                (CLASS_PLACE_HOLDER, className),
                (INHERIT_PLACE_HOLDER, allInheritances.Remove(allInheritances.Length - 2, 2)),
                (CONTENT_PLACE_HOLDER, classContent)
            });

            // Import file into Unity and ping
            CodeGeneratorUtils.CreateAndImportCSFile(className, fileContent, fullFolderPath, true);
        }

        private static string GetClassTemplateContent()
        {
            string content = string.Empty;
            content += "///This file is auto generated. Do not edit.";
            content += Utilities.Utils.LineBreak(1);
            content += USING_DIRECTIVES_PLACE_HOLDER;
            content += Utilities.Utils.LineBreak(1);
            content += $"namespace {NAMESPACE_PLACE_HOLDER}";
            content += Utilities.Utils.LineBreak(1);
            content += "{";
            content += Utilities.Utils.LineBreak(1);
            content += $"{Utilities.Utils.Tab(1)}public class {CLASS_PLACE_HOLDER + " " + INHERIT_PLACE_HOLDER}";
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