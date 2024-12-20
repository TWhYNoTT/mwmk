using DataAccessLayer.Data;
using Entities.Models;
using Entities.Reposatories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Implementation
{
    class UserMessageRepo : GenaricRepo<UserMessage>, IUserMessageRepo
    {
        private readonly ApplicationDbContext _context;
        public UserMessageRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
