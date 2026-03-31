namespace StevanFreeborn.Options
{
  /// <summary>
  /// Provides async extension methods for <see cref="Option{T}"/>.
  /// </summary>
  public static class OptionAsyncExtensions
  {
    /// <summary>
    /// Maps the value to a new type asynchronously if the option has a value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <typeparam name="TNew">The new type.</typeparam>
    /// <param name="option">The option.</param>
    /// <param name="mapper">The async function to map the value.</param>
    /// <returns>A task containing a new <see cref="Option{TNew}"/> with the mapped value if Some, otherwise None.</returns>
    /// <exception cref="ArgumentNullException">Thrown when option or mapper is null.</exception>
    public static async Task<Option<TNew>> MapAsync<T, TNew>(this Option<T> option, Func<T, Task<TNew>> mapper)
    {
#if NET6_0_OR_GREATER
      ArgumentNullException.ThrowIfNull(option);
      ArgumentNullException.ThrowIfNull(mapper);
#else
      if (option is null)
      {
        throw new ArgumentNullException(nameof(option));
      }

      if (mapper is null)
      {
        throw new ArgumentNullException(nameof(mapper));
      }
#endif

      return option.IsSome ? await mapper(option.Value).ConfigureAwait(false) : Option.None<TNew>();
    }

    /// <summary>
    /// Binds to a new async result if the current option has a value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <typeparam name="TNew">The new result type.</typeparam>
    /// <param name="option">The option.</param>
    /// <param name="binder">The async function to bind to on Some.</param>
    /// <returns>A task containing the result of the binder function if Some, otherwise None.</returns>
    /// <exception cref="ArgumentNullException">Thrown when option or binder is null.</exception>
    public static async Task<Option<TNew>> BindAsync<T, TNew>(this Option<T> option, Func<T, Task<Option<TNew>>> binder)
    {
#if NET6_0_OR_GREATER
      ArgumentNullException.ThrowIfNull(option);
      ArgumentNullException.ThrowIfNull(binder);
#else
      if (option is null)
      {
        throw new ArgumentNullException(nameof(option));
      }

      if (binder is null)
      {
        throw new ArgumentNullException(nameof(binder));
      }
#endif

      return option.IsSome ? await binder(option.Value).ConfigureAwait(false) : Option.None<TNew>();
    }

    /// <summary>
    /// Matches the option asynchronously and returns a value based on whether it has a value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="option">The option.</param>
    /// <param name="onSome">The async function to execute on Some.</param>
    /// <param name="onNone">The async function to execute on None.</param>
    /// <returns>A task containing the result of the appropriate function.</returns>
    /// <exception cref="ArgumentNullException">Thrown when option, onSome, or onNone is null.</exception>
    public static async Task<TResult> MatchAsync<T, TResult>(this Option<T> option, Func<T, Task<TResult>> onSome, Func<Task<TResult>> onNone)
    {
#if NET6_0_OR_GREATER
      ArgumentNullException.ThrowIfNull(option);
      ArgumentNullException.ThrowIfNull(onSome);
      ArgumentNullException.ThrowIfNull(onNone);
#else
      if (option is null)
      {
        throw new ArgumentNullException(nameof(option));
      }

      if (onSome is null)
      {
        throw new ArgumentNullException(nameof(onSome));
      }

      if (onNone is null)
      {
        throw new ArgumentNullException(nameof(onNone));
      }
#endif

      return option.IsSome ? await onSome(option.Value).ConfigureAwait(false) : await onNone().ConfigureAwait(false);
    }

    /// <summary>
    /// Filters the option asynchronously based on the specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="option">The option.</param>
    /// <param name="predicate">The async predicate to filter with.</param>
    /// <returns>A task containing the current option if Some and the predicate is met, otherwise None.</returns>
    /// <exception cref="ArgumentNullException">Thrown when option or predicate is null.</exception>
    public static async Task<Option<T>> WhereAsync<T>(this Option<T> option, Func<T, Task<bool>> predicate)
    {
#if NET6_0_OR_GREATER
      ArgumentNullException.ThrowIfNull(option);
      ArgumentNullException.ThrowIfNull(predicate);
#else
      if (option is null)
      {
        throw new ArgumentNullException(nameof(option));
      }

      if (predicate is null)
      {
        throw new ArgumentNullException(nameof(predicate));
      }
#endif

      if (option.IsNone)
      {
        return option;
      }

      return await predicate(option.Value).ConfigureAwait(false) ? option : Option.None<T>();
    }

    /// <summary>
    /// Returns the current option if Some, otherwise the result of the specified async factory function.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="option">The option.</param>
    /// <param name="factory">The async factory function to provide the fallback option.</param>
    /// <returns>A task containing the current option if Some, otherwise the result of the factory function.</returns>
    /// <exception cref="ArgumentNullException">Thrown when option or factory is null.</exception>
    public static async Task<Option<T>> OrElseAsync<T>(this Option<T> option, Func<Task<Option<T>>> factory)
    {
#if NET6_0_OR_GREATER
      ArgumentNullException.ThrowIfNull(option);
      ArgumentNullException.ThrowIfNull(factory);
#else
      if (option is null)
      {
        throw new ArgumentNullException(nameof(option));
      }

      if (factory is null)
      {
        throw new ArgumentNullException(nameof(factory));
      }
#endif

      return option.IsSome ? option : await factory().ConfigureAwait(false);
    }
  }
}