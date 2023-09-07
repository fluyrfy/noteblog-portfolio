using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;

namespace noteblog.Models.Mappings
{
    public class UserMap : EntityMap<User>
    {
        public UserMap()
        {
            Map(x => x.id).ToColumn("id");
            Map(x => x.name).ToColumn("name");
            Map(x => x.email).ToColumn("email");
            Map(x => x.passwordHash).ToColumn("password_hash");
            Map(x => x.avatar).ToColumn("avatar");
            Map(x => x.role).ToColumn("role");
            Map(x => x.verificationCode).ToColumn("verification_code");
            Map(x => x.isVerified).ToColumn("is_verified");
            Map(x => x.updatedAt).ToColumn("updated_at");
            Map(x => x.createdAt).ToColumn("created_at");
        }

    }
}