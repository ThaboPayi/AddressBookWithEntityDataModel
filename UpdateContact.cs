using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using AddressBookWithEntityDataModel.Models.DB;
using AddressBookWithEntityDataModel.Models.ViewModels;

namespace AddressBookWithEntityDataModel.Models.Manager
{
    public class UpdateContact
    {
        public string SaveEditContact(EditModalView viewModel)
        {
            string contactCheck = "";
            using (AddressBookEntities ab = new AddressBookEntities())
            {
                ContactDetail cd = ab.ContactDetails.Where(x => x.Id == viewModel.ContactID).Distinct().FirstOrDefault();
                if (cd != null)
                {

                    cd = new ContactDetail
                    {
                        Id = viewModel.ContactID,
                        ContactName = viewModel.ContactName,
                        ContactSurname = viewModel.ContactSurname,
                        ContactEmail = viewModel.ContactEmail,
                        DateCreated = DateTime.Now
                    };

                    ab.ContactDetails.AddOrUpdate(cd);
                    ab.SaveChanges();
                }
                else
                {
                    contactCheck =  "There is no such !";

                }

            }
            return contactCheck;
        }
    }
}