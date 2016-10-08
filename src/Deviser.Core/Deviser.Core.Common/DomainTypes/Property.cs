using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Deviser.Core.Common.DomainTypes
{
    public class Property
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        //Value property is not in DB, It is here to maintain JSON strucuture
        public string Value { get; set; }
        public Guid? PropertyOptionListId { get; set; }
        //SelectedOption property is not in DB, It is here to maintain JSON strucuture. 
        //In addtion, value will be assigned only if property is optionlist
        [JsonIgnore]
        public PropertyOption SelectedOption
        {
            get
            {
                Guid selectedOptionId;

                var option = (PropertyOptionListId != null &&
                    !string.IsNullOrEmpty(Value) &&
                    Guid.TryParse(Value, out selectedOptionId) &&
                    selectedOptionId != Guid.Empty &&
                    PropertyOptionList != null &&
                    PropertyOptionList.List != null) ? PropertyOptionList.List.FirstOrDefault(o => o.Id == selectedOptionId):null;
                return option;
            }
        }
        [JsonIgnore]
        public bool IsMoreOption
        {
            get
            {
                return PropertyOptionListId != null && PropertyOptionListId != Guid.Empty;
            }
        }

        public PropertyOptionList PropertyOptionList { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
