using System.Threading.Tasks;

namespace Phonebook.Services.Base
{
    public interface ICacheService<T>
    {
        Task<T> Get(int id);
        Task Set(T content);
    }
}