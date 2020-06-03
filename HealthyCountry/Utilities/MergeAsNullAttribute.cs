using System;

namespace HealthyCountry.Utilities
{
    public class MergeAsNullAttribute : Attribute
    {
        public bool Value;
        public MergeAsNullAttribute()
        {
            Value = true;
        }
    }
}