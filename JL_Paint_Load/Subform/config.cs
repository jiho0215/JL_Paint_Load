using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JL_Paint_Load.Subform
{
    public partial class config : Form
    {
        CheckBox change = new CheckBox();
        Button connTest = new Button();
        Button save = new Button();
        Label message = new Label();
        List<string> settings = new List<string>() { "Cycle Time", "MES_SERVER", "D/B NAME", "D/B USER", "D/B PW" };
        ComboBox Combo_mesdb_server = new ComboBox();
        
        public config()
        {
            this.Icon = Properties.Resources._1445304131_Halloween;
            InitializeComponent();
            InitialDisplay();
            Load();
        }

        public void Load()
        {
            //string connectionString = "Data Source = " + Func.GetEntryValue("MESDB", "MESDB_SERVER") + "; Initial Catalog = " + Func.GetEntryValue("MESDB", "MESDB_NAME") + "; User ID = " + Func.GetEntryValue("MESDB", "MESDB_USER") + "; Password = " + Func.GetEntryValue("MESDB", "MESDB_PSWD") + "";

            this.Controls["MES_SERVER"].Text = Func.GetEntryValue("MESDB", "MESDB_SERVER");
            this.Controls["D/B NAME"].Text = Func.GetEntryValue("MESDB", "MESDB_NAME");
            this.Controls["D/B USER"].Text = Func.GetEntryValue("MESDB", "MESDB_USER");
            this.Controls["D/B PW"].Text = Func.GetEntryValue("MESDB", "MESDB_PSWD");
        }

        public void InitialDisplay()
        {
            this.Size = new Size(500, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.LightBlue;

            settings.ForEach(x =>
            {
                Label a = new Label();
                a.Text = x;
                a.Size = new Size(150, 30);
                a.Location = new Point(0, 0 + (30 * settings.IndexOf(x)));
                a.Parent = this;

                TextBox b = new TextBox();
                b.Size = a.Size;
                b.Location = new Point(a.Right + 5, a.Top);
                b.BackColor = Color.White;
                b.BorderStyle = BorderStyle.Fixed3D;
                b.Enabled = false;
                b.Name = x;
                b.Parent = this;
            }
            );
            Controls["mes_server"].Visible = false;
            Combo_mesdb_server.Size = Controls["MES_SERVER"].Size;
            Combo_mesdb_server.Location = Controls["MES_SERVER"].Location;
            Combo_mesdb_server.BackColor = Color.White;
            Combo_mesdb_server.Enabled = false;
            Combo_mesdb_server.Name = "Combo_MES_SERVER";
            Combo_mesdb_server.BringToFront();
            Combo_mesdb_server.Parent = this;
            //StringBuilder DB_List = new StringBuilder();
            //DB_List.Insert(0,Func.GetEntryValue("COMBO_MES_SERVER", "DB_LIST"));
            //MessageBox.Show(DB_List+"");
            string DB_List = Func.GetEntryValue("Combo_MES_SERVER", "DB_List");
            while (DB_List != "")
            {
                Combo_mesdb_server.Items.Add(DB_List.Substring(0, DB_List.IndexOf(";")));
                DB_List = DB_List.Substring(DB_List.IndexOf(";") + 1);
            }
            Combo_mesdb_server.SelectedIndex = 0;
            Combo_mesdb_server.SelectedIndexChanged += Combo_mesdb_server_SelectedIndexChanged;

            change.Size = new Size(80, 20);
            change.Location = new Point(400, 10);
            change.Text = "Change";
            change.Name = "change";
            change.Parent = this;
            change.CheckedChanged += Change_CheckedChanged;
            change.Checked = false;

            connTest.Size = new Size(80, 20);
            connTest.Location = new Point(400, 50);
            connTest.Text = "Test";
            connTest.Name = "connTest";
            connTest.Parent = this;
            connTest.MouseClick += ConnTest_MouseClick;

            save.Size = new Size(80, 20);
            save.Location = new Point(400, 90);
            save.Text = "Save";
            save.Name = "save";
            save.Parent = this;
            save.MouseClick += Save_MouseClick;

            message.Size = new Size(300, 40);
            message.Location = new Point(10, this.Bottom - message.Height * 2);
            message.BorderStyle = BorderStyle.Fixed3D;
            message.BackColor = Color.White;
            message.Name = "message";
            message.Parent = this;
        }

        private void Combo_mesdb_server_SelectedIndexChanged(object sender, EventArgs e)
        {
            string temp = Combo_mesdb_server.Text;
            string value = Func.GetEntryValue("combo_mes_server", temp);

            this.Controls["D/B NAME"].Text = "";
            this.Controls["D/B USER"].Text = "";
            this.Controls["D/B PW"].Text = "";

            if(value.Length>3)
            {
                if (value.IndexOf(";") > 0) this.Controls["D/B NAME"].Text = value.Substring(0, value.IndexOf(";")); value = value.Substring(value.IndexOf(";") + 1);
                if (value.IndexOf(";") > 0) this.Controls["D/B USER"].Text = value.Substring(0, value.IndexOf(";")); value = value.Substring(value.IndexOf(";") + 1);
                if (value.IndexOf(";") > 0) this.Controls["D/B PW"].Text = value.Substring(0, value.IndexOf(";"));
            }            
        }

        private void Save_MouseClick(object sender, MouseEventArgs e)
        {
            string connectionString = "Data Source = " + this.Controls["combo_MES_SERVER"].Text + "; Initial Catalog = " + this.Controls["D/B NAME"].Text + "; User ID = " + this.Controls["D/B USER"].Text + "; Password = " + this.Controls["D/B PW"].Text + "";

            if (Func.checkConn(connectionString))
            {
                //SetDBConn();
                Func.SetIniValue("MESDB", "MESDB_SERVER", this.Controls["combo_MES_SERVER"].Text);
                Func.SetIniValue("MESDB", "MESDB_NAME", this.Controls["D/B NAME"].Text);
                Func.SetIniValue("MESDB", "MESDB_USER", this.Controls["D/B USER"].Text);
                Func.SetIniValue("MESDB", "MESDB_PSWD", this.Controls["D/B PW"].Text);

                //SaveConfig();

                Func.SetIniValue("COMBO_MES_SERVER", Controls["combo_MES_SERVER"].Text, Controls["D/B NAME"].Text + ";" + Controls["D/B USER"].Text + ";" + Controls["D/B PW"].Text + ";");
                this.Combo_mesdb_server.Items.Add(Combo_mesdb_server.Text);


                //DB_LIST SAVE
                string temp = Func.GetEntryValue("COMBO_MES_SERVER", "DB_List");

                //Check if Exist
                if (temp.IndexOf(Combo_mesdb_server.Text) < 0)
                {
                    temp = Combo_mesdb_server.Text + ";" + temp;
                    Func.SetIniValue("COMBO_MES_SERVER", "DB_List", temp);
                }
                else
                {
                    int i = temp.IndexOf(Combo_mesdb_server.Text);
                    temp = Combo_mesdb_server.Text + ";" + temp.Substring(0, i) + temp.Substring(i + Combo_mesdb_server.Text.Length + 1);
                    Func.SetIniValue("Combo_MES_SERVER", "DB_List", temp);
                }
                MessageBox.Show("Successfully Saved. ");
                this.Close();
            }
            else
            {
                MessageBox.Show("Please Input valid DB Information.");
            }
        }

        private void ConnTest_MouseClick(object sender, MouseEventArgs e)
        {
            //Conn Test
            if (IsIpV4AddressValid(this.Controls["combo_MES_SERVER"].Text))
            {
                string connectionString = "Data Source = " + this.Controls["combo_MES_SERVER"].Text + "; Initial Catalog = " + this.Controls["D/B NAME"].Text + "; User ID = " + this.Controls["D/B USER"].Text + "; Password = " + this.Controls["D/B PW"].Text + "";
                Controls["message"].Text = Func.checkConn(connectionString) ? "Conn Succeed." : "Conn Failed.";

            }
            else
            {
                Controls["message"].Text = "\"" + this.Controls["combo_MES_SERVER"].Text + "\" is not a valid IP.";
            }
        }

        private void Change_CheckedChanged(object sender, EventArgs e)
        {
            if (change.Checked) { settings.ForEach(x => Controls[x].Enabled = true); Controls["Combo_MES_SERVER"].Enabled = true; }
            else { settings.ForEach(x =>  Controls[x].Enabled = false); Controls["Combo_MES_SERVER"].Enabled = false; }
        }

        private static readonly Regex validIpV4AddressRegex = new Regex(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$", RegexOptions.IgnoreCase);

        public static bool IsIpV4AddressValid(string address)
        {
            if (!string.IsNullOrWhiteSpace(address))
            {
                return validIpV4AddressRegex.IsMatch(address.Trim());
            }

            return false;
        }

    }
}
