using EasyCache.Core.Abstractions;
using EasyCache.Memory.Concrete;
using Microsoft.Extensions.Caching.Memory;
using Org.BouncyCastle.X509;

namespace Bnnsoft.Sdk
{
    public class CertStore 
    {
        static CertStore _inst;        
        public static CertStore Instance
        {
            get
            {
                if (_inst == null)
                {
                    _inst = new CertStore();
                }
                return _inst;
            }
        }
        IEasyCacheService _easycache;
        public CertStore()
        {
            var cache = new MemoryCache(new MemoryCacheOptions()
            {

            });
            _easycache = new EasyMemoryCacheManager(cache);

        }

        public X509Certificate getX509(VinHsmServiceClient signClient)
        {
            var certbyte =  signClient.GetEndCertfiticate();
            return new Org.BouncyCastle.X509.X509CertificateParser().ReadCertificate(certbyte);
        }

        public X509Certificate[] getX509Chain(VinHsmServiceClient signClient)
        {
            var item = _easycache.Get<ChainJson>(signClient.Apikey);
            if (item == null)
            {
                item =  signClient.GetCertfiticateChain();
                _easycache.Set(signClient.Apikey, item, TimeSpan.FromMinutes(30));
            }
            return item.toX509();
        }
    }
}
