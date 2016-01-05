using FoucsBI.Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoucsBI.Dashboard.DAL
{
    public class KpiRepository : RepositoryBase
    {
        private class KpiDTO
        {
            public int StatusId { get; set; }
            public int RowCount { get; set; }
        }

        public virtual List<KPI> Fetch()
        {
            var sql = @"
                SELECT 0 StatusId, COUNT(*) [RowCount] 
                FROM catalog.executions 
                UNION ALL SELECT [Status] StatusId, COUNT(*) [RowCount] 
                FROM catalog.executions GROUP BY [status]";
            var data = context.Database.SqlQuery<KpiDTO>(sql).Select(k => new KPI
            {
                RowCount = k.RowCount,
                ExecutionStatus = Enum.GetName(typeof(ExecutionStatus), k.StatusId)
            })
            .ToList();
            return data;
        }
    }
}
