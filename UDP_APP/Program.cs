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
            //Envia e recebe mensagems AGENT_UDP12
            //Resultado da estimação de QOE é enviado em mensagem de resposta CAARF_RESPONSE
            string payload;
            byte[] wirepayload;
            int packetCount = 0;
            double sumDIFF = 0;
            double sumRTT = 0;
            double JITTER;
            double ALATENCY;
            double LOSS;
            double ELATENCY;
            double R;
            double MOS;           
            double last = -1;
            double d = 0;

            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Stopwatch stopWatch = new Stopwatch();
            TimeSpan elapsedTime;
            UdpClient temp = new UdpClient();

            //Connect() vai garantir que sejam recebidos apenas pacotes do agente remoto
            temp.Connect(remoteAgent);

            if (size == 0)
            {
                //Payload: 12 bytes de cabeçalho RTP + 160 bytes de G.711 payload
                payload = "AGENT_UDP12|######################################################################################################################################################";
                wirepayload = Encoding.ASCII.GetBytes(payload);
            }
            else
            {
                //Random Payload
                payload = "AGENT_UDP12|" + randomPayload(size);
                wirepayload = Encoding.ASCII.GetBytes(payload);
            }

            IAsyncResult asyncResult;
            int wireLength = wirepayload.Length;
            double time;
            for (int i = 0; i < pNumber; i++)
            {
                stopWatch.Start();
                temp.Send(wirepayload, wireLength);
                asyncResult = temp.BeginReceive(null, null);
                asyncResult.AsyncWaitHandle.WaitOne(2000);

                time = stopWatch.Elapsed.TotalMilliseconds;
                if ((asyncResult.IsCompleted) && (time < 2000))
                {
                    elapsedTime = stopWatch.Elapsed;
                    Console.WriteLine(elapsedTime.TotalMilliseconds+"ms");
                    if (i==0)
                    {
                        d = 0;
                    }
                    else
                    {
                        if (last != -1)
                        {
                            d = Math.Abs(elapsedTime.TotalMilliseconds - last);
                        }
                        else
                        {
                            d = 0;
                        }

                    }
                    last = elapsedTime.TotalMilliseconds;
                    sumDIFF = sumDIFF + d;
                    sumRTT = sumRTT + elapsedTime.TotalMilliseconds;
                    packetCount++;
                }
                else
                {
                    //Caso o limite de tempo seja ultrapassado
                    Console.WriteLine("Esgotado tempo limite.");
                }
                stopWatch.Reset();
            }

            //Caso a quantidade de pacotes recebidos seja maior que zero
            if(packetCount > 0) 
            {
                if (packetCount == 1)
                {
                    JITTER = sumDIFF;
                }
                else
                {
                    JITTER = sumDIFF / (packetCount - 1);
                }

                ALATENCY = sumRTT / packetCount;
                LOSS = ((pNumber - packetCount) * 100) / pNumber;
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
            }
            else
            {
                //Caso todos pacotes sejam perdidos
                LOSS = ((pNumber - packetCount) * 100) / pNumber;
                JITTER = -1;
                ALATENCY = -1;
                ELATENCY = -1;
                R = 0;
                MOS = 0;
            }

            //RESPOSTA: CAARF_RESPONSE
            IPEndPoint CAARF = new IPEndPoint(IPAddress.Parse(CAARFIP), Int32.Parse(CAARFPort));
            payload = "CAARF_RESPONSE" + "|" + R + "|" + MOS;
            wirepayload = Encoding.ASCII.GetBytes(payload);
            agent.Send(wirepayload, wirepayload.Length, CAARF);

            //CONSOLE OUTPUT
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("Pacotes recebidos = {0}", packetCount);
            Console.WriteLine("Latência média = {0}", ALATENCY);
            Console.WriteLine("Jitter = {0}", JITTER);
            Console.WriteLine("Perda de pacotes = {0} %", LOSS);
            Console.WriteLine("Latência Efetiva = {0}", ELATENCY);
            Console.WriteLine("R-factor= {0}", R);
            Console.WriteLine("MOS = {0}\n", MOS);


            temp.Close();
        }


        static void sendSIGNAL1(IPEndPoint remote, UdpClient agent, int size, int qtd, IPEndPoint CAARF)
        {
            //Envia mensagem de sinalização AGENT_SIGNAL1

            string payload = "AGENT_SIGNAL1|" + size + "|" + qtd + "|" + CAARF.Address.ToString() + "|" + CAARF.Port.ToString();
            byte[] wirepayload = Encoding.ASCII.GetBytes(payload);
            agent.Send(wirepayload, wirepayload.Length, remote);

        }

        static void sendSIGNAL2(IPEndPoint remote, UdpClient agent, int size, int qtd, string CAARFIP, string CAARFPort)
        {
            //Envia mensagem de sinalização AGENT_SIGNAL2

            string payload = "AGENT_SIGNAL2|" + size + "|" + qtd + "|" + CAARFIP + "|" + CAARFPort;
            byte[] wirepayload = Encoding.ASCII.GetBytes(payload);
            agent.Send(wirepayload, wirepayload.Length, remote);
        }


        static void Main()
        {
            //AGENTE_UDP
            //Prototípo de agente utilizado para estimação de QOE
            try
            {
                //Informações do host local
                Console.WriteLine("AGENTE_UDP\n");
                string hostName = Dns.GetHostName();

                IPHostEntry hostInfo = Dns.GetHostEntry(hostName);

                Console.WriteLine("Nome do host: {0}", hostInfo.HostName);
                Console.WriteLine("Endereços IP:");
                foreach (IPAddress ipaddr in hostInfo.AddressList)
                {
                    Console.WriteLine(ipaddr.ToString());
                }
                Console.WriteLine();
            }
            catch (Exception)
            {
                Console.WriteLine("Não foi possivel resolver o host local...");
            }

            //Endereço IP
            IPAddress IPaddr = null;
            bool IPisValid = false;
            while (IPisValid == false)
            {
                Console.Write("IP: ");
                IPisValid = IPAddress.TryParse(Console.ReadLine(), out IPaddr);
                if (!IPisValid)
                {
                    Console.WriteLine("Erro: Endereço IP inválido.\n");
                }
            }

            //Porta
            UInt16 port = 5060;
            bool isValid = false;
            while (isValid == false)
            {
                Console.Write("PORTA: ");
                isValid = UInt16.TryParse(Console.ReadLine(), out port);
                if (!isValid)
                {
                    Console.WriteLine("Erro: Idealmente o numéro de porta informado deve estar no intervalo 49152 - 65535.\n");
                }
            }

            //Criação do UdpClient utilizado pelo agente
            IPEndPoint localIPEndPoint = new IPEndPoint(IPaddr, port);
            UdpClient agent = null;
            try
            {
                agent = new UdpClient(localIPEndPoint);        

            }
            catch (SocketException ex)
            {
                Console.WriteLine("Erro: Ocorreu um erro durante a criação do socket");
                Console.WriteLine(ex.ErrorCode+": "+ex.Message);
                Console.ReadLine();
                Environment.Exit(ex.ErrorCode);
            }
            Console.WriteLine("ESCUTANDO EM {0}...", localIPEndPoint);


            //IPEndPoint que vai armazenar informações do IPEndPoint remoto passado como referencia ao metodo Receive()
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);

            string        payload;
            const char    delimiter = '|';
            int           size;
            int           qtd;


            /*Loop principal - responsável pelo processamento das mensagens UDP
            TIPOS DE MENSAGEMS: CAARF_REQUEST, AGENT_SIGNAL1, AGENT_SIGNAL2, AGENT_UDP12*/
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
                            Console.WriteLine("{1} - RECEBEU REQUISICAO: {0} - {2} pacotes - tamanho de payload {3} bytes.", remoteIPEndPoint, DateTime.Now, payloadMessage[4], payloadMessage[3]);
                            IPEndPoint remoteAgent = new IPEndPoint(IPAddress.Parse(payloadMessage[1]), Int32.Parse(payloadMessage[2]));
                            Console.WriteLine("AGENTE REMOTO EM: {0}", remoteAgent);

                            size = Int32.Parse(payloadMessage[3]);
                            qtd = Int32.Parse(payloadMessage[4]);
                            sendSIGNAL1(remoteAgent, agent, size, qtd, remoteIPEndPoint);
                            break;
                        case "AGENT_SIGNAL1":
                            //Caso receba uma mensagem de sinal - AGENT_SIGNAL1
                            size = Int32.Parse(payloadMessage[1]);
                            qtd = Int32.Parse(payloadMessage[2]);
                            sendSIGNAL2(remoteIPEndPoint, agent, size, qtd, payloadMessage[3], payloadMessage[4]);
                            break;
                        case "AGENT_SIGNAL2":
                            //Caso receba uma mensagem de sinal - AGENT_SIGNAL2 
                            size = Int32.Parse(payloadMessage[1]);
                            qtd = Int32.Parse(payloadMessage[2]);                       
                            handleUDP1(remoteIPEndPoint, size, qtd, payloadMessage[3], payloadMessage[4], agent);
                            break;
                        case "AGENT_UDP12":
                            //Caso receba mensagem AGENT_UDP1 - Devolve o pacote ao agente remoto
                            agent.Send(wirePayload, wirePayload.Length, remoteIPEndPoint);
                            break;

                        default:
                            Console.WriteLine("Mensagem em formato incorreto.");
                            break;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro:" + ex.Message);                 
                }
            }
        }
    }
}
