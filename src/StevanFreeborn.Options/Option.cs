namespace StevanFreeborn.Options;

/// <summary>
/// Provides factory methods for creating <see cref="Option{T}"/>.
/// </summary>
#pragma warning disable CA1716 // Identifiers should not match keywords
public static class Option
#pragma warning restore CA1716 // Identifiers should not match keywords
{
  /// <summary>
  /// Creates an option with the specified value.
  /// </summary>
  /// <typeparam name="T">The type of the value.</typeparam>
  /// <param name="value">The value.</param>
  /// <returns>An <see cref="Option{T}"/> with the specified value.</returns>
  public static Option<T> Some<T>(T value)
  {
#if NET6_0_OR_GREATER
    ArgumentNullException.ThrowIfNull(value);
#else
    if (value is null)
    {
      throw new ArgumentNullException(nameof(value));
    }
#endif
    return new Option<T>(true, value);
  }

  /// <summary>
  /// Creates an empty option.
  /// </summary>
  /// <typeparam name="T">The type of the value.</typeparam>
  /// <returns>An empty <see cref="Option{T}"/>.</returns>
  public static Option<T> None<T>() => new(false, default!);

  /// <summary>
  /// Creates an option from the specified value, returning None if the value is null.
  /// </summary>
  /// <typeparam name="T">The type of the value.</typeparam>
  /// <param name="value">The value.</param>
  /// <returns>An <see cref="Option{T}"/> with the value if not null, otherwise None.</returns>
  public static Option<T> From<T>(T? value) => value is null ? None<T>() : Some(value);

  /// <summary>
  /// Converts a value to an <see cref="Option{T}"/>.
  /// </summary>
  /// <typeparam name="T">The type of the value.</typeparam>
  /// <param name="value">The value.</param>
  /// <returns>An <see cref="Option{T}"/>.</returns>
  public static Option<T> FromT<T>(T? value) => From(value);
}

/// <summary>
/// Represents an optional value that may or may not be present.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
#pragma warning disable CA1716 // Identifiers should not match keywords
public sealed class Option<T>
#pragma warning restore CA1716 // Identifiers should not match keywords
{
  private readonly T _value;

  /// <summary>
  /// Gets a value indicating whether the option has a value.
  /// </summary>
  public bool IsSome { get; }

  /// <summary>
  /// Gets a value indicating whether the option is empty.
  /// </summary>
  public bool IsNone => !IsSome;

  /// <summary>
  /// Gets the value of the option.
  /// </summary>
  /// <exception cref="InvalidOperationException">Thrown when accessing Value on a None option.</exception>
  public T Value => IsSome
    ? _value
    : throw new InvalidOperationException("Cannot access Value on a None option.");

  internal Option(bool isSome, T value)
  {
    IsSome = isSome;
    _value = value;
  }

  /// <summary>
  /// Implicitly converts a value to an <see cref="Option{T}"/>.
  /// </summary>
  /// <param name="value">The value to convert.</param>
#pragma warning disable CA2225 // Operator overloads have named alternates
  public static implicit operator Option<T>(T? value) => Option.From(value);
#pragma warning restore CA2225 // Operator overloads have named alternates

  /// <summary>
  /// Deconstructs the option into its components.
  /// </summary>
  /// <param name="isSome">Indicates whether the option has a value.</param>
  /// <param name="value">The value of the option.</param>
  public void Deconstruct(out bool isSome, out T value)
  {
    isSome = IsSome;
    value = _value;
  }

  /// <summary>
  /// Maps the value to a new type if the option has a value.
  /// </summary>
  /// <typeparam name="TNew">The new type.</typeparam>
  /// <param name="mapper">The function to map the value.</param>
  /// <returns>A new <see cref="Option{TNew}"/> with the mapped value if Some, otherwise None.</returns>
  /// <exception cref="ArgumentNullException">Thrown when mapper is null.</exception>
  public Option<TNew> Map<TNew>(Func<T, TNew> mapper)
  {
#if NET6_0_OR_GREATER
    ArgumentNullException.ThrowIfNull(mapper);
#else
    if (mapper is null)
    {
      throw new ArgumentNullException(nameof(mapper));
    }
#endif

    return IsSome ? Option.From(mapper(_value)) : Option.None<TNew>();
  }

  /// <summary>
  /// Binds to a new option if the current option has a value.
  /// </summary>
  /// <typeparam name="TNew">The new option type.</typeparam>
  /// <param name="binder">The function to bind to on Some.</param>
  /// <returns>The result of the binder function if Some, otherwise None.</returns>
  /// <exception cref="ArgumentNullException">Thrown when binder is null.</exception>
  public Option<TNew> Bind<TNew>(Func<T, Option<TNew>> binder)
  {
#if NET6_0_OR_GREATER
    ArgumentNullException.ThrowIfNull(binder);
#else
    if (binder is null)
    {
      throw new ArgumentNullException(nameof(binder));
    }
#endif

    return IsSome ? binder(_value) : Option.None<TNew>();
  }

  /// <summary>
  /// Matches the option and returns a value based on whether it has a value.
  /// </summary>
  /// <typeparam name="TResult">The type of the result.</typeparam>
  /// <param name="onSome">The function to execute on Some.</param>
  /// <param name="onNone">The function to execute on None.</param>
  /// <returns>The result of the appropriate function.</returns>
  /// <exception cref="ArgumentNullException">Thrown when onSome or onNone is null.</exception>
  public TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone)
  {
#if NET6_0_OR_GREATER
    ArgumentNullException.ThrowIfNull(onSome);
    ArgumentNullException.ThrowIfNull(onNone);
#else
    if (onSome is null)
    {
      throw new ArgumentNullException(nameof(onSome));
    }

    if (onNone is null)
    {
      throw new ArgumentNullException(nameof(onNone));
    }
#endif

    return IsSome ? onSome(_value) : onNone();
  }

  /// <summary>
  /// Matches the option and executes the appropriate action.
  /// </summary>
  /// <param name="onSome">The action to execute on Some.</param>
  /// <param name="onNone">The action to execute on None.</param>
  /// <exception cref="ArgumentNullException">Thrown when onSome or onNone is null.</exception>
  public void Match(Action<T> onSome, Action onNone)
  {
#if NET6_0_OR_GREATER
    ArgumentNullException.ThrowIfNull(onSome);
    ArgumentNullException.ThrowIfNull(onNone);
#else
    if (onSome is null)
    {
      throw new ArgumentNullException(nameof(onSome));
    }

    if (onNone is null)
    {
      throw new ArgumentNullException(nameof(onNone));
    }
#endif

    if (IsSome)
    {
      onSome(_value);
    }
    else
    {
      onNone();
    }
  }

  /// <summary>
  /// Filters the option based on the specified predicate.
  /// </summary>
  /// <param name="predicate">The predicate to filter with.</param>
  /// <returns>The current option if Some and the predicate is met, otherwise None.</returns>
  /// <exception cref="ArgumentNullException">Thrown when predicate is null.</exception>
  public Option<T> Where(Func<T, bool> predicate)
  {
#if NET6_0_OR_GREATER
    ArgumentNullException.ThrowIfNull(predicate);
#else
    if (predicate is null)
    {
      throw new ArgumentNullException(nameof(predicate));
    }
#endif

    if (IsNone)
    {
      return this;
    }

    return predicate(_value) ? this : Option.None<T>();
  }

  /// <summary>
  /// Gets the value of the option if Some, otherwise the specified default value.
  /// </summary>
  /// <param name="defaultValue">The default value.</param>
  /// <returns>The value if Some, otherwise the default value.</returns>
  public T GetValueOrDefault(T defaultValue) => IsSome ? _value : defaultValue;

  /// <summary>
  /// Gets the value of the option if Some, otherwise the result of the specified factory function.
  /// </summary>
  /// <param name="factory">The factory function to provide the default value.</param>
  /// <returns>The value if Some, otherwise the result of the factory function.</returns>
  /// <exception cref="ArgumentNullException">Thrown when factory is null.</exception>
  public T GetValueOrDefault(Func<T> factory)
  {
#if NET6_0_OR_GREATER
    ArgumentNullException.ThrowIfNull(factory);
#else
    if (factory is null)
    {
      throw new ArgumentNullException(nameof(factory));
    }
#endif

    return IsSome ? _value : factory();
  }

  /// <summary>
  /// Returns the current option if Some, otherwise the result of the specified factory function.
  /// </summary>
  /// <param name="factory">The factory function to provide the fallback option.</param>
  /// <returns>The current option if Some, otherwise the result of the factory function.</returns>
  /// <exception cref="ArgumentNullException">Thrown when factory is null.</exception>
  public Option<T> OrElse(Func<Option<T>> factory)
  {
#if NET6_0_OR_GREATER
    ArgumentNullException.ThrowIfNull(factory);
#else
    if (factory is null)
    {
      throw new ArgumentNullException(nameof(factory));
    }
#endif
    return IsSome ? this : factory();
  }
}