using System;
using System.Threading.Tasks;
using System.Net.Http;

namespace HttpPortable
{
    public class NetHttp
    {
        IRenderer ad;

        public NetHttp(IRenderer ad)
        {
            this.ad = ad;
        }

        public async Task HttpSample(string url)
        {
            var client = new System.Net.Http.HttpClient();
            ad.RenderStream(await client.GetStreamAsync(url));
        }
    }
}

