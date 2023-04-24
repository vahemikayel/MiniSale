using Microsoft.Extensions.Options;
using MiniSale.Api.Infrastructure.Options;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System;

namespace MiniSale.Api.Certificate
{
    public class CertificateManager
    {
        IdentityJWTOptions _options;
        public CertificateManager(IOptions<IdentityJWTOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public X509Certificate2 Get()
        {
            var assembly = typeof(CertificateManager).GetTypeInfo().Assembly;
            var names = assembly.GetManifestResourceNames();

            using (var stream = File.OpenRead(_options.Certificate.CertificatePath))
            {
                return new X509Certificate2(ReadStream(stream), _options.Certificate.CertificatePassword);
            }
        }

        private byte[] ReadStream(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
