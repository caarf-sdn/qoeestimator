using System;
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
        private string QOEREQ()
        {
            //Envia e recebe mensagem de QoE
            UdpClient CAARF = new UdpClient();
            IPEndPoint remoteAgent = new IPEndPoint(IPAddress.Parse(textBoxAgentIp.Text), Int16.Parse(textBoxAgentPort.Text));

            string payload = "CAARF_REQUEST|" + textBoxRip.Text + "|" + textBoxRport.Text + "|" + textBoxRsize.Text + "|" + textBoxRqt.Text;
            byte[] wirepayload = Encoding.ASCII.GetBytes(payload);
            CAARF.Send(wirepayload, wirepayload.Length, remoteAgent);

            wirepayload = CAARF.Receive(ref remoteAgent);
            payload = Encoding.ASCII.GetString(wirepayload, 0, wirepayload.Length);

            CAARF.Close();
            return payload;

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Task<string> task = new Task<string>(QOEREQ);
                task.Start();

                string payload = await task;
                string[] payloadMessage = payload.Split('|');
                textBoxOutput.AppendText("RESPOSTA-> R:" + payloadMessage[1] + "\t MOS:" + payloadMessage[2] + "\n");
            }
            catch (Exception ex)
            {
                textBoxOutput.AppendText(ex.Message + "\n");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                double JITTER;
                double ALATENCY;
                double LOSS;
                double R;
                double MOS;
                double ELATENCY;

                ALATENCY=   Double.Parse(textBoxTlat.Text);
                JITTER =    Double.Parse(textBoxTjit.Text);
                LOSS =      Double.Parse(textBoxTloss.Text);

                ELATENCY = ALATENCY + JITTER * 2 + 10;
                if (ELATENCY < 160)
                {
                    R = 93.2 - (ELATENCY / 40);
                }
                else
                {
                    R = 93.2 - (ELATENCY - 120) / 10;
                }

                R = R - (LOSS * 2.5);
                if (R < 0)
                {
                    MOS = 1;
                }
                else if (R > 100)
                {
                    MOS = 4.5;
                }
                else
                {
                    MOS = 1 + 0.035 * R + 0.000007 * R * (R - 60) * (100 - R);
                }

                textBoxOutput.AppendText("TESTE-> R-Factor:" + R + "\t MOS:" + MOS + "\n");
            }
            catch (Exception ex)
            {
                textBoxOutput.AppendText(ex.Message+"\n");
            }
        }
    }
}
