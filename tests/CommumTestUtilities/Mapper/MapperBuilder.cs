using AutoMapper;
using CashFlow.Application.AutoMapper;
using Microsoft.Extensions.Logging;

namespace CommomTestUtilities.Mapper;

public class MapperBuilder
{
    public static IMapper Build()
    {
        var mapper = new MapperConfiguration(config =>
        {
            config.AddProfile(new AutoMapping());
        }, new LoggerFactory());

        return mapper.CreateMapper();
    }
}

