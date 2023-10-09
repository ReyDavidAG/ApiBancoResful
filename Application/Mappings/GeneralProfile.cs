using Application.Features.Clientes.Commands;
using Application.Features.Clientes.Queries;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region Dtos
            CreateMap<Cliente, ClienteDto>();
            CreateMap<AppUser, UserDatosDto>();
            #endregion
            #region Commands
            CreateMap<CreateClienteCommand, Cliente>(); 
            #endregion
        }
    }
}
