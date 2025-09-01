﻿using CashFlow.Domain.Security.Criptografy;
using Moq;

namespace CommomTestUtilities.Cryptography;

public class PasswordEncrypterBuilder
{
    private readonly Mock<IPasswordEncripter> _mock;

    public PasswordEncrypterBuilder()
    {
        _mock = new Mock<IPasswordEncripter>();

        _mock.Setup(passwordEncripter => passwordEncripter.Encrypt(It.IsAny<string>())).Returns("!%dlfjkd545");
    }

    public PasswordEncrypterBuilder Verify(string? password)
    {
        if (!string.IsNullOrWhiteSpace(password))
            _mock.Setup(passwordEncripter => passwordEncripter.Verify(password, It.IsAny<string>())).Returns(true);

        return this;
    }

    public IPasswordEncripter Build() => _mock.Object;
}

