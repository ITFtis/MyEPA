using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using System.Collections.Generic;
using System.ComponentModel;

namespace MyEPA.Services
{
    public class VolunteerService
    {
        VolunteerRepository VolunteerRepository = new VolunteerRepository();
        public List<VolunteerModel> GetByFilter(VolunteerFilterParameter filter)
        {
            return VolunteerRepository.GetByFilter(filter);
        }
    }
}