using SCS.Dashboard;
using FoucsBI.Dashboard.DAL;
using FoucsBI.Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FoucsBI.Dashboard.UI.Controllers
{
    public class HomeController : Controller
    {
        private IApplicationLogging logWriter;
        public HomeController()
            : base()
        {
            logWriter = new ApplicationLogging();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetKPI()
        {
            try
            {
                var repo = new KpiRepository();
                var data = repo.Fetch();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                this.LogError(String.Format("{0} List controller method failed with an error of {0}:{1}", "GetKPI", e));
                return Json(new { exception = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetExecutions()
        {
            try
            {
                var repo = new ExecutionRepository();
                var data = repo.Fetch();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                this.LogError(String.Format("{0} List controller method failed with an error of {0}:{1}", "GetExecutions", e));
                return Json(new { exception = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetExecutables(int executionId)
        {
            try
            {
                var repo = new ExecutableRepository();
                var data = repo.Fetch(executionId);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                this.LogError(String.Format("{0} List controller method failed with an error of {0}:{1}", "GetExecutables", e));
                return Json(new { exception = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetMessages(int executionId, string type)
        {
            MessageType messageType = MessageType.Unknown;
            switch(type)
            {
                case "error":
                    messageType = MessageType.Error;
                    break;
                case "warning":
                    messageType = MessageType.Warning;
                    break;
            }

            try
            {
                if (messageType == MessageType.Unknown)
                {
                    throw new ArgumentException(String.Format("Unknown messagae type of:'{0}'.", type));
                }
                var data = new MessageRepository().Fetch(executionId, messageType);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                this.LogError(String.Format("{0} List controller method failed with an error of {0}:{1}", "GetMessges", e));
                return Json(new { exception = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private List<Message> GetMessages(int executionId, MessageType messageType)
        {
            return new MessageRepository().Fetch(executionId, messageType);
        }
    
        #region Logging Methods

        #region Debugging Log

        protected void LogInfo(string message)
        {
            if (logWriter != null)
                logWriter.LogInfo(message);
        }

        protected void LogDebug(string message)
        {
            if (logWriter != null)
                logWriter.LogDebug(message);
        }

        protected void LogError(string message)
        {
            if (logWriter != null)
                logWriter.LogError(message);
        }

        protected void LogFatal(string message)
        {
            if (logWriter != null)
                logWriter.LogFatal(message);
        }

        protected void LogWarn(string message)
        {
            if (logWriter != null)
                logWriter.LogWarn(message);
        }

        #endregion

        #endregion

    }
}
