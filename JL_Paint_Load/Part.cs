using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL_Paint_Load
{
    class Part
    {
        int _Part_No;
        string _Part_Name;

        public Part(int Part_No, string Part_Name)
        {
            _Part_No = Part_No;
            _Part_Name = Part_Name;

        }

        public int No()
        {
            return _Part_No;
        }


        public string Name()
        {
            return _Part_Name;
        }

        public override string ToString()
        {
            return _Part_No.ToString() + "     --------------    " + _Part_Name;
        }
    }
}
