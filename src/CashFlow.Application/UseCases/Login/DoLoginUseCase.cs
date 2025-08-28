using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Criptografy;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception;

namespace CashFlow.Application.UseCases.Login;

public class DoLoginUseCase(IUsersRepository repository, IPasswordEncripter encripter, IAccessTokenGenerator tokenGenerator) : IDoLoginUseCase
{
    private readonly IUsersRepository _repository = repository;
    private readonly IPasswordEncripter _encripter = encripter;
    private readonly IAccessTokenGenerator _tokenGenerator = tokenGenerator;

    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var user = await _repository.GetUserByEmail(request.Email) 
            ?? throw new InvalidLoginExeption();

        if (!_encripter.Verify(request.Password, user.Password)) 
            throw new InvalidLoginExeption();

        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Token = _tokenGenerator.GenerateAccessToken(user)
        };
    }
}
