using Asee.Rules;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Asee.Models.Domain
{
    public class FeeRules
    {
        [Key]
        public string RuleId { get; set; }
        public string RuleType { get; set; }
        public string Description { get; set; }
        public string Criteria { get; set; }
        public string Action { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }

        public bool IsMatch(TransactionContext tx, FeeRules rule)
        {
            var criteria = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(rule.Criteria);

            // Check for POS rules
            if (rule.RuleType == "POS" && tx.Type == "POS")
            {
                // Using GetDecimal to properly cast JsonElement to decimal
                return tx.Amount <= criteria["maxAmount"].GetDecimal();
            }

            // Check for E-commerce rules
            if (rule.RuleType == "E-commerce" && tx.Type == "E-commerce")
            {
                // Using GetDecimal to properly cast JsonElement to decimal
                return tx.Amount >= criteria["minAmount"].GetDecimal();
            }

            // Check for CreditScoreDiscount
            if (rule.RuleType == "CreditScoreDiscount" && tx.Client.CreditScore > criteria["creditScore"].GetInt32())
            {
                return true;
            }

            return false;
        }

        public AppliedRuleResult Apply(TransactionContext tx, FeeRules rule)
        {
            var result = new AppliedRuleResult
            {
                RuleId = rule.RuleId,
                Description = rule.Description,
                Amount = 0
            };

            var criteria = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(rule.Criteria);

            // Handle fixed_fee logic for POS transactions
            if (rule.Action == "fixed_fee" && rule.RuleType == "POS" && tx.Type == "POS")
            {
                if (tx.Amount <= criteria["maxAmount"].GetDecimal())
                {
                    result.Amount = rule.Amount;  // Apply fixed fee for POS transactions with amount <= maxAmount
                }
                else
                {
                    result.Amount = tx.Amount * 0.002m;  // Apply percentage fee for POS transactions > 100
                }
            }

            // Handle percentage_fee logic for E-commerce transactions
            else if (rule.Action == "percentage_fee" && rule.RuleType == "E-commerce" && tx.Type == "E-commerce")
            {
                var percentageFee = tx.Amount * rule.Amount;
                var fixedFee = criteria["fixedFee"].GetDecimal();
                var totalFee = percentageFee + fixedFee;

                result.Amount = totalFee > criteria["maxFee"].GetDecimal()
                    ? criteria["maxFee"].GetDecimal()  // Cap the fee at the maxFee
                    : totalFee;
            }

            // Handle discount logic for credit score based discounts
            else if (rule.Action == "discount" && rule.RuleType == "CreditScoreDiscount" && tx.Client.CreditScore > criteria["creditScore"].GetInt32())
            {
                result.Amount = -(tx.Amount * rule.Amount);  // Apply negative fee (discount)
            }

            return result;
        }

    }
}
