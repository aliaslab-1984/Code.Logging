using log4net.ObjectRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Code.Logging.Reporting
{
    public class JsonObjectRenderer : IObjectRenderer
    {
        public void RenderObject(RendererMap rendererMap, object obj, TextWriter writer)
        {
            writer.WriteLine(JsonConvert.SerializeObject(obj).Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t"));
        }
    }
}
