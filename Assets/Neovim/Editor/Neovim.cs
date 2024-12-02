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

using Unity.CodeEditor;
using UnityEditor;
using UnityEngine;

namespace Neovim.Editor {
  [InitializeOnLoad]
  public class Neovim : IExternalCodeEditor {
    private static readonly ILogger Logger;
    private static readonly NeovimPreferences Prefs = new NeovimPreferences();

    static Neovim() {
      Logger = new UnityLoggerImpl(Prefs.LogLevel);
      Logger.Debug("Registering...");

      CodeEditor.Register(new Neovim());
    }

    public bool TryGetInstallationForPath(string editorPath,
        out CodeEditor.Installation installation) {
      Logger.Debug($"Checking if Neovim is installed at {editorPath}...");

      if (editorPath == Editor.Installations.Homebrew.Path) {
        installation = Editor.Installations.Homebrew;
        return true;
      }

      installation = default;

      return false;
    }

    public void OnGUI() {
      EditorGUILayout.Space();
      EditorGUILayout.LabelField("Neovim Plugin Configuration", EditorStyles.boldLabel);
      EditorGUILayout.Space();

      var labelWidth = GUILayout.Width(120);
      var fieldWidth = GUILayout.Width(150);
      var buttonWidth = GUILayout.Width(120);

      // XXX: Host
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField("Server Host:", labelWidth);
      Prefs.Host = EditorGUILayout.TextField(Prefs.Host, fieldWidth);
      EditorGUILayout.EndHorizontal();

      // XXX: Port
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField("Server Port:", labelWidth);
      Prefs.Port = EditorGUILayout.TextField(Prefs.Port, fieldWidth);
      EditorGUILayout.EndHorizontal();

      // XXX: LogLevel
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField("Log Level:", labelWidth);
      Prefs.LogLevel = (LevelType)EditorGUILayout.EnumPopup(Prefs.LogLevel, fieldWidth);
      EditorGUILayout.EndHorizontal();

      // XXX: Proxy Plugins
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField("Proxy Plugins:", labelWidth);
      EditorGUILayout.TextField("Unknown", Styles.Labels.Unknown);
      EditorGUILayout.EndHorizontal();

      // XXX: Current Proxy
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField("Current Proxy:", labelWidth);
      Prefs.ProxyPlugin = (ProxyType)EditorGUILayout.EnumPopup(Prefs.ProxyPlugin, fieldWidth);
      EditorGUILayout.EndHorizontal();

      // XXX: Server Status
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField("Server Status:", labelWidth);
      EditorGUILayout.TextField("Unknown", Styles.Labels.Unknown);
      EditorGUILayout.EndHorizontal();

      EditorGUILayout.Space();

      if (GUILayout.Button("Restart Server", buttonWidth)) {
        RestartServerImpl();
      }

      EditorGUILayout.Space();
    }

    public void SyncIfNeeded(string[] addedFiles, string[] deletedFiles, string[] movedFiles,
        string[] movedFromFiles, string[] importedFiles) {
      Logger.Debug("SyncIfNeeded...");
    }

    public void SyncAll() {
      Logger.Debug("SyncAll...");
    }

    public void Initialize(string editorInstallationPath) {
      Logger.Debug($"Initializing Neovim at {editorInstallationPath}...");
    }

    public bool OpenProject(string filePath = "", int line = -1, int column = -1) {
      Logger.Debug($"OpenProject: {filePath} at line {line}, column {column}");
      return false;
    }

    private void RestartServerImpl() {
      Logger.Debug("Restarting server...");
      // TODO: Implement.
    }

    public CodeEditor.Installation[] Installations {
      get {
        return new[] {
            Editor.Installations.Homebrew,
        };
      }
    }
  }
}
