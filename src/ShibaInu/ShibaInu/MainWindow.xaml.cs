using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Reflection;

namespace ShibaInu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Values from Marvel's Tables that require division
        /// </summary>
        public string[] RequiresDivision =
        {
            "introFireTime",
            "introFireLength",
            "fireTime",
            "lastFireTime",
            "fireDelay",
            "holdFireTime",
            "burstFireDelay",
            "jamFireTime",
            "spinUpTime",
            "spinDownTime",
            "spinRate",
            "slamTime",
            "reloadTime",
            "reloadEmptyTime",
            "reloadAddTime",
            "reloadQuickTime",
            "reloadQuickEmptyTime",
            "reloadQuickAddTime",
            "reloadStartAddTime",
            "reloadStartTime",
            "reloadEndTime",
            "reloadEmptyAddTime",
            "reloadQuickEmptyAddTime",
            "reloadSpecialComboTime",
            "reloadSpecialComboEmptyTime",
            "reloadSpecialComboAddTime",
            "reloadSpecialComboQuickTime",
            "reloadSpecialComboQuickEmptyTime",
            "reloadSpecialComboQuickAddTime",
            "reloadSpecialComboEmptyAddTime",
            "reloadSpecialComboQuickEmptyAddTime",
            "rechamberTime",
            "rechamberBoltTime",
            "dropTime",
            "raiseTime",
            "firstRaiseTime",
            "altDropTime",
            "altRaiseTime",
            "adsAltDropTime",
            "adsAltRaiseTime",
            "quickDropTime",
            "quickRaiseTime",
            "emptyDropTime",
            "emptyRaiseTime",
            "chargeSprintInTime",
            "chargeSprintLoopTime",
            "chargeSprintOutTime",
            "sprintInTime",
            "sprintLoopTime",
            "sprintOutTime",
            "sprintCombatTime",
            "sprintCombatCooldownTime",
            "lowReadyInTime",
            "lowReadyLoopTime",
            "lowReadyOutTime",
            "contFireInTime",
            "contFireLoopTime",
            "contFireOutTime",
            "crawlInTime",
            "crawlForwardTime",
            "crawlBackTime",
            "crawlRightTime",
            "crawlLeftTime",
            "crawlOutFireTime",
            "crawlOutTime",
            "slideInTime",
            "slideLoopTime",
            "slideOutTime",
            "leapInTime",
            "leapLoopTime",
            "leapCancelTime",
            "leapOutTime",
            "diveInTime",
            "diveLoopTime",
            "diveOutTime",
            "swimFromLandTime",
            "swimIdleLoopTime",
            "swimCombatInTime",
            "swimCombatCooldownTime",
            "swimCombatOutTime",
            "swimMovingInTime",
            "swimMovingForwardTime",
            "swimMovingBackwardTime",
            "swimMovingLeftTime",
            "swimMovingRightTime",
            "swimMovingOutTime",
            "swimSprintInTime",
            "swimSprintLoopTime",
            "swimSprintSurfaceLoopTime",
            "swimSprintOutTime",
            "swimToLandTime",
            "swimDropTime",
            "wallRunInTime",
            "wallRunLoopTime",
            "wallRunOutTime",
            "doubleJumpInTime",
            "doubleJumpLoopTime",
            "doubleJumpCancelTime",
            "doubleJumpOutTime",
            "castInTime",
            "castLoopTime",
            "castOutTime",
            "castOutHitTime",
            "castTime",
            "weaponSwitchCancelTransitionTime",
            "adsTransInTime",
            "adsTransInTimeSprint",
            "adsTransOutTime",
        };

        /// <summary>
        /// MainWindow Constructor
        /// </summary>
        public MainWindow()
        {
            // Initialize
            InitializeComponent();

            // Load Templates
            LoadTemplates(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "WeaponTemplates"));
        }

        /// <summary>
        /// Loads template GDT files
        /// </summary>
        private void LoadTemplates(string folderPath)
        {
            // Check does it exist
            if(!Directory.Exists(folderPath))
            {
                // Doesn't exist (what a mad lad for deleting the folder)
                MessageBox.Show("Failed to find WeaponTemplates folder. Shiba cannot do his magic without this folder and its files.\n\nAborting.", "ShibaInu", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }

            // Get all GDTs from it
            string[] files = Directory.GetFiles(folderPath, "*.gdt", SearchOption.TopDirectoryOnly);

            // Check do we have any hits
            if(files.Length == 0)
            {
                // No files (what a mad lad for deleting the files)
                MessageBox.Show("There are no GDT templates in the WeaponTemplates folder. Shiba cannot do his magic without these template files.\n\nAborting.", "ShibaInu", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }

            // Loop them
            foreach (var file in files)
            {
                // Try load the GDT
                try
                {
                    // Get name of this template by file name
                    string templateName = Path.GetFileNameWithoutExtension(file);

                    // Load
                    var gdtFile = new GameDataTable(file);

                    // Check does the template asset exist
                    if(!gdtFile.Assets.ContainsKey("template"))
                    {
                        // Asset doesn't exist
                        MessageBox.Show(String.Format("Failed to find the template asset in {0}. Shibba cannot do his magic without the template asset.\n\nAborting.", Path.GetFileName(file)), "ShibaInu", MessageBoxButton.OK, MessageBoxImage.Error);
                        Close();
                        return;
                    }

                    // Add to combo box
                    UITemplateList.Items.Add(gdtFile);
                }
                catch(Exception e)
                {
                    // No files (what a mad lad for deleting the files)
                    MessageBox.Show(String.Format("Shiba ran into an unexpected issue while loading {0}:\n\n{1}.\n\nAborting.", Path.GetFileName(file), e), "ShibaInu", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                    return;
                }
            }
        }
        
        /// <summary>
        /// Handles processing Weapon files on drop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UIWeaponListFileDrop(object sender, DragEventArgs e)
        {
            // Get dropped files
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];

            // Check did we actually get files
            if (files != null)
            {
                // Loop them
                foreach (string file in files)
                {
                    // Get extension
                    var extension = Path.GetExtension(file);

                    // Only accept files with no extension or CSV
                    if (String.IsNullOrWhiteSpace(extension))
                        LoadWeaponFile(file);
                    else if (extension == ".csv")
                        LoadWeaponTable(file);
                }
            }
        }

        /// <summary>
        /// Loads a weapon file into the Weapons list
        /// </summary>
        private void LoadWeaponFile(string fileName)
        {
            // Attempt to load it
            try
            {
                // Load it
                UIWeaponList.Items.Add(new Weapon(fileName) { Template = UITemplateList.SelectedItem as GameDataTable });
            }
            catch(Exception e)
            {
                // Failed
                MessageBox.Show(String.Format("Shiba failed to parse {0}. Error: \n\n{1}", Path.GetFileName(fileName), e), "ShibaInu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadWeaponTable(string fileName)
        {
            // Attempt to load it
            try
            {
                // Load CSV
                var csvFile = new CSV(fileName);

                // Check did we get any columns
                if(csvFile.Columns.Count == 0)
                {
                    // Failed
                    MessageBox.Show(String.Format("Shiba found no columns in the header of {0}.", Path.GetFileName(fileName)), "ShibaInu", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Check did we get any rows
                if (csvFile.Rows.Count == 0)
                {
                    // Failed
                    MessageBox.Show(String.Format("Shiba found no rows in {0}.", Path.GetFileName(fileName)), "ShibaInu", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Check first column
                if (csvFile.Columns[0] != "WEAPONFILE")
                {
                    // Failed
                    MessageBox.Show(String.Format("Shiba was expecting WEAPONFILE in the first column of {0}, this CSV's first column is {1}", Path.GetFileName(fileName), csvFile.Columns[0]), "ShibaInu", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Loop over each weapon
                foreach (var row in csvFile.Rows)
                {
                    // New Weapon
                    var weapon = new Weapon() { Name = row[0], Template = UITemplateList.SelectedItem as GameDataTable };

                    // Add settings
                    for (int i = 1; i < row.Count; i++)
                    {
                        // Set it
                        weapon.Settings[csvFile.Columns[i]] = row[i].Replace("TRUE", "1").Replace("FALSE", "0");

                        // Check do we need to convert the value
                        if(RequiresDivision.Contains(csvFile.Columns[i]))
                            // Attempt to parse it
                            if(int.TryParse(weapon.Settings[csvFile.Columns[i]], out int result))
                                // Set it
                                weapon.Settings[csvFile.Columns[i]] = (result / 1000.0).ToString();
                    }

                    // Add to list
                    UIWeaponList.Items.Add(weapon);
                }
            }
            catch (Exception e)
            {
                // Failed
                MessageBox.Show(String.Format("Shiba failed to parse {0}. Error: \n\n{1}", Path.GetFileName(fileName), e), "ShibaInu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Displays selected Weapon's settings
        /// </summary>
        private void UIWeaponListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Check if it is a valid weapon
            if (UIWeaponList.SelectedItem is Weapon weapon)
            {
                // Set Item Source
                UIWeaponSettings.ItemsSource = weapon.Settings;
                // Set Template
                UITemplateList.SelectedItem = weapon.Template;
            }
        }

        private void LoadWeaponClick(object sender, RoutedEventArgs e)
        {
            // Create Dialog
            var fileDialog = new OpenFileDialog()
            {
                Title = "Select Weapon Files or Tables to Load",
                Multiselect = true,
            };

            // Open Dialog
            if(fileDialog.ShowDialog() == true)
            {
                // Loop results
                foreach (var file in fileDialog.FileNames)
                {
                    // Get extension
                    var extension = Path.GetExtension(file);

                    // Only accept files with no extension or CSV
                    if (String.IsNullOrWhiteSpace(extension))
                        LoadWeaponFile(file);
                    else if (extension == ".csv")
                        LoadWeaponTable(file);
                }
            }
        }

        /// <summary>
        /// Converts Selected Weapons to GDT Assets
        /// </summary>
        private void SaveSelectedClick(object sender, RoutedEventArgs e)
        {
            ConvertWeapons(UIWeaponList.SelectedItems.Cast<Weapon>().ToList());
        }

        /// <summary>
        /// Converts All listed Weapons to GDT Assets
        /// </summary>
        private void SaveAllClick(object sender, RoutedEventArgs e)
        {
            ConvertWeapons(UIWeaponList.Items.Cast<Weapon>().ToList());
        }

        /// <summary>
        /// Opens About Window
        /// </summary>
        private void AboutClick(object sender, RoutedEventArgs e)
        {
            // Show Shade
            UIBlock.Visibility = Visibility.Visible;

            // Show Dialog
            new AboutWindow() { Owner = this }.ShowDialog();

            // Disable Shade
            UIBlock.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Converts a list of Weapons files to a GDT file
        /// </summary>
        private void ConvertWeapons(List<Weapon> weapons)
        {
            // Check did we received any weapons
            if(weapons.Count == 0)
            {
                // Failed
                MessageBox.Show("Shiba received no weapons to convert.", "ShibaInu", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Create Dialog
            var fileDialog = new SaveFileDialog()
            {
                Title = "Save to GDT",
                Filter = "Game Data Table (.gdt)|*.gdt"
            };

            // Show dialog, check for result
            if (fileDialog.ShowDialog() == true)
            {
                // New GDT
                var gdtFile = new GameDataTable();

                // Check if it exists, if it does, load it
                if (File.Exists(fileDialog.FileName))
                {
                    // Attempt to load it
                    try
                    {
                        // Load it
                        gdtFile.Load(fileDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        // Failed
                        MessageBox.Show(String.Format("Shiba failed to parse {0}. Error: \n\n{1}", Path.GetFileName(fileDialog.FileName), ex), "ShibaInu", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                // Loop over items
                foreach (var weapon in weapons)
                    // Add it
                    gdtFile.Assets[weapon.Name] = ConvertWeapon(weapon);

                // Attempt to save
                try
                {
                    // Done, save it
                    gdtFile.Save(fileDialog.FileName);
                }
                catch (Exception ex)
                {
                    // Failed
                    MessageBox.Show(String.Format("Shiba failed to save {0}. Error: \n\n{1}", Path.GetFileName(fileDialog.FileName), ex), "ShibaInu", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        /// <summary>
        /// Converts a Weapon File to a GDT asset
        /// </summary>
        private GameDataTable.Asset ConvertWeapon(Weapon weapon)
        {
            // Get template GDT and Asset
            var templateFile = weapon.Template;
            var templateAsset = templateFile.Assets["template"];

            // Clone it
            var resultAsset = templateAsset.Copy();

            // Active settin
            string activeSetting;

            // Loop over settings
            foreach(var property in templateAsset.Properties)
            {
                // Check for excluder
                if (property.Value.Contains("__EXCLUDE__"))
                {
                    // Purge excluder string
                    resultAsset.Properties[property.Key] = property.Value.Replace("__EXCLUDE__", "");
                    // Skip
                    continue;
                }

                // Check does this setting exist in weapon
                if (weapon.Settings.ContainsKey(property.Key))
                {
                    // Get setting
                    activeSetting = weapon.Settings[property.Key];

                    // Check if it's not empty
                    if (!String.IsNullOrWhiteSpace(activeSetting))
                        // Set it
                        resultAsset.Properties[property.Key] = activeSetting;
                }
            }

            // Return result
            return resultAsset;
        }

        /// <summary>
        /// Handles executing keyboard shortcuts
        /// </summary>
        private void WindowPreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Check for CTRL/CTRL+Shift Modifier first
            if (Keyboard.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift) || Keyboard.Modifiers == ModifierKeys.Control)
            {
                // Switch by which key was pressed
                switch (e.Key)
                {
                    // Clear UI List
                    case Key.X:
                        {
                            // Only accept CTRL+Shift
                            if (Keyboard.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift))
                            {
                                UIWeaponList.Items.Clear();
                                UIWeaponSettings.ItemsSource = null;
                            }
                            // Done
                            break;
                        }
                    // Safe
                    case Key.S:
                        {
                            // Execute based off CTRL or CTRL+S (Save/Save All)
                            if (Keyboard.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift))
                                ConvertWeapons(UIWeaponList.Items.Cast<Weapon>().ToList());
                            else
                                ConvertWeapons(UIWeaponList.SelectedItems.Cast<Weapon>().ToList());
                            // Done
                            break;
                        }
                    // Load
                    case Key.O:
                        {
                            // Only accept CTRL+O
                            if (Keyboard.Modifiers == ModifierKeys.Control)
                                LoadWeaponClick(null, null);
                            // Done
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Handles changing selected weapon's template on change of the selected template
        /// </summary>
        private void UITemplateListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Check if it is a valid weapon
            if (UIWeaponList.SelectedItem is Weapon weapon)
                // Set Template
                weapon.Template = UITemplateList.SelectedItem as GameDataTable;
        }
    }
}
