﻿using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AddressBookWithEntityDataModel.Models.DB;
using AddressBookWithEntityDataModel.Models.Manager;
using AddressBookWithEntityDataModel.Models.ViewModels;

namespace AddressBookWithEntityDataModel.Controllers
{
    public class HomeController : Controller
    {
        private GetContacts _getContacts;
        private CreateContact _createContact;
        private UpdateContact _updateContact;
        private SearchContact _searchContact;
        private DeleteContact _deleteContact;
        public HomeController()
        {
            _getContacts = new GetContacts();
            _createContact = new CreateContact();
            _updateContact = new UpdateContact();
            _searchContact = new SearchContact();
            _deleteContact = new DeleteContact();
        }
        public ActionResult Index(ContactDetails cd)
        {
            cd = ReLoad(cd);

            return View(cd);
        }
        [HttpPost]
        public ActionResult AddContact(string btnAddContact, string btnCancel, string cName, string cSurname, string cEmail)
        {
            //Sending new values to a model
            string contactName = cName;
            string contactSurname = cSurname;
            string contactEmail = cEmail;

            ContactDetails cd;

            if (!string.IsNullOrEmpty(btnAddContact))
            {
                string checkMessage = "";

                //storing in DB
                checkMessage = _createContact.AddContact(contactName, contactSurname, contactEmail);
                ViewBag.Message = checkMessage;
            }

            cd = new ContactDetails();
            cd = ReLoad(cd);

            return PartialView("_ContactsList", cd);
        }
        [HttpPost]
        public ActionResult AddViewPartial()
        {
            return PartialView("_CreateContact");
        }
        [HttpPost]
        public ActionResult SearchTable(string searchValue)
        {
            ContactDetails cd = new ContactDetails();
            if (!string.IsNullOrEmpty(searchValue))
            {
                using (AddressBookEntities ab = new AddressBookEntities())
                {
                    var contacts = _searchContact.SearchContactDetails(searchValue).Select(x => new ContactDetail
                    {
                        Id = x.Id,
                        ContactName = x.ContactName,
                        ContactSurname = x.ContactSurname,
                        ContactEmail = x.ContactEmail
                    }).ToList();

                    cd = new ContactDetails
                    {
                        ContactItems = contacts,
                        PageNo = 1,
                        ItemsPerPage = 10
                    };
                }
            }
            else
            {
                cd = ReLoad(cd);
            }
            return PartialView("_ContactsList", cd);
        }
        [HttpPost]
        public ActionResult EditContactView(Guid contactId)
        {
            using (AddressBookEntities ab = new AddressBookEntities())
            {
                //Obtaining specific contact values to prepopulate the fields
                ContactDetail cd = ab.ContactDetails.FirstOrDefault(x => x.Id == contactId);

                EditModalView emdv = new EditModalView();
                emdv = new EditModalView
                {
                    ContactID = contactId,
                    ContactName = cd.ContactName,
                    ContactSurname = cd.ContactSurname,
                    ContactEmail = cd.ContactEmail
                };
                return PartialView("_UpdateContact", emdv);
            }
        }
        [HttpPost]
        public ActionResult SaveEditContact(EditModalView viewModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { isValid = false, view = RenderViewAsString("UpdateContact", viewModel) });
            }

            string checkMessage = "";

            //storing in DB
            checkMessage = _updateContact.SaveEditContact(viewModel);
            ViewBag.Message = checkMessage;

            ContactDetails cd = new ContactDetails();
            cd = ReLoad(cd);

            return Json(new { isValid = true, view = RenderViewAsString("_ContactsList", cd) });
        }
        [HttpPost]
        public ActionResult DeleteContact(Guid contactId)
        {
            ContactDetails cd = new ContactDetails();
            if (contactId == Guid.Empty)
            {

                cd.ItemsPerPage = cd.ItemsPerPage != 0 ? cd.ItemsPerPage : 10;
                var contacts = _getContacts.PullContacts().OrderBy(x => x.ContactName).ToList();

                cd = new ContactDetails
                {
                    ContactItems =
                        contacts
                            .OrderBy(p => p.ContactName)
                            .Skip(((cd.PageNo == 0 ? 1 : cd.PageNo) - 1) * (cd.ItemsPerPage == 0 ? 10 : cd.ItemsPerPage))
                            .Take(cd.ItemsPerPage == 0 ? 10 : cd.ItemsPerPage).ToList(),

                    ItemsPerPage = 10,
                    NoPages = contacts.Distinct().Count() / (cd.ItemsPerPage == 0 ? 10 : cd.ItemsPerPage) + (contacts.Distinct().Count() % (cd.ItemsPerPage == 0 ? 10 : cd.ItemsPerPage) > 0 ? 1 : 0)
                };
            }
            else
            {
                string checkMessage = "";
                //Remove in DB
                checkMessage = _deleteContact.DeleteContactDetails(contactId);
                ViewBag.Message = checkMessage;

                cd = ReLoad(cd);
            }
            return PartialView("_ContactsList", cd);
        }
        private string RenderViewAsString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                    viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        private ContactDetails ReLoad(ContactDetails contactDetails)
        {
            ContactDetails cd = new ContactDetails();


            cd.ItemsPerPage = cd.ItemsPerPage != 0 ? cd.ItemsPerPage : 10;
            var contacts = _getContacts.PullContacts();

            cd = new ContactDetails
            {
                ContactItems =
                    contacts
                        .OrderBy(p => p.ContactName)
                        .Skip(((cd.PageNo == 0 ? 1 : cd.PageNo) - 1) * (cd.ItemsPerPage == 0 ? 10 : cd.ItemsPerPage))
                        .Take(cd.ItemsPerPage == 0 ? 10 : cd.ItemsPerPage).ToList(),

                ItemsPerPage = 10,
                NoPages = contacts.Distinct().Count() / (cd.ItemsPerPage == 0 ? 10 : cd.ItemsPerPage) + (contacts.Distinct().Count() % (cd.ItemsPerPage == 0 ? 10 : cd.ItemsPerPage) > 0 ? 1 : 0)
            };
            return cd;
        }
    }
}