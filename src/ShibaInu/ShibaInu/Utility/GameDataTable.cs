﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShibaInu
{
    /// <summary>
    /// Class to hold Game Data Table Assets and Read/Write Logic
    /// </summary>
    public class GameDataTable
    {
        /// <summary>
        /// Class to hold GDT Asset Information
        /// </summary>
        public class Asset
        {
            /// <summary>
            /// Name of Asset
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Type of Asset (GDF)
            /// </summary>
            public string Type { get; set; }

            /// <summary>
            /// If this asset is derived or not
            /// </summary>
            public bool Derivative { get; set; }

            /// <summary>
            /// Asset Properties/Settings
            /// </summary>
            public Dictionary<string, string> Properties = new Dictionary<string, string>();

            /// <summary>
            /// Initializes a GDT Asset
            /// </summary>
            public Asset() { }

            /// <summary>
            /// Initializes a GDT Asset with Name and Type
            /// </summary>
            /// <param name="name">Name of the Asset</param>
            /// <param name="type">Type of Asset (GDF)</param>
            public Asset(string name, string type)
            {
                Name = name;
                Type = type;
            }

            /// <summary>
            /// Clones this Asset
            /// </summary>
            /// <returns>Resulting Asset</returns>
            public Asset Copy()
            {
                // New Asset
                var result = new Asset(Name, Type) { Derivative = Derivative };

                // Copy properties
                foreach (var property in Properties)
                    result.Properties[property.Key] = property.Value;

                // Return result
                return result;
            }
        }

        /// <summary>
        /// Name of this GDT
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Assets within this Game Data Table
        /// </summary>
        public Dictionary<string, Asset> Assets { get; set; }

        /// <summary>
        /// Initializes a Game Data Table with an Asset List
        /// </summary>
        public GameDataTable()
        {
            Assets = new Dictionary<string, Asset>();
        }

        /// <summary>
        /// Initializes a Game Data Table with a GDT file
        /// </summary>
        /// <param name="fileName">GDT File to Load</param>
        public GameDataTable(string fileName)
        {
            Name = Path.GetFileNameWithoutExtension(fileName);
            Load(fileName);
        }

        /// <summary>
        /// Saves Game Data Table to a File
        /// </summary>
        /// <param name="fileName">File Path to save to</param>
        public void Save(string fileName)
        {
            // Create output stream
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                // Write initial bracket
                writer.WriteLine("{");

                // Write each asset
                foreach(var asset in Assets)
                {
                    // Get whether this asset is derived or not
                    bool isDerived = asset.Value.Derivative;

                    // Write name and type
                    writer.WriteLine("	\"{0}\" {2} \"{1}\" {3}", 
                        asset.Key, 
                        asset.Value.Type + (isDerived ? "" : ".gdf"),
                        isDerived ? "[" : "(",
                        isDerived ? "]" : ")");

                    // Write initial setting bracket
                    writer.WriteLine("	{");

                    // Write Settings by KVP
                    foreach (var setting in asset.Value.Properties)
                        writer.WriteLine("		\"{0}\" \"{1}\"", setting.Key, setting.Value);

                    // Write end bracket
                    writer.WriteLine("	}");
                }

                // Write EOF bracket
                writer.WriteLine("}");

            }
        }

        /// <summary>
        /// Loads a Game Data Table from a File
        /// </summary>
        /// <param name="fileName">File Path of GDT</param>
        public void Load(string fileName)
        {
            // Create new asset list
            Assets = new Dictionary<string, Asset>();

            // Read GDT lines
            string[] lines = File.ReadAllLines(fileName);

            // Current State (0 = Begin, 1 = Passed First Brace, 2 = Parsing Asset Name 3 = Parsing Asset, 4 = Done)
            int state = -1;

            // Active Asset
            Asset activeAsset = null;

            // Regex
            MatchCollection matches;

            // Settings
            string key;
            string value;

            // Loop all lines
            for(int i = 0; i < lines.Length; i++)
            {
                // Trim line
                string lineTrim = lines[i].Trim();

                // Check for empty line
                if (String.IsNullOrWhiteSpace(lineTrim))
                    continue;

                // Check for initial bracket
                if(lineTrim == "{" && state == -1)
                {
                    // Set to 0, we're parsing Asset Name/Type
                    state = 0;
                }
                // Check for asset bracket
                else if (lineTrim == "{" && state == 2)
                {
                    // Set to 3, we're parsing settings
                    state = 3;
                }
                // Check for asset end bracket
                else if (lineTrim == "}" && state == 3)
                {
                    // Check for duplicate
                    if(Assets.ContainsKey(activeAsset.Name))
                        throw new ArgumentException(String.Format("Error - Duplicate Asset: {0}.", activeAsset.Name));

                    // Add asset
                    Assets.Add(activeAsset.Name, activeAsset);

                    // Reset state
                    state = 0;
                }
                // Check for EOF Bracket
                else if (lineTrim == "}" && state == 0)
                {
                    // Done
                    state = 4;
                    break;
                }
                // Parse Asset Name + Type
                else if (state == 0)
                {
                    // Check for normal parentheses
                    matches = Regex.Matches(lineTrim, "\\(([^\\)]*)\\)");

                    // Create new asset
                    activeAsset = new Asset() { Derivative = matches.Count == 0 };

                    // No matches, check for derived parentheses
                    if (matches.Count == 0)
                        matches = Regex.Matches(lineTrim, "\\[([^\\)]*)\\]");

                    // Last check
                    if (matches.Count == 0)
                        throw new ArgumentException(String.Format("Parse error on line: {0}. Expecting Parentheses for GDF or Derived Asset.", i));

                    // Get Quotes
                    matches = Regex.Matches(lineTrim, "\"([^\"]*)\"");

                    // Check matches (we should have 2 (Asset Name and GDF/Parent))
                    if (matches.Count < 2)
                        throw new ArgumentException(String.Format("Parse error on line: {0}. Expecting Asset Name and GDF/Parent.", i));

                    // Set Name/Type
                    activeAsset.Name = matches[0].Value.Replace("\"", "");
                    activeAsset.Type = matches[1].Value.Replace("\"", "").Replace(".gdf", "");

                    // Set State
                    state = 2;
                }
                // Check for Asset Settings
                else if(state == 3)
                {
                    // Get Quotes
                    matches = Regex.Matches(lineTrim, "\"([^\"]*)\"");

                    // Check matches (we should have 2 (Asset Name and GDF/Parent))
                    if (matches.Count < 2)
                        throw new ArgumentException(String.Format("Parse error on line: {0}. Expecting Setting Group.", i + 1));

                    // Set Values
                    key = matches[0].Value.Replace("\"", "");
                    value = matches[1].Value.Replace("\"", "");

                    // Check for duplicate setting
                    if(activeAsset.Properties.ContainsKey(key))
                        throw new ArgumentException(String.Format("Error - Duplicate Setting: {0}. Asset: {1}. Line: {2}", key, activeAsset.Name, i + 1));

                    // Add setting
                    activeAsset.Properties.Add(key, value);
                }
                // Unknown Line
                else
                {
                    // Unexpected token/Syntax issue
                    throw new ArgumentException(String.Format("Parse error on line: {0}. Unexpected line {1}.", i + 1, lineTrim));
                }
            }

            // Check did we finish up on end bracket
            if(state != 4)
            {
                // Unexpected token/Syntax issue
                throw new ArgumentException(String.Format("Expecting EOF Bracket."));
            }
        }

        /// <summary>
        /// Returns Name of this GDT file
        /// </summary>
        /// <returns>GDT Name</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
