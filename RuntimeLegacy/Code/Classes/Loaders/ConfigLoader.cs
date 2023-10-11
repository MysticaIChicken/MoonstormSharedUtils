﻿using BepInEx;
using BepInEx.Configuration;
using Moonstorm.Config;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Moonstorm.Loaders
{
    /// <summary>
    /// The ConfigLoader is a class that can be used to simplify the implementation of ConfigFiles from BepInEx
    /// <para>ConfigLoader will easily create new Config files, config files created by it can be wiped between major versions</para>
    /// <para>ConfigLoader inheriting classes are treated as Singletons</para>
    /// </summary>
    /// <typeparam name="T">The class that's inheriting from ConfigLoader</typeparam>
    public abstract class ConfigLoader<T> : ConfigLoader where T : ConfigLoader<T>
    {
        /// <summary>
        /// Retrieves the instance of <typeparamref name="T"/>
        /// </summary>
        public static T Instance { get; private set; }

        /// <summary>
        /// Parameterless Constructor for ConfigLoader, this will throw an invalid operation exception if an instance of <typeparamref name="T"/> already exists
        /// </summary>
        public ConfigLoader()
        {
            try
            {
                if (Instance != null)
                {
                    throw new InvalidOperationException("Singleton class \"" + typeof(T).Name + "\" inheriting ConfigLoader was instantiated twice");
                }
                Instance = this as T;
            }
            catch (Exception e) { MSULog.Error(e); }
        }

        /// <summary>
        /// Creates a ConfigurableVariable of type <typeparamref name="TVal"/> and automatically sets it's <see cref="ConfigurableVariable.ModGUID"/> and <see cref="ConfigurableVariable.ModName"/> to <typeparamref name="T"/>'s instance using it's <see cref="ConfigLoader.MainClass"/>.
        /// <br>Requires an <see cref="Instance"/> of <typeparamref name="T"/> to exist.</br>
        /// </summary>
        /// <typeparam name="TVal">The type that the configurable variable will configure</typeparam>
        /// <param name="defaultVal">The default value for the variable</param>
        /// <param name="initializer">Optional initializer</param>
        /// <returns>The created ConfigurableVariable</returns>
        public static ConfigurableVariable<TVal> MakeConfigurableVariable<TVal>(TVal defaultVal, Action<ConfigurableVariable<TVal>> initializer = null)
        {
            ThrowIfNoInstance($"Create {nameof(ConfigurableVariable<TVal>)}");

            var metadata = Instance.MainClass.Info.Metadata;
            var cfg = new ConfigurableVariable<TVal>(defaultVal)
            {
                ModGUID = metadata.GUID,
                ModName = metadata.Name,
            };
            initializer?.Invoke(cfg);
            return cfg;
        }

        /// <summary>
        /// Creates a ConfigurableBool and automatically sets it's <see cref="ConfigurableVariable.ModGUID"/> and <see cref="ConfigurableVariable.ModName"/> to <typeparamref name="T"/>'s instance using it's <see cref="ConfigLoader.MainClass"/>.
        /// <br>Requires an <see cref="Instance"/> of <typeparamref name="T"/> to exist.</br>
        /// </summary>
        /// <param name="defaultVal">The default value for the bool</param>
        /// <param name="initializer">Optional initializer</param>
        /// <returns>The created ConfigurableBool</returns>
        public static ConfigurableBool MakeConfigurableBool(bool defaultVal, Action<ConfigurableBool> initializer = null)
        {
            ThrowIfNoInstance($"Create {nameof(ConfigurableBool)}");

            var metadata = Instance.MainClass.Info.Metadata;
            var cfg = new ConfigurableBool(defaultVal)
            {
                ModGUID = metadata.GUID,
                ModName = metadata.Name,
            };
            initializer?.Invoke(cfg);
            return cfg;
        }

        /// <summary>
        /// Creates a ConfigurableColor and automatically sets it's <see cref="ConfigurableVariable.ModGUID"/> and <see cref="ConfigurableVariable.ModName"/> to <typeparamref name="T"/>'s instance using it's <see cref="ConfigLoader.MainClass"/>.
        /// <br>Requires an <see cref="Instance"/> of <typeparamref name="T"/> to exist.</br>
        /// </summary>
        /// <param name="defaultVal">The default value for the color</param>
        /// <param name="initializer">Optional initializer</param>
        /// <returns>The created ConfigurableColor</returns>
        public static ConfigurableColor MakeConfigurableColor(Color defaultVal, Action<ConfigurableColor> initializer = null)
        {
            ThrowIfNoInstance($"Create {nameof(ConfigurableColor)}");

            var metadata = Instance.MainClass.Info.Metadata;
            var cfg = new ConfigurableColor(defaultVal)
            {
                ModGUID = metadata.GUID,
                ModName = metadata.Name,
            };
            initializer?.Invoke(cfg);
            return cfg;
        }

        /// <summary>
        /// Creates a ConfigurableEnum of type <typeparamref name="TEnum"/> and automatically sets it's <see cref="ConfigurableVariable.ModGUID"/> and <see cref="ConfigurableVariable.ModName"/> to <typeparamref name="T"/>'s instance using it's <see cref="ConfigLoader.MainClass"/>.
        /// <br>Requires an <see cref="Instance"/> of <typeparamref name="T"/> to exist.</br>
        /// </summary>
        /// <typeparam name="TEnum">The enum type that the configurable variable will configure</typeparam>
        /// <param name="defaultVal">The default value for the enum</param>
        /// <param name="initializer">Optional initializer</param>
        /// <returns>The created ConfigurableEnum</returns>
        public static ConfigurableEnum<TEnum> MakeConfigurableEnum<TEnum>(TEnum defaultVal, Action<ConfigurableEnum<TEnum>> initializer = null) where TEnum : Enum
        {
            ThrowIfNoInstance($"Create {nameof(ConfigurableEnum<TEnum>)}");

            var metadata = Instance.MainClass.Info.Metadata;
            var cfg = new ConfigurableEnum<TEnum>(defaultVal)
            {
                ModGUID = metadata.GUID,
                ModName = metadata.Name,
            };
            initializer?.Invoke(cfg);
            return cfg;
        }

        /// <summary>
        /// Creates a ConfigurableFloat and automatically sets it's <see cref="ConfigurableVariable.ModGUID"/> and <see cref="ConfigurableVariable.ModName"/> to <typeparamref name="T"/>'s instance using it's <see cref="ConfigLoader.MainClass"/>.
        /// <br>Requires an <see cref="Instance"/> of <typeparamref name="T"/> to exist.</br>
        /// </summary>
        /// <param name="defaultVal">The default value for the float</param>
        /// <param name="initializer">Optional initializer</param>
        /// <returns>The created ConfigurableFloat</returns>
        public static ConfigurableFloat MakeConfigurableFloat(float defaultVal, Action<ConfigurableFloat> initializer = null)
        {
            ThrowIfNoInstance($"Create {nameof(ConfigurableFloat)}");

            var metadata = Instance.MainClass.Info.Metadata;
            var cfg = new ConfigurableFloat(defaultVal)
            {
                ModGUID = metadata.GUID,
                ModName = metadata.Name,
            };
            initializer?.Invoke(cfg);
            return cfg;
        }

        /// <summary>
        /// Creates a ConfigurableInt and automatically sets it's <see cref="ConfigurableVariable.ModGUID"/> and <see cref="ConfigurableVariable.ModName"/> to <typeparamref name="T"/>'s instance using it's <see cref="ConfigLoader.MainClass"/>.
        /// <br>Requires an <see cref="Instance"/> of <typeparamref name="T"/> to exist.</br>
        /// </summary>
        /// <param name="defaultVal">The default value for the int</param>
        /// <param name="initializer">Optional initializer</param>
        /// <returns>The created ConfigurableInt</returns>
        public static ConfigurableInt MakeConfigurableInt(int defaultVal, Action<ConfigurableInt> initializer = null)
        {
            ThrowIfNoInstance($"Create {nameof(ConfigurableInt)}");

            var metadata = Instance.MainClass.Info.Metadata;
            var cfg = new ConfigurableInt(defaultVal)
            {
                ModGUID = metadata.GUID,
                ModName = metadata.Name,
            };
            initializer?.Invoke(cfg);
            return cfg;
        }


        /// <summary>
        /// Creates a ConfigurableString and automatically sets it's <see cref="ConfigurableVariable.ModGUID"/> and <see cref="ConfigurableVariable.ModName"/> to <typeparamref name="T"/>'s instance using it's <see cref="ConfigLoader.MainClass"/>.
        /// <br>Requires an <see cref="Instance"/> of <typeparamref name="T"/> to exist.</br>
        /// </summary>
        /// <param name="defaultVal">The default value for the string</param>
        /// <param name="initializer">Optional initializer</param>
        /// <returns>The created ConfigurableString</returns>
        public static ConfigurableString MakeConfigurableString(string defaultVal, Action<ConfigurableString> initializer = null)
        {
            ThrowIfNoInstance($"Create {nameof(ConfigurableString)}");

            var metadata = Instance.MainClass.Info.Metadata;
            var cfg = new ConfigurableString(defaultVal)
            {
                ModGUID = metadata.GUID,
                ModName = metadata.Name,
            };
            initializer?.Invoke(cfg);
            return cfg;
        }

        /// <summary>
        /// Creates a ConfigurableKeyBind and automatically sets it's <see cref="ConfigurableVariable.ModGUID"/> and <see cref="ConfigurableVariable.ModName"/> to <typeparamref name="T"/>'s instance using it's <see cref="ConfigLoader.MainClass"/>.
        /// <br>Requires an <see cref="Instance"/> of <typeparamref name="T"/> to exist.</br>
        /// </summary>
        /// <param name="defaultVal">The default value for the key bind</param>
        /// <param name="initializer">Optional initializer</param>
        /// <returns>The created ConfigurableKeyBind</returns>
        public static ConfigurableKeyBind MakeConfigurableKeyBind(KeyboardShortcut defaultVal, Action<ConfigurableKeyBind> initializer = null)
        {
            ThrowIfNoInstance($"Create {nameof(ConfigurableKeyBind)}");

            var metadata = Instance.MainClass.Info.Metadata;
            var cfg = new ConfigurableKeyBind(defaultVal)
            {
                ModGUID = metadata.GUID,
                ModName = metadata.Name,
            };
            initializer?.Invoke(cfg);
            return cfg;
        }

        /// <summary>
        /// Throws a null reference exception if no isntance is found. use with caution
        /// </summary>
        protected static void ThrowIfNoInstance(string attemptedAction)
        {
#if !UNITY_EDITOR
            if (Instance == null)
                throw new NullReferenceException($"Cannot {attemptedAction} when there is no instance of {typeof(T).Name}!");
#endif
        }
    }

    /// <summary>
    /// <inheritdoc cref="ConfigLoader{T}"/>
    /// <para>You probably want to use <see cref="ConfigLoader{T}"/> instead</para>
    /// </summary>
    public abstract class ConfigLoader
    {
        /// <summary>
        /// Your mod's main class
        /// </summary>
        public abstract BaseUnityPlugin MainClass { get; }
        /// <summary>
        /// Wether ConfigFiles created by the ConfigLoader will be created in a subfolder, or in the Bepinex's ConfigPath
        /// </summary>
        public abstract bool CreateSubFolder { get; }
        /// <summary>
        /// Returns the folder where the config files for this ConfigLoader are located
        /// </summary>
        public string ConfigFolderPath
        {
            get
            {
                return CreateSubFolder ? System.IO.Path.Combine(Paths.ConfigPath, OwnerMetaData.Name) : Paths.ConfigPath;
            }
        }
        /// <summary>
        /// Retrieves the MainClass's Owner Metadata
        /// </summary>
        public BepInPlugin OwnerMetaData { get => MainClass.Info.Metadata; }

        /// <summary>
        /// Creates a config file.
        /// <para>The config file's name will be the <paramref name="identifier"/></para>
        /// </summary>
        /// <param name="identifier">A unique identifier for this config file</param>
        /// <param name="wipedBetweenMinorVersions">Wether the ConfigFile is wiped between minor version changes of your mod</param>
        /// <returns>The config file</returns>
        public ConfigFile CreateConfigFile(string identifier, bool wipedBetweenMinorVersions = true)
        {
            return CreateConfigFile(identifier, wipedBetweenMinorVersions, false);
        }

        /// <summary>
        /// Creates a config file.
        /// <para>The config file's name will be the <paramref name="identifier"/></para>
        /// </summary>
        /// <param name="identifier">A unique identifier for this config file</param>
        /// <param name="wipedBetweenMinorVersions">Wether the ConfigFile is wiped between minor version changes of your mod</param>
        /// <param name="createSeparateRooEntry">If true, the ConfigSystem will create a new Risk of Options entry for the ConfigFile.</param>
        /// <returns>The config file</returns>
        public ConfigFile CreateConfigFile(string identifier, bool wipedBetweenMinorVersions = true, bool createSeparateRooEntry = false)
        {
            string fileName = identifier;
            if (!fileName.EndsWith(".cfg", StringComparison.OrdinalIgnoreCase))
            {
                fileName += ".cfg";
            }
            var path = System.IO.Path.Combine(ConfigFolderPath, fileName);
            ConfigFile configFile = new ConfigFile(path, true, OwnerMetaData);
            if (wipedBetweenMinorVersions)
                TryWipeConfig(configFile);

            ConfigSystem.AddConfigFileAndIdentifier(identifier, configFile, createSeparateRooEntry);
            return configFile;
        }

        private void TryWipeConfig(ConfigFile configFile)
        {
            ConfigDefinition configDef = new ConfigDefinition("Version", "Config File Version");
            string configVersionValue = $"{OwnerMetaData.Version.Major}.{OwnerMetaData.Version.Minor}";
            ConfigEntry<string> versionEntry = null;
            if (configFile.TryGetEntry<string>(configDef, out versionEntry))
            {
                string currentValue = versionEntry.Value;

                if (currentValue != configVersionValue)
                {
                    WipeConfig(configFile);
                    versionEntry.Value = configVersionValue;
                }
                return;
            }
            configFile.Bind<string>("Version", "Config File Version", $"{OwnerMetaData.Version.Major}.{OwnerMetaData.Version.Minor}", "Version of this ConfigFile, do not change this value.");
        }

        private void WipeConfig(ConfigFile configFile)
        {
            configFile.Clear();

            var orphanedEntriesProp = typeof(ConfigFile).GetProperty("OrphanedEntries", BindingFlags.Instance | BindingFlags.NonPublic);
            Dictionary<ConfigDefinition, string> orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProp.GetValue(configFile);
            orphanedEntries.Clear();

            configFile.Save();
        }
    }
}