using Application.Interfaces;
using Ardalis.Specification.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class MyRepositoryAsync<T> : RepositoryBase<T>, IRepositoryAsync<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public MyRepositoryAsync(ApplicationDbContext context) : base (context)
        {
            this._context = context;
        }
    }
}
