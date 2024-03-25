using BankOfBit_JS.Controllers;
using BankOfBit_JS.Data;
using BankOfBit_JS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineBanking
{
    public partial class CreateTransaction : System.Web.UI.Page
    {
        BankOfBit_JSContext db; 

        /// <summary>
        /// Represents the page load event for the create transaction page
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                db = new BankOfBit_JSContext();

                if (!IsPostBack)
                {
                    if (!this.Page.User.Identity.IsAuthenticated)
                    {
                        Response.Redirect("~/Account/Login.aspx");
                    }
                    else
                    {
                        // Right align the amount textbox
                        txtAmount.Style.Add("text-align", "right");

                        // Populate Account Number and Balance using stored session variables
                        string selectedAccount = Session["SessionSelectedAccount"].ToString();
                        lblCreateTransactionAccountNumber.Text = selectedAccount;

                        double balance = (double)Session["SessionAccountBalance"];
                        lblCreateTransactionBalance.Text = balance.ToString("C");

                        // Bind the Transaction Type DropDownList with a LINQ Query obtaining Bill Payment and Transfer
                        IQueryable<TransactionType> transactionTypes = db.TransactionTypes.Where(x => x.TransactionTypeId == 3 || x.TransactionTypeId == 4);

                        ddlTransactionType.DataSource = transactionTypes.ToList();
                        ddlTransactionType.DataTextField = "Description";
                        ddlTransactionType.DataValueField = "Description";
                        ddlTransactionType.DataBind();

                        bindPayees(db, ddlTo);
                    }
                }
            }
            catch (Exception ex)
            {
                // If exception occurs display an error message
                lblCreateTransactionError.Text = "An error has occured : " + ex.Message;
                lblCreateTransactionError.Visible = true;
            }
        }

        /// <summary>
        /// Represents the selected index changed event for the Transaction type drop down list
        /// </summary>
        protected void ddlTransactionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If Bill Payment is selected
            if (ddlTransactionType.SelectedIndex == 0)
            {
                // Clear previous databindings on the Payee/Account DropDownList
                clearDropDownListDataBinding(ddlTo);

                bindPayees(db, ddlTo);
            }
            // If Transfer is selected
            else
            {
                // Clear previous databindings on the Payee/Account DropDownList
                clearDropDownListDataBinding(ddlTo);

                Client client = (Client)Session["SessionClient"];
                string account = (string)Session["SessionSelectedAccount"];
                long accountNumber = long.Parse(account);

                IQueryable<BankAccount> accounts = db.BankAccounts.Where(x => x.ClientId == client.ClientId).Where(x => x.AccountNumber != accountNumber);

                ddlTo.DataSource = accounts.ToList();
                ddlTo.DataTextField = "AccountNumber";
                ddlTo.DataValueField = "BankAccountId";

                ddlTo.DataBind();
            }
        }

        /// <summary>
        /// Represents the click event when the user clicks Return to Accounts.
        /// </summary>
        protected void lbReturnToAccounts_Click(object sender, EventArgs e)
        {
            // If user clicks on the Return to Account Listing link, take them to the AccountListing.aspx page
            Response.Redirect("AccountListing.aspx", false);
        }

        /// <summary>
        /// Represents the Complete Tranaction click event
        /// </summary>
        protected void lbCompleteTransaction_Click(object sender, EventArgs e)
        {
            try
            {
                // Enable the Required Field Validator control
                rfvRequiredFieldValidator.Enabled = true;

                // Ensure data in the textbox is validated against the Required Field Validator control
                Page.Validate();

                // If the page is valid
                if (Page.IsValid)
                {
                    double balance = (double)Session["SessionAccountBalance"];
                    double enteredAmount = double.Parse(txtAmount.Text);
                    IQueryable<BankAccount> accounts = (IQueryable<BankAccount>)Session["SessionClientAccounts"];
                    string notes = "";

                    int accountId = int.Parse(lblCreateTransactionAccountNumber.Text);

                    BankAccount current = accounts.Where(x => x.AccountNumber == accountId).SingleOrDefault();

                    // Service Instance
                    BankServiceServiceReference.TransactionManagerClient service = new BankServiceServiceReference.TransactionManagerClient();

                    // If there is insufficient funds
                    if (balance < enteredAmount)
                    {
                        try
                        {
                            throw new Exception("Insufficient funds to complete the requested transaction.");
                        }
                        catch (Exception ex)
                        {
                            // If exception occurs display an error message
                            lblCreateTransactionError.Text = "An error has occured : " + ex.Message;
                            lblCreateTransactionError.Visible = true;
                        }
                    }
                    else
                    {
                        // If the selected transaction type is Bill Payment
                        if (ddlTransactionType.SelectedIndex == 0)
                        {
                            notes = "Online Banking Payment to: " + ddlTo.Text;
                            double? newBalance = service.BillPayment(current.BankAccountId, enteredAmount, notes);

                            if (isValidTransaction(newBalance, lblCreateTransactionError))
                            {
                                double updatedBalance = (double)newBalance;

                                lblCreateTransactionBalance.Text = updatedBalance.ToString("C");
                            }
                        }
                        // If the selected transaction type is Transfer
                        else
                        {
                            notes = "Online Banking Transfer From: " + lblCreateTransactionAccountNumber.Text + " To: " + ddlTo.Text;

                            int toAccountId = int.Parse(ddlTo.SelectedValue);

                            double? newBalance = service.Transfer(current.BankAccountId, toAccountId, enteredAmount, notes);

                            if (isValidTransaction(newBalance, lblCreateTransactionError))
                            {
                                double updatedBalance = (double)newBalance;
                                lblCreateTransactionBalance.Text = updatedBalance.ToString("C");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblCreateTransactionError.Text = "Error: " + ex;
                lblCreateTransactionError.Visible = true;
            }
        }

        /// <summary>
        /// Clear the data binding for a drop down list
        /// </summary>
        /// <param name="list">Drop down list ID</param>
        private static void clearDropDownListDataBinding(DropDownList id)
        {
            id.DataSource = null;
            id.DataTextField = null;
            id.DataValueField = null;

        }

        /// <summary>
        /// Bind the payees to the payees dropdownlist.
        /// </summary>
        /// <param name="db">Database object</param>
        /// <param name="list">Drop Down List wanting to bind to</param>
        private static void bindPayees(BankOfBit_JSContext db, DropDownList list)
        {
            IQueryable<Payee> payees = db.Payees;

            list.DataSource = payees.ToList();
            list.DataTextField = "Description";
            list.DataValueField = "Description";
            list.DataBind();
        }
        
        private static bool isValidTransaction(double? returnedAmount, Label label)
        {
            if (returnedAmount == null)
            {
                try
                {
                    throw new Exception("cannot process transaction. Please try again later.");
                }
                catch (Exception ex)
                {
                    label.Text = "Error: " + ex;
                    label.Visible = true;
                }
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
