using BankOfBit_JS.Data;
using BankOfBit_JS.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineBanking
{
    public partial class TransactionListing : System.Web.UI.Page
    {
        /// <summary>
        /// Represents the page loading event for the Transaction listings
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            
            BankOfBit_JSContext db;

            try
            {
                db = new BankOfBit_JSContext();

                // If the page has not been reloaded
                if (!IsPostBack)
                {
                    // If the user is not authenticated, redirect to the login screen
                    if (!this.Page.User.Identity.IsAuthenticated)
                    {
                        Response.Redirect("~/Account/Login.aspx");
                    }
                    else
                    {
                        // Get client number from session variable
                        string clientNumber = (string)Session["SessionClientNumber"];
                        long clientNumberInt = long.Parse(clientNumber);

                        // Obtain Client object that corresponds to the client number using Lambda syntax
                        Client client = (db.Clients.Where(x => x.ClientNumber == clientNumberInt)).SingleOrDefault();

                        // Set the name label to the Full Name of the Client Object
                        lblTransactionClientName.Text = client.FullName;

                        // Obtain the clients selected account number
                        string selectedAccount = (string)Session["SessionSelectedAccount"];
                        long selectedAccountInt = long.Parse(selectedAccount);

                        // Populate the Account Number
                        lblTransactionAccountNumber.Text = selectedAccount;

                        // Obtain the bank account records associated with client that has been selected
                        BankAccount bankAccount = db.BankAccounts.Where(x => x.AccountNumber == selectedAccountInt).SingleOrDefault();

                        // Obtain the bank account balance and convert it to a string
                        double balance = bankAccount.Balance;
                        string normalBalance = balance.ToString();
                        string balanceCurrency = balance.ToString("C");

                        // Record the account balance in a session variable
                        Session["SessionAccountBalance"] = balance;

                        // Display the balance
                        lblTransactionAccountBalance.Text = balanceCurrency;

                        // Obtain all the transaction records associated with the selected Bank Account
                        IQueryable <Transaction> transactions = db.Transactions.Where(x => x.BankAccountId == bankAccount.BankAccountId);

                        // Bind the results of the LINQ query to the GridView control
                        gvTransactions.DataSource = transactions.ToList();
                        gvTransactions.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                // If an error occurs display an error message
                lblTransactionError.Text = "An error has occured : " + ex.Message;
                lblTransactionError.Visible = true;
            }
        }

        /// <summary>
        /// Represents the Create Transaction link click
        /// </summary>
        protected void lbTransactionWebForm_Click(object sender, EventArgs e)
        {
            // If user clicks on the Create Transaction link, take them to the CreateTransaction.aspx page
            Response.Redirect("CreateTransaction.aspx", false);
        }

        /// <summary>
        /// Represents the return to account listing link click
        /// </summary>
        protected void lbAccountListingWebForm_Click(object sender, EventArgs e)
        {
            // If user clicks on the Return to Account Listing link, take them to the AccountListing.aspx page
            Response.Redirect("AccountListing.aspx", false);
        }
    }
}