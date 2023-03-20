using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodingProject1.Properties;
using CodingProject3;

//AUTHOR:   Sebastian Ortiz, Hannah Bang, Dean Cuzela, Tristan De Leon, Sourav Dhar
//COURSE:   ISTM 250-502
//FORM:  FRMOrder.cs
//PURPOSE:  This program is designed to receive orders from customers for Kirby's Deli.
//INPUT:  The customer information in the text boxes of GBXCustomerInformation and the order information in GBXOrderInformation.
//        If needed, the program will also prompt the user to put delivery information in GBXDeliveryInformation.
//PROCESS:  The program will ask the customer to enter their information before putting in their order. They will select whether
//          they want a pizza or a sandwich, what type of pizza or sandwich they want, and the quantity of that item. The user can
//          also choose if they want to do carryout or delivery - if they choose delivery, another group box with delivery information
//          will pop up and the customer will have to put that in. Finally, the customer can see all of what they've order in LBXOrderItems
//          before they can click the button to complete the order before the program prompts the user to enter another order by clearing
//          everything.
//OUTPUT:   The items of the order in LBXOrderItems, the subtotal in TXTSubtotal, and the total in TXTTotal.
//HONOR CODE: “On my honor, as an Aggie, I have neither given  
//   nor received unauthorized aid on this academic  
//   work.”

namespace CodingProject1
{
    public partial class FRMOrder : Form
    {
        public FRMOrder()
        {
            InitializeComponent();
        }

        #region Global Variables
        //variables for subtotal, total, and tax calculations
        decimal decSubtotalBeforeTax = 0m; 
        decimal decSubtotalAfterTax = 0m;
        decimal decOrderTotal = 0m;
        decimal decItemPrice = 0m;
        //int intQuantity = 0; //[Hannah.B] modify
        //arrays for inventory computations
        public static decimal[] decInventoryAmounts = { 200m, 50m, 30m, 25m, 10m, 10m, 20m, 14m, 14m, 10m, 20m, 15m, 12m, 20m, 60m, 25m, 10m, 10m };
        //        decimal[] decTotalUsageOfInv = null; // [Hannah.B] modify
        //        int[] intQuantityOfOrders = new int[6]; // [Hannah.B] This is hard coding, need to change.
        int[] intQuantityOfOrders = null;
        //Ingredient Table from instructions page 3
        decimal[,] decIngredientTable = {
                    { 1, 0.5M, 0.03M, 0.05M, 0.1M, 0,    0.1M, 0.25M, 0.25M, 0,    0.02M, 0.02M, 0.02M, 0,    0, 0,    0.01M, 0.01M },   // H&S
                    { 1, 0.5M, 0.03M, 0.05M, 0,    0.1M, 0.1M, 0.25M, 0.25M, 0,    0.02M, 0.02M, 0.02M, 0,    0, 0,    0.01M, 0.01M },  // T&P
                    { 1, 0.5M, 0.03M, 0.05M, 0,    0,    0,    0.3M,  0.3M,  0.1M, 0,     0.02M, 0.02M, 0,    0, 0,    0.01M, 0.01M },  // BLT
                    { 3, 2,    0.5M,  0.1M,  0,    0,    0,    0,     0,     0,    0,     0,     0,     0,    1, 0.3M, 0.02M, 0.02M },  // Ch
                    { 3, 2,    0.5M,  0.1M,  0,    0,    0,    0,     0,     0,    0,     0,     0,     0.3M, 1, 0.2M, 0.02M, 0.02M },  // Pep
                    { 3, 2,    0.5M,  0.1M,  0.1M, 0.1M, 0,    0,     0.3M,  0.1M, 0,     0,     0,     0.3M, 1, 0.2M, 0.02M, 0.02M }  // Sup
                };

        #endregion

        #region Event Wired Code

        /// <summary>
        /// when the form loads, the delivery information is not displayed by default
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FRMOrder_Load(object sender, EventArgs e)
        {
            //when the form loads, the groupbox for delivery info is hidden, the listbox is cleared
            //and the picture box is hidden
            //            decTotalUsageOfInv = new decimal[decIngredientTable.GetLength(1)]; //dynamic array allocation // [Hannah.B] modify

            GBXDeliveryInformation.Hide(); 
            LBXOrderItems.Items.Clear();
            PBXOrderImage.Hide();
            
            //adds the item options into the order type combo boxes
            CBOOrderType.Items.Add("Ham & Swiss Sandwich");
            CBOOrderType.Items.Add("Turkey & Provolone Sandwich");
            CBOOrderType.Items.Add("BLT Sandwich");
            CBOOrderType.Items.Add("Med. Cheese Pizza");
            CBOOrderType.Items.Add("Med. Pepperoni Pizza");
            CBOOrderType.Items.Add("Med. Supreme Pizza");

            intQuantityOfOrders = new int[CBOOrderType.Items.Count];// [Hannah.B] add

            //fills in default customer information; General Specifications #3
            SetDefaultCustomer();

        }
        /// <summary>
        /// sets default customer information
        /// </summary>
        private void SetDefaultCustomer()
        {
            TXTCustomerName.Text = "John Smith"; 
            TXTCustomerAddress.Text = "123 Main St";
            TXTCustomerCity.Text = "College Station";
            TXTCustomerState.Text = "TX";
            TXTCustomerZipCode.Text = "12345";
            TXTCustomerPhone.Text = "1234567890";
            TXTCustomerSubdivision.Text = "subdivision";
        }

        /// <summary>
        /// when the check value changes for the default selected radio button the check if delivery method runs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RDOCarryOut_CheckedChanged(object sender, EventArgs e)
        {
            CheckIfDelivery(); //calling the method which copies the customer text and reveals the delivery groupbox when the delivery radio button is checked
        }

        /// <summary>
        /// when the add item button is clicked, it calculates the subtotal prior and after adding the tax
        /// for a particular segment of the order, then it calculates the overall total of the order (with tax)
        /// and displays it on the list box using a user made method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTNAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidData())
                {
                    //clears the subtotal text box for new use
                    TXTSubtotal.Clear();

                    //processing
                    int intOrderTypeIndex = CBOOrderType.SelectedIndex;
                    int intQuantity = Convert.ToInt16(TXTQuantity.Text);    //[Hannah.B] modify

                    decSubtotalBeforeTax = decItemPrice * intQuantity;

                    decSubtotalAfterTax = decSubtotalBeforeTax * 1.0825m;

                    decOrderTotal += decSubtotalAfterTax;

                    //adding quantity to quantity array, but if the user clears fields, the array is reset [Hannah.B] fix a bug.
                    intQuantityOfOrders[CBOOrderType.SelectedIndex] += intQuantity;

                    //ouput
                    TXTSubtotal.Text = decSubtotalAfterTax.ToString("c2"); //this is subtotal with tax because the listbox displays subtotal without tax; ref #14 General Specifications
                    TXTTotal.Text = decOrderTotal.ToString("c2");
                    GetOrderItems(); //user made method that gets the current order inputs and copies them to the list box
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString());
            }
        }

        /// <summary>
        /// this method adds the order options into the combo box for orders then depending on the selected index
        /// offers either the crust options for a sandwhich or a pizza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBOOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CBOOrderCrust.Items.Clear(); //clears all previous items from the crust type combo box
            PBXOrderImage.Show(); //displays the proper image for the selected item
            if (CBOOrderType.SelectedIndex <= 2) //if the selected index is less than or greater than 2 (meaning its a sandwhich) then it adds the crust options for a sandwhich
            {
                CBOOrderCrust.Items.Add("White");
                CBOOrderCrust.Items.Add("Pumpernickel");
                CBOOrderCrust.Items.Add("Rye");
                CBOOrderCrust.Items.Add("Sourdough");
                CBOOrderCrust.Items.Add("Multigrain");
                PBXOrderImage.Image = (Resources.deli); //sets the picture box image to the deli jpg
                decItemPrice = 5.00m;
            }
            else if (CBOOrderType.SelectedIndex > 2) //if the selected index is greater than 2 (meaning its a pizza) then the combo box displays the crust options for a pizza
            {
                CBOOrderCrust.Items.Add("Original");
                CBOOrderCrust.Items.Add("Pan");
                CBOOrderCrust.Items.Add("Thin");
                CBOOrderCrust.Items.Add("Wheat");
                PBXOrderImage.Image = (Resources.pizza); //sets the picture box image to the pizza jpg
                decItemPrice = 9.50m;
            }
        }

        /// <summary>
        /// clears all text fields as stated in the instructions, we were not sure if this meant all fields
        /// or only certain text fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTNClear_Click(object sender, EventArgs e)
        {
            ClearFields(); //calls on method that clears all fields
        }

        /// <summary>
        /// displays a message box that shows the order total, then clears all of the controls
        /// for a new order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTNComplete_Click(object sender, EventArgs e) //add validation for processing order delivery
        {
            if (IsValidDataCustomer()) //checks the validation for customer data
            {
                if (RDODelivery.Checked) //only checks delivery validation if the delivery radio button is checked
                {
                    if (IsValidDataDelivery())
                    {
                        CalculateInventoryUsed();
                        SaveCompleteOrder(); //calls method for processing the completed order
                    }
                }
                else
                {
                    CalculateInventoryUsed();
                    SaveCompleteOrder(); //still calls method for processing order if the customer data is valid (carryout order)
                }
            }
        }

        /// <summary>
        /// this method closes the form when the exit button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTNExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// this method when clicked will allow viewing of the inventory 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTNInventory_Click(object sender, EventArgs e)
        {
            FRMInventory inventoryForm = new FRMInventory();
            inventoryForm.ShowDialog();
        }

        /// <summary>
        /// this button shows the vendor form when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTNVender_Click(object sender, EventArgs e)
        {
            FRMVendor vendorDlg = new FRMVendor();
            vendorDlg.ShowDialog();
        }

        #endregion

        #region User Made Methods

        /// <summary>
        /// checks if delivery radio button is checked then shows the delivery group box and pastes the info from the customer info group box
        /// </summary>
        private void CheckIfDelivery()
        {
            if (RDODelivery.Checked) //only if the delivery radio button is checked, the delivery group box is displayed
            {
                GBXDeliveryInformation.Show();
                TXTDeliveryName.Text = TXTCustomerName.Text; //copies all info from customer groupbox into delivery groupbox
                TXTDeliveryAddress.Text = TXTCustomerAddress.Text;
                TXTDeliveryCity.Text = TXTCustomerCity.Text;
                TXTDeliveryState.Text = TXTCustomerState.Text;
                TXTDeliveryZip.Text = TXTCustomerZipCode.Text;
                TXTDeliveryPhone.Text = TXTCustomerPhone.Text;
                TXTDeliverySubdivision.Text = TXTCustomerSubdivision.Text;
            }
            else if (!RDODelivery.Checked)
            {
                GBXDeliveryInformation.Hide();
            }
        }

        /// <summary>
        /// this method will get the order items selected by the user and display them in the list box
        /// </summary>
        private void GetOrderItems()
        {
            int intOrderTypeIndex = CBOOrderType.SelectedIndex; //used for output on listbox
            LBXOrderItems.Items.Add(CBOOrderCrust.Text + " " + CBOOrderType.Text + ", " + TXTQuantity.Text + "@" + decItemPrice.ToString() + ", Total: " + decSubtotalBeforeTax.ToString()); //outputs into list box
            CBOOrderType.SelectedIndex = -1; //resets/clears all order groupbox controls
            CBOOrderCrust.SelectedIndex = -1;
            TXTQuantity.Clear();
            PBXOrderImage.Hide(); ; 
        }

        /// <summary>
        /// Method for saving the complete processed order 
        /// </summary>
        private void SaveCompleteOrder()
        {
            if (LBXOrderItems.Items.Count == 0)
            {
                MessageBox.Show("Order must have an item.");
            }
            else
            {
                MessageBox.Show("Thank you for your order! Your total is " + TXTTotal.Text + ".", "Order Completion");
                ClearFields();

                //fills in default customer information; General Specifications #3
                SetDefaultCustomer();
            }
        }

        /// <summary>
        /// this method clears all fields & global variables
        /// </summary>
        private void ClearFields()
        {
            //clear all text fields in the customer and delivery group boxes if necessary
            foreach (Control c in GBXCustomerInformation.Controls) //for each control in the group box, if the control is a text box then the value is set to "" (empty)
            {
                if (c is TextBox)
                    c.Text = "";
            }

            foreach (Control c in GBXDeliveryInformation.Controls)
            {
                if (c is TextBox)
                    c.Text = "";
            }

            //clears the other text fields not in the above group boxes
            CBOOrderType.SelectedIndex = -1;
            CBOOrderCrust.SelectedIndex = -1;
            TXTQuantity.Clear();
            TXTSubtotal.Clear();
            TXTTotal.Clear();
            RDOCarryOut.Checked = true;
            PBXOrderImage.Hide();
            LBXOrderItems.Items.Clear();

            //resets all global variables
            decOrderTotal = 0m;
            decSubtotalBeforeTax = 0m;
            decSubtotalAfterTax = 0m;
            decItemPrice = 0m;
            //            intQuantityOfOrders = new int[6];   // [Hannah.B] this is hard coding , need to change
            intQuantityOfOrders = new int[CBOOrderType.Items.Count];// [Hannah.B] add
        }

        /// <summary>
        /// this method gets the amount of inventory used for a completed order
        /// </summary>
        private void CalculateInventoryUsed() 
        {
            //runs a loop for as long as the list box has items in it (before its cleared in SaveCompleteOrder() [Hannah.B] fix a bug
            for (int k = 0; k < intQuantityOfOrders.Length; k++)
            {
                for (int i = 0; i < decIngredientTable.GetLength(1); i++)
                {
                    decInventoryAmounts[i] -= decIngredientTable[k, i] * intQuantityOfOrders[k];
                }
            }
        }
        #endregion

        #region Validation
        /// <summary>
        /// Validates customer name, item, crust type, quantity
        /// </summary>
        /// <returns></returns>
        private bool IsValidData() //if a texbox is not valid, it draws focus to that textbox? Not having to enlarge the form every time it starts up
        {
            string strErrorMessage = "";

            string tmpMessage = "";
            //checks if quantity is present
            tmpMessage += Validator.IsPresent(TXTQuantity.Text, TXTQuantity.Tag.ToString());
            if (String.IsNullOrEmpty(tmpMessage)) //if quantity is present
            {
                //checks if quantity is an integer
                tmpMessage += Validator.IsInteger(TXTQuantity.Text, TXTQuantity.Tag.ToString());
                if (String.IsNullOrEmpty(tmpMessage)) //if quantity is an integer
                    //checks if it's positive
                    tmpMessage += Validator.IsGreaterThan(TXTQuantity.Text, TXTQuantity.Tag.ToString(), 0);
            }
            strErrorMessage += tmpMessage;

            //checks if combo box items are selected
            strErrorMessage += Validator.IsSelected(CBOOrderType.SelectedIndex, CBOOrderType.Tag.ToString());
            strErrorMessage += Validator.IsSelected(CBOOrderCrust.SelectedIndex, CBOOrderCrust.Tag.ToString());

            if (strErrorMessage.Trim() != "") //we generated one or more error messages
            {
                MessageBox.Show(strErrorMessage, "Entry Error");
                return false;
            }
            else
                return true;
        }
        /// <summary>
        /// checks is input is valid for Delivery section
        /// </summary>
        /// <returns></returns>
        private bool IsValidDataDelivery()
        {
            string strErrorMessage = "";

            //changed .Tag for all delivery textboxes!!
            //checks if delivery name is present
            strErrorMessage += Validator.IsPresent(TXTDeliveryName.Text, TXTDeliveryName.Tag.ToString());
            string tmpMessage = "";
            //checks if phone number is present
            tmpMessage += Validator.IsPresent(TXTDeliveryPhone.Text, TXTDeliveryPhone.Tag.ToString());
            if(String.IsNullOrEmpty(tmpMessage)) //if phone number is present
            {
                //checks if phone number is integer64
                tmpMessage += Validator.IsInteger(TXTDeliveryPhone.Text, TXTDeliveryPhone.Tag.ToString());
                if (String.IsNullOrEmpty(tmpMessage))
                {
                    //checks if it's positive
                    tmpMessage += Validator.IsGreaterThan(TXTDeliveryPhone.Text, TXTDeliveryPhone.Tag.ToString(), 0);
                }
            }
            strErrorMessage += tmpMessage;
            //checks delivery address is present
            strErrorMessage += Validator.IsPresent(TXTDeliveryAddress.Text, TXTDeliveryAddress.Tag.ToString());

            tmpMessage = "";
            //checks if zip code is present
            tmpMessage += Validator.IsPresent(TXTDeliveryZip.Text, TXTDeliveryZip.Tag.ToString());
            if (String.IsNullOrEmpty(tmpMessage)) //if zip code is present
            {
                //check is zip code is integer
                tmpMessage += Validator.IsInteger(TXTDeliveryZip.Text, TXTDeliveryZip.Tag.ToString());
                if (String.IsNullOrEmpty(tmpMessage))
                {
                    //checks if it's positive
                    tmpMessage += Validator.IsGreaterThan(TXTDeliveryZip.Text, TXTDeliveryZip.Tag.ToString(), 0);
                }
            }
            strErrorMessage += tmpMessage;

            tmpMessage = "";
            tmpMessage += Validator.IsPresent(TXTDeliveryCity.Text, TXTDeliveryCity.Tag.ToString()); //Added a tag for Delivery City as "Customer City"
            if (String.IsNullOrEmpty(tmpMessage))
            {
                strErrorMessage += Validator.IsCityWithinRange(TXTDeliveryCity.Text, TXTDeliveryCity.Tag.ToString());
            }
            strErrorMessage += tmpMessage;

            tmpMessage = "";
            tmpMessage += Validator.IsPresent(TXTDeliveryState.Text, TXTDeliveryState.Tag.ToString());
            if (String.IsNullOrEmpty(tmpMessage))
            {
                tmpMessage += Validator.IsStateWithinRange(TXTDeliveryState.Text, TXTDeliveryState.Tag.ToString());
            }
            strErrorMessage += tmpMessage;

            strErrorMessage += Validator.IsPresent(TXTDeliverySubdivision.Text, TXTDeliverySubdivision.Tag.ToString());
            //are phone and zip code numeric
            //City and State within range

            if (strErrorMessage.Trim() != "") //we generated one or more error messages
            {
                MessageBox.Show(strErrorMessage, "Entry Error");
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// validation for customer info
        /// </summary>
        /// <returns></returns>
        private bool IsValidDataCustomer() // validation for only customer name and phone #?
        {
            string strErrorMessage = "";
            string tmpMessage = "";

            //checks if customer name is present
            strErrorMessage += Validator.IsPresent(TXTCustomerName.Text, TXTCustomerName.Tag.ToString());
            //didn't include other information for customer ; reference Code Specification #1

            tmpMessage = "";
            //checks if phone number is present
            tmpMessage += Validator.IsPresent(TXTCustomerPhone.Text, TXTCustomerPhone.Tag.ToString());
            if (String.IsNullOrEmpty(tmpMessage)) //if phone number is present
            {
                //checks if phone number is integer64
                tmpMessage += Validator.IsInteger(TXTCustomerPhone.Text, TXTCustomerPhone.Tag.ToString());
                if (String.IsNullOrEmpty(tmpMessage))
                {
                    //checks if it's positive
                    tmpMessage += Validator.IsGreaterThan(TXTCustomerPhone.Text, TXTCustomerPhone.Tag.ToString(), 0);
                }
            }
            strErrorMessage += tmpMessage;

            tmpMessage = "";
            //checks if zip code is present
            tmpMessage += Validator.IsPresent(TXTCustomerZipCode.Text, TXTCustomerZipCode.Tag.ToString());
            if (String.IsNullOrEmpty(tmpMessage)) //if zip code is present
            {
                //check is zip code is integer
                tmpMessage += Validator.IsInteger(TXTCustomerZipCode.Text, TXTCustomerZipCode.Tag.ToString());
                if (String.IsNullOrEmpty(tmpMessage))
                {
                    //checks if it's positive
                    tmpMessage += Validator.IsGreaterThan(TXTCustomerZipCode.Text, TXTCustomerZipCode.Tag.ToString(), 0);
                }
            }
            strErrorMessage += tmpMessage;

            tmpMessage = "";
            strErrorMessage += Validator.IsPresent(TXTCustomerAddress.Text, TXTCustomerAddress.Tag.ToString());
            strErrorMessage += tmpMessage;

            tmpMessage = "";
            tmpMessage += Validator.IsPresent(TXTCustomerCity.Text, TXTCustomerCity.Tag.ToString());
            strErrorMessage += tmpMessage;

            tmpMessage = "";
            tmpMessage += Validator.IsPresent(TXTCustomerState.Text, TXTCustomerState.Tag.ToString());
            strErrorMessage += tmpMessage;

            tmpMessage = "";
            tmpMessage += Validator.IsPresent(TXTCustomerSubdivision.Text, TXTCustomerSubdivision.Tag.ToString());
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

