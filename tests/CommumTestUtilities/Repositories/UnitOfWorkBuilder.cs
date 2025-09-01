using CashFlow.Domain.Repositories;
using Moq;

namespace CommomTestUtilities.Repositories;

public class UnitOfWorkBuilder
{
    public static IUnitOfWork Build()
    {
        var moq = new Mock<IUnitOfWork>();

        return moq.Object;
    }
}
