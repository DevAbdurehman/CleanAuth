using CleanAuth.Application.Interfaces.Repositories;
using CleanAuth.Domain.Entities;
using CleanAuth.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAuth.Infrastructure.Repositories
{
    public  class UserRepositories: IUserRepository
    {
        private readonly ApplicationDBContext _context;

        public UserRepositories(ApplicationDBContext context) { 

              _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);

        }
          public async Task<User?> GetEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

    }
