using AutoMapper;
using Repository.Model;
using Repository.Models;
using Repository.ModelView;
using static Repository.ModelView.AccountView;
using static Repository.ModelView.BlogView;
using static Repository.ModelView.InteriorView;
using static Repository.ModelView.MaterialView;
namespace Repository.Tools
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Account, RegisterAccountView>().ReverseMap();
            CreateMap<AccountStatus, RegisterAccountView>().ReverseMap();
            CreateMap<Account, RegisterForStaffAccountView>().ReverseMap();
            CreateMap<AccountStatus, RegisterForStaffAccountView>().ReverseMap();

            CreateMap<Material, AddMaterialView>().ReverseMap();

            CreateMap<Interior, AddInteriorView>().ReverseMap();

            CreateMap<Blog, AddBlogView>().ReverseMap();
        }
    }
}
