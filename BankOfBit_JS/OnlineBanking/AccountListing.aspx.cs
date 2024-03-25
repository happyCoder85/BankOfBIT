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
    public partial class AccountListing : System.Web.UI.Page
    {
        BankOfBit_JSContext db;

        /// <summary>
        /// Represents the page load event for the account listing page
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                db = new BankOfBit_JSContext();

                if (!IsPostBack)
                {
                    // If the user is not authenticated, redirect to the login screen
                    if (!this.Page.User.Identity.IsAuthenticated)
                    {
                        Response.Redirect("~/Account/Login.aspx");
                    }
                    else
                    {
                        // Get client number from logon id
                        string user = Page.User.Identity.Name;
                        string clientNumber = user.Substring(0, user.IndexOf('@'));

                        long clientNumberInt = long.Parse(clientNumber);

                        // Obtain Client object that corresponds to the client number using Lambda syntax
                        Client client = (db.Clients.Where(x => x.ClientNumber == clientNumberInt)).SingleOrDefault();

                        Session["SessionClient"] = client;

                        // Save the client record to a session variable
                        Session["SessionClientNumber"] = clientNumber;

                        // Set the name label to the Full Name of the Client Object
                        lblClientName.Text = client.FullName;

                        // Obtain a collection of bank account records associated with client
                        IQueryable<BankAccount> bankAccounts = db.BankAccounts.Where(x => x.ClientId == client.ClientId);

                        // Bind the results of the LINQ query to the GridView control
                        gvClientAccounts.DataSource = bankAccounts.ToList();
                        gvClientAccounts.DataBind();

                        // Save the clients bank accounts to a session variable
                        Session["SessionClientAccounts"] = bankAccounts;
                    }
                }
            }
            catch (Exception ex)
            {
                // If exception occurs display an error message
                lblError.Text = "An error has occured : " + ex.Message;
                lblError.Visible = true;
            }
        }

        /// <summary>
        /// Represents the Selected Index Changed event when an account is clicked on
        /// </summary>
        protected void gvClientAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Save the selected account in a session variable
            Session["SessionSelectedAccount"] = gvClientAccounts.Rows[gvClientAccounts.SelectedIndex].Cells[1].Text;

            // Redirect to the TransactionListing Web form
            Response.Redirect("/TransactionListing.aspx");
        }
    }
}