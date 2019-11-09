using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace LeagueManager.Infrastructure.WritableOptions
{
    public class WritableOptions<T> : IWritableOptions<T> where T : class, new()
    {
        private readonly IHostingEnvironment environment;
        private readonly IOptionsMonitor<T> options;
        private readonly string section;
        private readonly string file;

        public WritableOptions(
            IHostingEnvironment environment,
            IOptionsMonitor<T> options,
            string section,
            string file)
        {
            this.environment = environment;
            this.options = options;
            this.section = section;
            this.file = file;
        }

        public T Value => options.CurrentValue;
        public T Get(string name) => options.Get(name);

        public void Update(Action<T> applyChanges)
        {
            var fileProvider = environment.ContentRootFileProvider;
            var fileInfo = fileProvider.GetFileInfo(file);
            var physicalPath = fileInfo.PhysicalPath;

            var jObject = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(physicalPath));
            var sectionObject = jObject.TryGetValue(section, out JToken jtokenSection) ?
                JsonConvert.DeserializeObject<T>(jtokenSection.ToString()) : (Value ?? new T());

            applyChanges(sectionObject);

            jObject[section] = JObject.Parse(JsonConvert.SerializeObject(sectionObject));
            File.WriteAllText(physicalPath, JsonConvert.SerializeObject(jObject, Formatting.Indented));
        }
    }
}