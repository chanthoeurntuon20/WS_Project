using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService
{
    public class ValidationCheck
    {
        public bool hasSpecialChar(string input)
        {
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }
    }
}