using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;




namespace coreLogic.Models.NumberGenerator
{
    public class NumberGenerator
    {


        public string Generate(string fieldName, bool hasContent, string prefix, int numberLength, string lastMemoNumber)
        {
            char splitAt = prefix.Last();
            string length = "";
            for (var i= 0; i < numberLength; i++)
            {
                if(i == numberLength - 1){length += "1";}
                else{length += "0";}
            }

            string lengthChar = length;
            lengthChar = lengthChar.Replace("1", "0");

            string generatedNumber = !hasContent ? prefix + length
                : prefix + (int.Parse(lastMemoNumber.Split(splitAt)[1] + 1).ToString(lengthChar));
            
            return generatedNumber;
        }


    }
}
