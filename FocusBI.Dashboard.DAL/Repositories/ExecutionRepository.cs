using FoucsBI.Dashboard.Models;
using System.Collections.Generic;
using System.Linq;

namespace FoucsBI.Dashboard.DAL
{
    public class ExecutionRepository : RepositoryBase
    {
        public virtual List<Execution> Fetch()
        {
            var sql = @"
                SELECT
	                e.execution_id Id
	                ,e.project_name ProjectName
	                ,e.package_name PackageName
	                ,e.project_lsn ProjectLsn
	                ,e.status StatusId
	                ,CASE [status] 
			                WHEN 1 THEN 'created' 
			                WHEN 2 THEN 'Running' 
			                WHEN 3 THEN 'canceled' 
			                WHEN 4 THEN 'failed' 
			                WHEN 5 THEN 'pending' 
			                WHEN 6 THEN 'ended unexpectedly' 
			                WHEN 7 THEN 'succeeded' 
			                WHEN 8 THEN 'stopping' 
			                WHEN 9 THEN 'completed' 
	                END [Status]
	                ,CAST(e.start_time AS DATETIME) StartTime
	                ,CAST(e.end_time AS DATETIME) EndTime
	                ,ISNULL(DATEDIFF(ss, e.start_time, e.end_time) / 60,0) ElapsedTimeInMinutes
	                ,ISNULL(errors.warnings,0) NumberOfWarnings
	                ,ISNULL(errors.errors,0) NumberOfErrors
                    ,ISNULL((SELECT COUNT(*) FROM catalog.executables eX WHERE ex.execution_id = e.execution_id),0) NumberOfExecutables
                FROM catalog.executions e
	                LEFT JOIN (
			                SELECT operation_id
				                ,SUM(CASE WHEN event_name = 'OnError' THEN 1 ELSE 0 END) Errors
				                ,SUM(CASE WHEN event_name = 'OnWarning' THEN 1 ELSE 0 END) Warnings
			                FROM catalog.event_messages 
			                WHERE event_name IN ('OnError', 'OnWarning')
			                GROUP BY operation_id
		                ) errors ON errors.operation_id = e.execution_id
                ORDER BY e.execution_id ASC
                ";
            var data = context.Database.SqlQuery<Execution>(sql).ToList(); ;
            return data;
        }
    }
}
