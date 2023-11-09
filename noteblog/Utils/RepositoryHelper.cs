using Dapper.FluentMap;

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