using Asee.Models;
using Asee.Models.Domain;
using Asee.Models.ViewM;
using System;

namespace Asee.Helper
{
    public static class Mapper
    {
        public static TransactionContext ToDomain(TransactionRequest request) => new()
        {
            TransactionId = request.TransactionId,
            Type = request.Type,
            Amount = request.Amount,
            Currency = request.Currency,
            IsInternational = request.IsInternational,
            Client = new ClientProfile
            {
                Id = request.Client.Id,
                CreditScore = request.Client.CreditScore,
                Segment = request.Client.Segment
            }
        };

        public static FeeResponse ToResponse(FeeCalculationResult result) => new()
        {
            TransactionId = result.TransactionId,
            CalculatedFee = result.TotalFee,
            TotalAmountWithFee = result.TotalAmountWithFee,
            AppliedRules = result.AppliedRules.Select(r => new AppliedRuleDto
            {
                RuleId = r.RuleId,
                Description = r.Description,
                FeeComponent = r.Amount
            }).ToList()
        };
    }

}