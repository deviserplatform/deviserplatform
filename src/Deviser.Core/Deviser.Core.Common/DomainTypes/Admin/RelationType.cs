using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Common.DomainTypes.Admin
{
    public enum RelationType
    {
        None = 0,
        OneToOne = 1,
        OneToMany = 2,
        ManyToOne = 3,
        ManyToMany = 4
    }

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
