using CashFlow.Domain.Enums;
using CashFlow.Domain.Reports;

namespace CashFlow.Domain.Extensions;

public static class PaymentTypeExtensions
{
    public static string PaymentsTypeToString(this PaymentType payment)
    {
        return payment switch
        {
            PaymentType.Cash => ResourcePaymentTypeMessages.CASH,
            PaymentType.CreditCard => ResourcePaymentTypeMessages.CREDIT_CARD,
            PaymentType.DebitCard => ResourcePaymentTypeMessages.DEBIT_CARD,
            PaymentType.EletronicTransfer => ResourcePaymentTypeMessages.ELETRONIC_TRANSFER,
            _ => string.Empty
        };
    }
}

