using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AddressBookWithEntityDataModel.Models.DB;

namespace AddressBookWithEntityDataModel.Models.ViewModels
{
    public class EditModalView
    {
        public Guid ContactID { get; set; }
        public string SearchValue { get; set; }
        [Required(ErrorMessage = "Contact Name")]
        public string ContactName { get; set; }
        [Required(ErrorMessage = "Contact Surname")]
        public string ContactSurname { get; set; }
        [Required(ErrorMessage = "Contact Email")]
        public string ContactEmail { get; set; }
        public List<ContactDetail> ContactItems { get; set; }
    }
}