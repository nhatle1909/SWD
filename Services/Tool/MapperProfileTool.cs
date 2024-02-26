using AutoMapper;
using Repositories.Model;
using Repositories.Models;
using Repositories.ModelView;
using static Repositories.ModelView.AccountView;
using static Repositories.ModelView.BlogView;
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

            CreateMap<Interior, AddInteriorView>().ReverseMap();

            CreateMap<Blog, AddBlogView>().ReverseMap();
        }
    }
}
