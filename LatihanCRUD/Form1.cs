using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace LatihanCRUD
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        String id = "0";
        String klik = "" ;
        OleDbConnection koneksi;
        OleDbCommand oleDbCmd = new OleDbCommand();
        String connParam = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\Pendidikan\Pemrograman Visual\simpel-crud-csharp\Database\db.mdb;Persist Security Info=False";


        public void offForm()
        {
            txtNama.Enabled = false;
            txtAlamat.Enabled = false;
            txtTelp.Enabled = false;
            radPutra.Enabled = false;
            radPutri.Enabled = false;
            metroButton1.Enabled = false;
            metroButton2.Enabled = false;
        }

        public void onForm()
        {
            txtNama.Enabled = true;
            txtAlamat.Enabled = true;
            txtTelp.Enabled = true;
            radPutra.Enabled = true;
            radPutri.Enabled = true;
            metroButton1.Enabled = true;
            metroButton2.Enabled = true;
        }

        public void loadData()
        {
            gridPegawai.DataSource = null;
            gridPegawai.Rows.Clear();
            gridPegawai.Refresh();

            OleDbDataAdapter adpPegawai = new OleDbDataAdapter("SELECT * FROM siswa", connParam);

            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            ds.Clear();

            adpPegawai.Fill(ds);

            gridPegawai.DataSource = ds.Tables[0];

            adpPegawai.Dispose();

        }

        public void refreshForm()
        {
            txtNama.Text = "";
            txtAlamat.Text = "";
            txtTelp.Text = "";
            radPutra.Checked = false;
            radPutri.Checked = false;
        }

        public Form1()
        {
            koneksi = new OleDbConnection(connParam);
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            offForm();
            loadData();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            String nama = txtNama.Text;
            String alamat = txtAlamat.Text;
            String telp = txtTelp.Text;
            bool putra = radPutra.Checked;
            bool putri = radPutri.Checked;
            String jk;

            if (putra)
            {
                jk = "Putra";
            } else
            {
                jk = "Putri";
            }

            String sql = "";

            if (klik == "tambah")
            {
                sql = "insert into siswa (sisnama, sisalamat, sisnotelp, sisjeniskelamin) values ('" + nama + "','" + alamat + "','" + telp + "','" + jk + "')";
            } else
            {
                sql = "update siswa set sisnama ='" + nama + "' , sisalamat ='" + alamat + "' , sisnotelp ='" + telp + "' , sisjeniskelamin = '" + jk+"' where sisid=" + id;
            }

            koneksi.Open();
            oleDbCmd.Connection = koneksi;
            oleDbCmd.CommandText = sql;
            int temp = oleDbCmd.ExecuteNonQuery();
            if (temp > 0)
            {
                koneksi.Close();
                refreshForm();
                offForm();
                loadData();
                MessageBox.Show("Berhasil tambah data.");
            }
            else
            {
                koneksi.Close();
                MessageBox.Show("Gagal menambah Data");
            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            refreshForm();
        }

        private void gridPegawai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in gridPegawai.SelectedRows)
            {
                id = row.Cells[0].Value.ToString();
                txtNama.Text = row.Cells[1].Value.ToString();
                txtAlamat.Text = row.Cells[2].Value.ToString();
                txtTelp.Text = row.Cells[3].Value.ToString();
                if (row.Cells[4].Value.ToString() == "Putra")
                {
                    radPutra.Checked = true;
                } else {
                    radPutri.Checked = true;
                }
            } 
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            if (id != "0")
            {
                String sql = "delete from siswa where sisid = " + id;

                koneksi.Open();
                oleDbCmd.Connection = koneksi;
                oleDbCmd.CommandText = sql;
                int temp = oleDbCmd.ExecuteNonQuery();
                koneksi.Close();
                if (temp > 0)
                {
                    refreshForm();
                    loadData();
                    offForm();
                    MessageBox.Show("Berhasil hapus data.");
                }
                else
                {
                    MessageBox.Show("Gagal hapus Data");
                }
            }
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            klik = "tambah";
            refreshForm();
            onForm();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            klik = "ubah";
            onForm();
        }
    }
}
