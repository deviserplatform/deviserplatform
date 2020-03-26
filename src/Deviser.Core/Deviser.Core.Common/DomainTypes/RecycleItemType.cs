using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Common.DomainTypes
{
    public class RecycleItemType
    {
        private static IList<RecycleItemType> _recycleItemTypes;
        public int Id { get; set; }
        public string Name { get; set; }
        //Layout = 1,
        //Page = 2,
        //PageContent = 3,
        //PageModule = 4,
        public static IList<RecycleItemType> GetRecycleItemTypes()
        {
            return _recycleItemTypes ?? new List<RecycleItemType>()
            {
                new RecycleItemType()
                {
                    Id = 1,
                    Name = "Layouts"
                },
                new RecycleItemType()
                {
                    Id = 2,
                    Name = "Page"
                },
                new RecycleItemType()
                {
                    Id = 3,
                    Name = "PageContent"
                },
                new RecycleItemType()
                {
                    Id = 4,
                    Name = "PageModule"
                }
            };
        }
    }
}
