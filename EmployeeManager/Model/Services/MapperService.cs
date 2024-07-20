using AutoMapper;
using EmployeeManager.Model.BaseModel;

namespace EmployeeManager.Model.Services
{
    public class MapperService : Profile
    {
        public MapperService()
        {
            CreateMap<Employee, EmployeeDTO>();
        }
    }
}
