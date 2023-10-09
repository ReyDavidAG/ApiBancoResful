using Application.Features.User.Commands;
using Application.Interfaces;
using Domain.Entities;
using FluentValidation;

namespace Application.Features.User.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator(IRepositoryAsync<AppUser> _repositoryAsync)
        {
            RuleFor(p => p.Nombre)
               .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.")
               .MaximumLength(80).WithMessage("{PropertyName} no debe de exceder de {MaxLength} caracteres.");

            RuleFor(p => p.Apellido)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.")
                .MaximumLength(80).WithMessage("{PropertyName} no debe de exceder de {MaxLength} caracteres.");

            RuleFor(p => p.Email)
               .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.")
               .EmailAddress().WithMessage("{PropertyName} debe de ser una direccion de correo valida.")
               .MaximumLength(100).WithMessage("{PropertyName} no debe de exceder de {MaxLength} caracteres.");

            RuleFor(p => p.UserName)
               .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.")
               .MaximumLength(15).WithMessage("{PropertyName} no debe de exceder de {MaxLength} caracteres.");

            RuleFor(p => p.Password)
               .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.");

            RuleFor(p => p.Role)
               .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.")
               .MaximumLength(5).WithMessage("{PropertyName} no debe de exceder de {MaxLength} caracteres.");
        }
    }
}
