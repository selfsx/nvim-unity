using System;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEditor.PackageManager;

namespace Neovim.Editor.Projects {
  internal interface IAssemblyNameProvider {
    string[] ProjectSupportedExtensions { get; }

    string ProjectGenerationRootNamespace { get; }

    ProjectType ProjectType { get; }

    string GetAssemblyNameFromScriptPath(string path);

    string GetProjectName(string name, IEnumerable<string> defines);

    bool IsInternalizedPackagePath(string path);

    IEnumerable<Assembly> GetAssemblies(Func<string, bool> shouldFileBePartOfSolution);

    IEnumerable<string> GetAllAssetPaths();

    PackageInfo FindForAssetPath(string assetPath);

    ResponseFileData ParseResponseFile(string responseFilePath, string projectDirectory, string[] systemReferenceDirectories);

    IEnumerable<string> GetRoslynAnalyzerPaths();

    void ToggleProjectGeneration(ProjectType preference);

    void ResetPackageInfoCache();

    void ResetAssembliesCache();
  }
}
