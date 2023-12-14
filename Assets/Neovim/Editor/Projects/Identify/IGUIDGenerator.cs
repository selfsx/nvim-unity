namespace Neovim.Editor.Projects {
  internal interface IGUIDGenerator {
    string ProjectGuid(string name);
  }
}
