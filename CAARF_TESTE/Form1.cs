using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace CAARF_TESTE
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                UdpClient CAARF = new UdpClient();
                IPEndPoint remoteAgent = new IPEndPoint(IPAddress.Parse(textBox1.Text), Int32.Parse(textBox3.Text));

                string payload = "CAARF_REQUEST|" + textBox2.Text + "|" + textBox4.Text + "|" + textBox6.Text + "|" + textBox5.Text;
                byte[] wirepayload = Encoding.ASCII.GetBytes(payload);
                CAARF.Send(wirepayload, wirepayload.Length, remoteAgent);

                CAARF.Client.ReceiveTimeout = 5000;
                wirepayload = CAARF.Receive(ref remoteAgent);
                payload = Encoding.ASCII.GetString(wirepayload, 0, wirepayload.Length);
                string[] payloadMessage = payload.Split('|');

                textBoxTeste.AppendText("RESPOSTA-> R:" + payloadMessage[1] + "\t MOS:" + payloadMessage[2] + "\n");
            }
            catch (Exception ex)
            {
                textBoxTeste.Text= ex.Message ;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                UdpClient CAARF = new UdpClient();
                IPEndPoint remoteAgent = new IPEndPoint(IPAddress.Parse(textBox1.Text), Int32.Parse(textBox3.Text));
                char delimiter = '|';
                string payload = "AGENT_TEST|" + textBoxAL.Text + "|" + textBoxJITTER.Text + "|" + textBoxLOSS.Text;
                byte[] wirepayload = Encoding.ASCII.GetBytes(payload);
                CAARF.Send(wirepayload, wirepayload.Length, remoteAgent);

                CAARF.Client.ReceiveTimeout = 5000;
                wirepayload = CAARF.Receive(ref remoteAgent);
                payload = Encoding.ASCII.GetString(wirepayload, 0, wirepayload.Length);
                string[] payloadMessage = payload.Split(delimiter);

                textBoxTeste.AppendText("TESTE-> R-Factor:" + payloadMessage[0] + "\t MOS:" + payloadMessage[1] + "\n");
            }
            catch (Exception ex)
            {
                textBoxTeste.Text= ex.Message;
            }
        }
    }
}
