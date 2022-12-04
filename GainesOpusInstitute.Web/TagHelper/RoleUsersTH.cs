
using GainesOpusInstitute.DataEntity.Entity;
using GainesOpusInstitute.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GainesOpusInstitute.Web.Models
{
    [HtmlTargetElement("td", Attributes = "i-role")]

    public class RoleUsersTH :TagHelper
    {
        private UserManager<User> userManager;
        private RoleManager<Role> roleManager;
        private readonly IAccountManager _accountManager;

        public RoleUsersTH(UserManager<User> usermgr, RoleManager<Role> rolemgr, IAccountManager accountManager)
        {
            userManager = usermgr;
            roleManager = rolemgr;
            _accountManager = accountManager;
        }

        [HtmlAttributeName("i-role")]
        public long Role { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            List<string> names = new List<string>();
            // var role = await roleManager.FindByIdAsync(Role);

            var rolee = await _accountManager.GetRoleByIdAsync(Role);

            if (rolee != null)
            {
                var results = userManager.Users;
                foreach (var user in results)
                {
                    if (user != null && await userManager.IsInRoleAsync(user, rolee.Name))

                        names.Add(user.UserName);
                }
            }
            output.Content.SetContent(names.Count == 0 ? "No Users" : string.Join(", ", names));
        }
    }
}
