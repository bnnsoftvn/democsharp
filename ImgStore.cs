using EasyCache.Core.Abstractions;
using EasyCache.Memory.Concrete;
using Microsoft.Extensions.Caching.Memory;

namespace Bnnsoft.Sdk
{
    public class ImgStore
    {     
        static ImgStore _inst;        
        public static ImgStore Instance
        {
            get
            {
                if (_inst == null)
                {
                    _inst = new ImgStore();
                }
                return _inst;
            }
        }
        IEasyCacheService _easycache;
        public ImgStore()
        {
            var cache = new MemoryCache(new MemoryCacheOptions()
            {
               
            });
            _easycache = new EasyMemoryCacheManager(cache);

        }
        
        public byte[] GetSigntureImage(VinHsmServiceClient signClient)
        {
            var img = _easycache.Get<byte[]>(signClient.Apikey);
            if (img == null)
            {
                img = signClient.GetSignatureImg(new GetSigntureImageRequest { });
                _easycache.Set(signClient.Apikey, img, TimeSpan.FromMinutes(30));
            }
            return img;
        }
    }
}
