using FastEndpoints;
using FluentValidation;

namespace PropertyControl.Features.Auth.Login
{
    public class Request
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Required")
                .MaximumLength(255);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(255);
        }
    }
}