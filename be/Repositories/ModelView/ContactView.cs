﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Repositories.Model.Contact;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

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
            public required IFormFile Picture { get; set; }
        }

        public class AddForCustomerContactView
        {
            public required string Content { get; set; }
            public required IFormFile Picture { get; set; }
        }

        public class AddressContactView
        {
            public required string ContactId { get; set; }
            public required string ResponseOfStaff { get; set; }
            public IFormFile? File { get; set; }
            public required State StatusResponseOfStaff { get; set; }
        }

        public class DeleteContactView
        {
            public required string ContactId { get; set; }
        }

        public class PagingContactView
        {
            public int PageIndex { get; set; }
            public bool IsAsc { get; set; }
            public string? SearchValue { get; set; }
        }

        public class DetailContactView
        {
            public required string ContactId { get; set; }
        }

        public class ArrayInterior
        {
            public required string InteriorId { get; set; }
            public int Quantity { get; set;}
        }
    }
}
