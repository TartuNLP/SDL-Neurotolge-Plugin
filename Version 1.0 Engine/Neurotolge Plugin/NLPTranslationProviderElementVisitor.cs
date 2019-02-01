using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sdl.LanguagePlatform.Core;

namespace Neurotolge_Plugin
{
    class NLPTranslationProviderElementVisitor : ISegmentElementVisitor
    {
        private NLPTranslationOptions _options;
        private string _plainText;

        public string PlainText
        {
            get
            {
                if (_plainText == null)
                {
                    _plainText = "";
                }
                return _plainText;
            }
            set
            {
                _plainText = value;
            }
        }

        public void Reset()
        {
            _plainText = "";
        }

        public NLPTranslationProviderElementVisitor(NLPTranslationOptions options)
        {
            _options = options;
        }

        #region ISegmentElementVisitor Members
        public void VisitText(Text text)
        {
            _plainText += text;
        }

        public void VisitTag(Tag tag)
        {
            _plainText += tag.TextEquivalent;
        }

        public void VisitDateTimeToken(Sdl.LanguagePlatform.Core.Tokenization.DateTimeToken token)
        {
            _plainText += token.Text;
        }

        public void VisitNumberToken(Sdl.LanguagePlatform.Core.Tokenization.NumberToken token)
        {
            _plainText += token.Text;
        }

        public void VisitMeasureToken(Sdl.LanguagePlatform.Core.Tokenization.MeasureToken token)
        {
            _plainText += token.Text;
        }

        public void VisitSimpleToken(Sdl.LanguagePlatform.Core.Tokenization.SimpleToken token)
        {
            _plainText += token.Text;
        }

        public void VisitTagToken(Sdl.LanguagePlatform.Core.Tokenization.TagToken token)
        {
            _plainText += token.Text;
        }
        #endregion
    }
}
