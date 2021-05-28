using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace VK_Desktop
{
    public class NewsClass
    {
        public string PublicName { get; set; }
        public string PostDate { get; set; }
        public string PublicAvatarURL { get; set; }
        public string TextContent { get; set; }
        public string[] Images { get; set; }
        public NewsClass(string PublicName,string PostDate, string PublicAvatarURL, string Content)
        {
            this.PublicName = PublicName;
            this.PostDate = PostDate;
            this.PublicAvatarURL = PublicAvatarURL;
            this.TextContent = Content;
            Images = new string[10];
        }
    }
}
