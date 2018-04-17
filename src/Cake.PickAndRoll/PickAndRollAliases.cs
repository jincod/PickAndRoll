using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using PickAndRoll.Models;

namespace Cake.PickAndRoll
{
    /// <summary>
    ///     Keep environment specific attributes in separate configuration files and build desired config on demand.
    /// </summary>
    [CakeAliasCategory("Configuration")]
    [CakeNamespaceImport("PickAndRoll")]
    public static class PickAndRollAliases
    {
        /// <summary>
        /// Make configs inside project root
        /// </summary>
        /// <example>
        /// <code>
        /// Task("PickAndRoll")
        ///   .Does(() => {
        ///     PickAndRoll();
        /// });
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="files">Custom file list for processing</param>
        /// <param name="parConfigFileName">Custom parconfig filename</param>
        /// <param name="configFileName">Custom config filename</param>
        /// <param name="extraConfigFiles">Extra config filenames</param>
        /// <param name="pwd">Custom working directory. Default current</param>
        [CakeMethodAlias]
        public static void PickAndRoll(
            this ICakeContext context,
            IEnumerable<string> files = null,
            string parConfigFileName = null,
            string configFileName = null,
            IEnumerable<string> extraConfigFiles = null,
            string pwd = null
        )
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            try
            {
                void Logger(string message) => context.Log.Information(message);
                var pickAndRoll = new global::PickAndRoll.PickAndRoll(Logger);

                pickAndRoll.Go(new PickAndRollSettings
                {
                    ConfigFileName = configFileName,
                    ParConfigFileName = parConfigFileName,
                    ExtraConfigsPath = extraConfigFiles,
                    Files = files,
                    Pwd = pwd
                });
            }
            catch (Exception e)
            {
                context.Log.Error(e.Message);
                context.Log.Error(e.StackTrace);
                throw;
            }
        }
    }
}