using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using FoucsBI.Dashboard.Models;
using System.Configuration;
using System;

namespace FoucsBI.Dashboard.DAL
{
    public class XmlDataRepository
    {
        protected EfContext context = new EfContext();

        private static string KpiPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "App_Data\\kpi.xml");
        private static string ExecutionPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "App_Data\\execution.xml");
        private static string ExecutablePath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "App_Data\\executable.xml");
        private static string MessagePath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "App_Data\\message.xml");

        public static List<KPI> Kpis { get; set; }
        public static List<Execution> Executions { get; set; }
        public static List<Executable> Executables { get; set; }
        public static List<Message> Messages { get; set; }

        static XmlDataRepository()
        {
            bool dumpdata = false;
            try
            {
                Boolean.TryParse(ConfigurationManager.AppSettings["app.DumpData"], out dumpdata);
            }
            catch { }
            if (dumpdata)
            {
                new XmlDataRepository().DumpDataFromDatabase();
            }

            LoadDataFromXml();
        }

        public XmlDataRepository()
        {
            if (XmlDataRepository.Kpis == null)
            {
                XmlDataRepository.LoadDataFromXml();
            }
        }

        private void DumpDataFromDatabase()
        {
            var kpis = new KpiRepository().FetchAll();
            var executions = new ExecutionRepository().FetchAll();
            var executables = new ExecutableRepository().FetchAll();
            var messages = new MessageRepository().FetchAll();

            WriteToXmlFile<List<KPI>>(KpiPath, kpis, false);
            WriteToXmlFile<List<Execution>>(ExecutionPath, executions, false);
            WriteToXmlFile<List<Executable>>(ExecutablePath, executables, false);
            WriteToXmlFile<List<Message>>(MessagePath, messages, false);
        }

        public static void LoadDataFromXml()
        {
            XmlDataRepository.Kpis = ReadFromXmlFile<List<KPI>>(KpiPath);
            XmlDataRepository.Executions = ReadFromXmlFile<List<Execution>>(ExecutionPath);
            XmlDataRepository.Executables = ReadFromXmlFile<List<Executable>>(ExecutablePath);
            XmlDataRepository.Messages = ReadFromXmlFile<List<Message>>(MessagePath);
        }

        /// <summary>
        /// Writes the given object instance to an XML file.
        /// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
        /// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [XmlIgnore] attribute.</para>
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        /// <summary>
        /// Reads an object instance from an XML file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object to read from the file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the XML file.</returns>
        public static T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }
}
