using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;


namespace InputChange2
{
    public class HttpServer
    {
        private HttpListener listener;

        public void Start()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8000/");
            listener.Start();
            listener.BeginGetContext(OnContext, null);
        }
        public Action<string> OnInput = null;
        private void OnContext(IAsyncResult ar)
        {
            if (!listener.IsListening) return;
            try
            {


                var context = listener.EndGetContext(ar);

                // Begin listening for the next request
                listener.BeginGetContext(OnContext, null);

                var request = context.Request;
                var response = context.Response;
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                if (request.HttpMethod == "GET")
                {
                    // Handle GET request
                    var dataInputValue = request.QueryString["input"];
                    if (OnInput != null)
                    {
                        OnInput(dataInputValue);
                    }
                    // Write response
                    var responseString = $"{dataInputValue} is ok.";
                    var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                    response.ContentLength64 = buffer.Length;
                    var output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void Stop()
        {
            listener.Stop();
        }
    }
}
