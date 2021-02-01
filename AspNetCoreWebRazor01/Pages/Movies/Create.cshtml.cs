using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreWebRazor01.Data;
using AspNetCoreWebRazor01.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AspNetCoreWebRazor01.Authorization;

namespace AspNetCoreWebRazor01.Pages.Movies
{
    public class CreateModel : DI_BasePageModel
    {
        public CreateModel(
            AspNetCoreWebRazor01.Data.MyAppContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager) 
            : base(context, authorizationService, userManager)
        {
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Movie Movie { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Movie.OwnerID = UserManager.GetUserId(User);

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                User, Movie, AppOperations.Create);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            
            Context.Movie.Add(Movie);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
