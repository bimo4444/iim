using PluginInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Trap;

namespace PluginManager
{
    public class PluginService : IPluginService, IPluginHost
    {
        IExceptionTrap exceptionTrap;
        private List<IPlugin> plugins = new List<IPlugin>();
        public string ConnectionString { get; set; }
        public int ConnectionTimeout { get; set; }
        public PluginService(IExceptionTrap exceptionTrap)
        {
            this.exceptionTrap = exceptionTrap;
        }
        public void Init(string path)
        {
            FindPlugins(path);
            if (!plugins.Any())
                return;
            List<Task> tasks = new List<Task>();
            foreach (var plugin in plugins)
            {
                Task t = Task.Factory.StartNew(() => 
                { 
                    plugin.Host = this;
                    exceptionTrap.Catch(() => plugin.Init()); 
                });
                tasks.Add(t);
            }
            Task.WaitAll(tasks.ToArray());
        }
        private void FindPlugins(string path)
        {
            string assemblyPath = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location);
            path = assemblyPath + "\\" + path;
            if (!Directory.Exists(path))
                return;
            foreach (string file in Directory.GetFiles(path))
            {
                FileInfo fileInfo = new FileInfo(file);
                if(fileInfo.Extension.Equals(".dll"))
                    AddPlugin(file);
            }
        }
        private void AddPlugin(string file)
        {
            exceptionTrap.Catch(() => 
            {
                var assembly = Assembly.LoadFile(file);
                var plugin = Activator.CreateInstance(
                    assembly.GetTypes().First(t => t.GetInterfaces().Contains(typeof(IPlugin)))) as IPlugin;
                plugins.Add(plugin);
            });
        }
        public void Dispose()
        {
            foreach (var plugin in plugins)
                plugin.Dispose();
        }
    }
}
