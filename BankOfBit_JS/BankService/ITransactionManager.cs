using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BankService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITransactionManager" in both code and config file together.
    [ServiceContract]
    public interface ITransactionManager
    {
        /// <summary>
        /// Implementation will deposit an amount into an account based off the 
        /// arguments passed to it and returns an updated account balance.
        /// </summary>
        /// <param name="accountId">Represents the account id</param>
        /// <param name="amount">Represents the amount being deposited</param>
        /// <param name="notes">Represents the notes regarding a deposit.</param>
        /// <returns>Returns an updated balance of the account.</returns>
        [OperationContract]
        double? Deposit(int accountId, double amount, String notes);

        /// <summary>
        /// Implementation will withdrawal an amount from an account based off the 
        /// arguments passed to it and returns an updated account balance.
        /// </summary>
        /// <param name="accountId">Represents the account id.</param>
        /// <param name="amount">Represents the amount being withdrawn.</param>
        /// <param name="notes">Represents the notes regarding a withdrawal.</param>
        /// <returns>Returns an updated balance of the account.</returns>
        [OperationContract]
        double? Withdrawal(int accountId, double amount, String notes);

        /// <summary>
        /// Implementation will withdrawal an amount from an account and "pay a bill"
        /// based off the arguments passed to it and returns an updated account balance.
        /// </summary>
        /// <param name="accountId">Represents the account id.</param>
        /// <param name="amount">Represents the amount being paid to the bill.</param>
        /// <param name="notes">Represents the notes regarding a bill payment.</param>
        /// <returns>Returns an updated balance of the account.</returns>
        [OperationContract]
        double? BillPayment(int accountId, double amount, String notes);

        /// <summary>
        /// Implementation will withdraw funds from one account and transfer them into another
        /// account based on the arguments passed to it and return an updated account balance.
        /// </summary>
        /// <param name="fromAccountId"></param>
        /// <param name="toAccountId"></param>
        /// <param name="amount"></param>
        /// <param name="notes"></param>
        /// <returns></returns>
        [OperationContract]
        double? Transfer(int fromAccountId, int toAccountId, double amount, String notes);

        /// <summary>
        /// Implementation will calculate and return the interest of an account based on the arguments passed.
        /// </summary>
        /// <param name="accountId">Represents the account id.</param>
        /// <param name="notes">Represents the notes about a rate calculation.</param>
        /// <returns></returns>
        [OperationContract]
        double? CalculateInterest(int accountId, String notes);
    }
}
