using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sdl.LanguagePlatform.Core;

namespace TartuNLP
{
	/// <summary>
	/// Holds data on a source segment and the tags it contains, which can be used to insert the tags in the target segment
	/// </summary>
	public class TartuNLPTagPlacer
	{
		private string _returnedText;
		private readonly Segment _sourceSegment;
		private Dictionary<string, TartuNLPTag> _dict;

		private List<string> TagsInfo { get; }
		public TartuNLPTagPlacer(Segment sourceSegment)
		{
			_sourceSegment = sourceSegment;
			TagsInfo = new List<string>();
			_dict = GetSourceTagsDict();
		}

		/// <summary>
		/// Returns the source text with markup replacing the tags in the source segment
		/// </summary>
		public string PreparedSourceText { get; private set; }

		/// <summary>
		/// Returns a tagged segments from a target string containing markup, where the target string represents the translation of the class instance's source segment
		/// </summary>
		/// <param name="returnedText"></param>
		/// <returns></returns>
		public Segment GetTaggedSegment(string returnedText)
		{
			try
			{
				_returnedText = returnedText;

				var segment = new Segment();
				var targetElements = GetTargetElements();

				var detectedTags = 0;
				
				foreach (var t in targetElements)
				{
					var text = t; //the text to be compared/added
					if (_dict.ContainsKey(text)) //if our text in question is in the tagText list
					{
						var padLeft = _dict[text].PadLeft;
						var padRight = _dict[text].PadRight;
						if (padLeft.Length > 0) segment.Add(padLeft); //add leading space if applicable in the source text
						segment.Add(_dict[text].SdlTag); //add the actual tag element after casting it back to a Tag
						if (padRight.Length > 0) segment.Add(padRight); //add trailing space if applicable in the source text
						detectedTags++;
					}
					else
					{
						//if it is not in the list of tagTexts then the element is just the text
						if (text.Trim().Length <= 0) continue;
						text = text.Trim(); //trim out extra spaces, since they are dealt with by associating them with the tags
						segment.Add(text); //add to the segment
					}
				}

				// returns null if tags in output does not contain the same number of matching tags TODO will not cover all issues
				return detectedTags != _dict.Count ? null : segment;
			}
			catch (Exception ex)
			{
				throw new Exception($"GetTaggedSegment method: {ex.Message}\n { ex.StackTrace}");
			}
		}

		/// <summary>
		/// Get the corresponding dictionary for the source tags
		/// </summary>
		/// <returns></returns>
		private Dictionary<string, TartuNLPTag> GetSourceTagsDict()
		{			 
			//build dict by adding the new tag which is used for translation process and the actual tag from segment that will be used to display the translation in editor
			_dict = new Dictionary<string, TartuNLPTag>();
			try
			{
				for (var i = 0; i < _sourceSegment.Elements.Count; i++)
				{
					var elType = _sourceSegment.Elements[i].GetType();

					if (elType.ToString() == "Sdl.LanguagePlatform.Core.Tag") //if tag, add to dictionary
					{
						var theTag = new TartuNLPTag((Tag)_sourceSegment.Elements[i].Duplicate());
						var tagText = theTag.SdlTag.ToString();
						PreparedSourceText += tagText;
						
						if (!TagsInfo.Any(n => n.Equals(theTag.SdlTag.TagID)))
						{
							TagsInfo.Add(theTag.SdlTag.TagID);
						}

						//now we have to figure out whether this tag is preceded and/or followed by whitespace
						if (i > 0 && !_sourceSegment.Elements[i - 1].GetType().ToString().Equals("Sdl.LanguagePlatform.Core.Tag"))
						{
							var prevText = _sourceSegment.Elements[i - 1].ToString();
							if (!prevText.Trim().Equals(""))//and not just whitespace
							{
								//get number of trailing spaces for that segment
								var whitespace = prevText.Length - prevText.TrimEnd().Length;
								//add that trailing space to our tag as leading space
								theTag.PadLeft = prevText.Substring(prevText.Length - whitespace);
							}
						}
						if (i < _sourceSegment.Elements.Count - 1 && !_sourceSegment.Elements[i + 1].GetType().ToString().Equals("Sdl.LanguagePlatform.Core.Tag"))
						{
							//here we don't care whether it is only whitespace
							//get number of leading spaces for that segment
							var nextText = _sourceSegment.Elements[i + 1].ToString();
							var whitespace = nextText.Length - nextText.TrimStart().Length;

							//add that trailing space to our tag as leading space
							theTag.PadRight = nextText.Substring(0, whitespace);
						}
						_dict.Add(tagText, theTag); //add our new tag code to the dict with the corresponding tag
					}
					else
					{
						PreparedSourceText += _sourceSegment.Elements[i].ToString();
					}
				}
				TagsInfo.Clear();
			}
			catch(Exception ex)
			{
				throw new Exception($"GetSourceTagsDict method: {ex.Message}\n { ex.StackTrace}");
			}
			return _dict;
		}

		/// <summary>
		/// puts returned string into an array of elements
		/// </summary>
		/// <returns></returns>
		private IEnumerable<string> GetTargetElements()
		{
			//first create a regex to put our array separators around the tags
			var translation = _returnedText;

			translation = GetTags(translation);
			translation = GetTagsWithDecimals(translation);

			var stringSeparators = new[] { "```" };
			var strAr = translation.Split(stringSeparators, StringSplitOptions.None);
			return strAr;
		}

		private static string GetTagsWithDecimals(string translation)
		{
			try
			{
				const string decimalPattern = @"(<[0-9,\.]+ id=[0-9,\.]+\>)|(<[0-9,\.]+ id=[0-9,\.]+/\>)|(<\/[0-9,\.]+\>)";

				var tagRgx = new Regex(decimalPattern);
				var tagMatches = tagRgx.Matches(translation);
				if (tagMatches.Count > 0)
				{
					return AddSeparators(translation, tagMatches);
				}
			}
			catch(Exception ex)
			{
				throw new Exception($"GetTagsWithDecimals method: {ex.Message}\n { ex.StackTrace}");
			}
			return translation;
		}

		private static string GetTags(string translation)
		{
			try
			{
				const string tagsPattern = @"(<[0-9]+ id=[0-9]+\>)|(<[0-9]+ id=[0-9]+/\>)|(<\/[0-9]+\>)";
				var tagRgx = new Regex(tagsPattern);
				var tagMatches = tagRgx.Matches(translation);
				if (tagMatches.Count > 0)
				{
					return AddSeparators(translation, tagMatches);
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"GetTags method: {ex.Message}\n { ex.StackTrace}");
			}
			return translation;
		}

		private static string AddSeparators(string text, IEnumerable matches)
		{
			return matches
				.Cast<Match>()
				.Aggregate(text, (current, match) => current
					.Replace(match.Value, "```" + match.Value + "```"));
		}
	}
	
	internal class TartuNLPTag
	{
		internal TartuNLPTag(Tag tag)
		{
			SdlTag = tag;
			PadLeft = string.Empty;
			PadRight = string.Empty;
		}

		internal string PadLeft { get; set; }

		internal string PadRight { get; set; }

		internal Tag SdlTag { get; }
	}
	
	
}