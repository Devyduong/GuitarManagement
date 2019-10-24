using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarManagement.CommonDefine
{
    public class DataUtil
    {
        public static bool IsNumber(string input)
        {
            try
            {
                int convert = int.Parse(input);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
