using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic.Models
{
    public class RegularSusuAccountViewModel
    {
        /*
 * SusuContributionViewModel properties
 */

        //client id
        public int clientID { get; set; }

        //susu account id 
        public int susuAccountID { get; set; }

        //susu contribution id
        public int susuContributionID { get; set; }

        //susu group id
        public int susuGroupID { get; set; }

        //susu group name 
        public string susuGroupName { get; set; }

        //susu position 
        public string susuPosition { get; set; }

        //susu position name
        public string susuPositionName { get; set; }

        //contribution rate
        public double contributionRate { get; set; }

        //start date
        public DateTime startDate { get; set; }

        //contribution date
        public DateTime contributionDate { get; set; }

        //contribution amount
        public double contributionAmount { get; set; }

        //relation officer ->staff name
        public string staffName { get; set; }

        //client name
        public string clientName { get; set; }

        //susu Account Number
        public string susuAccountNO { get; set; }

        //narratioon
        public string narration { get; set; }

        //amount disbursed
        public double amountDisbursed { get; set; }

        //interest amount
        public double interestAmount { get; set; }

        //commission amount
        public double commisionAmount { get; set; }


        //company logo
        public byte[] companyLogo { get; set; }

        //client picture
        public byte[] clientPicture { get; set; }

        //company address
        public string companyAddress { get; set; }

        //company phone
        public string companyPhone { get; set; }

        //cummulative paid
        public double cummulativePaid { get; set; }

        //expected totak contribution
        public double expectedtotalContribution { get; set; }
    }
}
