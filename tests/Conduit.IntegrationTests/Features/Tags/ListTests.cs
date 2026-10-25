using System.Threading.Tasks;
using Conduit.Domain;
using Conduit.Features.Articles;
using Conduit.IntegrationTests.Features.Articles;
using Xunit;
using TagsList = Conduit.Features.Tags.List;

namespace Conduit.IntegrationTests.Features.Tags;

public class ListTests : SliceFixture
{
    [Fact]
    public async Task Expect_List_Tags_Only_Returns_Tags_With_Articles()
    {
        await InsertAsync(new Tag { TagId = "orphan" });
        await ArticleHelpers.CreateArticle(
            this,
            new Create.Command(
                new Create.ArticleData
                {
                    Title = "Test tags list",
                    Description = "Description",
                    Body = "Body",
                    TagList = ["used2", "used1"],
                }
            )
        );

        var result = await SendAsync(new TagsList.Query());

        Assert.Equal(["used1", "used2"], result.Tags);
    }

    [Fact]
    public async Task Expect_List_Tags_Returns_Empty_When_No_Tag_Has_Articles()
    {
        await InsertAsync(new Tag { TagId = "orphan1" }, new Tag { TagId = "orphan2" });

        var result = await SendAsync(new TagsList.Query());

        Assert.Empty(result.Tags);
    }
}
