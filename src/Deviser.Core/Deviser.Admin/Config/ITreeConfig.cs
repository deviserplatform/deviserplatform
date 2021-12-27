namespace Deviser.Admin.Config
{
    public interface ITreeConfig
    {
        BaseField ChildrenField { get; }
        BaseField DisplayField { get;}
        BaseField ParentField { get; }
        BaseField SortField { get; }
        string Title { get; set; }
    }
}