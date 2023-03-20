using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodingProject1;
//AUTHOR:   Sebastian Ortiz, Hannah Bang, Dean Cuzela, Tristan De Leon, Sourav Dhar
//COURSE:   ISTM 250-502
//FORM:  FRMVendor.cs
//PURPOSE:  This program is designed to store information for the deli's vendors.
//INPUT: The vendor's name, street address, city, state, zipcode, phone number, sales year and representative,
//       any additional comments, and a default discount (in days).
//PROCESS: If the user wishes to load the vendor form, the form will load with text boxes available to store information in.
//         Upon completing filling out the form, the user has the chance to save the vendor data and then go back and forth
//         between past vendors in the system while also having the ability to update their information. 
//OUTPUT: There is no output - the program just saves the data in the form for future reference. 
//HONOR CODE: “On my honor, as an Aggie, I have neither given  
//   nor received unauthorized aid on this academic  
//   work.”
//Our group (ALL OF US) completed our student evaluations!

namespace CodingProject3
{
    public partial class FRMVendor : Form
    {
        List<Vendor> lstVendor = null; //creates a list for the list of vendors
        int intVendorViewIndex = -1; //nothing has been selected
        int[] intDiscountOptions = { 10, 15, 20 }; //creates array for discount options

        public FRMVendor()
        {
            InitializeComponent();
        }

        #region Event Wired Methods

        /// <summary>
        /// Adds discount options and displays the first vendor in the list when the form loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FRMVendor_Load(object sender, EventArgs e)
        {
            foreach (int intDiscount in intDiscountOptions)
            {
                CBODiscount.Items.Add(intDiscount);//add each item in the array to the cbox
            }

            lstVendor = VendorDB.GetVendors(); //gets vendor list from database, store into listvendor list

            if(lstVendor.Count > 0) //if there's something on the list
            {
                intVendorViewIndex = 0; //show the item on the list that has 0 index position (show the 1st vendor)
                DisplayCurrentVendor(); //display the current vendor method
            }
        }


        /// <summary>
        /// closes the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTNClose_Click(object sender, EventArgs e)
        {
            //checks modification
            if (ChangedDataDlgDecision())
            {
                Close();
            }
        }

        /// <summary>
        /// saves the current vendor information, if there are any changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTNSave_Click(object sender, EventArgs e)
        {
            //checks modification
            if(CheckChangedData())
            {
                // Check Data Validation
                //string strErrMsg = Validator.IsValidVendorData(this);

                if (IsValidVendorData())
                {
                    // Save
                    SaveVendor();
                    MessageBox.Show("The vendor data was saved successfully!", "Vendor Information Update");
                }
                else
                {
                    //MessageBox.Show(strErrMsg, "Vendor Update Error");
                    return;
                }

            }
            else
            {
                MessageBox.Show("The data has not changed", "Vendor Update Error");
            }

        }

        /// <summary>
        /// moves to the next vendor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTNNext_Click(object sender, EventArgs e)
        {
            if(ChangedDataDlgDecision())
            {
                MoveUpVendor(); 
                DisplayCurrentVendor(); //calls the current vendor record to display
            }
        }

        /// <summary>
        /// moves to the previous vendor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTNPrevious_Click(object sender, EventArgs e)
        {
            if (ChangedDataDlgDecision())
            {
                MoveDownVendor();
                DisplayCurrentVendor();
            }
        }

        #endregion

        #region User Made Methods

        /// <summary>
        /// presents to user options for unsaved changes (save, don't save, cancel) before moving on
        /// </summary>
        /// <returns></returns>
        private bool ChangedDataDlgDecision()
        {
   
            // check the modification.
            if(CheckChangedData())
            {
                // displays a dialog box to confirm
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                DialogResult result = MessageBox.Show("Do you want to save changed data?", "Vendor Data Changed", buttons);
                if (result == DialogResult.Yes)
                {
                    // Check Data Validation
                    //string strErrMsg = Validator.IsValidVendorData(this);

                    if (IsValidVendorData())
                    {
                        // Save
                        SaveVendor();
                    }
                    else
                    {
                        //MessageBox.Show(strErrorMessage, "Vendor Information Error");
                        return false; //does not go next
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    return false;
                }

            }

            return true;
        }


        /// <summary>
        /// Display the currently selected vendor on the texboxes
        /// </summary>
        private void DisplayCurrentVendor()
        {
            if (intVendorViewIndex > -1 && intVendorViewIndex < lstVendor.Count) //index is valid (already at 0 and increases/decreases when press next/prev btn)
            {
                Vendor currentVendor = lstVendor[intVendorViewIndex]; //references the vendor of the indicated index value
                //puts all information from the list into the textbox
                TXTName.Text = currentVendor.Name;
                TXTStreetAddress.Text = currentVendor.Address;
                TXTCity.Text = currentVendor.City;
                TXTState.Text = currentVendor.State;
                TXTZipCode.Text = currentVendor.Zip;
                TXTPhoneNumber.Text = currentVendor.Phone;
                TXTSalesYear.Text = currentVendor.YTD.ToString();
                TXTSalesRepresentative.Text = currentVendor.Contact;
                TXTComments.Text = currentVendor.Comment;
                CBODiscount.SelectedItem = currentVendor.DefaultDiscount;
            }
        }

        /// <summary>
        /// saves current vendor information to lstVendor
        /// </summary>
        private void SaveVendor()
        {
            Vendor currentVendor = lstVendor[intVendorViewIndex];
            currentVendor.Name = TXTName.Text;
            currentVendor.Address = TXTStreetAddress.Text;
            currentVendor.City = TXTCity.Text;
            currentVendor.State = TXTState.Text;
            currentVendor.Zip = TXTZipCode.Text;
            currentVendor.Phone = TXTPhoneNumber.Text;
            currentVendor.YTD = int.Parse(TXTSalesYear.Text);
            currentVendor.Contact = TXTSalesRepresentative.Text;
            currentVendor.Comment = TXTComments.Text;
            currentVendor.DefaultDiscount = (int)CBODiscount.SelectedItem;

            VendorDB.SaveVendors(lstVendor); //stores all info in the database
        }

        /// <summary>
        /// this method moves up the vendor index (for the next button)
        /// </summary>
        private void MoveUpVendor()
        {
            if (lstVendor.Count <= 1)
                return;

            if (intVendorViewIndex == lstVendor.Count - 1)    // if on the last vendor record, 
                intVendorViewIndex = 0;                         // loop back to first vendor
            else
                intVendorViewIndex++; //index increases
        }

        /// <summary>
        /// this method moves down the vendor index (for the previous button)
        /// </summary>
        private void MoveDownVendor()
        {
            if (lstVendor.Count <= 1)
                return;

            if (intVendorViewIndex == 0)    // if on the first vendor record,
                intVendorViewIndex = lstVendor.Count - 1; // loop  to last vendor record
            else
                intVendorViewIndex--; //index decreases
        }

        /// <summary>
        /// check if there is changed data
        /// </summary>
        /// <returns></returns>
        private bool CheckChangedData()
        {
            string strUI = TXTName.Text + TXTStreetAddress.Text + TXTCity.Text + TXTState.Text + 
                TXTZipCode.Text + TXTPhoneNumber.Text + TXTSalesYear.Text + TXTSalesRepresentative.Text + 
                TXTComments.Text + CBODiscount.SelectedItem.ToString();
            Vendor currentVendor = lstVendor[intVendorViewIndex];
            string strVendor = currentVendor.Name + currentVendor.Address + currentVendor.City + currentVendor.State +
                currentVendor.Zip + currentVendor.Phone + currentVendor.YTD.ToString() + currentVendor.Contact +
                 currentVendor.Comment + currentVendor.DefaultDiscount.ToString();
            if (strUI != strVendor)
                return true;

            return false;
        }

        #endregion


        #region Validation

        //improved validation ?
        /// <summary>
        /// validation for vendor information
        /// </summary>
        private bool IsValidVendorData()
        {
            string strErrorMessage = "";
            string tmpMessage = "";

            //checks if customer name is present
            strErrorMessage += Validator.IsPresent(TXTName.Text, TXTName.Tag.ToString());
            if(String.IsNullOrEmpty(tmpMessage)) //if name is present
            {
                //checks if it is a valid name
                tmpMessage += Validator.IsString(TXTName.Text, TXTName.Tag.ToString());
            }
            strErrorMessage = tmpMessage;

            tmpMessage = "";
            //checks if phone number is present
            tmpMessage += Validator.IsPresent(TXTPhoneNumber.Text, TXTPhoneNumber.Tag.ToString());
            if (String.IsNullOrEmpty(tmpMessage)) //if phone number is present
            {
                //checks if phone number is integer64
                tmpMessage += Validator.IsInteger(TXTPhoneNumber.Text, TXTPhoneNumber.Tag.ToString());
                if (String.IsNullOrEmpty(tmpMessage))
                {
                    //checks if it's positive
                    tmpMessage += Validator.IsGreaterThan(TXTPhoneNumber.Text, TXTPhoneNumber.Tag.ToString(), 0);
                }
            }
            strErrorMessage += tmpMessage;

            tmpMessage = "";
            //checks if zip code is present
            tmpMessage += Validator.IsPresent(TXTZipCode.Text, TXTZipCode.Tag.ToString());            
            strErrorMessage += tmpMessage;

            tmpMessage = ""; //validates all text boxes using the same methods we used to validate FRMOrder
            strErrorMessage += Validator.IsPresent(TXTStreetAddress.Text, TXTStreetAddress.Tag.ToString());
            strErrorMessage += tmpMessage;

            tmpMessage = "";
            strErrorMessage += Validator.IsPresent(TXTCity.Text, TXTCity.Tag.ToString());
            if (String.IsNullOrEmpty(tmpMessage)) //if name is present
            {
                //checks if it is a valid city
                tmpMessage += Validator.IsString(TXTCity.Text, TXTCity.Tag.ToString());
            }
            strErrorMessage += tmpMessage;

            tmpMessage = "";
            strErrorMessage += Validator.IsPresent(TXTState.Text, TXTState.Tag.ToString());
            if (String.IsNullOrEmpty(tmpMessage)) //if name is present
            {
                //checks if it is a valid state
                tmpMessage += Validator.IsString(TXTState.Text, TXTState.Tag.ToString());
            }
            strErrorMessage += tmpMessage;

            tmpMessage = "";
            strErrorMessage += Validator.IsPresent(TXTSalesYear.Text, TXTSalesYear.Tag.ToString());
            if (String.IsNullOrEmpty(tmpMessage)) //if TXTSalesYear is present
            {
                //check is zip code is integer
                tmpMessage += Validator.IsInteger(TXTSalesYear.Text, TXTSalesYear.Tag.ToString());
                if (String.IsNullOrEmpty(tmpMessage))
                {
                    //checks if it's positive
                    tmpMessage += Validator.IsGreaterThan(TXTSalesYear.Text, TXTSalesYear.Tag.ToString(), 0);
                }
            }
            strErrorMessage += tmpMessage;

            tmpMessage = "";
            strErrorMessage += Validator.IsPresent(TXTSalesRepresentative.Text, TXTSalesRepresentative.Tag.ToString());
            strErrorMessage += tmpMessage;

            if (strErrorMessage.Trim() != "") //we generated one or more error messages
            {
                MessageBox.Show(strErrorMessage, "Entry Error");
                return false;
            }
            else
                return true;
        }

        #endregion
    }
}
