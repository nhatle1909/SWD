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
                    Request contact = _mapper.Map<Request>(add);
                    contact.InteriorId = interiorIdList;
                    contact.Picture = fileBytes;

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
                        Request contact = _mapper.Map<Request>(add);
                        contact.Email = getUser.Email;
                        contact.Phone = getUser.PhoneNumber;
                        contact.Address = getUser.Address;
                        contact.InteriorId = interiorIdList;
                        contact.Picture = fileBytes;

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
                    g => g.RequestId.Equals(address.RequestId))).FirstOrDefault();
            if (getContact != null)
            {
                if (address.StatusResponseOfStaff == Request.State.Completed)
                {
                    getContact.ResponseOfStaff = address.ResponseOfStaff;
                    getContact.StatusResponseOfStaff = address.StatusResponseOfStaff;

                    getContact.UpdatedAt = DateTime.UtcNow;
                    await _unit.ContactRepo.UpdateItemByValue("RequestId", getContact.RequestId, getContact);
                    string subject = "Interior quotation system";
                    string body = $@"
                    <h3><strong>
                        {address.ResponseOfStaff}
                    </strong></h3>";
                    await _emailSender.SendEmailAsync(getContact.Email, subject, body);
                    return (true, $"You have addressed the contact of email: {getContact.Email}");
                }
                else if (address.StatusResponseOfStaff == Request.State.Awaiting_Payment)
                {
                    getContact.ResponseOfStaff = address.ResponseOfStaff;
                    getContact.StatusResponseOfStaff = address.StatusResponseOfStaff;

                    getContact.UpdatedAt = DateTime.UtcNow;
                    await _unit.ContactRepo.UpdateItemByValue("RequestId", getContact.RequestId, getContact);
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
            const int pageSize = 5;
            const string sortField = "CreatedAt";
            List<string> searchFields = ["Email"];
            List<string> returnFields = ["Email", "StatusResponseOfStaff", "CreatedAt"];

            int skip = (paging.PageIndex - 1) * pageSize;
            var items = (await _unit.ContactRepo.PagingAsync(skip, pageSize, paging.IsAsc, sortField, paging.SearchValue, searchFields, returnFields)).ToList();
            var responses = new List<object>();

            foreach (var item in items)
            {
                responses.Add(new
                {
                    RequestId = item.RequestId,
                    Email = item.Email,
                    Status = item.StatusResponseOfStaff,
                    CreatedAt = item.CreatedAt
                });
            }
            return responses;
        }

        public async Task<(bool, object)> GetContactDetail(DetailContactView detail)
        {
            var getContact = (await _unit.ContactRepo.GetFieldsByFilterAsync([],
                    g => g.RequestId.Equals(detail.RequestId))).FirstOrDefault();
            if (getContact != null)
            {
                return (true, getContact);
            }
            return (false, "Contact is not existed");
        }


        public async Task<(bool, string, byte[])> GenerateContractPdf(string staffId, string RequestId, AddCartView[] array)
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
                                                text.Span($" {DateTime.UtcNow.Day}").SemiBold();
                                                text.Span(" month");
                                                text.Span($" {DateTime.UtcNow.Month}").SemiBold();
                                                text.Span(" year");
                                                text.Span($" {DateTime.UtcNow.Year}").SemiBold();
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

                //QuestPDF.Settings.License = LicenseType.Community;
                //Document.Create(container =>
                //{
                //    container.Page(p =>
                //    {
                //        p.Size(PageSizes.A4);
                //        p.Margin(1, Unit.Centimetre);
                //        p.PageColor(Colors.White);
                //        p.DefaultTextStyle(x => x.FontSize(20).FontFamily("Arial"));


                //        p.Content()
                //        .Table(
                //            contract =>
                //            {
                //                contract.ColumnsDefinition(cols =>
                //                {
                //                    cols.RelativeColumn();
                //                });
                //                contract.Cell().BorderVertical(3).BorderTop(3).BorderBottom(1).PaddingVertical(1).Column(x =>
                //                {
                //                    x.Item().Row(header =>
                //                    {
                //                        header.RelativeItem().Column(col =>
                //                        {
                //                            col.Item().AlignCenter().Text("CONTRACT").FontSize(16).Italic().FontColor(Colors.Black);

                //                            col.Item().AlignCenter().Text(text =>
                //                            {
                //                                text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                //                                text.Span("Date:");
                //                                text.Span($" {DateTime.UtcNow.Day}").SemiBold();
                //                                text.Span(" month");
                //                                text.Span($" {DateTime.UtcNow.Month}").SemiBold();
                //                                text.Span(" year");
                //                                text.Span($" {DateTime.UtcNow.Year}").SemiBold();
                //                            });

                //                        });
                //                    }
                //                    );
                //                });


                //                contract.Cell().BorderVertical(3).BorderHorizontal(1).PaddingHorizontal(4).Column(
                //                    col =>
                //                    {
                //                        col.Item().Text(text =>
                //                        {
                //                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                //                            text.Span("Company Name: ");
                //                            text.Span("Interior Quote System").SemiBold();
                //                        });
                //                        col.Item().Text(text =>
                //                        {
                //                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                //                            text.Span("Address: ");
                //                            text.Span("Thu Duc district, Ho Chi Minh city");
                //                        });
                //                        col.Item().Text(text =>
                //                        {
                //                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                //                            text.Span("Phone: ");
                //                            text.Span("0123456789");
                //                        });
                //                    });
                //                contract.Cell().BorderVertical(3).BorderHorizontal(1).PaddingHorizontal(4).Column(
                //                    col =>
                //                    {
                //                        col.Item().Text(text =>
                //                        {
                //                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                //                            text.Span("Buyer Email: ");
                //                            text.Span($"{getContact.Email}");
                //                        });
                //                        col.Item().Text(text =>
                //                        {
                //                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                //                            text.Span("Buyer Phone: ");
                //                            text.Span($"{getContact.Phone}");
                //                        });
                //                        col.Item().Text(text =>
                //                        {
                //                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                //                            text.Span("Buyer Address: ");
                //                            text.Span($"{getContact.Address}");
                //                        });
                //                    });


                //                contract.Cell().BorderVertical(3).BorderHorizontal(1).Table(
                //                    table =>
                //                    {
                //                        table.ColumnsDefinition(col =>
                //                        {
                //                            col.ConstantColumn(70);
                //                            col.RelativeColumn();
                //                            col.ConstantColumn(70);
                //                            col.ConstantColumn(70);
                //                            col.ConstantColumn(70);
                //                            col.ConstantColumn(70);
                //                            col.ConstantColumn(70);
                //                        });

                //                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignCenter().AlignMiddle().Text(text =>
                //                        {
                //                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                //                            text.Span("Interior Id").SemiBold();
                //                            text.Span("(Id)").Italic();
                //                        });
                //                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignCenter().AlignMiddle().Text(text =>
                //                        {
                //                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                //                            text.Span("Interior Name").SemiBold();
                //                            text.Span("\r\nName").Italic();
                //                        });
                //                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignCenter().AlignMiddle().Text(text =>
                //                        {
                //                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                //                            text.Span("Quantity").SemiBold();
                //                            text.Span("(Quantity)").Italic();
                //                        });
                //                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignCenter().AlignMiddle().Text(text =>
                //                        {
                //                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                //                            text.Span("Unit price").SemiBold();
                //                            text.Span("(Unit price)").Italic();
                //                        });
                //                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignCenter().AlignMiddle().Text(text =>
                //                        {
                //                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                //                            text.Span("Tax").SemiBold();
                //                            text.Span("(Rate)").Italic();
                //                        });
                //                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignCenter().AlignMiddle().Text(text =>
                //                        {
                //                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                //                            text.Span("Tax Section").SemiBold();
                //                            text.Span("(Tax Section)").Italic();
                //                        });
                //                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignCenter().AlignMiddle().Text(text =>
                //                        {
                //                            text.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));
                //                            text.Span("Amount").SemiBold();
                //                            text.Span("(Amount)").Italic();
                //                        });


                //                        for (var i = 0; i < list.Count; i++)
                //                        {
                //                            var item = list[i];

                //                            table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignCenter().AlignMiddle().Text($"{item.ProductId}").FontSize(12).FontColor(Colors.Black);
                //                            table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignMiddle().Text($"{item.ProductName}").FontSize(12).FontColor(Colors.Black);
                //                            table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignMiddle().AlignRight().Text($"{item.Quantity}").FontSize(12).FontColor(Colors.Black);
                //                            table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignMiddle().AlignRight().Text($"{item.UnitPrice}").FontSize(12).FontColor(Colors.Black);
                //                            table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignMiddle().AlignRight().Text("10%").FontSize(12).FontColor(Colors.Black);
                //                            table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignMiddle().AlignRight().Text($"{item.TotalCostOfPoduct * 0.1}").FontSize(12).FontColor(Colors.Black);
                //                            table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignMiddle().AlignRight().Text($"{item.TotalCostOfPoduct * 1.1}").FontSize(12).FontColor(Colors.Black);
                //                        }
                //                        ;
                //                    });

                //                contract.Cell().BorderVertical(3).BorderTop(1).BorderBottom(3).Table(
                //                    table =>
                //                    {
                //                        table.ColumnsDefinition(col =>
                //                        {
                //                            col.RelativeColumn();
                //                            col.ConstantColumn(70);
                //                            col.ConstantColumn(70);
                //                            col.ConstantColumn(70);
                //                            col.ConstantColumn(70);
                //                        });
                //                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignMiddle().AlignRight().Text("Ship:").FontSize(12).FontColor(Colors.Black);
                //                        table.Cell().Border(1);
                //                        table.Cell().Border(1);
                //                        table.Cell().Border(1);
                //                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignMiddle().AlignRight().Text("100.000").FontSize(12).FontColor(Colors.Black);

                //                        var total = 0;
                //                        foreach (var product in list)
                //                        {
                //                            total = total + product.TotalCostOfPoduct;
                //                        }
                //                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignMiddle().AlignRight().Text("Total amount:").FontSize(12).FontColor(Colors.Black);
                //                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignMiddle().AlignRight().Text($"{total}").FontSize(12).FontColor(Colors.Black);
                //                        table.Cell().Border(1);
                //                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignMiddle().AlignRight().Text($"{(total) * 0.1}").FontSize(12).FontColor(Colors.Black);
                //                        table.Cell().Border(1).PaddingHorizontal(4).PaddingVertical(2).AlignMiddle().AlignRight().Text($"{Math.Round(total * 1.1 + 100000)}").FontSize(12).FontColor(Colors.Black);
                //                        ;
                //                    });
                //            });
                //    });
                //}).GeneratePdf($"{RequestId}_Contract.pdf");
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
                                    The total amount includes the above fee items: {Math.Ceiling(totalPrice + totalPrice * 0.1 + 100000)}
                                    ",
                    Status = Contract.State.Pending,
                    ContractFile = pdf
                };
                var pdfstream = new MemoryStream();

                await _unit.ContractRepo.AddOneItem(ct);
                return (true, "", pdf);
            }
            return (false, "The contact is not existed", null);
        }

        public async Task<(bool, string)> UpdateContact(string RequestId, AddCartView[] array)
        {
            int count = 0;
            var getContact = (await _unit.ContactRepo.GetFieldsByFilterAsync([],
                    g => g.RequestId.Equals(RequestId))).FirstOrDefault();
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
                        count++;
                        if (count == array.Length) break;
                        else continue;
                    }
                    return (false, $"The product with ID: {item.InteriorId} does not exist");
                }
                getContact.InteriorId = interiorIdList;
                await _unit.ContactRepo.UpdateItemByValue("RequestId", RequestId, getContact);
                return (true, "Update Contact success");
            }
            return (false, "The contact is not existed");
        }

        public async Task<(bool, object)> GetCustomerContactList(string _id)
        {
            var responses = new List<object>();
            var email = (await _unit.AccountRepo.GetFieldsByFilterAsync(["Email"], a => a.AccountId.Equals(_id))).FirstOrDefault().Email;
            if (email != null)
            {
                IEnumerable<Request> requestList = await _unit.ContactRepo.GetByFilterAsync(a => a.Email.Equals(email));

                foreach (var request in requestList)
                {
                    var name = (await _unit.InteriorRepo.GetFieldsByFilterAsync(["InteriorName"], i => i.InteriorId.Equals(request.InteriorId.FirstOrDefault()))).FirstOrDefault().InteriorName;
                    responses.Add(new
                    {
                        RequestId = request.RequestId,
                        InteriorId = request.InteriorId.FirstOrDefault(),
                        InteriorName = name,
                        CreateAt = request.CreatedAt,

                        Status = request.StatusResponseOfStaff
                    });
                }
                return (true, responses);
            }
            else
            {
                return (false, null);
            }
        }
    }

}
