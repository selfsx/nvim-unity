using System.Collections.Generic;

namespace Neovim.Editor.Projects {
  internal interface IGenerator {
    bool SyncIfNeeded(IEnumerable<string> affectedFiles, IEnumerable<string> reimportedFiles,
        bool checkProjectFiles = false);

    void Sync();
    bool HasSolutionBeenGenerated();
    string SolutionFile();

    IAssemblyNameProvider AssemblyNameProvider { get; }
  }
}
