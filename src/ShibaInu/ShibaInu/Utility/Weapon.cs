using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ShibaInu
{
    /// <summary>
    /// Class to hold Weapon file settings and Logic
    /// </summary>
    class Weapon
    {
        /// <summary>
        /// Weapon Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Weapon Template for GDT Conversion
        /// </summary>
        public GameDataTable Template { get; set; }

        /// <summary>
        /// Weapon Settings
        /// </summary>
        public Dictionary<string, string> Settings = new Dictionary<string, string>();

        /// <summary>
        /// Initializes Weapon
        /// </summary>
        public Weapon() { }

        /// <summary>
        /// Initializes Weapon and Loads a Weapon File
        /// </summary>
        /// <param name="filePath">File Path</param>
        public Weapon(string filePath)
        {
            Load(filePath);
        }

        /// <summary>
        /// Clears previous settings and loads Weapon File
        /// </summary>
        /// <param name="filePath">File Path</param>
        public void Load(string filePath)
        {
            // Clear previous settings
            Settings.Clear();

            // Use File Name for our Weapon Name
            Name = Path.GetFileNameWithoutExtension(filePath);

            // Read and split by each setting
            string[] weaponfile = File.ReadAllText(filePath).Split('\\');

            // Check if we have settings
            if (weaponfile.Length == 0)
                throw new Exception("Empty Weapon File. (weaponfile.Length == 0)");

            // Check Header
            if (weaponfile[0] != "WEAPONFILE")
                throw new Exception("Expecting WEAPONFILE header. (weaponfile[0] != \"WEAPONFILE\")");

            // Check Settings Count (Must be even)
            if ((weaponfile.Length - 1) % 2 != 0)
                throw new Exception("Weapon File split is not even. ((weaponfile.Length - 1 % 2) != 0)");

            // Parse settings, each setting is group by KVP, so increment 2
            for (int i = 1; i < weaponfile.Length; i += 2)
                Settings[weaponfile[i]] = weaponfile[i + 1];
        }

        /// <summary>
        /// Returns Name of this Weapon file
        /// </summary>
        /// <returns>Weapom Name</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}