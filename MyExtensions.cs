using Bnnsoft.Sdk;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bnnsoft.Sdk
{
    public static class MyExtensions
    {
        public static X509Certificate[] toX509(this ChainJson chain)
        {
            
            if (!string.IsNullOrEmpty(chain.sub))
            {
                X509Certificate[] rs = new X509Certificate[3];
                rs[0] = new Org.BouncyCastle.X509.X509CertificateParser().ReadCertificate(Convert.FromBase64String(chain.end));
                rs[1] = new Org.BouncyCastle.X509.X509CertificateParser().ReadCertificate(Convert.FromBase64String(chain.sub));
                rs[2] = new Org.BouncyCastle.X509.X509CertificateParser().ReadCertificate(Convert.FromBase64String(chain.root));
                return rs;
            }
            else
            {
                X509Certificate[] rs = new X509Certificate[2];
                rs[0] = new Org.BouncyCastle.X509.X509CertificateParser().ReadCertificate(Convert.FromBase64String(chain.end));
                rs[1] = new Org.BouncyCastle.X509.X509CertificateParser().ReadCertificate(Convert.FromBase64String(chain.root));
                return rs;
            }
        }
    }
}
