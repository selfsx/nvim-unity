// Copyright (c) 2024 Sergey Ivonchik
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
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Unity.CodeEditor;
using UnityEngine;

namespace Neovim.Editor {
  public class VisualStudioCodeProxy : IProxy {
    private readonly ILogger _logger;

    [CanBeNull]
    private IExternalCodeEditor _instance;

    public VisualStudioCodeProxy(ILogger logger) {
      _logger = logger;
    }

    public void Activate() {
      Active = false;

      var current = CodeEditor.Editor;
      var editors = current.AsRegisteredEditors();

      var registeredOpt = editors
          .FirstOrDefault(e => e.AsProxyType() == ProxyType.VisualStudioCode);

      if (null != registeredOpt) {
        _instance = registeredOpt;
        _logger.Debug("[VsCodeProxy]: Found registered instance.");

        Active = true;
        return;
      }

      var assembly = AppDomain.CurrentDomain
          .GetAssemblies()
          .FirstOrDefault(a => a.GetName().Name == "Unity.VSCode.Editor");

      if (null == assembly) {
        _logger.Debug("[VsCodeProxy]: Assembly not found.");
        return;
      }

      // XXX: Reflection version of the following code:
      // var editor = new VSCodeScriptEditor(new VSCodeDiscovery(),
      // new ProjectGeneration(Directory.GetParent(Application.dataPath).FullName));

      try {
        var vscodeEditorType = assembly.GetType("VSCodeEditor.VSCodeScriptEditor");
        var vscodeDiscoveryType = assembly.GetType("VSCodeEditor.VSCodeDiscovery");
        var projectGenerationType = assembly.GetType("VSCodeEditor.ProjectGeneration");

        var vscodeDiscoveryInstance = Activator.CreateInstance(vscodeDiscoveryType);

        var projectGenerationConstructor =
            projectGenerationType.GetConstructor(new[] { typeof(string) });

        var projectGenerationInstance =
            projectGenerationConstructor?.Invoke(new object[] {
                Directory.GetParent(Application.dataPath)?.FullName
            });

        var vscodeEditorConstructor = vscodeEditorType.GetConstructor(new[] {
            vscodeDiscoveryType,
            projectGenerationType
        });

        var vscodeEditorInstance =
            vscodeEditorConstructor?.Invoke(new[] {
                vscodeDiscoveryInstance,
                projectGenerationInstance
            });

        _instance = vscodeEditorInstance as IExternalCodeEditor;
        _logger.Debug("[VsCodeProxy]: Successfully created instance.");

        Active = true;
      } catch (Exception e) {
        _logger.Error("[VsCodeProxy]: Failed to activate...");
        _logger.Error(e);
      }
    }

    public void Initialize(string editorInstallationPath) {
      _instance?.Initialize(editorInstallationPath);
      Initialized = true;
    }

    public void SyncIfNeeded(string[] addedFiles, string[] deletedFiles, string[] movedFiles,
        string[] movedFromFiles, string[] importedFiles) {
      _instance?.SyncIfNeeded(addedFiles, deletedFiles, movedFiles,
          movedFromFiles, importedFiles);
    }

    public void SyncAll() {
      _instance?.SyncAll();
    }

    public ProxyType Type => ProxyType.VisualStudioCode;

    public bool Active { get; private set; }

    public bool Initialized { get; private set; }
  }
}
