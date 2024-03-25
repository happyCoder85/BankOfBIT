using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    /// <summary>
    /// TransactionTypeValues:  Enum representing each of the transaction types
    /// supported within the BANK OF BIT System.
    /// </summary>
    public enum TransactionTypeValues
    {
        DEPOSIT = 1,
        WITHDRAWAL = 2,
        BILL_PAYMENT = 3,
        TRANSFER = 4,
        TRANSFER_RECIPIENT = 5,
        INTEREST = 6
    }
}
