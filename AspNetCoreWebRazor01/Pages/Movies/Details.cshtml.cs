using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AspNetCoreWebRazor01.Data;
using AspNetCoreWebRazor01.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AspNetCoreWebRazor01.Authorization;

namespace AspNetCoreWebRazor01.Pages.Movies
{
    public class DetailsModel : DI_BasePageModel
    {
        public DetailsModel(
            AspNetCoreWebRazor01.Data.MyAppContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        public Movie Movie { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Movie = await Context.Movie.FirstOrDefaultAsync(m => m.ID == id);

            if (Movie == null)
            {
                return NotFound();
            }

            var isAuthorized = User.IsInRole(Constants.MovieManagersRole) 
                || User.IsInRole(Constants.MovieAdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            if (!isAuthorized
                && currentUserId != Movie.OwnerID
                && Movie.Status != MovieStatus.Approved)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, MovieStatus status)
        {
            var movie = await Context.Movie.FirstOrDefaultAsync(m => m.ID == id);

            if (movie == null)
            {
                return NotFound();
            }

            var currentOperation = 
                (status == MovieStatus.Approved) ? AppOperations.Approve : AppOperations.Reject;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                User, movie, currentOperation);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            movie.Status = status;
            Context.Movie.Update(movie);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
