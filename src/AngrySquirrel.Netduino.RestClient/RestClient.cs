using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AngrySquirrel.Netduino.RestClient
{
    /// <summary>
    /// Represents a client for interacting with a REST API
    /// </summary>
    public class RestClient
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RestClient"/> class
        /// </summary>
        /// <param name="host">
        /// The REST API host name
        /// </param>
        /// <param name="port">
        /// The port on which to connect to the REST API
        /// </param>
        public RestClient(string host, int port)
        {
            Host = host;

            var hostEntry = Dns.GetHostEntry(host);
            foreach (var address in hostEntry.AddressList)
            {
                if (address == null)
                {
                    continue;
                }

                var ipEndPoint = new IPEndPoint(address, port);

                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    socket.Connect(ipEndPoint);
                    var isConnected = socket.Poll(10, SelectMode.SelectWrite);

                    if (isConnected)
                    {
                        IpEndPoint = ipEndPoint;
                        break;
                    }
                }
                finally
                {
                    socket.Close();
                }
            }
        }

        #endregion

        #region Properties

        private string Host { get; set; }

        private IPEndPoint IpEndPoint { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Sends an HTTP DELETE request to the given resource
        /// </summary>
        /// <param name="resource">
        /// The resource to which to send the DELETE request
        /// </param>
        public void Delete(string resource)
        {
            PerformHttpRequest(resource, HttpMethod.Delete, null);
        }

        /// <summary>
        /// Sends an HTTP GET request to the given resource
        /// </summary>
        /// <param name="resource">
        /// The resource to which to send the GET request
        /// </param>
        public void Get(string resource)
        {
            PerformHttpRequest(resource, HttpMethod.Get, null);
        }

        /// <summary>
        /// Sends an HTTP POST request to the given resource
        /// </summary>
        /// <param name="resource">
        /// The resource to which to send the POST request
        /// </param>
        /// <param name="content">
        /// The content body to submit with the request
        /// </param>
        public void Post(string resource, string content)
        {
            PerformHttpRequest(resource, HttpMethod.Post, content);
        }

        /// <summary>
        /// Sends an HTTP PUT request to the given resource
        /// </summary>
        /// <param name="resource">
        /// The resource to which to send the PUT request
        /// </param>
        /// <param name="content">
        /// The content body to submit with the request
        /// </param>
        public void Put(string resource, string content)
        {
            PerformHttpRequest(resource, HttpMethod.Put, content);
        }

        #endregion

        #region Methods

        private byte[] GenerateHttpRequest(string resource, HttpMethod httpMethod, string content)
        {
            string httpRequest;
            switch (httpMethod)
            {
                case HttpMethod.None:
                    throw new ArgumentException("The specified HTTP method is not supported.");
                case HttpMethod.Delete:
                    httpRequest = "DELETE " + resource + " HTTP/1.1\r\n" +
                                  "Host: " + Host + ":" + IpEndPoint.Port + "\r\n\r\n";
                    break;
                case HttpMethod.Get:
                    httpRequest = "GET " + resource + " HTTP/1.1\r\n" +
                                  "Host: " + Host + ":" + IpEndPoint.Port + "\r\n\r\n";
                    break;
                case HttpMethod.Post:
                    httpRequest = "POST " + resource + " HTTP/1.1\r\n" +
                                  "Host: " + Host + ":" + IpEndPoint.Port + "\r\n" +
                                  "Content-Type: application/json\r\n" +
                                  "Content-Length: " + Encoding.UTF8.GetBytes(content)
                                                               .Length + "\r\n\r\n" +
                                  content + "\r\n\r\n";
                    break;
                case HttpMethod.Put:
                    httpRequest = "PUT " + resource + " HTTP/1.1\r\n" +
                                  "Host: " + Host + ":" + IpEndPoint.Port + "\r\n" +
                                  "Content-Type: application/json\r\n" +
                                  "Content-Length: " + Encoding.UTF8.GetBytes(content)
                                                               .Length + "\r\n\r\n" +
                                  content + "\r\n\r\n";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("httpMethod");
            }

            return Encoding.UTF8.GetBytes(httpRequest);
        }

        /// <summary>
        /// Performs an HTTP request
        /// </summary>
        /// <param name="resource">
        /// The resource to which to send the HTTP request
        /// </param>
        /// <param name="httpMethod">
        /// The HTTP method to use
        /// </param>
        /// <param name="content">
        /// The content body to submit with the request
        /// </param>
        private void PerformHttpRequest(string resource, HttpMethod httpMethod, string content)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(IpEndPoint);
                socket.Send(GenerateHttpRequest(resource, httpMethod, content));
            }
            finally
            {
                socket.Close();
            }
        }

        #endregion
    }
}