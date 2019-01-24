using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace name_sorter
{
    public partial class name_sorter : Form
    {     
        #region FUNCTION
        private void loadFile(string FILE)
        {
            System.IO.StreamReader file = new System.IO.StreamReader(FILE + ".txt");
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            
            string newline;
            while ((newline = file.ReadLine()) != null)
            {                
                dt.Rows.Add(newline);
            }
            file.Close();
            DGV_UNSORTED.DataSource = dt;
        }

        private void processFile(string FILE)
        {
            System.IO.StreamReader file = new System.IO.StreamReader(FILE + ".txt");
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");

            string newline;
            while ((newline = file.ReadLine()) != null)
            {
                if (newline.Contains(" ") == true)
                {
                    dt.Rows.Add(newline);
                }
                else
                { dt.Rows.Add( " " + newline); }
            }
            file.Close();
            if (RB_FIRST.Checked == true)
            {
                var newDataTable = dt.AsEnumerable()
                   .OrderBy(r => r.Field<String>("Name"))
                   .CopyToDataTable();
                DGV_SORTED.DataSource = newDataTable;
            }
            else
            {
                var newDataTable = dt.AsEnumerable()
                   .OrderBy(r => r.Field<String>("Name").Substring(r.Field<String>("Name").ToString().LastIndexOf(" "), r.Field<String>("Name").Length - r.Field<String>("Name").ToString().LastIndexOf(" ")))
                   .CopyToDataTable();
                DGV_SORTED.DataSource = newDataTable;
            }

            ExportUserData();
        }

        private async Task ExportUserData()
        {
            TextWriter writer = new StreamWriter("sorted-names-list.txt");

            for (int i = 0; i < DGV_SORTED.Rows.Count; i++)
            {
                for (int j = 0; j < DGV_SORTED.Columns.Count; j++)
                {
                    await writer.WriteAsync(DGV_SORTED.Rows[i].Cells[j].Value.ToString());
                }

                await writer.WriteLineAsync("");
            }

            writer.Close();
        }
        #endregion

        public name_sorter()
        {
            InitializeComponent();
        }

        private void name_sorter_Load(object sender, EventArgs e)
        {
            this.TransparencyKey = this.BackColor;
            loadFile("unsorted-names-list");
        }

        private void BTN_MIN_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void BTN_CLOSE_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BTN_HOW_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@"doc.pdf");
            }
            catch
            { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("notepad.exe", "unsorted-names-list.txt");
            }
            catch
            { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("notepad.exe", "sorted-names-list.txt");
            }
            catch
            { }
        }

        private void BTN_SORT_Click(object sender, EventArgs e)
        {
            processFile("unsorted-names-list");
        }

    }
}
