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
using static Repositories.ModelView.CartView;
using Org.BouncyCastle.Utilities;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Http;
using System.Collections;
namespace Services.Service
{
    public class ContactService : IContactService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IOptions<MailSettings> _mailSettings;
        private readonly ILogger<SendEmailTool> _logger;
        private readonly ITransactionService _vnpayService;
        public ContactService(ITransactionService vnpayService, IUnitOfWork unit, IMapper mapper, IEmailSender emailSender, IOptions<MailSettings> mailSettings, ILogger<SendEmailTool> logger)
        {
            _unit = unit;
            _mapper = mapper;
            _emailSender = emailSender;
            _logger = logger;
            _mailSettings = mailSettings;
            _vnpayService = vnpayService;
        }

        public async Task<(bool, string)> AddContactForGuest(AddContactView add)
        {
            if (add.Phone.Length == 10)
            {
                foreach (var item in add.ListInterior)
                {
                    var getInterior = (await _unit.InteriorRepo.GetFieldsByFilterAsync([],
                        g => g.InteriorId.Equals(item.InteriorId))).FirstOrDefault();
                    if (getInterior != null)
                    {
                        if (item.Quantity <= getInterior.Quantity)
                        {
                            Request contact = _mapper.Map<Request>(add);
                            contact.StatusResponseOfStaff = Request.State.Processing;
                            await _unit.ContactRepo.AddOneItem(contact);
                            return (true, "The request have been sent");
                        }
                        return (false, "The quantity of product available is less than the quantity you want");
                    }
                    return (false, $"The product with ID: {item.InteriorId} does not exist");
                }
            }
            return (false, "Phone number is not valid");
        }

        public async Task<(bool, string)> AddContactForCustomer(string id, AddForCustomerContactView add)
        {
            var getUser = (await _unit.AccountRepo.GetFieldsByFilterAsync([],
                            c => c.AccountId.Equals(id))).First();
            if (getUser.PhoneNumber != null)
            {
                if (!string.IsNullOrEmpty(getUser.Address))
                {
                    foreach (var item in add.ListInterior)
                    {
                        var getInterior = (await _unit.InteriorRepo.GetFieldsByFilterAsync([],
                            g => g.InteriorId.Equals(item.InteriorId))).FirstOrDefault();
                        if (getInterior != null)
                        {
                            if (item.Quantity <= getInterior.Quantity)
                            {
                                Request contact = _mapper.Map<Request>(add);
                                contact.Email = getUser.Email;
                                contact.Phone = getUser.PhoneNumber;
                                contact.Address = getUser.Address;
                                contact.StatusResponseOfStaff = Request.State.Processing;
                                await _unit.ContactRepo.AddOneItem(contact);
                                return (true, "The contact have been sent");
                            }
                            return (false, "The quantity of product available is less than the quantity you want");
                        }
                        return (false, $"The product with ID: {item.InteriorId} does not exist");
                    }
                }
                return (false, "Address is empty");
            }
            return (false, "Phone number is not valid");
        }

        public async Task<(bool, string)> AddressTheContact(AddressContactView address)
        {
            var getContact = (await _unit.ContactRepo.GetFieldsByFilterAsync([],
                    g => g.RequestId.Equals(address.RequestId))).FirstOrDefault();
            if (getContact != null)
            {
                if (address.StatusResponseOfStaff != Request.State.Consulting)
                {
                    byte[]? fileBytes = null;
                    if (address.ResponseOfStaffInFile != null)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await address.ResponseOfStaffInFile.CopyToAsync(ms);
                            fileBytes = ms.ToArray();
                        }
                    }
                    getContact.ResponseOfStaffInFile = fileBytes;
                    getContact.ResponseOfStaff = address.ResponseOfStaff;
                    getContact.StatusResponseOfStaff = address.StatusResponseOfStaff;
                    getContact.UpdatedAt = DateTime.Now;
                    await _unit.ContactRepo.UpdateItemByValue("RequestId", getContact.RequestId, getContact);
                    string subject = "Interior quotation system";
                    string body = $@"
                    <h3><strong>
                        {address.ResponseOfStaff}
                    </strong></h3>";
                    SendEmailTool sendEmail = new SendEmailTool(_mailSettings, _logger);
                    await sendEmail.SendEmailWithPdfAsync(getContact.Email, subject, body, address.ResponseOfStaffInFile);
                    return (true, $"You have addressed the contact of email: {getContact.Email}");
                }
                else
                {
                    byte[]? fileBytes = null;
                    if (address.ResponseOfStaffInFile != null)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await address.ResponseOfStaffInFile.CopyToAsync(ms);
                            fileBytes = ms.ToArray();
                        }
                    }
                    getContact.ResponseOfStaffInFile = fileBytes;
                    getContact.ResponseOfStaff = address.ResponseOfStaff;
                    getContact.StatusResponseOfStaff = address.StatusResponseOfStaff;
                    getContact.UpdatedAt = DateTime.Now;
                    await _unit.ContactRepo.UpdateItemByValue("RequestId", getContact.RequestId, getContact);
                    string subject = "Interior quotation system";
                    string acceptButtonLink = $"https://swdapi.azurewebsites.net/api/Contact/Accepted?requestId={getContact.RequestId}";
                    string refuseButtonLink = $"https://swdapi.azurewebsites.net/api/Contact/Refused?requestId={getContact.RequestId}";
                    //string acceptButtonLink = $"https://localhost:7220/api/Contact/Accepted?requestId={getContact.RequestId}";
                    //string refuseButtonLink = $"https://localhost:7220/api/Contact/Refused?requestId={getContact.RequestId}";
                    string body = $@"
                    <h3><strong>
                        {address.ResponseOfStaff}
                    </strong></h3>
                    <br />
                    <a href='{acceptButtonLink}' style='text-decoration: none;'><button style='background-color: blue; color: white; padding: 10px 20px; border: none; border-radius: 5px; font-size: 16px; cursor: pointer;'>Accept</button></a>
                    <a href='{refuseButtonLink}' style='text-decoration: none;'><button style='background-color: blue; color: white; padding: 10px 20px; border: none; border-radius: 5px; font-size: 16px; cursor: pointer;'>Refuse</button></a>
                    <br/>                    
                    ";

                    SendEmailTool sendEmail = new SendEmailTool(_mailSettings, _logger);
                    await sendEmail.SendEmailWithPdfAsync(getContact.Email, subject, body, address.ResponseOfStaffInFile);
                    return (true, $"You have addressed the contact of email: {getContact.Email}");
                }
            }
            return (false, "The contact is not existed");
        }

        public async Task<(bool, string)> DeleteContact(DeleteContactView delete)
        {
            var getContact = (await _unit.ContactRepo.GetFieldsByFilterAsync([],
                    g => g.RequestId.Equals(delete.RequestId))).FirstOrDefault();
            if (getContact != null && getContact.StatusResponseOfStaff == Request.State.Completed)
            {
                await _unit.ContactRepo.RemoveItemByValue("RequestId", getContact.RequestId);
                return (true, "Delete the contact successfully");
            }
            return (false, "The Contact does not exist or the Contact in progress cannot be deleted");
        }

        public async Task<object> GetPagingContact(PagingContactView paging)
        {
            const int pageSize = 20;
            const string sortField = "CreatedAt";
            List<string> searchFields = ["Email"];
            List<string> returnFields = [];

            int skip = (paging.PageIndex - 1) * pageSize;
            var items = (await _unit.ContactRepo.PagingAsync(skip, pageSize, paging.IsAsc, sortField, paging.SearchValue,
                searchFields, returnFields)).ToList();
            return items;
        }

        public async Task<(bool, object)> GetContactDetail(DetailContactView detail)
        {
            var getContact = (await _unit.ContactRepo.GetFieldsByFilterAsync([],
                    g => g.RequestId.Equals(detail.RequestId))).FirstOrDefault();
            if (getContact != null)
            {
                return (true, getContact);
            }
            return (false, "Request is not existed");
        }


        public async Task<(bool, string, byte[]?)> GenerateContractPdf(string RequestId, AddCartView[] array)
        {
            var totalPrice = 0;
            var getContact = (await _unit.ContactRepo.GetFieldsByFilterAsync([],
                    g => g.RequestId.Equals(RequestId))).FirstOrDefault();
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
                            else continue;
                        }
                        return (false, "The quantity of product available is less than the quantity you want", null);
                    }
                    return (false, $"The product with ID: {item.InteriorId} does not exist", null);
                }


                QuestPDF.Settings.License = LicenseType.Community;
                var pdf = Document.Create(container =>
                {
                    container.Page(p =>
                    {
                        p.Size(PageSizes.A4);
                        p.Margin(1, Unit.Centimetre);
                        p.PageColor(Colors.White);
                        p.DefaultTextStyle(x => x.FontSize(20).FontFamily("Arial"));

                        p.Content()
                        .Table(
                            contract =>
                            {
                                contract.ColumnsDefinition(cols =>
                                {
                                    cols.RelativeColumn();
                                });
                                contract.Cell().BorderVertical(3).BorderTop(3).BorderBottom(1).PaddingVertical(1).Column(x =>
                                {
                                    x.Item().Row(header =>
                                    {
                                        header.RelativeItem().Column(col =>
                                        {
                                            col.Item().AlignCenter().Text("CONTRACT").FontSize(16).Italic().FontColor(Colors.Black);

                                            col.Item().AlignCenter().Text(text =>
                                            {
                                                text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                                                text.Span("Date:");
                                                text.Span($" {DateTime.Now.Day}").SemiBold();
                                                text.Span(" month");
                                                text.Span($" {DateTime.Now.Month}").SemiBold();
                                                text.Span(" year");
                                                text.Span($" {DateTime.Now.Year}").SemiBold();
                                            });

                                        });
                                    }
                                    );
                                });


                                contract.Cell().BorderVertical(3).BorderHorizontal(1).PaddingHorizontal(4).Column(
                                    col =>
                                    {
                                        col.Item().Text(text =>
                                        {
                                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                                            text.Span("Company Name: ");
                                            text.Span("Interior Quote System").SemiBold();
                                        });
                                        col.Item().Text(text =>
                                        {
                                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                                            text.Span("Address: ");
                                            text.Span("Thu Duc district, Ho Chi Minh city");
                                        });
                                        col.Item().Text(text =>
                                        {
                                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                                            text.Span("Phone: ");
                                            text.Span("0123456789");
                                        });
                                    });
                                contract.Cell().BorderVertical(3).BorderHorizontal(1).PaddingHorizontal(4).Column(
                                    col =>
                                    {
                                        col.Item().Text(text =>
                                        {
                                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                                            text.Span("Buyer Email: ");
                                            text.Span($"{getContact.Email}");
                                        });
                                        col.Item().Text(text =>
                                        {
                                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                                            text.Span("Buyer Phone: ");
                                            text.Span($"{getContact.Phone}");
                                        });
                                        col.Item().Text(text =>
                                        {
                                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                                            text.Span("Buyer Address: ");
                                            text.Span($"{getContact.Address}");
                                        });
                                    });


                                contract.Cell().BorderVertical(3).BorderHorizontal(1).Table(
                                    table =>
                                    {
                                        table.ColumnsDefinition(col =>
                                        {
                                            col.ConstantColumn(35);
                                            col.RelativeColumn();
                                            col.ConstantColumn(70);
                                            col.ConstantColumn(70);
                                            col.ConstantColumn(70);
                                            col.ConstantColumn(70);
                                            col.ConstantColumn(70);
                                        });

                                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignCenter().AlignMiddle().Text(text =>
                                        {
                                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                                            text.Span("Interior Id").SemiBold();
                                            text.Span("(Id)").Italic();
                                        });
                                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignCenter().AlignMiddle().Text(text =>
                                        {
                                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                                            text.Span("Interior Name").SemiBold();
                                            text.Span("\r\nName").Italic();
                                        });
                                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignCenter().AlignMiddle().Text(text =>
                                        {
                                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                                            text.Span("Quantity").SemiBold();
                                            text.Span("(Quantity)").Italic();
                                        });
                                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignCenter().AlignMiddle().Text(text =>
                                        {
                                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                                            text.Span("Unit price").SemiBold();
                                            text.Span("(Unit price)").Italic();
                                        });
                                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignCenter().AlignMiddle().Text(text =>
                                        {
                                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                                            text.Span("Tax").SemiBold();
                                            text.Span("(Rate)").Italic();
                                        });
                                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignCenter().AlignMiddle().Text(text =>
                                        {
                                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                                            text.Span("Tax Section").SemiBold();
                                            text.Span("(Tax Section)").Italic();
                                        });
                                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignCenter().AlignMiddle().Text(text =>
                                        {
                                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                                            text.Span("Amount").SemiBold();
                                            text.Span("(Amount)").Italic();
                                        });


                                        for (var i = 0; i < list.Count; i++)
                                        {
                                            var item = list[i];

                                            table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignCenter().AlignMiddle().Text($"{item.ProductId}").FontSize(12).FontColor(Colors.Black);
                                            table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignMiddle().Text($"{item.ProductName}").FontSize(12).FontColor(Colors.Black);
                                            table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignMiddle().AlignRight().Text($"{item.Quantity}").FontSize(12).FontColor(Colors.Black);
                                            table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignMiddle().AlignRight().Text($"{item.UnitPrice}").FontSize(12).FontColor(Colors.Black);
                                            table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignMiddle().AlignRight().Text("10%").FontSize(12).FontColor(Colors.Black);
                                            table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignMiddle().AlignRight().Text($"{item.TotalCostOfPoduct * 0.1}").FontSize(12).FontColor(Colors.Black);
                                            table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignMiddle().AlignRight().Text($"{item.TotalCostOfPoduct * 1.1}").FontSize(12).FontColor(Colors.Black);
                                        }
                                        ;
                                    });

                                contract.Cell().BorderVertical(3).BorderTop(1).BorderBottom(3).Table(
                                    table =>
                                    {
                                        table.ColumnsDefinition(col =>
                                        {
                                            col.RelativeColumn();
                                            col.ConstantColumn(70);
                                            col.ConstantColumn(70);
                                            col.ConstantColumn(70);
                                            col.ConstantColumn(70);
                                        });
                                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignMiddle().AlignRight().Text("Ship:").FontSize(12).FontColor(Colors.Black);
                                        table.Cell().Border(1);
                                        table.Cell().Border(1);
                                        table.Cell().Border(1);
                                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignMiddle().AlignRight().Text("100.000").FontSize(12).FontColor(Colors.Black);

                                        var total = 0;
                                        foreach (var product in list)
                                        {
                                            total = total + product.TotalCostOfPoduct;
                                        }
                                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignMiddle().AlignRight().Text("Total amount:").FontSize(12).FontColor(Colors.Black);
                                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignMiddle().AlignRight().Text($"{total}").FontSize(12).FontColor(Colors.Black);
                                        table.Cell().Border(1);
                                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignMiddle().AlignRight().Text($"{(total) * 0.1}").FontSize(12).FontColor(Colors.Black);
                                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignMiddle().AlignRight().Text($"{Math.Round(total * 1.1 + 100000)}").FontSize(12).FontColor(Colors.Black);
                                        ;
                                    });
                            });
                    });
                }).GeneratePdf();

                Contract ct = new()
                {
                    ContractId = ObjectId.GenerateNewId().ToString(),
                    RequestId = RequestId,
                    EmailOfCustomer = getContact.Email,
                    CreatedAt = DateTime.Now,
                    Description = $@"
                                    Company Name: Interior Quote System
                                    Creation Date: {DateTime.Now}
                                    Company Address: Thu Duc district, Ho Chi Minh city
                                    Company Phone: 0123456789
                                    Buyer Address: {getContact.Address}
                                    Buyer Phone: {getContact.Phone}
                                    Buyer Email: {getContact.Email}
                                    Tax: 10%
                                    Ship: 100.000
                                    The total amount includes the above fee items: {Math.Ceiling(totalPrice + totalPrice * 0.1 + 100000)}
                                    ",
                    ContractFile = pdf
                };
                var pdfstream = new MemoryStream();

                await _unit.ContractRepo.AddOneItem(ct);
                return (true, "", pdf);
            }
            return (false, "The request is not existed", null);
        }


        public async Task<(bool, object?)> GetCustomerContactList(string _id)
        {
            var responses = new List<object>();
            var email = (await _unit.AccountRepo.GetFieldsByFilterAsync(["Email"],
                a => a.AccountId.Equals(_id))).FirstOrDefault();
            if (email != null)
            {
                IEnumerable<Request> requestList = await _unit.ContactRepo.GetByFilterAsync(a => a.Email.Equals(email.Email));
                if (requestList.Any())
                {
                    foreach (var request in requestList)
                    {
                        responses.Add(new
                        {
                            RequestId = request.RequestId,
                            Email = request.Email,
                            Phone = request.Phone,
                            StatusResponseOfStaff = request.StatusResponseOfStaff,
                            CreateAt = request.CreatedAt
                        });
                    }
                    return (true, responses);
                }
                return (true, "You currently have no requests");
            }
            return (false, null);
        }

        public async Task<(bool, object)> GetCustomerContactDetail(DetailContactView detail)
        {
            var getContact = (await _unit.ContactRepo.GetFieldsByFilterAsync([],
                    g => g.RequestId.Equals(detail.RequestId))).FirstOrDefault();
            if (getContact != null)
            {
                return (true, getContact);
            }
            return (false, "Request is not existed");
        }

        public async Task<(bool, object)> Accepted(string requestId)
        {
            var getContact = (await _unit.ContactRepo.GetFieldsByFilterAsync([],
                    g => g.RequestId.Equals(requestId))).FirstOrDefault();
            if (getContact != null)
            {
                if (getContact.StatusResponseOfStaff == Request.State.Completed)
                {
                    return (false, "false");
                }
                string check = await _vnpayService.AddPendingTransaction(requestId, getContact.ListInterior);
                int deposit = await _vnpayService.CalculateDeposit(check);
                string depositLink;
                if (check != null)
                {
                    depositLink = await _vnpayService.Payment(check, deposit);
                }
                else depositLink = "";
                var pdf = GenerateContractPdf(requestId, getContact.ListInterior).Result.Item3;
                var stream = new MemoryStream(pdf);

                // Tạo đối tượng IFormFile từ MemoryStream
                IFormFile file = new FormFile(stream, 0, pdf.Length, "file.pdf", "file.pdf");

                getContact.StatusResponseOfStaff = Request.State.Consulting;
                getContact.UpdatedAt = DateTime.Now;
                await _unit.ContactRepo.UpdateItemByValue("RequestId", getContact.RequestId, getContact);
                string subject = "Interior quotation system";
                string body = $@"
                    <h3><strong>
                        Thank you for accepting, here is the link to deposit 30% of the order value.<br>
                        <a href='{depositLink}'>Here</a>
                    </strong></h3>";
                SendEmailTool sendEmail = new SendEmailTool(_mailSettings, _logger);
                await sendEmail.SendEmailWithPdfAsync(getContact.Email, subject, body, file);
                return (true, $"");
            }
            return (false, "Request is not existed");
        }
        public async Task<(bool, object)> Refused(string requestId)
        {
            var getContact = (await _unit.ContactRepo.GetFieldsByFilterAsync([],
                    g => g.RequestId.Equals(requestId))).FirstOrDefault();
            if (getContact != null)
            {
                getContact.StatusResponseOfStaff = Request.State.Completed;
                getContact.UpdatedAt = DateTime.Now;
                await _unit.ContactRepo.UpdateItemByValue("RequestId", getContact.RequestId, getContact);
                string subject = "Interior quotation system";
                string body = $@"
        <h3><strong>
            We are sorry that you have refused this order. If you need further advice, please contact us via phone number: 0123456789
        </strong></h3>";
                SendEmailTool sendEmail = new SendEmailTool(_mailSettings, _logger);
                await sendEmail.SendEmailWithPdfAsync(getContact.Email, subject, body, null);
                return (true, $"");
            }
            return (false, "Request is not existed");
        }
    }

}
