using Application.Features.Accounts.Commands;
using Application.Interfaces;
using FluentValidation;

namespace Application.Features.Accounts.Validators;

public class JwtRequestValidator : AbstractValidator<JWTRequest>
{
    public JwtRequestValidator()
    {
        RuleFor(j => j.RefreshToken)
          .NotEmpty().WithMessage("{PropertyName} no puede estar vacio.");
    }
}
