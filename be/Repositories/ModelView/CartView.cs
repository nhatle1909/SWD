using Microsoft.AspNetCore.Http;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Repositories.Model.Interior;

namespace Repositories.ModelView
{
    public class CartView
    {
        public class AddCartView
        {
            public required string InteriorId { get; set; }
            [Range(1, int.MaxValue)]
            public required int Quantity { get; set; }
        }

    }
}
