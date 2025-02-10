using API.Models.Enum;
using API.Models;

namespace API.BL.Interface
{
    /// <summary>
    /// Interface for handling data operations.
    /// </summary>
    /// <typeparam name="T">Type of the DTO class.</typeparam>
    public interface IDataHandler<T> where T : class
    {
        /// <summary>
        /// Gets or sets the type of the operation.
        /// </summary>
        EnmType Type { get; set; }

        /// <summary>
        /// Prepares the object before saving.
        /// </summary>
        /// <param name="objDTO">The DTO object.</param>
        void PreSave(T objDTO);

        /// <summary>
        /// Validates the object before saving.
        /// </summary>
        /// <returns>The response of the validation.</returns>
        Response Validation();

        /// <summary>
        /// Saves the object.
        /// </summary>
        /// <returns>The response of the save operation.</returns>
        Response Save();
    }
}