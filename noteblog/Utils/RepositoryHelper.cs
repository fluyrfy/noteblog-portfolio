using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using K4os.Hash.xxHash;
using noteblog.Models.Mappings;

namespace noteblog.Utils
{
    public class RepositoryHelper
    {
        public RepositoryHelper()
        {

        }

        public static void Initialize()
        {
            FluentMapper.EntityMaps.Clear();
            FluentMapper.Initialize(config =>
            {
                //config.AddMap();
            });
        }
    }
}