using GainesOpusInstitute.DataEntity.Entity;

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GainesOpusInstitute.Repository
{
   public class ClaimRepo: IClaimRepo
    {
        private readonly UserManager<User> _userManager;
        public ClaimRepo(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ChangeClaim> GetClaims(ChangeClaim request)
        {
            if (request.NewClaim == request.CurrentClaim)
            {
                throw new Exception("User already has that authority.");
            }

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var claimToRemove = _userManager.GetClaimsAsync(user).Result.Single(c => c.Type == request.CurrentClaim);
            var removedResult = await _userManager.RemoveClaimAsync(user, claimToRemove);
            if (removedResult.Succeeded)
            {
                var addClaim = await _userManager.AddClaimAsync(user, new Claim(request.NewClaim, "true"));
                if (addClaim.Succeeded)
                {
                    throw new Exception("Successfully changed role");
                    //return new BaseResult { IsSuccess = true, Success = $"Successfully changed role from {request.CurrentClaim} to {request.NewClaim} for user {user.Email}" };
                }
            }
            return request;
        }
    }
}
