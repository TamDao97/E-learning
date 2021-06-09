using Elearning.Model.Models.Fontend.Expert;
using Elearning.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NTS.Common;

namespace Elearning.Services.Fontend.Expert
{
    public class ExpertService : IExpertService
    {
        private readonly ElearningContext _sqlContext;

        public ExpertService (ElearningContext sqlContext)
        {
            _sqlContext = sqlContext;
        }
        public async Task<List<ExpertModel>> GetExpertAsync ()
        {
            var data = (from a in _sqlContext.HomeSpecialist.AsNoTracking()
                        select new ExpertModel
                        {
                            Description=a.Description,
                            Id=a.Id,
                            Title=a.Title
                        }).ToList();
            foreach(var item in data)
            {
                var expert = (from a in _sqlContext.EmployeeSpecialist.AsNoTracking()
                              join b in _sqlContext.Employee.AsNoTracking() on a.EmployeeId equals b.Id
                              where a.HomeSpecialistId == item.Id
                              select new EmployeeSpecialistModel
                              {
                                  Avatar=a.Avartar,
                                  Description=a.Description,
                                  Name=b.Name,
                                  Facebook = a.Facebook,
                                  Lotus = a.Lotus,
                                  Twitter = a.Twitter,
                                  Instagram = a.Instagram,
                              }
                    );
                item.EmployeeSpecialistModel = expert.ToList();
            }    
            return data;
        }
    }
}
