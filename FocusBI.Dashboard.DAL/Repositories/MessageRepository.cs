using FoucsBI.Dashboard.Models;
using System.Collections.Generic;
using System.Linq;

namespace FoucsBI.Dashboard.DAL
{
    public class MessageRepository: RepositoryBase
    {
        public List<Message> Fetch(int executionId, MessageType messageType)
        {
            var data = XmlDataRepository.Messages.Where(e => e.ExecutionId == executionId && e.MessageType == (int)messageType ).ToList();
            return data;
        }

        internal List<Message> FetchAll()
        {
            var sql = @"
                    SELECT 
	                    event_message_id                        Id
	                    ,CAST(message_time AS DATETIME)			Time
	                    ,message				                MessageText
                        ,message_type                           MessageType
	                    ,message_source_name	                Source
	                    ,ISNULL(subcomponent_name,'')	        Component
                        ,operation_id                           ExecutionId
                    FROM catalog.event_messages m
                    WHERE m.message_type IN (110,120)
                ";
            var data = context.Database.SqlQuery<Message>(sql).ToList();
            return data;
        }
    }
}
