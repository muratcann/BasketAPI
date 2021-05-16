using Basket.Api.Rules.Validations;
using System;

namespace Basket.Api.Rules.Exeption
{
    public class BusinessRuleValidationException : Exception
    {
        public RuleApprover Approver { get; }
        public string Details { get; }

        public BusinessRuleValidationException(RuleApprover approver) : base(approver.Message)
        {
            Approver = approver;
            this.Details = approver.Message;
        }

        public override string ToString()
        {
            return $"{Approver.GetType().FullName}: {Approver.Message}";
        }
    }
}
