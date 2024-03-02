using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.UI.Services;
using MongoDB.Bson;
using Repositories.Model;
using Repositories.Models;
using Repositories.ModelView;
using Repositories.Repository;
using Services.Interface;
using Services.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static Repositories.ModelView.ContactView;

namespace Services.Service
{
    public class ContactService : IContactService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        public ContactService(IUnitOfWork unit, IMapper mapper, IEmailSender emailSender)
        {
            _unit = unit;
            _mapper = mapper;
            _emailSender = emailSender;
        }

        public async Task<string> AddContact(AddContactView add)
        {
            List<byte[]> picturesBytesList = new List<byte[]>();
            if (add.Pictures != null)
            {
                if (add.Pictures.Length > 0)
                {
                    foreach (var picture in add.Pictures)
                    {
                        //Encode picture
                        using (var ms = new MemoryStream())
                        {
                            await picture.CopyToAsync(ms);
                            byte[] fileBytes = ms.ToArray();
                            picturesBytesList.Add(fileBytes);
                        }
                    }
                }
            }
            else picturesBytesList = [];
            Contact contact = _mapper.Map<Contact>(add);
            contact.Pictures = picturesBytesList;
            contact.Status = Contact.State.Processing;
            await _unit.ContactRepo.AddOneItem(contact);
            return "The contact have been sent";
        }

        public async Task<string> AddressTheContact(AddressContactView address)
        {
            var getContact = (await _unit.ContactRepo.GetFieldsByFilterAsync([],
                    g => g.ContactId.Equals(address.ContactId))).FirstOrDefault();
            if (getContact != null)
            {
                getContact.ResponseOfStaff = address.ResponseOfStaff;
                getContact.Status = Contact.State.Completed;
                getContact.UpdatedAt = DateTime.UtcNow;
                await _unit.ContactRepo.UpdateItemByValue("ContactId", getContact.ContactId, getContact);
                string subject = "Interior quotation system";
                string body = $"<h3><strong>{address.ResponseOfStaff}</strong></h3>";
                await _emailSender.SendEmailAsync(getContact.Email, subject, body);
                return $"You have addressed the contact of email: {getContact.Email}";
            }
            return "The contact is not existed";
        }

        public async Task<string> DeleteContact(DeleteContactView delete)
        {
            var getContact = (await _unit.ContactRepo.GetFieldsByFilterAsync([],
                    g => g.ContactId.Equals(delete.ContactId))).FirstOrDefault();
            if (getContact != null && getContact.Status == Contact.State.Completed)
            {
                await _unit.ContactRepo.RemoveItemByValue("ContactId", getContact.ContactId);
                return "Delete the contact successfully";
            }
            return "The Contact does not exist or the Contact in progress cannot be deleted";
        }

        public async Task<object> GetPagingContact(PagingContactView paging)
        {
            const int pageSize = 5;
            const string sortField = "CreatedAt";
            List<string> searchFields = ["Email", "Title"];
            List<string> returnFields = ["Email", "Title", "Status", "CreatedAt"];

            int skip = (paging.PageIndex - 1) * pageSize;
            var items = (await _unit.ContactRepo.PagingAsync(skip, pageSize, paging.IsAsc, sortField, paging.SearchValue, searchFields, returnFields)).ToList();
            var responses = new List<object>();

            foreach (var item in items)
            {
                responses.Add(new 
                { 
                    ContactId = item.ContactId,
                    Email = item.Email, 
                    Title = item.Title,
                    Status = item.Status,
                    CreatedAt = item.CreatedAt
                });
            }
            return responses;
        }

        public async Task<object?> GetContactDetail(DetailContactView detail)
        {
            var getContact = (await _unit.ContactRepo.GetFieldsByFilterAsync([],
                    g => g.ContactId.Equals(detail.ContactId))).FirstOrDefault();
            if (getContact != null)
            {
                var pictures = new List<byte[]>();
                if (getContact.Pictures != null)
                {
                    foreach (var picture in getContact.Pictures)
                    {
                        var getPicture = SomeTool.GetImage(Convert.ToBase64String(picture));
                        pictures.Add(getPicture!);
                    }
                    getContact.Pictures = pictures;
                }
                return getContact;
            }
            return null;
        }
    }
}
