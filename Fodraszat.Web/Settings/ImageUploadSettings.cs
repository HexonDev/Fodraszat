using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fodraszat.Web.Settings
{
    public class ImageUploadSettings
    {
        public long MaxFileSize { get; set; }
        public List<string> AllowedExtensions { get; set; }
    }
}
