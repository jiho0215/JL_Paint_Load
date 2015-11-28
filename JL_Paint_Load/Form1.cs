using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Data.SqlClient;
using System.Threading;


namespace JL_Paint_Load
{
    public partial class Form1 : Form
    {
        string[] skid_ID = new string[] { "PO", "BF", "X3", "L1", "L2", "L3", "X4", "L4", "L5", "L6", "X5", "IN" };
        List<string> clickMenuList = new List<string>() {"Exit","Config","Show Border"};
        delegate void TimerEventFiredDelegate();//for thread
        delegate void TimerEventFiredDelegate2();//for thread2
        string connectionString = string.Empty;
        Screen[] sc = Screen.AllScreens;
        ContextMenu m = new ContextMenu();
        NotifyIcon noti = new NotifyIcon();

        public Form1()
        {
            InitializeComponent();
            InitialDisplay();
            ConfingLoad();
            ThreadList();
        }

        /// <summary>
        /// Load Config.ini
        /// </summary>
        private void ConfingLoad()
        {
            connectionString  = "Data Source = " + Func.GetEntryValue("MESDB", "MESDB_SERVER") + "; Initial Catalog = " + Func.GetEntryValue("MESDB", "MESDB_NAME") + "; User ID = " + Func.GetEntryValue("MESDB", "MESDB_USER") + "; Password = " + Func.GetEntryValue("MESDB", "MESDB_PSWD") + "";
            this.Controls["MESDB_Display"].Text = Func.GetEntryValue("MESDB", "MESDB_SERVER");
        }

        private void ThreadList()
        {
            Thread.Sleep(1000);
            System.Threading.Timer timer = new System.Threading.Timer(Callback);
            timer.Change(0, 1000);    // dueTime is waiting time (ms) before the Timer starts for the first time. 
            System.Threading.Timer skidUpdate = new System.Threading.Timer(Callback2);
            skidUpdate.Change(0, 3000);
        }
     
        private void Callback(object status)
        {
            BeginInvoke(new TimerEventFiredDelegate(Work));
        }
        private void Work()
        {
            Controls["clock"].Text = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }
        private void Callback2(object status)
        {
            try
            {
                BeginInvoke(new TimerEventFiredDelegate2(getData));
            }catch(Exception e)
            {

            }
        }

        /// <summary>
        /// Initial Display Setting
        /// </summary>
        private void InitialDisplay()
        {
            ///Form Size Setting///
            this.WindowState = FormWindowState.Normal;
            this.Icon = Properties.Resources._1445304131_Halloween;
            this.Size = new Size(1300, 700);
            int clientW = this.ClientSize.Width;
            int clientH = this.ClientSize.Height;                
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.DarkGray;
            this.Text = Func.GetEntryValue("Info", "Name");
            this.SizeChanged += Form1_SizeChanged;
          
            noti.Icon = Properties.Resources._1445304131_Halloween;
            noti.BalloonTipText = "This program is still running on background.";
            noti.BalloonTipTitle = "Pumpkin Load";
            noti.Text = "Pumpkin Load";
            noti.Visible = false;
            noti.MouseDoubleClick += Noti_MouseDoubleClick;
            noti.MouseClick += Noti_MouseClick;
            noti.MouseMove += Noti_MouseMove;

            this.MouseClick += Form1_MouseClick;

            ///Virtical Position Numbers///
            int gap = 5;
            int topLabelH = clientH * 15 / 100;
            int top1stPoint = topLabelH * 30 / 100;
            int top2ndPoint = (topLabelH * 70 / 100) - (gap * 2);
            int bottomLabelH = clientH * 6 / 100;
            int section = clientH * 80 / 100;
            int sec1stH = section * 50 / 100;
            int sec2ndH = section * 50 / 100 - (gap * 2);
            int sec1stPoint = topLabelH;
            int sec2ndPoint = topLabelH + sec1stH + gap;

            ///Virtical Position Numbers///
            Label lbl1 = new Label();
            Label lbl2 = new Label();
            Label lbl3 = new Label();
            Label lbl4 = new Label();
            Label lbl5 = new Label();
            Label lbl6 = new Label();
            Label lbl7 = new Label();
            PictureBox pb1 = new PictureBox();
            Image img1 = Properties.Resources.AniLogo;
            Image img2 = Properties.Resources.UnifySystemsLogo;

            lbl1.Size = new Size(90, top1stPoint);
            lbl1.Location = new Point(gap, gap);
            Assembly assem = Assembly.GetEntryAssembly();
            AssemblyName assemName = assem.GetName();
            Version ver = assemName.Version;
            lbl1.Text = "ver."+ver.ToString();
            lbl1.Font = new Font("arial", 11, FontStyle.Regular);
            lbl1.TextAlign = ContentAlignment.MiddleLeft;
            lbl1.ForeColor = Color.White;
            lbl1.Parent = this;

            lbl2.Size = new Size(150, top1stPoint);
            lbl2.Location = new Point(lbl1.Right + gap, lbl1.Top + (lbl1.Height / 2 - lbl2.Height / 2));
            lbl2.Name = "MESDB_Display";
            lbl2.Text = Func.GetEntryValue("MESDB", "MESDB_SERVER");
            lbl2.Font = new Font("arial", 11, FontStyle.Bold);
            lbl2.ForeColor = Color.Lime;
            lbl2.TextAlign = ContentAlignment.MiddleLeft;
            lbl2.Parent = this;

            lbl3.Size = new Size(this.Width / 3, top2ndPoint);
            lbl3.Location = new Point(30, lbl1.Bottom + 3);
            lbl3.Text = Func.GetEntryValue("Info", "Station");
            lbl3.Font = new Font("arial", 30, FontStyle.Bold);
            lbl3.TextAlign = ContentAlignment.MiddleLeft;
            lbl3.Anchor =  AnchorStyles.Left;
            lbl3.ForeColor = Color.White;
            lbl3.Parent = this;

            pb1.Height = topLabelH - gap * 2;
            pb1.Width = (img1.Width * pb1.Height) / img1.Height;
            pb1.SizeMode = PictureBoxSizeMode.StretchImage;
            pb1.Location = new Point(clientW - (pb1.Width + gap), gap);
            pb1.Image = img1;
            pb1.Anchor = AnchorStyles.Top | AnchorStyles.Right ;
            pb1.Parent = this;
            
            lbl4.Name = "Clock";
            lbl4.Size = new Size(230, pb1.Height / 2);
            lbl4.Location = new Point(pb1.Left - (lbl4.Width + gap), pb1.Bottom - lbl4.Height);
            lbl4.Font = new Font("arial", 15);
            lbl4.ForeColor = Color.Khaki;
            lbl4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lbl4.TextAlign = ContentAlignment.MiddleCenter;
            lbl4.BackColor = Color.MidnightBlue;
            lbl4.Parent = this;

            lbl5.Size = new Size(120, bottomLabelH - gap * 2);
            lbl5.Location = new Point(gap, clientH - (lbl5.Height + gap));
            lbl5.Font = new Font("arial", 14);
            lbl5.Text = "Message";
            lbl5.TextAlign = ContentAlignment.MiddleCenter;
            lbl5.BorderStyle = BorderStyle.FixedSingle;
            lbl5.BackColor = Color.Pink;
            lbl5.Parent = this;
            lbl5.MouseClick += Lbl5_MouseClick;

            lbl6.Size = new Size(150, lbl5.Height);
            lbl6.Location = new Point(clientW - (lbl6.Width + gap), lbl5.Top);
            lbl6.BackColor = Color.MediumBlue;
            lbl6.Image = img2;
            lbl6.ImageAlign = ContentAlignment.MiddleRight;
            lbl6.Font = new Font("arial", 12, FontStyle.Bold);
            lbl6.TextAlign = ContentAlignment.MiddleCenter;
            lbl6.Text = Func.GetEntryValue("Info", "User");
            lbl6.ForeColor = Color.Khaki;
            lbl6.BorderStyle = BorderStyle.FixedSingle;
            lbl6.MouseClick += Lbl6_MouseClick;
            lbl6.Parent = this;

            lbl7.Size = new Size(lbl6.Left - lbl5.Right - (gap * 2), lbl5.Height);
            lbl7.Location = new Point(lbl5.Right + gap, lbl5.Top);
            lbl7.Font = new Font("arial", 14);
            lbl7.Name = "lbl7";
            lbl7.TextAlign = ContentAlignment.MiddleLeft;
            lbl7.BackColor = Color.White;
            lbl7.BorderStyle = BorderStyle.FixedSingle;
            lbl7.Parent = this;

            Panel p1 = new Panel();
            p1.Size = new Size(clientW - gap * 2, sec1stH);
            p1.Location = new Point(gap, sec1stPoint);
            p1.BackColor = Color.LightSkyBlue;
            p1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            p1.BorderStyle = BorderStyle.Fixed3D;
            p1.Name = "p1";
            this.Controls.Add(p1);

            List<string> skid_ID0 = new List<string>() { "PO", "BF", "X3", "L1", "L2", "L3", "X4", "L4", "L5", "L6", "X5", "IN" };

            List<Part> skid_ID1 = new List<Part>();

            skid_ID0.ForEach(x =>
            {
                skid_ID1.Add(new Part(skid_ID0.IndexOf(x), x));
            }
            );
          
            skid_ID1.ForEach(x =>
            {
                int i = x.No();
                int p1_1W = p1.Width / 10;
                int p1_1H = p1.Height / 4;
                int gapW = 5;
                int gapH = 10;
                int startW = (p1.Width - (p1_1W * 9 + gapW * 8)) / 2;
                int startH = (p1.Height - (p1_1H * 3 + gapH * 2)) / 2;

                Panel p1_1 = new Panel();
                p1_1.MouseDown += drDown;
                p1_1.Size = new Size(p1_1W, p1_1H);
                p1_1.Name = "SKID_" + x.Name();
                p1_1.BorderStyle = BorderStyle.None;
                //Locations
                if (i < 2) p1_1.Location = new Point(startW, startH + (i * (p1_1.Height + gapH)));
                else if (i >= 11) p1_1.Location = new Point(startW + (8 * (p1_1.Width + gapW)), startH + (1 * (p1_1.Height + gapH)));
                else p1_1.Location = new Point(startW + ((i - 2) * (p1_1.Width + gapW)), startH + (2 * (p1_1.Height + gapH)));
                p1.Controls.Add(p1_1);
                //End Locations

                //Components inside p1_1
                Label lblProd = new Label();
                Label lblModel = new Label();
                Label lblColor = new Label();
                Label lblSkid = new Label();

                lblProd.Size = new Size(p1_1.Width, p1_1.Height / 3);
                lblProd.Location = new Point(0, 0);
                lblProd.BackColor = Color.White;
                lblProd.Text = "Production";
                lblProd.Font = new Font("arial", 12);
                lblProd.TextAlign = ContentAlignment.MiddleLeft;
                lblProd.BorderStyle = BorderStyle.FixedSingle;
                
                lblProd.Name = "lblProd";
                p1_1.Controls.Add(lblProd);

                lblModel.Size = new Size(p1_1.Width / 2 + 5, p1_1.Height / 3);
                lblModel.Location = new Point(0, lblProd.Bottom - 1);
                lblModel.BackColor = Color.LightGreen;
                lblModel.Text = "Model";
                lblModel.Font = new Font("arial", 12);
                lblModel.TextAlign = ContentAlignment.MiddleCenter;
                lblModel.BorderStyle = BorderStyle.FixedSingle;
                lblModel.Name = "lblModel";
                p1_1.Controls.Add(lblModel);

                lblColor.Size = new Size(p1_1.Width - lblModel.Width + 1, p1_1.Height / 3);
                lblColor.Location = new Point(lblModel.Right - 1, lblProd.Bottom - 1);
                lblColor.BackColor = Color.Gold;
                lblColor.Text = "Color";
                lblColor.Font = new Font("arial", 12);
                lblColor.TextAlign = ContentAlignment.MiddleCenter;
                lblColor.BorderStyle = BorderStyle.FixedSingle;
                lblColor.Name = "lblColor";
                p1_1.Controls.Add(lblColor);

                lblSkid.Size = new Size(p1_1.Width / 3, p1_1.Height / 3);
                lblSkid.Location = new Point((p1_1.Width - lblSkid.Width) / 2, lblModel.Bottom - 1);
                lblSkid.BackColor = Color.DarkGray;
                lblSkid.ForeColor = Color.White;
                lblSkid.Font = new Font("arial", 10);
                lblSkid.TextAlign = ContentAlignment.MiddleCenter;
                lblSkid.BorderStyle = BorderStyle.FixedSingle;
                lblSkid.MouseDown += drDown;
                lblSkid.Name = "lblSkid";
                p1_1.Controls.Add(lblSkid);
            }
            );
            string[] p2titles = new string[] { "Skid ID", "Model", "Production", "Color" };
            string[] p2lbls = new string[] { "SKID_ID", "MODEL", "PART_NAME", "ALC_COLOR" };
            List<string> p2lblsColor = new List<string>() { "Red", "Green", "Navy", "Black" };
            Panel p2 = new Panel();
            p2.Name = "line_l1";
            p2.Size = new Size(p1.Width / 2 - gap/2, sec2ndH);
            p2.Location = new Point(p1.Left, p1.Bottom + gap);
            p2.BackColor = Color.LightSkyBlue;

            p2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            p2.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(p2);

            Label p2_l1 = new Label();
            p2_l1.Size = new Size(p2.Width, p2.Height / 8);
            p2_l1.BackColor = Color.LightBlue;
            p2_l1.BorderStyle = BorderStyle.FixedSingle;
            p2_l1.TextAlign = ContentAlignment.MiddleLeft;
            p2_l1.Text = "Line1 Status";
            p2_l1.Anchor = AnchorStyles.Top | AnchorStyles.Left  | AnchorStyles.Right;
            p2_l1.Font = new Font("Arial", 13, FontStyle.Bold);
            p2_l1.ForeColor = Color.MidnightBlue;
            p2_l1.Parent = p2;

            for (int i = 0; i <= 3; i++)
            {
                int p1_1W = p1.Width / 10;
                int p1_1H = p1.Height / 5;
                int gapW = 5;
                int gapH = 10;
                int startW = (p1.Width - (p1_1W * 9 + gapW * 8)) / 2;
                int startH = (p1.Height - (p1_1H * 3 + gapH * 2)) / 2;

                Label l1 = new Label();
                Label l2 = new Label();

                l1.Size = new Size(200, 35);
                l1.Location = new Point(30, 50 + (i * 47));
                l1.ForeColor = Color.Navy;
                l1.Font = new Font("Arial", 15, FontStyle.Bold);
                l1.Text = p2titles[i];
                l1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
                l1.TextAlign = ContentAlignment.MiddleLeft;

                l1.Parent = p2;

                l2.Size = new Size(300, 35);
                l2.Location = new Point(100 + l1.Width, l1.Top);
                l2.Name = "line_"+ p2lbls[i];
                l2.BorderStyle = BorderStyle.Fixed3D;
                l2.Font = new Font("Arial", 15, FontStyle.Bold);
                l2.ForeColor = Color.FromName(p2lblsColor[i]); //Color.Black;
                l2.BackColor = Color.White;
                l2.Anchor = AnchorStyles.Top| AnchorStyles.Right;
                l2.TextAlign = ContentAlignment.MiddleCenter;
                l2.Parent = p2;
            }// End for

            Panel p3 = new Panel();
            p3.Name = "line_l4";
            p3.Size = new Size(p1.Width / 2 - gap / 2, sec2ndH);
            p3.Location = new Point(p1.Right - p3.Width, p1.Bottom + gap);
            p3.BackColor = Color.LightSkyBlue;
            p3.Anchor = AnchorStyles.Top| AnchorStyles.Right | AnchorStyles.Bottom;
            p3.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(p3);

            Label p3_l1 = new Label();
            p3_l1.Size = new Size(p3.Width, p3.Height / 8);
            p3_l1.BackColor = Color.LightBlue;
            p3_l1.BorderStyle = BorderStyle.FixedSingle;
            p3_l1.TextAlign = ContentAlignment.MiddleLeft;
            p3_l1.Text = "Line4 Status";
            p3_l1.Font = new Font("Arial", 13, FontStyle.Bold);
            p3_l1.ForeColor = Color.MidnightBlue;
            p3_l1.Parent = p3;

            for (int i = 0; i <= 3; i++)
            {
                int p1_1W = p1.Width / 10;
                int p1_1H = p1.Height / 5;
                int gapW = 5;
                int gapH = 10;
                int startW = (p1.Width - (p1_1W * 9 + gapW * 8)) / 2;
                int startH = (p1.Height - (p1_1H * 3 + gapH * 2)) / 2;

                Label l1 = new Label();
                Label l2 = new Label();

                l1.Size = new Size(200, 35);
                l1.Location = new Point(30, 50 + (i * 47));
                l1.ForeColor = Color.Navy;
                l1.Font = new Font("Arial", 15, FontStyle.Bold);
                l1.Text = p2titles[i];
                l1.TextAlign = ContentAlignment.MiddleLeft;
                l1.Parent = p3;

                l2.Size = new Size(300, 35);
                l2.Location = new Point(100 + l1.Width, l1.Top);
                l2.Name = "line_" + p2lbls[i];
                l2.BorderStyle = BorderStyle.Fixed3D;
                l2.Font = new Font("Arial", 15, FontStyle.Bold);
                l2.ForeColor = Color.Black;
                l2.BackColor = Color.White;
                l2.TextAlign = ContentAlignment.MiddleCenter;
                l2.Parent = p3;
            }// End for
        }//End InitialDisplay

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) ToTray();
        }

        private void Form1_MaximumSizeChanged(object sender, EventArgs e)
        {
            ToTray();
        }

        private void ToTray()
        {
            this.Hide();
            noti.Visible = true;
            noti.ShowBalloonTip(100);
        }

        private void Noti_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void Noti_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void Noti_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            this.Activate();
            noti.Visible = false;
        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) return;
            if (e.Button == System.Windows.Forms.MouseButtons.Right) RightMouseClick();
        }

        private void Lbl5_MouseClick(object sender, MouseEventArgs e)
        {
            Subform.config con = new Subform.config();
            con.ShowDialog();
            ConfingLoad();
        }

        private void Lbl6_MouseDown(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void drDown(object sender, MouseEventArgs e)
        {
            Control Ctrl = (Control)sender;
            Func.ReleaseCapture();
            Func.SendMessage(Ctrl.Handle, Func.WM_NCLBUTTONDOWN, Func.HT_CAPTION, 0);
        }

        private void Lbl6_MouseClick(object sender, EventArgs e)
        {
            Application.Exit();
            DialogResult a = MessageBox.Show("Close Program","Exit",MessageBoxButtons.OKCancel);
            if (a == DialogResult.OK) Application.Exit();
            return;
        }//End Lbl6_MouseClick

        public void getData()
        {
            try
            {
                using (SqlConnection _con = new SqlConnection(connectionString))
                {
                    _con.Open();
                    using (SqlCommand _cmd = new SqlCommand("SP_GET_POP211_LOADING", _con))
                    {
                        _cmd.CommandType = CommandType.StoredProcedure;
                        _cmd.Parameters.Add(new SqlParameter("@EQCODE", "2pt01"));
                        SqlDataReader reader = _cmd.ExecuteReader();
                        int cnt = 0;
                        while (reader.Read())
                        {
                            cnt++;
                            if (this.Controls["p1"].Controls["SKID_" + reader["REGION"].ToString()] != null && reader["SKID_ID"].ToString() != "")
                            {
                                this.Controls["p1"].Controls["SKID_" + reader["REGION"].ToString()].Visible = true;
                                this.Controls["p1"].Controls["SKID_" + reader["REGION"].ToString()].Controls["lblProd"].Text = reader["PART_NAME"].ToString();
                                this.Controls["p1"].Controls["SKID_" + reader["REGION"].ToString()].Controls["lblModel"].Text = reader["Model"].ToString();
                                this.Controls["p1"].Controls["SKID_" + reader["REGION"].ToString()].Controls["lblColor"].Text = reader["ALC_COLOR"].ToString();
                                this.Controls["p1"].Controls["SKID_" + reader["REGION"].ToString()].Controls["lblSkid"].Text = reader["SKID_ID"].ToString();
                            }
                            if (this.Controls["p1"].Controls["SKID_" + reader["REGION"].ToString()] != null && reader["SKID_ID"].ToString() == "")
                            {
                                this.Controls["p1"].Controls["SKID_" + reader["REGION"].ToString()].Visible = false;
                            }
                            if (this.Controls["line_" + reader["region"].ToString()] != null)
                            {
                                this.Controls["line_" + reader["region"]].Controls["line_skid_id"].Text = reader["SKID_ID"].ToString();
                                this.Controls["line_" + reader["region"]].Controls["line_model"].Text = reader["Model"].ToString();
                                this.Controls["line_" + reader["region"]].Controls["line_alc_color"].Text = reader["ALC_COLOR"].ToString();
                                this.Controls["line_" + reader["region"]].Controls["line_part_name"].Text = reader["ps_desc"].ToString();
                            }

                        }
                        if (this.Controls["lbl7"] != null) this.Controls["lbl7"].Text =  "Updated at "+ DateTime.Now.ToString("hh:mm:ss");
                        _con.Close();
                    }//end Using
                }//end Using
            }
            catch (Exception e)
            {
                MessageBox.Show("Connection Failed.");
            }
        }//end GetData Method

        public void RightMouseClick()
        {
        }
        private void MI_Click(object sender, EventArgs e)
        {
            MenuItem mn = (MenuItem)sender;
            Mn(mn);
        }
        private void Mn(MenuItem mn)
        {
            if (mn.Text.Equals("Exit"))
            {
                DialogResult dr = MessageBox.Show("Do you want to exit?", "Exit", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes) Application.Exit();
                if (dr == DialogResult.No) return;
            }
            else if (mn.Text.Equals("Config"))
            {
                Subform.config con = new Subform.config();
                con.ShowDialog();
                ConfingLoad();
            }
            else if (mn.Text.Equals("Show Border"))
            {
                this.FormBorderStyle = this.FormBorderStyle == FormBorderStyle.None ? FormBorderStyle.Sizable : FormBorderStyle.None;
            }
            else if (mn.Text.Equals("Switch Display"))
            {
                SwitchDisplay();
            }
            else
            {
                return;
            }
        }

        private void SwitchDisplay()
        {
            Screen now = Screen.FromControl(this);
            int index = 0;
            while (!sc[index].Equals(now)) index++;
            int newIndex = index + 1;
            if (newIndex >= sc.Length) newIndex = 0;
            this.Location = sc[newIndex].Bounds.Location;
        }
    }//End Class
}//End Namespace
