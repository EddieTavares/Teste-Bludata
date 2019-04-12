namespace TesteCSharp_Ednilson.Models.Validation
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using TesteCSharp_Ednilson.Utils;

    public class TipoPessoaAttribute : ValidationAttribute, IClientValidatable
    {
        public TipoPessoaAttribute()
        {
            this.ErrorMessage = "The value {0} is invalid for TipoPessoa";
        }

        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext)
        {
            if (value.ToString() != "F" && value.ToString() != "J")
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));

            return null;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
            ModelMetadata metadata,
            ControllerContext context)
        {
            var modelClientValidationRule = new ModelClientValidationRule
            {
                ValidationType = "tipopessoa",
                ErrorMessage = this.FormatErrorMessage(metadata.DisplayName)
            };

            return new List<ModelClientValidationRule> { modelClientValidationRule };
        }
    }
}