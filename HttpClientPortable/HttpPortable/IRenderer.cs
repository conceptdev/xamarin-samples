using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HttpPortable
{
    public interface IRenderer
    {
        void RenderStream(Stream stream);
    }
}
