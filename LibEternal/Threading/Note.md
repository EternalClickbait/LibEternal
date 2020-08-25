I got all of this stuff from https://stackoverflow.com/a/58470597 (by aybe at 22/8/19)



>Here's why you need a pseudo dispatcher in Unity:
>
>In Unity, most objects can only be created from within main Unity thread.
>
>But suppose you have some heavy task you'd like to run in background such as with Task.Run, in your task you wouldn't be able to instantiate such objects as aforementioned but still would like to.
>
>There are quite a couple of solutions in solving this problem but they all leverage the same thing:
>Capturing Unity's synchronization context and posting messages to it
>Here's an original way in doing so, inspired from Raymond Chen's The Old New Thing:
>
>C++/WinRT envy: Bringing thread switching tasks to C# (WPF and WinForms edition)
>
>The concept is the following: switch to a specific thread at any time in a method !
>
>
>**Public types**
>
>IThreadSwitcher:
>
>```c#
>using System.Runtime.CompilerServices;
>using JetBrains.Annotations;
>
>namespace Threading
>{
>    /// <summary>
>    ///     Defines an object that switches to a thread.
>    /// </summary>
>    [PublicAPI]
>    public interface IThreadSwitcher : INotifyCompletion
>    {
>        bool IsCompleted { get; }
>
>        IThreadSwitcher GetAwaiter();
>
>        void GetResult();
>    }
>}
>```
>
>ThreadSwitcher:
>```c#
>using Threading.Internal;
>
>namespace Threading
>{
>    /// <summary>
>    ///     Switches to a particular thread.
>    /// </summary>
>    public static class ThreadSwitcher
>    {
>        /// <summary>
>        ///     Switches to the Task thread.
>        /// </summary>
>        /// <returns></returns>
>        public static IThreadSwitcher ResumeTaskAsync()
>        {
>            return new ThreadSwitcherTask();
>        }
>
>        /// <summary>
>        ///     Switch to the Unity thread.
>        /// </summary>
>        /// <returns></returns>
>        public static IThreadSwitcher ResumeUnityAsync()
>        {
>            return new ThreadSwitcherUnity();
>        }
>    }
>}
>
>```
>**Private types**
>
>ThreadSwitcherTask:
>```c#
>using System;
>using System.Threading;
>using System.Threading.Tasks;
>using JetBrains.Annotations;
>
>namespace Threading.Internal
>{
>    internal struct ThreadSwitcherTask : IThreadSwitcher
>    {
>        public IThreadSwitcher GetAwaiter()
>        {
>            return this;
>        }
>
>        public bool IsCompleted => SynchronizationContext.Current == null;
>
>        public void GetResult()
>        {
>        }
>
>        public void OnCompleted([NotNull] Action continuation)
>        {
>            if (continuation == null)
>                throw new ArgumentNullException(nameof(continuation));
>
>            Task.Run(continuation);
>        }
>    }
>}
>```
>
>ThreadSwitcherUnity:
>```c#
>using System;
>using System.Threading;
>using JetBrains.Annotations;
>
>namespace Threading.Internal
>{
>    internal struct ThreadSwitcherUnity : IThreadSwitcher
>    {
>        public IThreadSwitcher GetAwaiter()
>        {
>            return this;
>        }
>
>        public bool IsCompleted => SynchronizationContext.Current == UnityThread.Context;
>
>        public void GetResult()
>        {
>        }
>
>        public void OnCompleted([NotNull] Action continuation)
>        {
>            if (continuation == null)
>                throw new ArgumentNullException(nameof(continuation));
>
>            UnityThread.Context.Post(s => continuation(), null);
>        }
>    }
>}
>```
>
>UnityThread:
>```c#
>using System.Threading;
>using UnityEngine;
>#if UNITY_EDITOR
>using UnityEditor;
>
>#endif
>
>namespace Threading.Internal
>{
>    internal static class UnityThread
>    {
>#pragma warning disable IDE0032 // Use auto property
>        private static SynchronizationContext _context;
>#pragma warning restore IDE0032 // Use auto property
>
>        public static SynchronizationContext Context => _context;
>
>#if UNITY_EDITOR
>        [InitializeOnLoadMethod]
>#endif
>        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
>        private static void Capture()
>        {
>            _context = SynchronizationContext.Current;
>        }
>    }
>}
>```
>
>**Example**
>
>Albeit it is an exotic approach it has huge advantages, namely that by using a single call you can do work in different threads in the same method. The following code is executed with Task.Run but won't produce any errors while instantiating Unity objects since it's done in the right thread.
>```c#
>private static async Task DoWork(CancellationToken token)
>{
>    token.ThrowIfCancellationRequested();
>
>    var gameObjects = new List<GameObject>();
>
>    await ThreadSwitcher.ResumeUnityAsync();
>
>    for (var i = 0; i < 25; i++)
>    {
>        if (token.IsCancellationRequested)
>            token.ThrowIfCancellationRequested();
>
>        await Task.Delay(125, token);
>
>        var gameObject = new GameObject(i.ToString());
>
>        gameObjects.Add(gameObject);
>    }
>}
>```
>
>Now it's up to you to finely slice your work since by nature Unity synchronization context is not meant to run heavy computation, rather, just instantiate stuff you wouldn't be able to from another thread.
>
>A simple example would be generating some procedural mesh:
>
>    do all your maths in your task and produce enough data to create a mesh
>        i.e. vertices, normals, colors, uvs
>    switch to Unity thread
>        create a mesh from this data, PERIOD, this will be fast enough to be non-perceptible
>
>That was an interesting question, I hope I've answered it !