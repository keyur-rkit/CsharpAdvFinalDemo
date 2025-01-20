using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace API.Extensions
{
    public static class DTOToPOCO
    {
        public static POCO Convert<POCO>(this object objDTO)
        {
            // Get the type of the POCO
            Type pocoType = typeof(POCO);

            // Create a new instance of the POCO type
            POCO pocoInstance = (POCO)Activator.CreateInstance(pocoType);

            // Get the properties of the DTO object
            PropertyInfo[] dtoProperties = objDTO.GetType().GetProperties();

            // Get the properties of the POCO type
            PropertyInfo[] pocoProperties = pocoType.GetProperties();

            // Iterate through each property in the DTO
            foreach (PropertyInfo dtoProperty in dtoProperties)
            {
                // Find the corresponding property in the POCO with the same name
                PropertyInfo pocoProperty = Array.Find(pocoProperties, p => p.Name == dtoProperty.Name);

                // If the matching property is found and their types are compatible, copy the value
                if (pocoProperty != null && dtoProperty.PropertyType == pocoProperty.PropertyType)
                {
                    // Get the value from the DTO property
                    object value = dtoProperty.GetValue(objDTO);

                    // Set the value in the POCO property
                    pocoProperty.SetValue(pocoInstance, value);
                }
            }

            // Return the populated POCO instance
            return pocoInstance;
        }
    }
}