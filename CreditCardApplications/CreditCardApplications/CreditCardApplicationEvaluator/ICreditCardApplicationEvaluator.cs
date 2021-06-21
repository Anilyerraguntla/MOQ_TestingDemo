using System;
using System.Collections.Generic;
using System.Text;

namespace CreditCardApplications.BLL.CreditCardApplicationEvaluator
{
    public interface ICreditCardApplicationEvaluator
    {
        public CreditCardApplicationDecision Evaluate(CreditCardApplication application);
    }
}
