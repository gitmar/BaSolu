using BlazorBootstrap;

using Bunit;
using Xunit;

using GxShared.Others;
using GxTie.Components.FrmHelper;

namespace GxTie.MyApp.Tests
{
    public class SelectedDisplayTests
    {
        [Fact]
        public void RendersOptionsAndUpdatesValue()
        {
            using var ctx = new TestContext(); // ✅ create context

            // Arrange: fake options
            var options = new List<Gptbl>
            {
                new Gptbl { Elea = 1, Liba = "France" },
                new Gptbl { Elea = 2, Liba = "Burkina Faso" }
            };

            int? value = 1;

            // Act: render component
            var cut = ctx.Render<SelectedDisplay<int?>>(
                parameters => parameters
                    .Add(p => p.isEditing, true)
                    .Add(p => p.Value, value)
                    .Add(p => p.ValueChanged, v => value = v)
                    .Add(p => p.Options, options)
                    .Add(p => p.GetOptionValue, g => g.Elea ?? 0)
                    .Add(p => p.GetOptionLabel, g => g.Liba ?? "")
            );

            // Assert: initial render shows France
            cut.MarkupMatches(@"
<select name=""Value"" class=""form-select"" value=""1"">
  <option value="""">?choisir</option>
  <option value=""1"" selected>France</option>
  <option value=""2"">Burkina Faso</option>
</select>");


            // Act: simulate user selecting Burkina Faso
            cut.Find("select").Change("2");

            // Assert: bound value updated
            Assert.Equal(2, value);
        }
    }
}
