using FluentValidation;
using WebApiPractice.Models;
using WebApiPractice.Requests.Command;

namespace WebApiPractice.Validators
{
    public class OrderValidator : AbstractValidator<CreateOrderRequest>
    {
        public OrderValidator()
        {
            RuleFor(order => order.reference1)
                .MinimumLength(4).WithMessage("Reference 1 must have at least 1 character");           
        }
    }
}
