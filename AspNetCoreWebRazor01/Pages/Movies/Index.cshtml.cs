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

namespace AspNetCoreWebRazor01.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly AspNetCoreWebRazor01.Data.MyAppContext _context;

        public IndexModel(AspNetCoreWebRazor01.Data.MyAppContext context)
        {
            _context = context;
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
            var genres = from m in _context.Movie
                         orderby m.Genre
                         select m.Genre;
            
            GenreOptions = new SelectList(
                await genres.Distinct().ToListAsync());

            // get movie list
            var movies = from m in _context.Movie
                         select m;

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
