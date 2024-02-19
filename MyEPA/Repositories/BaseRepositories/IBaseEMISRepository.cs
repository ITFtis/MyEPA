using System;
using System.Collections.Generic;

namespace MyEPA.Repositories.BaseRepositories
{
    public interface IBaseEMISRepository<T>
    {
        List<T> GetList();
        T Get<S>(S id);
        bool Create(T model);
        bool Update(T model);
        bool Delete<S>(S id) where S : IComparable;
    }
}