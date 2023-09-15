using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace noteblog.Utils
{
    public class CacheHelper
    {
        public static void ClearAllCache()
        {
            Cache cache = HttpRuntime.Cache;
            IDictionaryEnumerator enumerator = cache.GetEnumerator();
            List<string> keysToRemove = new List<string>();

            while (enumerator.MoveNext())
            {
                keysToRemove.Add(enumerator.Key.ToString());
            }

            foreach (string key in keysToRemove)
            {
                cache.Remove(key);
            }

            //HttpResponse.RemoveOutputCacheItem("/Default.aspx");
        }
    }
}