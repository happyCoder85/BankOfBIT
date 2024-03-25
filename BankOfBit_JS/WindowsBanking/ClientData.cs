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
    public partial class ClientData : Form
    {
        ConstructorData constructorData = new ConstructorData();

        BankOfBit_JSContext db = new BankOfBit_JSContext();

        /// <summary>
        /// This constructor will execute when the form is opened
        /// from the MDI Frame.
        /// </summary>
        public ClientData()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This constructor will execute when the form is opened by
        /// returning from the History or Transaction forms.
        /// </summary>
        /// <param name="constructorData">Populated ConstructorData object.</param>
        public ClientData(ConstructorData constructorData)
        {
            //Given:
            InitializeComponent();
            this.constructorData = constructorData;

            //More code to be added:
            clientNumberMaskedTextBox.Text = this.constructorData.Client.ClientNumber.ToString();
            clientNumberMaskedTextBox_Leave(null, null);

        }

        /// <summary>
        /// Open the Transaction form passing ConstructorData object.
        /// </summary>
        private void lnkProcess_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PopulateConstructorData();

            //Given, more code to be added.
            ProcessTransaction transaction = new ProcessTransaction(constructorData);
            transaction.MdiParent = this.MdiParent;
            transaction.Show();
            this.Close();
        }

        /// <summary>
        /// Open the History form passing ConstructorData object.
        /// </summary>
        private void lnkDetails_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PopulateConstructorData();

            //Given, more code to be added.
            History history = new History(constructorData);
            history.MdiParent = this.MdiParent;
            history.Show();
            this.Close();
        }

        /// <summary>
        /// Always display the form in the top right corner of the frame.
        /// </summary>
        private void ClientData_Load(object sender, EventArgs e)
        {
            this.Location = new Point(0,0);
        }

        /// <summary>
        /// Represents the event handler for when the textbox is taken out of focus
        /// </summary>
        private void clientNumberMaskedTextBox_Leave(object sender, EventArgs e)
        {
            if (clientNumberMaskedTextBox.Text.Length == 8)
            {
                int clientNumber = int.Parse(clientNumberMaskedTextBox.Text);

                Client client = (db.Clients.Where(x => x.ClientNumber == clientNumber)).SingleOrDefault();
                
                // If no record is found display a error message
                if (client == null)
                {
                    MessageBox.Show("Client Number: " + clientNumberMaskedTextBox.Text +
                                    " does not exist.", "Invalid Client Number");

                    lnkDetails.Enabled = false;
                    lnkProcess.Enabled = false;

                    clientNumberMaskedTextBox.Focus();

                    clientBindingSource.DataSource = typeof(Client);
                    bankAccountBindingSource.DataSource = typeof(BankAccount);
                }
                else
                {
                    // Bind the client data 
                    clientBindingSource.DataSource = client;
                    
                    // Retreive the bank account records for the current client record selected
                    IQueryable<BankAccount> bankAccounts = db.BankAccounts.Where(x => x.ClientId == client.ClientId);

                    if (bankAccounts == null)
                    {
                        // Disable the link buttons
                        lnkDetails.Enabled = false;
                        lnkProcess.Enabled = false;

                        // Clear the bank accounts binding source
                        bankAccountBindingSource.DataSource = typeof(BankAccount);
                    }
                    else
                    {
                        // Bind the Bank Accounts
                        bankAccountBindingSource.DataSource = bankAccounts.ToList();

                        // Enable link buttons
                        lnkDetails.Enabled = true;
                        lnkProcess.Enabled = true;   
                    }
                }
            }
        }

        /// <summary>
        /// Populates the constructor data object
        /// </summary>
        private void PopulateConstructorData()
        {
            this.constructorData.Client = (Client)clientBindingSource.Current;
            this.constructorData.BankAccount = (BankAccount)bankAccountBindingSource.Current;
        }
    }
}
