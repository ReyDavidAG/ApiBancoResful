using Application.Features.Clientes.Commands.CreateClienteCommand;
using Application.Interfaces;
using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clientes.Validators
{
    public class CreateClienteCommandValidator : AbstractValidator<CreateClienteCommand>
    {
        public CreateClienteCommandValidator(IRepositoryAsync<Cliente> _repositoryAsync)
        {
            RuleFor(p => p.Nombre)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.")
                .MaximumLength(80).WithMessage("{PropertyName} no debe de exceder de {MaxLength} caracteres.");

            RuleFor(p => p.Apellido)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.")
                .MaximumLength(80).WithMessage("{PropertyName} no debe de exceder de {MaxLength} caracteres.");

            RuleFor(p => p.FechaNacimiento)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.");

            RuleFor(p => p.Telefono)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.")
                .Matches(@"^\d{4}-\d{4}$").WithMessage("{PropertyName} debe de cumplir el formato 0000-0000.")
                .MaximumLength(9).WithMessage("{PropertyName} no debe de exceder de {MaxLength} caracteres.");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.")
                .EmailAddress().WithMessage("{PropertyName} debe de ser una direccion de correo valida.")
                .MaximumLength(100).WithMessage("{PropertyName} no debe de exceder de {MaxLength} caracteres.");

            RuleFor(p => p.Direccion)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.")
                .MaximumLength(120).WithMessage("{PropertyName} no debe de exceder de {MaxLength} caracteres.");
        }
    }
}


