using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Covea.Interface;

namespace Covea.Data
{
    public class DataFactory
    {
        public static IDataHelper GetDataHelperObj()
        {
            return new DataHelperXML();
        }
    }
}