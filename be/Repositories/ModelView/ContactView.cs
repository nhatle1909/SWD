using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Repositories.Model.Contact;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Repositories.ModelView
{
    public class ContactView
    {
        public class AddContactView
        {
            [EmailAddress] public required string Email { get; set; }
            [StringLength(1000, MinimumLength = 1)] public required string Title { get; set; }
            [StringLength(10000, MinimumLength = 1)] public required string Content { get; set; }
            public IFormFile[]? Pictures { get; set; }
        }

        public class AddressContactView
        {
            public required string Jwt { get; set; }
            public required string ContactId { get; set; }
            public required string ResponseOfStaff { get; set; }
        }

        public class DeleteContactView
        {
            public required string Jwt { get; set; }
            public required string ContactId { get; set; }
        }

        public class PagingContactView
        {
            public required string Jwt { get; set; }
            public int PageIndex { get; set; }
            public bool IsAsc { get; set; }
            public string? SearchValue { get; set; }
        }

        public class DetailContactView
        {
            public required string Jwt { get; set; }
            public required string ContactId { get; set; }
        }
    }
}
