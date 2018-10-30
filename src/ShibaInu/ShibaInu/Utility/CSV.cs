using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ShibaInu
{
    /// <summary>
    /// Class to hold Comma Separate Values and Logic for CSV Files
    /// </summary>
    public class CSV
    {
        /// <summary>
        /// Header/Columns
        /// </summary>
        public List<string> Columns = new List<string>();

        /// <summary>
        /// CSV Rows and Columns
        /// </summary>
        public List<List<string>> Rows = new List<List<string>>();

        /// <summary>
        /// Initializes CSVs
        /// </summary>
        public CSV() { }

        /// <summary>
        /// Initializes CSVs with a file
        /// </summary>
        public CSV(string fileName)
        {
            Load(fileName);
        }

        /// <summary>
        /// Gets Value at the given Row and Column
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="column">Column Index</param>
        /// <returns>Value</returns>
        public string GetData(int row, int column)
        {
            return Rows[row][column];
        }

        /// <summary>
        /// Gets Value at the given Row and Column
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="column">Column Name</param>
        /// <returns>Value</returns>
        public string GetData(int row, string column)
        {
            return Rows[row][Columns.IndexOf(column)];
        }

        public void Save(string file)
        {
            using (StreamWriter writer = new StreamWriter(file))
            {
                // Write Headers
                for (int i = 0; i < Columns.Count; i++)
                    writer.Write("{0}{1}", Columns[i], i == Columns.Count - 1 ? "" : ",");

                writer.WriteLine();

                // Write Rows
                for(int i = 0; i < Rows.Count; i++)
                {
                    for(int j = 0; j < Rows[i].Count; j++)
                    {
                        writer.Write("{0}{1}", Rows[i][j], j == Rows[i].Count - 1 ? "" : ",");
                    }

                    writer.WriteLine();
                }
            }
        }

        /// <summary>
        /// Loads a CSV File
        /// </summary>
        /// <param name="file">File Path</param>
        public void Load(string file)
        {
            // Clear previously loaded data
            Columns.Clear();
            Rows.Clear();

            // Create IO
            using (TextFieldParser parser = new TextFieldParser(file))
            {
                // Set Delimiter (Comma for CSV)
                parser.SetDelimiters(",");
                parser.CommentTokens = new string[] { "#" };

                var columns = parser.ReadFields();

                if (columns == null)
                    return;

                // Read Header
                Columns = columns.ToList();

                // Read fields until EOF
                while (!parser.EndOfData)
                    Rows.Add(parser.ReadFields().ToList());
            }
        }
    }
}
