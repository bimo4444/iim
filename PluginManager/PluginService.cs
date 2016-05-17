﻿using PluginInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
            foreach (var plugin in plugins)
            {
                plugin.Host = this;
                exceptionTrap.Catch(() => plugin.Init());
            }
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
            try
            {
                var asm = Assembly.LoadFile(file);
                var plugin = Activator.CreateInstance(
                    asm.GetTypes().First(t => t.GetInterfaces().Contains(typeof(IPlugin)))) as IPlugin;
                plugins.Add(plugin);
            }
            catch { }
        }
        public void Dispose()
        {
            foreach (var plugin in plugins)
                plugin.Dispose();
        }
    }
}