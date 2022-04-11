using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Numerals_Eng
{
    class NumeralEng
    {
        private string type;
        private string valueOfNum;
        
        public void SetTypeNum(string type)
        {
            this.type = type;
        }
        public string GetTypeNum()
        {
            return this.type;
        }

        public void SetValue(string val)
        {
            valueOfNum = val;
        }
        public string GetValue()
        {
            return valueOfNum;
        }
    }
}
