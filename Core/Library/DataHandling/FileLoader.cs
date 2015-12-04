using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;

namespace Clues.Library.DataHandling
{
    public class FileLoader
    {
        private readonly ILog Logger = LogManager.GetLogger(typeof(FileLoader).Name);

        private readonly Dictionary<string, ColumnDefinition> referenceCols = null;

        private DataTable table = new DataTable();

        public FileLoader()
        {
            referenceCols = KnownHeaders();
        }

        //public DataTable GetData(Type classType, string filePath)
        //{
        //    var engine = new FileHelperEngine<>

        //    var dt = engine.ReadFileAsDT(filePath);

        //    return dt;
        //}




        public void LoadFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("The supplied file does not exist.");

            var lines = File.ReadLines(filePath);

            // Check first line, if it contains knows headers.
            bool first = true;
            foreach (var line in lines)
            {
                if (first)
                {
                    CheckHeader(line);
                    first = false;
                }

                else
                {
                    LoadLine(line);
                }
            }

            // debug
            PrintDataTable(table);


        }

        private void LoadLine(string line)
        {
            List<string> columnData = GetFields(line);


            var newRow = table.NewRow();
            foreach (DataColumn column in table.Columns)
            {
                // get the position in the dataset that correspond to the column
                var pos = referenceCols[column.ColumnName].Position;

                // update the "cell" with the data
                newRow[column.ColumnName] = columnData[pos];
            }

            table.Rows.Add(newRow);
        }

        private void CheckHeader(string line)
        {
            List<string> columnHeaders = GetFields(line);

            int position = 0;
            foreach (var columnName in columnHeaders)
            {
                if (referenceCols.ContainsKey(columnName) && referenceCols[columnName].Count == 0)
                {
                    // maintains 
                    referenceCols[columnName].Count = 1;
                    referenceCols[columnName].Position = position;
                    table.Columns.Add(columnName);
                }

                position++;
            }

            Logger.InfoFormat("The file headers was checked and {0} headers have been loaded out of {1} the file contains.", table.Columns.Count, columnHeaders.Count);

        }

        private static List<string> GetFields(string line)
        {
            return line.Split('\t').ToList().Select(s => s.Trim().ToLower()).ToList();
        }

        private Dictionary<string, ColumnDefinition> KnownHeaders()
        {
            var knowsHeaders =
                "Selected(x)	Parent	Name	ObjectType	NewName	UniqueID	NewParent	Description	ReferenceType	Template	PrimaryReferencedElement	DefaultAttribute	Categories	IsLocked	AreValuesCaptured	StartTime	EndTime	ModifyDate	SecurityString	ExtendedPropertyType	ExtendedPropertyValue	AttributeIsHidden	AttributeIsConfigurationItem	AttributeIsExcluded	AttributeDefaultUOM	AttributeType	AttributeTypeQualifier	AttributeValue	AttributeDataReference	AttributeConfigString"
                    .Split('\t').ToList().Select(s => s.Trim().ToLower()).ToList();

            var dictionary = new Dictionary<string, ColumnDefinition>();

            knowsHeaders.ForEach(h =>
            {
                if (!dictionary.ContainsKey(h))
                    dictionary.Add(h, new ColumnDefinition());
            });

            return dictionary;

        }


        public void PrintDataTable(DataTable table)
        {

            foreach (DataColumn dataColumn in table.Columns)
            {
                Console.Write(dataColumn.ColumnName + '\t');
            }

            Console.Write('\n');

            foreach (DataRow row in table.Rows)
            {
                foreach (var data in row.ItemArray)
                {
                    Console.Write(data.ToString() + '\t');
                }
                Console.Write('\n');
            }
        }


        public class ColumnDefinition
        {
            public int Count { get; set; }
            public int Position { get; set; }
        }
    }
}
