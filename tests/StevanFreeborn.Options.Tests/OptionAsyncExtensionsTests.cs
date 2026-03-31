using System.Globalization;

namespace StevanFreeborn.Options.Tests;

public class OptionAsyncExtensionsTests
{
  [Test]
  public async Task MapAsync_WhenSome_ItShouldTransformValue()
  {
    var option = Option.Some(5);
    var mapped = await option.MapAsync(x => Task.FromResult(x.ToString(CultureInfo.InvariantCulture)));

    await Assert.That(mapped.IsSome).IsTrue();
    await Assert.That(mapped.Value).IsEqualTo("5");
  }

  [Test]
  public async Task BindAsync_WhenSome_ItShouldChainToBinderResult()
  {
    var option = Option.Some(5);
    var bound = await option.BindAsync(x => Task.FromResult(Option.Some(x.ToString(CultureInfo.InvariantCulture))));

    await Assert.That(bound.IsSome).IsTrue();
    await Assert.That(bound.Value).IsEqualTo("5");
  }

  [Test]
  public async Task MatchAsync_WhenSome_ItShouldInvokeOnSome()
  {
    var option = Option.Some("test");
    var result = await option.MatchAsync(v => Task.FromResult(v.Length), () => Task.FromResult(0));

    await Assert.That(result).IsEqualTo(4);
  }

  [Test]
  public async Task WhereAsync_WhenPredicateMatches_ItShouldReturnSameOption()
  {
    var option = Option.Some(10);
    var filtered = await option.WhereAsync(x => Task.FromResult(x > 5));

    await Assert.That(filtered).IsEqualTo(option);
  }

  [Test]
  public async Task OrElseAsync_WhenNone_ItShouldReturnFallbackOption()
  {
    var option = Option.None<string>();
    var result = await option.OrElseAsync(() => Task.FromResult(Option.Some("fallback")));

    await Assert.That(result.IsSome).IsTrue();
    await Assert.That(result.Value).IsEqualTo("fallback");
  }
}