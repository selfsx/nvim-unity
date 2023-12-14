namespace Neovim.Editor.Projects {
  class GUIDProvider : IGUIDGenerator {
    public string ProjectGuid(string name) {
      return SolutionGuidGenerator.GuidForProject(name);
    }
  }
}
