using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ApplicationLauncher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        List<AppClass> list = new List<AppClass>();

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                getApplications(@Properties.Settings.Default.userFilePath);

                pnlLaunch.AutoScroll = false;
                pnlLaunch.HorizontalScroll.Enabled = false;
                pnlLaunch.HorizontalScroll.Visible = false;
                pnlLaunch.HorizontalScroll.Maximum = 0;
                pnlLaunch.AutoScroll = true;

                toolTip1.SetToolTip(btnLaunchApp, "Display all apps found in the folder specified in your user settings.");
                toolTip1.SetToolTip(button1, "Set the folder to hold all your application shortcuts.");

                this.ActiveControl = txtFilter;
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void getApplications(string path, string search = "*")
        {
            int xVal = 0;
            int yVal = 0;

            foreach (var file in Directory.EnumerateFiles(path, search, SearchOption.AllDirectories))
            {
                FileInfo info = new FileInfo(file);

                AppClass x = new AppClass();
                x.Name = info.Name;
                x.Path = info.DirectoryName;
                x.Icon = (Icon.ExtractAssociatedIcon(x.Path + @"\" + x.Name)).ToBitmap();
                x.LastAccessed = Convert.ToDateTime(info.CreationTime);

                list.Add(x);

                PictureBox pb = new PictureBox();
                pb.Image = x.Icon;
                pb.Width = 100;
                pb.Height = 100;
                pb.BackColor = Color.WhiteSmoke;
                pb.SizeMode = PictureBoxSizeMode.CenterImage;
                

                pb.MouseClick += new MouseEventHandler((o, a) => confirmDialog(x.Name, (x.Path + @"\" + x.Name)));
                pb.MouseEnter += new EventHandler((o, a) => pb.BackColor = Color.LightGray);
                pb.MouseLeave += new EventHandler((o, a) => pb.BackColor = Color.WhiteSmoke);
                toolTip1.SetToolTip(pb, "Name: " + x.Name + "\nFile Location: " + x.Path + "\nLast Accessed: " + x.LastAccessed.ToShortDateString());

                Label l = new Label();
                l.Text = x.Name.Substring(0, x.Name.Length - 4);
                l.AutoSize = false;
                l.Width = 100;
                l.Height = 100;
                l.TextAlign = ContentAlignment.TopCenter;
                l.ForeColor = System.Drawing.ColorTranslator.FromHtml("#d35400");
                l.Font = new Font("Century Gothic", 12, FontStyle.Regular);

                

                if (xVal * 100 >= pnlLaunch.Width - 400)
                {
                    yVal++;
                    xVal = 0;
                    pb.Location = new Point(xVal * 150, yVal * 200);
                    l.Location = new Point(xVal * 150, (yVal * 200) + 100);

                    
                } else {
                    pb.Location = new Point(xVal * 150, yVal * 200);
                    l.Location = new Point(xVal * 150, (yVal * 200) + 100);
                }

                pnlLaunch.Controls.Add(pb);
                pnlLaunch.Controls.Add(l);

                xVal++;
            }
        }

        private void getActiveApps()
        {
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnActiveApps_Click(object sender, EventArgs e)
        {
            try
            {
                label1.Visible = true;
                label1.Text = "Directories";
                list.Clear();
                pnlLaunch.Controls.Clear();
                txtFilter.Visible = false;

                int xVal = 0;
                int yVal = 0;

                foreach (var file in Directory.EnumerateDirectories(@Properties.Settings.Default.userFolderPath, "*", SearchOption.TopDirectoryOnly))
                {
                    FileInfo info = new FileInfo(file);

                    AppClass x = new AppClass();
                    x.Name = info.Name;
                    x.Path = info.DirectoryName;
                    x.Icon = Properties.Resources.Flat_Folder_icon;
                    x.LastAccessed = Convert.ToDateTime(info.CreationTime);

                    list.Add(x);

                    PictureBox pb = new PictureBox();
                    pb.Image = x.Icon;
                    pb.Width = 100;
                    pb.Height = 100;
                    pb.BackColor = Color.WhiteSmoke;
                    pb.SizeMode = PictureBoxSizeMode.Zoom;

                    pb.MouseClick += new MouseEventHandler((o, a) => System.Diagnostics.Process.Start(x.Path + @"\" + x.Name));
                    pb.MouseEnter += new EventHandler((o, a) => pb.BackColor = Color.LightGray);
                    pb.MouseLeave += new EventHandler((o, a) => pb.BackColor = Color.WhiteSmoke);
                    toolTip1.SetToolTip(pb, "Name: " + x.Name + "\nFile Location: " + x.Path + "\nLast Accessed: " + x.LastAccessed.ToShortDateString());

                    Label l = new Label();
                    l.Text = x.Name.Substring(0, x.Name.Length);
                    l.AutoSize = false;
                    l.Width = 100;
                    l.Height = 100;
                    l.TextAlign = ContentAlignment.TopCenter;
                    l.ForeColor = System.Drawing.ColorTranslator.FromHtml("#d35400");
                    l.Font = new Font("Century Gothic", 12, FontStyle.Regular);



                    if (xVal * 100 >= pnlLaunch.Width - 400)
                    {
                        yVal++;
                        xVal = 0;
                        pb.Location = new Point(xVal * 150, yVal * 200);
                        l.Location = new Point(xVal * 150, (yVal * 200) + 100);


                    }
                    else
                    {
                        pb.Location = new Point(xVal * 150, yVal * 200);
                        l.Location = new Point(xVal * 150, (yVal * 200) + 100);
                    }

                    pnlLaunch.Controls.Add(pb);
                    pnlLaunch.Controls.Add(l);

                    xVal++;
                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLaunchApp_Click(object sender, EventArgs e)
        {
            label1.Visible = true;
            label1.Text = "Applications";
            list.Clear();
            pnlLaunch.Controls.Clear();
            getApplications(@Properties.Settings.Default.userFilePath);
            txtFilter.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            list.Clear();
            pnlLaunch.Controls.Clear();
            txtFilter.Visible = false;

            Label l = new Label();
            l.Text = "Paste the file path to where your application links are stored.";
            l.AutoSize = true;
            l.TextAlign = ContentAlignment.TopLeft;
            l.ForeColor = System.Drawing.ColorTranslator.FromHtml("#d35400");
            l.Font = new Font("Century Gothic", 14, FontStyle.Regular);
            l.Location = new Point(150, 50);

            TextBox t = new TextBox();
            t.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            t.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            t.Location = new System.Drawing.Point(150, 100);
            t.Size = new System.Drawing.Size(589, 27);
            t.TabIndex = 0;

            Button b = new Button();
            b.FlatAppearance.BorderSize = 0;
            b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            b.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            b.ForeColor = System.Drawing.ColorTranslator.FromHtml("#d35400");
            b.BackColor = Color.WhiteSmoke;
            b.Location = new System.Drawing.Point(150, 150);
            b.Name = "btnSaveSettings";
            b.Size = new System.Drawing.Size(200, 48);
            b.Text = "Save Settings";
            b.UseVisualStyleBackColor = true;
            b.Click += new System.EventHandler((o, a) => updateSettings(t.Text));

            pnlLaunch.Controls.Add(l);
            pnlLaunch.Controls.Add(t);
            pnlLaunch.Controls.Add(b);



            Label l2 = new Label();
            l2.Text = "Paste the directory path to the folder with the root directory is.";
            l2.AutoSize = true;
            l2.TextAlign = ContentAlignment.TopLeft;
            l2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#d35400");
            l2.Font = new Font("Century Gothic", 14, FontStyle.Regular);
            l2.Location = new Point(150, 300);

            TextBox t2 = new TextBox();
            t2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            t2.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            t2.Location = new System.Drawing.Point(150, 350);
            t2.Size = new System.Drawing.Size(589, 27);
            t2.TabIndex = 0;

            Button b2 = new Button();
            b2.FlatAppearance.BorderSize = 0;
            b2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            b2.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            b2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#d35400");
            b2.BackColor = Color.WhiteSmoke;
            b2.Location = new System.Drawing.Point(150, 400);
            b2.Name = "btnSaveSettings";
            b2.Size = new System.Drawing.Size(200, 48);
            b2.Text = "Save Settings";
            b2.UseVisualStyleBackColor = true;
            b2.Click += new System.EventHandler((o, a) => updateSettingsDIR(t2.Text));

            pnlLaunch.Controls.Add(l2);
            pnlLaunch.Controls.Add(t2);
            pnlLaunch.Controls.Add(b2);
        }

        private void updateSettings(string path)
        {
            Properties.Settings.Default.userFilePath = path;
            Properties.Settings.Default.Save();
            MessageBox.Show("Updated file path to " + path);
        }

        private void updateSettingsDIR(string path)
        {
            Properties.Settings.Default.userFolderPath = path;
            Properties.Settings.Default.Save();
            MessageBox.Show("Updated directory path to " + path);
        }


        Point mouseDownPoint = Point.Empty;

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownPoint = new Point(e.X, e.Y);
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDownPoint = Point.Empty;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDownPoint.IsEmpty)
            {
                return;
            }
            Form f = sender as Form;
            f.Location = new Point(f.Location.X + (e.X - mouseDownPoint.X), f.Location.Y + (e.Y - mouseDownPoint.Y));
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void confirmDialog(string appName, string path)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to open " + appName.Substring(0, appName.Length - 4) + "?", "Just checking", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(path);
            }
            else if (dialogResult == DialogResult.No)
            {
                
            }
        }

        private void txtFilter_KeyUp(object sender, KeyEventArgs e)
        {
            list.Clear();
            pnlLaunch.Controls.Clear();
            string filter = "*" + txtFilter.Text + "*";
            getApplications(@Properties.Settings.Default.userFilePath, filter);
        }
    }
}
