namespace Phonebook.Data.Repositories.Base
{
    public interface IRepositoryManager
    {
        IContactsRepository Contacts { get; }
        void Save();
    }
}