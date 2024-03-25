using BankOfBit_JS.Data;
using BankOfBit_JS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsBanking
{
    public partial class BatchProcess : Form
    {
        BankOfBit_JSContext db = new BankOfBit_JSContext();

        Batch batch = new Batch();

        public BatchProcess()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Always display the form in the top right corner of the frame.
        /// </summary>
        private void BatchProcess_Load(object sender, EventArgs e)
        {
            
        }

        private void lnkProcess_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //given:  Ensure key has been entered.  Note: for use with Assignment 9
            //if(txtKey.Text.Length == 0)
            //{
            //    MessageBox.Show("Please enter a key to decrypt the input file(s).", "Key Required");              
            //}
            
                string capture = "";

                // If select a transmission is selected
                if (radSelect.Checked)
                {
                    // If Royal Bank is selected
                    if (descriptionComboBox.SelectedIndex == 0)
                    {
                        batch.ProcessTransmission(descriptionComboBox.SelectedValue.ToString(), "abcd3983");
                        capture = batch.WriteLogData();

                        rtxtLog.Text += capture;
                    }
                    else if (descriptionComboBox.SelectedIndex == 1)
                    {
                        batch.ProcessTransmission(descriptionComboBox.SelectedValue.ToString(), "abcd3983");
                        capture = batch.WriteLogData();
                        rtxtLog.Text += capture;
                    }
                    else if (descriptionComboBox.SelectedIndex == 2)
                    {
                        batch.ProcessTransmission(descriptionComboBox.SelectedValue.ToString(), "abcd3983");
                        capture = batch.WriteLogData();
                        rtxtLog.Text += capture;
                    }
                    else if (descriptionComboBox.SelectedIndex == 3)
                    {
                        batch.ProcessTransmission(descriptionComboBox.SelectedValue.ToString(), "abcd3983");
                        capture = batch.WriteLogData();
                        rtxtLog.Text += capture;
                    }
                    else if (descriptionComboBox.SelectedIndex == 4)
                    {
                        batch.ProcessTransmission(descriptionComboBox.SelectedValue.ToString(), "abcd3983");
                        capture = batch.WriteLogData();
                        rtxtLog.Text += capture;
                    }
                    else if (descriptionComboBox.SelectedIndex == 5)
                    {
                    batch.ProcessTransmission(descriptionComboBox.SelectedValue.ToString(), "abcd3983") ;
                        capture = batch.WriteLogData();
                        rtxtLog.Text += capture;
                    }
                    else if (descriptionComboBox.SelectedIndex == 6)
                    {
                        batch.ProcessTransmission(descriptionComboBox.SelectedValue.ToString(), "abcd3983");
                        capture = batch.WriteLogData();
                        rtxtLog.Text += capture;
                    }
                    else if (descriptionComboBox.SelectedIndex == 7)
                    {
                        batch.ProcessTransmission(descriptionComboBox.SelectedValue.ToString(), "abcd3983");
                        capture = batch.WriteLogData();
                        rtxtLog.Text += capture;
                    }
                }
                // If All transmissions is selected
                else if (radAll.Checked)
                {
                    IEnumerable<Institution> institutions = db.Institutions;
                    foreach (Institution institution in institutions)
                    {
                        batch.ProcessTransmission(institution.InstitutionNumber.ToString(), "abcd3983");
                        capture = batch.WriteLogData();
                        rtxtLog.Text += capture;
                    }
                }
        }

        /// <summary>
        /// Represents the radio button check changed event
        /// </summary>
        private void radSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (radSelect.Checked)
            {
                descriptionComboBox.Enabled = true;

                this.Location = new Point(0, 0);

                IQueryable<Institution> institutions = db.Institutions;

                descriptionComboBox.DataSource = institutions.ToList();
                descriptionComboBox.DisplayMember = "Description";
                descriptionComboBox.ValueMember = "InstitutionNumber";
            }
            else
            {
                descriptionComboBox.Enabled = false;
            }
            
        }
    }
}
