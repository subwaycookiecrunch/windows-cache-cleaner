using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WindowsCacheCleaner
{
    public partial class MainForm : Form
    {
        private CheckBox chkWindowsTemp;
        private CheckBox chkPrefetch;
        private CheckBox chkChrome;
        private CheckBox chkFirefox;
        private CheckBox chkEdge;
        private Button btnClean;
        private Button btnExit;
        private Label lblStatus;
        private ProgressBar progressBar;
        private Panel panelOptions;
        private GroupBox grpWindows;
        private GroupBox grpBrowsers;
        private Button btnSelectAll;
        private Button btnDeselectAll;
        
        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Windows Cache Cleaner";
            this.Size = new System.Drawing.Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            
            // Windows cleanup options group
            grpWindows = new GroupBox();
            grpWindows.Text = "Windows Cache";
            grpWindows.Size = new System.Drawing.Size(250, 100);
            grpWindows.Location = new System.Drawing.Point(20, 20);

            chkWindowsTemp = new CheckBox();
            chkWindowsTemp.Text = "Windows Temp Files";
            chkWindowsTemp.Checked = true;
            chkWindowsTemp.Location = new System.Drawing.Point(15, 25);
            chkWindowsTemp.Width = 200;

            chkPrefetch = new CheckBox();
            chkPrefetch.Text = "Windows Prefetch";
            chkPrefetch.Checked = true;
            chkPrefetch.Location = new System.Drawing.Point(15, 50);
            chkPrefetch.Width = 200;

            grpWindows.Controls.Add(chkWindowsTemp);
            grpWindows.Controls.Add(chkPrefetch);

            // Browser cleanup options group
            grpBrowsers = new GroupBox();
            grpBrowsers.Text = "Browser Cache";
            grpBrowsers.Size = new System.Drawing.Size(250, 150);
            grpBrowsers.Location = new System.Drawing.Point(300, 20);

            chkChrome = new CheckBox();
            chkChrome.Text = "Google Chrome";
            chkChrome.Checked = true;
            chkChrome.Location = new System.Drawing.Point(15, 25);
            chkChrome.Width = 200;

            chkFirefox = new CheckBox();
            chkFirefox.Text = "Mozilla Firefox";
            chkFirefox.Checked = true;
            chkFirefox.Location = new System.Drawing.Point(15, 50);
            chkFirefox.Width = 200;

            chkEdge = new CheckBox();
            chkEdge.Text = "Microsoft Edge";
            chkEdge.Checked = true;
            chkEdge.Location = new System.Drawing.Point(15, 75);
            chkEdge.Width = 200;

            grpBrowsers.Controls.Add(chkChrome);
            grpBrowsers.Controls.Add(chkFirefox);
            grpBrowsers.Controls.Add(chkEdge);

            // Selection buttons
            btnSelectAll = new Button();
            btnSelectAll.Text = "Select All";
            btnSelectAll.Location = new System.Drawing.Point(20, 130);
            btnSelectAll.Size = new System.Drawing.Size(100, 30);
            btnSelectAll.Click += new EventHandler(btnSelectAll_Click);

            btnDeselectAll = new Button();
            btnDeselectAll.Text = "Deselect All";
            btnDeselectAll.Location = new System.Drawing.Point(130, 130);
            btnDeselectAll.Size = new System.Drawing.Size(100, 30);
            btnDeselectAll.Click += new EventHandler(btnDeselectAll_Click);

            // Action buttons
            btnClean = new Button();
            btnClean.Text = "Clean Now";
            btnClean.Location = new System.Drawing.Point(150, 280);
            btnClean.Size = new System.Drawing.Size(120, 40);
            btnClean.Click += new EventHandler(btnClean_Click);

            btnExit = new Button();
            btnExit.Text = "Exit";
            btnExit.Location = new System.Drawing.Point(300, 280);
            btnExit.Size = new System.Drawing.Size(120, 40);
            btnExit.Click += new EventHandler(btnExit_Click);

            // Status and progress
            lblStatus = new Label();
            lblStatus.Text = "Ready";
            lblStatus.Location = new System.Drawing.Point(20, 210);
            lblStatus.Size = new System.Drawing.Size(560, 20);

            progressBar = new ProgressBar();
            progressBar.Location = new System.Drawing.Point(20, 240);
            progressBar.Size = new System.Drawing.Size(560, 20);
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Value = 0;
            progressBar.Style = ProgressBarStyle.Continuous;

            // Add controls to form
            this.Controls.Add(grpWindows);
            this.Controls.Add(grpBrowsers);
            this.Controls.Add(btnSelectAll);
            this.Controls.Add(btnDeselectAll);
            this.Controls.Add(lblStatus);
            this.Controls.Add(progressBar);
            this.Controls.Add(btnClean);
            this.Controls.Add(btnExit);
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            chkWindowsTemp.Checked = true;
            chkPrefetch.Checked = true;
            chkChrome.Checked = true;
            chkFirefox.Checked = true;
            chkEdge.Checked = true;
        }

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            chkWindowsTemp.Checked = false;
            chkPrefetch.Checked = false;
            chkChrome.Checked = false;
            chkFirefox.Checked = false;
            chkEdge.Checked = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            int totalTasks = 0;
            int completedTasks = 0;

            // Count selected tasks
            if (chkWindowsTemp.Checked) totalTasks++;
            if (chkPrefetch.Checked) totalTasks++;
            if (chkChrome.Checked) totalTasks++;
            if (chkFirefox.Checked) totalTasks++;
            if (chkEdge.Checked) totalTasks++;

            if (totalTasks == 0)
            {
                MessageBox.Show("Please select at least one option to clean.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            btnClean.Enabled = false;

            try
            {
                // Windows temp files
                if (chkWindowsTemp.Checked)
                {
                    lblStatus.Text = "Cleaning Windows Temp Files...";
                    progressBar.Value = (int)((++completedTasks / (float)totalTasks) * 100);
                    Application.DoEvents();
                    
                    CleanWindowsTempFiles();
                }

                // Prefetch files
                if (chkPrefetch.Checked)
                {
                    lblStatus.Text = "Cleaning Windows Prefetch...";
                    progressBar.Value = (int)((++completedTasks / (float)totalTasks) * 100);
                    Application.DoEvents();
                    
                    CleanPrefetchFiles();
                }

                // Chrome cache
                if (chkChrome.Checked)
                {
                    lblStatus.Text = "Cleaning Chrome Cache...";
                    progressBar.Value = (int)((++completedTasks / (float)totalTasks) * 100);
                    Application.DoEvents();
                    
                    CleanChromeCache();
                }

                // Firefox cache
                if (chkFirefox.Checked)
                {
                    lblStatus.Text = "Cleaning Firefox Cache...";
                    progressBar.Value = (int)((++completedTasks / (float)totalTasks) * 100);
                    Application.DoEvents();
                    
                    CleanFirefoxCache();
                }

                // Edge cache
                if (chkEdge.Checked)
                {
                    lblStatus.Text = "Cleaning Edge Cache...";
                    progressBar.Value = (int)((++completedTasks / (float)totalTasks) * 100);
                    Application.DoEvents();
                    
                    CleanEdgeCache();
                }

                lblStatus.Text = "All cleaning tasks completed!";
                MessageBox.Show("Cache cleaning completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = $"Error: {ex.Message}";
            }
            finally
            {
                btnClean.Enabled = true;
            }
        }

        private void CleanWindowsTempFiles()
        {
            string tempPath = Path.GetTempPath();
            DeleteFilesInDirectory(tempPath);
            
            // Also clean Windows temp directory
            string winTempPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Temp");
            DeleteFilesInDirectory(winTempPath);
        }

        private void CleanPrefetchFiles()
        {
            string prefetchPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Prefetch");
            DeleteFilesInDirectory(prefetchPath);
        }

        private void CleanChromeCache()
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string chromeCachePath = Path.Combine(localAppData, @"Google\Chrome\User Data\Default\Cache");
            DeleteFilesInDirectory(chromeCachePath);
        }

        private void CleanFirefoxCache()
        {
            string appDataRoaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            
            // Find Firefox profile directories
            string firefoxProfilesPath = Path.Combine(appDataRoaming, @"Mozilla\Firefox\Profiles");
            
            if (Directory.Exists(firefoxProfilesPath))
            {
                foreach (string profileDir in Directory.GetDirectories(firefoxProfilesPath))
                {
                    string cachePath = Path.Combine(profileDir, "cache");
                    string cache2Path = Path.Combine(profileDir, "cache2");
                    
                    DeleteFilesInDirectory(cachePath);
                    DeleteFilesInDirectory(cache2Path);
                }
            }
        }

        private void CleanEdgeCache()
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string edgeCachePath = Path.Combine(localAppData, @"Microsoft\Edge\User Data\Default\Cache");
            DeleteFilesInDirectory(edgeCachePath);
        }

        private void DeleteFilesInDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                return;

            DirectoryInfo di = new DirectoryInfo(directoryPath);

            try
            {
                // Try to delete each file
                foreach (FileInfo file in di.GetFiles())
                {
                    try
                    {
                        file.Delete();
                    }
                    catch
                    {
                        // Ignore errors for individual files
                    }
                }

                // Try to delete each subdirectory
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    try
                    {
                        dir.Delete(true);
                    }
                    catch
                    {
                        // Ignore errors for individual directories
                    }
                }
            }
            catch (Exception)
            {
                // Handle access denied or other issues
            }
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Run as administrator
            if (IsAdministrator())
            {
                Application.Run(new MainForm());
            }
            else
            {
                MessageBox.Show("This application needs administrative privileges to clean some cache locations. Please run as administrator.", 
                             "Administrator Privileges Required", 
                             MessageBoxButtons.OK, 
                             MessageBoxIcon.Warning);
                
                // Try to restart with admin privileges
                RestartAsAdmin();
            }
        }

        private static bool IsAdministrator()
        {
            try
            {
                System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
                return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
            }
            catch
            {
                return false;
            }
        }

        private static void RestartAsAdmin()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.UseShellExecute = true;
                startInfo.WorkingDirectory = Environment.CurrentDirectory;
                startInfo.FileName = Application.ExecutablePath;
                startInfo.Verb = "runas"; // Request admin privileges
                Process.Start(startInfo);
                Application.Exit();
            }
            catch
            {
                // User cancelled the UAC prompt or other error
                Application.Exit();
            }
        }
    }
} 