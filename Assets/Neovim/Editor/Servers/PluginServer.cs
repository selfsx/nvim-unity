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
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace Neovim.Editor {
  public class PluginServer : IDisposable {
    private readonly ILogger _logger;
    private WebSocketServer _server;

    public PluginServer(ILogger logger) {
      _logger = logger;
    }

    public Task StartAsync(string host, int port) {
      var source = new TaskCompletionSource<object>();
      var url = $"ws://{host}:{port}";

      try {
        _server = new WebSocketServer(url);
        _server.Start();

        _logger.Info($"PluginServer. Started... {url}");
        source.SetResult(null);
      } catch (Exception e) {
        _logger.Error($"Failed to start PluginServer, {url}: {e.Message}");
        source.SetException(e);
      }

      return source.Task;
    }

    public void Dispose() {
      _server?.Stop();
      _server = null;
    }
  }
}
