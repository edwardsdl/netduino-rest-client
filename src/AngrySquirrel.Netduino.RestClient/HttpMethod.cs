namespace AngrySquirrel.Netduino.RestClient
{
    /// <summary>
    /// Represents a set of common HTTP methods
    /// </summary>
    public enum HttpMethod
    {
        /// <summary>
        /// Represents the lack of a method
        /// </summary>
        None, 

        /// <summary>
        /// Represents the DELETE method
        /// </summary>
        Delete, 

        /// <summary>
        /// Represents the GET method
        /// </summary>
        Get, 

        /// <summary>
        /// Represents the POST method
        /// </summary>
        Post, 

        /// <summary>
        /// Represents the PUT method
        /// </summary>
        Put
    }
}