using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Users.Profile;

public interface IGetUserProfileUseCase
{
    public Task<ResponseUserProfileJson> Execute();
}

