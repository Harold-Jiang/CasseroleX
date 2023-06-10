using CasseroleX.Application.Common.Interfaces;

namespace CasseroleX.Infrastructure.Services;
public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
