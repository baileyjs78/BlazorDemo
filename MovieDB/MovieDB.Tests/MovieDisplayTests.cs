using Microsoft.AspNetCore.Components.Testing;
using MovieDB.Client.Components;
using MovieDB.Shared;
using System;
using System.Collections.Generic;
using Xunit;
using Microsoft.AspNetCore.Components;

namespace MovieDB.Tests
{
    public class MovieDisplayTests
    {
        private TestHost host = new TestHost();

        [Fact]
        public void DisplaySingleMovie()
        {
            // Arrange
            var movies = GetMovies();

            ParameterView paramView = ParameterView.FromDictionary(new Dictionary<string, object>() { { "movies", movies.GetRange(0,1) } });
            var component = host.AddComponent<MovieDisplay>(paramView);

            // Assert
            Assert.Equal(movies[0].Title,component.Find(".card-title").InnerText);
            Assert.Equal(movies[0].Year + " - " + movies[0].Director, component.Find(".card-subtitle").InnerText);
            // Must use Contains because of new lines
            Assert.Contains(movies[0].Description, component.Find(".card-text").InnerText);

        }

        [Fact]
        public void DisplayMultipleMovies()
        {
            // Arrange
            var movies = GetMovies();

            ParameterView paramView = ParameterView.FromDictionary(new Dictionary<string, object>() { { "movies", movies } });
            var component = host.AddComponent<MovieDisplay>(paramView);

            // Assert
            Assert.Equal(3, component.FindAll(".card-title").Count);
            Assert.Collection(component.FindAll(".card-title"),
                    x => Assert.Equal(movies[0].Title, x.InnerText),
                    x => Assert.Equal(movies[1].Title, x.InnerText),
                    x => Assert.Equal(movies[2].Title, x.InnerText));

            Assert.Equal(3, component.FindAll(".card-subtitle").Count);
            Assert.Collection(component.FindAll(".card-subtitle"),
                    x => Assert.Equal(movies[0].Year + " - " + movies[0].Director, x.InnerText),
                    x => Assert.Equal(movies[1].Year + " - " + movies[1].Director, x.InnerText),
                    x => Assert.Equal(movies[2].Year + " - " + movies[2].Director, x.InnerText));

            // Must use Contains because of new lines
            Assert.Equal(3, component.FindAll(".card-text").Count);
            Assert.Collection(component.FindAll(".card-text"),
                    x => Assert.Contains(movies[0].Description, x.InnerText),
                    x => Assert.Contains(movies[1].Description, x.InnerText),
                    x => Assert.Contains(movies[2].Description, x.InnerText));
        }

        private List<Movie> GetMovies()
        {
            List<Movie> movies = new List<Movie> {
                    new Movie {
                        Title = "First Movie Title",
                        Year = 1995,
                        Director = "My Director",
                        Description = "Great Movie"
                    },
                    new Movie {
                        Title = "Second Movie Title",
                        Year = 2000,
                        Director = "Second Director",
                        Description = "Bad Movie"
                    },
                    new Movie {
                        Title = "Third Movie Title",
                        Year = 2005,
                        Director = "Third Director",
                        Description = "Worst Movie"
                    },
            };
            return movies;
        }
    }
}
