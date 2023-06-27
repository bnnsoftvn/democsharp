using Bnn.SignLib;
using Bnnsoft.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bnnsoft.Sdk
{
    public class SignByte : IByteSigner
    {
        private VinHsmServiceClient signClient;

        public SignByte(VinHsmServiceClient signClient)
        {
            this.signClient = signClient;
        }

        public byte[] Sign(byte[] digestInfo)
        {
            return signClient.Sign(digestInfo);
        }
    }
}
