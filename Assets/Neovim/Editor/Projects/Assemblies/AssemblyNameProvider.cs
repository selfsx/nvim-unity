using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditor.PackageManager;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace Neovim.Editor.Projects {
  internal class AssemblyNameProvider : IAssemblyNameProvider {
    // @formatter:off
    private readonly Dictionary<string, PackageInfo> _packageCache = new Dictionary<string, PackageInfo>();

    private Assembly[] _editorAssemblies;
    private Assembly[] _playerAssemblies;
    private ProjectType _projectType = (ProjectType)EditorPrefs.GetInt("unity_project_generation_flag", 3);

    public string[] ProjectSupportedExtensions => EditorSettings.projectGenerationUserExtensions;
    public string ProjectGenerationRootNamespace => EditorSettings.projectGenerationRootNamespace;
    // @formatter:on

    public ProjectType ProjectType {
      get => _projectType;

      private set {
        EditorPrefs.SetInt("unity_project_generation_flag", (int)value);
        _projectType = value;
      }
    }

    public string GetAssemblyNameFromScriptPath(string path) {
      return CompilationPipeline.GetAssemblyNameFromScriptPath(path);
    }

    public IEnumerable<Assembly> GetAssemblies(Func<string, bool> shouldFileBePartOfSolution) {
      _editorAssemblies ??= GetAssembliesByType(AssembliesType.Editor).ToArray();

      if (ProjectType.HasFlag(ProjectType.PlayerAssemblies)) {
        _playerAssemblies ??= GetAssembliesByType(AssembliesType.Player).ToArray();
      }

      if (!ProjectType.HasFlag(ProjectType.PlayerAssemblies)) {
        return _editorAssemblies.Where(a => a.sourceFiles.Any(shouldFileBePartOfSolution));
      }

      return _editorAssemblies
          .Concat(_playerAssemblies)
          .Where(a => a.sourceFiles.Any(shouldFileBePartOfSolution));
    }

    private static IEnumerable<Assembly> GetAssembliesByType(AssembliesType type) {
      foreach (var assembly in CompilationPipeline.GetAssemblies(type)) {
        var outputPath = type == AssembliesType.Editor
            ? $@"Temp\Bin\Debug\{assembly.name}\"
            : $@"Temp\Bin\Debug\{assembly.name}\Player\";

        yield return new Assembly(assembly.name, outputPath, assembly.sourceFiles,
            assembly.defines, assembly.assemblyReferences, assembly.compiledAssemblyReferences,
            assembly.flags, assembly.compilerOptions, assembly.rootNamespace);
      }
    }

    public string GetProjectName(string name, IEnumerable<string> defines) {
      if (!ProjectType.HasFlag(ProjectType.PlayerAssemblies)) {
        return name;
      }

      return !defines.Contains("UNITY_EDITOR") ? name + ".Player" : name;
    }

    public IEnumerable<string> GetAllAssetPaths() {
      return AssetDatabase.GetAllAssetPaths();
    }

    public PackageInfo FindForAssetPath(string assetPath) {
      var parentPackageAssetPath = ResolvePotentialParentPackageAssetPath(assetPath);
      if (parentPackageAssetPath == null) {
        return null;
      }

      if (_packageCache.TryGetValue(parentPackageAssetPath, out var cachedPackageInfo)) {
        return cachedPackageInfo;
      }

      var result = PackageInfo.FindForAssetPath(parentPackageAssetPath);
      _packageCache[parentPackageAssetPath] = result;
      return result;
    }

    public void ResetPackageInfoCache() {
      _packageCache.Clear();
    }

    public void ResetAssembliesCache() {
      _editorAssemblies = null;
      _playerAssemblies = null;
    }

    public bool IsInternalizedPackagePath(string path) {
      if (string.IsNullOrEmpty(path.Trim())) {
        return false;
      }

      var packageInfo = FindForAssetPath(path);
      if (packageInfo == null) {
        return false;
      }

      var packageSource = packageInfo.source;

      return packageSource switch {
          PackageSource.Embedded => !ProjectType.HasFlag(ProjectType.Embedded),
          PackageSource.Registry => !ProjectType.HasFlag(ProjectType.Registry),
          PackageSource.BuiltIn => !ProjectType.HasFlag(ProjectType.BuiltIn),
          PackageSource.Unknown => !ProjectType.HasFlag(ProjectType.Unknown),
          PackageSource.Local => !ProjectType.HasFlag(ProjectType.Local),
          PackageSource.Git => !ProjectType.HasFlag(ProjectType.Git),
          PackageSource.LocalTarball => !ProjectType.HasFlag(ProjectType.LocalTarBall),
          _ => false
      };
    }

    // @formatter:off
    public ResponseFileData ParseResponseFile(string responseFilePath, string projectDirectory, string[] systemReferenceDirectories) {
      return CompilationPipeline.ParseResponseFile(responseFilePath, projectDirectory, systemReferenceDirectories);
    }
    // @formatter:on

    public IEnumerable<string> GetRoslynAnalyzerPaths() {
      return PluginImporter.GetAllImporters()
          .Where(i =>
              !i.isNativePlugin
              && AssetDatabase.GetLabels(i).SingleOrDefault(l => l == "RoslynAnalyzer") != null)
          .Select(i => i.assetPath);
    }

    public void ToggleProjectGeneration(ProjectType preference) {
      if (ProjectType.HasFlag(preference)) {
        ProjectType ^= preference;
      } else {
        ProjectType |= preference;
      }
    }

    public void ResetProjectGenerationFlag() {
      ProjectType = ProjectType.None;
    }

    private static string ResolvePotentialParentPackageAssetPath(string assetPath) {
      const string packagesPrefix = "packages/";

      if (!assetPath.StartsWith(packagesPrefix, StringComparison.OrdinalIgnoreCase)) {
        return null;
      }

      var followupSeparator = assetPath.IndexOf('/', packagesPrefix.Length);

      return followupSeparator == -1
          ? assetPath.ToLowerInvariant()
          : assetPath.Substring(0, followupSeparator).ToLowerInvariant();
    }
  }
}
