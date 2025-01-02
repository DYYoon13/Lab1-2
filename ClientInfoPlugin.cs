using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace DNWS
{
  class ClientInfoPlugin : IPlugin
  {
    protected static Dictionary<String, int> statDictionary = null;
    public ClientInfoPlugin()
    {
      if (statDictionary == null)
      {
        statDictionary = new Dictionary<String, int>();

      }
    }

    public void PreProcessing(HTTPRequest request)
    {
      if (statDictionary.ContainsKey(request.Url))
      {
        statDictionary[request.Url] = (int)statDictionary[request.Url] + 1;
      }
      else
      {
        statDictionary[request.Url] = 1;
      }
    }

    public HTTPResponse GetResponse(HTTPRequest request)
    {
      HTTPResponse response = null;
      StringBuilder sb = new StringBuilder();

      IPEndPoint endpoint = IPEndPoint.Parse(request.getPropertyByKey("remoteendpoint"));
      sb.Append("<html><head> <style> body {font-family: courier new: 20px;} h1 {font-size:40px;} div {line-height: 1.5;} </style></head></html>");
      sb.Append("<html><h1>Client Info:</h1>");
      sb.Append("<div>");
      sb.AppendFormat("<b>Client IP:</b> " + endpoint.Address + "<br/>" );
      sb.AppendFormat("<b>Client Port:</b> " + endpoint.Port + "<br/>");
      sb.AppendFormat("<b>Browser Information:</b> " + request.getPropertyByKey("user-agent").Trim() + "<br/>");
      sb.AppendFormat("<b>Accept Language:</b> " + request.getPropertyByKey("accept-language").Trim() + "<br/>");
      sb.AppendFormat("<b>Accept Encoding:</b> " + request.getPropertyByKey("accept-encoding").Trim() + "<br/>");
      sb.Append("</div>");
      sb.Append("</body></html>");

      response = new HTTPResponse(200);
      response.body = Encoding.UTF8.GetBytes(sb.ToString());
      return response;
    }


    public HTTPResponse PostProcessing(HTTPResponse response)
    {
      throw new NotImplementedException();
    }
  }
}