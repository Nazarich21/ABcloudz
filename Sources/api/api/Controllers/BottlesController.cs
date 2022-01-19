using api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BottlesController : ControllerBase
    {
        private const string FileName = "bottle.json";
        private const string FileDir = "AppData";
        private static string _fileDirectoryPath;
       

        private IBottleCache _cache;
        public BottlesController(IBottleCache bottleCache)
        {
            _cache = bottleCache;
        }


        [HttpGet]
        public ActionResult<IEnumerable<Bottle>> Get()
        {
            List<Bottle> bottles = DeserializeBottles();
            return bottles;
        }

        [HttpGet("{id}")]
        public ActionResult<Bottle> Get(int id)
        {
            var bottle = _cache.Get(id);
            if (bottle == null)
            {
                List<Bottle> bottles = DeserializeBottles();
                bottle = id >= 0 && id < bottles.Count ? bottles[id] : null;
                if (bottle == null)
                {
                    return NotFound();
                }
                _cache.Set(bottle);
            }

            return bottle;
        }

        [HttpPost]
        public ActionResult<Bottle> Post([FromBody] Bottle value)
        {
            List<Bottle> bottles = DeserializeBottles();
            bottles.Add(value);
            SerializeBottles(bottles);
            return value;
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Bottle value)
        {
            List<Bottle> bottles = DeserializeBottles();
            if (id < 0 || id >= bottles.Count)
                return NotFound();

            bottles[id] = value;
            SerializeBottles(bottles);
            _cache.Remove(id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            List<Bottle> bottles = DeserializeBottles();
            if (id < 0 || id >= bottles.Count)
                return NotFound();

            bottles.RemoveAt(id);
            SerializeBottles(bottles);
            _cache.Remove(id);
            return NoContent();
        }

        private static string FileFullPath
        {
            get
            {
                if (_fileDirectoryPath == null)
                    _fileDirectoryPath = Path.Combine(MapDirectory(), FileName);

                return _fileDirectoryPath;
            }
        }

        private static string MapDirectory()
        {
            string mappedDir = Path.Combine(Environment.CurrentDirectory, FileDir);
            Directory.CreateDirectory(mappedDir);

            return mappedDir;
        }

        private void SerializeBottles(List<Bottle> bottles)
        {
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string str = JsonSerializer.Serialize(bottles, typeof(List<Bottle>), options);

            using (FileStream fs = new FileStream(FileFullPath, FileMode.Create, FileAccess.ReadWrite))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(str);
            }
        }

        private List<Bottle> DeserializeBottles()
        {
            using (FileStream fs = new FileStream(FileFullPath, FileMode.Open, FileAccess.Read))
            using (StreamReader sr = new StreamReader(fs))
            {
                string str = sr.ReadToEnd();
                return JsonSerializer.Deserialize<List<Bottle>>(str);
            }
        }
    }
}
