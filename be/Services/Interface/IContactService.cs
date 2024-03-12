﻿using Repositories.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Repositories.ModelView.ContactView;

namespace Services.Interface
{
    public interface IContactService
    {
        Task<(bool, string)> AddContactForGuest(string interiorId, AddContactView add);
        Task<(bool, string)> AddContactForCustomer(string id, string interiorId, AddForCustomerContactView add);
        Task<(bool, string)> AddressTheContact(AddressContactView address);
        Task<(bool, string)> CreateContractPdfAndPaymentLink();
        //Task<(bool, string)> DeleteContact(DeleteContactView delete);
        //Task<object> GetPagingContact(PagingContactView paging);
        //Task<(bool, object)> GetContactDetail(DetailContactView detail);
    }
}
