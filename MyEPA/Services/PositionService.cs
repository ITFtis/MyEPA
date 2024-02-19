using MyEPA.Models;
using MyEPA.Repositories;
using System.Collections.Generic;

namespace MyEPA.Services
{
    public class PositionService
    {
        PositionRepository PositionRepository = new PositionRepository();
        public List<PositionModel> GetAll()
        {
            return PositionRepository.GetList();
        }
    }
}