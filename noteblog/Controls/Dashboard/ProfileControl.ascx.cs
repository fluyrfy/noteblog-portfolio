using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using noteblog.Models;
using noteblog.Utils;

namespace noteblog.Controls
{
    public partial class ProfileControl : System.Web.UI.UserControl
    {
        private Logger log = new Logger("ProfileControl");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindProfileData();
            }
        }

        protected void bindProfileData()
        {
            int userId = AuthenticationHelper.GetUserId();
            UserRepository userRepository = new UserRepository();
            User user = userRepository.get(userId);
            List<dynamic> userSkills = userRepository.getSkills(userId);
            byte[] avatar = new byte[0];
            if (user.avatar != null && user.avatar.Length > 0)
            {
                avatar = user.avatar;
            }
            string name = user.name;
            txtEditProfileName.Text = name;
            txtPhone.Text = user.phone;
            txtRegionName.Text = user.region;
            txtRegionLink.Text = user.regionLink;
            txtGitHubLink.Text = user.githubLink;
            txtJobTitle.Text = user.jobTitle;
            txtAbout.Text = user.about;
            litUserName.Text = user.name;
            litEmail.Text = user.email;
            litRole.Text = user.role;
            litCreatedAt.Text = user.createdAt.ToLongDateString();
            foreach (var skill in userSkills)
            {
                string skillNameID = $"txtSkill{skill.skill_id}Name";
                string skillPercentID = $"txtSkill{skill.skill_id}Percent";
                TextBox txtSkillName = FindControl(skillNameID) as TextBox;
                TextBox txtSkillPercent = FindControl(skillPercentID) as TextBox;
                if (txtSkillName != null && txtSkillPercent != null)
                {
                    if (skill.name != null)
                    {
                        txtSkillName.Text = skill.name;
                    }
                    else if (skill.percent != null)
                    {
                        txtSkillPercent.Text = skill.percent;
                    }
                }
            }
            if (avatar.Length != 0)
            {
                imgProfileAvatar.ImageUrl = $"data:image/png;base64,{Convert.ToBase64String(avatar)}";
            }
        }

        protected void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            int id = AuthenticationHelper.GetUserId();
            try
            {
                log.Info("Starting to update profile");
                string name = txtEditProfileName.Text;
                string phone = txtPhone.Text;
                string region = txtRegionName.Text;
                string regionLink = txtRegionLink.Text;
                string githubLink = txtGitHubLink.Text;
                string jobTitle = txtJobTitle.Text;
                string about = txtAbout.Text;
                string skillOneName = txtSkill1Name.Text;
                string skillOnePercent = txtSkill1Percent.Text;
                string skillTwoName = txtSkill2Name.Text;
                string skillTwoPercent = txtSkill2Percent.Text;
                string skillThreeName = txtSkill3Name.Text;
                string skillThreePercent = txtSkill3Percent.Text;
                byte[] avatar = new byte[0];
                byte[] resume = new byte[0];
                if (fuEditProfileAvatar.HasFile)
                {
                    avatar = ConverterHelper.ConvertFileToBytes(fuEditProfileAvatar.PostedFile.InputStream);
                }
                if (fuEditResume.HasFile)
                {
                    resume = ConverterHelper.ConvertFileToBytes(fuEditResume.PostedFile.InputStream);
                }
                log.Debug($"Update profile name: {name}");
                UserRepository userRepository = new UserRepository();
                userRepository.update(name, avatar, resume, id, phone, region, regionLink, githubLink, jobTitle, about);
                userRepository.updateSkill(id, skillOneName, skillOnePercent, skillTwoName, skillTwoPercent, skillThreeName, skillThreePercent);
                log.Info("Profile updated successfully");
            }
            catch (Exception ex)
            {
                log.Error("Failed to update profile", ex);
            }
            finally
            {
                log.Info("End of profile update method");
                bindProfileData();
                if (Page is Dashboard dashboard)
                {
                    dashboard.bindUserData();
                }
            }
        }

    }
}