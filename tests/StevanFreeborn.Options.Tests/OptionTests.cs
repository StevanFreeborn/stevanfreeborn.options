using System.Globalization;

namespace StevanFreeborn.Options.Tests;

public class OptionTests
{
  [Test]
  public async Task Some_WhenCalledWithValue_ItShouldReturnSomeOption()
  {
    var option = Option.Some("test");

    await Assert.That(option.IsSome).IsTrue();
    await Assert.That(option.IsNone).IsFalse();
    await Assert.That(option.Value).IsEqualTo("test");
  }

  [Test]
  public async Task None_WhenCalled_ItShouldReturnNoneOption()
  {
    var option = Option.None<string>();

    await Assert.That(option.IsNone).IsTrue();
    await Assert.That(option.IsSome).IsFalse();
  }

  [Test]
  public async Task Some_WhenCalledWithNull_ItShouldThrowArgumentNullException()
  {
    Assert.Throws<ArgumentNullException>(() => _ = Option.Some<string>(null!));
  }

  [Test]
  public async Task From_WhenValueNotNull_ItShouldReturnSomeOption()
  {
    var option = Option.From("test");

    await Assert.That(option.IsSome).IsTrue();
    await Assert.That(option.Value).IsEqualTo("test");
  }

  [Test]
  public async Task From_WhenValueNull_ItShouldReturnNoneOption()
  {
    var option = Option.From<string>(null);

    await Assert.That(option.IsNone).IsTrue();
  }

  [Test]
  public async Task Value_WhenOptionIsNone_ItShouldThrowInvalidOperationException()
  {
    var option = Option.None<string>();

    Assert.Throws<InvalidOperationException>(() => _ = option.Value);
  }

  [Test]
  public async Task ImplicitConversion_FromValue_ItShouldReturnSomeOption()
  {
    Option<string> option = "test";

    await Assert.That(option.IsSome).IsTrue();
    await Assert.That(option.Value).IsEqualTo("test");
  }

  [Test]
  public async Task ImplicitConversion_FromNull_ItShouldReturnNoneOption()
  {
    string? nullString = null;
    Option<string> option = nullString;

    await Assert.That(option.IsNone).IsTrue();
  }

  [Test]
  public async Task Deconstruct_WhenSome_ItShouldReturnTrueAndValue()
  {
    var option = Option.Some("test");
    var (isSome, value) = option;

    await Assert.That(isSome).IsTrue();
    await Assert.That(value).IsEqualTo("test");
  }

  [Test]
  public async Task Deconstruct_WhenNone_ItShouldReturnFalseAndDefault()
  {
    var option = Option.None<string>();
    var (isSome, value) = option;

    await Assert.That(isSome).IsFalse();
    await Assert.That(value).IsNull();
  }

  [Test]
  public async Task Map_WhenSome_ItShouldTransformValue()
  {
    var option = Option.Some(5);
    var mapped = option.Map(x => x.ToString(CultureInfo.InvariantCulture));

    await Assert.That(mapped.IsSome).IsTrue();
    await Assert.That(mapped.Value).IsEqualTo("5");
  }

  [Test]
  public async Task Map_WhenNone_ItShouldReturnNone()
  {
    var option = Option.None<int>();
    var mapped = option.Map(x => x.ToString(CultureInfo.InvariantCulture));

    await Assert.That(mapped.IsNone).IsTrue();
  }

  [Test]
  public async Task Bind_WhenSome_ItShouldChainToBinderResult()
  {
    var option = Option.Some(5);
    var bound = option.Bind(x => Option.Some(x.ToString(CultureInfo.InvariantCulture)));

    await Assert.That(bound.IsSome).IsTrue();
    await Assert.That(bound.Value).IsEqualTo("5");
  }

  [Test]
  public async Task Match_WhenSome_ItShouldReturnOnSomeValue()
  {
    var option = Option.Some("test");
    var result = option.Match(v => v.Length, () => 0);

    await Assert.That(result).IsEqualTo(4);
  }

  [Test]
  public async Task Match_WhenNone_ItShouldReturnOnNoneValue()
  {
    var option = Option.None<string>();
    var result = option.Match(v => v.Length, () => -1);

    await Assert.That(result).IsEqualTo(-1);
  }

  [Test]
  public async Task Where_WhenPredicateMatches_ItShouldReturnSameOption()
  {
    var option = Option.Some(10);
    var filtered = option.Where(x => x > 5);

    await Assert.That(filtered).IsEqualTo(option);
  }

  [Test]
  public async Task Where_WhenPredicateDoesNotMatch_ItShouldReturnNone()
  {
    var option = Option.Some(3);
    var filtered = option.Where(x => x > 5);

    await Assert.That(filtered.IsNone).IsTrue();
  }

  [Test]
  public async Task GetValueOrDefault_WhenSome_ItShouldReturnValue()
  {
    var option = Option.Some("test");

    await Assert.That(option.GetValueOrDefault("default")).IsEqualTo("test");
  }

  [Test]
  public async Task GetValueOrDefault_WhenNone_ItShouldReturnDefault()
  {
    var option = Option.None<string>();

    await Assert.That(option.GetValueOrDefault("default")).IsEqualTo("default");
  }

  [Test]
  public async Task OrElse_WhenSome_ItShouldReturnSameOption()
  {
    var option = Option.Some("test");
    var result = option.OrElse(() => Option.Some("fallback"));

    await Assert.That(result).IsEqualTo(option);
  }

  [Test]
  public async Task OrElse_WhenNone_ItShouldReturnFallbackOption()
  {
    var option = Option.None<string>();
    var result = option.OrElse(() => Option.Some("fallback"));

    await Assert.That(result.IsSome).IsTrue();
    await Assert.That(result.Value).IsEqualTo("fallback");
  }
}