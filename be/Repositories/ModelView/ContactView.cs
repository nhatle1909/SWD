using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Repositories.Model.Request;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;
using static Repositories.ModelView.CartView;

namespace Repositories.ModelView
{
    public class ContactView
    {
        public class AddContactView
        {
            [EmailAddress] public required string Email { get; set; }
            [Phone] public required string Phone { get; set; }
            public required string Address { get; set; }
            public required string Content { get; set; }
            public required AddCartView[] ListInterior {  get; set; }
        }

        public class AddForCustomerContactView
        {
            public required string Content { get; set; }
            public required AddCartView[] ListInterior { get; set; }
        }

        public class AddressContactView
        {
            public required string RequestId { get; set; }
            public required string ResponseOfStaff { get; set; }
            public IFormFile? ResponseOfStaffInFile { get; set; }
            public required State StatusResponseOfStaff { get; set; }
        }

        public class DeleteContactView
        {
            public required string RequestId { get; set; }
        }

        public class PagingContactView
        {
            public int PageIndex { get; set; }
            public bool IsAsc { get; set; }
            public string? SearchValue { get; set; }
        }

        public class DetailContactView
        {
            public required string RequestId { get; set; }
        }

    }
}
