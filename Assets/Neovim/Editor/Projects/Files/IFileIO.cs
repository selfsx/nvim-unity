namespace Neovim.Editor.Projects {
  internal interface IFileIO {
    bool Exists(string fileName);

    string ReadAllText(string fileName);
    void WriteAllText(string path, string content);

    string EscapedRelativePathFor(string file, string projectDirectory);
  }
}
