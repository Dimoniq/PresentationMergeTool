using System;
using System.Collections.Generic;
using System.Configuration;
using PresentationMergeTool.Contracts;
using PresentationMergeTool.Infrastructure.Properties;

namespace PresentationMergeTool.Infrastructure
{
  public class ConfigManager : IConfigManager
  {
    private const string ConfigAllowedPresentationExtension = "AllowedPresentationExtensions";
    private const char Separator = ';';

    public IEnumerable<string> GetAllowedPresentationExtensions()
    {
      return this.GetSplitConfiguration(ConfigAllowedPresentationExtension);
    }

    private IEnumerable<string> GetSplitConfiguration(string configName)
    {
      var settings = ConfigurationManager.AppSettings[configName]?.Split(Separator);
      if (settings == null)
      {
        throw new Exception(string.Format(Resources.ConfMng_ConfigurationMissing, configName));
      }

      return settings;
    }
  }
}