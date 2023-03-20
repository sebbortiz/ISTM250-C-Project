using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//AUTHOR:   Sebastian Ortiz, Hannah Bang, Dean Cuzela, Tristan De Leon, Sourav Dhar
//COURSE:   ISTM 250-502
//FORM:  FRMInventory.cs
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
    public partial class FRMInventory : Form
    {
        public FRMInventory()
        {
            InitializeComponent();
        }

        //global variables
        string[] strInventoryItems = { "flour", "yeast", "sugar", "oil", "ham", "turkey", "scheese", "lettuce", "tomato", "bacon", "pickles", "mayo", "mustard", "pepperoni", "sauce", "gcheese", "salt", "pepper" };
       
        /// <summary>
        /// initial display of inventory items in list box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FRMInventory_Load(object sender, EventArgs e)
        {
            LBLTime.Text = "Today: " + DateTime.Now.ToString(); //shows the current date and time ; refer to Prolem Statement in instructions
            LBXInventory.Items.Clear();
            GetInventoryUsage();
        }

        /// <summary>
        /// This method gets the inventory usage array from FRMOrder and subtracts it from the decInventoryAmounts array
        /// then displays it on the LBXInventory list box
        /// </summary>
        public void GetInventoryUsage() 
        {
            //gets the inventory amounts array from FRMOrder
            decimal[] decInventoryAmounts = FRMOrder.decInventoryAmounts;
            for (int i = 0; i < strInventoryItems.Length; i++)
            {
                LBXInventory.Items.Add(strInventoryItems[i] + "   " + "( " + decInventoryAmounts[i] + " )");
            }
        }

        /// <summary>
        /// closes form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTNClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// refills inventory levels when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTNRefill_Click(object sender, EventArgs e)
        {
            LBXInventory.Items.Clear();
            RefillInventory();
        }

        /// <summary>
        /// refills inventory level back to original amounts
        /// </summary>
        private void RefillInventory()
        {
            FRMOrder.decInventoryAmounts = new decimal[] { 200m, 50m, 30m, 25m, 10m, 10m, 20m, 14m, 14m, 10m, 20m, 15m, 12m, 20m, 60m, 25m, 10m, 10m };
            GetInventoryUsage();
        }
    }

}
