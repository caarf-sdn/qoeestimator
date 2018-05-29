using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace UDP_APP
{
    class Program
    {
        private static string randomPayload(int size)
        {
            //Cria string que será utilizada como payload pelo agente
            StringBuilder builder = new StringBuilder();
            char ch= '#';
            for (int i = 0; i < size; i++)
            {
                builder.Append(ch);
            }

            return builder.ToString();
        }

        static void handleUDP1(IPEndPoint remoteAgent, int size, int pNumber, string CAARFIP, string CAARFPort, UdpClient agent) 
        {
            //Envia mensagem AGENT_UDP1 
            string payload;
            byte[] wirepayload;
            double[] RTT = new double[pNumber];
            double[] DIFF = new double[pNumber];
            int packetCount = 0;
            double sumDIFF = 0;
            double sumRTT = 0;
            double JITTER;
            double ALATENCY;
            double LOSS;
            double ELATENCY;
            double R;
            double MOS;

            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Stopwatch stopWatch = new Stopwatch();
            TimeSpan elapsedTime;
            UdpClient temp = new UdpClient();

            //Connect() vai garantir que sejam recebidos apenas pacotes do agente remoto
            temp.Connect(remoteAgent);

            if (size == 0)
            {
                //Payload: 12 bytes de cabeçalho RTP + 160 bytes de G.711 payload
                payload = "AGENTE_UDP1|######################################################################################################################################################";
                wirepayload = Encoding.ASCII.GetBytes(payload);
            }
            else
            {
                //Random Payload
                payload = "AGENTE_UDP1|" + randomPayload(size);
                wirepayload = Encoding.ASCII.GetBytes(payload);
            }

            IAsyncResult asyncResult;

            for (int i = 0; i < pNumber; i++)
            {
                stopWatch.Start();
                temp.Send(wirepayload, wirepayload.Length);
                asyncResult = temp.BeginReceive(null, null);
                asyncResult.AsyncWaitHandle.WaitOne(2000);
                if (asyncResult.IsCompleted)
                {
                    elapsedTime = stopWatch.Elapsed;
                    RTT[i] = elapsedTime.TotalMilliseconds;
                    if (i==0)
                    {
                        DIFF[i] = 0;
                    }
                    else
                    {
                        DIFF[i] = Math.Abs(RTT[i] - RTT[i - 1]);
                    }
                    sumDIFF = sumDIFF + DIFF[i];
                    sumRTT = sumRTT + RTT[i];
                    packetCount++;
                }
                stopWatch.Reset();
            }
            JITTER = sumDIFF / (pNumber - 1);
            ALATENCY = sumRTT / pNumber;
            LOSS = ((pNumber-packetCount) * 100) / pNumber;

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
            MOS = 1 + 0.035 * R + 0.000007 * R * (R - 60) * (100 - R);


            //TODO: CAARF_RESPONSE
            IPEndPoint CAARF = new IPEndPoint(IPAddress.Parse(CAARFIP), Int32.Parse(CAARFPort));
            payload = "CAARF_RESPONSE" + "|" + R + "|" + MOS;
            wirepayload = Encoding.ASCII.GetBytes(payload);
            agent.Send(wirepayload, wirepayload.Length, CAARF);

            //CONSOLE OUTPUT
            for (int i = 0; i < pNumber; i++)
            {
                Console.WriteLine("{0}\t{1}", RTT[i], DIFF[i]);
            }
            Console.WriteLine("Pacotes = {0}", packetCount);
            Console.WriteLine("Latencia media = {0}", ALATENCY);
            Console.WriteLine("Jitter = {0}", JITTER);
            Console.WriteLine("Perda de pacotes = {0} %", LOSS);
            Console.WriteLine("R-factor= {0}", R);
            Console.WriteLine("MOS = {0}", MOS);

            temp.Close();
        }


        static void sendSIGNAL1(IPEndPoint remote, UdpClient agent, int size, int qtd, IPEndPoint CAARF)
        {
            //Envia mensagem AGENT_SIGNAL1

            //string payload = "AGENT_SIGNAL1|" + size + "|"+ qtd;
            string payload = "AGENT_SIGNAL1|" + size + "|" + qtd + "|" + CAARF.Address.ToString() + "|" + CAARF.Port.ToString();
            byte[] wirepayload = Encoding.ASCII.GetBytes(payload);
            agent.Send(wirepayload, wirepayload.Length, remote);

        }

        static void sendSIGNAL2(IPEndPoint remote, UdpClient agent, int size, int qtd, string CAARFIP, string CAARFPort)
        {
            //Envia mensagem AGENT_SIGNAL2
            //string payload = "AGENT_SIGNAL2|" + size + "|" + qtd;
            string payload = "AGENT_SIGNAL2|" + size + "|" + qtd + "|" + CAARFIP + "|" + CAARFPort;
            byte[] wirepayload = Encoding.ASCII.GetBytes(payload);
            agent.Send(wirepayload, wirepayload.Length, remote);
        }

        static void teste(string[] payloadMessage, UdpClient agent, IPEndPoint remote)
        {


            double JITTER;
            double ALATENCY;
            double LOSS;
            double R;
            double MOS;
            double ELATENCY;

            ALATENCY = Double.Parse(payloadMessage[1]);
            JITTER = Double.Parse(payloadMessage[2]);
            LOSS = Double.Parse(payloadMessage[3]);

            ELATENCY = ALATENCY + JITTER * 2 + 10;
            if (ELATENCY < 160)
            {
                R = 93.2 - (ELATENCY / 40);
            }
            else
            {
                R = 93.2 - (ELATENCY - 120) / 10;
            }
            R = R - ((LOSS/100) * 2.5);
            MOS = 1 + 0.035 * R + 0.000007 * R * (R - 60) * (100 - R);

            string payload = R + "|" + MOS;
            byte[] wirepayload = Encoding.ASCII.GetBytes(payload);
            agent.Send(wirepayload, wirepayload.Length, remote);

            //CONSOLE OUTPUT
            Console.WriteLine("Latencia media = {0}", ALATENCY);
            Console.WriteLine("Jitter = {0}", JITTER);
            Console.WriteLine("Perda de pacotes = {0} %", LOSS);
            Console.WriteLine("Effective latency = {0}", ELATENCY);
            Console.WriteLine("R-factor = {0}", R);
            Console.WriteLine("MOS = {0}", MOS);
        }

        static void Main(string[] args)
        {
            //AGENTE_UDP

            try
            {
                //LOCAL HOST INFO
                Console.WriteLine("AGENTE_UDP");
                string hostName = Dns.GetHostName();

                IPHostEntry hostInfo = Dns.GetHostEntry(hostName);

                Console.WriteLine("Nome do host: {0}", hostInfo.HostName);
                Console.WriteLine("Endereços IP:");
                foreach (IPAddress ipaddr in hostInfo.AddressList)
                {
                    Console.WriteLine(ipaddr.ToString());
                }
                Console.WriteLine();
                Console.Write("PORTA: ");
            }
            catch (Exception)
            {
                Console.WriteLine("Não foi possivel resolver o host local...");
            }

            int port_number = 5060;

            port_number = Int32.Parse(Console.ReadLine());


            //Cria uma instância de UdpClient que escuta na porta
            UdpClient agent = null;
            try
            {
                agent = new UdpClient(port_number);        

            }
            catch (SocketException se)
            {
                Console.WriteLine(se.ErrorCode + ": " + se.Message);
                Environment.Exit(se.ErrorCode);
            }

            //IPEndPoint que vai armazenar informações do IPEndPoint remoto
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);

            string  payload;
            char    delimiter = '|';
            int     size;
            int     qtd;


            Console.WriteLine("ESCUTANDO EM PORTA {0}...", port_number);
            while (true)
            {
                try
                {
                    byte[] wirePayload = agent.Receive(ref remoteIPEndPoint);

                    payload = Encoding.ASCII.GetString(wirePayload, 0, wirePayload.Length);
                    string[] payloadMessage = payload.Split(delimiter);
                   
                    switch (payloadMessage[0])
                    {
                        case "CAARF_REQUEST":
                            //Caso receba receba uma mensagem de requisição de QOE - CAARF_REQUEST
                            Console.WriteLine("{1} - RECEBEU REQUISICAO: {0} - {2} pacotes - {3} bytes...", remoteIPEndPoint, DateTime.Now, payloadMessage[4], payloadMessage[3]);
                            //Console.WriteLine("AGENTE_1: {0} AGENTE_2: {1}", )

                            IPEndPoint remote = new IPEndPoint(IPAddress.Parse(payloadMessage[1]),Int32.Parse(payloadMessage[2]));
                            size = Int32.Parse(payloadMessage[3]);
                            qtd = Int32.Parse(payloadMessage[4]);
                            sendSIGNAL1(remote, agent, size, qtd, remoteIPEndPoint);
                            break;
                        case "AGENT_SIGNAL1":
                            //Caso receba uma mensagem de sinal - AGENT_SIGNAL1
                            size = Int32.Parse(payloadMessage[1]);
                            qtd = Int32.Parse(payloadMessage[2]);
                            sendSIGNAL2(remoteIPEndPoint, agent, size, qtd, payloadMessage[3], payloadMessage[4]);
                            break;
                        case "AGENT_SIGNAL2":
                            //Caso receba uma mensagem de sinal - AGENT_SIGNAL2 - Enviar/Receber pacotes AGENT_UDP1
                            size = Int32.Parse(payloadMessage[1]);
                            qtd = Int32.Parse(payloadMessage[2]);

                            handleUDP1(remoteIPEndPoint, size, qtd, payloadMessage[3], payloadMessage[4], agent);
                            break;
                        case "AGENTE_UDP1":
                            //Caso receba mensagem AGENT_UDP1 - Devolve o pacote ao agente remoto
                            agent.Send(wirePayload, wirePayload.Length, remoteIPEndPoint);
                            break;
                        case "TESTE":
                            //Caso receba mensagem de teste com inputs manuais
                            Console.WriteLine("{1} - RECEBEU TESTE: {0} - {2} latencia media - {3} jitter - {4} perda de pacotes...", remoteIPEndPoint, DateTime.Now, payloadMessage[1], payloadMessage[2], payloadMessage[3]);
                            teste(payloadMessage, agent, remoteIPEndPoint);
                            break;

                        default:
                            Console.WriteLine("MENSAGEM EM FORMATO INCORRETO...");
                            break;
                    }

                }
                catch (SocketException se)
                {
                    Console.WriteLine(se.ErrorCode + ": " + se.Message);
                }
            }
        }
    }
}
