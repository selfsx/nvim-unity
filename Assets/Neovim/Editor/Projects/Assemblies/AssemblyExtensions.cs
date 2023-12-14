// Copyright (c) 2023 Sergey Ivonchik
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE
// OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Compilation;
using UnityEngine;

namespace Neovim.Editor.Projects {
  public static class AssemblyExtensions {
    public static string RootNamespaceCompat(this Assembly assembly) {
#if UNITY_2020_2_OR_NEWER
      return assembly != null ? assembly.rootNamespace : string.Empty;
#else
      return UnityEditor.EditorSettings.projectGenerationRootNamespace;
#endif
    }

    public static IEnumerable<ResponseFileData> ParseResponseFileData(this Assembly assembly,
        string projectDirectory) {
      if (assembly == null) {
        return Array.Empty<ResponseFileData>();
      }

      var systemReferenceDirectories =
          CompilationPipeline.GetSystemAssemblyDirectories(assembly.compilerOptions
              .ApiCompatibilityLevel);

      var responseFilesData = assembly.compilerOptions.ResponseFiles.ToDictionary(x => x, x =>
          CompilationPipeline.ParseResponseFile(x, projectDirectory, systemReferenceDirectories));

      var responseFilesWithErrors = responseFilesData
          .Where(x => x.Value.Errors.Any())
          .ToDictionary(x => x.Key, x => x.Value);

      if (!responseFilesWithErrors.Any()) {
        return responseFilesData.Select(x => x.Value);
      }

      foreach (var error in responseFilesWithErrors) {
        foreach (var valueError in error.Value.Errors) {
          Debug.LogError($"{error.Key} Parse Error : {valueError}");
        }
      }

      return responseFilesData.Select(x => x.Value);
    }
  }
}
