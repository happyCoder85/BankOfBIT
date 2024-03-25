using BankOfBit_JS.Data;
using Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BankOfBit_JS.Models;

namespace WindowsBanking
{
    public partial class History : Form
    {
        ConstructorData constructorData;

        BankOfBit_JSContext db;


        /// <summary>
        /// Form can only be opened with a Constructor Data object
        /// containing client and account details.
        /// </summary>
        /// <param name="constructorData">Populated Constructor data object.</param>
        public History(ConstructorData constructorData)
        {
            //Given, more code to be added.
            InitializeComponent();
            this.constructorData = constructorData;

            clientBindingSource.DataSource = this.constructorData.Client;
            bankAccountBindingSource.DataSource = this.constructorData.BankAccount;
        }


        /// <summary>
        /// Return to the Client Data form passing specific client and 
        /// account information within ConstructorData.
        /// </summary>
        private void lnkReturn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ClientData client = new ClientData(constructorData);
            client.MdiParent = this.MdiParent;
            client.Show();
            this.Close();
        }
        /// <summary>
        /// Represents the History Load event
        /// </summary>
        private void History_Load(object sender, EventArgs e)
        {
            try
            {
                // Database context object
                db = new BankOfBit_JSContext();



                // Always display the form in the top right corner of the frame.
                this.Location = new Point(0, 0);

                // Add the mask for the account number based on account type
                accountNumberMaskedLabel.Mask = Utility.BusinessRules.AccountFormat(this.constructorData.BankAccount.Description);

                // Linq query to obtain the transactions + transactionTypes using a join (Traditional Linq Syntax)
                var gridViewRecords =
                    from transactions in db.Transactions
                    where transactions.BankAccountId == this.constructorData.BankAccount.BankAccountId
                    join transactionTypes in db.TransactionTypes on transactions.TransactionTypeId equals transactionTypes.TransactionTypeId
                    select new { DateCreated = transactions.DateCreated, TransactionType = transactionTypes.Description, Deposit = transactions.Deposit, Withdrawal = transactions.Withdrawal, Notes = transactions.Notes };

                // Bind the Linq Query to binding sources data source
                transactionBindingSource.DataSource = gridViewRecords.ToList();
            }
            catch (Exception ex)
            {
                string title = "Error!";
                string message = "Error: " + ex;
                
                MessageBox.Show(message, title);
            }
        }

    }
}
