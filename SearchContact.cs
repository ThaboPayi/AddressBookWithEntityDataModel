using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AddressBookWithEntityDataModel.Models.DB;

namespace AddressBookWithEntityDataModel.Models.Manager
{
    public class SearchContact
    {
        public List<ContactDetail> SearchContactDetails(string searchValue)
        {
            using (AddressBookEntities ab = new AddressBookEntities())
            {
                var allContacts = ab.ContactDetails.Where(m => m.ContactName.ToUpper().Contains(searchValue.ToUpper()) || m.ContactEmail.ToUpper().Contains(searchValue.ToUpper())).OrderBy(x => x.ContactName).ToList();
                return allContacts;
            }
        }
    }
}