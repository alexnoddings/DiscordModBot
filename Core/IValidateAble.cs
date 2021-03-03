using Elvet.Core.Config.Exceptions;

namespace Elvet.Core
{
    /// <summary>
    /// Represents something which, when validated, produces a <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The type produced when validated.</typeparam>
    /// <remarks>
    /// <para>Intended for use in chaining validation when getting a Dependency-Injected service. Example:</para>
    /// <example>
    /// <code>
    /// class ExampleConfig : IValidateAble&lt;ExampleConfig&gt; { ... }
    /// <br />
    /// <br />
    /// services.GetRequiredService&lt;IConfiguration&gt;().GetRequiredSection&lt;ExampleConfig&gt;("example").Validate()
    /// </code>
    /// </example>
    /// </remarks>
    public interface IValidateAble<out T>
    {
        /// <summary>
        /// Validates this object.
        /// </summary>
        /// <exception cref="BadConfigurationException">Thrown when validation fails.</exception>
        /// <returns>A valid <typeparamref name="T" />.</returns>
        public T Validate();
    }
}
