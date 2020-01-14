using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorDemo.Pages;

namespace BlazorDemo.Components
{
    public class MovieService
    {
        public List<Movie> movies = new List<Movie>();
        public bool ShowMovieForm { get; set; } = false;
        public bool IsNewMovie { get; set; } = false;
        public Movie movieForm { get; set; } = new Movie();


        public async Task AddMovie()
        {
            movieForm = new Movie();
            IsNewMovie = true;
            ShowMovieModal();
        }
        public async Task EditMovie(Guid movieId)
        {
            var mymovie = movies.FirstOrDefault(x => x.Id == movieId);
            movieForm = new Movie { Id = mymovie.Id, Title = mymovie.Title, Year = mymovie.Year, Director = mymovie.Director, Description = mymovie.Description };
            IsNewMovie = false;
            ShowMovieModal();
        }
        public async Task DeleteMovie(Movie movie)
        {
            movies.Remove(movie);
        }

        public async Task SaveMovie()
        {
            if (IsNewMovie)
            {
                movies.Add(movieForm);
            }
            else
            {
                var movieIndex = movies.FindIndex(x => x.Id == movieForm.Id);
                movies.RemoveAt(movieIndex);
                movies.Insert(movieIndex, movieForm);
            }
            HideMovieModal();
        }

        public void ShowMovieModal()
        {
            ShowMovieForm = true;

        }
        public async Task HideMovieModal()
        {
            ShowMovieForm = false;
        }
    }
}
