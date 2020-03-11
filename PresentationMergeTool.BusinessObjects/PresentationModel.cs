namespace PresentationMergeTool.BusinessObjects
{
  public class PresentationModel
  {
    public PresentationModel(string fullPath, string name)
    {
      this.FullPath = fullPath;
      this.Name = name;
    }

    public string FullPath { get; }
    public string Name { get; }

    public override bool Equals(object obj)
    {
      return obj is PresentationModel item && string.Equals(item.FullPath, this.FullPath);
    }

    public override int GetHashCode()
    {
      if (this.FullPath == null)
      {
        return 0;
      }

      return this.FullPath.GetHashCode();
    }
  }
}