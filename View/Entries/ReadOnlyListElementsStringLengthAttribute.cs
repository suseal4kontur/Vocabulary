using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace View.Entries
{
    public sealed class ReadOnlyListElementsStringLengthAttribute : ValidationAttribute
    {
        public int Length { get; }

        public ReadOnlyListElementsStringLengthAttribute(int length) => Length = length;
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var collection = (IReadOnlyList<string>)value;

            foreach (var element in collection)
            {
                if (element.Length > Length)
                    return new ValidationResult($"String length must be not more than {Length}");
            }

            return ValidationResult.Success;
        }
    }
}
