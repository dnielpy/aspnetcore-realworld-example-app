using Conduit.Infrastructure;
using Xunit;

namespace Conduit.IntegrationTests;

public class SlugTests
{
    [Fact]
    public void Null_Returns_Null()
    {
        string? title = null;

        var slug = title.GenerateSlug();

        Assert.Null(slug);
    }

    [Fact]
    public void Uppercase_And_Punctuation_Are_Normalized()
    {
        var title = "Hello, World!";

        var slug = title.GenerateSlug();

        Assert.Equal("hello-world", slug);
    }

    [Theory]
    [InlineData("HELLO", "hello")]
    [InlineData("MiXeD CaSe", "mixed-case")]
    public void Text_Is_Lowercased(string title, string expected)
    {
        var slug = title.GenerateSlug();

        Assert.Equal(expected, slug);
    }

    [Theory]
    [InlineData("hello@world#", "helloworld")]
    [InlineData("c# & .net", "c-net")]
    [InlineData("already-hyphenated 123", "already-hyphenated-123")]
    public void Invalid_Characters_Are_Removed(string title, string expected)
    {
        var slug = title.GenerateSlug();

        Assert.Equal(expected, slug);
    }

    [Fact]
    public void Multiple_Spaces_Are_Normalized()
    {
        var title = "Hello     World";

        var slug = title.GenerateSlug();

        Assert.Equal("hello-world", slug);
    }

    [Fact]
    public void Leading_And_Trailing_Spaces_Are_Trimmed()
    {
        var title = "   hello world   ";

        var slug = title.GenerateSlug();

        Assert.Equal("hello-world", slug);
    }

    [Fact]
    public void Spaces_Are_Replaced_With_Hyphens()
    {
        var title = "one two three";

        var slug = title.GenerateSlug();

        Assert.Equal("one-two-three", slug);
    }

    [Fact]
    public void Long_Title_Is_Cut_To_45_Characters()
    {
        var title = new string('a', 60);

        var slug = title.GenerateSlug();

        Assert.Equal(new string('a', 45), slug);
    }

    [Fact]
    public void Cut_Landing_On_A_Space_Does_Not_Leave_A_Trailing_Hyphen()
    {
        var title = new string('a', 44) + " bbbbb";

        var slug = title.GenerateSlug();

        Assert.Equal(new string('a', 44), slug);
    }

    [Fact]
    public void Accented_Characters_Are_Removed()
    {
        var title = "Canción";

        var slug = title.GenerateSlug();
        Assert.Equal("cancin", slug);
    }
}
