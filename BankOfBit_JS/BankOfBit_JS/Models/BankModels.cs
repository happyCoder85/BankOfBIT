/* 
    Name: Jonathan Spurling
    Course: ADEV-3008(234120)
    Assignment: #1
    Date: January 8, 2023 
 */

using BankOfBit_JS.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.UI;
using Utility;
using WebGrease;
using static BankOfBit_JS.Models.BronzeState;

namespace BankOfBit_JS.Models
{


    /// <summary>
    /// Payee Model. Represents the Payee table in the database.
    /// </summary>
    public class Payee
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PayeeId { get; set; }

        [Required]
        [Display(Name = "Payee")]
        public String Description { get; set; }
    }

    ///// <summary>
    ///// Institution Model. Represents the Institution table in the database.
    ///// </summary>
    public class Institution
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int InstitutionId { get; set; }

        [Required]
        [Display(Name = "Number")]
        public int InstitutionNumber { get; set; }

        [Required]
        [Display(Name = "Institution")]
        public String Description { get; set; }
    }

    ///// <summary>
    ///// TransactionType Model. Represents the TransactionType table in the database.
    ///// </summary>
    public class TransactionType
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int TransactionTypeId { get; set; }

        [Required]
        [Display(Name = "Type")]
        public String Description { get; set; }

        // Navigational Property
        public virtual ICollection<Transaction> Transaction { get; set; }
    }

    ///// <summary>
    ///// Transaction Model. Represents the Transaction table in the database.
    ///// </summary>
    public class Transaction
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        [ForeignKey("BankAccount")]
        public int BankAccountId { get; set; }

        [ForeignKey("TransactionType")]
        public int TransactionTypeId { get; set; }

        [Display(Name = "Number")]
        public long TransactionNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:c2}", ApplyFormatInEditMode = true)]
        public double? Deposit { get; set; }

        [DisplayFormat(DataFormatString = "{0:c2}", ApplyFormatInEditMode = true)]
        public double? Withdrawal { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateCreated { get; set; }

        public String Notes { get; set; }

        // Navigational Properties
        public virtual TransactionType TransactionType { get; set; }
        public virtual BankAccount BankAccount { get; set; }

        /// <summary>
        /// Sets the next transaction number
        /// </summary>
        public void SetNextTransactionNumber()
        {
            TransactionNumber = (long)StoredProcedure.NextNumber("NextTransaction");
        }
    }

    ///// <summary>
    ///// NextUniqueNumber Model. Represents the basis for a unique number that is used in each sub-class.
    ///// </summary>
    public abstract class NextUniqueNumber
    {
        protected static BankOfBit_JSContext db = new BankOfBit_JSContext();

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int NextUniqueNumberId { get; set; }

        [Required]
        public long NextAvailableNumber { get; set; }
    }

    ///// <summary>
    ///// NextSavingsAccount. Represents the NextUniqueNumber class for Savings Accounts.
    ///// </summary>
    public class NextSavingsAccount : NextUniqueNumber
    {
        private static NextSavingsAccount nextSavingsAccount = db.NextSavingsAccounts.SingleOrDefault();

        /// <summary>
        /// Represents the Next available number for a savings account.
        /// </summary>
        private NextSavingsAccount()
        {
            NextAvailableNumber = 20000;
        }

        /// <summary>
        /// Returns an instance of the NextSavingsAccount number for a new account.
        /// </summary>
        /// <returns>Next savings account number</returns>
        public static NextSavingsAccount GetInstance()
        {
            if (nextSavingsAccount == null)
            {
                nextSavingsAccount = new NextSavingsAccount();
                db.NextSavingsAccounts.Add(nextSavingsAccount);
                db.SaveChanges();
            }

            return nextSavingsAccount;
        }
    }

    ///// <summary>
    ///// NextMortgageAccount. Represents the NextUniqueNumber class for Mortgage Accounts.
    ///// </summary>
    public class NextMortgageAccount : NextUniqueNumber
    {
        private static NextMortgageAccount nextMortgagesAccount = db.NextMortgageAccounts.SingleOrDefault();

        /// <summary>
        /// Represents the next available number for a Mortgage account.
        /// </summary>
        private NextMortgageAccount()
        {
            NextAvailableNumber = 200000;
        }

        /// <summary>
        /// Returns the next mortgage account number.
        /// </summary>
        /// <returns>The next mortgage account number for new mortgage accounts.</returns>
        public static NextMortgageAccount GetInstance()
        {
            if (nextMortgagesAccount == null)
            {
                nextMortgagesAccount = new NextMortgageAccount();
                db.NextMortgageAccounts.Add(nextMortgagesAccount);
                db.SaveChanges();
            }

            return nextMortgagesAccount;
        }
    }

    ///// <summary>
    ///// NextInvestmentAccount. Represents the NextUniqueNumber class for Investment Accounts.
    ///// </summary>
    public class NextInvestmentAccount : NextUniqueNumber
    {
        private static NextInvestmentAccount nextInvestmentAccount = db.NextInvestmentAccounts.SingleOrDefault();

        /// <summary>
        /// Represents the next investment account number.
        /// </summary>
        private NextInvestmentAccount()
        {
            NextAvailableNumber = 2000000;
        }

        /// <summary>
        /// Returns the next investment account number for new accounts.
        /// </summary>
        /// <returns>The next investment account number.</returns>
        public static NextInvestmentAccount GetInstance()
        {
            if (nextInvestmentAccount == null)
            {
                nextInvestmentAccount = new NextInvestmentAccount();
                db.NextInvestmentAccounts.Add(nextInvestmentAccount);
                db.SaveChanges();
            }

            return nextInvestmentAccount;
        }
    }

    ///// <summary>
    ///// NextChequingAccount. Represents the NextUniqueNumber class for Chequing Accounts.
    ///// </summary>
    public class NextChequingAccount : NextUniqueNumber
    {
        private static NextChequingAccount nextChequingAccount = db.NextChequingAccounts.SingleOrDefault();

        /// <summary>
        /// Represents the next chequing account number.
        /// </summary>
        private NextChequingAccount()
        {
            NextAvailableNumber = 20000000;
        }

        /// <summary>
        /// Returns the next chequing account number for new accounts.
        /// </summary>
        /// <returns>The next chequing account number.</returns>
        public static NextChequingAccount GetInstance()
        {
            if (nextChequingAccount == null)
            {
                nextChequingAccount = new NextChequingAccount();
                db.NextChequingAccounts.Add(nextChequingAccount);
                db.SaveChanges();
            }

            return nextChequingAccount;
        }
    }

    ///// <summary>
    ///// NextClient. Represents the NextUniqueNumber for a client.
    ///// </summary>
    public class NextClient : NextUniqueNumber
    {
        private static NextClient nextClient;

        /// <summary>
        /// Represents the next client number.
        /// </summary>
        private NextClient()
        {
            NextAvailableNumber = 20000000;
        }

        /// <summary>
        /// Returns the next client number for new clients.
        /// </summary>
        /// <returns>The next client number</returns>
        public static NextClient GetInstance()
        {
            if (nextClient == null)
            {
                nextClient = new NextClient();
                db.NextClients.Add(nextClient);
                db.SaveChanges();
            }
            
            return nextClient;
        }
    }

    ///// <summary>
    ///// NextTransaction. Represents the NextUniqueNumber class for the next transaction.
    ///// </summary>
    public class NextTransaction : NextUniqueNumber
    {
        private static NextTransaction nextTransactionAccount = db.NextTransactions.SingleOrDefault();

        /// <summary>
        /// Represents the next transaction number.
        /// </summary>
        private NextTransaction()
        {
            NextAvailableNumber = 700;
        }

        /// <summary>
        /// Returns the next transaction number for a transaction.
        /// </summary>
        /// <returns>The next transaction number.</returns>
        public static NextTransaction GetInstance()
        {
            if (nextTransactionAccount == null)
            {
                nextTransactionAccount = new NextTransaction();
                db.NextTransactions.Add(nextTransactionAccount);
                db.SaveChanges();
            }
            return nextTransactionAccount;
        }
    }

    /// <summary>
    /// Represents the stored procedure in the database
    /// </summary>
    public static class StoredProcedure
    {
        /// <summary>
        /// Returns the next number based on what input you provide.
        /// </summary>
        /// <param name="discriminator">String that represents the next account number for a selected account type.</param>
        /// <returns></returns>
        public static long? NextNumber(String discriminator)
        {
            try
            {
                // Connection string to the BankOfBit_JSContext Database
                SqlConnection connection = new SqlConnection("Data Source=localhost; " +
                              "Initial Catalog=BankOfBit_JSContext;Integrated Security=True");

                long? returnValue = 0;

                // Command to connect to the next_number stored procedure in the BankOfBit_JSContext Database
                // and identify it as a stored procedure.
                SqlCommand storedProcedure = new SqlCommand("next_number", connection);
                storedProcedure.CommandType = System.Data.CommandType.StoredProcedure;

                // Set the discriminator for the stored procedure
                storedProcedure.Parameters.AddWithValue("@Discriminator", discriminator);

                // The returned value for the stored procedure
                SqlParameter outputParameter = new SqlParameter("@NewVal", System.Data.SqlDbType.BigInt)
                {
                    Direction = System.Data.ParameterDirection.Output
                };
                storedProcedure.Parameters.Add(outputParameter);

                // Establish connection to database
                connection.Open();

                // Executes the stored procedure returning an integer specifying the number of rows inserted.
                storedProcedure.ExecuteNonQuery();

                // Close the connection to database
                connection.Close();

                // Sets the returnValue with the output parameter from the stored procedure
                returnValue = (long?)outputParameter.Value;

                return returnValue;
            }
            // Return null if null is returned
            catch (Exception)
            {
                return null;  
            };
        }
    }

        /// <summary>
        /// Client Model. Represents the Clients table in the database.
        /// </summary>
        public class Client
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ClientId { get; set; }

        [Display(Name = "Client\nNumber")]
        public long ClientNumber { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 1)]
        [Display(Name = "First\nName")]
        public String FirstName { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 1)]
        [Display(Name = "Last\nName")]
        public String LastName { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 1)]
        public string Address { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 1)]
        public string City { get; set; }

        [Required]
        [RegularExpression("^(N[BLSTU]|[AMN]B|[BQ]C|ON|PE|SK|YT)",
                           ErrorMessage = "Please enter a two-digit Province Abbreviation")]
        public string Province { get; set; }

        [Required]
        [Display(Name = "Date\nCreated")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateCreated { get; set; }

        public string Notes { get; set; }

        [Display(Name = "Name")]
        public String FullName
        {
            get
            {
                return String.Format("{0} {1}", FirstName, LastName);
            }
        }

        [Display(Name = "Address")]
        public string FullAddress
        {
            get
            {
                return String.Format("{0} {1} {2}", Address, City, Province);
            }
        }

        // Navigation property
        public virtual ICollection<BankAccount> BankAccount { get; set; }

        /// <summary>
        /// Sets the next client number
        /// </summary>
        public void SetNextClientNumber()
        {
            ClientNumber = (long)StoredProcedure.NextNumber("NextClient");
        }
    }

    /// <summary>
    /// AccountState Model. Represents the AccountStates table in the database.
    /// </summary>
    public abstract class AccountState
    {
        protected static BankOfBit_JSContext db = new BankOfBit_JSContext();

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AccountStateId { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:c2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Lower\nLimit")]
        public double LowerLimit { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:c2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Upper\nLimit")]
        public double UpperLimit { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:p2}", ApplyFormatInEditMode = true)]
        public double Rate { get; set; }

        [Display(Name = "Account\nState")]
        public string Description
        {
            get
            {
                return BusinessRules.ParseName(GetType().Name, "State");
            }
        }

        public abstract double RateAdjustment(BankAccount bankAccount);

        public abstract void StateChangeCheck(BankAccount bankAccount);

        // Navigation property
        public virtual ICollection<BankAccount> BankAccount { get; set; }
    }

    /// <summary>
    /// BronzeState Model. Represents the BronzeState in the AccountStates Table.
    /// </summary>
    public class BronzeState : AccountState
    {
        private static BronzeState bronzeState = db.BronzeStates.SingleOrDefault();

        private const int LOWER_LIMIT = 0;
        private const int UPPER_LIMIT = 5000;
        private const double RATE = 0.0100;

        /// <summary>
        /// Represents the BronzeState lower and upper limit plus the rate.
        /// </summary>
        private BronzeState()
        {
            LowerLimit = LOWER_LIMIT;
            UpperLimit = UPPER_LIMIT;
            Rate = RATE;
        }

        /// <summary>
        /// Returns an instance of BronzeState
        /// </summary>
        /// <returns>An instance of BronzeState</returns>
        public static BronzeState GetInstance()
        {
            if (bronzeState == null)
            {
                bronzeState = new BronzeState();
                db.BronzeStates.Add(bronzeState);
                db.SaveChanges();
            }

            return bronzeState;
        }

        /// <summary>
        /// Checks the Account Balance and moves the account to a different state if it is higher than the UpperLimit for BronzeState
        /// </summary>
        /// <param name="bankAccount">Bank Account to check</param>
        public override void StateChangeCheck(BankAccount bankAccount)
        {
            if (bankAccount.Balance > UPPER_LIMIT)
            {
                bankAccount.AccountStateId = SilverState.GetInstance().AccountStateId;
            }
        }

        /// <summary>
        /// Returns the correct rate based on account balance being above 0 or below 0.
        /// </summary>
        /// <param name="bankAccount">Bank Account to check.</param>
        /// <returns>Rate of bank account.</returns>
        public override double RateAdjustment(BankAccount bankAccount)
        {
            double adjustedRate = bronzeState.Rate;

            if (bankAccount.Balance <= 0)
            {
                adjustedRate = 0.055;
            }

            return adjustedRate;
        }
    }
    /// <summary>
    /// SilverState Model. Represents the SilverState in the AccountStates table.
    /// </summary>
    public class SilverState : AccountState
    {
        private static SilverState silverState = db.SilverStates.SingleOrDefault();

        private const int LOWER_LIMIT = 5000;
        private const int UPPER_LIMIT = 10000;
        private const double RATE = 0.0125;

        private SilverState()
        {
            LowerLimit = LOWER_LIMIT;
            UpperLimit = UPPER_LIMIT;
            Rate = RATE;
        }

        /// <summary>
        /// Returns an instance of SilverState
        /// </summary>
        /// <returns>An instance of SilverState</returns>
        public static SilverState GetInstance()
        {
            if (silverState == null)
            {
                silverState = new SilverState();
                db.SilverStates.Add(silverState);
                db.SaveChanges();
            }

            return silverState;
        }

        /// <summary>
        /// Checks if the balance of an account is higher or lower than its current state id.
        /// </summary>
        /// <param name="bankAccount">Bank Account to check.</param>
        public override void StateChangeCheck(BankAccount bankAccount)
        {
            if (bankAccount.Balance < LOWER_LIMIT)
            {
                bankAccount.AccountStateId = BronzeState.GetInstance().AccountStateId;
            }
            else if (bankAccount.Balance > UPPER_LIMIT)
            {
                bankAccount.AccountStateId = GoldState.GetInstance().AccountStateId;
            }
        }

        /// <summary>
        /// Returns the silver state rate.
        /// </summary>
        /// <param name="bankAccount">Bank Account to determine rate for.</param>
        /// <returns>Rate of bank account.</returns>
        public override double RateAdjustment(BankAccount bankAccount)
        {
            return silverState.Rate;
        }
    }

    /// <summary>
    /// GoldState Model. Represents the GoldStates in the AccountState table.
    /// </summary>
    public class GoldState : AccountState
    {
        private static GoldState goldState = db.GoldStates.SingleOrDefault();

        private const int LOWER_LIMIT = 10000;
        private const int UPPER_LIMIT = 20000;
        private const double RATE = 0.0200;

        private GoldState()
        {
            LowerLimit = LOWER_LIMIT;
            UpperLimit = UPPER_LIMIT;
            Rate = RATE;
        }

        /// <summary>
        /// Returns an instance of GoldState
        /// </summary>
        /// <returns>An instance of GoldState</returns>
        public static GoldState GetInstance()
        {
            if (goldState == null)
            {
                goldState = new GoldState();
                db.GoldStates.Add(goldState);
                db.SaveChanges();
            }

            return goldState;
        }

        /// <summary>
        /// Checks the balance of an account to ensure it is in the correct account state.
        /// </summary>
        /// <param name="bankAccount">Bank Account to check</param>
        public override void StateChangeCheck(BankAccount bankAccount)
        {
            if (bankAccount.Balance < LOWER_LIMIT)
            {
                bankAccount.AccountStateId = SilverState.GetInstance().AccountStateId;
            }
            else if (bankAccount.Balance > UPPER_LIMIT)
            {
                bankAccount.AccountStateId = PlatinumState.GetInstance().AccountStateId;
            }
        }

        /// <summary>
        /// Returns the rate for the account based on when it was created.
        /// </summary>
        /// <param name="bankAccount">The Bank Account to check balance.</param>
        /// <returns>The rate for the account.</returns>
        public override double RateAdjustment(BankAccount bankAccount)
        {
            double adjustedRate = goldState.Rate;

            if (DateTime.Today.Year - bankAccount.DateCreated.Year >= 10)
            {
                adjustedRate += .01;
            }

            return adjustedRate;
        }
    }

    /// <summary>
    /// PlatinumState Model. Represents the PlatinumState in the AccountState table.
    /// </summary>
    public class PlatinumState : AccountState
    {
        private static PlatinumState platinumState = db.PlatinumStates.SingleOrDefault();

        private const int LOWER_LIMIT = 20000;
        private const int UPPER_LIMIT = 0;
        private const double RATE = 0.0250;

        private PlatinumState()
        {
            LowerLimit = LOWER_LIMIT;
            UpperLimit = UPPER_LIMIT;
            Rate = RATE;
        }

        /// <summary>
        /// Returns an instance of PlatinumState
        /// </summary>
        /// <returns>An instance of PlatinumState</returns>
        public static PlatinumState GetInstance()
        {
            if (platinumState == null)
            {
                platinumState = new PlatinumState();
                db.PlatinumStates.Add(platinumState);
                db.SaveChanges();
            }

            return platinumState;
        }

        /// <summary>
        /// Checks a bank account balance to ensure it is in the right account state.
        /// </summary>
        /// <param name="bankAccount">Bank Account to check.</param>
        public override void StateChangeCheck(BankAccount bankAccount)
        {
            if (bankAccount.Balance < LOWER_LIMIT)
            {
                bankAccount.AccountStateId = GoldState.GetInstance().AccountStateId;
            }
        }

        /// <summary>
        /// Returns the rate based on how old account is and if the balance is double the upper limit.
        /// </summary>
        /// <param name="bankAccount">Bank account to adjust rate for.</param>
        /// <returns>Rate for the bank account.</returns>
        public override double RateAdjustment(BankAccount bankAccount)
        {
            double adjustedRate = platinumState.Rate;

            if (DateTime.Today.Year - bankAccount.DateCreated.Year >= 10)
            {
                adjustedRate += .01;
            }
            if (bankAccount.Balance > (LOWER_LIMIT * 2))
            {
                adjustedRate += .005;
            }

            return adjustedRate;
        }
    }

    /// <summary>
    /// BankAccount Model. Represents the BankAccounts table in the database.
    /// </summary>
    public abstract class BankAccount
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int BankAccountId { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }

        [ForeignKey("AccountState")]
        public int AccountStateId { get; set; }

        [Display(Name = "Account\nNumber")]
        public long AccountNumber { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:c2}", ApplyFormatInEditMode = true)]
        public double Balance { get; set; }

        [Required]
        [Display(Name = "Date\nCreated")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime DateCreated { get; set; }

        public string Notes { get; set; }

        public string Description
        {
            get
            {
                return BusinessRules.ParseName(GetType().Name, "Account");
            }
        }

        /// <summary>
        /// Ensures the proper state for the bank account.
        /// </summary>
        public void ChangeState()
        {
            BankOfBit_JSContext db = new BankOfBit_JSContext();

            AccountState state = db.AccountStates.Find(this.AccountStateId);
            int previousStateId = 0;

            while (previousStateId != state.AccountStateId)
            {
                state.StateChangeCheck(this);
                previousStateId = state.AccountStateId;
                state = db.AccountStates.Find(this.AccountStateId);
            }
        }

        // Navigation Properties.
        public virtual Client Client { get; set; }
        public virtual AccountState AccountState { get; set; }
        public virtual ICollection<Transaction> Transaction { get; set; }

        // Method that will be overrided in SavingsAccount, MortgageAccount, InvestmentAccount and ChequingAccount
        public abstract void SetNextAccountNumber();
    }

    /// <summary>
    /// SavingsAccount Model. Represents the SavingsAccount in the BankAccount table.
    /// </summary>
    public class SavingsAccount : BankAccount
    {
        [Required]
        [Display(Name = "Savings\nService\nCharges")]
        [DisplayFormat(DataFormatString = "{0:c2}", ApplyFormatInEditMode = true)]
        public double SavingsServiceCharges { get; set; }

        /// <summary>
        /// Set the next account number for a Savings account.
        /// </summary>
        public override void SetNextAccountNumber()
        {
            AccountNumber = (long)StoredProcedure.NextNumber("NextSavingsAccount");
        }
    }

    /// <summary>
    /// MortgageAccount Model. Represents the MortgageAccount in the BankAccount table.
    /// </summary>
    public class MortgageAccount : BankAccount
    {
        [Required]
        [Display(Name = "Mortgage\nRate")]
        [DisplayFormat(DataFormatString = "{0:p2}", ApplyFormatInEditMode = true)]
        public double MortgageRate { get; set; }

        [Required]
        public int Amortization { get; set; }

        /// <summary>
        /// Sets the next account number for a Mortgage account.
        /// </summary>
        public override void SetNextAccountNumber()
        {
            AccountNumber = (long)StoredProcedure.NextNumber("NextMortgageAccount");
        }
    }

    /// <summary>
    /// InvestmentAccount Model. Represents the InvestmentAccount in the BankAccount table.
    /// </summary>
    public class InvestmentAccount : BankAccount
    {
        [Required]
        [Display(Name = "Interest\nRate")]
        [DisplayFormat(DataFormatString = "{0:p2}", ApplyFormatInEditMode = true)]
        public double InterestRate { get; set; }

        /// <summary>
        /// Set the next account number for an Investment account.
        /// </summary>
        public override void SetNextAccountNumber()
        {
            AccountNumber = (long)StoredProcedure.NextNumber("NextInvestmentAccount");
        }
    }

    /// <summary>
    /// ChequingAccount Model. Represents the ChequingAccount in the BankAccount table.
    /// </summary>
    public class ChequingAccount : BankAccount
    {
        [Required]
        [Display(Name = "Chequing\nService\nCharges")]
        [DisplayFormat(DataFormatString = "{0:c2}", ApplyFormatInEditMode = true)]
        public double ChequingServiceCharges { get; set; }

        /// <summary>
        /// Sets the next account number for a Chequing account
        /// </summary>
        public override void SetNextAccountNumber()
        {
            AccountNumber = (long)StoredProcedure.NextNumber("NextChequingAccount");
        }
    }
}