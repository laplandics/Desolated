using System;
using System.IO;

namespace Tools
{
    public static class PathHelper
    {
        public static string AssetPathToResourcePath(string assetPath)
        {
            if (string.IsNullOrWhiteSpace(assetPath))
                throw new ArgumentException("Asset path is null or empty.", nameof(assetPath));
            
            assetPath = assetPath.Replace('\\', '/');
            var resourcesFolder = "/Resources/";
            
            var resourcesIndex = assetPath.IndexOf(resourcesFolder, StringComparison.Ordinal);
            
            if (resourcesIndex < 0 && assetPath.StartsWith("Resources/"))
            {
                resourcesIndex = -1;
                resourcesFolder = "Resources/";
            }

            if (resourcesIndex < 0)
                throw new ArgumentException(
                    $"Path does not contain a Resources folder: {assetPath}",
                    nameof(assetPath));
            
            var startIndex = resourcesIndex + resourcesFolder.Length;
            var relativePath = assetPath[startIndex..];
            relativePath = Path.ChangeExtension(relativePath, null);
            
            return relativePath;
        }
    }
}