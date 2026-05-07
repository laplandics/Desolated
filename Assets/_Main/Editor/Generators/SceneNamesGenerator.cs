using System.IO;
using System.Linq;
using UnityEditor;

namespace EditorTools.Generators
{
    public static class SceneNamesGenerator
    {
        private const string STAGE_NAMES_PATH = "Assets/_Main/Scripts/Constant/SceneNames.cs";

        [MenuItem("Tools/Generate Scene Names")]
        public static void Generate()
        {
            var scenesGuids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets/_Main/Scenes" });
            var scenes = new string[scenesGuids.Length];
            for (var guidIndex = 0; guidIndex < scenesGuids.Length; guidIndex++)
            {
                var sceneAssetPath = AssetDatabase.GUIDToAssetPath(scenesGuids[guidIndex]);
                var sceneAssetName = Path.GetFileNameWithoutExtension(sceneAssetPath);
                scenes[guidIndex] = sceneAssetName;
            }

            EnumGenerator.GenerateEnum(STAGE_NAMES_PATH, "SceneNames", scenes.ToList());
        }
    }
}