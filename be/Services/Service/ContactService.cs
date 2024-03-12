using Amazon.Runtime.Internal.Util;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
using QuestPDF.Infrastructure;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using QuestPDF;
using QuestPDF.Drawing;
using System.IO;
using QuestPDF.Fluent;
using QuestPDF.Previewer;
using QuestPDF.Helpers;
using static Repositories.ModelView.ContractView;
namespace Services.Service
{
    public class ContactService : IContactService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IOptions<MailSettings> _mailSettings;
        private readonly ILogger<SendEmailTool> _logger;
        public ContactService(IUnitOfWork unit, IMapper mapper, IEmailSender emailSender, IOptions<MailSettings> mailSettings, ILogger<SendEmailTool> logger)
        {
            _unit = unit;
            _mapper = mapper;
            _emailSender = emailSender;
            _logger = logger;
            _mailSettings = mailSettings;
        }

        public async Task<(bool, string)> AddContactForGuest(string interiorId, AddContactView add)
        {
            if (add.Phone.Length == 10)
            {
                if (add.Picture.Length > 0)
                {
                    //Encode picture
                    byte[] fileBytes;
                    using (var ms = new MemoryStream())
                    {
                        await add.Picture.CopyToAsync(ms);
                        fileBytes = ms.ToArray();
                    }
                    List<string> interiorIdList = [];
                    interiorIdList.Add(interiorId);
                    Contact contact = _mapper.Map<Contact>(add);
                    contact.InteriorId = interiorIdList;
                    contact.Picture = fileBytes;
                    contact.StatusOfContact = Contact.StateContact.Processing;
                    await _unit.ContactRepo.AddOneItem(contact);
                    return (true, "The contact have been sent");
                }
                return (false, "Missing the picture");
            }
            return (false, "Phone number is not valid");
        }

        public async Task<(bool, string)> AddContactForCustomer(string id, string interiorId, AddForCustomerContactView add)
        {
            var getUser = (await _unit.AccountRepo.GetFieldsByFilterAsync([],
                            c => c.AccountId.Equals(id))).First();
            if (getUser.PhoneNumber != null)
            {
                if (getUser.Address != null)
                {
                    if (add.Picture.Length > 0)
                    {
                        //Encode picture
                        byte[] fileBytes;
                        using (var ms = new MemoryStream())
                        {
                            await add.Picture.CopyToAsync(ms);
                            fileBytes = ms.ToArray();
                        }
                        List<string> interiorIdList = [];
                        interiorIdList.Add(interiorId);
                        Contact contact = _mapper.Map<Contact>(add);
                        contact.Email = getUser.Email;
                        contact.Phone = getUser.PhoneNumber;
                        contact.Address = getUser.Address;
                        contact.InteriorId = interiorIdList;
                        contact.Picture = fileBytes;
                        contact.StatusOfContact = Contact.StateContact.Processing;
                        await _unit.ContactRepo.AddOneItem(contact);
                        return (true, "The contact have been sent");
                    }
                    return (false, "Missing the picture");
                }
                return (false, "Address is empty");
            }
            return (false, "Phone number is not valid");
        }

        public async Task<(bool, string)> AddressTheContact(AddressContactView address)
        {
            var getContact = (await _unit.ContactRepo.GetFieldsByFilterAsync([],
                    g => g.ContactId.Equals(address.ContactId))).FirstOrDefault();
            if (getContact != null)
            {
                if (address.StatusResponseOfStaff == Contact.State.Completed)
                {
                    getContact.ResponseOfStaff = address.ResponseOfStaff;
                    getContact.StatusResponseOfStaff = address.StatusResponseOfStaff;
                    getContact.StatusOfContact = Contact.StateContact.Completed;
                    getContact.UpdatedAt = DateTime.UtcNow;
                    await _unit.ContactRepo.UpdateItemByValue("ContactId", getContact.ContactId, getContact);
                    string subject = "Interior quotation system";
                    string body = $@"
                    <h3><strong>
                        {address.ResponseOfStaff}
                    </strong></h3>";
                    await _emailSender.SendEmailAsync(getContact.Email, subject, body);
                    return (true, $"You have addressed the contact of email: {getContact.Email}");
                }
                else if (address.StatusResponseOfStaff == Contact.State.Awaiting_Payment)
                {
                    getContact.ResponseOfStaff = address.ResponseOfStaff;
                    getContact.StatusResponseOfStaff = address.StatusResponseOfStaff;
                    getContact.StatusOfContact = Contact.StateContact.Processing;
                    getContact.UpdatedAt = DateTime.UtcNow;
                    await _unit.ContactRepo.UpdateItemByValue("ContactId", getContact.ContactId, getContact);
                    string subject = "Interior quotation system";
                    string body = $@"
                    <h3><strong>
                        {address.ResponseOfStaff}
                    </strong></h3>";
                    SendEmailTool sendEmail = new SendEmailTool(_mailSettings, _logger);
                    await sendEmail.SendEmailWithPdfAsync(getContact.Email, subject, body, address.File);
                    return (true, $"You have addressed the contact of email: {getContact.Email}");
                }
            }
            return (false, "The contact is not existed");
        }

        //public async Task<(bool, string)> DeleteContact(DeleteContactView delete)
        //{
        //    var getContact = (await _unit.ContactRepo.GetFieldsByFilterAsync([],
        //            g => g.ContactId.Equals(delete.ContactId))).FirstOrDefault();
        //    if (getContact != null && getContact.StatusOfStaff == Contact.State.Completed)
        //    {
        //        await _unit.ContactRepo.RemoveItemByValue("ContactId", getContact.ContactId);
        //        return (true, "Delete the contact successfully");
        //    }
        //    return (false, "The Contact does not exist or the Contact in progress cannot be deleted");
        //}

        //public async Task<object> GetPagingContact(PagingContactView paging)
        //{
        //    const int pageSize = 5;
        //    const string sortField = "CreatedAt";
        //    List<string> searchFields = ["Email", "Title"];
        //    List<string> returnFields = ["Email", "Title", "Status", "CreatedAt"];

        //    int skip = (paging.PageIndex - 1) * pageSize;
        //    var items = (await _unit.ContactRepo.PagingAsync(skip, pageSize, paging.IsAsc, sortField, paging.SearchValue, searchFields, returnFields)).ToList();
        //    var responses = new List<object>();

        //    foreach (var item in items)
        //    {
        //        responses.Add(new
        //        {
        //            ContactId = item.ContactId,
        //            Email = item.Email,
        //            Title = item.Title,
        //            Status = item.StatusOfStaff,
        //            CreatedAt = item.CreatedAt
        //        });
        //    }
        //    return responses;
        //}

        //public async Task<(bool, object)> GetContactDetail(DetailContactView detail)
        //{
        //    var getContact = (await _unit.ContactRepo.GetFieldsByFilterAsync([],
        //            g => g.ContactId.Equals(detail.ContactId))).FirstOrDefault();
        //    if (getContact != null)
        //    {
        //        var pictures = new List<byte[]>();
        //        if (getContact.Pictures != null)
        //        {
        //            foreach (var picture in getContact.Pictures)
        //            {
        //                var getPicture = SomeTool.GetImage(Convert.ToBase64String(picture));
        //                pictures.Add(getPicture!);
        //            }
        //            getContact.Pictures = pictures;
        //        }
        //        return (true, getContact);
        //    }
        //    return (false, "Contact is not existed");
        //}

        //private string GetImageTags(List<byte[]> picturesBytesList)
        //{
        //    StringBuilder imageTags = new StringBuilder();

        //    foreach (var pictureBytes in picturesBytesList)
        //    {
        //        // Chuyển đổi byte[] thành base64 string
        //        string base64Image = Convert.ToBase64String(pictureBytes);

        //        // Tạo thẻ <img> với base64 string
        //        string imgTag = $"<img src='data:image/png;base64,{base64Image}' alt='Interior Product' />";
        //        imageTags.AppendLine(imgTag);
        //    }

        //    return imageTags.ToString();
        //}

        private async Task<(bool, object)> GenerateContractPdf(string staffId, string contactId, ArrayInterior[] array)
        {
            var totalPrice = 0;
            var getContact = (await _unit.ContactRepo.GetFieldsByFilterAsync([],
                    g => g.ContactId.Equals(contactId))).FirstOrDefault();
            if (getContact != null)
            {
                List<ContractViewList> list = [];
                foreach (var item in array)
                {
                    int count = 0;
                    var getInterior = (await _unit.InteriorRepo.GetFieldsByFilterAsync([],
                        g => g.InteriorId.Equals(item.InteriorId))).FirstOrDefault();
                    if (getInterior != null)
                    {
                        if (item.Quantity <= getInterior.Quantity)
                        {
                            ContractViewList contract = new()
                            {
                                ProductId = item.InteriorId,
                                ProductName = getInterior.InteriorName,
                                Quantity = item.Quantity,
                                UnitPrice = getInterior.Price,
                                TotalCostOfPoduct = getInterior.Price * item.Quantity
                            };
                            list.Add(contract);
                            count++;
                            if (count == array.Length) break;
                        }
                        return (false, "The quantity of product available is less than the quantity you want");
                    }
                    return (false, $"The product with ID: {item.InteriorId} does not exist");
                }
                QuestPDF.Settings.License = LicenseType.Community;
                Document.Create(container =>
                {
                    container.Page(p =>
                    {
                        p.Size(PageSizes.A4);
                        p.Margin(3, Unit.Centimetre);
                        p.PageColor(Colors.White);
                        p.DefaultTextStyle(x => x.FontSize(20));

                        p.Header().Text("Contract")
                                  .SemiBold().FontSize(30).FontColor(Colors.Black);

                        p.Content().PaddingVertical(1, Unit.Centimetre)
                                    .Column(x =>
                                    {
                                        x.Spacing(20);
                                        x.Item().Text("Company Name: Interior Quote System");
                                        x.Item().Text($"Creation Date: {DateTime.UtcNow}");
                                        x.Item().Text("Company Address: Thu Duc district, Ho Chi Minh city");
                                        x.Item().Text("Company Phone: 0123456789");
                                        x.Item().Text($"Buyer Address: {getContact.Address}");
                                        x.Item().Text($"Buyer Phone: {getContact.Phone}");
                                        x.Item().Text($"Buyer Email: {getContact.Email}");
                                        foreach (var item in list)
                                        {
                                            totalPrice = +item.TotalCostOfPoduct;
                                            x.Item().Text($"InteriorId: {item.ProductId}, Name: {item.ProductName}, Unit Price: {item.UnitPrice}, Quantity: {item.Quantity}, Total of product: {item.TotalCostOfPoduct}");
                                        }
                                        x.Item().Text("Tax: 10%");
                                        x.Item().Text("Ship: 100.000");
                                        x.Item().Text($"The total amount includes the above fee items: {totalPrice + totalPrice * 0.1 + 100000}");
                                    });
                    });
                })
                    .GeneratePdf("Contract.pdf");
                Contract ct = new()
                {
                    ContractId = ObjectId.GenerateNewId().ToString(),
                    EmailOfCustomer = getContact.Email,
                    StaffId = staffId,
                    CreatedAt = DateTime.UtcNow,
                    Description = $@"
                                    Company Name: Interior Quote System
                                    Creation Date: {DateTime.UtcNow}
                                    Company Address: Thu Duc district, Ho Chi Minh city
                                    Company Phone: 0123456789
                                    Buyer Address: {getContact.Address}
                                    Buyer Phone: {getContact.Phone}
                                    Buyer Email: {getContact.Email}
                                    Tax: 10%
                                    Ship: 100.000
                                    The total amount includes the above fee items: {totalPrice + totalPrice * 0.1 + 100000}
                                    ",
                    Status = Contract.State.Pending
                };
                await _unit.ContractRepo.AddOneItem(ct);
                return (true, "Contract created successfully");
            }
            return (false, "The contact is not existed");
        }

        private async Task<(bool, string)> UpdateContact(string contactId, ArrayInterior[] array)
        {
            var getContact = (await _unit.ContactRepo.GetFieldsByFilterAsync([],
                    g => g.ContactId.Equals(contactId))).FirstOrDefault();
            if (getContact != null)
            {
                List<string> interiorIdList = [];
                foreach (var item in array)
                {
                    var getInterior = (await _unit.InteriorRepo.GetFieldsByFilterAsync([],
                        g => g.InteriorId.Equals(item.InteriorId))).FirstOrDefault();
                    if (getInterior != null)
                    {
                        interiorIdList.Add(getInterior.InteriorId);
                    }
                    return (false, $"The product with ID: {item.InteriorId} does not exist");
                }
                getContact.InteriorId = interiorIdList;
                await _unit.ContactRepo.UpdateItemByValue("ContactId", contactId, getContact);
            }
            return (false, "The contact is not existed");
        }

    }

}
