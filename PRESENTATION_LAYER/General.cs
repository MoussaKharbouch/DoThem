using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BUSINESS_LAYER;

namespace PRESENTATION_LAYER
{
    /// <summary>
    /// Static class that holds global application data
    /// Maintains the currently logged-in user throughout the application session
    /// This class acts as a bridge between the presentation layer and business logic
    /// </summary>
    static internal class General
    {
        /// <summary>
        /// Represents the current authenticated user in the application
        /// Initialized with empty values and updated upon successful login
        /// Accessed globally throughout the application to identify the active user
        /// and retrieve user-specific data like tasks and task types
        /// </summary>
        static internal User User = new User();
    }
}