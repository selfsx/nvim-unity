// Copyright (c) 2022 Sergey Ivonchik
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
using UnityEditor.Compilation;

// ProjectHeader
// ProjectContents

namespace Neovim.Editor.Projects {
  // ProjectAssembly
  internal class ProjectPart {
    public ProjectPart(string name, Assembly assembly, string assetsProjectPart) {
      Name = name;
      Assembly = assembly;
      AssetsProjectPart = assetsProjectPart;
      OutputPath = assembly != null ? assembly.outputPath : "Temp/Bin/Debug";
      SourceFiles = assembly != null ? assembly.sourceFiles : Array.Empty<string>();
      RootNamespace = assembly.RootNamespaceCompat();

      AssemblyReferences = assembly != null
          ? assembly.assemblyReferences
          : Array.Empty<Assembly>();

      CompiledAssemblyReferences = assembly != null
          ? assembly.compiledAssemblyReferences
          : Array.Empty<string>();

      Defines = assembly != null
          ? assembly.defines
          : Array.Empty<string>();

      CompilerOptions = assembly != null
          ? assembly.compilerOptions
          : new ScriptCompilerOptions();
    }

    public string Name { get; }

    public string OutputPath { get; }

    public Assembly Assembly { get; }

    public string AssetsProjectPart { get; }

    public IEnumerable<string> SourceFiles { get; }

    public string RootNamespace { get; }

    public IEnumerable<Assembly> AssemblyReferences { get; }

    public IEnumerable<string> CompiledAssemblyReferences { get; }

    public IEnumerable<string> Defines { get; }

    public ScriptCompilerOptions CompilerOptions { get; }
  }
}
