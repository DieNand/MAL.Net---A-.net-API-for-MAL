using System;
using System.ServiceModel.Channels;
using System.Web.Http.SelfHost;
using System.Web.Http.SelfHost.Channels;

namespace MAL.NetSelfHosted
{
    public class SecureHttpSelfHostConfiguration : HttpSelfHostConfiguration
    {
        public SecureHttpSelfHostConfiguration(string baseAddress) : base(baseAddress)
        {
        }

        public SecureHttpSelfHostConfiguration(Uri baseAddress) : base(baseAddress)
        {
        }

        protected override BindingParameterCollection OnConfigureBinding(HttpBinding httpBinding)
        {
            httpBinding.Security.Mode = HttpBindingSecurityMode.Transport;
            return base.OnConfigureBinding(httpBinding);
        }
    }
}