using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MovieDB.Shared;
using MovieDB.Client.Services;

namespace MovieDB.Client.Services
{
    public class MovieService
    {
        public List<Movie> movies = new List<Movie>();
        public bool ShowMovieForm { get; set; } = false;
        public bool IsNewMovie { get; set; } = false;
        public bool ShowMovieFormAlert { get; set; } = false;
        public Movie movieForm { get; set; } = new Movie();

        public string BaseUri;

        public MovieService(string baseUri)
        {
            BaseUri = baseUri;
        }

        public async Task InitiateMovieAPI()
        {
            var client = new HttpClient();
            movies = await client.GetJsonAsync<List<Movie>>(BaseUri + "api/Movies");
        }
        
        private async Task UpdateMovieAPI(Movie movie)
        {
            var client = new HttpClient();
            await client.SendJsonAsync(HttpMethod.Put, BaseUri + "api/Movies/" + movie.Id, movie);
        }
        private async Task AddMovieAPI(Movie movie)
        {
            var client = new HttpClient();
            await client.SendJsonAsync(HttpMethod.Post, BaseUri + "api/Movies", movie);
        }
        private async Task DeleteMovieAPI(Movie movie)
        {
            var client = new HttpClient();
            await client.DeleteAsync(BaseUri + "api/Movies/" + movie.Id);
        }

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
            await DeleteMovieAPI(movie);
        }

        public async Task SaveMovie()
        {
            if (IsNewMovie)
            {
                movies.Add(movieForm);
                await AddMovieAPI(movieForm);
            }
            else
            {
                var movieIndex = movies.FindIndex(x => x.Id == movieForm.Id);
                movies.RemoveAt(movieIndex);
                movies.Insert(movieIndex, movieForm);
                await UpdateMovieAPI(movieForm);
            }
            HideMovieModal();
        }

        public void ShowMovieModal()
        {
            ShowMovieFormAlert = false;
            ShowMovieForm = true;

        }
        public async Task HideMovieModal()
        {
            ShowMovieForm = false;
        }
    }
}
