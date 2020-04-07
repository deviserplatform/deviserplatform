using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Admin.Config
{
    public class TreeConfig : ITreeConfig
    {
        public BaseField ChildrenField { get; }
        public BaseField DisplayField { get; }

        public TreeConfig()
        {
            ChildrenField = new BaseField();
            DisplayField = new BaseField();
        }
    }
}
