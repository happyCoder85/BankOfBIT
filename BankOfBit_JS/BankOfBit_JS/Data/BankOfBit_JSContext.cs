using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BankOfBit_JS.Data
{
    public class BankOfBit_JSContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public BankOfBit_JSContext() : base("name=BankOfBit_JSContext")
        {
        }

        public System.Data.Entity.DbSet<BankOfBit_JS.Models.Client> Clients { get; set; }

        public System.Data.Entity.DbSet<BankOfBit_JS.Models.AccountState> AccountStates { get; set; }

        public System.Data.Entity.DbSet<BankOfBit_JS.Models.BronzeState> BronzeStates { get; set; }

        public System.Data.Entity.DbSet<BankOfBit_JS.Models.SilverState> SilverStates { get; set; }

        public System.Data.Entity.DbSet<BankOfBit_JS.Models.GoldState> GoldStates { get; set; }

        public System.Data.Entity.DbSet<BankOfBit_JS.Models.PlatinumState> PlatinumStates { get; set; }

        public System.Data.Entity.DbSet<BankOfBit_JS.Models.BankAccount> BankAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBit_JS.Models.ChequingAccount> ChequingAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBit_JS.Models.InvestmentAccount> InvestmentAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBit_JS.Models.MortgageAccount> MortgageAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBit_JS.Models.SavingsAccount> SavingsAccounts { get; set; }

        // Added for assignment 4
        public System.Data.Entity.DbSet<BankOfBit_JS.Models.NextUniqueNumber> NextUniqueNumbers { get; set; }

        public System.Data.Entity.DbSet<BankOfBit_JS.Models.NextSavingsAccount> NextSavingsAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBit_JS.Models.NextMortgageAccount> NextMortgageAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBit_JS.Models.NextInvestmentAccount> NextInvestmentAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBit_JS.Models.NextChequingAccount> NextChequingAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBit_JS.Models.NextTransaction> NextTransactions { get; set; }

        public System.Data.Entity.DbSet<BankOfBit_JS.Models.NextClient> NextClients { get; set; }

        public System.Data.Entity.DbSet<BankOfBit_JS.Models.Payee> Payees { get; set; }

        public System.Data.Entity.DbSet<BankOfBit_JS.Models.Institution> Institutions { get; set; }

        public System.Data.Entity.DbSet<BankOfBit_JS.Models.TransactionType> TransactionTypes { get; set; }

        public System.Data.Entity.DbSet<BankOfBit_JS.Models.Transaction> Transactions { get; set; }
    }
}
