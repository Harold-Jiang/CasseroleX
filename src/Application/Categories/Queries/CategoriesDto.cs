using CasseroleX.Application.Common.Mappings;
using CasseroleX.Application.Common.Models;
using CasseroleX.Domain.Entities;
using CasseroleX.Domain.Enums;

namespace CasseroleX.Application.Categories.Queries;
public class CategoriesDto : TreeDto<CategoriesDto>, IMapFrom<Category>
{  
    
    public Status Status { get; set; }

}
