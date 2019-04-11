using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using AddressBookWithEntityDataModel.Models.DB;
using AddressBookWithEntityDataModel.Models.ViewModels;

namespace AddressBookWithEntityDataModel.Models.Manager
{
    public class DeleteContact
    {
        public string DeleteContactDetails(Guid contactId)
        {
            string contactCheck = "";
            using (AddressBookEntities ab = new AddressBookEntities())
            {
                ContactDetail cd = ab.ContactDetails.Where(x => x.Id == contactId).Distinct().FirstOrDefault();
                if (cd != null)
                {
                    ab.ContactDetails.Remove(cd);
                    ab.SaveChanges();
                    contactCheck = "Contact Deleted !";
                }
                else
                {
                    contactCheck = "There is no such !";

                }
            }
            return contactCheck;
        }
    }
}