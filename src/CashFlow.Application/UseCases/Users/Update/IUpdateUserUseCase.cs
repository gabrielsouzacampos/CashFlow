using CashFlow.Communication.Requests;

namespace CashFlow.Application.UseCases.Users.Update;

public interface IUpdateUserUseCase
{
    public Task Execute(RequestUpdateUserJson request);
}
