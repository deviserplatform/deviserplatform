using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Deviser.Core.Common.DomainTypes
{
    public class Property
    {
        public Guid Id { get; set; }

        [BindRequired]
        public string Name { get; set; }

        [BindRequired]
        public string Label { get; set; }
        //Value property is not in DB, It is here to maintain JSON strucuture
        public string Value { get; set; }
        public string DefaultValue { get; set; }
        public string Description { get; set; }
        public Guid? OptionListId { get; set; }
        //SelectedOption property is not in DB, It is here to maintain JSON strucuture. 
        //In addtion, value will be assigned only if property is optionlist
        [JsonIgnore]
        public PropertyOption SelectedOption
        {
            get
            {
                Guid selectedOptionId;

                var option = (OptionListId != null &&
                    !string.IsNullOrEmpty(Value) &&
                    Guid.TryParse(Value, out selectedOptionId) &&
                    selectedOptionId != Guid.Empty &&
                    OptionList != null &&
                    OptionList.List != null) ? OptionList.List.FirstOrDefault(o => o.Id == selectedOptionId) : null;
                return option;
            }
        }
        
        
        public bool IsMoreOption
        {
            get
            {
                return OptionListId != null && OptionListId != Guid.Empty;
            }
        }

        public OptionList OptionList { get; set; }
        public PropertyOption DefaultValuePropertyOption { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        //public string IsActiveText => IsActive ? "Active" : "In Active";
        public string IsActiveBadgeClass => IsActive ? "badge-primary" : "badge-secondary";

        public override string ToString()
        {
            return GetPropertyValue();
        }

        public string ToString(bool returnDefault)
        {
            return GetPropertyValue(returnDefault);
        }

        private string GetPropertyValue(bool returnDefault = true)
        {
            if (!IsMoreOption)
            {
                return (!string.IsNullOrEmpty(Value) || !returnDefault) ? Value : DefaultValue;
            }

            PropertyOption propertyOption = null;
            if (!string.IsNullOrEmpty(Value))
            {
                propertyOption = OptionList?.List?.FirstOrDefault(li => li.Id == Guid.Parse(Value));
            }
            else if (!string.IsNullOrEmpty(DefaultValue) && returnDefault)
            {
                propertyOption = OptionList?.List?.FirstOrDefault(li => li.Id == Guid.Parse(DefaultValue));
            }

            return propertyOption != null ? propertyOption.Name : string.Empty;
        }
    }
}
