using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;

namespace chat
{
    public partial class Form1 : Form
    {
        const int portNo = 500;
        TcpClient client;
        byte[] data;

        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            if (btnSignIn.Text == "Sign In")
            {
                try
                {
                    // Menghubungkan ke server
                    client = new TcpClient();
                    client.Connect(IPServer.Text, portNo);
                    data = new byte[client.ReceiveBufferSize];

                    // Membaca Server
                    SendMessage(txtNick.Text);
                    client.GetStream().BeginRead(data, 0, System.Convert.ToInt32(client.ReceiveBufferSize), ReceiveMessage, null);

                    btnSignIn.Text = "Sign Out";
                    btnSend.Enabled = true;
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                // Disconnect dari server
                Disconnect();
                btnSignIn.Text = "Sign In";
                btnSend.Enabled = false;
            }
        }

        private void txtNick_TextChanged(object sender, EventArgs e)
        {

        }

        private void IPServer_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtMessageHistory_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessage(txtMessage.Text);
            txtMessage.Clear();
        }

        public void SendMessage(string message)
        {
            try
            {
                // Mengirim pesan ke server

                NetworkStream ns = client.GetStream();
                byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                ns.Write(data, 0, data.Length);
                ns.Flush();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void ReceiveMessage(IAsyncResult ar)
        {
            try
            {
                int bytesRead;

                // Membaca data dari server
                bytesRead = client.GetStream().EndRead(ar);

                if (bytesRead < 1)
                {
                    return;
                }
                else
                {
                    // Memanggil delegate untuk menampilkan data yang diterima
                    object[] para = { System.Text.Encoding.ASCII.GetString(data, 0, bytesRead) };
                    this.Invoke(new delUpdateHistory(UpdateHistory), para);
                }

                // Melanjutkan membaca
                client.GetStream().BeginRead(data, 0, System.Convert.ToInt32(client.ReceiveBufferSize), ReceiveMessage, null);
            }

            catch (Exception ex)
            {
                
            }
        }

        public delegate void delUpdateHistory(string str);

        public void UpdateHistory(string str)
        {
            txtMessageHistory.AppendText(str);
        }

        public void Disconnect()
        {
            try
            {
                // Disconnect server

                client.GetStream().Close();
                client.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            Disconnect();
        }
    }
}
