using System;
using System.IO;
using System.Linq;
using System.Net;

namespace ModernProxyModule
{
    public class CustomProxy : IWebProxy
    {
        private string _proxyConfig => Environment.GetEnvironmentVariable("HTTP_PROXY");
        private string[] _noProxyList => (Environment.GetEnvironmentVariable("NO_PROXY") ?? "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        public ICredentials Credentials
        {
            get
            {
                NetworkCredential cred = null;
                if (!string.IsNullOrEmpty(_proxyConfig))
                {
                    var uri = new Uri(_proxyConfig);
                    if (!string.IsNullOrEmpty(uri.UserInfo))
                    {
                        cred = new NetworkCredential(uri.UserInfo.Split(':')[0], uri.UserInfo.Split(':')[1]);
                        Logger.Log($"Proxy User: {cred.UserName}{(string.IsNullOrEmpty(cred?.Password) ? " (No Pass)" : " (With Password)")}");
                    }
                }
                return cred;
            }
            set => throw new NotImplementedException();
        }

        public Uri GetProxy(Uri destination)
        {
            if (string.IsNullOrEmpty(_proxyConfig)) return null;
            Logger.Log($"Proxying {destination} via {_proxyConfig}");
            return new Uri(_proxyConfig);
        }

        public bool IsBypassed(Uri host)
        {
            return _noProxyList.Any(u => host.Host.EndsWith(u.Split(':').First(), StringComparison.OrdinalIgnoreCase) 
                && (u.Split(':').Count() == 0 || u.Split(':').Last() == host.Port.ToString()));
        }
    }
}
