using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AspNetCoreWebRazor01.Data;
using AspNetCoreWebRazor01.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AspNetCoreWebRazor01.Authorization;

namespace AspNetCoreWebRazor01.Pages.Movies
{
    public class IndexModel : DI_BasePageModel
    {
        public IndexModel(
            AspNetCoreWebRazor01.Data.MyAppContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        public IList<Movie> Movie { get;set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string MovieGenre { get; set; }

        public SelectList GenreOptions { get; set; }

        public async Task OnGetAsync()
        {
            // get genres options
            var genres = from m in Context.Movie
                         orderby m.Genre
                         select m.Genre;
            
            GenreOptions = new SelectList(
                await genres.Distinct().ToListAsync());

            // get movie list
            var movies = from m in Context.Movie
                         select m;

            var isAuthorized = User.IsInRole(Constants.MovieManagersRole) 
                || User.IsInRole(Constants.MovieAdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            // Only approved movies are shown UNLESS you're authorized to see them
            // or you are the owner.
            if (!isAuthorized)
            {
                movies = movies.Where(
                    m => m.Status == MovieStatus.Approved 
                    || m.OwnerID == currentUserId);
            }

            if (!string.IsNullOrEmpty(SearchString))
            {
                movies = movies.Where(m => m.Title.Contains(SearchString));
            }
            if (!string.IsNullOrEmpty(MovieGenre))
            {
                movies = movies.Where(m => m.Genre == MovieGenre);
            }

            Movie = await movies.ToListAsync();
        }
    }
}
