using AutoMapper;
using Repositories.Model;
using Repositories.Models;
using Repositories.ModelView;
using static Repositories.ModelView.AccountView;
using static Repositories.ModelView.BlogView;
using static Repositories.ModelView.CartView;
using static Repositories.ModelView.ContactView;
using static Repositories.ModelView.InteriorView;
using static Repositories.ModelView.MaterialView;
namespace Services.Tool
{
    public class MapperProfileTool : Profile
    {
        public MapperProfileTool()
        {
            CreateMap<Account, RegisterAccountView>().ReverseMap();
            CreateMap<AccountStatus, RegisterAccountView>().ReverseMap();
            CreateMap<Account, RegisterForStaffAccountView>().ReverseMap();
            CreateMap<AccountStatus, RegisterForStaffAccountView>().ReverseMap();

            CreateMap<Material, AddMaterialView>().ReverseMap();

            CreateMap<Interior, AddInteriorView>().ReverseMap()
                                                  .ForMember(a => a.Image, a => a.Ignore()); ;

            CreateMap<Blog, AddBlogView>().ReverseMap()
                                          .ForMember(a => a.Pictures, a => a.Ignore());

            CreateMap<BlogComment, AddCommentBlogView>().ReverseMap();
            CreateMap<Blog,IEnumerable<Blog>>().ReverseMap();
            CreateMap<Request, AddContactView>().ReverseMap();
            CreateMap<Request, AddForCustomerContactView>().ReverseMap();

            CreateMap<Transaction, IEnumerable<Transaction>>().ReverseMap();
            CreateMap<Transaction, RequestView>().ReverseMap();

            CreateMap<Cart, AddCartView>().ReverseMap();

            CreateMap<TransactionDetail, AddCartView>().ReverseMap();
        }
    }
}
