using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Models
{
    public class ResponseModel
    {
        public IEnumerable<ContactsModel> Contacts { get; set; }
        public int TotalNoOfContacts { get; set; }
    }
}
