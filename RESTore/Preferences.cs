using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RESTore
{
    /// <summary>
    /// Preferences loaded from the App.config file
    /// </summary>
    public static class Preferences
    {
        /// <summary>
        /// List of settings from the App.config as a KeyValue collection
        /// </summary>
        static internal KeyValueConfigurationCollection _kvList;

        /// <summary>
        /// The reader for the config file
        /// </summary>
        static internal ConfigReader _reader;

        /// <summary>
        /// The app settings section of the App.config file
        /// </summary>
        static internal AppSettingsSection _appSettings;

        /// <summary>
        /// The adress of the proxy server
        /// </summary>
        public static string ProxyAddress { get; set; }

        /// <summary>
        /// Whether to use a proxy server
        /// </summary>
        public static string UseProxy { get; set; }


        /// <summary>
        /// Internal static constructor
        /// </summary>
        static Preferences()
        {
            GetDriverReader();
        }


        /// <summary>
        /// Loads the App.config file and populates the Preferences
        /// </summary>
        /// <returns>A config reader</returns>
        static private ConfigReader GetDriverReader()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            _reader = new ConfigReader(Path.GetDirectoryName(assembly.Location) + @"\RESTore.dll.config");
            _reader.GetAppSettings();
            _appSettings = _reader.AppSettings;
            _kvList = _appSettings.Settings;
            UseProxy = !string.IsNullOrEmpty(_kvList["UseProxy"].Value) ? _kvList["UseProxy"].Value : null;
            ProxyAddress = !string.IsNullOrEmpty(_kvList["ProxyAddress"].Value) ? _kvList["ProxyAddress"].Value : null;
            return _reader;
        }
    }
}
