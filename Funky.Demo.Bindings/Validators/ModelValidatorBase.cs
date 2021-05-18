using FluentValidation;
using FluentValidation.Results;

namespace Funky.Demo.Validators
{
    public abstract class ModelValidatorBase<TModel> : AbstractValidator<TModel>
    {
        protected ModelValidatorBase()
        {
            CascadeMode = CascadeMode.Stop;
        }
        
        protected override bool PreValidate(ValidationContext<TModel> context, ValidationResult result)
        {
            var instance = context.InstanceToValidate;

            if (instance != null)
            {
                return true;
            }
            
            result.Errors.Add(new ValidationFailure("Instance", "Instance is null"));
            return false;
        }
    }
}