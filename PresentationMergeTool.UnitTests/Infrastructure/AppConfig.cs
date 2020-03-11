/*
 * Code copied from https://stackoverflow.com/questions/6150644/change-default-app-config-at-runtime
 * It temporarily changes the active app.config file, as long as using(AppConfig.Change(tempFileName)) is open.
 */

using System;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace PresentationMergeTool.UnitTests.Infrastructure
{
  public abstract class AppConfig : IDisposable
  {
    public abstract void Dispose();

    public static AppConfig Change(string path)
    {
      return new ChangeAppConfig(path);
    }

    private class ChangeAppConfig : AppConfig
    {
      private readonly string oldConfig =
        AppDomain.CurrentDomain.GetData("APP_CONFIG_FILE").ToString();

      private bool disposedValue;

      public ChangeAppConfig(string path)
      {
        AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", path);
        ResetConfigMechanism();
      }

      public override void Dispose()
      {
        if (!this.disposedValue)
        {
          AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", this.oldConfig);
          ResetConfigMechanism();


          this.disposedValue = true;
        }

        GC.SuppressFinalize(this);
      }

      private static void ResetConfigMechanism()
      {
        typeof(ConfigurationManager)
          .GetField("s_initState", BindingFlags.NonPublic |
                                   BindingFlags.Static)
          .SetValue(null, 0);

        typeof(ConfigurationManager)
          .GetField("s_configSystem", BindingFlags.NonPublic |
                                      BindingFlags.Static)
          .SetValue(null, null);

        typeof(ConfigurationManager)
          .Assembly.GetTypes()
          .Where(x => x.FullName ==
                      "System.Configuration.ClientConfigPaths")
          .First()
          .GetField("s_current", BindingFlags.NonPublic |
                                 BindingFlags.Static)
          .SetValue(null, null);
      }
    }
  }
}