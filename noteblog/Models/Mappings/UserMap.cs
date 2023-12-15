using Dapper.FluentMap.Mapping;

namespace noteblog.Models.Mappings
{
  public class UserMap : EntityMap<User>
  {
    public UserMap()
    {
      Map(x => x.id).ToColumn("id");
      Map(x => x.name).ToColumn("name");
      Map(x => x.phone).ToColumn("phone");
      Map(x => x.email).ToColumn("email");
      Map(x => x.region).ToColumn("region");
      Map(x => x.regionLink).ToColumn("region_link");
      Map(x => x.githubLink).ToColumn("github_link");
      Map(x => x.jobTitle).ToColumn("job_title");
      Map(x => x.about).ToColumn("biography");
      Map(x => x.passwordHash).ToColumn("password_hash");
      Map(x => x.avatar).ToColumn("avatar");
      Map(x => x.resume).ToColumn("resume");
      Map(x => x.role).ToColumn("role");
      Map(x => x.verificationCode).ToColumn("verification_code");
      Map(x => x.isVerified).ToColumn("is_verified");
      Map(x => x.updatedAt).ToColumn("updated_at");
      Map(x => x.createdAt).ToColumn("created_at");
    }

  }
}