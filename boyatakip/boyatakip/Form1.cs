using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using Microsoft.VisualBasic;
using System.IO;

namespace boyatakip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public MySqlConnection mysqlbaglan = new MySqlConnection("Server=sql214.main-hosting.eu;Database=u658845894_boyatakip;Uid=u658845894_boyatakip;Pwd='Boyatakip1108';");
        public MySqlCommand mysqlkomut = new MySqlCommand();//işlemlerimiz içinde bir komut nesnesi
        public DataTable table = new DataTable();//bir adette datatable
        public DataSet dataset = new DataSet();
        public MySqlDataAdapter mysqldataadapter = new MySqlDataAdapter();

        public void listele()//Müşterileri Listeleme fonksiyonu
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox3.Text = "";
            comboBox4.Text = "";
            table.Clear();
            MySqlDataAdapter adtr = new MySqlDataAdapter(" Select * from boyatakip", mysqlbaglan);
            adtr.Fill(table);
            dataGridView1.DataSource = table;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Tarih";
            dataGridView1.Columns[2].HeaderText = "Müşteri Adı";
            dataGridView1.Columns[3].HeaderText = "Boya Türü";
            dataGridView1.Columns[4].HeaderText = "Miktar";
            dataGridView1.Columns[5].HeaderText = "Baz";
            dataGridView1.Columns[6].HeaderText = "Boya Kodu";
            dataGridView1.Columns[7].HeaderText = "Kayıt Yapan";


        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                if (ConnectionState.Closed == mysqlbaglan.State) mysqlbaglan.Open(); //oluşturtuğumuz tanımı çalıştırarak açılmasını sağlıyoruz
                if (mysqlbaglan.State != ConnectionState.Closed) // tanımın durumunu kontrol ediyoruz bağlı mı değil mi
                {
                    MessageBox.Show("Bağlantı Başarılı Bir Şekilde Gerçekleşti"); // bağlı ise buradaki işlemler gerçekleşiyor
                }
                else
                {
                    MessageBox.Show("Maalesef Bağlantı Yapılamadı...!"); // bağlı değilse buradaki işlemler gerçekleşiyor
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Hata! " + err.Message, "Hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            listele();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (ConnectionState.Closed == mysqlbaglan.State) mysqlbaglan.Open();
                
                MySqlCommand ekle = new MySqlCommand("insert into boyatakip (tarih,musteriadi,boyaturu,miktar,baz,boyakodu,kayityapan) values (@tarih, @musteriadi, @boyaturu, @miktar, @baz, @boyakodu, @kayityapan)", mysqlbaglan);
                ekle.Parameters.AddWithValue("@tarih", dateTimePicker1.Value);
                ekle.Parameters.AddWithValue("@musteriadi", textBox1.Text);
                ekle.Parameters.AddWithValue("@boyaturu", comboBox4.Text);
                ekle.Parameters.AddWithValue("@miktar", comboBox1.Text);
                ekle.Parameters.AddWithValue("@baz", comboBox2.Text);
                ekle.Parameters.AddWithValue("@boyakodu", textBox2.Text);
                ekle.Parameters.AddWithValue("@kayityapan", comboBox3.Text);
                mysqlbaglan.Close();
                mysqlbaglan.Open();
                ekle.ExecuteNonQuery();
                MessageBox.Show("Sisteme başarıyla eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
               
            }
            catch (Exception HataYakala)
            {
                MessageBox.Show("Hata: " + HataYakala.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show("Sisteme eklenemedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            listele();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int erisimkodu;
            try
            {
                Random r = new Random();
                int rastgele = r.Next(100, 999);
                erisimkodu = rastgele + 273 * 2 * rastgele + 528 - 13;
                int erisim = Convert.ToInt32(Interaction.InputBox("Erişim Kodu :" + rastgele, "Yetki Kodu", ""));
                if (erisimkodu == erisim)
                {
                    if (ConnectionState.Closed == mysqlbaglan.State) mysqlbaglan.Open();
                    mysqlkomut.Connection = mysqlbaglan;
                    mysqlkomut.CommandText = "DELETE FROM boyatakip WHERE id = '" + dataGridView1.CurrentRow.Cells["id"].Value.ToString() + "'";
                    mysqlkomut.ExecuteNonQuery();
                    MessageBox.Show("Silme İşlemi Başarılı");
                    listele();
                }


            }
            catch (Exception hata)
            {

                MessageBox.Show("Silme İşlemi Başarısız" + hata.ToString() );
            }

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            mysqldataadapter = new MySqlDataAdapter("select * from boyatakip where musteriadi like '" + textBox3.Text + "%'", mysqlbaglan);
            dataset = new DataSet();
            if (ConnectionState.Closed == mysqlbaglan.State) mysqlbaglan.Open();
            mysqldataadapter.Fill(dataset, "boyatakip");
            dataGridView1.DataSource = dataset.Tables["boyatakip"];
            mysqlbaglan.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            DateTime dt = Convert.ToDateTime(dateTimePicker2.Value);
            DateTime dt2 = Convert.ToDateTime(dateTimePicker3.Value);
            try
            {
                mysqldataadapter = new MySqlDataAdapter("select * from boyatakip where tarih between @tarih1 and @tarih2 and musteriadi like '" + textBox3.Text + "%' ", mysqlbaglan);
                mysqldataadapter.SelectCommand.Parameters.AddWithValue("@tarih1", dt);
                mysqldataadapter.SelectCommand.Parameters.AddWithValue("@tarih2", dt2);
                dataset = new DataSet();
                if (ConnectionState.Closed == mysqlbaglan.State) mysqlbaglan.Open();
                mysqldataadapter.Fill(dataset, "boyatakip");
                dataGridView1.DataSource = dataset.Tables["boyatakip"];
                mysqlbaglan.Close();


            }
            catch (Exception sa)
            {
                MessageBox.Show("Program Sağlayıcınızla Görüşün" + sa.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listele();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bu program Bir Söve Yalıtım tarafından yaptırılmıştır. www.birsove.com.tr 0534 300 48 82", "Hakkında", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MessageBox.Show("Programın kullanılması ücretsizdir. Çoğaltılabilir, dağıtılabilir ve geliştirilebilir.", "Hakkında", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
