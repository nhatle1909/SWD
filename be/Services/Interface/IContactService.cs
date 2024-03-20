using Repositories.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Repositories.ModelView.CartView;
using static Repositories.ModelView.ContactView;

namespace Services.Interface
{
    public interface IContactService
    {
        Task<(bool, string)> AddContactForGuest(AddContactView add);
        Task<(bool, string)> AddContactForCustomer(string id, AddForCustomerContactView add);
        Task<(bool, string)> AddressTheContact(AddressContactView address);
        Task<(bool, string, byte[])> GenerateContractPdf(string contactId, AddCartView[] cartViews);

        Task<(bool, string)> DeleteContact(DeleteContactView delete);
        Task<object> GetPagingContact(PagingContactView paging);
        Task<(bool, object)> GetContactDetail(DetailContactView detail);
        Task<(bool, object?)> GetCustomerContactList(string _id);
        Task<(bool, object)> GetCustomerContactDetail(DetailContactView detail);
        Task<(bool, object)> Accepted(string requestId);
        Task<(bool, object)> Refused(string requestId);
    }
}
