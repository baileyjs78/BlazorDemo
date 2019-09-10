using Microsoft.AspNetCore.Components.Testing;
using MovieDB.Client.Components;
using MovieDB.Shared;
using System;
using System.Collections.Generic;
using Xunit;
using Microsoft.AspNetCore.Components;

namespace MovieDB.Tests
{
    public class MovieAddEditTests
    {
        private TestHost host = new TestHost();

        [Fact]
        public void NewMovie_AllFieldsAreBlank()
        {
            ParameterView paramView = ParameterView.FromDictionary(new Dictionary<string, object>() { { "movie", new Movie() }, { "IsNewMovie", true} });
            var component = host.AddComponent<MovieAddedit>(paramView);

            Assert.Equal("", component.Find("#Title").InnerText);
            Assert.Equal("", component.Find("#Year").InnerText);
            Assert.Equal("", component.Find("#Director").InnerText);
            Assert.Equal("", component.Find("#Description").InnerText);
        }

        [Fact]
        public void NewMovie_FieldsAreRequired()
        {
            ParameterView paramView = ParameterView.FromDictionary(new Dictionary<string, object>() { { "movie", new Movie() }, { "IsNewMovie", true } });
            var component = host.AddComponent<MovieAddedit>(paramView);

            component.Find("form").Submit();

            Assert.Collection(component.FindAll(".validation-message"),
                x => Assert.Equal("Enter a Title", x.InnerText),
                x => Assert.Equal("Year must be from 1920 to 2020", x.InnerText),
                x => Assert.Equal("The Director field is required.", x.InnerText),
                x => Assert.Equal("The Description field is required.", x.InnerText));
        }

        [Fact]
        public void NewMovie_TitleIsLessThan100()
        {
            ParameterView paramView = ParameterView.FromDictionary(new Dictionary<string, object>() { { "movie", new Movie() }, { "IsNewMovie", true } });
            var component = host.AddComponent<MovieAddedit>(paramView);

            component.Find("#Title").Change("This Is a really long string that is more than 100 characters long and will cause the validation to return an error");
            component.Find("form").Submit();

            Assert.Equal("That name is too long",component.Find(".validation-message").InnerText);
        }

        [Fact]
        public void NewMovie_ValidationFailsWhenYearOutofRangeLow()
        {
            ParameterView paramView = ParameterView.FromDictionary(new Dictionary<string, object>() { { "movie", new Movie() }, { "IsNewMovie", true } });
            var component = host.AddComponent<MovieAddedit>(paramView);

            component.Find("#Title").Change("Test");
            component.Find("#Year").Change("1900");
            component.Find("#Director").Change("Test");
            component.Find("#Description").Change("Test");
            component.Find("form").Submit();


            Assert.Equal("Year must be from 1920 to 2020", component.Find(".validation-message").InnerText);
        }

        public void NewMovie_ValidationFailsWhenYearOutofRangeHigh()
        {
            ParameterView paramView = ParameterView.FromDictionary(new Dictionary<string, object>() { { "movie", new Movie() }, { "IsNewMovie", true } });
            var component = host.AddComponent<MovieAddedit>(paramView);

            component.Find("#Title").Change("Test");
            component.Find("#Year").Change("2900");
            component.Find("#Director").Change("Test");
            component.Find("#Description").Change("Test");
            component.Find("form").Submit();


            Assert.Equal("Year must be from 1920 to 2020", component.Find(".validation-message").InnerText);
        }

        [Fact]
        public void NewMovie_ValidationSuccessWhenAllFieldsAreValid()
        {
            ParameterView paramView = ParameterView.FromDictionary(new Dictionary<string, object>() { { "movie", new Movie() }, { "IsNewMovie", true } });
            var component = host.AddComponent<MovieAddedit>(paramView);

            component.Find("#Title").Change("Test");
            component.Find("#Year").Change("1995");
            component.Find("#Director").Change("Test");
            component.Find("#Description").Change("Test");
            component.Find("form").Submit();

            Assert.Equal(0, component.FindAll(".validation-message").Count);
        }

        
        [Fact]
        public void EditMovie_AllFieldsHaveValues()
        {
            var movie = new Movie
            {
                Title = "First Movie Title",
                Year = 1995,
                Director = "My Director",
                Description = "Great Movie"
            };
            ParameterView paramView = ParameterView.FromDictionary(new Dictionary<string, object>() { { "movie", movie }, { "IsNewMovie", false } });
            var component = host.AddComponent<MovieAddedit>(paramView);

            // When you edit, the initial value (in the DOM) [innterText] doesn't seem to contain the value we want (it is blank)
            Assert.Contains(movie.Title, component.Find("#Title").OuterHtml);
            Assert.Contains(movie.Year.ToString(), component.Find("#Year").OuterHtml);
            Assert.Contains(movie.Director, component.Find("#Director").OuterHtml);
            Assert.Contains(movie.Description, component.Find("#Description").OuterHtml);
        }

        [Fact]
        public void NewMovie_FormTitleIsCorrect()
        {
            ParameterView paramView = ParameterView.FromDictionary(new Dictionary<string, object>() { { "movie", new Movie() }, { "IsNewMovie", true } });
            var component = host.AddComponent<MovieAddedit>(paramView);

            Assert.Contains("Add Movie", component.Find(".modal-title").InnerText);
        }

        [Fact]
        public void EditMovie_FormTitleIsCorrect()
        {
            ParameterView paramView = ParameterView.FromDictionary(new Dictionary<string, object>() { { "movie", new Movie() }, { "IsNewMovie", false } });
            var component = host.AddComponent<MovieAddedit>(paramView);

            Assert.Contains("Edit Movie", component.Find(".modal-title").InnerText);
        }
    }
}
