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

using UnityEditor;

namespace Neovim.Editor {
  public class NeovimPreferences {
    private const string HostKey = "k_neovim_server_host";
    private const string PortKey = "k_neovim_server_port";
    private const string LogLevelKey = "k_neovim_server_log_level";
    private const string ProxyPluginKey = "k_neovim_server_proxy_plugin";

    public string Host {
      get => EditorPrefs.GetString(HostKey, "127.0.0.1");
      set => EditorPrefs.SetString(HostKey, value);
    }

    public string Port {
      get => EditorPrefs.GetString(PortKey, "8448");
      set => EditorPrefs.SetString(PortKey, value);
    }

    public LevelType LogLevel {
      get => (LevelType)EditorPrefs.GetInt(LogLevelKey, (int)LevelType.Info);
      set => EditorPrefs.SetInt(LogLevelKey, (int)value);
    }

    public ProxyType ProxyPlugin {
      get => (ProxyType)EditorPrefs.GetInt(ProxyPluginKey, (int)ProxyType.Auto);
      set => EditorPrefs.SetInt(ProxyPluginKey, (int)value);
    }
  }
}
