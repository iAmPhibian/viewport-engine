namespace ViewportEngine.Util;

/// <summary>
/// Represents a method that handles general events.
/// </summary>
/// <typeparam name="TSender">The type emitting the event</typeparam>
/// <typeparam name="TResult">The type of the event data</typeparam>
public delegate void TypedEventHandler<TSender,TResult>();