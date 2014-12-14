﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Turnable.Tiled
{
    public class Tileset
    {
        public string Name { get; private set; }
        public uint FirstGlobalId { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        public int Spacing { get; private set; }
        public int Margin { get; private set; }

        public Tileset(XElement xTileset)
        {
            // .tmx files have the concept of an external tileset which can be shared among various maps
            Name = (string)xTileset.Attribute("name");
            FirstGlobalId = (uint)xTileset.Attribute("firstgid");
            TileWidth = (int)xTileset.Attribute("tilewidth");
            TileHeight = (int)xTileset.Attribute("tileheight");
            Spacing = (int?)xTileset.Attribute("spacing") ?? 0;
            Margin = (int?)xTileset.Attribute("margin") ?? 0;
        }
    }
}
