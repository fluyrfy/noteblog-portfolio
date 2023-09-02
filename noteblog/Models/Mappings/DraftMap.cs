using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;

namespace noteblog.Models.Mappings
{
    public class DraftMap : EntityMap<Draft>
    {
        public DraftMap()
        {
            Map(x => x.id).ToColumn("id");
            Map(x => x.categoryId).ToColumn("category_id");
            Map(x => x.title).ToColumn("title");
            Map(x => x.content).ToColumn("content");
            Map(x => x.keyword).ToColumn("keyword");
            Map(x => x.pic).ToColumn("pic");
            Map(x => x.noteId).ToColumn("note_id");
            Map(x => x.userId).ToColumn("user_id");
            Map(x => x.updatedAt).ToColumn("updated_at");
            Map(x => x.createdAt).ToColumn("created_at");
        }

    }
}