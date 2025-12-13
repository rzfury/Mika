using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mika.Icons
{
    public class IconCollection
    {
        private ContentManager _content;
        private Dictionary<string, Icon> _icons;

        private Texture2D _texture;
        public Texture2D TextureAtlas { get { return _texture; } }

        public IconCollection(ContentManager contentManager)
        {
            _content = contentManager;
            _icons = new Dictionary<string, Icon>();
        }

        public void LoadIcons(string imageAtlasContentPath, string csvContentPath)
        {
            _texture = _content.Load<Texture2D>(imageAtlasContentPath);

            var csvPath = GetContentPath(csvContentPath);
            if (Path.GetExtension(csvPath) != ".csv")
                throw new ArgumentException(string.Format("'{0}' is not a .csv file.", csvPath));

            var csvData = File.ReadAllLines(csvPath);

            _icons.Clear();

            var index = 0;
            foreach (var line in csvData)
            {
                index++;
                if (index == 1) continue;

                var values = line.Split(',');
                var icon = new Icon();

                if (values.Length != 5)
                    throw new FormatException(string.Format("Line '{0}' does not contain exactly 5 values.", index));

                icon.Name = values[0];
                int.TryParse(values[1], out icon.X);
                int.TryParse(values[2], out icon.Y);
                int.TryParse(values[3], out icon.Width);
                int.TryParse(values[4], out icon.Height);

                _icons.Add(icon.Name, icon);
            }
        }

        public Icon GetIcon(string name)
        {
            if (!_icons.TryGetValue(name, out Icon icon))
                throw new KeyNotFoundException(string.Format("Icon '{0}' not found in collection.", name));
            return icon;
        }

        private string GetContentPath(string path)
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var fullPath = Path.GetFullPath(Path.Combine(basePath, _content.RootDirectory, path));
            if (!fullPath.StartsWith(basePath))
                throw new UnauthorizedAccessException("Access to paths outside base directory is not allowed.");
            return fullPath;
        }
    }
}
