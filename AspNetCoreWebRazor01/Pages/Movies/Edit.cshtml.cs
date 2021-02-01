using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AspNetCoreWebRazor01.Data;
using AspNetCoreWebRazor01.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AspNetCoreWebRazor01.Authorization;

namespace AspNetCoreWebRazor01.Pages.Movies
{
    public class EditModel : DI_BasePageModel
    {
        public EditModel(
            AspNetCoreWebRazor01.Data.MyAppContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
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

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                User, Movie, AppOperations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Fetch record from DB to get OwnerID.
            var movieDb = await Context.Movie
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (movieDb == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                User, movieDb, AppOperations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            Movie.OwnerID = movieDb.OwnerID;

            Context.Attach(Movie).State = EntityState.Modified;

            if (Movie.Status == MovieStatus.Approved)
            {
                // If the record is updated after approval, 
                // and the user cannot approve,
                // set the status back to submitted so the update can be
                // checked and approved.
                var canApprove = await AuthorizationService.AuthorizeAsync(
                    User, Movie, AppOperations.Approve);

                if (!canApprove.Succeeded)
                {
                    Movie.Status = MovieStatus.Submitted;
                }
            }

            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
