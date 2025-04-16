using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace reLIFE.BusinessLogic.Data
{
    public static class DbHelper
    {
        private static readonly string _connectionString;

        // Static constructor: Executes once to read the configuration.
        static DbHelper()
        {
            try
            {
                // Build configuration to read appsettings.json
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                    .Build();

                // Read the specific connection string
                string? connectionStringValue = configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrWhiteSpace(connectionStringValue))
                {
                    throw new InvalidOperationException("Connection string 'DefaultConnection' is not found or is empty in appsettings.json.");
                }

                _connectionString = connectionStringValue;
            }
            catch (FileNotFoundException fnfEx)
            {
                Console.WriteLine($"Configuration Error: {fnfEx.Message}. Make sure 'appsettings.json' exists and 'Copy to Output Directory' is set.");
                throw new InvalidOperationException("Configuration file 'appsettings.json' not found.", fnfEx);
            }
            catch (Exception ex) // Catch other errors during configuration building/reading
            {
                Console.WriteLine($"Unexpected Error during DbHelper static initialization: {ex.Message}");
                throw new InvalidOperationException("Failed to initialize DbHelper due to an unexpected error reading configuration.", ex);
            }
        }

        /// <summary>
        /// Gets the configured database connection string read from appsettings.json.
        /// </summary>
        /// <returns>The connection string.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the connection string was not successfully loaded during initialization.</exception>
        public static string GetConnectionString()
        {
            // Static constructor ensures this is non-null if initialization succeeded
            return _connectionString;
        }

        // REMOVED: GetOpenConnection() method is removed as the helper
        //          is no longer responsible for creating connections.
        //          Repositories will handle connection creation using the
        //          string provided via their constructor.

        // REMOVED: Optional parameter helpers could still exist if desired,
        //          but they are not related to connection management.
    }
}