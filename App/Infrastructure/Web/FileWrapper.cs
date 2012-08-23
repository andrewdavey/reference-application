using System.IO;
using System.Web;

namespace App.Infrastructure.Web
{
    public class FileWrapper : HttpPostedFileBase
    {
        readonly Stream stream;
        readonly string contentType;

        public FileWrapper(Stream stream, string contentType)
        {
            this.stream = stream;
            this.contentType = contentType;
        }

        public override Stream InputStream
        {
            get { return stream; }
        }

        public override string ContentType
        {
            get { return contentType; }
        }

        public override int ContentLength
        {
            get { return (int)stream.Length; }
        }
    }
}