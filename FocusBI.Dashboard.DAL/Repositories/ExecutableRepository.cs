using FoucsBI.Dashboard.Models;
using System.Collections.Generic;
using System.Linq;

namespace FoucsBI.Dashboard.DAL
{
    public class ExecutableRepository : RepositoryBase
    {
         public virtual List<Executable> Fetch(int executionId)
        {
            var sql = @"
                    SELECT s.statistics_id	Id
	                    ,e.executable_name	Name
	                    ,e.package_name		PackageName
	                    ,e.package_path		PackagePath
	                    ,CAST(s.start_time AS DATETIME) StartTime
	                    ,CAST(s.end_time AS DATETIME) EndTime
	                    ,ISNULL(s.execution_duration /60000,0)	Duration
	                    ,CAST(s.execution_result AS INT)	ExecutionResult
	                    ,CAST(s.execution_value AS VARCHAR)	ExecutionValue
                    FROM catalog.executables e
	                    LEFT JOIN catalog.executable_statistics s ON s.executable_id = e.executable_id AND s.execution_id = e.execution_id
                    WHERE e.execution_id = {0}
                    ORDER BY e.executable_id ASC
                ";
            sql = string.Format(sql, executionId);
            var data = context.Database.SqlQuery<Executable>(sql).ToList(); ;
            return data;
        }
    }
}


