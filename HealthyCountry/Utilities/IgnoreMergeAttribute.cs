using System;

namespace HealthyCountry.Utilities
{
    public class IgnoreMergeAttribute : Attribute
    {
        public bool Value;
        public IgnoreMergeAttribute()
        {
            Value = true;
        }
    }
}