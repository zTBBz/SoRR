using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace SoRR
{
    /// <summary>
    ///   <para>Provides a set of extension methods for <see cref="Task{T}"/> and related types.</para>
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        ///   <para>Transitions the <paramref name="source"/>'s underlying task to a completion state corresponding to the result of the specified <paramref name="func"/>.</para>
        /// </summary>
        /// <typeparam name="T">The type of the result produced by the task.</typeparam>
        /// <param name="source">The task completion source to transition to a completion state.</param>
        /// <param name="func">The function whose result should be used as a completion state for the <paramref name="source"/>'s underlying task.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="func"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">The <paramref name="source"/>'s underlying task has already completed.</exception>
        public static void SetFromFunc<T>(this TaskCompletionSource<T> source, [InstantHandle] Func<T> func)
        {
            const string msg = "An attempt was made to transition a task to a final state when it had already completed.";
            if (!TrySetFromFunc(source, func)) throw new InvalidOperationException(msg);
        }
        /// <summary>
        ///   <para>Attempts to transition the <paramref name="source"/>'s underlying task to a completion state corresponding to the result of the specified <paramref name="func"/>.</para>
        /// </summary>
        /// <typeparam name="T">The type of the result produced by the task.</typeparam>
        /// <param name="source">The task completion source to transition to a completion state.</param>
        /// <param name="func">The function whose result should be used as a completion state for the <paramref name="source"/>'s underlying task.</param>
        /// <returns><see langword="true"/>, if the operation was successful; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="func"/> is <see langword="null"/>.</exception>
        [MustUseReturnValue]
        public static bool TrySetFromFunc<T>(this TaskCompletionSource<T> source, [InstantHandle] Func<T> func)
        {
            Guard.ThrowIfNull(source);
            Guard.ThrowIfNull(func);

            if (source.Task.IsCompleted) return false;

            T result;
            try
            {
                result = func();
            }
            catch (Exception ex)
            {
                return source.TrySetException(ex);
            }
            return source.TrySetResult(result);
        }

    }
}
