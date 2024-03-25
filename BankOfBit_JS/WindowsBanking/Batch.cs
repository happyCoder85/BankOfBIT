using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using BankOfBit_JS.Data;
using BankOfBit_JS.Models;
using Utility;
using System.Data.Entity;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsBanking
{
    public class Batch
    {
        /// <summary>
        /// The name of the xml input file.
        /// </summary>
        private String inputFileName;

        /// <summary>
        /// The name of the log file.
        /// </summary>
        private String logFileName;

        /// <summary>
        /// The data to be written to the log file.
        /// </summary>
        private String logData;

        /// <summary>
        /// The ERROR messages
        /// </summary>
        private string ERRORMessage;

        /// <summary>
        /// The database object
        /// </summary>
        BankOfBit_JSContext db = new BankOfBit_JSContext();

        private XDocument xDocument;

        /// <summary>
        /// Method that processes the errors in the XML file.
        /// </summary>
        /// <param name="beforeQuery">The starting query.</param>
        /// <param name="afterQuery">The resulting query.</param>
        /// <param name="message">The error message.</param>
        private void ProcessERRORs(IEnumerable<XElement> beforeQuery, IEnumerable<XElement> afterQuery, String message)
        {
            // Get all the records that have errors
            IEnumerable<XElement> errors = beforeQuery.Except(afterQuery);

            // Loop through and append information to log data
            foreach (XElement record in errors)
            {
                logData += "----------ERROR-----------\n";
                logData += "File: " + inputFileName + "\n";
                logData += "Institution: " + record.Element("institution") + "\n";
                logData += "Account Number: " + record.Element("account_no") + "\n";
                logData += "Transaction Type: " + record.Element("type") + "\n";
                logData += "Amount: " + record.Element("amount") + "\n";
                logData += "Note: " + record.Element("notes") + "\n";
                logData += "Nodes: " + record.Nodes().Count() + "\n";
                logData += message + "\n";
            }
        }

        /// <summary>
        /// Method to verify the attributes of an xml file's root element.
        /// </summary>
        private void ProcessHeader()
        {
            // Current input file
            xDocument = XDocument.Load(inputFileName);

            // Root element
            XElement root = xDocument.Element("account_update");
            
            // Check to see if attributes does not equal 3
            if (root.Attributes().Count() != 3)
            {
                throw new Exception (String.Format("ERROR: Incorrect number of root Attributes for file {0}\n", inputFileName));
            }

            // Check to see if date does not match todays date.
            if (!DateTime.Parse(root.Attribute("date").Value).Equals(DateTime.Today))
            {
                throw new Exception("Date: " + root.Attribute("date").Value + " in file " + inputFileName + " does not exist.\n");
            }

            // List of institution numbers on the database
            IEnumerable<Institution> institutions = (IEnumerable<Institution>)db.Institutions.ToList();

            int inputInstitutionNumber = int.Parse(root.Attribute("institution").Value);

            bool isValidInstitutionNumber = false;

            // Loop through the institution numbers in the database and see if its valid
            foreach (Institution institution in institutions)
            {
                if(!isValidInstitutionNumber && inputInstitutionNumber == institution.InstitutionNumber)
                {
                    isValidInstitutionNumber = true;
                }
            }

            // If the institution number is invalid throw an ERROR
            if (!isValidInstitutionNumber)
            {
                throw new Exception("Institution: " + inputInstitutionNumber + " in file " + inputFileName + " does not exist.");
            }

            CheckChecksum();
        }

        /// <summary>
        /// Method to process an XML files elements
        /// </summary>
        private void ProcessDetails()
        {
            // Current input file
            LoadDocument();

            // All the transaction elements
            IEnumerable<XElement> transactions = xDocument.Descendants("transaction");

            // All transactions that have 5 elements
            IEnumerable<XElement> correctTransactions = transactions.Where(x => x.Elements().Nodes().Count() == 5);

            ProcessERRORs(transactions, correctTransactions, "Error: Transaction does not have 5 child elements.");

            // All transactions that have the correct institution number
            string institution = xDocument.Element("account_update").Attribute("institution").Value;

            // All transactions with the correct institutions.
            IEnumerable<XElement> correctInstitutions = correctTransactions.Where(x => x.Element("institution").Value.Equals(institution));

            ProcessERRORs(correctTransactions, correctInstitutions, "ERROR: Institution does not match.");

            // All transactions where type and amount are numeric.
            IEnumerable<XElement> isNumeric = correctInstitutions.Where(x => Utility.Numeric.IsNumeric(x.Element("type").Value, System.Globalization.NumberStyles.Integer) || Utility.Numeric.IsNumeric(x.Element("type").Value, System.Globalization.NumberStyles.Float));

            ProcessERRORs(correctInstitutions, isNumeric, "ERROR: Type and Amount must be numeric.");

            // All transactions where type is 2 or 6
            IEnumerable<XElement> correctType = isNumeric.Where(x => x.Element("type").Value.Equals("2") || x.Element("type").Value.Equals("6"));

            ProcessERRORs(isNumeric, correctType, "ERROR: Incorrect type associated with transaction.");

            // All transactions where amount is 0 when type node = 6 and must have a value greater than 0 when type is 2.
            IEnumerable<XElement> correctAmount = correctType.Where(x => x.Element("type").Value.Equals("2") && double.Parse(x.Element("amount").Value) > 0 || x.Element("type").Value.Equals("6") && double.Parse(x.Element("amount").Value) == 0);

            ProcessERRORs(correctType, correctAmount, "ERROR: Amount of transaction is incorrect.");

            // Database account numbers
            IEnumerable<long> accountNos = db.BankAccounts.Select(x => x.AccountNumber).ToList();

            // Compare database account numbers with account numbers in XML file
            IEnumerable<XElement> correctAccountNo = correctAmount.Where(x => accountNos.Contains(long.Parse(x.Element("account_no").Value))); 
            

            ProcessERRORs(correctAmount, correctAccountNo, "ERROR: Account Number is invalid.");

            ProcessTransactions(correctAccountNo);
        }

        /// <summary>
        /// Method that processes a transaction based on the type it is
        /// </summary>
        /// <param name="transactionRecords">Records remaining after validation</param>
        private void ProcessTransactions(IEnumerable<XElement> transactionRecords)
        {
            // Service reference
            OnlineBankingServiceServiceReference.TransactionManagerClient service = new OnlineBankingServiceServiceReference.TransactionManagerClient();

            string message = "";
            
            // Loop through the transactions in transactionRecords
            foreach (XElement transaction in transactionRecords)
            {
                double amount = double.Parse(transaction.Element("amount").Value);
                int accountNumber = int.Parse(transaction.Element("account_no").Value);
                BankAccount account = (BankAccount)db.BankAccounts.Where(x => x.AccountNumber.Equals(accountNumber)).SingleOrDefault();

                double accountBalance = (double)(db.BankAccounts.Where(x => x.AccountNumber == account.AccountNumber).Select(x => x.Balance)).SingleOrDefault();

                double balance = account.Balance;
                // If the type is 2 (Withdrawal)
                if (transaction.Element("type").Value.Equals("2"))
                {
                    message = "Withdrawl " + amount + " from " + account;

                    double newBalance = (double)service.Withdrawal(account.BankAccountId, amount, message);

                    if (accountBalance == newBalance)
                    {
                        logData += "Transaction completed unsuccessfully.\n" ;
                    }
                    else
                    {
                        string log = "Transaction completed successfully: Withdrawal - " + amount.ToString("C") + " applied to account " + account.AccountNumber + "\n";
                        logData += log;
                    }
                }
                // If the type is Calculate Interest
                else if (transaction.Element("type").Value.Equals("6"))
                {
                    message = "Interest applied to account: " + account;

                    double newBalance = (double)service.CalculateInterest(account.BankAccountId, message);

                    // If the balance is not updated
                    if (accountBalance == newBalance)
                    {
                        logData += "Transaction completed unsuccessfully.\n";
                    }
                    // If the balance is updated
                    else
                    {
                        string log = "Transaction completed successfully: Interest - *** applied to account " + account.AccountNumber + "\n";
                        logData += log;
                    }
                }
            }
        }

        /// <summary>
        /// Method that writes the logData to the log
        /// </summary>
        /// <returns></returns>
        public String WriteLogData()
        {  
            // How to write log data to file
            StreamWriter writer = new StreamWriter(logFileName, false); ;
            writer.Write(logData);
            writer.Close();

            string logDataCapture = logData;

            // Set logData to an emptry string to be used to capture data for the next transmission file
            logData = "";

            // Set logFileName to an emptry string to be used to store the next log file name
            logFileName = "";

            return logDataCapture;
        }

        /// <summary>
        /// Method to verify a file exists with a proper file name.
        /// </summary>
        /// <param name="institution">Institution number</param>
        /// <param name="key">Encryption key</param>
        public void ProcessTransmission(String institution, String key)
        {
            // Generate the input file name, and log file name 
            inputFileName = DateTime.Now.Year + "-" + DateTime.Now.DayOfYear + "-"+ institution + ".xml";
            logFileName = "LOG " + DateTime.Now.Year + "-" + DateTime.Now.DayOfYear + "-" + institution + ".txt";

            try
            {
                // See if the input file exists
                if (!File.Exists(inputFileName))
                {
                    // If file does not exist, produce appropriate ERROR message.
                    // If file does not exist, produce appropriate ERROR message.
                    logData += "\r\nERROR: The file " + inputFileName + " does not exist.";
                }
                else
                {
                    // If file does exist, proceed to process header and details
                    ProcessHeader();
                    ProcessDetails();
                }
            }
            catch (Exception ex)
            {
                // If exception occurs, display an ERROR message
                logData += "ERROR: " + ex.Message;
                
            }
        }

        /// <summary>
        /// Checks the checksum in the header by adding the account numbers in the account number elements
        /// </summary>
        /// <exception cref="Exception">If checksum does not match added account numbers</exception>
        public void CheckChecksum()
        {
            // Current input file
            LoadDocument();

            // Root element
            XElement root = xDocument.Element("account_update");

            int inputChecksum = (int)root.Attribute("checksum");
            int checksumCheck = 0;

            IEnumerable<XElement> accountNumbers = root.Descendants().Where(x => x.Name == "account_no");

            // Add the account numbers up
            foreach (XElement accountNumber in accountNumbers)
            {
                checksumCheck += int.Parse(accountNumber.Value);
            }

            // If the checksum and the check does not match, throw an exception
            if (inputChecksum != checksumCheck)
            {
                throw new Exception("The checksum: " + inputChecksum + " in file " + inputFileName + " is invalid.");
            }
        }

        /// <summary>
        /// Loads the XML file in the XDocument Object
        /// </summary>
        public void LoadDocument()
        {
            xDocument = XDocument.Load(inputFileName);
        }
    }
}
