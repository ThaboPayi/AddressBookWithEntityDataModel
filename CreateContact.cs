using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using AddressBookWithEntityDataModel.Models.DB;

namespace AddressBookWithEntityDataModel.Models.Manager
{
    public class CreateContact
    {
        public string AddContact(string _contactName, string _contactSurname, string _contactEmail)
        {
            string contacttCheck = "";
            using (AddressBookEntities ab = new AddressBookEntities())
            {
                ContactDetail cd = ab.ContactDetails.Where(x => x.ContactName == _contactSurname || x.ContactEmail == _contactEmail).Distinct().FirstOrDefault();
                if (cd != null)
                {
                    contacttCheck = cd.ContactName.ToString() + "Already exists !";
                }
                else
                {
                    cd = new ContactDetail
                    {
                        Id = Guid.NewGuid(),
                        ContactName = _contactName,
                        ContactSurname = _contactSurname,
                        ContactEmail = _contactEmail,
                        DateCreated = DateTime.Now
                    };

                    ab.ContactDetails.AddOrUpdate(cd);
                    ab.SaveChanges();

                    contacttCheck = cd.ContactName.ToString() + "Added Succesfully";
                }
            }
            return contacttCheck;
        }
    }
}