﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLauncher
{
    class AppClass
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public Image Icon { get; set; }
        public DateTime LastAccessed { get; set; }
    }
}
