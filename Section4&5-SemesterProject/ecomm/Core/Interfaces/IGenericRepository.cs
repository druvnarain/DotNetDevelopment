using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    //Restricts Generic to accept classes BaseEntity and derivations
    public interface IGenericRepository<T> where T : BaseEntity
    {
         Task<T> GetByIdAsync(int id);
         Task<IReadOnlyList<T>> ListAllAsync();
    }
}