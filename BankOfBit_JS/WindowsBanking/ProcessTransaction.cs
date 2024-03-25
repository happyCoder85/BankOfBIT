using BankOfBit_JS.Data;
using BankOfBit_JS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsBanking
{
    public partial class ProcessTransaction : Form
    {
        ConstructorData constructorData;
        BankOfBit_JSContext db;

        /// Form can only be opened with a Constructor Data object
        /// containing client and account details.
        /// </summary>
        /// <param name="constructorData">Populated Constructor data object.</param>
        public ProcessTransaction(ConstructorData constructorData)
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
        /// Always display the form in the top right corner of the frame.
        /// </summary>
        private void ProcessTransaction_Load(object sender, EventArgs e)
        {
            // Add the mask for the account number based on account type
            accountNumberMaskedLabel.Mask = Utility.BusinessRules.AccountFormat(this.constructorData.BankAccount.Description);

            db = new BankOfBit_JSContext();         

            try
            {
                // Query to fill the transaction type combo box
                IQueryable<TransactionType> type = db.TransactionTypes.Where(x => x.TransactionTypeId != 5 && x.TransactionTypeId != 6);

                this.Location = new Point(0, 0);

                // Fill the transaction type combo box with the query results
                descriptionComboBox.DataSource = type.ToList();
            }
            catch (Exception ex)
            {
                string message = "Error: " + ex;
                MessageBox.Show(message, "Error");
            }
        }

        private void descriptionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If the selected item in the description combo box is Deposit or Withdrawl
            if (descriptionComboBox.SelectedIndex == 0 || descriptionComboBox.SelectedIndex == 1)
            {
                cboPayeeAccount.Visible = false;
                lblPayeeAccount.Visible = false;
                lblNoAdditionalAccounts.Visible = false;

                lnkUpdate.Enabled = true;
                
            }
            // If the selected item in the description combo box is Bill Payment
            if (descriptionComboBox.SelectedIndex == 2)
            {
                cboPayeeAccount.Visible = true;
                lblPayeeAccount.Visible = true;
                lblNoAdditionalAccounts.Visible = false;
                lnkUpdate.Enabled = true;

                IQueryable<Payee> payees = db.Payees;

                cboPayeeAccount.DataSource = payees.ToList();

                
            }
            // Otherwise if the selected item is Transfer
            if (descriptionComboBox.SelectedIndex == 3)
            {
                
                // Query that selects all bank accounts in the bank account table related to the selected client and excluding the current account.
                IQueryable<BankAccount> bankAccounts = db.BankAccounts.Where(x => x.Client.ClientNumber == this.constructorData.Client.ClientNumber && x.AccountNumber != this.constructorData.BankAccount.AccountNumber);

                // If nothing is returned in the query
                if (bankAccounts == null)
                {
                    cboPayeeAccount.Visible = false;
                    lblPayeeAccount.Visible = false;
                    lblNoAdditionalAccounts.Visible = true;
                    lnkUpdate.Enabled = false;
                }
                // Otherwise
                else
                {
                    cboPayeeAccount.DataSource = bankAccounts.ToList();
                    cboPayeeAccount.DisplayMember = "AccountNumber";
                    cboPayeeAccount.Visible = true;
                    lblPayeeAccount.Visible = true;
                    lblNoAdditionalAccounts.Visible = false;
                    lnkUpdate.Enabled = true;
                }
            }
        }

        private void lnkUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OnlineBankingServiceServiceReference.TransactionManagerClient service = new OnlineBankingServiceServiceReference.TransactionManagerClient();

            // Check if the entered amount is a number.
            if (!Utility.Numeric.IsNumeric(txtAmount.Text, System.Globalization.NumberStyles.Float))
            {
                MessageBox.Show("Error: Please enter a numeric number", "Error!");
            }
            else
            {
                int id = descriptionComboBox.SelectedIndex;

                double inputAmount = double.Parse(txtAmount.Text);
                double balance = this.constructorData.BankAccount.Balance;
                int clientsAccountId = this.constructorData.BankAccount.BankAccountId;

                double amount = double.Parse(txtAmount.Text);

                string notes = "";

                // If Deposit is selected
                if (descriptionComboBox.SelectedIndex == 0)
                {
                    notes = "Online Banking Deposit To: " + this.constructorData.BankAccount.AccountNumber;

                    // Generate the deposit 

                    try
                    {
                        double newBalance = (double)service.Deposit(clientsAccountId, amount, notes);

                        if (newBalance != balance)
                        {
                            this.constructorData.BankAccount.Balance = newBalance;
                            balanceLabel1.Text = newBalance.ToString("C");
                        }
                        else
                        {
                            MessageBox.Show("Error completing Transaction", "Transaction Error");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex);
                    }
                }
                // If Withdrawal is selected
                else if (descriptionComboBox.SelectedIndex == 1)
                {
                    notes = "Online Banking Withdrawal From: " + this.constructorData.BankAccount.AccountNumber;
                    

                    // If balance is less than amount entered display error.
                    if (inputAmount > balance)
                    {
                        MessageBox.Show("Error: Insufficient Funds!", "Insufficient Funds!");
                    }
                    else
                    {
                        double newBalance = (double)service.Withdrawal(clientsAccountId, amount, notes);

                        // If the new balance is not the same as the old balance
                        if (newBalance != balance)
                        {
                            this.constructorData.BankAccount.Balance = newBalance;
                            balanceLabel1.Text = newBalance.ToString("C");
                        }
                        else
                        {
                            MessageBox.Show("Error completing Transaction", "Transaction Error");
                        }
                    }
                }
                else if (descriptionComboBox.SelectedIndex == 2)
                {
                    notes = "Online Banking Payment to: " + cboPayeeAccount.Text;

                    if (inputAmount > balance)
                    {
                        MessageBox.Show("Error: Insufficient Funds!", "Insufficient Funds!");
                    }
                    else
                    {
                        double newBalance = (double)service.BillPayment(clientsAccountId, amount, notes);

                        // If the new balance is not the same as the old balance
                        if (newBalance != balance)
                        {
                            this.constructorData.BankAccount.Balance = newBalance;
                            balanceLabel1.Text = newBalance.ToString("C");
                        }
                        else
                        {
                            MessageBox.Show("Error completing Transaction", "Transaction Error");
                        }
                    }
                }
                else
                {                   
                    notes = "Online Banking Transfer From: " + this.constructorData.BankAccount.AccountNumber + " To: " + cboPayeeAccount.Text;

                    int toAccountNumber = int.Parse(cboPayeeAccount.Text);
                    BankAccount toAccount = (BankAccount)db.BankAccounts.Where(x => x.AccountNumber == toAccountNumber).SingleOrDefault();

                    int toAccountId = toAccount.BankAccountId;

                    if (inputAmount > balance)
                    {
                        MessageBox.Show("Error: Insufficient Funds!", "Insufficient Funds!");
                    }
                    else
                    {                       
                        double newBalance = (double)service.Transfer(clientsAccountId, toAccountId, amount, notes);

                        // If the new balance is not the same as the old balance
                        if (newBalance != balance)
                        {
                            this.constructorData.BankAccount.Balance = newBalance;
                            balanceLabel1.Text = newBalance.ToString("C");
                        }
                        else
                        {
                            MessageBox.Show("Error completing Transaction", "Transaction Error");
                        }
                    }
                }
            }
        }
    }
}
