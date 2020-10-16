using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;

namespace Neurotolge_plugin.Model
{
    public class PreTranslateSegment
    {
        public SearchSettings SearchSettings { get; set; }
        public TranslationUnit TranslationUnit { get; set; }
        public string PlainTranslation { get; set; }
        public string SourceText { get; set; }
        public Segment TranslationSegment { get; set; }
    }
}
