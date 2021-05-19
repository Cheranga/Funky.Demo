using FluentValidation;
using Funky.Demo.Requests;

namespace Funky.Demo.Validators
{
    public class AcceptOrderRequestValidator : ModelValidatorBase<AcceptOrderRequest>
    {
        public AcceptOrderRequestValidator()
        {
            RuleFor(x => x.CorrelationId).NotNull().NotEmpty();
            RuleFor(x => x.OrderId).NotNull().NotEmpty();
            RuleFor(x => x.OrderItems).NotNull().NotEmpty();
        }
    }
}