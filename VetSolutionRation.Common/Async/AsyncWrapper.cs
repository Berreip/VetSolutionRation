using System.Diagnostics;
using PRF.Utils.CoreComponents.Async;
using PRF.Utils.CoreComponents.Diagnostic;

// ReSharper disable UnusedType.Global

namespace VetSolutionRation.Common.Async
{
    public static class AsyncWrapper
    {
        /// <summary>
        /// Create a task and invoke a callback with a try catch
        /// </summary>
        public static async Task DispatchAndWrapAsync(Action callback, Action? onfinally = null)
        {
            await AsyncWrapperBase.DispatchAndWrapAsyncBase(callback, OnException, onfinally).ConfigureAwait(false);
        }

        /// <summary>
        /// Create a task and invoke a callback with a try catch. 
        /// </summary>
        public static async Task<T> DispatchAndWrapAsync<T>(Func<T> callback, Action? onfinally = null)
        {
            return await AsyncWrapperBase.DispatchAndWrapAsyncBase(callback, OnException, onfinally).ConfigureAwait(false);
        }

        /// <summary>
        /// Create a task and invoke a callback with a try catch
        /// </summary>
        public static async Task DispatchAndWrapAsync(Func<Task> callback, Action? onfinally = null)
        {
            await AsyncWrapperBase.DispatchAndWrapAsyncBase(callback, OnException, onfinally).ConfigureAwait(false);
        }

        /// <summary>
        /// Create a task and invoke a callback with a try catch
        /// </summary>
        public static async Task<T> DispatchAndWrapAsync<T>(Func<Task<T>> callback, Action? onfinally = null)
        {
            return await AsyncWrapperBase.DispatchAndWrapAsyncBase(callback, OnException, onfinally).ConfigureAwait(false);
        }

        /// <summary>
        /// Create a task and invoke a callback with a try catch
        /// </summary>
        public static async void DispatchAndWrapInFireAndForget(Action callback, Action? onfinally = null)
        {
            await AsyncWrapperBase.DispatchAndWrapAsyncBase(callback, OnException, onfinally).ConfigureAwait(false);
        }

        /// <summary>
        /// Create a task and invoke a callback with a try catch
        /// </summary>
        public static async void DispatchAndWrapInFireAndForget(Func<Task> callback, Action? onfinally = null)
        {
            await AsyncWrapperBase.DispatchAndWrapAsyncBase(callback, OnException, onfinally).ConfigureAwait(false);
        }

        /// <summary>
        /// Create a task and invoke a callback with a try catch
        /// </summary>
        public static async Task WrapAsync(Func<Task> callback, Action? onfinally = null)
        {
            await callback.WrapAsync(OnException, onfinally).ConfigureAwait(false);
        }

        /// <summary>
        /// Create a task and invoke a callback with a try catch
        /// </summary>
        public static async Task<T> WrapAsync<T>(Func<Task<T>> callback, Action? onfinally = null)
        {
            return await callback.WrapAsync(OnException, onfinally).ConfigureAwait(false);
        }

        public static void Wrap(Action callback)
        {
            try
            {
                callback.Invoke();
            }
            catch (Exception e)
            {
                OnException(e);
            }
        }
        
        public static T? Wrap<T>(Func<T?> callback)
        {
            try
            {
                return callback.Invoke();
            }
            catch (Exception e)
            {
                OnException(e);
                return default;
            }
        }
        
        private static void OnException(Exception e)
        {
            ErrorHandler.HandleError($"Error in AsyncWrapper: {e}");
        }
    }
}