using ContactsManagement.Core.Models;
using ContactsManagement.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Moq;
using Newtonsoft.Json;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace ContactsManagement.Core.Tests.Service
{
    public class ContactsManagementServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IHostEnvironment> _mockHostingEnvironment;
        private readonly ContactsManagementService _contactService;

        public ContactsManagementServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockHostingEnvironment = new Mock<IHostEnvironment>();
            _contactService = new ContactsManagementService(_mockConfiguration.Object,_mockHostingEnvironment.Object);
        }
        [Fact]
        public async Task GetAsync_ReturnsAllContacts()
        {
            // Arrange
            var contactList = new List<ContactsModel>
            {
                new ContactsModel() { Id = 1, FirstName = "Rsys", LastName = "NOID", Email = "tech.doe@example.com" }
            };

            _mockHostingEnvironment.Setup(env => env.ContentRootPath).Returns(Directory.GetCurrentDirectory());
            _mockConfiguration.Setup(cfg => cfg["DataFileName"]).Returns("contacts.json");

            var json = JsonConvert.SerializeObject(contactList);
            File.WriteAllText("contacts.json", json);

            // Act
            var result = await _contactService.GetAsync();

            // Assert
            Assert.Equal(1, result.TotalNoOfContacts);
            Assert.Single(result.Contacts);
            Assert.Equal("NOID", result.Contacts.First().FirstName);
        }
        [Fact]
        public async Task CreateAsync_AddsNewContact()
        {
            // Arrange
            var initialContacts = new List<ContactsModel>
            {
                new ContactsModel() { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" }
            };

            var newContact = new ContactsModel() { FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" };

            _mockHostingEnvironment.Setup(env => env.ContentRootPath).Returns(Directory.GetCurrentDirectory());
            _mockConfiguration.Setup(cfg => cfg["DataFileName"]).Returns("contacts.json");

            File.WriteAllText("contacts.json", JsonConvert.SerializeObject(initialContacts));

            // Act
            var result = await _contactService.CreateAsync(newContact);

            // Assert
            Assert.True(result);

            var updatedContacts = JsonConvert.DeserializeObject<List<ContactsModel>>(File.ReadAllText("contacts.json"));
            Assert.Equal(2, updatedContacts.Count);
            Assert.Equal("Jane", updatedContacts.Last().FirstName);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingContact()
        {
            // Arrange
            var contacts = new List<ContactsModel>
            {
                new ContactsModel() { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" }
            };

            var updatedContact = new ContactsModel() { Id = 1, FirstName = "Johnny", LastName = "Doe", Email = "johnny.doe@example.com" };

            _mockHostingEnvironment.Setup(env => env.ContentRootPath).Returns(Directory.GetCurrentDirectory());
            _mockConfiguration.Setup(cfg => cfg["DataFileName"]).Returns("contacts.json");

            File.WriteAllText("contacts.json", JsonConvert.SerializeObject(contacts));

            // Act
            var result = await _contactService.UpdateAsync(updatedContact);

            // Assert
            Assert.True(result);

            var updatedContacts = JsonConvert.DeserializeObject<List<ContactsModel>>(File.ReadAllText("contacts.json"));
            var contact = updatedContacts.FirstOrDefault(c => c.Id == 1);
            Assert.NotNull(contact);
            Assert.Equal("Johnny", contact.FirstName);
        }
        [Fact]
        public async Task DeleteAsync_RemovesContact()
        {
            // Arrange
            var contacts = new List<ContactsModel>
            {
                new ContactsModel() { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
                new ContactsModel() { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" }
            };

            _mockHostingEnvironment.Setup(env => env.ContentRootPath).Returns(Directory.GetCurrentDirectory());
            _mockConfiguration.Setup(cfg => cfg["DataFileName"]).Returns("contacts.json");

            File.WriteAllText("contacts.json", JsonConvert.SerializeObject(contacts));

            // Act
            var result = await _contactService.DeleteAsync(1);

            // Assert
            Assert.True(result);

            var remainingContacts = JsonConvert.DeserializeObject<List<ContactsModel>>(File.ReadAllText("contacts.json"));
            Assert.Single(remainingContacts);
            Assert.DoesNotContain(remainingContacts, c => c.Id == 1);
        }
    }
}
