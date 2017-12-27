using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace Hallearn.Model.Model
{
    public class websocket
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        public static List<Socket> listachat = new List<Socket>();


        public websocket()
        {
            conectar();
        }


        //public static void conectar()
        //{

        //    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);



        //    IPAddress ip = IPAddress.Parse("127.0.0.1");
        //    IPEndPoint midireccion = new IPEndPoint(ip,1234);

        //    //asocia el socket a la direccion ip que va a escuchar
        //    socket.Bind(midireccion);
        //    socket.Listen(5);

        //    byte[] buffer = new byte[255];


        //    Socket cliente = socket.Accept();

        //    Byte[] response = Encoding.UTF8.GetBytes("Data received");
        //    socket.Write(response, 0, response.Length);

        //    cliente.Receive(buffer);


        //    try
        //    {




        //        while (true)
        //        {


        //        }
        //    }
        //    catch(Exception e) { }

        //}





        public static void conectar()
        {


            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 1234);

            try
            {
                server.Start();
            }
            catch { }
            
            TcpClient client = server.AcceptTcpClient();

            NetworkStream stream = client.GetStream();

          

            //enter to an infinite cycle to be able to handle every change in stream
            while (true)
            {

                while (!stream.DataAvailable) ; 
                
                    Byte[] bytes = new Byte[client.Available];

                    stream.Read(bytes, 0, bytes.Length);

                    //translate bytes of request to string
                    String request = Encoding.UTF8.GetString(bytes);

                    if (new Regex("^GET").IsMatch(request))
                    {
                        Byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + Environment.NewLine
                            + "Connection: Upgrade" + Environment.NewLine
                            + "Upgrade: websocket" + Environment.NewLine
                            + "Sec-WebSocket-Accept: " + Convert.ToBase64String(
                                SHA1.Create().ComputeHash(
                                    Encoding.UTF8.GetBytes(
                                        new Regex("Sec-WebSocket-Key: (.*)").Match(request).Groups[1].Value.Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
                                    )
                                )
                            ) + Environment.NewLine
                            + Environment.NewLine);

                        stream.Write(response, 0, response.Length);
                    }
                    else
                    {
                        //some bug in this part
                        //txtOutput.Text += request;
                        byte[] response = Encoding.UTF8.GetBytes("Data received");
                        stream.Write(response, 0, response.Length);

                    }
                


            }
        }




    }
}
