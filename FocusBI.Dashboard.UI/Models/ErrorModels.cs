using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoucsBI.Dashboard.UI
{
    public class ErrorModel //: ModelBase
    {
        public string Message { get; set; }
        public ErrorModel(string msg)
        {
            this.Message = msg;
        }
    }
}