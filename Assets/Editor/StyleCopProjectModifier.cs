using UnityEditor;

public class StyleCopProjectModifier : AssetPostprocessor
{
    private static string OnGeneratedCSProject(string path, string content)
    {
        // StyleCop.Analyzers
        const string analyzer =
            "<Analyzer Include=\"packages\\StyleCop.Analyzers.1.1.118\\analyzers\\dotnet\\cs\\StyleCop.Analyzers.dll\" />";

        // stylecop.json
        const string additionalFile =
            "<AdditionalFiles Include=\"stylecop.json\" />";

        // すでに存在する場合は何もしない
        if (!content.Contains(analyzer))
        {
            content = content.Replace(
                "</Project>",
                "  <ItemGroup>\n" +
                "    " + analyzer + "\n" +
                "  </ItemGroup>\n" +
                "</Project>"
            );
        }

        if (!content.Contains(additionalFile))
        {
            content = content.Replace(
                "</Project>",
                "  <ItemGroup>\n" +
                "    " + additionalFile + "\n" +
                "  </ItemGroup>\n" +
                "</Project>"
            );
        }

        return content;
    }
}
