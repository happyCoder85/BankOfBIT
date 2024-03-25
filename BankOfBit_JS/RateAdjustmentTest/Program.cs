using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankOfBit_JS;
using BankOfBit_JS.Data;
using BankOfBit_JS.Models;
using Utility;

namespace RateAdjustmentTest
{
    public class Program
    {
        private static BankOfBit_JSContext db = new BankOfBit_JSContext();

        static void Main(string[] args)
        {
            Console.WriteLine("Rate Adjustment BronzeState Positive Test");
            Rate_Adjustment_BronzeState_Positive_Balance();

            Console.WriteLine("\nRate Adjustment BronzeState Negative Balance");

            Rate_Adjustment_BronzeState_Negative_Balance();

            Console.WriteLine("\nRate Adjustment SilverState Positive Balance");
            Rate_Adjustment_SilverState_Positive_Balance();

            Console.WriteLine("\nRate Adjustment GoldState Less Than Ten Years Old");
            Rate_Adjustment_GoldState_Less_Than_Ten_Years_Old();

            Console.WriteLine("\nRate Adjustment GoldState Ten Years Or Older");
            
            Rate_Adjustment_GoldState_Ten_Years_Or_Older();

            Console.WriteLine("\nRate Adjustment PlatinumState Normal Rate");
            Rate_Adjustment_PlatinumState_Standard_Rate();

            Console.WriteLine("\nRate Adjustment PlatinumState 10 years old");
            Rate_Adjustment_PlatinumState_Ten_Years_Old();

            Console.WriteLine("\nRate Adjustment PlatinumState Double Upper Limit Less Than Ten Years");
            Rate_Adjustment_PlatinumState_Double_Upper_Limit_Less_Than_Ten_Years();

            Console.WriteLine("\nRate Adjustment PlatinumState Double Upper Limit And Greater Than Ten Years");
            Rate_Adjustment_PlatinumState_Double_Upper_Limit_Greater_Than_Ten_Years();

            Console.ReadKey();
        }

        static void Rate_Adjustment_BronzeState_Positive_Balance()
        {
            //Set up the test account
            BankAccount account = db.BankAccounts.Find(1);
            account.Balance = 1000;
            account.AccountStateId = 1;
            account.DateCreated = DateTime.Now;
            account.ChangeState();
            db.SaveChanges();

            //Get instance of AccountState
            AccountState state = db.AccountStates.Find(account.AccountStateId);

            double expected = 0.010;
            double actual = state.RateAdjustment(account);

            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Actual:   " + actual);

        }

        static void Rate_Adjustment_BronzeState_Negative_Balance()
        {
            //Set up the test account
            BankAccount account = db.BankAccounts.Find(1);
            account.Balance = -1000;
            account.AccountStateId = 2;
            account.DateCreated = DateTime.Now;
            account.ChangeState();
            db.SaveChanges();

            //Get instance of AccountState
            AccountState state = db.AccountStates.Find(account.AccountStateId);

            double expected = 0.055;
            double actual = state.RateAdjustment(account);

            Console.WriteLine(state);
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Actual:   " + actual);
        }

        static void Rate_Adjustment_SilverState_Positive_Balance()
        {
            //Set up the test account
            BankAccount account = db.BankAccounts.Find(1);
            account.Balance = 6000;
            account.AccountStateId = 2;
            account.DateCreated = DateTime.Now;
            account.ChangeState();
            db.SaveChanges();

            //Get instance of AccountState
            AccountState state = db.AccountStates.Find(account.AccountStateId);

            double expected = 0.0125;
            double actual = state.RateAdjustment(account);

            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Actual:   " + actual);
        }

        static void Rate_Adjustment_GoldState_Less_Than_Ten_Years_Old()
        {
            //Set up the test account
            BankAccount account = db.BankAccounts.Find(1);
            account.Balance = 11000;
            account.AccountStateId = 3;
            account.DateCreated = DateTime.Now.AddYears(-6);
            account.ChangeState();
            db.SaveChanges();

            //Get instance of AccountState
            AccountState state = db.AccountStates.Find(account.AccountStateId);

            double expected = 0.020;
            double actual = state.RateAdjustment(account);

            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Actual:   " + actual);
        }

        static void Rate_Adjustment_GoldState_Ten_Years_Or_Older()
        {
            //Set up the test account
            BankAccount account = db.BankAccounts.Find(1);
            account.Balance = 11000;
            account.AccountStateId = 3;
            account.DateCreated = DateTime.Now.AddYears(-10);
            account.ChangeState();
            db.SaveChanges();

            //Get instance of AccountState
            AccountState state = db.AccountStates.Find(account.AccountStateId);

            double expected = 0.020;
            double actual = state.RateAdjustment(account);

            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Actual:   " + actual);
        }

        static void Rate_Adjustment_PlatinumState_Standard_Rate()
        {
            //Set up the test account
            BankAccount account = db.BankAccounts.Find(1);
            account.Balance = 21000;
            account.AccountStateId = 4;
            account.DateCreated = DateTime.Now.AddYears(-6);
            account.ChangeState();
            db.SaveChanges();

            //Get instance of AccountState
            AccountState state = db.AccountStates.Find(account.AccountStateId);

            double expected = 0.0250;
            double actual = state.RateAdjustment(account);

            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Actual:   " + actual);
        }

        static void Rate_Adjustment_PlatinumState_Ten_Years_Old()
        {
            //Set up the test account
            BankAccount account = db.BankAccounts.Find(1);
            account.Balance = 21000;
            account.AccountStateId = 4;
            account.DateCreated = DateTime.Now.AddYears(-10);
            account.ChangeState();
            db.SaveChanges();

            //Get instance of AccountState
            AccountState state = db.AccountStates.Find(account.AccountStateId);

            double expected = 0.0350;
            double actual = state.RateAdjustment(account);

            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Actual: " + actual);
        }

        static void Rate_Adjustment_PlatinumState_Double_Upper_Limit_Less_Than_Ten_Years()
        {
            //Set up the test account
            BankAccount account = db.BankAccounts.Find(1);
            account.Balance = 40001;
            account.AccountStateId = 4;
            account.DateCreated = DateTime.Now.AddYears(-6);
            account.ChangeState();
            db.SaveChanges();

            //Get instance of AccountState
            AccountState state = db.AccountStates.Find(account.AccountStateId);

            double expected = 0.0300;
            double actual = state.RateAdjustment(account);

            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Actual: " + actual);
        }

        static void Rate_Adjustment_PlatinumState_Double_Upper_Limit_Greater_Than_Ten_Years()
        {
            //Set up the test account
            BankAccount account = db.BankAccounts.Find(1);
            account.Balance = 40001;
            account.AccountStateId = 4;
            account.DateCreated = DateTime.Now.AddYears(-10);
            account.ChangeState();
            db.SaveChanges();

            //Get instance of AccountState
            AccountState state = db.AccountStates.Find(account.AccountStateId);

            double expected = 0.0400;
            double actual = state.RateAdjustment(account);

            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Actual: " + actual);
        }

    }   
}
