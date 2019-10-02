namespace Vestis.Core.Failures
{
    /// <summary>
    /// Represents a failed operation (of
    /// any kind) within Vestis. Implementing
    /// classes may provide additional information.
    /// </summary>
    public interface IFailure
    {
        /// <summary>
        /// Returns a unique key for this type of
        /// failure. Such key will be interpreted
        /// by the UI to display a proper user
        /// friendly error message.
        /// </summary>
        /// <returns></returns>
        string FailureKey { get; }
        /// <summary>
        /// Contains internal data, like stack traces
        /// or other information, useful for logs and
        /// other debug elements.
        /// </summary>
        string InternalData { get; }
    }
}
