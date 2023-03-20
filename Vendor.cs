using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingProject3
{
    public class Vendor
	{
		private string vname;
		private string vaddress;
		private string vcity;
        private string vstate;
        private string vzip;
        private string vphone;
        private decimal vytd;
        private string vcomment;
        private string vcontact;
        private int vdefaultdiscount;

		public Vendor() { }

		public Vendor(string vname, string vaddress, string vcity, string vstate, string vzip, string vphone, decimal vytd, string vcomment, string vcontact, int vdefaultdiscount)
		{
			this.Name = vname;
			this.Address = vaddress;
			this.City = vcity;
            this.State = vstate;
            this.Zip = vzip;
            this.Phone = vphone;
            this.YTD = vytd;
            this.Comment = vcomment;
            this.Contact = vcontact;
            this.DefaultDiscount = vdefaultdiscount;
		}

		public string Name
		{
			get
			{
				return vname;
			}
			set
			{
				vname = value;
			}
		}

		public string Address
		{
			get
			{
				return vaddress;
			}
			set
			{
				vaddress = value;
			}
		}

		public string City
		{
			get
			{
				return vcity;
			}
			set
			{
				vcity = value;
			}
		}

        public string State
        {
            get
            {
                return vstate;
            }
            set
            {
                vstate = value;
            }
        }

   		public string Zip
		{
			get
			{
				return vzip;
			}
			set
			{
				vzip = value;
			}
		}
 
        public string Phone
		{
			get
			{
				return vphone;
			}
			set
			{
				vphone = value;
			}
		}

        public decimal YTD
		{
			get
			{
				return vytd;
			}
			set
			{
				vytd = value;
			}
		}

        		public string Comment
		{
			get
			{
				return vcomment;
			}
			set
			{
				vcomment = value;
			}
		}
        		
        public string Contact
		{
			get
			{
				return vcontact;
			}
			set
			{
				vcontact = value;
			}
		}

        public int DefaultDiscount
		{
			get
			{
				return vdefaultdiscount;
			}
			set
			{
                vdefaultdiscount = value;
			}
		}

	}
}
