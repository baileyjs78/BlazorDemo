using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using NoDb;

namespace ServerSide.Data
{
    public class MovieService
    {
        private IBasicCommands<Movie> _movieCommands;
        private IBasicQueries<Movie> _movieQueries;
        private IMemoryCache _cache;
        private const string projectId = "movie-app";
        private const string moviesCacheKey = "all-movies";

        public MovieService(IBasicCommands<Movie> movieCommands, IBasicQueries<Movie> movieQueries, IMemoryCache memoryCache)
        {
            _movieCommands = movieCommands;
            _movieQueries = movieQueries;
            _cache = memoryCache;
        }

        public async Task<List<Movie>> GetMovies()
        {
            var movies = await GetAllMovies();

            return movies.OrderBy(m => m.Title).ToList();
        }

        private async Task<IEnumerable<Movie>> GetAllMovies()
        {
            IEnumerable<Movie> movies;
            if (_cache.TryGetValue(moviesCacheKey, out movies))
            {
                return movies;
            }

            movies = await _movieQueries.GetAllAsync(projectId).ConfigureAwait(false);

            _cache.Set(moviesCacheKey, movies);
            return movies;
        }

        public async Task<Movie> GetMovie(Guid id)
        {
            var movies = await GetAllMovies();
            var movie = movies.FirstOrDefault(m => m.Id == id);

            if (movie == null)
            {
                throw new System.Exception("Movie Not found");
            }

            return movie;
        }

        public async Task<bool> UpdateMovie(Movie movie)
        {

            await _movieCommands.UpdateAsync(projectId, movie.Id.ToString(), movie).ConfigureAwait(false);
            _cache.Remove(moviesCacheKey);

            return true;
        }

        public async Task<bool> CreateMovie(Movie movie)
        {

            await _movieCommands.CreateAsync(projectId, movie.Id.ToString(), movie).ConfigureAwait(false);
            _cache.Remove(moviesCacheKey);

            return true;
        }

        public async Task<bool> DeleteMovie(Guid id)
        {

            var movies = await GetAllMovies();
            var movie = movies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                throw new System.Exception("Movie Not found");
            }

            await _movieCommands.DeleteAsync(projectId, id.ToString()).ConfigureAwait(false);
            _cache.Remove(moviesCacheKey);

            return true;
        }
    }
}
