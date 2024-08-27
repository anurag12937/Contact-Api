using ContactsManagement.Core.Interfaces;
using ContactsManagement.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace ContactsManagement.Core.Services
{
    public class ContactsManagementService : IContactsManagementService
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _env;
        public ContactsManagementService(IConfiguration configuration, IHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
        public async Task<ResponseModel> GetAsync()
        {
            IEnumerable<ContactsModel> contacts = new List<ContactsModel>();
            try
            {
                //combine the content root path with that of our json file
                var fullPath = Path.Combine(GetContentRootPath(), _configuration["DBFile"]);

                using var rdReader = new StreamReader(fullPath);
                var json = rdReader.ReadToEnd();
                if (!string.IsNullOrWhiteSpace(json))
                    contacts = JsonConvert.DeserializeObject<IEnumerable<ContactsModel>>(json);

                if (!contacts.Any()) return null;
                var result = new ResponseModel
                {
                    TotalNoOfContacts = contacts.Count(),
                    Contacts = contacts
                };
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Insert new record for contact management
        /// ContactsModel
        /// </summary>
        /// <returns>bool</returns>
        public async Task<bool> CreateAsync(ContactsModel contactsModel)
        {
            try
            {
                var contacts = await GetAllContacts();
                if (contacts is not null && contacts.Any())
                {
                    contactsModel.Id = contacts.LastOrDefault().Id + 1;
                    var contactsToSave = contacts.Append(contactsModel);
                    return await SaveAsync(contactsToSave);
                }
                else
                {
                    // first insertion
                    contactsModel.Id = 1;
                    contacts = new List<ContactsModel>() { contactsModel };
                    return await SaveAsync(contacts);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
       
        /// <summary>
        /// Update existing record for contact management
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(ContactsModel contactsModel)
        {
            try
            {
                var contacts = await GetAllContacts();
                if (contacts is not null && contacts.Any())
                {
                    var updateContactsModel = contacts.FirstOrDefault(c => c.Id == contactsModel.Id);
                    if (updateContactsModel != null)
                    {
                        updateContactsModel.FirstName = contactsModel.FirstName;
                        updateContactsModel.LastName = contactsModel.LastName;
                        updateContactsModel.Email = contactsModel.Email;

                        var updatedModels = contacts.Select(x => x.Id == updateContactsModel.Id ? updateContactsModel : x);
                        return await SaveAsync(updatedModels);
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Delete Contact Management data
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var contacts = await GetAllContacts();
                if (contacts is null || !contacts.Any())
                    return false;
                else
                {
                    var excepts = contacts.Where(x => x.Id != id);


                    return await SaveAsync(excepts);
                }
            }
            catch (Exception)
            {
                return false;
            }

        }
        public string GetContentRootPath()
        {
            return _env.ContentRootPath;
        }
        public async Task<IEnumerable<ContactsModel>> GetAllContacts()
        {
             List<ContactsModel> contactsList;
            var fullPath = Path.Combine(GetContentRootPath(), _configuration["DBFile"]);

            using var rdReader = new StreamReader(fullPath);
            var json = rdReader.ReadToEnd();
            if (!string.IsNullOrWhiteSpace(json))
                contactsList = (List<ContactsModel>)JsonConvert.DeserializeObject<IEnumerable<ContactsModel>>(json);
            else
            {
                contactsList = new List<ContactsModel>();
            }
            return contactsList;
        }
        public async Task<bool> SaveAsync(IEnumerable<ContactsModel> contactsModels)
        {
            var fullPath = Path.Combine(GetContentRootPath(), _configuration["DBFile"]);
            using var streamWriter = new StreamWriter(fullPath);
            streamWriter.Write(JsonConvert.SerializeObject(contactsModels));
            return true;
        }
    }
}
