using System;
using System.ComponentModel.DataAnnotations;

namespace View.Meanings
{
    public sealed class PartOfSpeechAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var valueAsString = (string)value;

            if (value != null && !Enum.TryParse(typeof(PartOfSpeech), valueAsString, true, out object result))
            {
                return new ValidationResult($"There's no part of speech named {valueAsString}.");
            }

            return ValidationResult.Success;
        }
    }
}
