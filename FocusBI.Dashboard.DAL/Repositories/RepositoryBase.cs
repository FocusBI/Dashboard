using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace FoucsBI.Dashboard.DAL
{
    public class RepositoryBase
    {
        protected EfContext context = new EfContext();
    }
}
