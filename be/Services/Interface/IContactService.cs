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
        Task<string> AddContact(AddContactView add);
        Task<(bool, string)> AddressTheContact(AddressContactView address);
        Task<(bool, string)> DeleteContact(DeleteContactView delete);
        Task<object> GetPagingContact(PagingContactView paging);
        Task<(bool, object)> GetContactDetail(DetailContactView detail);
    }
}
