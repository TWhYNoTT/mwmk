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
    public class MarketerRepo:GenaricRepo<Marketer>, IMarketerRepo
    {
        private readonly ApplicationDbContext _context;
        public MarketerRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public Marketer GetById(int userid)
        {
            return _context.Marketers.FirstOrDefault(x => x.Id==userid);
        }
    }
}
