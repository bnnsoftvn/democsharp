using Amazon.Runtime;
using Amazon.Runtime.Internal.Util;
using Bnn.SignLib;
using iTextSharp.text;
using System.Diagnostics;

namespace Bnnsoft.Sdk
{
    public class Demo
    {
        public string ApiKey { get; set; }
        public string Secret { get; set; }
        public string Uri { get; set; }
        VinHsmServiceClient SignClient;

        public static int TYPE_PDFSIGNATURE_DONTSHOW = 0;
        public static int TYPE_PDFSIGNATURE_TEXT = 1;
        public static int TYPE_PDFSIGNATURE_IMA = 2;
        public static int TYPE_PDFSIGNATURE_TEXTIMA = 3;
        public Demo()
        {
            ApiKey = "37d48efb378945d387687ef32aaa705e";//API có ảnh chữ ký
            Secret = "eAVGPhrqIYwuMG6iULfRp6IkBtc4aygzUNnbd0+l";
            //ApiKey = "9c4556584de04bdfac5eeecdb1d89ff0";//API không có ảnh chữ ký
            //Secret = "3YNLFEaBH5jKuKAZmdJtnp/nUSZA9lvsBAuJ9sAI";
            Uri = "https://sign-hn5.vin-hsm.com";
            //Uri = "http://localhost:4315";
            FontFactory.RegisterDirectory("font");
            SignClient = new VinHsmServiceClient(new BasicAWSCredentials(ApiKey, Secret), new SignserverConfig()
            {
                ServiceURL = Uri
            });
        }

        public void Start()
        {
            SignPdfYdhcm(new SignPdfInput()
            {
                condau = File.ReadAllBytes("image/condau.png"),
                chukytuoi = File.ReadAllBytes("image/chukytuoi.png"),
                datapdf = File.ReadAllBytes("document/giayravien.pdf"),
                signaturename = Guid.NewGuid().ToString(),
                chucvu = "Giám Đốc",
                nguoiky = "Bác sỹ Nguyễn Demo",
                fontsize = 12,
                fontcolor = "#002f7a",
                padtop = 0,
                xpoint = 400,
                ypoint = 140,
                width = 160,
                height = 160,
                pagesign = -1,
                typesignature = TYPE_PDFSIGNATURE_TEXTIMA,
                hashalg = "SHA-256"
            });
            SignPdfYdhcm(new SignPdfInput()
            {
                //condau = File.ReadAllBytes("image/condau.png"),
                //chukytuoi = File.ReadAllBytes("image/chukytuoi.png"),
                datapdf = File.ReadAllBytes("document/giayravien.pdf"),
                signaturename = Guid.NewGuid().ToString(),
                chucvu = "Giám Đốc",
                nguoiky = "Bác sỹ Nguyễn Demo",
                fontsize = 12,
                fontcolor = "#002f7a",
                padtop = 0,
                xpoint = 400,
                ypoint = 140,
                width = 160,
                height = 160,
                pagesign = -1,
                typesignature = TYPE_PDFSIGNATURE_TEXTIMA,
                hashalg = "SHA-256"
            });

            SignSignatureImg(new SignPdfInput()
            {
                chucvu = "Giám Đốc",
                nguoiky = "Bác sỹ Nguyễn Demo",
                hashalg = "SHA-256",
                datapdf = File.ReadAllBytes("document/giayravien.pdf"),
                xpoint = 400,
                ypoint = 140,
                width = 160,
                height = 160,
                pagesign = -1,
            });

            InsertPdfImg(new SignPdfInput()
            {
                datapdf = File.ReadAllBytes("document/giayravien.pdf"),
                xpoint = 400,
                ypoint = 140,
                width = 160,
                height = 160,
                pagesign = 1,
            });


            SignXml(new SignXmlMultiRefRequest
            {
                base64xml = Convert.ToBase64String(File.ReadAllBytes("document/giayravien.xml")),
                datapath = new string[] { "//RootPaper/Root" },
                hashalg = "SHA-256",
                signatureid = Guid.NewGuid().ToString(),
                signaturepath = "//RootPaper",
            });

            SignXmlBhxh(new SignXmlBhxhRequest
            {
                base64xml = Convert.ToBase64String(File.ReadAllBytes("document/giaychungsinh.xml")),
                xpathdata = "",
                hashalg = "SHA-256",
                xpathsign = "//root",
            });

            SignXmlBhxh(new SignXmlBhxhRequest
            {
                base64xml = Convert.ToBase64String(File.ReadAllBytes("document/giaychungtu.xml")),
                xpathdata = "",
                hashalg = "SHA-256",
                xpathsign = "//root",
            });
            SignXmlBhxh(new SignXmlBhxhRequest
            {
                base64xml = Convert.ToBase64String(File.ReadAllBytes("document/giayksk.xml")),
                xpathdata = "",
                hashalg = "SHA-256",
                xpathsign = "//root",
            });

        }

        private void SignXmlBhxh(SignXmlBhxhRequest inp)
        {
            try
            {
                var signedxml = SignClient.SignXmlBhxh(inp);
                string output = Guid.NewGuid().ToString() + ".bhxh.signed.xml";
                File.WriteAllBytes(output, signedxml);
                Debug.WriteLine(output);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {

            }
        }

        public void InsertPdfImg(SignPdfInput input)
        {
            try
            {
                SignByte signByte = new SignByte(SignClient);
                byte[] bytes = input.datapdf;
                var pdf = new PdfHash(bytes, signByte);
                ImgStore imgStore = ImgStore.Instance;
                var img = imgStore.GetSigntureImage(SignClient);
                if (input.typesignature < 1 || input.typesignature > 3)
                    input.typesignature = 1;
                if (string.IsNullOrWhiteSpace(input.signaturename))
                    input.signaturename = "KySoDienTu";
                var signed = pdf.InsertSignatureImg(img, input.pagesign,
                                                    input.xpoint,
                                                    input.ypoint,
                                                    input.width,
                                                    input.height,
                                                    input.imgwidth,
                                                    input.imgheight);

                File.WriteAllBytes(Guid.NewGuid().ToString() + ".img.inserted.pdf", signed);
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

        }
        public void SignSignatureImg(SignPdfInput input)
        {
            try
            {
                SignByte signByte = new SignByte(SignClient);
                byte[] bytes = input.datapdf;
                var pdf = new PdfHash(bytes, signByte);
                ImgStore imgStore = ImgStore.Instance;
                var img = imgStore.GetSigntureImage(SignClient);
                if(img !=null)
                {
                    input.anhkhung = img;
                }    
                if (input.typesignature < 1 || input.typesignature > 3)
                    input.typesignature = 1;
                if (string.IsNullOrWhiteSpace(input.signaturename))
                    input.signaturename = "KySoDienTu";
                CertStore certStore = CertStore.Instance;
                var chain = certStore.getX509Chain(SignClient);
                var cert = chain[0];
                if (input.typesignature < 1 || input.typesignature > 3)
                    input.typesignature = 1;
                if (string.IsNullOrWhiteSpace(input.signaturename))
                    input.signaturename = "KySoDienTu";
                if (string.IsNullOrWhiteSpace(input.nguoiky))
                    input.nguoiky = cert.SubjectDN.ToString();

                var signed = pdf.SignSignatureImg(cert, chain, input.hashalg, input.typesignature, input.anhkhung, input.nguoiky, input.chucvu, input.signaturename,
                    input.pagesign, input.xpoint, input.ypoint, input.width, input.height, input.imgwidth, input.imgheight, input.borderstyle, input.bordercolor, input.fontcolor, input.fontname, input.fontstyle, input.fontsize, input.tylecondau, input.tylechukytuoi, input.textscale, input.padleft, input.padtop);

                File.WriteAllBytes(Guid.NewGuid().ToString() + "img.signed.pdf", signed);
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

        }

        public void SignXml(SignXmlMultiRefRequest inp)
        {
            try
            {
                var signedxml = SignClient.SignXmlMultiRef(inp);
                string output = Guid.NewGuid().ToString() + ".signed.xml";
                File.WriteAllBytes(output, signedxml);
                Debug.WriteLine(output);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {

            }
        }
        public void SignPdfYdhcm(SignPdfInput input)
        {
            try
            {
                SignByte signByte = new SignByte(SignClient);
                byte[] bytes = input.datapdf;
                var pdf = new PdfHash(bytes, signByte);
                CertStore certStore = CertStore.Instance;
                var chain = certStore.getX509Chain(SignClient);
                var cert = chain[0];
                if (input.typesignature < 1 || input.typesignature > 3)
                    input.typesignature = 1;
                if (string.IsNullOrWhiteSpace(input.signaturename))
                    input.signaturename = "KySoDienTu";
                var signed = pdf.SignPdfHash(cert, chain, input.hashalg, input.typesignature, input.condau, input.chukytuoi, input.nguoiky, input.chucvu, input.signaturename,
                    input.pagesign, input.xpoint, input.ypoint, input.width, input.height, input.imgwidth, input.imgheight, input.borderstyle, input.bordercolor, input.fontcolor, input.fontname, input.fontstyle, input.fontsize, input.tylecondau, input.tylechukytuoi, input.textscale, input.padleft, input.padtop);

                File.WriteAllBytes(Guid.NewGuid().ToString() + ".signed.pdf", signed);
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

        }
    }
}
