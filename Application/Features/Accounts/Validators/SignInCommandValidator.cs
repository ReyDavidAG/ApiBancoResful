using Application.Features.Acounts.Commands;
using Application.Interfaces;
using Domain.Entities;
using FluentValidation;

namespace Application.Features.Acounts.Validators
{
    public class SignInCommandValidator : AbstractValidator<SignInCommand>
    {
        public SignInCommandValidator(IRepositoryAsync<AppUser> repositoryAsync)
        {
            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("{PropertyName} es requerido.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("{PropertyName} es requerido.");
        }
    }
}
