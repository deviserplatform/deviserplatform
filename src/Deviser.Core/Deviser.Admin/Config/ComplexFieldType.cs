namespace Deviser.Admin.Config
{
    /// <summary>
    /// ComplexField can have only ManyToOne (Dropdown) or ManyToMany (MultiSelect)
    /// </summary>
    public enum ComplexFieldType
    {
        None = 0,
        ManyToOne = 3,
        ManyToMany = 4
    }
}
