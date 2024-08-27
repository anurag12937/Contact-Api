using ContactsManagement.Core.Models;

namespace ContactsManagement.Core.Interfaces
{
    public interface IContactsManagementService
    {
        Task<ResponseModel> GetAsync();
        Task<bool> CreateAsync(ContactsModel contactsModel);
        Task<bool> UpdateAsync(ContactsModel contactsModel);
        Task<bool> DeleteAsync(int id);
    }
}
