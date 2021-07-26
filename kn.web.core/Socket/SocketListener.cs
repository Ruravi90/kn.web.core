using kn.web.core.Models;
using kn.web.core.Socket;
using System;
using System.Net;  
using System.Net.Sockets;  
using System.Text;  
  
namespace kn.web.core._Socket
{
    public class SocketListener {

        // Incoming data from the client.  
        public static string data = null;

        private readonly EFKNContext _context;

        public SocketListener(string url) {
            _context = new EFKNContext(url);
        }

        public void StartListening()
        {
            // Data buffer for incoming data.  
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.  
            // Dns.GetHostName returns the name of the
            // host running the application.  
            //IPHostEntry host = Dns.GetHostEntry("localhost");
            //Console.WriteLine(host.AddressList[0]);
            IPAddress ipAddress = IPAddress.Parse("0.0.0.0");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 20001);

            // Create a TCP/IP socket.  
            System.Net.Sockets.Socket listener = new System.Net.Sockets.Socket(ipAddress.AddressFamily,  
                SocketType.Stream, ProtocolType.Tcp );  
    
            // Bind the socket to the local endpoint and
            // listen for incoming connections.  
            try {  
                listener.Bind(localEndPoint);  
                listener.Listen(10);  
    
                // Start listening for connections.  
                while (true) {  
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.  
                    System.Net.Sockets.Socket handler = listener.Accept();  
                    data = null;

                    // An incoming connection needs to be processed.  
                    while (true) {
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                        if (data.Contains("#L#")) {
                            Console.WriteLine(data);
                            ParseData getEvent = new ParseData();
                            try
                            {
                                Models.Event _event = getEvent.parse(data);
                                if (_event != null || (_event.antena != "" && _event.antena != null) )
                                {
                                    _context.Events.Add(_event);
                                    _context.SaveChanges();
                                }
                            }
                            catch (Exception e) { }
                        }

                        break;
                    }  
    
                    // Show the data on the console.  
                    //Console.WriteLine( "Text received : {0}", data);  
                    // Echo the data back to the client.  
                    //byte[] msg = Encoding.ASCII.GetBytes(data);  
                    //handler.Send(msg);  
                    handler.Shutdown(SocketShutdown.Both);  
                    handler.Close();  
                }  
    
            } catch (Exception e) {  
                Console.WriteLine(e.ToString());  
            }  
    
        }  
    }  
}