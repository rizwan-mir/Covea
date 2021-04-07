using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Covea.Interface;

namespace Covea.Data
{
    public class DataHelperXML : IDataHelper
    {
        public DataHelperXML()
        { }

        public XElement GetRiskRules()
        {
            string strAppPath = AppDomain.CurrentDomain.BaseDirectory;
            return XElement.Load(strAppPath + "\\Data\\RiskRate.xml");
        }


    }
}