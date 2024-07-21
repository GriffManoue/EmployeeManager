using AutoMapper;
using EmployeeManager.Model.BaseModel;

namespace EmployeeManager.Model.Services;

/// <summary>
/// Provides mapping configurations for converting between Employee entities and their Data Transfer Object (DTO) representations.
/// Utilizes AutoMapper for mapping configurations.
/// </summary>
public class MapperService : Profile
{
    /// <summary>
    /// Configures the mappings between Employee entities and EmployeeDTOs.
    /// </summary>
    public MapperService()
    {
        // Maps properties from Employee to EmployeeDTO.
        CreateMap<Employee, EmployeeDTO>();
    }
}