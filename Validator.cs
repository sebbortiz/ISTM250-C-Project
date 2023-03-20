using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodingProject3;

namespace CodingProject1
{
    public static class Validator //can add other methods for validation, all in a seperate class, can be used for other projects
    {
        /// <summary>
        /// checks if value is present
        /// </summary>
        /// <param name="strTestValue"></param>
        /// <param name="strControlName"></param>
        /// <returns></returns>
        public static string IsPresent(string strTestValue, string strControlName)
        {
            string strMessage = "";
            if (strTestValue == "")
            {
                strMessage += strControlName + " is a required field.\n";
            }
            return strMessage;
        }

        /// <summary>
        /// checks if value is integer64
        /// </summary>
        /// <param name="strTestValue"></param>
        /// <param name="strControlName"></param>
        /// <returns></returns>
        public static string IsInteger(string strTestValue, string strControlName)
        {
            string strMessage = "";
            if (!Int64.TryParse(strTestValue, out _))
            {
                strMessage += strControlName + " must be a valid integer value.\n";
            }
            return strMessage;
        }


        /// <summary>
        /// checks if value is positive
        /// </summary>
        /// <param name="strTestValue"></param>
        /// <param name="strControlName"></param>
        /// <param name="intMin"></param>
        /// <returns></returns>
        public static string IsGreaterThan(string strTestValue, string strControlName, int intMin)
        {
            string strMessage = "";
            if (Convert.ToInt64(strTestValue) <= intMin)
            {
                strMessage += strControlName + " must be greater than " + intMin + ".\n";
            }
            return strMessage;
        }

        /// <summary>
        /// checks if value is selected
        /// </summary>
        /// <param name="IntIndex"></param>
        /// <param name="StrItemName"></param>
        /// <returns></returns>
        public static string IsSelected(int IntIndex, string StrItemName)
        {
            string strMessage = "";
            if (IntIndex < 0)
            {
                strMessage += StrItemName + " must be selected.\n";
            }
            return strMessage;
        }
        /// <summary>
        /// checks if city is Bryan or College Station
        /// </summary>
        /// <param name="strTestValue"></param>
        /// <param name="strControlName"></param>
        /// <returns></returns>
        public static string IsCityWithinRange(string strTestValue, string strControlName)
        {
            string strMessage = "";
            if (strTestValue.ToUpper() != "BRYAN" && strTestValue.ToUpper() != "COLLEGE STATION")
            {
                strMessage += strControlName + " must be Bryan or College Station for delivery.\n";
            }
            return strMessage;
        }
        /// <summary>
        /// checks if state is Texas
        /// </summary>
        /// <param name="strTestValue"></param>
        /// <param name="strControlName"></param>
        /// <returns></returns>
        public static string IsStateWithinRange(string strTestValue, string strControlName)
        {
            string strMessage = "";
            if (strTestValue.ToUpper() != "TX" && strTestValue.ToUpper() != "TEXAS")
            {
                strMessage += strControlName + " must be TX or Texas for delivery.\n";
            }
            return strMessage;
        }

        /// <summary>
        /// checks if an input is in a string format
        /// </summary>
        /// <param name="strTestValue"></param>
        /// <param name="strControlName"></param>
        /// <returns></returns>
        public static string IsString(string strTestValue, string strControlName)
        {
            string strMessage = "";
            bool blnValidString = true;
            foreach(char c in strTestValue)
            {
                if (!Char.IsLetter(c) && !Char.IsPunctuation(c) && c != ' ')
                {
                    blnValidString = false;
                }
            }
            if(blnValidString == false)
            {
                strMessage += strControlName + " must be in a string format and a valid name.\n";
            }
            return strMessage;
        }
    }
}
