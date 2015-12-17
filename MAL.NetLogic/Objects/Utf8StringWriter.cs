using System.IO;
using System.Text;

namespace MAL.NetLogic.Objects
{
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}