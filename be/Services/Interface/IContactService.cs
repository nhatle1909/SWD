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
        Task<string> AddressTheContact(AddressContactView address);
        Task<string> DeleteContact(DeleteContactView delete);
        Task<object> GetPagingContact(PagingContactView paging);
        Task<object?> GetContactDetail(DetailContactView detail);
    }
}
