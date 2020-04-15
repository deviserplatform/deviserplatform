namespace Deviser.Admin.Config
{
    public interface ITreeConfig
    {
        BaseField ChildrenField { get; }
        BaseField DisplayField { get;}
        BaseField SortField { get; }
    }
}