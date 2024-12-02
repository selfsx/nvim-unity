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

namespace Neovim.Editor {
  /// <summary>
  /// Represents a type used to perform logging. Describes most common
  /// logging pattern based on log levels.
  /// </summary>
  public interface ILogger {
    /// <summary>
    /// Writes VERBOSE log entry (reference level 0).
    /// </summary>
    void Verbose(object message);

    /// <summary>
    /// Writes DEBUG log entry (reference level 1).
    /// </summary>
    void Debug(object message);

    /// <summary>
    /// Writes INFO log entry (reference level 2).
    /// </summary>
    void Info(object message);

    /// <summary>
    /// Writes WARNING log entry (reference level 3).
    /// </summary>
    void Warning(object message);

    /// <summary>
    /// Writes ERROR log entry (reference level 4).
    /// </summary>
    void Error(object message);
  }
}
