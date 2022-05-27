using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Req.Assets
{
    public class AssetImgV2
    {
        public string AssetImageClientID { get; set; }
        public string AssetImageServerID { get; set; }
        public string AssetClientID { get; set; }
        public string AssetServerID { get; set; }
        public string CreateDateClient { get; set; }
        public string Ext { get; set; }
        public string ImgPath { get; set; }
        public string Remark { get; set; }
    }
}