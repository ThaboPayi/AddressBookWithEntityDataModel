using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AddressBookWithEntityDataModel.Models.DB;

namespace AddressBookWithEntityDataModel.Models.Manager
{
    public class GetContacts
    {
        public List<ContactDetail> PullContacts()
        {
            using (AddressBookEntities ab = new AddressBookEntities())
            {
                var allContacts = ab.ContactDetails.Where(m => m.ContactName != null || m.ContactEmail != null).OrderBy(x => x.ContactName).ToList();
                return allContacts;
            }
        }
    }
}