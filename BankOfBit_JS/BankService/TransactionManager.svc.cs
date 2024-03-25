using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BankOfBit_JS;
using BankOfBit_JS.Data;
using BankOfBit_JS.Models;
using Utility;

namespace BankService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TransactionManager" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TransactionManager.svc or TransactionManager.svc.cs at the Solution Explorer and start debugging.
    public class TransactionManager : ITransactionManager
    {
        // Instance of the db context object
        public BankOfBit_JSContext db = new BankOfBit_JSContext();
        
        /// <summary>
        /// Updates the account balance and returns the new balance based on arguments supplied.
        /// </summary>
        /// <param name="accountId">Represents the account id.</param>
        /// <param name="amount">Represents the amount to update the account with.</param>
        /// <returns>Returns an updated balance of an account.</returns>
        private double? UpdateBalance(int accountId, double amount)
        {
            try
            {
                BankAccount account = db.BankAccounts.Find(accountId);

                account.Balance += amount;
                account.ChangeState();
                db.SaveChanges();
               
                return account.Balance;
            }
            catch(Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a transaction based from the inputed arguments.
        /// </summary>
        /// <param name="accountId">Represents the account id.</param>
        /// <param name="amount">Represents the amount of the transaction.</param>
        /// <param name="transactionTypeId">Represents the type of transaction.</param>
        /// <param name="notes">Represents the notes regarding a transaction.</param>
        private void CreateTransaction(int accountId, double amount, int transactionTypeId, String notes)
        {
            Transaction transaction = new Transaction();

            transaction.BankAccountId = accountId;
            transaction.TransactionTypeId = transactionTypeId;

            // If amount entered is negative set the withdrawal amount to the absolute value of the amount entered.
            if(amount < 0)
            {
                transaction.Deposit = null;
                transaction.Withdrawal = Math.Abs(amount) * -1;
            }
            // Otherwise set the deposit amount to the amount entered.
            else
            {
                transaction.Deposit = Math.Abs(amount);
                transaction.Withdrawal = null;
            }

            transaction.DateCreated = DateTime.Now;
            transaction.Notes = notes;
            transaction.SetNextTransactionNumber();

            db.Transactions.Add(transaction);
            db.SaveChanges();
        }

        /// <summary>
        /// Bill payment where it withdraws based off the arguments supplied.
        /// </summary>
        /// <param name="accountId">Represents the account id.</param>
        /// <param name="amount">Represents the amount of the bill payment.</param>
        /// <param name="notes">Represents the notes regarding the bill payment.</param>
        /// <returns></returns>
        public double? BillPayment(int accountId, double amount, string notes)
        {
            BankAccount account = db.BankAccounts.Find(accountId);

            try
            {
                UpdateBalance(accountId, Math.Abs(amount) * -1);
                CreateTransaction(accountId, Math.Abs(amount) * -1, (int)TransactionTypeValues.BILL_PAYMENT, notes);
                return account.Balance;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Calculates the interest of an account.
        /// </summary>
        /// <param name="accountId">Represents the account id.</param>
        /// <param name="notes">Represents the notes regarding an interest calculation.</param>
        /// <returns></returns>
        public double? CalculateInterest(int accountId, string notes)
        {
            BankAccount account = db.BankAccounts.Find(accountId);

            try
            {
                double rate = account.AccountState.RateAdjustment(account);
                double balance = account.Balance;
                double interest = (rate * balance * 1) / 12;

                UpdateBalance(accountId, interest);
                CreateTransaction(accountId, interest, (int)TransactionTypeValues.INTEREST, notes);
                
                return account.Balance;
            }
            catch(Exception)
            {
                return null;
            }


            
        }

        /// <summary>
        /// Returns an updated account balance after a deposit is made.
        /// </summary>
        /// <param name="accountId">Represents the account id.</param>
        /// <param name="amount">Represents the amount of the deposit.</param>
        /// <param name="notes">Represents the notes regarding a deposit.</param>
        /// <returns>Updated account balance.</returns>
        public double? Deposit(int accountId, double amount, string notes)
        {
            BankAccount account = db.BankAccounts.Find(accountId);

            try
            {
                UpdateBalance(accountId, Math.Abs(amount));
                CreateTransaction(accountId, Math.Abs(amount), (int)TransactionTypeValues.DEPOSIT, notes);
                return account.Balance;
            }
            catch(Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Transfer money from one bank account to another.
        /// </summary>
        /// <param name="fromAccountId">Represents the senders account id.</param>
        /// <param name="toAccountId">Represents the recipients account id.</param>
        /// <param name="amount">Represents the amount of the transfer.</param>
        /// <param name="notes">Represents the notes regarding the transfer.</param>
        /// <returns></returns>
        public double? Transfer(int fromAccountId, int toAccountId, double amount, string notes)
        {
            BankAccount accountFrom = db.BankAccounts.Find(fromAccountId);
            BankAccount accountTo = db.BankAccounts.Find(toAccountId);

            try
            {
                UpdateBalance(fromAccountId, Math.Abs(amount) * -1);
                CreateTransaction(fromAccountId, Math.Abs(amount) * -1, (int)TransactionTypeValues.TRANSFER, notes);

                UpdateBalance(toAccountId, Math.Abs(amount));
                CreateTransaction(toAccountId, Math.Abs(amount), (int)TransactionTypeValues.TRANSFER_RECIPIENT, notes);

                return accountFrom.Balance;
            }
            catch(Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Returns an updated account balance after a withdrawal.
        /// </summary>
        /// <param name="accountId">Represents the account id.</param>
        /// <param name="amount">Represents the withdrawal amount.</param>
        /// <param name="notes">Represents the notes regarding a withdrawal.</param>
        /// <returns>Updated account balance.</returns>
        public double? Withdrawal(int accountId, double amount, string notes)
        {
            BankAccount account = db.BankAccounts.Find(accountId);

            try
            {
                UpdateBalance(accountId, amount * -1);
                CreateTransaction(accountId, Math.Abs(amount) * -1, (int)TransactionTypeValues.WITHDRAWAL, notes);
                return account.Balance;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
